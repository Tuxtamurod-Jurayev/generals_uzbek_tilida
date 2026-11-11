# Command & Conquer: Generals & Zero Hour — O'zbek Tilida

Ushbu repozitoriy afsonaviy **Command & Conquer: Generals** hamda uning rasmiy kengaytmasi **Generals: Zero Hour** uchun to'liq o'zbek tilidagi lokalizatsiya, avtomatlashtirilgan vositalar va o'yinning rasmiy SAGE dvigateli C++ manba kodini o'z ichiga oladi.

Loyiha muallifi: [Tuxtamurod-Jurayev](https://github.com/Tuxtamurod-Jurayev)

---

## 🚀 1. O'zbek Tili Patchini Avtomatik O'rnatish (1-klikda)

Agar kompyuteringizda o'yin o'rnatilgan bo'lsa (Steam, EA App yoki boshqa versiya), o'zbek tilini avtomatik o'rnatish uchun:

1. Repozitoriyni yuklab oling.
2. Papkadagi **`install_uzbek_patch.bat`** faylini sichqoncha bilan ikki marta bosing (yoki terminalda quyidagini bajaring):
   ```bash
   python tools/installer.py
   ```
3. O'rnatuvchi avtomatik ravishda o'yin papkasini aniqlaydi, asl fayllarni xavfsiz zaxiraga oladi (*backup*) va barcha o'zbek tili matnlarini o'yinga joylashtiradi.
4. O'yinni odatiy tarzda ishga tushiring — barcha menyular, boshqaruv tugmalari va bildirishnomalar o'zbek tilida chiqadi!

---

## 🛠️ 2. Tarjima Vositalari (`tools/csf_tool.py`)

C&C Generals o'yinida barcha matnlar binar **`.csf`** (String Table) va oddiy matn **`.str`** formatlarida saqlanadi. 

Ushbu repozitoriyda maxsus ishlab chiqilgan `csf_tool.py` orqali matnlarni osongina tahrirlash mumkin:

### .csf faylni oddiy matnga (.str) o'girish:
```bash
python tools/csf_tool.py export_str uzbek_localization/Generals.csf matnlar.str
```

### Yangi tarjima qilingan .str faylni o'yin tushunadigan .csf ga yig'ish:
```bash
python tools/csf_tool.py compile_str uzbek_localization/Generals_UZ.str uzbek_localization/Generals.csf
```

### JSON formatida eksport/import qilish:
```bash
python tools/csf_tool.py export_json uzbek_localization/Generals.csf matnlar.json
python tools/csf_tool.py compile_json matnlar.json uzbek_localization/Generals.csf
```

---

## 📁 3. Repozitoriy Tuzilishi

* **`uzbek_localization/`**:
  * `Generals_UZ.str` — O'zbek tilidagi barcha o'yin matnlari (bosh menyu, sozlamalar, jangovar bo'linmalar, qurollar, bildirishnomalar).
  * `Generals.csf` — Kompilyatsiya qilingan binar string jadvali (o'yin bevosita shu fayldan o'qiydi).
* **`tools/`**:
  * `csf_tool.py` — Westwood CSF formatini dekodlash va kodlash vositasi.
  * `installer.py` — O'yinni avtomatik topib, patchni o'rnatuvchi skript.
* **`Generals/`** — Asosiy C&C Generals o'yinining to'liq C++ manba kodi.
* **`GeneralsMD/`** — Generals Zero Hour (*Mission Disk*) kengaytmasining C++ manba kodi.
* **`install_uzbek_patch.bat`** — Windows foydalanuvchilari uchun 1-bosqichli tezkor o'rnatuvchi.

---

## ⚙️ 4. Dasturchilar uchun: SAGE Dvigatelini Kompilyatsiya Qilish (.exe)

Agar siz o'yin dvigatelini o'zgartirmoqchi bo'lsangiz:
1. Loyiha Microsoft Visual C++ 6.0 (SP6) yoki Visual Studio .NET 2003 uchun yozilgan (`Code/RTS.dsw`).
2. Talab qilinadigan tashqi SDK'lar:
   * DirectX 9.0 SDK
   * STLport 4.5.3 (`stlport.diff` patchi bilan)
   * RAD Miles Sound SDK
   * RAD Bink SDK
3. Barcha qaramliklar o'rnatilgach, `rts.dsw` loyihasini ochib **Rebuild All** orqali yangi `generals.exe` yig'ish mumkin.

---

## 📜 5. Litsenziya

Ushbu manba kodi Electronic Arts Inc. tomonidan **GNU General Public License v3 (GPL v3)** litsenziyasi ostida taqdim etilgan.
Batafsil ma'lumot uchun `LICENSE.md` fayliga qarang.
"Command & Conquer" savdo belgisi Electronic Arts Inc.ga tegishli.
