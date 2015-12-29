namespace User
{
    partial class Form1
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
            this.buttonWyslij = new System.Windows.Forms.Button();
            this.labelLogin = new System.Windows.Forms.Label();
            this.labelHasło = new System.Windows.Forms.Label();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.textBoxHasło = new System.Windows.Forms.TextBox();
            this.buttonLogIn = new System.Windows.Forms.Button();
            this.labelLogowanie = new System.Windows.Forms.Label();
            this.linkLabelRejestracja = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // buttonWyslij
            // 
            this.buttonWyslij.Location = new System.Drawing.Point(95, 217);
            this.buttonWyslij.Name = "buttonWyslij";
            this.buttonWyslij.Size = new System.Drawing.Size(75, 23);
            this.buttonWyslij.TabIndex = 0;
            this.buttonWyslij.Text = "Wyślij";
            this.buttonWyslij.UseVisualStyleBackColor = true;
            this.buttonWyslij.Click += new System.EventHandler(this.buttonWyslij_Click);
            // 
            // labelLogin
            // 
            this.labelLogin.AutoSize = true;
            this.labelLogin.Location = new System.Drawing.Point(24, 47);
            this.labelLogin.Name = "labelLogin";
            this.labelLogin.Size = new System.Drawing.Size(33, 13);
            this.labelLogin.TabIndex = 1;
            this.labelLogin.Text = "Login";
            // 
            // labelHasło
            // 
            this.labelHasło.AutoSize = true;
            this.labelHasło.Location = new System.Drawing.Point(24, 78);
            this.labelHasło.Name = "labelHasło";
            this.labelHasło.Size = new System.Drawing.Size(36, 13);
            this.labelHasło.TabIndex = 2;
            this.labelHasło.Text = "Hasło";
            // 
            // textBoxLogin
            // 
            this.textBoxLogin.Location = new System.Drawing.Point(70, 47);
            this.textBoxLogin.Name = "textBoxLogin";
            this.textBoxLogin.Size = new System.Drawing.Size(143, 20);
            this.textBoxLogin.TabIndex = 3;
            // 
            // textBoxHasło
            // 
            this.textBoxHasło.Location = new System.Drawing.Point(70, 75);
            this.textBoxHasło.Name = "textBoxHasło";
            this.textBoxHasło.Size = new System.Drawing.Size(143, 20);
            this.textBoxHasło.TabIndex = 4;
            this.textBoxHasło.TextChanged += new System.EventHandler(this.textBoxHasło_TextChanged);
            // 
            // buttonLogIn
            // 
            this.buttonLogIn.Location = new System.Drawing.Point(219, 45);
            this.buttonLogIn.Name = "buttonLogIn";
            this.buttonLogIn.Size = new System.Drawing.Size(53, 46);
            this.buttonLogIn.TabIndex = 5;
            this.buttonLogIn.Text = "Log In";
            this.buttonLogIn.UseVisualStyleBackColor = true;
            this.buttonLogIn.Click += new System.EventHandler(this.buttonLogIn_Click);
            // 
            // labelLogowanie
            // 
            this.labelLogowanie.AutoSize = true;
            this.labelLogowanie.Location = new System.Drawing.Point(12, 9);
            this.labelLogowanie.Name = "labelLogowanie";
            this.labelLogowanie.Size = new System.Drawing.Size(58, 13);
            this.labelLogowanie.TabIndex = 6;
            this.labelLogowanie.Text = "Zaloguj się";
            // 
            // linkLabelRejestracja
            // 
            this.linkLabelRejestracja.AutoSize = true;
            this.linkLabelRejestracja.Location = new System.Drawing.Point(200, 9);
            this.linkLabelRejestracja.Name = "linkLabelRejestracja";
            this.linkLabelRejestracja.Size = new System.Drawing.Size(72, 13);
            this.linkLabelRejestracja.TabIndex = 7;
            this.linkLabelRejestracja.TabStop = true;
            this.linkLabelRejestracja.Text = "Zarejestruj się";
            this.linkLabelRejestracja.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelRejestracja_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.linkLabelRejestracja);
            this.Controls.Add(this.labelLogowanie);
            this.Controls.Add(this.buttonLogIn);
            this.Controls.Add(this.textBoxHasło);
            this.Controls.Add(this.textBoxLogin);
            this.Controls.Add(this.labelHasło);
            this.Controls.Add(this.labelLogin);
            this.Controls.Add(this.buttonWyslij);
            this.Name = "Form1";
            this.Text = "User";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonWyslij;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.Label labelHasło;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.TextBox textBoxHasło;
        private System.Windows.Forms.Button buttonLogIn;
        private System.Windows.Forms.Label labelLogowanie;
        private System.Windows.Forms.LinkLabel linkLabelRejestracja;
    }
}

