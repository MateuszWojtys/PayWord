using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vendor
{
    //Klasa odwzorowujaca raport od Sprzedawcy
    public partial class Report : Form
    {
        //Lista prechowuja rapory od Sprzedawcow
        List<UserReport> usersReports;

        /// <summary>
        ///Struktura odzwierciedlajaca raport usera
        /// </summary>
       public struct UserReport
        {
            public Users.UserCertificate uc; //certyfikat
            public string[] lastPayment;//ostatnia platnosc
        }

        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="ur"></param>
        /// <param name="userNames"></param>
        public Report(List<UserReport> ur, List<string> userNames)
        {
            InitializeComponent();
            addToComboBox(userNames);
            usersReports = ur;
            UserReport report = findReport(getTextFromComboBox());
            
        }
        /// <summary>
        /// pozwala na doanie do comboboxa
        /// </summary>
        /// <param name="usernames"></param>
        public void addToComboBox(List<string> usernames)
        {
            for(int i = 0; i< usernames.Count; i++)
            {
                comboBoxUsers.Items.Add(usernames[i]);
               
            }
        }
        /// <summary>
        /// pobiera text z comboboxa
        /// </summary>
        /// <returns></returns>
        public string getTextFromComboBox()
        {
            string tmp = null;
            tmp = comboBoxUsers.Text;
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

            for (int i = 0; i < usersReports.Count; i++ )
            {
                if(usersReports[i].uc.userName == userName)
                {

                    tmp = usersReports[i];
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
                
                labelBrokerName.Text = ur.uc.brokerName;
                labelCoinNumberValue.Text = ur.lastPayment[1];
                labelCoinValue.Text = ur.lastPayment[0];
                labelDate.Text = ur.uc.expirationDate.ToString();
                labelUserName.Text = ur.uc.userName;

                return 0;
            };
            Invoke(del);
        }

        /// <summary>
        /// pokazanie raportu po wybrnaiu z listy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateShowingData(findReport(getTextFromComboBox()));
        }
    }
}
