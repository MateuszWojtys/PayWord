namespace User
{
    partial class Certificate
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
            this.labelBank = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelBank
            // 
            this.labelBank.AutoSize = true;
            this.labelBank.Location = new System.Drawing.Point(12, 19);
            this.labelBank.Name = "labelBank";
            this.labelBank.Size = new System.Drawing.Size(76, 13);
            this.labelBank.TabIndex = 0;
            this.labelBank.Text = "Nazwa banku:";
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(12, 43);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(77, 13);
            this.labelUser.TabIndex = 1;
            this.labelUser.Text = "Nazwa klienta:";
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(12, 67);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(80, 13);
            this.labelDate.TabIndex = 2;
            this.labelDate.Text = "Data ważności:";
            // 
            // Certificate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 105);
            this.Controls.Add(this.labelDate);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.labelBank);
            this.Name = "Certificate";
            this.Text = "Certificate";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBank;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelDate;
    }
}