namespace SnapAssistForCentering
{
    partial class Indicator
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Indicator));
            this.tmrCursor = new System.Windows.Forms.Timer(this.components);
            this.SystemTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mnuSystemTrayIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.chkRunAtStartup = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRestartNow = new System.Windows.Forms.ToolStripMenuItem();
            this.chkRestartEvery5Mins = new System.Windows.Forms.ToolStripMenuItem();
            this.chkRestartEveryHours = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrRestart = new System.Windows.Forms.Timer(this.components);
            this.chkRestartEvery15Mins = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSystemTrayIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrCursor
            // 
            this.tmrCursor.Tick += new System.EventHandler(this.tmrCursor_Tick);
            // 
            // SystemTrayIcon
            // 
            this.SystemTrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.SystemTrayIcon.ContextMenuStrip = this.mnuSystemTrayIcon;
            this.SystemTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("SystemTrayIcon.Icon")));
            this.SystemTrayIcon.Visible = true;
            // 
            // mnuSystemTrayIcon
            // 
            this.mnuSystemTrayIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkRunAtStartup,
            this.btnRestart,
            this.btnExit});
            this.mnuSystemTrayIcon.Name = "mnuSystemTrayIcon";
            this.mnuSystemTrayIcon.Size = new System.Drawing.Size(181, 92);
            // 
            // chkRunAtStartup
            // 
            this.chkRunAtStartup.Name = "chkRunAtStartup";
            this.chkRunAtStartup.Size = new System.Drawing.Size(180, 22);
            this.chkRunAtStartup.Text = "Run at Startup";
            this.chkRunAtStartup.Click += new System.EventHandler(this.chkRunAtStartup_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRestartNow,
            this.chkRestartEvery5Mins,
            this.chkRestartEvery15Mins,
            this.chkRestartEveryHours});
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(180, 22);
            this.btnRestart.Text = "Restart";
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnRestartNow
            // 
            this.btnRestartNow.Name = "btnRestartNow";
            this.btnRestartNow.Size = new System.Drawing.Size(180, 22);
            this.btnRestartNow.Text = "Now";
            this.btnRestartNow.Click += new System.EventHandler(this.btnRestartNow_Click);
            // 
            // chkRestartEvery5Mins
            // 
            this.chkRestartEvery5Mins.Name = "chkRestartEvery5Mins";
            this.chkRestartEvery5Mins.Size = new System.Drawing.Size(180, 22);
            this.chkRestartEvery5Mins.Text = "Every 5 mins";
            this.chkRestartEvery5Mins.Click += new System.EventHandler(this.chkRestartEvery5Mins_Click);
            // 
            // chkRestartEveryHours
            // 
            this.chkRestartEveryHours.Name = "chkRestartEveryHours";
            this.chkRestartEveryHours.Size = new System.Drawing.Size(180, 22);
            this.chkRestartEveryHours.Text = "Every hours";
            this.chkRestartEveryHours.Click += new System.EventHandler(this.chkRestartEveryHours_Click);
            // 
            // btnExit
            // 
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(180, 22);
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // tmrRestart
            // 
            this.tmrRestart.Tick += new System.EventHandler(this.tmrRestart_Tick);
            // 
            // chkRestartEvery15Mins
            // 
            this.chkRestartEvery15Mins.Name = "chkRestartEvery15Mins";
            this.chkRestartEvery15Mins.Size = new System.Drawing.Size(180, 22);
            this.chkRestartEvery15Mins.Text = "Every 15 mins";
            this.chkRestartEvery15Mins.Click += new System.EventHandler(this.chkRestartEvery15Mins_Click);
            // 
            // Indicator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(96, 96);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Indicator";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Indicator";
            this.Load += new System.EventHandler(this.Indicator_Load);
            this.Shown += new System.EventHandler(this.Indicator_Shown);
            this.mnuSystemTrayIcon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrCursor;
        private System.Windows.Forms.NotifyIcon SystemTrayIcon;
        private ContextMenuStrip mnuSystemTrayIcon;
        private ToolStripMenuItem chkRunAtStartup;
        private ToolStripMenuItem btnExit;
        private ToolStripMenuItem btnRestart;
        private ToolStripMenuItem btnRestartNow;
        private ToolStripMenuItem chkRestartEvery5Mins;
        private ToolStripMenuItem chkRestartEveryHours;
        private System.Windows.Forms.Timer tmrRestart;
        private ToolStripMenuItem chkRestartEvery15Mins;
    }
}