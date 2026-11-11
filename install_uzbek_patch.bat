@echo off
chcp 65001 >nul
title Command ^& Conquer Generals - O'zbek Tili O'rnatuvchi
echo ======================================================================
echo    Command ^& Conquer: Generals ^& Zero Hour - O'zbek Tili Patchi
echo    Loyiha: https://github.com/Tuxtamurod-Jurayev/generals_uzbek_tilida
echo ======================================================================
echo.

python --version >nul 2>&1
if %errorlevel% neq 0 (
    echo [XATO] Kompyuteringizda Python topilmadi!
    echo Iltimos, Python o'rnating yoki installer.py ni qo'lda ishga tushiring.
    pause
    exit /b 1
)

python tools\installer.py %*

echo.
pause
