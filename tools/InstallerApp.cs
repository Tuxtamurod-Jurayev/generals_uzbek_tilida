using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace GeneralsUzbekInstaller
{
    public class InstallerForm : Form
    {
        private TextBox txtGamePath;
        private Button btnBrowse;
        private Button btnInstall;
        private Button btnLaunch;
        private CheckBox chkTexts;
        private CheckBox chkVoices;
        private CheckBox chkMusic;
        private CheckBox chkBackup;
        private ProgressBar progressBar;
        private TextBox txtLog;
        private Label lblStatus;

        private static readonly string[] CommonPaths = new string[]
        {
            @"C:\Program Files (x86)\Steam\steamapps\common\Command & Conquer Generals",
            @"C:\Program Files (x86)\Steam\steamapps\common\Command & Conquer Generals Zero Hour",
            @"D:\SteamLibrary\steamapps\common\Command & Conquer Generals",
            @"D:\SteamLibrary\steamapps\common\Command & Conquer Generals Zero Hour",
            @"E:\SteamLibrary\steamapps\common\Command & Conquer Generals",
            @"E:\SteamLibrary\steamapps\common\Command & Conquer Generals Zero Hour",
            @"C:\Program Files (x86)\EA Games\Command and Conquer Generals",
            @"C:\Program Files (x86)\EA Games\Command and Conquer Generals Zero Hour",
            @"C:\Program Files\EA Games\Command and Conquer Generals",
            @"C:\Program Files\EA Games\Command and Conquer Generals Zero Hour"
        };

        public InstallerForm()
        {
            InitializeComponent();
            DetectGamePath();
        }

        private void InitializeComponent()
        {
            this.Text = "Command & Conquer: Generals & Zero Hour — O'zbek Tili O'rnatuvchi";
            this.Size = new Size(680, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(24, 28, 36);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // Title Header Banner
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = Color.FromArgb(16, 20, 26)
            };

            Label lblTitle = new Label
            {
                Text = "COMMAND & CONQUER: GENERALS — O'ZBEK TILI",
                Font = new Font("Segoe UI", 12.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(245, 175, 40),
                Location = new Point(15, 10),
                AutoSize = true
            };

            Label lblSubtitle = new Label
            {
                Text = "To'liq Lotin yozuvidagi matnlar, ovozlar va tizim sozlamalari paketi",
                Font = new Font("Segoe UI", 9.0f, FontStyle.Regular),
                ForeColor = Color.FromArgb(180, 190, 200),
                Location = new Point(16, 36),
                AutoSize = true
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            this.Controls.Add(pnlHeader);

            // Group: O'yin papkasi
            GroupBox grpPath = new GroupBox
            {
                Text = " O'yin O'rnatilgan Papka (Game Path) ",
                Location = new Point(18, 80),
                Size = new Size(628, 75),
                ForeColor = Color.FromArgb(220, 225, 230)
            };

            txtGamePath = new TextBox
            {
                Location = new Point(15, 30),
                Size = new Size(480, 25),
                BackColor = Color.FromArgb(32, 38, 48),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            btnBrowse = new Button
            {
                Text = "Ko'rib chiqish...",
                Location = new Point(505, 28),
                Size = new Size(110, 28),
                BackColor = Color.FromArgb(44, 52, 66),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };
            btnBrowse.Click += BtnBrowse_Click;

            grpPath.Controls.Add(txtGamePath);
            grpPath.Controls.Add(btnBrowse);
            this.Controls.Add(grpPath);

            // Group: Komponentlar
            GroupBox grpOptions = new GroupBox
            {
                Text = " O'rnatish Komponentlari ",
                Location = new Point(18, 165),
                Size = new Size(628, 90),
                ForeColor = Color.FromArgb(220, 225, 230)
            };

            chkTexts = new CheckBox
            {
                Text = "O'zbek tili matnlari (Lotin alifbosi: Generals.csf & Generals.str)",
                Checked = true,
                Location = new Point(18, 26),
                AutoSize = true
            };

            chkVoices = new CheckBox
            {
                Text = "O'zbekcha Ovozlar (EVA Jangovar Xabarlari va Nutqlar)",
                Checked = true,
                Location = new Point(18, 55),
                AutoSize = true
            };

            chkMusic = new CheckBox
            {
                Text = "O'zbekcha Musiqa sozlamalari (Music INI)",
                Checked = true,
                Location = new Point(380, 26),
                AutoSize = true
            };

            chkBackup = new CheckBox
            {
                Text = "Asl fayllarni xavfsiz zaxiraga olish (Backup)",
                Checked = true,
                Location = new Point(380, 55),
                AutoSize = true
            };

            grpOptions.Controls.Add(chkTexts);
            grpOptions.Controls.Add(chkVoices);
            grpOptions.Controls.Add(chkMusic);
            grpOptions.Controls.Add(chkBackup);
            this.Controls.Add(grpOptions);

            // Progress Bar
            progressBar = new ProgressBar
            {
                Location = new Point(18, 268),
                Size = new Size(628, 14),
                Style = ProgressBarStyle.Continuous
            };
            this.Controls.Add(progressBar);

            // Status label
            lblStatus = new Label
            {
                Text = "O'rnatishga tayyor.",
                Location = new Point(18, 288),
                Size = new Size(628, 20),
                ForeColor = Color.FromArgb(100, 200, 255)
            };
            this.Controls.Add(lblStatus);

            // Status Log TextBox
            txtLog = new TextBox
            {
                Location = new Point(18, 312),
                Size = new Size(628, 135),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(12, 16, 22),
                ForeColor = Color.FromArgb(160, 220, 160),
                Font = new Font("Consolas", 9.0f),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtLog);

            // Buttons: Install and Launch
            btnInstall = new Button
            {
                Text = "O'ZBEK TILINI O'RNATISH",
                Location = new Point(18, 460),
                Size = new Size(310, 42),
                BackColor = Color.FromArgb(30, 140, 60),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11.0f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnInstall.Click += BtnInstall_Click;

            btnLaunch = new Button
            {
                Text = "O'YINNI ISHGA TUSHIRISH",
                Location = new Point(336, 460),
                Size = new Size(310, 42),
                BackColor = Color.FromArgb(40, 90, 180),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11.0f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnLaunch.Click += BtnLaunch_Click;

            this.Controls.Add(btnInstall);
            this.Controls.Add(btnLaunch);
        }

        private void DetectGamePath()
        {
            // 1. Check registry
            string[] regKeys = new string[]
            {
                @"SOFTWARE\WOW6432Node\Electronic Arts\EA Games\Generals",
                @"SOFTWARE\WOW6432Node\Electronic Arts\EA Games\Command and Conquer Generals Zero Hour",
                @"SOFTWARE\Electronic Arts\EA Games\Generals"
            };

            foreach (string rk in regKeys)
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(rk))
                    {
                        if (key != null)
                        {
                            object val = key.GetValue("InstallPath");
                            if (val != null && Directory.Exists(val.ToString()))
                            {
                                txtGamePath.Text = val.ToString();
                                Log("[+] Registr orqali o'yin papkasi aniqlandi: " + val);
                                return;
                            }
                        }
                    }
                }
                catch { }
            }

            // 2. Check standard common folders
            foreach (string p in CommonPaths)
            {
                if (Directory.Exists(p))
                {
                    txtGamePath.Text = p;
                    Log("[+] Standart kutubxona orqali o'yin papkasi topildi: " + p);
                    return;
                }
            }

            // 3. Current folder check
            if (File.Exists("generals.exe") || File.Exists("game.dat"))
            {
                txtGamePath.Text = Environment.CurrentDirectory;
                Log("[+] Joriy papkada o'yin fayllari aniqlandi.");
                return;
            }

            Log("[!] O'yin papkasi avtomatik topilmadi. 'Ko'rib chiqish...' orqali o'yin papkasini tanlang.");
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Command & Conquer Generals yoki Zero Hour o'rnatilgan papkani tanlang:";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtGamePath.Text = fbd.SelectedPath;
                    Log("[+] Papka tanlandi: " + fbd.SelectedPath);
                }
            }
        }

        private void BtnInstall_Click(object sender, EventArgs e)
        {
            string gameDir = txtGamePath.Text.Trim();
            if (string.IsNullOrEmpty(gameDir) || !Directory.Exists(gameDir))
            {
                MessageBox.Show("Iltimos, avval to'g'ri o'yin papkasini ko'rsating!", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnInstall.Enabled = false;
                progressBar.Value = 10;
                lblStatus.Text = "O'rnatish boshlandi...";
                Log("\n================ O'RNATISH BOSHLANDI ================");

                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string uzbekLocDir = Path.Combine(appDir, "uzbek_localization");
                string uzbekAudioDir = Path.Combine(appDir, "uzbek_audio");

                string dataDir = Path.Combine(gameDir, "Data");
                string engDir = Path.Combine(dataDir, "English");
                string iniDir = Path.Combine(dataDir, "INI");
                string audioSpeechDir = Path.Combine(dataDir, "Audio", "Speech");

                Directory.CreateDirectory(engDir);
                Directory.CreateDirectory(iniDir);

                progressBar.Value = 30;

                // 1. Text installation
                if (chkTexts.Checked)
                {
                    lblStatus.Text = "O'zbek tili matnlari o'rnatilmoqda...";
                    string csfSrc = Path.Combine(uzbekLocDir, "Generals.csf");
                    string strSrc = Path.Combine(uzbekLocDir, "Generals_UZ.str");

                    if (File.Exists(csfSrc))
                    {
                        string targetCsf = Path.Combine(engDir, "Generals.csf");
                        string targetZhCsf = Path.Combine(engDir, "GeneralsMD.csf");

                        if (chkBackup.Checked && File.Exists(targetCsf) && !File.Exists(targetCsf + ".orig_backup"))
                        {
                            File.Copy(targetCsf, targetCsf + ".orig_backup", true);
                            Log("[+] Asl Generals.csf zaxiraga olindi (Generals.csf.orig_backup)");
                        }

                        File.Copy(csfSrc, targetCsf, true);
                        File.Copy(csfSrc, targetZhCsf, true);
                        Log("[+] O'zbekcha Generals.csf va GeneralsMD.csf nusxalandi.");
                    }

                    if (File.Exists(strSrc))
                    {
                        string targetStr = Path.Combine(dataDir, "Generals.str");
                        File.Copy(strSrc, targetStr, true);
                        Log("[+] O'zbekcha Generals.str joylashtirildi.");
                    }
                }

                progressBar.Value = 60;

                // 2. Audio & Speech installation
                if (chkVoices.Checked)
                {
                    lblStatus.Text = "O'zbekcha ovozlar o'rnatilmoqda...";
                    string speechSrcDir = Path.Combine(uzbekAudioDir, "Speech");
                    if (Directory.Exists(speechSrcDir))
                    {
                        Directory.CreateDirectory(audioSpeechDir);
                        string[] wavFiles = Directory.GetFiles(speechSrcDir, "*.wav");
                        foreach (string wav in wavFiles)
                        {
                            string dest = Path.Combine(audioSpeechDir, Path.GetFileName(wav));
                            File.Copy(wav, dest, true);
                        }
                        Log(string.Format("[+] {0} ta o'zbekcha EVA ovozli xabarlari (WAV) o'rnatildi.", wavFiles.Length));
                    }

                    string speechIni = Path.Combine(uzbekAudioDir, "Speech_UZ.ini");
                    if (File.Exists(speechIni))
                    {
                        File.Copy(speechIni, Path.Combine(iniDir, "Speech_UZ.ini"), true);
                        Log("[+] Speech_UZ.ini sozlamalari joylashtirildi.");
                    }
                }

                progressBar.Value = 85;

                // 3. Music installation
                if (chkMusic.Checked)
                {
                    string musicIni = Path.Combine(uzbekAudioDir, "Music_UZ.ini");
                    if (File.Exists(musicIni))
                    {
                        File.Copy(musicIni, Path.Combine(iniDir, "Music_UZ.ini"), true);
                        Log("[+] Music_UZ.ini musiqa konfiguratsiyasi qo'shildi.");
                    }
                }

                progressBar.Value = 100;
                lblStatus.Text = "O'zbek tili to'liq va muvaffaqiyatli o'rnatildi!";
                Log("=====================================================");
                Log("TABRIKLAYMIZ! O'ZBEK TILI MUVAFFAQIYATLI O'RNATILDI!");
                Log("Endi 'O'YINNI ISHGA TUSHIRISH' tugmasini bosing.");

                btnLaunch.Enabled = true;
                MessageBox.Show("Command & Conquer Generals uchun O'zbek tili (Lotin) paketi muvaffaqiyatli o'rnatildi!\n\nO'yinni ishga tushirishingiz mumkin.", "Muvaffaqiyat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Xatolik yuz berdi!";
                Log("[-] XATOLIK: " + ex.Message);
                MessageBox.Show("O'rnatishda xatolik yuz berdi:\n" + ex.Message, "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnInstall.Enabled = true;
            }
        }

        private void BtnLaunch_Click(object sender, EventArgs e)
        {
            string gameDir = txtGamePath.Text.Trim();
            string genExe = Path.Combine(gameDir, "generals.exe");
            if (!File.Exists(genExe))
            {
                genExe = Path.Combine(gameDir, "generals.exe");
            }

            if (File.Exists(genExe))
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo(genExe)
                    {
                        WorkingDirectory = gameDir
                    };
                    Process.Start(psi);
                    Log("[+] O'yin ishga tushirildi: " + genExe);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("O'yinni ochishda xatolik: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("generals.exe fayli topilmadi. O'yinni Steam yoki EA App orqali ishga tushirishingiz mumkin.", "Eslatma", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Log(string msg)
        {
            txtLog.AppendText(msg + Environment.NewLine);
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InstallerForm());
        }
    }
}
