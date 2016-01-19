namespace Broker
{
    partial class Report
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelCertificate = new System.Windows.Forms.Label();
            this.labelWydanyprzez = new System.Windows.Forms.Label();
            this.labelBrokername = new System.Windows.Forms.Label();
            this.labelNameUser = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labeldate = new System.Windows.Forms.Label();
            this.labelExpirationDate = new System.Windows.Forms.Label();
            this.labelosttaniaplatnosc = new System.Windows.Forms.Label();
            this.labelMoneta = new System.Windows.Forms.Label();
            this.labelLastCoin = new System.Windows.Forms.Label();
            this.labelPaymentNumber = new System.Windows.Forms.Label();
            this.labelLastPaymnet = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(258, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // labelCertificate
            // 
            this.labelCertificate.AutoSize = true;
            this.labelCertificate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCertificate.Location = new System.Drawing.Point(64, 36);
            this.labelCertificate.Name = "labelCertificate";
            this.labelCertificate.Size = new System.Drawing.Size(143, 25);
            this.labelCertificate.TabIndex = 1;
            this.labelCertificate.Text = "CERTYFIKAT";
            // 
            // labelWydanyprzez
            // 
            this.labelWydanyprzez.AutoSize = true;
            this.labelWydanyprzez.Location = new System.Drawing.Point(12, 68);
            this.labelWydanyprzez.Name = "labelWydanyprzez";
            this.labelWydanyprzez.Size = new System.Drawing.Size(77, 13);
            this.labelWydanyprzez.TabIndex = 2;
            this.labelWydanyprzez.Text = "Wydany przez:";
            // 
            // labelBrokername
            // 
            this.labelBrokername.AutoSize = true;
            this.labelBrokername.Location = new System.Drawing.Point(131, 68);
            this.labelBrokername.Name = "labelBrokername";
            this.labelBrokername.Size = new System.Drawing.Size(0, 13);
            this.labelBrokername.TabIndex = 3;
            // 
            // labelNameUser
            // 
            this.labelNameUser.AutoSize = true;
            this.labelNameUser.Location = new System.Drawing.Point(12, 91);
            this.labelNameUser.Name = "labelNameUser";
            this.labelNameUser.Size = new System.Drawing.Size(105, 13);
            this.labelNameUser.TabIndex = 4;
            this.labelNameUser.Text = "Nazwa użytkownika:";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(131, 91);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(0, 13);
            this.labelUsername.TabIndex = 5;
            // 
            // labeldate
            // 
            this.labeldate.AutoSize = true;
            this.labeldate.Location = new System.Drawing.Point(12, 114);
            this.labeldate.Name = "labeldate";
            this.labeldate.Size = new System.Drawing.Size(80, 13);
            this.labeldate.TabIndex = 6;
            this.labeldate.Text = "Data ważności:";
            // 
            // labelExpirationDate
            // 
            this.labelExpirationDate.AutoSize = true;
            this.labelExpirationDate.Location = new System.Drawing.Point(131, 114);
            this.labelExpirationDate.Name = "labelExpirationDate";
            this.labelExpirationDate.Size = new System.Drawing.Size(0, 13);
            this.labelExpirationDate.TabIndex = 7;
            // 
            // labelosttaniaplatnosc
            // 
            this.labelosttaniaplatnosc.AutoSize = true;
            this.labelosttaniaplatnosc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelosttaniaplatnosc.Location = new System.Drawing.Point(24, 142);
            this.labelosttaniaplatnosc.Name = "labelosttaniaplatnosc";
            this.labelosttaniaplatnosc.Size = new System.Drawing.Size(211, 24);
            this.labelosttaniaplatnosc.TabIndex = 8;
            this.labelosttaniaplatnosc.Text = "OSTATNIA PŁATNOŚĆ";
            // 
            // labelMoneta
            // 
            this.labelMoneta.AutoSize = true;
            this.labelMoneta.Location = new System.Drawing.Point(12, 177);
            this.labelMoneta.Name = "labelMoneta";
            this.labelMoneta.Size = new System.Drawing.Size(46, 13);
            this.labelMoneta.TabIndex = 9;
            this.labelMoneta.Text = "Moneta:";
            // 
            // labelLastCoin
            // 
            this.labelLastCoin.AutoSize = true;
            this.labelLastCoin.Location = new System.Drawing.Point(131, 177);
            this.labelLastCoin.Name = "labelLastCoin";
            this.labelLastCoin.Size = new System.Drawing.Size(0, 13);
            this.labelLastCoin.TabIndex = 10;
            // 
            // labelPaymentNumber
            // 
            this.labelPaymentNumber.AutoSize = true;
            this.labelPaymentNumber.Location = new System.Drawing.Point(12, 202);
            this.labelPaymentNumber.Name = "labelPaymentNumber";
            this.labelPaymentNumber.Size = new System.Drawing.Size(88, 13);
            this.labelPaymentNumber.TabIndex = 11;
            this.labelPaymentNumber.Text = "Numer płatności:";
            // 
            // labelLastPaymnet
            // 
            this.labelLastPaymnet.AutoSize = true;
            this.labelLastPaymnet.Location = new System.Drawing.Point(131, 202);
            this.labelLastPaymnet.Name = "labelLastPaymnet";
            this.labelLastPaymnet.Size = new System.Drawing.Size(0, 13);
            this.labelLastPaymnet.TabIndex = 12;
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.labelLastPaymnet);
            this.Controls.Add(this.labelPaymentNumber);
            this.Controls.Add(this.labelLastCoin);
            this.Controls.Add(this.labelMoneta);
            this.Controls.Add(this.labelosttaniaplatnosc);
            this.Controls.Add(this.labelExpirationDate);
            this.Controls.Add(this.labeldate);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.labelNameUser);
            this.Controls.Add(this.labelBrokername);
            this.Controls.Add(this.labelWydanyprzez);
            this.Controls.Add(this.labelCertificate);
            this.Controls.Add(this.comboBox1);
            this.Name = "Report";
            this.Text = "Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label labelCertificate;
        private System.Windows.Forms.Label labelWydanyprzez;
        private System.Windows.Forms.Label labelBrokername;
        private System.Windows.Forms.Label labelNameUser;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labeldate;
        private System.Windows.Forms.Label labelExpirationDate;
        private System.Windows.Forms.Label labelosttaniaplatnosc;
        private System.Windows.Forms.Label labelMoneta;
        private System.Windows.Forms.Label labelLastCoin;
        private System.Windows.Forms.Label labelPaymentNumber;
        private System.Windows.Forms.Label labelLastPaymnet;
    }
}