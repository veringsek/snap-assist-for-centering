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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Indicator));
            tmrCursor = new System.Windows.Forms.Timer(components);
            SystemTrayIcon = new NotifyIcon(components);
            mnuSystemTrayIcon = new ContextMenuStrip(components);
            chkRunAtStartup = new ToolStripMenuItem();
            btnRestart = new ToolStripMenuItem();
            btnRestartNow = new ToolStripMenuItem();
            chkRestartEvery5Mins = new ToolStripMenuItem();
            chkRestartEvery15Mins = new ToolStripMenuItem();
            chkRestartEveryHours = new ToolStripMenuItem();
            btnExit = new ToolStripMenuItem();
            tmrRestart = new System.Windows.Forms.Timer(components);
            mnuSystemTrayIcon.SuspendLayout();
            SuspendLayout();
            // 
            // tmrCursor
            // 
            tmrCursor.Tick += tmrCursor_Tick;
            // 
            // SystemTrayIcon
            // 
            SystemTrayIcon.BalloonTipIcon = ToolTipIcon.Info;
            SystemTrayIcon.ContextMenuStrip = mnuSystemTrayIcon;
            SystemTrayIcon.Icon = (Icon)resources.GetObject("SystemTrayIcon.Icon");
            SystemTrayIcon.Visible = true;
            // 
            // mnuSystemTrayIcon
            // 
            mnuSystemTrayIcon.ImageScalingSize = new Size(32, 32);
            mnuSystemTrayIcon.Items.AddRange(new ToolStripItem[] { chkRunAtStartup, btnRestart, btnExit });
            mnuSystemTrayIcon.Name = "mnuSystemTrayIcon";
            mnuSystemTrayIcon.Size = new Size(301, 156);
            // 
            // chkRunAtStartup
            // 
            chkRunAtStartup.Name = "chkRunAtStartup";
            chkRunAtStartup.Size = new Size(300, 36);
            chkRunAtStartup.Text = "Run at Startup";
            chkRunAtStartup.Click += chkRunAtStartup_Click;
            // 
            // btnRestart
            // 
            btnRestart.DropDownItems.AddRange(new ToolStripItem[] { btnRestartNow, chkRestartEvery5Mins, chkRestartEvery15Mins, chkRestartEveryHours });
            btnRestart.Name = "btnRestart";
            btnRestart.Size = new Size(300, 36);
            btnRestart.Text = "Restart";
            btnRestart.Click += btnRestart_Click;
            // 
            // btnRestartNow
            // 
            btnRestartNow.Name = "btnRestartNow";
            btnRestartNow.Size = new Size(359, 44);
            btnRestartNow.Text = "Now";
            btnRestartNow.Click += btnRestartNow_Click;
            // 
            // chkRestartEvery5Mins
            // 
            chkRestartEvery5Mins.Name = "chkRestartEvery5Mins";
            chkRestartEvery5Mins.Size = new Size(359, 44);
            chkRestartEvery5Mins.Text = "Every 5 mins";
            chkRestartEvery5Mins.Click += chkRestartEvery5Mins_Click;
            // 
            // chkRestartEvery15Mins
            // 
            chkRestartEvery15Mins.Name = "chkRestartEvery15Mins";
            chkRestartEvery15Mins.Size = new Size(359, 44);
            chkRestartEvery15Mins.Text = "Every 15 mins";
            chkRestartEvery15Mins.Click += chkRestartEvery15Mins_Click;
            // 
            // chkRestartEveryHours
            // 
            chkRestartEveryHours.Name = "chkRestartEveryHours";
            chkRestartEveryHours.Size = new Size(359, 44);
            chkRestartEveryHours.Text = "Every hours";
            chkRestartEveryHours.Click += chkRestartEveryHours_Click;
            // 
            // btnExit
            // 
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(300, 36);
            btnExit.Text = "Exit";
            btnExit.Click += btnExit_Click;
            // 
            // tmrRestart
            // 
            tmrRestart.Tick += tmrRestart_Tick;
            // 
            // Indicator
            // 
            AutoScaleDimensions = new SizeF(14F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(192, 192);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(6);
            Name = "Indicator";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Indicator";
            Load += Indicator_Load;
            Shown += Indicator_Shown;
            mnuSystemTrayIcon.ResumeLayout(false);
            ResumeLayout(false);

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