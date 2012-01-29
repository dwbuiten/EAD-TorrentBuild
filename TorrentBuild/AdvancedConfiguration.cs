namespace TorrentBuild
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class AdvancedConfiguration : Form
    {
        [AccessedThroughProperty("ApplyChg")]
        private Button _ApplyChg;
        [AccessedThroughProperty("ApplyWithclose")]
        private Button _ApplyWithclose;
        [AccessedThroughProperty("closeonly")]
        private Button _closeonly;
        [AccessedThroughProperty("EnableDelay")]
        private CheckBox _EnableDelay;
        [AccessedThroughProperty("GroupBox1")]
        private GroupBox _GroupBox1;
        [AccessedThroughProperty("VerboseGen")]
        private CheckBox _VerboseGen;
        [AccessedThroughProperty("WSAUse")]
        private CheckBox _WSAUse;
        private IContainer components;

        public AdvancedConfiguration()
        {
            base.Load += new EventHandler(this.AdvancedConfig_Load);
            this.InitializeComponent();
        }

        private void AdvancedConfig_Load(object sender, EventArgs e)
        {
            this.VerboseGen.Checked = TorrentBuild.GenerateVerbose;
            this.WSAUse.Checked = TorrentBuild.UseWSAConfig;
            this.EnableDelay.Checked = TorrentBuild.DelayMessages;
        }

        private void ApplyChg_Click(object sender, EventArgs e)
        {
            TorrentBuild.GenerateVerbose = this.VerboseGen.Checked;
            TorrentBuild.UseWSAConfig = this.WSAUse.Checked;
            TorrentBuild.DelayMessages = this.EnableDelay.Checked;
        }

        private void ApplyWithclose_Click(object sender, EventArgs e)
        {
            this.ApplyChg_Click(RuntimeHelpers.GetObjectValue(sender), e);
            this.closeonly_Click(RuntimeHelpers.GetObjectValue(sender), e);
        }

        private void closeonly_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        [DebuggerStepThrough]
        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(AdvancedConfiguration));
            this.VerboseGen = new CheckBox();
            this.GroupBox1 = new GroupBox();
            this.WSAUse = new CheckBox();
            this.closeonly = new Button();
            this.ApplyWithclose = new Button();
            this.ApplyChg = new Button();
            this.EnableDelay = new CheckBox();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            Point point = new Point(8, 0x10);
            this.VerboseGen.Location = point;
            this.VerboseGen.Name = "VerboseGen";
            Size size = new Size(0x218, 0x10);
            this.VerboseGen.Size = size;
            this.VerboseGen.TabIndex = 0;
            this.VerboseGen.Text = "Verbose Torrent Generation (Useful if you want to be informed between files)";
            this.GroupBox1.Controls.Add(this.EnableDelay);
            this.GroupBox1.Controls.Add(this.WSAUse);
            this.GroupBox1.Controls.Add(this.closeonly);
            this.GroupBox1.Controls.Add(this.ApplyWithclose);
            this.GroupBox1.Controls.Add(this.ApplyChg);
            point = new Point(0, 0);
            this.GroupBox1.Location = point;
            this.GroupBox1.Name = "GroupBox1";
            size = new Size(0x228, 200);
            this.GroupBox1.Size = size;
            this.GroupBox1.TabIndex = 1;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Advanced Settings";
            point = new Point(8, 0x20);
            this.WSAUse.Location = point;
            this.WSAUse.Name = "WSAUse";
            size = new Size(0x218, 0x10);
            this.WSAUse.Size = size;
            this.WSAUse.TabIndex = 3;
            this.WSAUse.Text = "Use WebSeed values from wsa.config in torrent generation";
            point = new Point(0x98, 0xa8);
            this.closeonly.Location = point;
            this.closeonly.Name = "closeonly";
            size = new Size(0x90, 0x18);
            this.closeonly.Size = size;
            this.closeonly.TabIndex = 2;
            this.closeonly.Text = "Close";
            point = new Point(8, 0xa8);
            this.ApplyWithclose.Location = point;
            this.ApplyWithclose.Name = "ApplyWithclose";
            size = new Size(0x90, 0x18);
            this.ApplyWithclose.Size = size;
            this.ApplyWithclose.TabIndex = 1;
            this.ApplyWithclose.Text = "Apply Changes and Close";
            point = new Point(8, 0x90);
            this.ApplyChg.Location = point;
            this.ApplyChg.Name = "ApplyChg";
            size = new Size(0x90, 0x18);
            this.ApplyChg.Size = size;
            this.ApplyChg.TabIndex = 0;
            this.ApplyChg.Text = "Apply Changes";
            point = new Point(8, 0x30);
            this.EnableDelay.Location = point;
            this.EnableDelay.Name = "EnableDelay";
            size = new Size(0x218, 0x10);
            this.EnableDelay.Size = size;
            this.EnableDelay.TabIndex = 4;
            this.EnableDelay.Text = "Enable all delay notifications";
            size = new Size(5, 13);
            this.AutoScaleBaseSize = size;
            size = new Size(560, 0xcd);
            this.ClientSize = size;
            this.Controls.Add(this.VerboseGen);
            this.Controls.Add(this.GroupBox1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = (Icon) manager.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancedConfiguration";
            this.Text = "TorrentGen Advanced Configuration";
            this.GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        internal virtual Button ApplyChg
        {
            get
            {
                return this._ApplyChg;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._ApplyChg != null)
                {
                    this._ApplyChg.Click -= new EventHandler(this.ApplyChg_Click);
                }
                this._ApplyChg = value;
                if (this._ApplyChg != null)
                {
                    this._ApplyChg.Click += new EventHandler(this.ApplyChg_Click);
                }
            }
        }

        internal virtual Button ApplyWithclose
        {
            get
            {
                return this._ApplyWithclose;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._ApplyWithclose != null)
                {
                    this._ApplyWithclose.Click -= new EventHandler(this.ApplyWithclose_Click);
                }
                this._ApplyWithclose = value;
                if (this._ApplyWithclose != null)
                {
                    this._ApplyWithclose.Click += new EventHandler(this.ApplyWithclose_Click);
                }
            }
        }

        internal virtual Button closeonly
        {
            get
            {
                return this._closeonly;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._closeonly != null)
                {
                    this._closeonly.Click -= new EventHandler(this.closeonly_Click);
                }
                this._closeonly = value;
                if (this._closeonly != null)
                {
                    this._closeonly.Click += new EventHandler(this.closeonly_Click);
                }
            }
        }

        internal virtual CheckBox EnableDelay
        {
            get
            {
                return this._EnableDelay;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._EnableDelay = value;
            }
        }

        internal virtual GroupBox GroupBox1
        {
            get
            {
                return this._GroupBox1;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._GroupBox1 = value;
            }
        }

        public virtual CheckBox VerboseGen
        {
            get
            {
                return this._VerboseGen;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._VerboseGen = value;
            }
        }

        internal virtual CheckBox WSAUse
        {
            get
            {
                return this._WSAUse;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._WSAUse = value;
            }
        }
    }
}

