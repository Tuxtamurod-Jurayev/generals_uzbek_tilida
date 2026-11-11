#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Command & Conquer: Generals & Zero Hour CSF / STR String Table Tool
Converts Westwood CSF files to STR/JSON and compiles STR/JSON back to binary CSF.

Authors: Antigravity & Tuxtamurod-Jurayev
License: GPL v3
"""

import sys
import os
import struct
import json

CSF_ID = 0x43534620            # ' FSC' on little-endian disk
CSF_LABEL = 0x4C424C20         # ' LBL'
CSF_STRING = 0x53545220        # ' RTS'
CSF_STRINGWITHWAVE = 0x53545257# 'WRTS'

def decode_csf_string(encoded_bytes: bytes) -> str:
    """Decode CSF UTF-16LE characters with bitwise NOT inversion (~ch)."""
    num_chars = len(encoded_bytes) // 2
    chars = []
    for i in range(num_chars):
        raw_val = struct.unpack_from('<H', encoded_bytes, i * 2)[0]
        # C++ logic: *ptr = ~*ptr;
        inverted = (~raw_val) & 0xFFFF
        chars.append(chr(inverted))
    return "".join(chars)

def encode_csf_string(text: str) -> bytes:
    """Encode string to UTF-16LE with bitwise NOT inversion (~ch)."""
    raw_bytes = bytearray()
    for ch in text:
        val = ord(ch) & 0xFFFF
        inverted = (~val) & 0xFFFF
        raw_bytes.extend(struct.pack('<H', inverted))
    return bytes(raw_bytes)

def read_csf(file_path: str):
    """Parse a .csf file and return a dictionary of {label: {'text': ..., 'speech': ...}}."""
    with open(file_path, 'rb') as f:
        data = f.read()

    if len(data) < 24:
        raise ValueError(f"Fayl hajmi juda kichik: {file_path}")

    magic, version, num_labels, num_strings, skip, langid = struct.unpack_from('<IIIIII', data, 0)
    if magic != CSF_ID:
        raise ValueError(f"Noto'g'ri CSF fayl imzosi: {hex(magic)} (Kutilgan: {hex(CSF_ID)})")

    offset = 24
    results = {}

    for _ in range(num_labels):
        if offset >= len(data):
            break
        lbl_magic, lbl_num_strings, lbl_len = struct.unpack_from('<III', data, offset)
        offset += 12

        if lbl_magic != CSF_LABEL:
            raise ValueError(f"Kutilmagan label tegi: {hex(lbl_magic)} offset={offset}")

        label_name = data[offset:offset+lbl_len].decode('ascii', errors='ignore')
        offset += lbl_len

        entry = {'text': '', 'speech': ''}

        for s_idx in range(lbl_num_strings):
            str_magic, str_len = struct.unpack_from('<II', data, offset)
            offset += 8

            if str_magic not in (CSF_STRING, CSF_STRINGWITHWAVE):
                raise ValueError(f"Kutilmagan matn tegi: {hex(str_magic)} offset={offset}")

            encoded_chars = data[offset:offset + (str_len * 2)]
            offset += str_len * 2

            decoded_text = decode_csf_string(encoded_chars).strip()
            if s_idx == 0:
                entry['text'] = decoded_text

            if str_magic == CSF_STRINGWITHWAVE:
                wave_len = struct.unpack_from('<I', data, offset)[0]
                offset += 4
                wave_name = data[offset:offset+wave_len].decode('ascii', errors='ignore')
                offset += wave_len
                if s_idx == 0:
                    entry['speech'] = wave_name

        results[label_name] = entry

    return results, langid, version

def write_csf(entries: dict, out_file_path: str, langid: int = 0, version: int = 3):
    """Compile dictionary/list of entries to binary .csf file."""
    num_labels = len(entries)
    num_strings = num_labels # Har bir label uchun 1 ta satr

    header = struct.pack('<IIIIII', CSF_ID, version, num_labels, num_strings, 0, langid)
    body = bytearray()

    for label, info in entries.items():
        if isinstance(info, str):
            text = info
            speech = ''
        else:
            text = info.get('text', '')
            speech = info.get('speech', '')

        label_bytes = label.encode('ascii', errors='ignore')
        label_len = len(label_bytes)

        # Write label header: ' LBL', num_strings=1, label_len
        body.extend(struct.pack('<III', CSF_LABEL, 1, label_len))
        body.extend(label_bytes)

        # Write string
        encoded_str = encode_csf_string(text)
        str_len = len(text)

        if speech:
            speech_bytes = speech.encode('ascii', errors='ignore')
            speech_len = len(speech_bytes)
            body.extend(struct.pack('<II', CSF_STRINGWITHWAVE, str_len))
            body.extend(encoded_str)
            body.extend(struct.pack('<I', speech_len))
            body.extend(speech_bytes)
        else:
            body.extend(struct.pack('<II', CSF_STRING, str_len))
            body.extend(encoded_str)

    with open(out_file_path, 'wb') as f:
        f.write(header)
        f.write(body)

def read_str(file_path: str):
    """Parse a .str plain text file."""
    entries = {}
    current_label = None
    current_text = None
    current_speech = ""

    with open(file_path, 'r', encoding='utf-8', errors='ignore') as f:
        lines = f.readlines()

    for raw_line in lines:
        line = raw_line.strip()
        if not line or line.startswith('//'):
            continue

        if current_label is None:
            current_label = line
        elif line.startswith('"'):
            # Text inside quotes
            # Handle possible trailing quote
            text = line
            if text.startswith('"') and text.endswith('"') and len(text) >= 2:
                text = text[1:-1]
            elif text.startswith('"'):
                text = text[1:]
            current_text = text.replace('\\n', '\n')
        elif line.lower() == 'end':
            if current_label and current_text is not None:
                entries[current_label] = {'text': current_text, 'speech': current_speech}
            current_label = None
            current_text = None
            current_speech = ""

    return entries

def write_str(entries: dict, out_file_path: str):
    """Write entries to .str format."""
    with open(out_file_path, 'w', encoding='utf-8') as f:
        f.write("// Command & Conquer: Generals & Zero Hour - Uzbek Translation String Table\n")
        f.write("// Generator: tools/csf_tool.py\n\n")
        for label, info in entries.items():
            if isinstance(info, str):
                text = info
            else:
                text = info.get('text', '')
            # Escape quotes if necessary
            text_escaped = text.replace('"', '""').replace('\n', '\\n')
            f.write(f"{label}\n")
            f.write(f"\"{text_escaped}\"\n")
            f.write("End\n\n")

def main():
    if len(sys.argv) < 3:
        print("C&C Generals CSF / STR Tarjima Vositasi")
        print("Foydalanish:")
        print("  python csf_tool.py export_str <fayl.csf> <chiqish.str>")
        print("  python csf_tool.py compile_str <fayl.str> <chiqish.csf> [lang_id]")
        print("  python csf_tool.py export_json <fayl.csf> <chiqish.json>")
        print("  python csf_tool.py compile_json <fayl.json> <chiqish.csf> [lang_id]")
        sys.exit(1)

    cmd = sys.argv[1].lower()
    in_file = sys.argv[2]
    out_file = sys.argv[3] if len(sys.argv) > 3 else "output"
    lang_id = int(sys.argv[4]) if len(sys.argv) > 4 else 0

    if cmd == 'export_str':
        data, _, _ = read_csf(in_file)
        write_str(data, out_file)
        print(f"[+] CSF -> STR muvaffaqiyatli o'girildi: {len(data)} ta satr -> {out_file}")
    elif cmd == 'compile_str':
        data = read_str(in_file)
        write_csf(data, out_file, langid=lang_id)
        print(f"[+] STR -> CSF muvaffaqiyatli yig'ildi: {len(data)} ta satr -> {out_file}")
    elif cmd == 'export_json':
        data, _, _ = read_csf(in_file)
        with open(out_file, 'w', encoding='utf-8') as f:
            json.dump(data, f, ensure_ascii=False, indent=2)
        print(f"[+] CSF -> JSON muvaffaqiyatli saqlandi: {len(data)} ta satr -> {out_file}")
    elif cmd == 'compile_json':
        with open(in_file, 'r', encoding='utf-8') as f:
            data = json.load(f)
        write_csf(data, out_file, langid=lang_id)
        print(f"[+] JSON -> CSF muvaffaqiyatli yig'ildi: {len(data)} ta satr -> {out_file}")
    else:
        print(f"[-] Noma'lum buyruq: {cmd}")
        sys.exit(1)

if __name__ == '__main__':
    main()
