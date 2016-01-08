namespace Vendor
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
            this.comboBoxUsers = new System.Windows.Forms.ComboBox();
            this.labelCertificate = new System.Windows.Forms.Label();
            this.labelLastPayment = new System.Windows.Forms.Label();
            this.labelBroker = new System.Windows.Forms.Label();
            this.labelBrokerName = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelExpirationDate = new System.Windows.Forms.Label();
            this.labelDate = new System.Windows.Forms.Label();
            this.labelCoinNumberValue = new System.Windows.Forms.Label();
            this.labelCoinValue = new System.Windows.Forms.Label();
            this.labelCoin = new System.Windows.Forms.Label();
            this.labelCoinNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxUsers
            // 
            this.comboBoxUsers.FormattingEnabled = true;
            this.comboBoxUsers.Location = new System.Drawing.Point(12, 12);
            this.comboBoxUsers.Name = "comboBoxUsers";
            this.comboBoxUsers.Size = new System.Drawing.Size(260, 21);
            this.comboBoxUsers.TabIndex = 0;
            this.comboBoxUsers.Text = "Wybierz użytkownika";
            this.comboBoxUsers.SelectedIndexChanged += new System.EventHandler(this.comboBoxUsers_SelectedIndexChanged);
            // 
            // labelCertificate
            // 
            this.labelCertificate.AutoSize = true;
            this.labelCertificate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCertificate.Location = new System.Drawing.Point(83, 46);
            this.labelCertificate.Name = "labelCertificate";
            this.labelCertificate.Size = new System.Drawing.Size(108, 20);
            this.labelCertificate.TabIndex = 1;
            this.labelCertificate.Text = "CERTYFIKAT";
            this.labelCertificate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelLastPayment
            // 
            this.labelLastPayment.AutoSize = true;
            this.labelLastPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLastPayment.Location = new System.Drawing.Point(52, 164);
            this.labelLastPayment.Name = "labelLastPayment";
            this.labelLastPayment.Size = new System.Drawing.Size(176, 20);
            this.labelLastPayment.TabIndex = 2;
            this.labelLastPayment.Text = "OSTATNIA PŁATNOŚĆ";
            // 
            // labelBroker
            // 
            this.labelBroker.AutoSize = true;
            this.labelBroker.Location = new System.Drawing.Point(12, 79);
            this.labelBroker.Name = "labelBroker";
            this.labelBroker.Size = new System.Drawing.Size(77, 13);
            this.labelBroker.TabIndex = 3;
            this.labelBroker.Text = "Wydany przez:";
            // 
            // labelBrokerName
            // 
            this.labelBrokerName.AutoSize = true;
            this.labelBrokerName.Location = new System.Drawing.Point(111, 79);
            this.labelBrokerName.Name = "labelBrokerName";
            this.labelBrokerName.Size = new System.Drawing.Size(0, 13);
            this.labelBrokerName.TabIndex = 4;
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(12, 103);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(105, 13);
            this.labelUser.TabIndex = 5;
            this.labelUser.Text = "Nazwa użytkownika:";
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(123, 103);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(0, 13);
            this.labelUserName.TabIndex = 6;
            // 
            // labelExpirationDate
            // 
            this.labelExpirationDate.AutoSize = true;
            this.labelExpirationDate.Location = new System.Drawing.Point(12, 130);
            this.labelExpirationDate.Name = "labelExpirationDate";
            this.labelExpirationDate.Size = new System.Drawing.Size(83, 13);
            this.labelExpirationDate.TabIndex = 7;
            this.labelExpirationDate.Text = "Data ważności: ";
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(101, 130);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(0, 13);
            this.labelDate.TabIndex = 8;
            // 
            // labelCoinNumberValue
            // 
            this.labelCoinNumberValue.AutoSize = true;
            this.labelCoinNumberValue.Location = new System.Drawing.Point(111, 226);
            this.labelCoinNumberValue.Name = "labelCoinNumberValue";
            this.labelCoinNumberValue.Size = new System.Drawing.Size(0, 13);
            this.labelCoinNumberValue.TabIndex = 9;
            // 
            // labelCoinValue
            // 
            this.labelCoinValue.AutoSize = true;
            this.labelCoinValue.Location = new System.Drawing.Point(111, 198);
            this.labelCoinValue.Name = "labelCoinValue";
            this.labelCoinValue.Size = new System.Drawing.Size(0, 13);
            this.labelCoinValue.TabIndex = 10;
            // 
            // labelCoin
            // 
            this.labelCoin.AutoSize = true;
            this.labelCoin.Location = new System.Drawing.Point(12, 198);
            this.labelCoin.Name = "labelCoin";
            this.labelCoin.Size = new System.Drawing.Size(49, 13);
            this.labelCoin.TabIndex = 11;
            this.labelCoin.Text = "Moneta: ";
            // 
            // labelCoinNumber
            // 
            this.labelCoinNumber.AutoSize = true;
            this.labelCoinNumber.Location = new System.Drawing.Point(12, 226);
            this.labelCoinNumber.Name = "labelCoinNumber";
            this.labelCoinNumber.Size = new System.Drawing.Size(85, 13);
            this.labelCoinNumber.TabIndex = 12;
            this.labelCoinNumber.Text = "Numer płatności";
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.labelCoinNumber);
            this.Controls.Add(this.labelCoin);
            this.Controls.Add(this.labelCoinValue);
            this.Controls.Add(this.labelCoinNumberValue);
            this.Controls.Add(this.labelDate);
            this.Controls.Add(this.labelExpirationDate);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.labelBrokerName);
            this.Controls.Add(this.labelBroker);
            this.Controls.Add(this.labelLastPayment);
            this.Controls.Add(this.labelCertificate);
            this.Controls.Add(this.comboBoxUsers);
            this.Name = "Report";
            this.Text = "Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxUsers;
        private System.Windows.Forms.Label labelCertificate;
        private System.Windows.Forms.Label labelLastPayment;
        private System.Windows.Forms.Label labelBroker;
        private System.Windows.Forms.Label labelBrokerName;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Label labelExpirationDate;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Label labelCoinNumberValue;
        private System.Windows.Forms.Label labelCoinValue;
        private System.Windows.Forms.Label labelCoin;
        private System.Windows.Forms.Label labelCoinNumber;
    }
}