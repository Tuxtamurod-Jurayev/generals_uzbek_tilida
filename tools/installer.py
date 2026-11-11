#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Command & Conquer Generals & Zero Hour - O'zbek Tili Avtomatik O'rnatuvchi
Avtomatik ravishda o'yin papkasini aniqlaydi va o'zbek tili patchini o'rnatadi.

Muallif: Tuxtamurod-Jurayev
Litsenziya: GPL v3
"""

import os
import sys
import shutil
import winreg

COMMON_PATHS = [
    r"C:\Program Files (x86)\Steam\steamapps\common\Command & Conquer Generals",
    r"C:\Program Files (x86)\Steam\steamapps\common\Command & Conquer Generals Zero Hour",
    r"D:\SteamLibrary\steamapps\common\Command & Conquer Generals",
    r"D:\SteamLibrary\steamapps\common\Command & Conquer Generals Zero Hour",
    r"E:\SteamLibrary\steamapps\common\Command & Conquer Generals",
    r"E:\SteamLibrary\steamapps\common\Command & Conquer Generals Zero Hour",
    r"C:\Program Files (x86)\EA Games\Command and Conquer Generals",
    r"C:\Program Files (x86)\EA Games\Command and Conquer Generals Zero Hour",
    r"C:\Program Files\EA Games\Command and Conquer Generals",
    r"C:\Program Files\EA Games\Command and Conquer Generals Zero Hour",
]

REG_KEYS = [
    r"SOFTWARE\WOW6432Node\Electronic Arts\EA Games\Generals",
    r"SOFTWARE\WOW6432Node\Electronic Arts\EA Games\Command and Conquer Generals Zero Hour",
    r"SOFTWARE\Electronic Arts\EA Games\Generals",
]

def find_game_directory():
    # 1. Check registry
    for rkey in REG_KEYS:
        try:
            with winreg.OpenKey(winreg.HKEY_LOCAL_MACHINE, rkey) as key:
                val, _ = winreg.QueryValueEx(key, "InstallPath")
                if val and os.path.isdir(val):
                    return val
        except Exception:
            pass

    # 2. Check common known directories
    for path in COMMON_PATHS:
        if os.path.isdir(path):
            return path

    # 3. Check current directory
    if os.path.exists("generals.exe") or os.path.exists("game.dat"):
        return os.path.abspath(".")

    return None

def install_patch(target_dir):
    print(f"\n[+] O'yin katalogi tanlandi: {target_dir}")
    script_dir = os.path.dirname(os.path.abspath(__file__))
    root_dir = os.path.abspath(os.path.join(script_dir, ".."))
    uzbek_dir = os.path.join(root_dir, "uzbek_localization")

    csf_src = os.path.join(uzbek_dir, "Generals.csf")
    str_src = os.path.join(uzbek_dir, "Generals_UZ.str")

    if not os.path.exists(csf_src):
        # Generate if not exists
        print("[*] Generals.csf yaratilmoqda...")
        from csf_tool import read_str, write_csf
        data = read_str(str_src)
        write_csf(data, csf_src)

    # Ensure Data and Data/English directories exist
    data_dir = os.path.join(target_dir, "Data")
    eng_dir = os.path.join(data_dir, "English")
    os.makedirs(eng_dir, exist_ok=True)

    # 1. Backup original Generals.csf if exists
    target_csf = os.path.join(eng_dir, "Generals.csf")
    if os.path.exists(target_csf):
        backup_csf = os.path.join(eng_dir, "Generals.csf.orig_backup")
        if not os.path.exists(backup_csf):
            shutil.copy2(target_csf, backup_csf)
            print(f"[+] Asl fayl nusxalandi (backup): {backup_csf}")

    # 2. Copy Uzbek CSF
    shutil.copy2(csf_src, target_csf)
    print(f"[+] O'zbek tili CSF fayli joylashtirildi: {target_csf}")

    # Also for Zero Hour if present
    target_zh_csf = os.path.join(eng_dir, "GeneralsMD.csf")
    if os.path.exists(os.path.join(target_dir, "generals.exe")) or os.path.exists(target_zh_csf):
        shutil.copy2(csf_src, target_zh_csf)
        print(f"[+] Zero Hour uchun CSF fayli joylashtirildi: {target_zh_csf}")

    # 3. Copy STR text file to Data\Generals.str
    target_str = os.path.join(data_dir, "Generals.str")
    shutil.copy2(str_src, target_str)
    print(f"[+] STR matn fayli joylashtirildi: {target_str}")

    print("\n" + "="*60)
    print(" TABRIKLAYMIZ! O'ZBEK TILI PATCHI MUVAFFAQIYATLI O'RNATILDI!")
    print(" Endi o'yinni odatiy tarzda ishga tushirishingiz mumkin.")
    print(" Barcha asosiy menyular, tugmalar va bildirishnomalar o'zbek tilida chiqadi.")
    print("="*60 + "\n")

def main():
    print("="*60)
    print(" Command & Conquer: Generals & Zero Hour - O'zbek Tili O'rnatuvchi")
    print(" Muallif: Tuxtamurod-Jurayev")
    print("="*60)

    game_dir = None
    if len(sys.argv) > 1:
        game_dir = sys.argv[1]
    else:
        game_dir = find_game_directory()

    if not game_dir or not os.path.isdir(game_dir):
        print("\n[-] O'yin papkasi avtomatik topilmadi.")
        manual_path = input("Iltimos, o'yin o'rnatilgan papka yo'lini kiriting (masalan: D:\\Games\\Generals): ").strip()
        if manual_path and os.path.isdir(manual_path):
            game_dir = manual_path
        else:
            print("[-] Ko'rsatilgan papka mavjud emas. O'rnatish to'xtatildi.")
            sys.exit(1)

    install_patch(game_dir)

if __name__ == "__main__":
    main()
