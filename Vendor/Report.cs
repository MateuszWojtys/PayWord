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
    public partial class Report : Form
    {
        List<UserReport> usersReports;
       public struct UserReport
        {
            public Users.UserCertificate uc;
            public string[] lastPayment;
        }

        public Report(List<UserReport> ur, List<string> userNames)
        {
            InitializeComponent();
            addToComboBox(userNames);
            usersReports = ur;
            UserReport report = findReport(getTextFromComboBox());
            
        }

        public void addToComboBox(List<string> usernames)
        {
            for(int i = 0; i< usernames.Count; i++)
            {
                comboBoxUsers.Items.Add(usernames[i]);
               
            }
        }
        public string getTextFromComboBox()
        {
            string tmp = null;
            tmp = comboBoxUsers.Text;
            return tmp;
        }

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

        private void comboBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateShowingData(findReport(getTextFromComboBox()));
        }
    }
}
