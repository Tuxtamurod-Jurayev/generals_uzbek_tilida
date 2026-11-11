using System;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace GeneralsUzbekStandaloneSetup
{
    public class SetupForm : Form
    {
        private TextBox txtInstallPath;
        private Button btnBrowse;
        private Button btnInstall;
        private Button btnLaunch;
        private ProgressBar progressBar;
        private Label lblStatus;
        private TextBox txtLog;

        private static readonly string[] SearchRegistryKeys = new string[]
        {
            @"SOFTWARE\WOW6432Node\Electronic Arts\EA Games\Generals",
            @"SOFTWARE\WOW6432Node\Electronic Arts\EA Games\Command and Conquer Generals Zero Hour",
            @"SOFTWARE\Electronic Arts\EA Games\Generals",
            @"SOFTWARE\Electronic Arts\EA Games\Command and Conquer Generals Zero Hour"
        };

        private static readonly string[] SearchCommonPaths = new string[]
        {
            @"C:\Program Files (x86)\Steam\steamapps\common\Command & Conquer Generals",
            @"C:\Program Files (x86)\Steam\steamapps\common\Command & Conquer Generals Zero Hour",
            @"D:\SteamLibrary\steamapps\common\Command & Conquer Generals",
            @"D:\SteamLibrary\steamapps\common\Command & Conquer Generals Zero Hour",
            @"C:\Program Files\EA Games\Command and Conquer Generals",
            @"C:\Program Files\EA Games\Command and Conquer Generals Zero Hour",
            @"C:\Program Files (x86)\EA Games\Command and Conquer Generals",
            @"C:\Program Files (x86)\EA Games\Command and Conquer Generals Zero Hour",
            @"D:\Games\Generals",
            @"C:\Games\Generals"
        };

        public SetupForm()
        {
            InitializeComponent();
            FindOrSetDefaultPath();
        }

        private void InitializeComponent()
        {
            this.Text = "Command & Conquer: Generals — O'zbek Tili (Lotin) O'rnatuvchi";
            this.Size = new Size(650, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(20, 24, 32);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // Banner
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                BackColor = Color.FromArgb(12, 16, 22)
            };

            Label lblTitle = new Label
            {
                Text = "COMMAND & CONQUER: GENERALS & ZERO HOUR",
                Font = new Font("Segoe UI", 12.0f, FontStyle.Bold),
                ForeColor = Color.FromArgb(245, 175, 40),
                Location = new Point(15, 12),
                AutoSize = true
            };

            Label lblSubtitle = new Label
            {
                Text = "To'liq O'zbek Tili (Lotin Alifbosi), EVA Jangovar Diktor Ovozlar Paketi",
                Font = new Font("Segoe UI", 9.0f, FontStyle.Regular),
                ForeColor = Color.FromArgb(170, 185, 200),
                Location = new Point(16, 40),
                AutoSize = true
            };

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);
            this.Controls.Add(pnlHeader);

            // Path Selection Group
            GroupBox grpPath = new GroupBox
            {
                Text = " O'yin O'rnatiladigan Joy (Papkasi) ",
                Location = new Point(18, 90),
                Size = new Size(598, 85),
                ForeColor = Color.FromArgb(220, 225, 230)
            };

            txtInstallPath = new TextBox
            {
                Location = new Point(15, 32),
                Size = new Size(450, 25),
                BackColor = Color.FromArgb(30, 36, 46),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            btnBrowse = new Button
            {
                Text = "Tanlash...",
                Location = new Point(475, 30),
                Size = new Size(105, 28),
                BackColor = Color.FromArgb(45, 55, 70),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White
            };
            btnBrowse.Click += BtnBrowse_Click;

            grpPath.Controls.Add(txtInstallPath);
            grpPath.Controls.Add(btnBrowse);
            this.Controls.Add(grpPath);

            // Progress bar
            progressBar = new ProgressBar
            {
                Location = new Point(18, 190),
                Size = new Size(598, 16),
                Style = ProgressBarStyle.Continuous
            };
            this.Controls.Add(progressBar);

            lblStatus = new Label
            {
                Text = "Oddiy o'rnatish uchun 'O'RNATISH' tugmasini bosing.",
                Location = new Point(18, 215),
                Size = new Size(598, 22),
                ForeColor = Color.FromArgb(100, 200, 255)
            };
            this.Controls.Add(lblStatus);

            // Log
            txtLog = new TextBox
            {
                Location = new Point(18, 242),
                Size = new Size(598, 170),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(10, 14, 18),
                ForeColor = Color.FromArgb(140, 220, 150),
                Font = new Font("Consolas", 9.0f),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(txtLog);

            // Install Button
            btnInstall = new Button
            {
                Text = "O'RNATISH (ODDIY REJIM)",
                Location = new Point(18, 422),
                Size = new Size(295, 44),
                BackColor = Color.FromArgb(35, 145, 65),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11.0f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnInstall.Click += BtnInstall_Click;

            // Launch Button
            btnLaunch = new Button
            {
                Text = "O'YINNI ISHGA TUSHIRISH",
                Location = new Point(321, 422),
                Size = new Size(295, 44),
                BackColor = Color.FromArgb(40, 95, 180),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11.0f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnLaunch.Click += BtnLaunch_Click;

            this.Controls.Add(btnInstall);
            this.Controls.Add(btnLaunch);
        }

        private void FindOrSetDefaultPath()
        {
            // Try registry
            foreach (string rk in SearchRegistryKeys)
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
                                txtInstallPath.Text = val.ToString();
                                Log("[+] O'yin papkasi registr orqali topildi: " + val);
                                return;
                            }
                        }
                    }
                }
                catch { }
            }

            // Try common paths
            foreach (string p in SearchCommonPaths)
            {
                if (Directory.Exists(p))
                {
                    txtInstallPath.Text = p;
                    Log("[+] Standart o'yin papkasi aniqlandi: " + p);
                    return;
                }
            }

            // Fallback default
            string defaultFolder = @"C:\Games\Command & Conquer Generals";
            txtInstallPath.Text = defaultFolder;
            Log("[*] O'yin papkasi sifatida standart yo'l tanlandi: " + defaultFolder);
            Log("[*] Agar o'yiningiz boshqa papkada bo'lsa, 'Tanlash...' orqali uni ko'rsatishingiz mumkin.");
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Generals yoki Zero Hour joylashgan papkani tanlang:";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtInstallPath.Text = fbd.SelectedPath;
                    Log("[+] Yangi papka tanlandi: " + fbd.SelectedPath);
                }
            }
        }

        private void BtnInstall_Click(object sender, EventArgs e)
        {
            string targetDir = txtInstallPath.Text.Trim();
            if (string.IsNullOrEmpty(targetDir))
            {
                MessageBox.Show("Iltimos, o'rnatish papkasini ko'rsating!", "Xatolik", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnInstall.Enabled = false;
                progressBar.Value = 10;
                lblStatus.Text = "O'rnatish jarayoni boshlandi...";
                Log("\n================ O'RNATISH BOSHLANDI ================");

                // Target paths
                string dataDir = Path.Combine(targetDir, "Data");
                string engDir = Path.Combine(dataDir, "English");
                string iniDir = Path.Combine(dataDir, "INI");
                string speechDir = Path.Combine(dataDir, "Audio", "Speech");
                string altSpeechDir = Path.Combine(dataDir, "Speech");

                Directory.CreateDirectory(engDir);
                Directory.CreateDirectory(iniDir);
                Directory.CreateDirectory(speechDir);
                Directory.CreateDirectory(altSpeechDir);

                progressBar.Value = 30;

                // 1. Extract Generals.csf & Generals.str
                lblStatus.Text = "O'zbek tili matnlari (Lotin) chiqarilmoqda...";
                ExtractResource("Generals.csf", Path.Combine(engDir, "Generals.csf"));
                ExtractResource("Generals.csf", Path.Combine(engDir, "GeneralsMD.csf"));
                ExtractResource("Generals.str", Path.Combine(dataDir, "Generals.str"));
                Log("[+] O'zbek tili matnlari (Generals.csf, GeneralsMD.csf, Generals.str) o'rnatildi.");

                progressBar.Value = 60;

                // 2. Extract EVA speech WAV files
                lblStatus.Text = "O'zbekcha EVA ovozlar paketi o'rnatilmoqda...";
                string[] wavResources = new string[]
                {
                    "Eva_BaseUnderAttack.wav",
                    "Eva_UnitReady.wav",
                    "Eva_BuildingComplete.wav",
                    "Eva_InsufficientFunds.wav",
                    "Eva_LowPower.wav",
                    "Eva_NuclearLaunched.wav",
                    "Eva_ParticleCannonReady.wav",
                    "Eva_SCUDStormReady.wav",
                    "Eva_Victory.wav",
                    "Eva_Defeat.wav",
                    "Eva_UnitPromoted.wav"
                };

                int wavCount = 0;
                foreach (string res in wavResources)
                {
                    string dest1 = Path.Combine(speechDir, res);
                    string dest2 = Path.Combine(altSpeechDir, res);
                    if (ExtractResource(res, dest1))
                    {
                        try { File.Copy(dest1, dest2, true); } catch { }
                        wavCount++;
                    }
                }
                Log(string.Format("[+] {0} ta O'zbekcha jangovar ovozli xabarlar o'rnatildi.", wavCount));

                progressBar.Value = 85;

                // 3. Extract INI files
                lblStatus.Text = "Ovoz va musiqa sozlamalari qo'shilmoqda...";
                ExtractResource("Speech_UZ.ini", Path.Combine(iniDir, "Speech_UZ.ini"));
                ExtractResource("Music_UZ.ini", Path.Combine(iniDir, "Music_UZ.ini"));
                Log("[+] Speech_UZ.ini va Music_UZ.ini konfiguratsiyalari saqlandi.");

                progressBar.Value = 100;
                lblStatus.Text = "O'zbek tili muvaffaqiyatli o'rnatildi!";
                Log("=====================================================");
                Log("TABRIKLAYMIZ! BARCHA FAYLLAR MUVAFFAQIYATLI O'RNATILDI.");

                btnLaunch.Enabled = true;

                // Check for game executable
                string foundExe = FindGameExecutable(targetDir);
                if (foundExe != null)
                {
                    Log("[+] O'yinning asosiy dasturi topildi: " + Path.GetFileName(foundExe));
                    MessageBox.Show(
                        "O'zbek tili (Lotin) paketi to'liq o'rnatildi!\n\n" +
                        "O'yin dasturi aniqlandi. Endi 'O'YINNI ISHGA TUSHIRISH' tugmasini bosib o'ynashingiz mumkin.",
                        "Muvaffaqiyatli O'rnatildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string infoMsg = string.Format(
                        "Barcha O'zbek tili fayllari (matnlar, lotin yozuvi, ovozlar) quyidagi papkaga muvaffaqiyatli o'rnatildi:\n{0}\n\n" +
                        "ESLATMA: Ushbu papkada asl o'yinning 'generals.exe' fayli hali mavjud emas.\n" +
                        "O'yinni to'liq ishga tushirish uchun asl Generals o'rnatilgan papkani tanlang yoki o'yin fayllarini shu yerga nusxalang.",
                        targetDir
                    );
                    MessageBox.Show(infoMsg, "Fayllar O'rnatildi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

        private bool ExtractResource(string resourceName, string outputPath)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            // Check direct name or with namespace
            Stream stream = asm.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                string[] names = asm.GetManifestResourceNames();
                foreach (string n in names)
                {
                    if (n.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase))
                    {
                        stream = asm.GetManifestResourceStream(n);
                        break;
                    }
                }
            }

            if (stream != null)
            {
                using (stream)
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[8192];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fs.Write(buffer, 0, read);
                    }
                }
                return true;
            }

            // Fallback: check local disk relative paths
            string fallbackPath = FindFallbackFile(resourceName);
            if (fallbackPath != null && File.Exists(fallbackPath))
            {
                File.Copy(fallbackPath, outputPath, true);
                return true;
            }

            return false;
        }

        private string FindFallbackFile(string fileName)
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] checkDirs = new string[]
            {
                Path.Combine(appDir, "uzbek_localization"),
                Path.Combine(appDir, "uzbek_audio", "Speech"),
                Path.Combine(appDir, "uzbek_audio"),
                appDir
            };

            foreach (string d in checkDirs)
            {
                string p = Path.Combine(d, fileName);
                if (File.Exists(p)) return p;
            }
            return null;
        }

        private string FindGameExecutable(string dir)
        {
            string[] names = new string[]
            {
                "generals.exe",
                "generalsMD.exe",
                "GeneralsZH.exe",
                "game.dat",
                "Command & Conquer Generals.exe"
            };

            foreach (string n in names)
            {
                string p = Path.Combine(dir, n);
                if (File.Exists(p)) return p;
            }
            return null;
        }

        private void BtnLaunch_Click(object sender, EventArgs e)
        {
            string targetDir = txtInstallPath.Text.Trim();
            string exe = FindGameExecutable(targetDir);
            if (exe != null)
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo(exe)
                    {
                        WorkingDirectory = targetDir
                    };
                    Process.Start(psi);
                    Log("[+] O'yin ishga tushirildi: " + exe);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ishga tushirishda xatolik: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("generals.exe topilmadi. O'yinni o'rnatilgan papkadan yoki Steam / EA App orqali oching.", "Eslatma", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Log(string msg)
        {
            txtLog.AppendText(msg + Environment.NewLine);
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SetupForm());
        }
    }
}
