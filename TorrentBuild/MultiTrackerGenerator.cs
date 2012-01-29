namespace TorrentBuild
{
    using EAD.Torrent;
    using Microsoft.VisualBasic;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class MultiTrackerGenerator : Form
    {
        [AccessedThroughProperty("BitTornadoGUI")]
        private TextBox _BitTornadoGUI;
        [AccessedThroughProperty("BTCMDSelect")]
        private RadioButton _BTCMDSelect;
        [AccessedThroughProperty("BTGuiSelect")]
        private RadioButton _BTGuiSelect;
        [AccessedThroughProperty("ReturnWithoutSave")]
        private Button _ReturnWithoutSave;
        [AccessedThroughProperty("ReturnWithSave")]
        private Button _ReturnWithSave;
        [AccessedThroughProperty("TornadoCommandLine")]
        private TextBox _TornadoCommandLine;
        private IContainer components;
        public static TorrentList MultiTrackerTiers = new TorrentList();

        public MultiTrackerGenerator()
        {
            base.Load += new EventHandler(this.MultiTrackerGenerator_Load);
            this.InitializeComponent();
        }

        private void BTCMDSelect_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeSelected();
        }

        private void BTGuiSelect_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeSelected();
        }

        private void ChangeSelected()
        {
            if (this.BTCMDSelect.Checked)
            {
                this.TornadoCommandLine.Enabled = true;
                this.BitTornadoGUI.Enabled = false;
                this.BTGuiSelect.Checked = false;
            }
            if (this.BTGuiSelect.Checked)
            {
                this.TornadoCommandLine.Enabled = false;
                this.BitTornadoGUI.Enabled = true;
                this.BTCMDSelect.Checked = false;
            }
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
            this.BTCMDSelect = new RadioButton();
            this.TornadoCommandLine = new TextBox();
            this.BTGuiSelect = new RadioButton();
            this.BitTornadoGUI = new TextBox();
            this.ReturnWithoutSave = new Button();
            this.ReturnWithSave = new Button();
            this.SuspendLayout();
            Point point = new Point(0, 0x18);
            this.BTCMDSelect.Location = point;
            this.BTCMDSelect.Name = "BTCMDSelect";
            Size size = new Size(0x2b0, 0x10);
            this.BTCMDSelect.Size = size;
            this.BTCMDSelect.TabIndex = 0;
            this.BTCMDSelect.Text = "BitTornado Command Line Announce-List Paramater. Use Commas to separate trackers within a Tier and Pipes \"|\" to separate tiers.";
            point = new Point(0, 40);
            this.TornadoCommandLine.Location = point;
            this.TornadoCommandLine.Name = "TornadoCommandLine";
            size = new Size(680, 20);
            this.TornadoCommandLine.Size = size;
            this.TornadoCommandLine.TabIndex = 1;
            this.TornadoCommandLine.Text = "";
            point = new Point(0, 0x40);
            this.BTGuiSelect.Location = point;
            this.BTGuiSelect.Name = "BTGuiSelect";
            size = new Size(680, 0x10);
            this.BTGuiSelect.Size = size;
            this.BTGuiSelect.TabIndex = 2;
            this.BTGuiSelect.Text = "BitTornado GUI Torrent Maker Announce-List Option. One tier per line. Separate trackers within a tier with spaces.";
            point = new Point(0, 80);
            this.BitTornadoGUI.Location = point;
            this.BitTornadoGUI.Multiline = true;
            this.BitTornadoGUI.Name = "BitTornadoGUI";
            size = new Size(680, 0x58);
            this.BitTornadoGUI.Size = size;
            this.BitTornadoGUI.TabIndex = 3;
            this.BitTornadoGUI.Text = "";
            point = new Point(520, 0xa8);
            this.ReturnWithoutSave.Location = point;
            this.ReturnWithoutSave.Name = "ReturnWithoutSave";
            size = new Size(160, 0x18);
            this.ReturnWithoutSave.Size = size;
            this.ReturnWithoutSave.TabIndex = 4;
            this.ReturnWithoutSave.Text = "Return Without Changing";
            point = new Point(520, 0xc0);
            this.ReturnWithSave.Location = point;
            this.ReturnWithSave.Name = "ReturnWithSave";
            size = new Size(160, 0x18);
            this.ReturnWithSave.Size = size;
            this.ReturnWithSave.TabIndex = 4;
            this.ReturnWithSave.Text = "Return Saving Changes";
            size = new Size(5, 13);
            this.AutoScaleBaseSize = size;
            size = new Size(680, 0xdd);
            this.ClientSize = size;
            this.Controls.Add(this.ReturnWithoutSave);
            this.Controls.Add(this.BitTornadoGUI);
            this.Controls.Add(this.BTGuiSelect);
            this.Controls.Add(this.TornadoCommandLine);
            this.Controls.Add(this.BTCMDSelect);
            this.Controls.Add(this.ReturnWithSave);
            this.Name = "MultiTrackerGenerator";
            this.Text = "MultiTrackerGenerator";
            this.ResumeLayout(false);
        }

        private void MultiTrackerGenerator_Load(object sender, EventArgs e)
        {
            this.BTGuiSelect.Checked = true;
            this.ChangeSelected();
        }

        private void ReturnWithoutSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReturnWithSave_Click(object sender, EventArgs e)
        {
            string[] strArray;
            if (this.BTCMDSelect.Checked)
            {
                strArray = Strings.Split(this.TornadoCommandLine.Text, "|", -1, CompareMethod.Binary);
                ArrayList list = new ArrayList();
                foreach (string str in strArray)
                {
                    TorrentList list3 = new TorrentList();
                    ArrayList list2 = new ArrayList();
                    foreach (string str2 in Strings.Split(Strings.Trim(str), ",", -1, CompareMethod.Binary))
                    {
                        if (Strings.Trim(str2) != "")
                        {
                            TorrentString str3 = new TorrentString {
                                Value = Strings.Trim(str2)
                            };
                            list2.Add(str3);
                        }
                    }
                    if (list2.Count > 0)
                    {
                        list3.Value = list2;
                        list.Add(list3);
                    }
                }
                MultiTrackerTiers.Value = list;
            }
            else if (this.BTGuiSelect.Checked)
            {
                strArray = Strings.Split(this.BitTornadoGUI.Text, "\n", -1, CompareMethod.Binary);
                ArrayList list4 = new ArrayList();
                foreach (string str4 in strArray)
                {
                    string mystr4 = "";
                    if (Strings.Right(str4, 1) == "\r")
                    {
                        mystr4 = Strings.Left(str4, Strings.Len(str4) - 1);
                    }
                    mystr4 = Strings.Trim(mystr4);
                    TorrentList list6 = new TorrentList();
                    ArrayList list5 = new ArrayList();
                    foreach (string str5 in Strings.Split(mystr4, " ", -1, CompareMethod.Binary))
                    {
                        if (Strings.Trim(str5) != "")
                        {
                            TorrentString str6 = new TorrentString {
                                Value = Strings.Trim(str5)
                            };
                            list5.Add(str6);
                        }
                    }
                    if (list5.Count > 0)
                    {
                        list6.Value = list5;
                        list4.Add(list6);
                    }
                }
                MultiTrackerTiers.Value = list4;
            }
            this.Close();
        }

        public void UpdateInput()
        {
            string str = "";
            IEnumerator enumerator = null;
            try
            {
                enumerator = MultiTrackerTiers.Value.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string str2 = "";
                    IEnumerator enumerator2 = null;
                    TorrentList current = (TorrentList) enumerator.Current;
                    try
                    {
                        enumerator2 = current.Value.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            TorrentString str3 = (TorrentString) enumerator2.Current;
                            str2 = str2 + " " + str3.Value;
                        }
                    }
                    finally
                    {
                        if (enumerator2 is IDisposable)
                        {
                            (enumerator2 as IDisposable).Dispose();
                        }
                    }
                    if (str != "")
                    {
                        str = str + "\r\n" + Strings.Trim(str2);
                    }
                    else
                    {
                        str = Strings.Trim(str2);
                    }
                    str2 = "";
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
            this.BitTornadoGUI.Text = str;
            this.BTCMDSelect.Checked = false;
            this.BTGuiSelect.Checked = true;
            this.ChangeSelected();
        }

        internal virtual TextBox BitTornadoGUI
        {
            get
            {
                return this._BitTornadoGUI;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._BitTornadoGUI = value;
            }
        }

        internal virtual RadioButton BTCMDSelect
        {
            get
            {
                return this._BTCMDSelect;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._BTCMDSelect != null)
                {
                    this._BTCMDSelect.CheckedChanged -= new EventHandler(this.BTCMDSelect_CheckedChanged);
                }
                this._BTCMDSelect = value;
                if (this._BTCMDSelect != null)
                {
                    this._BTCMDSelect.CheckedChanged += new EventHandler(this.BTCMDSelect_CheckedChanged);
                }
            }
        }

        internal virtual RadioButton BTGuiSelect
        {
            get
            {
                return this._BTGuiSelect;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._BTGuiSelect != null)
                {
                    this._BTGuiSelect.CheckedChanged -= new EventHandler(this.BTGuiSelect_CheckedChanged);
                }
                this._BTGuiSelect = value;
                if (this._BTGuiSelect != null)
                {
                    this._BTGuiSelect.CheckedChanged += new EventHandler(this.BTGuiSelect_CheckedChanged);
                }
            }
        }

        internal virtual Button ReturnWithoutSave
        {
            get
            {
                return this._ReturnWithoutSave;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._ReturnWithoutSave != null)
                {
                    this._ReturnWithoutSave.Click -= new EventHandler(this.ReturnWithoutSave_Click);
                }
                this._ReturnWithoutSave = value;
                if (this._ReturnWithoutSave != null)
                {
                    this._ReturnWithoutSave.Click += new EventHandler(this.ReturnWithoutSave_Click);
                }
            }
        }

        internal virtual Button ReturnWithSave
        {
            get
            {
                return this._ReturnWithSave;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (this._ReturnWithSave != null)
                {
                    this._ReturnWithSave.Click -= new EventHandler(this.ReturnWithSave_Click);
                }
                this._ReturnWithSave = value;
                if (this._ReturnWithSave != null)
                {
                    this._ReturnWithSave.Click += new EventHandler(this.ReturnWithSave_Click);
                }
            }
        }

        internal virtual TextBox TornadoCommandLine
        {
            get
            {
                return this._TornadoCommandLine;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                this._TornadoCommandLine = value;
            }
        }
    }
}

