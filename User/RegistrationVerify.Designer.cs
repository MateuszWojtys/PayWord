namespace User
{
    partial class RegistrationVerify
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
            this.labelRegistrationVerify = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelRegistrationVerify
            // 
            this.labelRegistrationVerify.AutoSize = true;
            this.labelRegistrationVerify.Location = new System.Drawing.Point(26, 22);
            this.labelRegistrationVerify.Name = "labelRegistrationVerify";
            this.labelRegistrationVerify.Size = new System.Drawing.Size(165, 13);
            this.labelRegistrationVerify.TabIndex = 0;
            this.labelRegistrationVerify.Text = "Rejestracja przebiegła pomyślnie!";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(60, 46);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // RegistrationVerify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 81);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelRegistrationVerify);
            this.Name = "RegistrationVerify";
            this.Text = "RegistrationVerify";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelRegistrationVerify;
        private System.Windows.Forms.Button buttonOK;

    }
}