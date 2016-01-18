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
   
    /// <summary>
    /// Okno pozwalające zobaczyć dane uzytkowników zarejestrowanych w systemie
    /// </summary>
    public partial class ClientsData : Form
    {

      
        /// <summary>
        /// Konstruktor danych klienta
        /// </summary>
        public ClientsData()
        {
            InitializeComponent();
            this.Show();
            this.Visible = false;
            
        }

        
        /// <summary>
        /// //Ustawienie źródła dla DTV w oknie
        /// </summary>
        /// <param name="dt"></param> // źródło do wyświetlenia
        public void setSourceForDTV(DataTable dt)
        {
            dataGridViewClientsData.DataSource = dt;
        }
        

        
        /// <summary>
        /// metoda pozwalająca na odświeżanie danych w DTV
        /// </summary>
        /// <param name="dt"></param> // miejsce w ktorym zmienily sie dane
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
