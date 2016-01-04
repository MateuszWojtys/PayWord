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
            // Broker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.linkLabelDataUsers);
            this.Name = "Broker";
            this.Text = "Broker";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabelDataUsers;
    }
}

