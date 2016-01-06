namespace User
{
    partial class User
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
            this.buttonZaplac = new System.Windows.Forms.Button();
            this.labelLogin = new System.Windows.Forms.Label();
            this.labelHasło = new System.Windows.Forms.Label();
            this.textBoxLogin = new System.Windows.Forms.TextBox();
            this.textBoxHasło = new System.Windows.Forms.TextBox();
            this.buttonLogIn = new System.Windows.Forms.Button();
            this.labelLogowanie = new System.Windows.Forms.Label();
            this.linkLabelRejestracja = new System.Windows.Forms.LinkLabel();
            this.buttonWyloguj = new System.Windows.Forms.Button();
            this.linkLabelCertificate = new System.Windows.Forms.LinkLabel();
            this.buttonCoinsGeneration = new System.Windows.Forms.Button();
            this.comboBoxCoins = new System.Windows.Forms.ComboBox();
            this.numericUpDownCoinsNumber = new System.Windows.Forms.NumericUpDown();
            this.labelCoins = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoinsNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonZaplac
            // 
            this.buttonZaplac.Location = new System.Drawing.Point(95, 217);
            this.buttonZaplac.Name = "buttonZaplac";
            this.buttonZaplac.Size = new System.Drawing.Size(75, 23);
            this.buttonZaplac.TabIndex = 0;
            this.buttonZaplac.Text = "Zapłać";
            this.buttonZaplac.UseVisualStyleBackColor = true;
            this.buttonZaplac.Click += new System.EventHandler(this.buttonZaplac_Click);
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
            // buttonWyloguj
            // 
            this.buttonWyloguj.Location = new System.Drawing.Point(197, 4);
            this.buttonWyloguj.Name = "buttonWyloguj";
            this.buttonWyloguj.Size = new System.Drawing.Size(75, 23);
            this.buttonWyloguj.TabIndex = 8;
            this.buttonWyloguj.Text = "Wyloguj";
            this.buttonWyloguj.UseVisualStyleBackColor = true;
            this.buttonWyloguj.Click += new System.EventHandler(this.buttonWyloguj_Click);
            // 
            // linkLabelCertificate
            // 
            this.linkLabelCertificate.AutoSize = true;
            this.linkLabelCertificate.Location = new System.Drawing.Point(161, 29);
            this.linkLabelCertificate.Name = "linkLabelCertificate";
            this.linkLabelCertificate.Size = new System.Drawing.Size(122, 13);
            this.linkLabelCertificate.TabIndex = 9;
            this.linkLabelCertificate.TabStop = true;
            this.linkLabelCertificate.Text = "Informacje o certyfikacie";
            this.linkLabelCertificate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCertificate_LinkClicked);
            // 
            // buttonCoinsGeneration
            // 
            this.buttonCoinsGeneration.Location = new System.Drawing.Point(12, 137);
            this.buttonCoinsGeneration.Name = "buttonCoinsGeneration";
            this.buttonCoinsGeneration.Size = new System.Drawing.Size(98, 27);
            this.buttonCoinsGeneration.TabIndex = 10;
            this.buttonCoinsGeneration.Text = "Generuj monety";
            this.buttonCoinsGeneration.UseVisualStyleBackColor = true;
            this.buttonCoinsGeneration.Click += new System.EventHandler(this.buttonCoinsGeneration_Click);
            // 
            // comboBoxCoins
            // 
            this.comboBoxCoins.FormattingEnabled = true;
            this.comboBoxCoins.Location = new System.Drawing.Point(12, 181);
            this.comboBoxCoins.Name = "comboBoxCoins";
            this.comboBoxCoins.Size = new System.Drawing.Size(260, 21);
            this.comboBoxCoins.TabIndex = 11;
            this.comboBoxCoins.Text = "Wybierz monetę";
            // 
            // numericUpDownCoinsNumber
            // 
            this.numericUpDownCoinsNumber.Location = new System.Drawing.Point(125, 142);
            this.numericUpDownCoinsNumber.Name = "numericUpDownCoinsNumber";
            this.numericUpDownCoinsNumber.Size = new System.Drawing.Size(34, 20);
            this.numericUpDownCoinsNumber.TabIndex = 12;
            // 
            // labelCoins
            // 
            this.labelCoins.AutoSize = true;
            this.labelCoins.Location = new System.Drawing.Point(159, 144);
            this.labelCoins.Name = "labelCoins";
            this.labelCoins.Size = new System.Drawing.Size(0, 13);
            this.labelCoins.TabIndex = 13;
            // 
            // User
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 262);
            this.Controls.Add(this.labelCoins);
            this.Controls.Add(this.numericUpDownCoinsNumber);
            this.Controls.Add(this.comboBoxCoins);
            this.Controls.Add(this.buttonCoinsGeneration);
            this.Controls.Add(this.linkLabelCertificate);
            this.Controls.Add(this.buttonWyloguj);
            this.Controls.Add(this.linkLabelRejestracja);
            this.Controls.Add(this.labelLogowanie);
            this.Controls.Add(this.buttonLogIn);
            this.Controls.Add(this.textBoxHasło);
            this.Controls.Add(this.textBoxLogin);
            this.Controls.Add(this.labelHasło);
            this.Controls.Add(this.labelLogin);
            this.Controls.Add(this.buttonZaplac);
            this.Name = "User";
            this.Text = "User";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoinsNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonZaplac;
        private System.Windows.Forms.Label labelLogin;
        private System.Windows.Forms.Label labelHasło;
        private System.Windows.Forms.TextBox textBoxLogin;
        private System.Windows.Forms.TextBox textBoxHasło;
        private System.Windows.Forms.Button buttonLogIn;
        private System.Windows.Forms.Label labelLogowanie;
        private System.Windows.Forms.LinkLabel linkLabelRejestracja;
        private System.Windows.Forms.Button buttonWyloguj;
        private System.Windows.Forms.LinkLabel linkLabelCertificate;
        private System.Windows.Forms.Button buttonCoinsGeneration;
        private System.Windows.Forms.ComboBox comboBoxCoins;
        private System.Windows.Forms.NumericUpDown numericUpDownCoinsNumber;
        private System.Windows.Forms.Label labelCoins;
    }
}

