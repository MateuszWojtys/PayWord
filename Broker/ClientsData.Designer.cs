namespace Broker
{
    partial class ClientsData
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
            this.dataGridViewClientsData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClientsData)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewClientsData
            // 
            this.dataGridViewClientsData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewClientsData.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewClientsData.Name = "dataGridViewClientsData";
            this.dataGridViewClientsData.Size = new System.Drawing.Size(867, 261);
            this.dataGridViewClientsData.TabIndex = 0;
            // 
            // ClientsData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 262);
            this.Controls.Add(this.dataGridViewClientsData);
            this.Name = "ClientsData";
            this.Text = "ClientsData";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClientsData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewClientsData;
    }
}