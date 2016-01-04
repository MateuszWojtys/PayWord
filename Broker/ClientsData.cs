using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Broker
{
    //Okno pozwalające zobaczyć dane uzytkowników zarejestrowanych w systemie
    public partial class ClientsData : Form
    {

        //Konstruktor danych klienta
        public ClientsData()
        {
            InitializeComponent();
            this.Show();
            this.Visible = false;
            
        }

        //Ustawienie źródła dla DTV w oknie
        public void setSourceForDTV(DataTable dt)
        {
            dataGridViewClientsData.DataSource = dt;
        }
        

        //metoda pozwalająca na odświeżanie danych w DTV
        public void refreshDataGridView(DataTable dt)
        {
            Func<int> del = delegate()
            {
                dataGridViewClientsData.DataSource = null;
                dataGridViewClientsData.DataSource = dt;

                return 0;
            };
            Invoke(del);
        }

        
    }
}
