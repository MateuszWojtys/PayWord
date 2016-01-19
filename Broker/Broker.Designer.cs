namespace Broker
{
    partial class Broker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.linkLabelDataUsers = new System.Windows.Forms.LinkLabel();
            this.linkLabelReports = new System.Windows.Forms.LinkLabel();
            this.linkLabelRozliczenie = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // linkLabelDataUsers
            // 
            this.linkLabelDataUsers.AutoSize = true;
            this.linkLabelDataUsers.Location = new System.Drawing.Point(12, 9);
            this.linkLabelDataUsers.Name = "linkLabelDataUsers";
            this.linkLabelDataUsers.Size = new System.Drawing.Size(131, 13);
            this.linkLabelDataUsers.TabIndex = 0;
            this.linkLabelDataUsers.TabStop = true;
            this.linkLabelDataUsers.Text = "Wyświetl dane o klientach";
            this.linkLabelDataUsers.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDataUsers_LinkClicked);
            // 
            // linkLabelReports
            // 
            this.linkLabelReports.AutoSize = true;
            this.linkLabelReports.Location = new System.Drawing.Point(12, 35);
            this.linkLabelReports.Name = "linkLabelReports";
            this.linkLabelReports.Size = new System.Drawing.Size(169, 13);
            this.linkLabelReports.TabIndex = 1;
            this.linkLabelReports.TabStop = true;
            this.linkLabelReports.Text = "Wyświetl raporty od Sprzedawców";
            this.linkLabelReports.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelReports_LinkClicked);
            // 
            // linkLabelRozliczenie
            // 
            this.linkLabelRozliczenie.AutoSize = true;
            this.linkLabelRozliczenie.Location = new System.Drawing.Point(12, 62);
            this.linkLabelRozliczenie.Name = "linkLabelRozliczenie";
            this.linkLabelRozliczenie.Size = new System.Drawing.Size(61, 13);
            this.linkLabelRozliczenie.TabIndex = 2;
            this.linkLabelRozliczenie.TabStop = true;
            this.linkLabelRozliczenie.Text = "Rozliczenie";
            this.linkLabelRozliczenie.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelRozliczenie_LinkClicked);
            // 
            // Broker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 208);
            this.Controls.Add(this.linkLabelRozliczenie);
            this.Controls.Add(this.linkLabelReports);
            this.Controls.Add(this.linkLabelDataUsers);
            this.Name = "Broker";
            this.Text = "Broker";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabelDataUsers;
        private System.Windows.Forms.LinkLabel linkLabelReports;
        private System.Windows.Forms.LinkLabel linkLabelRozliczenie;
    }
}

