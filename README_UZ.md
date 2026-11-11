# Command & Conquer: Generals & Zero Hour — O'zbek Tilida (To'liq Lotin Alifbosida)

Ushbu repozitoriy afsonaviy **Command & Conquer: Generals** hamda uning rasmiy kengaytmasi **Generals: Zero Hour** uchun to'liq o'zbek tilidagi (lotin alifbosida) matnlar, jangovar ovozli xabarlar (EVA speech), audio sozlamalari, avtomatlashtirilgan vositalar va o'yinning rasmiy SAGE dvigateli C++ manba kodini o'z ichiga oladi.

Loyiha muallifi: [Tuxtamurod-Jurayev](https://github.com/Tuxtamurod-Jurayev)

---

## ⚡ 1. O'rnatish: Tayyor `.exe` Dasturi Orqali (1-bosqichda)

Hech qanday qo'shimcha dastur (Python va h.k.) talab etilmaydi! Repozitoriy ildizidagi tayyor o'rnatuvchi dastur mavjud:

### 🎮 **`Generals_Uzbek_Installer.exe`**
1. Repozitoriy ildizidagi **`Generals_Uzbek_Installer.exe`** faylini sichqoncha bilan ikki marta bosing.
2. Dastur kompyuteringizdagi o'yin papkasini (Steam, EA App yoki Registr orqali) avtomatik aniqlaydi. Agar o'yin boshqa joyda bo'lsa, *"Ko'rib chiqish..."* tugmasi orqali papkani tanlang.
3. Kerakli komponentlarni belgilang:
   * ✅ **O'zbek tili matnlari (Lotin yozuvida):** Barcha asosiy menyular, sozlamalar, fraksiyalar, jangovar texnikalar, generallar va xabarlar.
   * ✅ **O'zbekcha Ovozlar (EVA Speech):** Baza hujum ostida ekanligi, bo'linma tayyorligi, bino qurilishi va o'ta qurollar haqidagi o'zbekcha ovozli xabarlar (`.wav`).
   * ✅ **O'zbekcha Musiqa sozlamalari:** Maxsus musiqa treklarini o'yinga integratsiya qilish.
   * ✅ **Asl fayllarni zaxiraga olish (Backup):** Istalgan vaqtda asl holatga qaytish imkoniyati.
4. **"O'ZBEK TILINI O'RNATISH"** tugmasini bosing.
5. O'rnatish yakunlangach, to'g'ridan-to'g'ri **"O'YINNI ISHGA TUSHIRISH"** tugmasi orqali o'yinga kiring!

*(Foydalanuvchilar uchun `install_uzbek_patch.bat` ham qo'shimcha sifatida mavjud).*

---

## 🎙️ 2. O'zbekcha Ovozlar va Musiqalar (Audio Tizimi)

O'yin dvigatelida audio tizimi `Data\Audio\` va `Data\INI\` orqali boshqariladi:

* **`uzbek_audio/Speech/`**: O'zbekcha EVA jangovar diktor ovozlari:
  * `Eva_BaseUnderAttack.wav` — *"Bazamizga dushman hujum qilmoqda!"*
  * `Eva_UnitReady.wav` — *"Bo'linma jangovar tayyor!"*
  * `Eva_BuildingComplete.wav` — *"Bino qurib bitkazildi!"*
  * `Eva_InsufficientFunds.wav` — *"Mablag' yetarli emas!"*
  * `Eva_LowPower.wav` — *"Ogohlantirish! Elektr quvvati kam!"*
  * `Eva_NuclearLaunched.wav` — *"Xavf! Yadroviy raketa uchirildi!"*
  * `Eva_ParticleCannonReady.wav` — *"Zarrachalar to'pi tayyor!"*
  * `Eva_SCUDStormReady.wav` — *"SKAD bo'roni tayyor!"*
  * `Eva_Victory.wav` — *"G'alaba! Dushman batamom tor-mor etildi!"*
  * `Eva_Defeat.wav` — *"Mag'lubiyat! Kuchlarimiz yo'q qilindi!"*
* **`uzbek_audio/Speech_UZ.ini`**: O'yin ovoz hodisalarini (AudioEvents) o'zbekcha tovushlar bilan bog'lovchi konfiguratsiya.
* **`uzbek_audio/Music_UZ.ini`**: O'zbekcha jangovar musiqalar va mavzularni ulash uchun konfiguratsiya.
* **`tools/generate_uzbek_audio.ps1`**: Yangi ovozli xabarlarni avtomatik generatsiya qilish vositasi.

---

## 🛠️ 3. Tarjima Vositalari (`tools/csf_tool.py`)

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

---

## 📁 4. Repozitoriy Tuzilishi

* **`Generals_Uzbek_Installer.exe`** — 1-klikda o'rnatuvchi tayyor Windows dasturi (.NET / C#).
* **`uzbek_localization/`**:
  * `Generals_UZ.str` — To'liq o'zbek lotin alifbosidagi matnlar bazasi.
  * `Generals.csf` — O'yin dvigateli to'g'ridan-to'g'ri o'qiydigan binar CSF fayli.
* **`uzbek_audio/`**:
  * `Speech/` — O'zbekcha EVA ovozli fayllari (`.wav`).
  * `Speech_UZ.ini` — Ovozlar sozlamasi.
  * `Music_UZ.ini` — Musiqalar sozlamasi.
* **`tools/`**:
  * `InstallerApp.cs` — Installer dasturi C# manba kodi.
  * `csf_tool.py` — Westwood CSF formatini tahrirlash dasturi.
  * `installer.py` — Python asosidagi muqobil o'rnatuvchi.
  * `generate_uzbek_audio.ps1` — Ovozlar generatori.
* **`Generals/`** — Asosiy C&C Generals o'yinining C++ manba kodi.
* **`GeneralsMD/`** — Generals Zero Hour kengaytmasining C++ manba kodi.

---

## 🐛 5. Test Qilish va Xatoliklar (Bug Reporting)

1. `Generals_Uzbek_Installer.exe` ni ishga tushiring va o'zbek tilini o'rnating.
2. O'yinga kirib quyidagilarni tekshiring:
   * Asosiy menyu, sozlamalar va tugmalar lotin yozuvida to'g'ri chiqayotgani;
   * Birliklarning nomlari va buyruqlar paneli;
   * Jang paytida diktor (EVA) xabarlari (masalan, baza hujum ostida qolganda yoki bo'linma chiqqanda);
3. Agar biron bir so'z, harf yoki ovozda kamchilik sezilsa, [Issues](https://github.com/Tuxtamurod-Jurayev/generals_uzbek_tilida/issues) bo'limida xabar bering — tizim uni zudlik bilan tuzatadi!

---

## 📜 6. Litsenziya

Ushbu manba kodi Electronic Arts Inc. tomonidan **GNU General Public License v3 (GPL v3)** litsenziyasi ostida taqdim etilgan.
"Command & Conquer" savdo belgisi Electronic Arts Inc.ga tegishli.
