namespace TorrentBuild
{
    using EAD.Torrent;
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ChangeBlacklistedFiles : Form
    {
        [AccessedThroughProperty("AddtoList")]
        private Button _AddtoList;
        [AccessedThroughProperty("Blacklisted")]
        private ListBox _Blacklisted;
        [AccessedThroughProperty("CloseThis")]
        private Button _CloseThis;
        [AccessedThroughProperty("FileName")]
        private TextBox _FileName;
        [AccessedThroughProperty("Label1")]
        private Label _Label1;
        [AccessedThroughProperty("Label2")]
        private Label _Label2;
        [AccessedThroughProperty("Label3")]
        private Label _Label3;
        [AccessedThroughProperty("RemoveFromList")]
        private Button _RemoveFromList;
        [AccessedThroughProperty("ReplaceAbove")]
        private Button _ReplaceAbove;
        [AccessedThroughProperty("SaveToMain")]
        private Button _SaveToMain;
        [AccessedThroughProperty("SaveToMainWithClose")]
        private Button _SaveToMainWithClose;
        private IContainer components;

        public ChangeBlacklistedFiles()
        {
            base.Load += new EventHandler(this.ChangeBlacklistedFiles_Load);
            this.InitializeComponent();
        }

        private void AddtoList_Click(object sender, EventArgs e)
        {
            this.Blacklisted.Items.Add(this.FileName.Text);
            this.FileName.Text = "";
        }

        private void Blacklisted_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FileName.Text = Conversions.ToString(this.Blacklisted.SelectedItem);
        }

        private void ChangeBlacklistedFiles_Load(object sender, EventArgs e)
        {
            IEnumerator enumerator = null;
            try
            {
                enumerator = TorrentBuild.BlackListedFiles.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TorrentString current = (TorrentString) enumerator.Current;
                    this.Blacklisted.Items.Add(current.Value);
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

        private void CloseThis_Click(object sender, EventArgs e)
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
            ResourceManager manager = new ResourceManager(typeof(ChangeBlacklistedFiles));
            this.Blacklisted = new ListBox();
            this.Label1 = new Label();
            this.Label2 = new Label();
            this.FileName = new TextBox();
            this.AddtoList = new Button();
            this.RemoveFromList = new Button();
            this.SaveToMain = new Button();
            this.SaveToMainWithClose = new Button();
            this.CloseThis = new Button();
            this.Label3 = new Label();
            this.ReplaceAbove = new Button();
            this.SuspendLayout();
            Point point = new Point(0, 0x10);
            this.Blacklisted.Location = point;
            this.Blacklisted.Name = "Blacklisted";
            Size size = new Size(0x198, 0x6c);
            this.Blacklisted.Size = size;
            this.Blacklisted.TabIndex = 0;
            point = new Point(0, 0);
            this.Label1.Location = point;
            this.Label1.Name = "Label1";
            size = new Size(0x198, 0x10);
            this.Label1.Size = size;
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Filenames And Extensions that are blacklisted:";
            point = new Point(0, 0x80);
            this.Label2.Location = point;
            this.Label2.Name = "Label2";
            size = new Size(0x198, 0x10);
            this.Label2.Size = size;
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Filename/Extension";
            point = new Point(0, 0x90);
            this.FileName.Location = point;
            this.FileName.Name = "FileName";
            size = new Size(0xe8, 20);
            this.FileName.Size = size;
            this.FileName.TabIndex = 3;
            this.FileName.Text = "";
            point = new Point(240, 0x90);
            this.AddtoList.Location = point;
            this.AddtoList.Name = "AddtoList";
            size = new Size(0x38, 0x18);
            this.AddtoList.Size = size;
            this.AddtoList.TabIndex = 4;
            this.AddtoList.Text = "Add";
            point = new Point(0x160, 0x90);
            this.RemoveFromList.Location = point;
            this.RemoveFromList.Name = "RemoveFromList";
            size = new Size(0x38, 0x18);
            this.RemoveFromList.Size = size;
            this.RemoveFromList.TabIndex = 5;
            this.RemoveFromList.Text = "Remove";
            point = new Point(0x110, 0xa8);
            this.SaveToMain.Location = point;
            this.SaveToMain.Name = "SaveToMain";
            size = new Size(0x88, 0x18);
            this.SaveToMain.Size = size;
            this.SaveToMain.TabIndex = 6;
            this.SaveToMain.Text = "Save to Main";
            point = new Point(0x110, 0xc0);
            this.SaveToMainWithClose.Location = point;
            this.SaveToMainWithClose.Name = "SaveToMainWithClose";
            size = new Size(0x88, 0x18);
            this.SaveToMainWithClose.Size = size;
            this.SaveToMainWithClose.TabIndex = 7;
            this.SaveToMainWithClose.Text = "Save to Main and Close";
            point = new Point(0x110, 0xd8);
            this.CloseThis.Location = point;
            this.CloseThis.Name = "CloseThis";
            size = new Size(0x88, 0x18);
            this.CloseThis.Size = size;
            this.CloseThis.TabIndex = 8;
            this.CloseThis.Text = "Close";
            point = new Point(0, 0xa8);
            this.Label3.Location = point;
            this.Label3.Name = "Label3";
            size = new Size(0x110, 80);
            this.Label3.Size = size;
            this.Label3.TabIndex = 9;
            this.Label3.Text = "When entering in file names, include the full filename. When entering in extensions, only include the extension, do not include any wildcards. Also, files that end in filenames that are listed here will be blocked. Changes are NOT permanent unless you save on the main window.";
            point = new Point(0x128, 0x90);
            this.ReplaceAbove.Location = point;
            this.ReplaceAbove.Name = "ReplaceAbove";
            size = new Size(0x38, 0x18);
            this.ReplaceAbove.Size = size;
            this.ReplaceAbove.TabIndex = 10;
            this.ReplaceAbove.Text = "Replace";
            size = new Size(5, 13);
            this.AutoScaleBaseSize = size;
            size = new Size(410, 0xf7);
            this.ClientSize = size;
            this.Controls.Add(this.ReplaceAbove);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.CloseThis);
            this.Controls.Add(this.SaveToMainWithClose);
            this.Controls.Add(this.SaveToMain);
            this.Controls.Add(this.RemoveFromList);
            this.Controls.Add(this.AddtoList);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Blacklisted);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = (Icon) manager.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeBlacklistedFiles";
            this.Text = "Change Blacklisted Filenames";
            this.ResumeLayout(false);
        }

        private void RemoveFromList_Click(object sender, EventArgs e)
        {
            this.Blacklisted.Items.RemoveAt(this.Blacklisted.SelectedIndex);
        }

        private void ReplaceAbove_Click(object sender, EventArgs e)
        {
            this.Blacklisted.SelectedItem = this.FileName.Text;
        }

        private void SaveToMain_Click(object sender, EventArgs e)
        {
            IEnumerator enumerator = null;
            ArrayList list = new ArrayList();
            try
            {
                enumerator = this.Blacklisted.Items.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string str2 = Conversions.ToString(enumerator.Current);
                    TorrentString str = new TorrentString {
                        Value = str2
                    };
                    list.Add(str);
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
            TorrentBuild.BlackListedFiles = list;
        }

        private void SaveToMainWithClose_Click(object sender, EventArgs e)
        {
            this.SaveToMain_Click(RuntimeHelpers.GetObjectValue(sender), e);
            this.Close();
        }

        internal virtual Button AddtoList
        {
            get
            {
                return this._AddtoList;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._AddtoList != null)
                {
                    this._AddtoList.Click -= new EventHandler(this.AddtoList_Click);
                }
                this._AddtoList = value;
                if (this._AddtoList != null)
                {
                    this._AddtoList.Click += new EventHandler(this.AddtoList_Click);
                }
            }
        }

        internal virtual ListBox Blacklisted
        {
            get
            {
                return this._Blacklisted;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._Blacklisted != null)
                {
                    this._Blacklisted.SelectedIndexChanged -= new EventHandler(this.Blacklisted_SelectedIndexChanged);
                }
                this._Blacklisted = value;
                if (this._Blacklisted != null)
                {
                    this._Blacklisted.SelectedIndexChanged += new EventHandler(this.Blacklisted_SelectedIndexChanged);
                }
            }
        }

        internal virtual Button CloseThis
        {
            get
            {
                return this._CloseThis;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._CloseThis != null)
                {
                    this._CloseThis.Click -= new EventHandler(this.CloseThis_Click);
                }
                this._CloseThis = value;
                if (this._CloseThis != null)
                {
                    this._CloseThis.Click += new EventHandler(this.CloseThis_Click);
                }
            }
        }

        internal virtual TextBox FileName
        {
            get
            {
                return this._FileName;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._FileName = value;
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

        internal virtual Button RemoveFromList
        {
            get
            {
                return this._RemoveFromList;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._RemoveFromList != null)
                {
                    this._RemoveFromList.Click -= new EventHandler(this.RemoveFromList_Click);
                }
                this._RemoveFromList = value;
                if (this._RemoveFromList != null)
                {
                    this._RemoveFromList.Click += new EventHandler(this.RemoveFromList_Click);
                }
            }
        }

        internal virtual Button ReplaceAbove
        {
            get
            {
                return this._ReplaceAbove;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._ReplaceAbove != null)
                {
                    this._ReplaceAbove.Click -= new EventHandler(this.ReplaceAbove_Click);
                }
                this._ReplaceAbove = value;
                if (this._ReplaceAbove != null)
                {
                    this._ReplaceAbove.Click += new EventHandler(this.ReplaceAbove_Click);
                }
            }
        }

        internal virtual Button SaveToMain
        {
            get
            {
                return this._SaveToMain;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._SaveToMain != null)
                {
                    this._SaveToMain.Click -= new EventHandler(this.SaveToMain_Click);
                }
                this._SaveToMain = value;
                if (this._SaveToMain != null)
                {
                    this._SaveToMain.Click += new EventHandler(this.SaveToMain_Click);
                }
            }
        }

        internal virtual Button SaveToMainWithClose
        {
            get
            {
                return this._SaveToMainWithClose;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._SaveToMainWithClose != null)
                {
                    this._SaveToMainWithClose.Click -= new EventHandler(this.SaveToMainWithClose_Click);
                }
                this._SaveToMainWithClose = value;
                if (this._SaveToMainWithClose != null)
                {
                    this._SaveToMainWithClose.Click += new EventHandler(this.SaveToMainWithClose_Click);
                }
            }
        }
    }
}

