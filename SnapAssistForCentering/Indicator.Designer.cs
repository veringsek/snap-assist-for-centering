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
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
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
            this.btnExit});
            this.mnuSystemTrayIcon.Name = "mnuSystemTrayIcon";
            this.mnuSystemTrayIcon.Size = new System.Drawing.Size(181, 70);
            // 
            // chkRunAtStartup
            // 
            this.chkRunAtStartup.Name = "chkRunAtStartup";
            this.chkRunAtStartup.Size = new System.Drawing.Size(180, 22);
            this.chkRunAtStartup.Text = "Run at Startup";
            this.chkRunAtStartup.Click += new System.EventHandler(this.chkRunAtStartup_Click);
            // 
            // btnExit
            // 
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(180, 22);
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
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
    }
}