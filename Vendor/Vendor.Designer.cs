namespace Vendor
{
    partial class Vendor
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
            this.dataGridViewUsersData = new System.Windows.Forms.DataGridView();
            this.buttonShowReport = new System.Windows.Forms.Button();
            this.buttonSendReport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsersData)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewUsersData
            // 
            this.dataGridViewUsersData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUsersData.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewUsersData.Name = "dataGridViewUsersData";
            this.dataGridViewUsersData.ReadOnly = true;
            this.dataGridViewUsersData.Size = new System.Drawing.Size(336, 149);
            this.dataGridViewUsersData.TabIndex = 0;
            // 
            // buttonShowReport
            // 
            this.buttonShowReport.Location = new System.Drawing.Point(342, 12);
            this.buttonShowReport.Name = "buttonShowReport";
            this.buttonShowReport.Size = new System.Drawing.Size(107, 23);
            this.buttonShowReport.TabIndex = 1;
            this.buttonShowReport.Text = "Pokaż raport";
            this.buttonShowReport.UseVisualStyleBackColor = true;
            this.buttonShowReport.Click += new System.EventHandler(this.buttonShowReport_Click);
            // 
            // buttonSendReport
            // 
            this.buttonSendReport.Location = new System.Drawing.Point(342, 41);
            this.buttonSendReport.Name = "buttonSendReport";
            this.buttonSendReport.Size = new System.Drawing.Size(107, 23);
            this.buttonSendReport.TabIndex = 2;
            this.buttonSendReport.Text = "Wyślij raport";
            this.buttonSendReport.UseVisualStyleBackColor = true;
            this.buttonSendReport.Click += new System.EventHandler(this.buttonSendReport_Click);
            // 
            // Vendor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 149);
            this.Controls.Add(this.buttonSendReport);
            this.Controls.Add(this.buttonShowReport);
            this.Controls.Add(this.dataGridViewUsersData);
            this.Name = "Vendor";
            this.Text = "Vendor";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUsersData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewUsersData;
        private System.Windows.Forms.Button buttonShowReport;
        private System.Windows.Forms.Button buttonSendReport;
    }
}

