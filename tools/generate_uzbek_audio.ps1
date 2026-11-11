# Command & Conquer Generals - O'zbek Tili Audio Generator
# Generates authentic WAV speech files for Generals EVA Announcer in Uzbek Latin

$outputDir = "uzbek_audio\Speech"
if (!(Test-Path $outputDir)) {
    New-Item -ItemType Directory -Force -Path $outputDir | Out-Null
}

Add-Type -AssemblyName System.Speech
$synth = New-Object System.Speech.Synthesis.SpeechSynthesizer
$synth.Rate = 0  # Normal speech rate
$synth.Volume = 100

$voiceLines = @{
    "Eva_BaseUnderAttack.wav"      = "Bazamizga dushman hujum qilmoqda!"
    "Eva_UnitReady.wav"            = "Bo'linma jangovar tayyor!"
    "Eva_BuildingComplete.wav"     = "Bino qurib bitkazildi!"
    "Eva_InsufficientFunds.wav"    = "Mablag' yetarli emas!"
    "Eva_LowPower.wav"             = "Ogohlantirish! Elektr quvvati kam!"
    "Eva_NuclearLaunched.wav"      = "Xavf! Yadroviy raketa uchirildi!"
    "Eva_ParticleCannonReady.wav"  = "Zarrachalar to'pi tayyor!"
    "Eva_SCUDStormReady.wav"       = "SKAD bo'roni tayyor!"
    "Eva_Victory.wav"              = "G'alaba! Dushman batamom tor-mor etildi!"
    "Eva_Defeat.wav"               = "Mag'lubiyat! Kuchlarimiz yo'q qilindi!"
    "Eva_UnitPromoted.wav"         = "Bo'linma faxriy unvonga ko'tarildi!"
}

Write-Host "O'zbek tili ovozli xabarlari (EVA) yaratilmoqda..."
foreach ($filename in $voiceLines.Keys) {
    $text = $voiceLines[$filename]
    $filePath = Join-Path $outputDir $filename
    $synth.SetOutputToWaveFile($filePath)
    $synth.Speak($text)
    Write-Host " [+] Yaratildi: $filename ($text)"
}

$synth.Dispose()
Write-Host "Barcha o'zbekcha ovoz fayllari muvaffaqiyatli tayyorlandi!"
