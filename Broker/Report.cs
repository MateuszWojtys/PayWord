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
    public partial class Report : Form
    {
        // <summary>
        ///Struktura odzwierciedlajaca raport usera
        /// </summary>
        public struct UserReport
        {
            public Clients.UserCertificate uc; //certyfikat
            public string[] lastPayment;//ostatnia platnosc
        }


        public List<List<Clients.UserReport>> allReports; // lista przechowująca raporty od sprzedawców nt płatności klientów
        public Report()
        {
            InitializeComponent();
            allReports = new List<List<Clients.UserReport>>();
        }

        public void addToComboBox()
        {
            comboBox1.Items.Clear();
            for(int i=0; i<allReports.Count;i++)
            {
                for(int j=0; j<allReports[i].Count; j++)
                {

                    comboBox1.Items.Add(allReports[i][j].uc.userName);

                }

            }
        }
        /// <summary>
        /// pobiera text z comboboxa
        /// </summary>
        /// <returns></returns>
        public string getTextFromComboBox()
        {
            string tmp = null;
            tmp = comboBox1.Text;
            return tmp;
        }

        /// <summary>
        /// Pozwla na wyszukanie raportu usera po nazwie usera
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserReport findReport(string userName)
        {
            UserReport tmp = new UserReport();
            tmp.lastPayment = new string[2];

            for (int i = 0; i < allReports.Count; i++)
            {
                for (int j = 0; j < allReports[i].Count; j++ )
                {
                    if (allReports[i][j].uc.userName == userName)
                    {
                        tmp.uc = allReports[i][j].uc;
                        tmp.lastPayment = allReports[i][j].lastPayment;
                    }
                }
                    
            }
            return tmp;

        }
        /// <summary>
        /// Update wyswietlanych dnaych
        /// </summary>
        /// <param name="ur"></param>
        public void updateShowingData(UserReport ur)
        {
            Func<int> del = delegate()
            {
                
                labelBrokername.Text = ur.uc.brokerName;
                labelosttaniaplatnosc.Text = ur.lastPayment[1];
                labelLastCoin.Text = ur.lastPayment[0];
                labelExpirationDate.Text = ur.uc.expirationDate.ToString();
                labelUsername.Text = ur.uc.userName;

                return 0;
            };
            Invoke(del);
        }

        /// <summary>
        /// pokazanie raportu po wybrnaiu z listy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateShowingData(findReport(getTextFromComboBox()));
        }


    }
}
