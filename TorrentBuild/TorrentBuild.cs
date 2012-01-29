namespace TorrentBuild
{
    using EAD.Conversion;
    using EAD.Cryptography.ThexCS;
    using EAD.Cryptography.VisualBasic;
    using EAD.PeerToPeer;
    using EAD.Torrent;
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.CompilerServices;
    using Mono.Security.Cryptography;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class TorrentBuild : Form
    {
        [AccessedThroughProperty("AdvSet")]
        private Button _AdvSet;
        [AccessedThroughProperty("AnnounceURL")]
        private TextBox _AnnounceURL;
        [AccessedThroughProperty("AutomaticPieceSize")]
        private CheckBox _AutomaticPieceSize;
        [AccessedThroughProperty("BlacklistingScreen")]
        private Button _BlacklistingScreen;
        [AccessedThroughProperty("BrowseForFile")]
        private OpenFileDialog _BrowseForFile;
        [AccessedThroughProperty("BrowseForFolder")]
        private FolderBrowserDialog _BrowseForFolder;
        [AccessedThroughProperty("BuildTorrentNow")]
        private Button _BuildTorrentNow;
        [AccessedThroughProperty("ExitWithoutSave")]
        private Button _ExitWithoutSave;
        [AccessedThroughProperty("ExitWithSave")]
        private Button _ExitWithSave;
        [AccessedThroughProperty("FileNameToMake")]
        private TextBox _FileNameToMake;
        [AccessedThroughProperty("HashProgress")]
        private ProgressBar _HashProgress;
        [AccessedThroughProperty("IncludeBlacklisted")]
        private CheckBox _IncludeBlacklisted;
        [AccessedThroughProperty("IncludeCRC32")]
        private CheckBox _IncludeCRC32;
        [AccessedThroughProperty("IncludeED2K")]
        private CheckBox _IncludeED2K;
        [AccessedThroughProperty("IncludeMD5")]
        private CheckBox _IncludeMD5;
        [AccessedThroughProperty("IncludeSHA1")]
        private CheckBox _IncludeSHA1;
        [AccessedThroughProperty("IncludeTiger")]
        private CheckBox _IncludeTiger;
        [AccessedThroughProperty("IncludeTorrents")]
        private CheckBox _IncludeTorrents;
        [AccessedThroughProperty("Label1")]
        private Label _Label1;
        [AccessedThroughProperty("Label2")]
        private Label _Label2;
        [AccessedThroughProperty("Label3")]
        private Label _Label3;
        [AccessedThroughProperty("Label4")]
        private Label _Label4;
        [AccessedThroughProperty("Label5")]
        private Label _Label5;
        [AccessedThroughProperty("Label6")]
        private Label _Label6;
        [AccessedThroughProperty("Label7")]
        private Label _Label7;
        [AccessedThroughProperty("Label8")]
        private Label _Label8;
        [AccessedThroughProperty("MakeExternals")]
        private CheckBox _MakeExternals;
        [AccessedThroughProperty("MakeSeparateTorrents")]
        private CheckBox _MakeSeparateTorrents;
        [AccessedThroughProperty("MultiTrackerEnabled")]
        private CheckBox _MultiTrackerEnabled;
        [AccessedThroughProperty("MultiTrackerSettings")]
        private Button _MultiTrackerSettings;
        [AccessedThroughProperty("OptionalHashProgress")]
        private ProgressBar _OptionalHashProgress;
        [AccessedThroughProperty("PieceSize")]
        private ComboBox _PieceSize;
        [AccessedThroughProperty("PrivateTorrent")]
        private CheckBox _PrivateTorrent;
        [AccessedThroughProperty("SaveSettings")]
        private Button _SaveSettings;
        [AccessedThroughProperty("SelectFile")]
        private Button _SelectFile;
        [AccessedThroughProperty("SelectFolder")]
        private Button _SelectFolder;
        [AccessedThroughProperty("TorrentComment")]
        private TextBox _TorrentComment;
        [AccessedThroughProperty("TorrentProgress")]
        private ProgressBar _TorrentProgress;
        private AdvancedConfiguration Advanced;
        public static ArrayList BlackListedFiles = new ArrayList();
        private IContainer components;
        public static int CountMultiplier;
        public static bool DelayMessages = true;
        private FileStream FileHandling;
        public static bool GenerateVerbose = true;
        public static string LocalPath;
        private MD4Managed md4;
        private MD5CryptoServiceProvider md5;
        private MultiTrackerGenerator Multitracker;
        private long piecesizetouse;
        private SHA1CryptoServiceProvider sha1;
        private ThexThreaded TigerHash;
        public static bool UseWSAConfig = false;

        public TorrentBuild()
        {
            base.Load += new EventHandler(this.TorrentBuild_Load);
            this.TigerHash = new ThexThreaded();
            this.sha1 = new SHA1CryptoServiceProvider();
            this.md5 = new MD5CryptoServiceProvider();
            this.md4 = new MD4Managed();
            this.Advanced = new AdvancedConfiguration();
            this.Multitracker = new MultiTrackerGenerator();
            this.InitializeComponent();
        }

        private void AdvSet_Click(object sender, EventArgs e)
        {
            new AdvancedConfiguration().Show();
        }

        private void AutomaticPieceSize_CheckedChanged(object sender, EventArgs e)
        {
            if (this.AutomaticPieceSize.Checked)
            {
                this.PieceSize.Enabled = false;
            }
            else
            {
                this.PieceSize.Enabled = true;
            }
        }

        private void BlacklistingScreen_Click(object sender, EventArgs e)
        {
            new ChangeBlacklistedFiles().Show();
        }

        private void BrowseForFile_FileOk(object sender, CancelEventArgs e)
        {
            this.FileNameToMake.Text = this.BrowseForFile.FileName;
        }

        private void BuildTorrentNow_Click(object sender, EventArgs e)
        {
            CountMultiplier = 0;
            this.OptionalHashProgress.Value = 0;
            this.AnnounceURL.Text = Strings.Trim(this.AnnounceURL.Text);
            if (Announces.IsValidAnnounce(this.AnnounceURL.Text) == 0)
            {
                if (this.AnnounceURL.Text != "")
                {
                    Interaction.MsgBox("AnnounceURL is invalid", MsgBoxStyle.Exclamation, "Error");
                    goto Label_024B;
                }
                if (Interaction.MsgBox("Announce URL is blank, continue anyway", MsgBoxStyle.YesNo, "Warning") == MsgBoxResult.No)
                {
                    goto Label_024B;
                }
            }
            if (DelayMessages)
            {
                int num = 0;
                if (this.IncludeCRC32.Checked)
                {
                    num++;
                }
                if (this.IncludeMD5.Checked)
                {
                    num += 2;
                }
                if (this.IncludeSHA1.Checked)
                {
                    num += 2;
                }
                if (num > 2)
                {
                    Interaction.MsgBox("The number of additional hashes you have requested may cause the torrent's generation to take an excessive amount of time.\nThis notification can be turned off in the advanced settings screen.", MsgBoxStyle.OkOnly, null);
                }
            }
            if (GenerateVerbose)
            {
                Interaction.MsgBox("Verbose torrent generation is active, additional information will be given between each file.\nThis setting can be turned off in the advanced settings creen.", MsgBoxStyle.OkOnly, null);
            }
            if (this.IncludeCRC32.Checked)
            {
                CountMultiplier++;
            }
            if (this.IncludeMD5.Checked)
            {
                CountMultiplier++;
            }
            if (this.IncludeSHA1.Checked)
            {
                CountMultiplier++;
            }
            if (this.IncludeED2K.Checked)
            {
                CountMultiplier++;
            }
            if (this.IncludeTiger.Checked)
            {
                CountMultiplier++;
            }
            if (Directory.Exists(this.FileNameToMake.Text))
            {
                this.BuildTorrentNow.Enabled = false;
                this.ExitWithoutSave.Enabled = false;
                this.ExitWithSave.Enabled = false;
                if (this.MakeSeparateTorrents.Checked)
                {
                    this.MakeTorrentsFromFolder();
                }
                else
                {
                    this.MakeOneTorrentFromFolder();
                }
                Interaction.MsgBox("Torrent Generated sucessfully", MsgBoxStyle.OkOnly, null);
                this.BuildTorrentNow.Enabled = true;
                this.ExitWithSave.Enabled = true;
                this.ExitWithoutSave.Enabled = true;
                return;
            }
            if (!File.Exists(this.FileNameToMake.Text))
            {
                Interaction.MsgBox("ERROR: Filename is not a valid file, can't make it", MsgBoxStyle.OkOnly, "WHOOPSIE!");
                return;
            }
            this.BuildTorrentNow.Enabled = false;
            this.ExitWithoutSave.Enabled = false;
            this.ExitWithSave.Enabled = false;
            this.MakeTorrentFromFile(this.FileNameToMake.Text);
            Interaction.MsgBox("Torrent Generated sucessfully", MsgBoxStyle.OkOnly, null);
        Label_024B:
            this.BuildTorrentNow.Enabled = true;
            this.ExitWithSave.Enabled = true;
            this.ExitWithoutSave.Enabled = true;
            GC.Collect();
        }

        private void CheckMultitTrackerTiers()
        {
            IEnumerator enumerator = null;
            try
            {
                enumerator = MultiTrackerGenerator.MultiTrackerTiers.Value.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    IEnumerator enumerator2 = null;
                    TorrentList current = (TorrentList) enumerator.Current;
                    try
                    {
                        enumerator2 = current.Value.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            TorrentString str2 = (TorrentString) enumerator2.Current;
                            if (str2.Value == Strings.Trim(this.AnnounceURL.Text))
                            {
                                return;
                            }
                        }
                        continue;
                    }
                    finally
                    {
                        if (enumerator2 is IDisposable)
                        {
                            (enumerator2 as IDisposable).Dispose();
                        }
                    }
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
            TorrentString str = new TorrentString {
                Value = Strings.Trim(this.AnnounceURL.Text)
            };
            TorrentList list = new TorrentList();
            list.Value.Add(str);
            MultiTrackerGenerator.MultiTrackerTiers.Value.Add(list);
            GC.Collect();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ExitWithoutSave_Click(object sender, EventArgs e)
        {
            ProjectData.EndApp();
        }

        private void ExitWithSave_Click(object sender, EventArgs e)
        {
            this.SaveSettings_Click(RuntimeHelpers.GetObjectValue(sender), e);
            ProjectData.EndApp();
        }

        private void FileNameToMake_DragDrop(object sender, DragEventArgs e)
        {
            this.FileNameToMake.Text = Conversions.ToString(e.Data.GetData("System.String", false));
        }

        private long getautopiecesize(long filesize)
        {
            if (filesize < 0x501720L)
            {
                return 0x8000L;
            }
            if (filesize < 0x9600000L)
            {
                return 0x10000L;
            }
            if (filesize < 0x15e00000L)
            {
                return 0x20000L;
            }
            if (filesize < 0x3333334L)
            {
                return 0x40000L;
            }
            if (filesize < 0x40000000L)
            {
                return 0x80000L;
            }
            if (filesize < 0x80000000L)
            {
                return 0x100000L;
            }
            return 0x200000L;
        }

        public void GetED2KHash(string Filename, ref string plaintext, ref string binaryvalue)
        {
            long num5;
            long num7 = FileSystem.FileLen(Filename);
            long num3 = 0L;
            long num6 = (long) Math.Round((double) (((double) num7) / 9728000.0));
            if ((num6 * 0x947000L) < num7)
            {
                num6 += 1L;
            }
            long num4 = num6 * 0x10L;
            byte[] buffer = new byte[((int) num4) + 1];
            long num2 = 0L;
            StringBuilder builder = new StringBuilder();
            this.HashProgress.Value = 0;
            this.FileHandling = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (num7 > 0x947000L)
            {
                do
                {
                    byte[] array = new byte[0x947001];
                    this.HashProgress.Value = (int) Math.Round((double) ((((double) num3) / ((double) num7)) * 100.0));
                    Application.DoEvents();
                    num5 = this.FileHandling.Read(array, 0, 0x947000);
                    byte[] buffer5 = new byte[((int) (num5 - 1L)) + 1];
                    long num13 = num5 - 1L;
                    for (long j = 0L; j <= num13; j += 1L)
                    {
                        buffer5[(int) j] = array[(int) j];
                    }
                    array = null;
                    byte[] bytes = this.md4.ComputeHash(buffer5);
                    buffer5 = null;
                    string str2 = "";
                    int index = 0;
                    foreach (byte num10 in bytes)
                    {
                        str2 = str2 + Encoding.Default.GetString(bytes, index, 1);
                        buffer[(int) num2] = num10;
                        num2 += 1L;
                        index++;
                    }
                    bytes = null;
                    num3 += num5;
                }
                while (num3 < num7);
                this.HashProgress.Value = 100;
                byte[] buffer3 = new byte[(buffer.GetUpperBound(0) - 1) + 1];
                int num15 = buffer.GetUpperBound(0) - 1;
                for (int i = 0; i <= num15; i++)
                {
                    buffer3[i] = buffer[i];
                }
                byte[] buffer2 = this.md4.ComputeHash(buffer3);
                HashChanger changer = new HashChanger {
                    bytehash = buffer2
                };
                binaryvalue = changer.rawhash;
                plaintext = changer.hexhash;
                GC.Collect();
            }
            else
            {
                byte[] buffer9 = new byte[0x947001];
                num5 = this.FileHandling.Read(buffer9, 0, 0x947000);
                byte[] buffer7 = new byte[((int) (num5 - 1L)) + 1];
                long num16 = num5 - 1L;
                for (long k = 0L; k <= num16; k += 1L)
                {
                    buffer7[(int) k] = buffer9[(int) k];
                }
                buffer9 = null;
                byte[] buffer8 = this.md4.ComputeHash(buffer7);
                HashChanger changer2 = new HashChanger {
                    bytehash = buffer8
                };
                binaryvalue = changer2.rawhash;
                plaintext = changer2.hexhash;
                buffer7 = null;
                this.HashProgress.Value = 100;
                GC.Collect();
            }
            this.FileHandling.Close();
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(TorrentBuild));
            this.FileNameToMake = new TextBox();
            this.BrowseForFile = new OpenFileDialog();
            this.SelectFile = new Button();
            this.PieceSize = new ComboBox();
            this.Label1 = new Label();
            this.Label2 = new Label();
            this.Label3 = new Label();
            this.AnnounceURL = new TextBox();
            this.IncludeMD5 = new CheckBox();
            this.IncludeSHA1 = new CheckBox();
            this.BuildTorrentNow = new Button();
            this.ExitWithSave = new Button();
            this.ExitWithoutSave = new Button();
            this.BrowseForFolder = new FolderBrowserDialog();
            this.SelectFolder = new Button();
            this.MakeSeparateTorrents = new CheckBox();
            this.Label4 = new Label();
            this.IncludeTorrents = new CheckBox();
            this.AutomaticPieceSize = new CheckBox();
            this.IncludeCRC32 = new CheckBox();
            this.IncludeED2K = new CheckBox();
            this.IncludeBlacklisted = new CheckBox();
            this.TorrentProgress = new ProgressBar();
            this.AdvSet = new Button();
            this.SaveSettings = new Button();
            this.Label5 = new Label();
            this.BlacklistingScreen = new Button();
            this.OptionalHashProgress = new ProgressBar();
            this.Label6 = new Label();
            this.Label7 = new Label();
            this.TorrentComment = new TextBox();
            this.Label8 = new Label();
            this.HashProgress = new ProgressBar();
            this.MakeExternals = new CheckBox();
            this.IncludeTiger = new CheckBox();
            this.PrivateTorrent = new CheckBox();
            this.MultiTrackerEnabled = new CheckBox();
            this.MultiTrackerSettings = new Button();
            this.SuspendLayout();
            this.FileNameToMake.AllowDrop = true;
            Point point = new Point(0, 0x18);
            this.FileNameToMake.Location = point;
            this.FileNameToMake.Name = "FileNameToMake";
            Size size = new Size(0x1f8, 20);
            this.FileNameToMake.Size = size;
            this.FileNameToMake.TabIndex = 0;
            this.FileNameToMake.Text = "";
            this.BrowseForFile.Filter = "All Files|*.*";
            point = new Point(0, 0x30);
            this.SelectFile.Location = point;
            this.SelectFile.Name = "SelectFile";
            size = new Size(0xf8, 0x18);
            this.SelectFile.Size = size;
            this.SelectFile.TabIndex = 1;
            this.SelectFile.Text = "Select File To Generate Torrent For";
            this.PieceSize.Items.AddRange(new object[] { "32768", "65536", "121072", "262144", "524288", "1048576", "2097152" });
            point = new Point(0, 0x58);
            this.PieceSize.Location = point;
            this.PieceSize.Name = "PieceSize";
            size = new Size(120, 0x15);
            this.PieceSize.Size = size;
            this.PieceSize.TabIndex = 2;
            this.PieceSize.Text = "262144";
            point = new Point(0, 8);
            this.Label1.Location = point;
            this.Label1.Name = "Label1";
            size = new Size(0x148, 0x10);
            this.Label1.Size = size;
            this.Label1.TabIndex = 3;
            this.Label1.Text = "Full path to the file or folder you want to generate torrent(s) for:";
            point = new Point(0, 0x48);
            this.Label2.Location = point;
            this.Label2.Name = "Label2";
            size = new Size(120, 0x10);
            this.Label2.Size = size;
            this.Label2.TabIndex = 4;
            this.Label2.Text = "Piece Size";
            point = new Point(0, 0x90);
            this.Label3.Location = point;
            this.Label3.Name = "Label3";
            size = new Size(0x178, 0x10);
            this.Label3.Size = size;
            this.Label3.TabIndex = 5;
            this.Label3.Text = "Announce URL";
            point = new Point(0, 160);
            this.AnnounceURL.Location = point;
            this.AnnounceURL.Name = "AnnounceURL";
            size = new Size(0x178, 20);
            this.AnnounceURL.Size = size;
            this.AnnounceURL.TabIndex = 6;
            this.AnnounceURL.Text = "";
            point = new Point(0, 240);
            this.IncludeMD5.Location = point;
            this.IncludeMD5.Name = "IncludeMD5";
            size = new Size(0x48, 0x10);
            this.IncludeMD5.Size = size;
            this.IncludeMD5.TabIndex = 7;
            this.IncludeMD5.Text = "MD5";
            point = new Point(0, 0x100);
            this.IncludeSHA1.Location = point;
            this.IncludeSHA1.Name = "IncludeSHA1";
            size = new Size(0x48, 0x10);
            this.IncludeSHA1.Size = size;
            this.IncludeSHA1.TabIndex = 8;
            this.IncludeSHA1.Text = "SHA1";
            point = new Point(360, 0x110);
            this.BuildTorrentNow.Location = point;
            this.BuildTorrentNow.Name = "BuildTorrentNow";
            size = new Size(0x90, 0x18);
            this.BuildTorrentNow.Size = size;
            this.BuildTorrentNow.TabIndex = 9;
            this.BuildTorrentNow.Text = "Build Torrent";
            this.ExitWithSave.DialogResult = DialogResult.Cancel;
            point = new Point(360, 0x1a8);
            this.ExitWithSave.Location = point;
            this.ExitWithSave.Name = "ExitWithSave";
            size = new Size(0x90, 0x18);
            this.ExitWithSave.Size = size;
            this.ExitWithSave.TabIndex = 10;
            this.ExitWithSave.Text = "Exit - Save Settings";
            point = new Point(360, 400);
            this.ExitWithoutSave.Location = point;
            this.ExitWithoutSave.Name = "ExitWithoutSave";
            size = new Size(0x90, 0x18);
            this.ExitWithoutSave.Size = size;
            this.ExitWithoutSave.TabIndex = 11;
            this.ExitWithoutSave.Text = "Exit - Don't Save Settings";
            point = new Point(0xf8, 0x30);
            this.SelectFolder.Location = point;
            this.SelectFolder.Name = "SelectFolder";
            size = new Size(0x100, 0x18);
            this.SelectFolder.Size = size;
            this.SelectFolder.TabIndex = 12;
            this.SelectFolder.Text = "Select Folder to Generate Torrent(s) for";
            point = new Point(0xb0, 0x48);
            this.MakeSeparateTorrents.Location = point;
            this.MakeSeparateTorrents.Name = "MakeSeparateTorrents";
            size = new Size(0x148, 0x10);
            this.MakeSeparateTorrents.Size = size;
            this.MakeSeparateTorrents.TabIndex = 13;
            this.MakeSeparateTorrents.Text = "Generate 1 torrent per file";
            point = new Point(0, 0xe0);
            this.Label4.Location = point;
            this.Label4.Name = "Label4";
            size = new Size(0x158, 0x10);
            this.Label4.Size = size;
            this.Label4.TabIndex = 14;
            this.Label4.Text = "Optional Hashes (some used by non-torrent peer to peer networks):";
            point = new Point(0xb0, 0x58);
            this.IncludeTorrents.Location = point;
            this.IncludeTorrents.Name = "IncludeTorrents";
            size = new Size(0x148, 0x10);
            this.IncludeTorrents.Size = size;
            this.IncludeTorrents.TabIndex = 15;
            this.IncludeTorrents.Text = "Include nested .torrent files in generation";
            this.AutomaticPieceSize.Checked = true;
            this.AutomaticPieceSize.CheckState = CheckState.Checked;
            point = new Point(0, 0x70);
            this.AutomaticPieceSize.Location = point;
            this.AutomaticPieceSize.Name = "AutomaticPieceSize";
            size = new Size(0x88, 0x10);
            this.AutomaticPieceSize.Size = size;
            this.AutomaticPieceSize.TabIndex = 0x10;
            this.AutomaticPieceSize.Text = "Automatic Piece Size";
            point = new Point(0x48, 240);
            this.IncludeCRC32.Location = point;
            this.IncludeCRC32.Name = "IncludeCRC32";
            size = new Size(0x48, 0x10);
            this.IncludeCRC32.Size = size;
            this.IncludeCRC32.TabIndex = 7;
            this.IncludeCRC32.Text = "CRC32";
            point = new Point(0x48, 0x100);
            this.IncludeED2K.Location = point;
            this.IncludeED2K.Name = "IncludeED2K";
            size = new Size(0x48, 0x10);
            this.IncludeED2K.Size = size;
            this.IncludeED2K.TabIndex = 7;
            this.IncludeED2K.Text = "ED2K";
            point = new Point(0xb0, 0x68);
            this.IncludeBlacklisted.Location = point;
            this.IncludeBlacklisted.Name = "IncludeBlacklisted";
            size = new Size(0x148, 0x10);
            this.IncludeBlacklisted.Size = size;
            this.IncludeBlacklisted.TabIndex = 0x13;
            this.IncludeBlacklisted.Text = "Include normally blacklisted files in generation";
            point = new Point(0, 0x180);
            this.TorrentProgress.Location = point;
            this.TorrentProgress.Name = "TorrentProgress";
            size = new Size(0x1f8, 0x10);
            this.TorrentProgress.Size = size;
            this.TorrentProgress.TabIndex = 20;
            point = new Point(0, 400);
            this.AdvSet.Location = point;
            this.AdvSet.Name = "AdvSet";
            size = new Size(0x90, 0x18);
            this.AdvSet.Size = size;
            this.AdvSet.TabIndex = 0x15;
            this.AdvSet.Text = "Advanced Settings";
            point = new Point(0, 0x1a8);
            this.SaveSettings.Location = point;
            this.SaveSettings.Name = "SaveSettings";
            size = new Size(0x90, 0x18);
            this.SaveSettings.Size = size;
            this.SaveSettings.TabIndex = 0x16;
            this.SaveSettings.Text = "Save All Settings";
            point = new Point(0, 0x170);
            this.Label5.Location = point;
            this.Label5.Name = "Label5";
            size = new Size(0x1f8, 0x10);
            this.Label5.Size = size;
            this.Label5.TabIndex = 0x17;
            this.Label5.Text = "Progress - Torrent Data Hashing:";
            point = new Point(0x128, 120);
            this.BlacklistingScreen.Location = point;
            this.BlacklistingScreen.Name = "BlacklistingScreen";
            size = new Size(0xd0, 0x18);
            this.BlacklistingScreen.Size = size;
            this.BlacklistingScreen.TabIndex = 0x18;
            this.BlacklistingScreen.Text = "Blacklisted Files/Extensions";
            point = new Point(0, 320);
            this.OptionalHashProgress.Location = point;
            this.OptionalHashProgress.Name = "OptionalHashProgress";
            size = new Size(0x1f8, 0x10);
            this.OptionalHashProgress.Size = size;
            this.OptionalHashProgress.TabIndex = 0x19;
            point = new Point(0, 0x130);
            this.Label6.Location = point;
            this.Label6.Name = "Label6";
            size = new Size(0x1f8, 0x10);
            this.Label6.Size = size;
            this.Label6.TabIndex = 0x1a;
            this.Label6.Text = "Progress - Optional Data Hashes";
            point = new Point(0, 0xb8);
            this.Label7.Location = point;
            this.Label7.Name = "Label7";
            size = new Size(0x1f8, 0x10);
            this.Label7.Size = size;
            this.Label7.TabIndex = 0x1b;
            this.Label7.Text = "Torrent Comment";
            point = new Point(0, 200);
            this.TorrentComment.Location = point;
            this.TorrentComment.Name = "TorrentComment";
            size = new Size(0x1f8, 20);
            this.TorrentComment.Size = size;
            this.TorrentComment.TabIndex = 0x1c;
            this.TorrentComment.Text = "";
            point = new Point(0, 0x150);
            this.Label8.Location = point;
            this.Label8.Name = "Label8";
            size = new Size(0x1f8, 0x10);
            this.Label8.Size = size;
            this.Label8.TabIndex = 0x1d;
            this.Label8.Text = "Progress - Current File's ED2K Hash:";
            point = new Point(0, 0x160);
            this.HashProgress.Location = point;
            this.HashProgress.Name = "HashProgress";
            size = new Size(0x1f8, 0x10);
            this.HashProgress.Size = size;
            this.HashProgress.TabIndex = 30;
            point = new Point(0, 0x110);
            this.MakeExternals.Location = point;
            this.MakeExternals.Name = "MakeExternals";
            size = new Size(0xd0, 0x10);
            this.MakeExternals.Size = size;
            this.MakeExternals.TabIndex = 0x1f;
            this.MakeExternals.Text = "Make External Hash Files";
            point = new Point(0x90, 240);
            this.IncludeTiger.Location = point;
            this.IncludeTiger.Name = "IncludeTiger";
            size = new Size(0x40, 0x10);
            this.IncludeTiger.Size = size;
            this.IncludeTiger.TabIndex = 0x20;
            this.IncludeTiger.Text = "Tiger";
            point = new Point(400, 0xe0);
            this.PrivateTorrent.Location = point;
            this.PrivateTorrent.Name = "PrivateTorrent";
            size = new Size(0x68, 0x10);
            this.PrivateTorrent.Size = size;
            this.PrivateTorrent.TabIndex = 0x21;
            this.PrivateTorrent.Text = "Private Torrent";
            point = new Point(0x178, 0x90);
            this.MultiTrackerEnabled.Location = point;
            this.MultiTrackerEnabled.Name = "MultiTrackerEnabled";
            size = new Size(0x80, 0x10);
            this.MultiTrackerEnabled.Size = size;
            this.MultiTrackerEnabled.TabIndex = 0x22;
            this.MultiTrackerEnabled.Text = "Multitracker Torrent";
            point = new Point(0x178, 160);
            this.MultiTrackerSettings.Location = point;
            this.MultiTrackerSettings.Name = "MultiTrackerSettings";
            size = new Size(0x80, 0x18);
            this.MultiTrackerSettings.Size = size;
            this.MultiTrackerSettings.TabIndex = 0x23;
            this.MultiTrackerSettings.Text = "Multitracker Settings";
            this.AcceptButton = this.BuildTorrentNow;
            this.AllowDrop = true;
            size = new Size(5, 13);
            this.AutoScaleBaseSize = size;
            this.CancelButton = this.ExitWithSave;
            size = new Size(0x1fa, 0x1bf);
            this.ClientSize = size;
            this.Controls.Add(this.MultiTrackerSettings);
            this.Controls.Add(this.MultiTrackerEnabled);
            this.Controls.Add(this.PrivateTorrent);
            this.Controls.Add(this.IncludeTiger);
            this.Controls.Add(this.MakeExternals);
            this.Controls.Add(this.HashProgress);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.TorrentComment);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.OptionalHashProgress);
            this.Controls.Add(this.BlacklistingScreen);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.SaveSettings);
            this.Controls.Add(this.AdvSet);
            this.Controls.Add(this.TorrentProgress);
            this.Controls.Add(this.IncludeBlacklisted);
            this.Controls.Add(this.AutomaticPieceSize);
            this.Controls.Add(this.IncludeTorrents);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.MakeSeparateTorrents);
            this.Controls.Add(this.SelectFolder);
            this.Controls.Add(this.ExitWithoutSave);
            this.Controls.Add(this.ExitWithSave);
            this.Controls.Add(this.BuildTorrentNow);
            this.Controls.Add(this.IncludeSHA1);
            this.Controls.Add(this.IncludeMD5);
            this.Controls.Add(this.AnnounceURL);
            this.Controls.Add(this.FileNameToMake);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.PieceSize);
            this.Controls.Add(this.SelectFile);
            this.Controls.Add(this.IncludeCRC32);
            this.Controls.Add(this.IncludeED2K);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = (Icon) manager.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TorrentBuild";
            this.Text = "Build A Torrent";
            this.ResumeLayout(false);
        }

        private bool IsFileCleared(string checkname)
        {
            bool flag = true;
            if (Strings.LCase(Strings.Right(checkname, 9)) == "thumbs.db")
            {
                flag = false;
            }
            if (Strings.LCase(Strings.Right(checkname, 11)) == "sthumbs.dat")
            {
                flag = false;
            }
            if (Strings.LCase(Strings.Right(checkname, 9)) == ".ds_store")
            {
                flag = false;
            }
            if ((Strings.LCase(Strings.Right(checkname, 8)) == ".torrent") & !this.IncludeTorrents.Checked)
            {
                flag = false;
            }
            if (!this.IncludeBlacklisted.Checked)
            {
                IEnumerator enumerator = null;
                try
                {
                    enumerator = BlackListedFiles.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        TorrentString current = (TorrentString) enumerator.Current;
                        if (Strings.LCase(Strings.Right(checkname, Strings.Len(current.Value))) == Strings.LCase(current.Value))
                        {
                            flag = false;
                        }
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }
            }
            GC.Collect();
            return flag;
        }

        [STAThread]
        public static void Main()
        {
            Application.Run(new TorrentBuild());
        }

        public void MakeOneTorrentFromFolder()
        {
            long num = 0;
            long num6 = 0;
            long num8 = 0;
            long num9 = 0;
            long num12 = 0;
            string[] strArray = Directory.GetFiles(this.FileNameToMake.Text, "*", SearchOption.AllDirectories);
            if (Strings.Right(this.FileNameToMake.Text, 1) != @"\")
            {
                this.FileNameToMake.Text = this.FileNameToMake.Text + @"\";
            }
            TorrentDictionary dictionary2 = new TorrentDictionary();
            TorrentList list = new TorrentList();
            ArrayList list2 = new ArrayList();
            this.OptionalHashProgress.Maximum = strArray.GetLength(0) * CountMultiplier;
            foreach (string str10 in strArray)
            {
                if (this.IsFileCleared(str10))
                {
                    FileStream stream;
                    byte[] hash;
                    IEnumerator enumerator = null;
                    TorrentDictionary dictionary3 = new TorrentDictionary();
                    TorrentNumber number2 = new TorrentNumber();
                    TorrentList list3 = new TorrentList();
                    ArrayList list4 = new ArrayList();
                    TorrentString str11 = new TorrentString();
                    number2.Value = FileSystem.FileLen(str10);
                    num12 += FileSystem.FileLen(str10);
                    if (this.AutomaticPieceSize.Checked)
                    {
                        num += number2.Value;
                    }
                    Array array = Strings.Right(str10, Strings.Len(str10) - Strings.Len(this.FileNameToMake.Text)).Split(new char[] { '\\' });
                    try
                    {
                        enumerator = array.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            string str13 = Conversions.ToString(enumerator.Current);
                            if (str13 != "")
                            {
                                TorrentString str14 = new TorrentString {
                                    Value = str13
                                };
                                list4.Add(str14);
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator is IDisposable)
                        {
                            (enumerator as IDisposable).Dispose();
                        }
                    }
                    list3.Value = list4;
                    dictionary3.Add("length", number2);
                    dictionary3.Add("path", list3);
                    StringBuilder builder = new StringBuilder();
                    HashChanger changer = new HashChanger();
                    if (number2.Value < 0x100000000L)
                    {
                        if (this.IncludeTiger.Checked)
                        {
                            string str16 = "";
                            this.TigerHash = new ThexThreaded();
                            TorrentString str15 = new TorrentString();
                            hash = this.TigerHash.GetTTH_Value(str10);
                            Application.DoEvents();
                            int index = 0;
                            foreach (byte num13 in hash)
                            {
                                str16 = str16 + Encoding.Default.GetString(hash, index, 1);
                                index++;
                            }
                            str15.Value = str16;
                            str16 = "";
                            hash = null;
                            this.OptionalHashProgress.Value++;
                            dictionary3.Add("tiger", str15);
                        }
                        GC.Collect();
                    }
                    else
                    {
                        this.OptionalHashProgress.Value++;
                    }
                    if (this.IncludeSHA1.Checked)
                    {
                        TorrentString str17 = new TorrentString();
                        stream = new FileStream(str10, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        this.sha1.ComputeHash(stream);
                        stream.Close();
                        Application.DoEvents();
                        hash = this.sha1.Hash;
                        changer.bytehash = this.sha1.Hash;
                        str17.Value = changer.rawhash;
                        builder.Remove(0, builder.Length);
                        hash = null;
                        this.OptionalHashProgress.Value++;
                        dictionary3.Add("sha1", str17);
                        GC.Collect();
                    }
                    if (this.IncludeMD5.Checked)
                    {
                        TorrentString str19 = new TorrentString();
                        stream = new FileStream(str10, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        builder = new StringBuilder();
                        this.md5.ComputeHash(stream);
                        Application.DoEvents();
                        stream.Close();
                        foreach (byte num13 in this.md5.Hash)
                        {
                            builder.AppendFormat("{0:x2}", num13);
                        }
                        str19.Value = builder.ToString();
                        builder.Remove(0, builder.Length);
                        hash = null;
                        this.OptionalHashProgress.Value++;
                        dictionary3.Add("md5sum", str19);
                        GC.Collect();
                    }
                    if (this.IncludeCRC32.Checked)
                    {
                        TorrentString str20 = new TorrentString();
                        CRC32 crc = new CRC32();
                        int num15 = 0;
                        stream = new FileStream(str10, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        Stream stream2 = stream;
                        stream = (FileStream) stream2;
                        num15 = crc.GetCrc32(ref stream2);
                        stream.Close();
                        Application.DoEvents();
                        str20.Value = string.Format("{0:X8}", num15);
                        this.OptionalHashProgress.Value++;
                        dictionary3.Add("crc32", str20);
                        GC.Collect();
                    }
                    if (this.IncludeED2K.Checked)
                    {
                        string str21 = "";
                        string str22 = "";
                        TorrentString str23 = new TorrentString();
                        this.GetED2KHash(str10, ref str22, ref str21);
                        this.OptionalHashProgress.Value++;
                        str23.Value = str21;
                        dictionary3.Add("ed2k", str23);
                        GC.Collect();
                    }
                    list2.Add(dictionary3);
                }
                else
                {
                    this.OptionalHashProgress.Maximum -= CountMultiplier;
                }
            }
            if (this.AutomaticPieceSize.Checked)
            {
                num9 = this.getautopiecesize(num);
            }
            else
            {
                num9 = Conversions.ToInteger(this.PieceSize.Text);
            }
            list.Value = list2;
            TorrentNumber number = new TorrentNumber {
                Value = num9
            };
            dictionary2.Add("files", list);
            dictionary2.Add("piece length", number);
            long upperBound = strArray.GetUpperBound(0);
            long num2 = 0L;
            long num3 = 0L;
            byte[] buffer = new byte[((int) num9) + 1];
            byte[] bytes = new byte[0x15];
            while (true)
            {
                if (this.IsFileCleared(strArray[(int) num2]))
                {
                    this.FileHandling = File.Open(strArray[(int) num2], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    if (GenerateVerbose)
                    {
                        Interaction.MsgBox("Current File: " + strArray[(int) num2] + "\n File offset for Piece 0 of this file: " + Conversions.ToString(num3), MsgBoxStyle.OkOnly, null);
                    }
                    break;
                }
                num2 += 1L;
            }
        Label_062B:
            Thread.Sleep(10);
            Application.DoEvents();
            this.TorrentProgress.Value = (int) Math.Round((double) ((((double) num3) / ((double) num12)) * 100.0));
            byte[] buffer4 = new byte[((int) (num9 - 1L)) + 1];
        Label_0668:
            num6 = this.FileHandling.Read(buffer4, (int) num8, (int) (num9 - num8));
            num8 += num6;
            if (!((num2 <= upperBound) & (num8 < num9)))
            {
                string str3 = "";
                byte[] buffer5 = new byte[((int) (num8 - 1L)) + 1];
                long num27 = num8 - 1L;
                for (long i = 0L; i <= num27; i += 1L)
                {
                    buffer5[(int) i] = buffer4[(int) i];
                }
                buffer4 = null;
                bytes = this.sha1.ComputeHash(buffer5);
                int num17 = 0;
                do
                {
                    str3 = str3 + Encoding.Default.GetString(bytes, num17, 1);
                    num17++;
                }
                while (num17 <= 0x13);
                bytes = null;
                num3 += num8;
                num8 = 0L;
                GC.Collect();
                if (num3 >= num12)
                {
                    this.FileHandling.Close();
                    if (GenerateVerbose)
                    {
                        Interaction.MsgBox("Size of files hashed: " + Conversions.ToString(num3) + " Size of files compared: " + Conversions.ToString(num12) + "\nNow Generating .torrent file", MsgBoxStyle.OkOnly, null);
                    }
                    this.TorrentProgress.Value = 100;
                    string path = Strings.Left(this.FileNameToMake.Text, Strings.Len(this.FileNameToMake.Text) - 1) + ".torrent";
                    TorrentString str9 = new TorrentString {
                        Value = str3
                    };
                    dictionary2.Add("pieces", str9);
                    int num5 = Strings.Left(this.FileNameToMake.Text, Strings.Len(this.FileNameToMake.Text) - 1).LastIndexOf(@"\");
                    int num4 = Strings.Len(this.FileNameToMake.Text) - num5;
                    string str8 = Strings.Right(Strings.Left(this.FileNameToMake.Text, Strings.Len(this.FileNameToMake.Text) - 1), num4 - 2);
                    TorrentString str7 = new TorrentString {
                        Value = str8
                    };
                    TorrentDictionary dictionary = new TorrentDictionary();
                    TorrentString str4 = new TorrentString();
                    dictionary2.Add("name", str7);
                    str4.Value = this.AnnounceURL.Text;
                    if (UseWSAConfig)
                    {
                        ArrayList returnarray = new ArrayList();
                        TorrentList list6 = new TorrentList();
                        int webSeedData = TorrentGenFamily.GetWebSeedData(returnarray);
                        list6.Value = returnarray;
                        if (webSeedData >= 1)
                        {
                            dictionary.Add("httpseeds", list6);
                        }
                    }
                    TorrentString str2 = new TorrentString {
                        Value = "Torrent Generated by VB.Net TorrentGen - Written by DWKnight"
                    };
                    dictionary.Add("created by", str2);
                    TorrentString str = new TorrentString();
                    if (Strings.Trim(this.TorrentComment.Text) != "")
                    {
                        str.Value = Strings.Trim(this.TorrentComment.Text);
                        dictionary.Add("comment", str);
                    }
                    if (this.AnnounceURL.Text != "")
                    {
                        dictionary.Add("announce", str4);
                    }
                    if (this.PrivateTorrent.Checked)
                    {
                        TorrentNumber number3 = new TorrentNumber {
                            Value = 1L
                        };
                        dictionary2["private"] = number3;
                    }
                    dictionary.Add("info", dictionary2);
                    TorrentString str5 = new TorrentString {
                        Value = "UTF8"
                    };
                    dictionary.Add("encoding", str5);
                    if (this.MultiTrackerEnabled.Checked)
                    {
                        this.CheckMultitTrackerTiers();
                        dictionary.Add("announce-list", MultiTrackerGenerator.MultiTrackerTiers);
                    }
                    if (File.Exists(path))
                    {
                        FileSystem.Kill(path);
                    }
                    int fileNumber = FileSystem.FreeFile();
                    FileSystem.FileOpen(fileNumber, path, OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.LockReadWrite, -1);
                    FileSystem.FilePut(fileNumber, dictionary.BEncoded, -1L, false);
                    FileSystem.FileClose(new int[] { fileNumber });
                    GC.Collect();
                    if (this.MakeExternals.Checked)
                    {
                        string str26 = "";
                        string str28 = "";
                        string str29 = "";
                        string str30 = "";
                        string str32 = "";
                        string str34 = "";
                        IEnumerator enumerator2 = null;
                        string expression = Strings.Left(this.FileNameToMake.Text, Strings.Len(this.FileNameToMake.Text) - 1);
                        string str27 = expression + ".md5";
                        string str31 = expression + ".sha1";
                        string str25 = expression + ".ed2k";
                        string str33 = expression + ".tiger";
                        string[] strArray2 = Strings.Split(expression, @"\", -1, CompareMethod.Binary);
                        strArray2[strArray2.GetUpperBound(0)] = Strings.Replace(strArray2[strArray2.GetUpperBound(0)], " ", ".", 1, -1, CompareMethod.Binary);
                        strArray2[strArray2.GetUpperBound(0)] = Strings.Replace(strArray2[strArray2.GetUpperBound(0)], "_", ".", 1, -1, CompareMethod.Binary);
                        foreach (string str35 in strArray2)
                        {
                            str29 = str29 + @"\" + str35;
                        }
                        str29 = Strings.Mid(str29, 2, Strings.Len(str29) - 1) + ".sfv";
                        LinkGeneration generation = new LinkGeneration();
                        HashChanger changer2 = new HashChanger();
                        try
                        {
                            enumerator2 = list2.GetEnumerator();
                            while (enumerator2.MoveNext())
                            {
                                IEnumerator enumerator3 = null;
                                TorrentDictionary current = (TorrentDictionary) enumerator2.Current;
                                generation = new LinkGeneration();
                                TorrentString str36 = new TorrentString();
                                try
                                {
                                    enumerator3 = ((IEnumerable) NewLateBinding.LateGet(current["path"], null, "value", new object[0], null, null, null)).GetEnumerator();
                                    while (enumerator3.MoveNext())
                                    {
                                        TorrentString str37 = (TorrentString) enumerator3.Current;
                                        str36 = str37;
                                    }
                                }
                                finally
                                {
                                    if (enumerator3 is IDisposable)
                                    {
                                        (enumerator3 as IDisposable).Dispose();
                                    }
                                }
                                if (this.IncludeMD5.Checked)
                                {
                                    TorrentString str38 = new TorrentString();
                                    str38 = (TorrentString) current["md5sum"];
                                    str28 = str28 + str36.Value + " " + str38.Value + "\r\n";
                                }
                                if (this.IncludeSHA1.Checked)
                                {
                                    changer2 = new HashChanger {
                                        rawhash = Conversions.ToString(NewLateBinding.LateGet(current["sha1"], null, "value", new object[0], null, null, null))
                                    };
                                    str32 = str32 + str36.Value + " " + changer2.hexhash + "\r\n";
                                }
                                if (this.IncludeTiger.Checked && current.Contains("tiger"))
                                {
                                    changer2 = new HashChanger {
                                        rawhash = Conversions.ToString(NewLateBinding.LateGet(current["tiger"], null, "value", new object[0], null, null, null))
                                    };
                                    str34 = str34 + str36.Value + " " + changer2.base32 + "\r\n";
                                }
                                if (this.IncludeCRC32.Checked)
                                {
                                    str30 = Conversions.ToString(Operators.AddObject(Operators.AddObject(Operators.AddObject(str30 + str36.Value + " ", NewLateBinding.LateGet(current["crc32"], null, "value", new object[0], null, null, null)), '\r'), '\n'));
                                }
                                if (this.IncludeED2K.Checked)
                                {
                                    generation.ED2KRaw = Conversions.ToString(NewLateBinding.LateGet(current["ed2k"], null, "Value", new object[0], null, null, null));
                                    generation.FileSize = Conversions.ToLong(NewLateBinding.LateGet(current["length"], null, "Value", new object[0], null, null, null));
                                    generation.FileName = str36.Value;
                                    str26 = str26 + generation.ClassicED2KLink + "\r\n";
                                    str26 = str26 + str36.Value + " " + generation.ED2KHex + "\r\n";
                                }
                                GC.Collect();
                            }
                        }
                        finally
                        {
                            if (enumerator2 is IDisposable)
                            {
                                (enumerator2 as IDisposable).Dispose();
                            }
                        }
                        if (this.IncludeMD5.Checked)
                        {
                            if (File.Exists(str27))
                            {
                                FileSystem.Kill(str27);
                            }
                            int num19 = FileSystem.FreeFile();
                            FileSystem.FileOpen(num19, str27, OpenMode.Output, OpenAccess.Default, OpenShare.Default, -1);
                            FileSystem.Print(num19, new object[] { str28 });
                            FileSystem.FileClose(new int[] { num19 });
                        }
                        if (this.IncludeTiger.Checked)
                        {
                            if (File.Exists(str33))
                            {
                                FileSystem.Kill(str33);
                            }
                            int num20 = FileSystem.FreeFile();
                            FileSystem.FileOpen(num20, str33, OpenMode.Output, OpenAccess.Default, OpenShare.Default, -1);
                            FileSystem.Print(num20, new object[] { str34 });
                            FileSystem.FileClose(new int[] { num20 });
                        }
                        if (this.IncludeSHA1.Checked)
                        {
                            if (File.Exists(str31))
                            {
                                FileSystem.Kill(str31);
                            }
                            int num21 = FileSystem.FreeFile();
                            FileSystem.FileOpen(num21, str31, OpenMode.Output, OpenAccess.Default, OpenShare.Default, -1);
                            FileSystem.Print(num21, new object[] { str32 });
                            FileSystem.FileClose(new int[] { num21 });
                        }
                        if (this.IncludeED2K.Checked)
                        {
                            if (File.Exists(str25))
                            {
                                FileSystem.Kill(str25);
                            }
                            int num22 = FileSystem.FreeFile();
                            FileSystem.FileOpen(num22, str25, OpenMode.Output, OpenAccess.Default, OpenShare.Default, -1);
                            FileSystem.Print(num22, new object[] { str26 });
                            FileSystem.FileClose(new int[] { num22 });
                        }
                        if (this.IncludeCRC32.Checked)
                        {
                            if (File.Exists(str29))
                            {
                                FileSystem.Kill(str29);
                            }
                            int num23 = FileSystem.FreeFile();
                            FileSystem.FileOpen(num23, str29, OpenMode.Output, OpenAccess.Default, OpenShare.Default, -1);
                            FileSystem.Print(num23, new object[] { str30 });
                            FileSystem.FileClose(new int[] { num23 });
                        }
                    }
                    GC.Collect();
                    return;
                }
                goto Label_062B;
            }
            num2 += 1L;
            while (true)
            {
                if (num2 > strArray.GetUpperBound(0))
                {
                    goto Label_0668;
                }
                if (this.IsFileCleared(strArray[(int) num2]))
                {
                    break;
                }
                num2 += 1L;
            }
            this.FileHandling.Close();
            this.FileHandling = File.Open(strArray[(int) num2], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (GenerateVerbose)
            {
                Interaction.MsgBox("Current File: " + strArray[(int) num2] + "\n File offset for Piece 0 of this file: " + Conversions.ToString(num3), MsgBoxStyle.OkOnly, null);
            }
            goto Label_0668;
        }

        public void MakeTorrentFromFile(string NameOfFile)
        {
            FileStream stream;
            string str6 = "";
            string hexhash = "";
            string str13 = "";
            string str14 = "";
            long filesize = FileSystem.FileLen(NameOfFile);
            TorrentString str10 = new TorrentString();
            TorrentString str7 = new TorrentString();
            TorrentString str5 = new TorrentString();
            TorrentString str4 = new TorrentString();
            TorrentString str12 = new TorrentString();
            StringBuilder builder = new StringBuilder();
            HashChanger changer = new HashChanger();
            this.OptionalHashProgress.Maximum = CountMultiplier;
            this.OptionalHashProgress.Value = 0;
            if (filesize < 0x100000000L)
            {
                if (this.IncludeTiger.Checked)
                {
                    this.TigerHash = new ThexThreaded();
                    changer = new HashChanger {
                        bytehash = this.TigerHash.GetTTH_Value(NameOfFile)
                    };
                    Application.DoEvents();
                    str12.Value = changer.rawhash;
                    str13 = changer.base32;
                    this.OptionalHashProgress.Value++;
                }
            }
            else
            {
                this.OptionalHashProgress.Value++;
            }
            if (this.IncludeSHA1.Checked)
            {
                changer = new HashChanger();
                stream = new FileStream(NameOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                this.sha1.ComputeHash(stream);
                stream.Close();
                Application.DoEvents();
                changer.bytehash = this.sha1.Hash;
                str10.Value = changer.rawhash;
                hexhash = changer.hexhash;
                this.OptionalHashProgress.Value++;
            }
            if (this.IncludeMD5.Checked)
            {
                changer = new HashChanger();
                stream = new FileStream(NameOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                this.md5.ComputeHash(stream);
                stream.Close();
                changer.bytehash = this.md5.Hash;
                Application.DoEvents();
                str7.Value = changer.hexhash;
                this.OptionalHashProgress.Value++;
            }
            if (this.IncludeCRC32.Checked)
            {
                CRC32 crc = new CRC32();
                int num7 = 0;
                stream = new FileStream(NameOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Stream stream2 = stream;
                stream = (FileStream) stream2;
                num7 = crc.GetCrc32(ref stream2);
                stream.Close();
                str4.Value = string.Format("{0:X8}", num7);
                Application.DoEvents();
                this.OptionalHashProgress.Value++;
            }
            if (this.IncludeED2K.Checked)
            {
                string str3 = "";
                this.GetED2KHash(NameOfFile, ref str6, ref str3);
                this.OptionalHashProgress.Value++;
                str5.Value = str3;
            }
            this.FileHandling = File.Open(NameOfFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] bytes = new byte[0x15];
            if (this.AutomaticPieceSize.Checked)
            {
                this.piecesizetouse = this.getautopiecesize(filesize);
            }
            else
            {
                this.piecesizetouse = Conversions.ToInteger(this.PieceSize.Text);
            }
            long num3 = 0L;
            this.FileHandling.Seek(0L, SeekOrigin.Begin);
            while (num3 < filesize)
            {
                this.TorrentProgress.Value = (int) Math.Round((double) ((((double) num3) / ((double) filesize)) * 100.0));
                Application.DoEvents();
                byte[] array = new byte[((int) this.piecesizetouse) + 1];
                long num5 = this.FileHandling.Read(array, 0, (int) this.piecesizetouse);
                byte[] buffer = new byte[((int) (num5 - 1L)) + 1];
                long num16 = num5 - 1L;
                for (long i = 0L; i <= num16; i += 1L)
                {
                    buffer[(int) i] = array[(int) i];
                }
                bytes = this.sha1.ComputeHash(buffer);
                int index = 0;
                do
                {
                    str14 = str14 + Encoding.Default.GetString(bytes, index, 1);
                    index++;
                }
                while (index <= 0x13);
                num3 += this.piecesizetouse;
                buffer = null;
                array = null;
                Thread.Sleep(10);
                GC.Collect();
            }
            this.TorrentProgress.Value = 100;
            this.FileHandling.Close();
            TorrentString str20 = new TorrentString();
            int length = NameOfFile.LastIndexOf(@"\");
            int num = Strings.Len(NameOfFile) - length;
            string str8 = Strings.Right(NameOfFile, num - 1);
            string str9 = Strings.Left(NameOfFile, length);
            TorrentString str19 = new TorrentString {
                Value = str8
            };
            TorrentString str17 = new TorrentString();
            TorrentNumber number = new TorrentNumber {
                Value = filesize
            };
            TorrentNumber number2 = new TorrentNumber {
                Value = this.piecesizetouse
            };
            str20.Value = str14;
            str17.Value = "UTF8";
            TorrentDictionary dictionary2 = new TorrentDictionary();
            TorrentDictionary dictionary = new TorrentDictionary();
            TorrentString str16 = new TorrentString {
                Value = this.AnnounceURL.Text
            };
            dictionary.Add("length", number);
            dictionary.Add("name", str19);
            dictionary.Add("pieces", str20);
            dictionary.Add("piece length", number2);
            if (this.MultiTrackerEnabled.Checked)
            {
                this.CheckMultitTrackerTiers();
                dictionary2.Add("announce-list", MultiTrackerGenerator.MultiTrackerTiers);
            }
            if (this.IncludeSHA1.Checked)
            {
                dictionary.Add("sha1", str10);
            }
            if (this.IncludeMD5.Checked)
            {
                dictionary.Add("md5sum", str7);
            }
            if (this.IncludeCRC32.Checked)
            {
                dictionary.Add("crc32", str4);
            }
            if (this.IncludeED2K.Checked)
            {
                dictionary.Add("ed2k", str5);
            }
            if (this.IncludeTiger.Checked)
            {
                dictionary.Add("tiger", str12);
            }
            if (UseWSAConfig)
            {
                ArrayList returnarray = new ArrayList();
                TorrentList list2 = new TorrentList();
                int webSeedData = TorrentGenFamily.GetWebSeedData(returnarray);
                list2.Value = returnarray;
                if (webSeedData >= 1)
                {
                    dictionary2.Add("httpseeds", list2);
                }
            }
            TorrentString str2 = new TorrentString {
                Value = "Torrent Generated by VB.Net TorrentGen - Written by DWKnight"
            };
            dictionary2.Add("created by", str2);
            TorrentString str = new TorrentString();
            if (Strings.Trim(this.TorrentComment.Text) != "")
            {
                str.Value = Strings.Trim(this.TorrentComment.Text);
                dictionary2.Add("comment", str);
            }
            if (this.PrivateTorrent.Checked)
            {
                TorrentNumber number3 = new TorrentNumber {
                    Value = 1L
                };
                dictionary["private"] = number3;
            }
            dictionary2.Add("info", dictionary);
            if (this.AnnounceURL.Text != "")
            {
                dictionary2.Add("announce", str16);
            }
            dictionary2.Add("encoding", str17);
            int fileNumber = FileSystem.FreeFile();
            string fileName = NameOfFile + ".torrent";
            FileSystem.FileOpen(fileNumber, fileName, OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.LockReadWrite, -1);
            FileSystem.FilePut(fileNumber, dictionary2.BEncoded, -1L, false);
            FileSystem.FileClose(new int[] { fileNumber });
            bytes = null;
            if (this.MakeExternals.Checked)
            {
                if (this.IncludeSHA1.Checked)
                {
                    int num11 = FileSystem.FreeFile();
                    FileSystem.FileOpen(num11, NameOfFile + ".sha1", OpenMode.Output, OpenAccess.Default, OpenShare.Default, -1);
                    FileSystem.PrintLine(num11, new object[] { str19.Value + " " + hexhash });
                    FileSystem.FileClose(new int[] { num11 });
                }
                if ((filesize <= 0x118940000L) && this.IncludeTiger.Checked)
                {
                    int num12 = FileSystem.FreeFile();
                    FileSystem.FileOpen(num12, NameOfFile + ".tiger", OpenMode.Output, OpenAccess.Default, OpenShare.Default, -1);
                    FileSystem.PrintLine(num12, new object[] { str19.Value + " " + str13 });
                    FileSystem.FileClose(new int[] { num12 });
                }
                if (this.IncludeMD5.Checked)
                {
                    int num13 = FileSystem.FreeFile();
                    FileSystem.FileOpen(num13, NameOfFile + ".md5", OpenMode.Output, OpenAccess.Default, OpenShare.Default, -1);
                    FileSystem.PrintLine(num13, new object[] { str19.Value + " " + str7.Value });
                    FileSystem.FileClose(new int[] { num13 });
                }
                if (this.IncludeCRC32.Checked)
                {
                    int num14 = FileSystem.FreeFile();
                    FileSystem.FileOpen(num14, NameOfFile + ".sfv", OpenMode.Output, OpenAccess.Default, OpenShare.Default, -1);
                    FileSystem.PrintLine(num14, new object[] { str19.Value + " " + str4.Value });
                    FileSystem.FileClose(new int[] { num14 });
                }
                if (this.IncludeED2K.Checked)
                {
                    LinkGeneration generation = new LinkGeneration {
                        FileName = str19.Value,
                        FileSize = number.Value,
                        ED2KHex = str6
                    };
                    int num15 = FileSystem.FreeFile();
                    FileSystem.FileOpen(num15, NameOfFile + ".ed2k", OpenMode.Output, OpenAccess.Default, OpenShare.Default, -1);
                    FileSystem.PrintLine(num15, new object[] { generation.ClassicED2KLink });
                    FileSystem.PrintLine(num15, new object[] { str19.Value + " " + generation.ED2KHex + "\r\n" });
                    FileSystem.FileClose(new int[] { num15 });
                }
            }
            GC.Collect();
        }

        private void MakeTorrentsFromFolder()
        {
            foreach (string str in Directory.GetFiles(this.FileNameToMake.Text))
            {
                if (this.IsFileCleared(str))
                {
                    this.MakeTorrentFromFile(str);
                }
            }
        }

        private void MultiTrackerSettings_Click(object sender, EventArgs e)
        {
            TorrentList multiTrackerTiers = new TorrentList();
            multiTrackerTiers = MultiTrackerGenerator.MultiTrackerTiers;
            this.Multitracker = new MultiTrackerGenerator();
            MultiTrackerGenerator.MultiTrackerTiers = multiTrackerTiers;
            this.Multitracker.UpdateInput();
            this.Multitracker.Show();
        }

        private void PieceSize_LostFocus(object sender, EventArgs e)
        {
            int num = (int) Math.Round((double) (Math.Round((double) (((double) Conversions.ToInteger(this.PieceSize.Text)) / 16384.0), 0) * 16384.0));
            if (num < 0x8000)
            {
                num = 0x8000;
            }
            this.PieceSize.Text = Conversions.ToString(num);
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {
            this.AnnounceURL.Text = Strings.Trim(this.AnnounceURL.Text);
            TorrentString str = new TorrentString();
            TorrentNumber number3 = new TorrentNumber();
            TorrentNumber number11 = new TorrentNumber();
            TorrentNumber number9 = new TorrentNumber();
            TorrentNumber number7 = new TorrentNumber();
            TorrentNumber number8 = new TorrentNumber();
            TorrentNumber number12 = new TorrentNumber();
            TorrentNumber number6 = new TorrentNumber();
            TorrentNumber number = new TorrentNumber();
            TorrentNumber number5 = new TorrentNumber();
            TorrentDictionary dictionary = new TorrentDictionary();
            TorrentNumber number14 = new TorrentNumber();
            TorrentNumber number13 = new TorrentNumber();
            TorrentNumber number4 = new TorrentNumber();
            TorrentList list = new TorrentList();
            TorrentString str2 = new TorrentString();
            TorrentNumber number10 = new TorrentNumber();
            list.Value = BlackListedFiles;
            number14.Value = (long) -(Convert.ToInt32(GenerateVerbose));
            number13.Value = (long) -(Convert.ToInt32(UseWSAConfig));
            number4.Value = (long) -(Convert.ToInt32(DelayMessages));
            dictionary.Add("verbose", number14);
            dictionary.Add("usewsa", number13);
            dictionary.Add("delay", number4);
            str.Value = this.AnnounceURL.Text;
            number3.Value = Conversions.ToInteger(this.PieceSize.Text);
            number5.Value = (long) -(Convert.ToInt32(this.MakeSeparateTorrents.Checked));
            number11.Value = (long) -(Convert.ToInt32(this.IncludeSHA1.Checked));
            number9.Value = (long) -(Convert.ToInt32(this.IncludeMD5.Checked));
            number7.Value = (long) -(Convert.ToInt32(this.IncludeCRC32.Checked));
            number8.Value = (long) -(Convert.ToInt32(this.IncludeED2K.Checked));
            number12.Value = (long) -(Convert.ToInt32(this.IncludeTiger.Checked));
            number6.Value = (long) -(Convert.ToInt32(this.MakeExternals.Checked));
            number.Value = (long) -(Convert.ToInt32(this.AutomaticPieceSize.Checked));
            str2.Value = Strings.Trim(this.TorrentComment.Text);
            number10.Value = (long)-(Convert.ToInt32(this.MultiTrackerEnabled.Checked));
            TorrentDictionary dictionary2 = new TorrentDictionary();
            TorrentNumber number2 = new TorrentNumber();
            TorrentList list2 = new TorrentList {
                Value = new ArrayList()
            };
            if (list.BEncoded != list2.BEncoded)
            {
                dictionary2.Add("blacklist", list);
                number2.Value = 0L;
            }
            else
            {
                number2.Value = -1L;
            }
            if (MultiTrackerGenerator.MultiTrackerTiers.BEncoded != list2.BEncoded)
            {
                this.CheckMultitTrackerTiers();
                dictionary2.Add("multitracker", MultiTrackerGenerator.MultiTrackerTiers);
            }
            dictionary2.Add("usemultitracker", number10);
            dictionary2.Add("blankblacklist", number2);
            dictionary2.Add("advanced", dictionary);
            dictionary2.Add("tracker", str);
            dictionary2.Add("piecesize", number3);
            dictionary2.Add("sha1", number11);
            dictionary2.Add("md5", number9);
            dictionary2.Add("crc", number7);
            dictionary2.Add("ed2k", number8);
            dictionary2.Add("tiger", number12);
            dictionary2.Add("externals", number6);
            dictionary2.Add("autopiece", number);
            dictionary2.Add("folder", number5);
            dictionary2.Add("comment", str2);
            int fileNumber = FileSystem.FreeFile();
            if (File.Exists(LocalPath + "tgen.configure"))
            {
                FileSystem.Kill(LocalPath + "tgen.configure");
            }
            FileSystem.FileOpen(fileNumber, LocalPath + "tgen.configure", OpenMode.Binary, OpenAccess.ReadWrite, OpenShare.LockReadWrite, -1);
            FileSystem.FilePut(fileNumber, dictionary2.BEncoded, -1L, false);
            FileSystem.FileClose(new int[] { fileNumber });
            GC.Collect();
        }

        private void SelectFile_Click(object sender, EventArgs e)
        {
            this.BrowseForFile.ShowDialog();
        }

        private void SelectFolder_Click(object sender, EventArgs e)
        {
            if (this.BrowseForFolder.ShowDialog() == DialogResult.OK)
            {
                this.FileNameToMake.Text = this.BrowseForFolder.SelectedPath;
            }
        }

        private void TorrentBuild_Load(object sender, EventArgs e)
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            if (commandLineArgs.GetLength(0) > 1)
            {
                this.FileNameToMake.Text = commandLineArgs[1];
            }
            TorrentString str3 = new TorrentString();
            TorrentString str = new TorrentString();
            TorrentString str2 = new TorrentString();
            str3.Value = ".sfv";
            str.Value = ".cdp";
            str2.Value = ".cdt";
            BlackListedFiles.Add(str3);
            BlackListedFiles.Add(str);
            BlackListedFiles.Add(str2);
            int length = Strings.Left(commandLineArgs[0], Strings.Len(commandLineArgs[0])).LastIndexOf(@"\");
            LocalPath = Path.GetFullPath(Strings.Left(commandLineArgs[0], length)) + @"\";
            if (File.Exists(LocalPath + "tgen.configure"))
            {
                int fileNumber = FileSystem.FreeFile();
                FileSystem.FileOpen(fileNumber, LocalPath + "tgen.configure", OpenMode.Binary, OpenAccess.Read, OpenShare.LockRead, -1);
                string str4 = Strings.Space((int) FileSystem.FileLen(LocalPath + "tgen.configure"));
                FileSystem.FileGet(fileNumber, ref str4, -1L, false);
                FileSystem.FileClose(new int[] { fileNumber });
                TorrentDictionary dictionary = new TorrentDictionary();
                dictionary.Parse(str4);
                TorrentString str5 = new TorrentString();
                TorrentNumber number2 = new TorrentNumber();
                TorrentNumber number8 = new TorrentNumber();
                TorrentNumber number7 = new TorrentNumber();
                TorrentNumber number5 = new TorrentNumber();
                TorrentString str6 = new TorrentString();
                TorrentNumber number6 = new TorrentNumber();
                TorrentNumber number9 = new TorrentNumber();
                TorrentNumber number = new TorrentNumber();
                TorrentNumber number3 = new TorrentNumber();
                TorrentNumber number4 = new TorrentNumber();
                str5 = (TorrentString) dictionary["tracker"];
                number2 = (TorrentNumber) dictionary["piecesize"];
                if (dictionary.Contains("externals"))
                {
                    number4 = (TorrentNumber) dictionary["externals"];
                    this.MakeExternals.Checked = number4.Value > 0L;
                }
                if (dictionary.Contains("folder"))
                {
                    number3 = (TorrentNumber) dictionary["folder"];
                    this.MakeSeparateTorrents.Checked = number3.Value > 0L;
                }
                if (dictionary.Contains("sha1"))
                {
                    number8 = (TorrentNumber) dictionary["sha1"];
                    this.IncludeSHA1.Checked = number8.Value > 0L;
                }
                if (dictionary.Contains("md5"))
                {
                    number7 = (TorrentNumber) dictionary["md5"];
                    this.IncludeMD5.Checked = number7.Value > 0L;
                }
                if (dictionary.Contains("crc"))
                {
                    number5 = (TorrentNumber) dictionary["crc"];
                    this.IncludeCRC32.Checked = number5.Value > 0L;
                }
                if (dictionary.Contains("ed2k"))
                {
                    number6 = (TorrentNumber) dictionary["ed2k"];
                    this.IncludeED2K.Checked = number6.Value > 0L;
                }
                if (dictionary.Contains("tiger"))
                {
                    number9 = (TorrentNumber) dictionary["tiger"];
                    this.IncludeTiger.Checked = number9.Value > 0L;
                }
                if (dictionary.Contains("comment"))
                {
                    str6 = (TorrentString) dictionary["comment"];
                    this.TorrentComment.Text = str6.Value;
                }
                if (dictionary.Contains("advanced"))
                {
                    TorrentDictionary dictionary2 = new TorrentDictionary();
                    dictionary2 = (TorrentDictionary) dictionary["advanced"];
                    TorrentNumber number12 = new TorrentNumber();
                    TorrentNumber number11 = new TorrentNumber();
                    TorrentNumber number10 = new TorrentNumber();
                    number12 = (TorrentNumber) dictionary2["verbose"];
                    number11 = (TorrentNumber) dictionary2["usewsa"];
                    number10 = (TorrentNumber) dictionary2["delay"];
                    GenerateVerbose = number12.Value > 0L;
                    UseWSAConfig = number11.Value > 0L;
                    DelayMessages = number10.Value > 0L;
                }
                else
                {
                    GenerateVerbose = true;
                    UseWSAConfig = false;
                    DelayMessages = true;
                }
                if (dictionary.Contains("blankblacklist"))
                {
                    TorrentNumber number13 = new TorrentNumber();
                    if (number13.Value == -1L)
                    {
                        BlackListedFiles = new ArrayList();
                    }
                }
                if (dictionary.Contains("blacklist"))
                {
                    TorrentList list = new TorrentList();
                    list = (TorrentList) dictionary["blacklist"];
                    BlackListedFiles = list.Value;
                }
                if (dictionary.Contains("multitracker"))
                {
                    MultiTrackerGenerator.MultiTrackerTiers = (TorrentList) dictionary["multitracker"];
                    this.Multitracker.UpdateInput();
                }
                if (dictionary.Contains("usemultitracker"))
                {
                    TorrentNumber number14 = new TorrentNumber();
                    number14 = (TorrentNumber) dictionary["usemultitracker"];
                    this.MultiTrackerEnabled.Checked = number14.Value > 0L;
                }
                number = (TorrentNumber) dictionary["autopiece"];
                this.AutomaticPieceSize.Checked = number.Value > 0L;
                this.AnnounceURL.Text = str5.Value;
                this.PieceSize.Text = Conversions.ToString(number2.Value);
            }
            this.AutomaticPieceSize_CheckedChanged(RuntimeHelpers.GetObjectValue(sender), e);
            GC.Collect();
        }

        internal virtual Button AdvSet
        {
            get
            {
                return this._AdvSet;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._AdvSet != null)
                {
                    this._AdvSet.Click -= new EventHandler(this.AdvSet_Click);
                }
                this._AdvSet = value;
                if (this._AdvSet != null)
                {
                    this._AdvSet.Click += new EventHandler(this.AdvSet_Click);
                }
            }
        }

        internal virtual TextBox AnnounceURL
        {
            get
            {
                return this._AnnounceURL;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._AnnounceURL = value;
            }
        }

        internal virtual CheckBox AutomaticPieceSize
        {
            get
            {
                return this._AutomaticPieceSize;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._AutomaticPieceSize != null)
                {
                    this._AutomaticPieceSize.CheckedChanged -= new EventHandler(this.AutomaticPieceSize_CheckedChanged);
                }
                this._AutomaticPieceSize = value;
                if (this._AutomaticPieceSize != null)
                {
                    this._AutomaticPieceSize.CheckedChanged += new EventHandler(this.AutomaticPieceSize_CheckedChanged);
                }
            }
        }

        internal virtual Button BlacklistingScreen
        {
            get
            {
                return this._BlacklistingScreen;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._BlacklistingScreen != null)
                {
                    this._BlacklistingScreen.Click -= new EventHandler(this.BlacklistingScreen_Click);
                }
                this._BlacklistingScreen = value;
                if (this._BlacklistingScreen != null)
                {
                    this._BlacklistingScreen.Click += new EventHandler(this.BlacklistingScreen_Click);
                }
            }
        }

        internal virtual OpenFileDialog BrowseForFile
        {
            get
            {
                return this._BrowseForFile;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._BrowseForFile != null)
                {
                    this._BrowseForFile.FileOk -= new CancelEventHandler(this.BrowseForFile_FileOk);
                }
                this._BrowseForFile = value;
                if (this._BrowseForFile != null)
                {
                    this._BrowseForFile.FileOk += new CancelEventHandler(this.BrowseForFile_FileOk);
                }
            }
        }

        internal virtual FolderBrowserDialog BrowseForFolder
        {
            get
            {
                return this._BrowseForFolder;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._BrowseForFolder = value;
            }
        }

        internal virtual Button BuildTorrentNow
        {
            get
            {
                return this._BuildTorrentNow;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._BuildTorrentNow != null)
                {
                    this._BuildTorrentNow.Click -= new EventHandler(this.BuildTorrentNow_Click);
                }
                this._BuildTorrentNow = value;
                if (this._BuildTorrentNow != null)
                {
                    this._BuildTorrentNow.Click += new EventHandler(this.BuildTorrentNow_Click);
                }
            }
        }

        internal virtual Button ExitWithoutSave
        {
            get
            {
                return this._ExitWithoutSave;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._ExitWithoutSave != null)
                {
                    this._ExitWithoutSave.Click -= new EventHandler(this.ExitWithoutSave_Click);
                }
                this._ExitWithoutSave = value;
                if (this._ExitWithoutSave != null)
                {
                    this._ExitWithoutSave.Click += new EventHandler(this.ExitWithoutSave_Click);
                }
            }
        }

        internal virtual Button ExitWithSave
        {
            get
            {
                return this._ExitWithSave;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._ExitWithSave != null)
                {
                    this._ExitWithSave.Click -= new EventHandler(this.ExitWithSave_Click);
                }
                this._ExitWithSave = value;
                if (this._ExitWithSave != null)
                {
                    this._ExitWithSave.Click += new EventHandler(this.ExitWithSave_Click);
                }
            }
        }

        internal virtual TextBox FileNameToMake
        {
            get
            {
                return this._FileNameToMake;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._FileNameToMake != null)
                {
                    this._FileNameToMake.DragDrop -= new DragEventHandler(this.FileNameToMake_DragDrop);
                }
                this._FileNameToMake = value;
                if (this._FileNameToMake != null)
                {
                    this._FileNameToMake.DragDrop += new DragEventHandler(this.FileNameToMake_DragDrop);
                }
            }
        }

        internal virtual ProgressBar HashProgress
        {
            get
            {
                return this._HashProgress;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._HashProgress = value;
            }
        }

        internal virtual CheckBox IncludeBlacklisted
        {
            get
            {
                return this._IncludeBlacklisted;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._IncludeBlacklisted = value;
            }
        }

        internal virtual CheckBox IncludeCRC32
        {
            get
            {
                return this._IncludeCRC32;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._IncludeCRC32 = value;
            }
        }

        internal virtual CheckBox IncludeED2K
        {
            get
            {
                return this._IncludeED2K;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._IncludeED2K = value;
            }
        }

        internal virtual CheckBox IncludeMD5
        {
            get
            {
                return this._IncludeMD5;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._IncludeMD5 = value;
            }
        }

        internal virtual CheckBox IncludeSHA1
        {
            get
            {
                return this._IncludeSHA1;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._IncludeSHA1 = value;
            }
        }

        internal virtual CheckBox IncludeTiger
        {
            get
            {
                return this._IncludeTiger;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._IncludeTiger = value;
            }
        }

        internal virtual CheckBox IncludeTorrents
        {
            get
            {
                return this._IncludeTorrents;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._IncludeTorrents = value;
            }
        }

        internal virtual Label Label1
        {
            get
            {
                return this._Label1;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._Label1 = value;
            }
        }

        internal virtual Label Label2
        {
            get
            {
                return this._Label2;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._Label2 = value;
            }
        }

        internal virtual Label Label3
        {
            get
            {
                return this._Label3;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._Label3 = value;
            }
        }

        internal virtual Label Label4
        {
            get
            {
                return this._Label4;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._Label4 = value;
            }
        }

        internal virtual Label Label5
        {
            get
            {
                return this._Label5;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._Label5 = value;
            }
        }

        internal virtual Label Label6
        {
            get
            {
                return this._Label6;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._Label6 = value;
            }
        }

        internal virtual Label Label7
        {
            get
            {
                return this._Label7;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._Label7 = value;
            }
        }

        internal virtual Label Label8
        {
            get
            {
                return this._Label8;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._Label8 = value;
            }
        }

        internal virtual CheckBox MakeExternals
        {
            get
            {
                return this._MakeExternals;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._MakeExternals = value;
            }
        }

        internal virtual CheckBox MakeSeparateTorrents
        {
            get
            {
                return this._MakeSeparateTorrents;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._MakeSeparateTorrents = value;
            }
        }

        internal virtual CheckBox MultiTrackerEnabled
        {
            get
            {
                return this._MultiTrackerEnabled;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._MultiTrackerEnabled = value;
            }
        }

        internal virtual Button MultiTrackerSettings
        {
            get
            {
                return this._MultiTrackerSettings;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._MultiTrackerSettings != null)
                {
                    this._MultiTrackerSettings.Click -= new EventHandler(this.MultiTrackerSettings_Click);
                }
                this._MultiTrackerSettings = value;
                if (this._MultiTrackerSettings != null)
                {
                    this._MultiTrackerSettings.Click += new EventHandler(this.MultiTrackerSettings_Click);
                }
            }
        }

        internal virtual ProgressBar OptionalHashProgress
        {
            get
            {
                return this._OptionalHashProgress;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._OptionalHashProgress = value;
            }
        }

        internal virtual ComboBox PieceSize
        {
            get
            {
                return this._PieceSize;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._PieceSize != null)
                {
                    this._PieceSize.LostFocus -= new EventHandler(this.PieceSize_LostFocus);
                }
                this._PieceSize = value;
                if (this._PieceSize != null)
                {
                    this._PieceSize.LostFocus += new EventHandler(this.PieceSize_LostFocus);
                }
            }
        }

        internal virtual CheckBox PrivateTorrent
        {
            get
            {
                return this._PrivateTorrent;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._PrivateTorrent = value;
            }
        }

        internal virtual Button SaveSettings
        {
            get
            {
                return this._SaveSettings;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._SaveSettings != null)
                {
                    this._SaveSettings.Click -= new EventHandler(this.SaveSettings_Click);
                }
                this._SaveSettings = value;
                if (this._SaveSettings != null)
                {
                    this._SaveSettings.Click += new EventHandler(this.SaveSettings_Click);
                }
            }
        }

        internal virtual Button SelectFile
        {
            get
            {
                return this._SelectFile;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._SelectFile != null)
                {
                    this._SelectFile.Click -= new EventHandler(this.SelectFile_Click);
                }
                this._SelectFile = value;
                if (this._SelectFile != null)
                {
                    this._SelectFile.Click += new EventHandler(this.SelectFile_Click);
                }
            }
        }

        internal virtual Button SelectFolder
        {
            get
            {
                return this._SelectFolder;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._SelectFolder != null)
                {
                    this._SelectFolder.Click -= new EventHandler(this.SelectFolder_Click);
                }
                this._SelectFolder = value;
                if (this._SelectFolder != null)
                {
                    this._SelectFolder.Click += new EventHandler(this.SelectFolder_Click);
                }
            }
        }

        internal virtual TextBox TorrentComment
        {
            get
            {
                return this._TorrentComment;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._TorrentComment = value;
            }
        }

        internal virtual ProgressBar TorrentProgress
        {
            get
            {
                return this._TorrentProgress;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._TorrentProgress = value;
            }
        }
    }
}

