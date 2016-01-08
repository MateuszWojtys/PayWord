using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendor
{
    public class Users
    {

         public DataTable dt;

        public Users()
         {
             createUsersTable();
         }
        public void createUsersTable()
        {
            DataColumn[] tmp = new DataColumn[1];
            dt = new DataTable("Użytkownicy");
            DataColumn name = new DataColumn("Nazwa klienta", typeof(string));
           // DataColumn certificateNumber = new DataColumn("Numer certyfikatu", typeof(string));????????????????
            DataColumn lastPaymentCoin = new DataColumn("Ostatnia przesłana moneta", typeof(string));
            DataColumn numberOfSpentCoins = new DataColumn("Ilość wydanych monet w sumie", typeof(string));

            dt.Columns.Add(name);
            dt.Columns.Add(lastPaymentCoin);
            dt.Columns.Add(numberOfSpentCoins);
            tmp[0] = name;
            dt.PrimaryKey = tmp;
        }

        public void updateDataTable(UsersData ud, string[] issuedCoins)
        {
            DataRow foundRow = dt.Rows.Find(issuedCoins[0]);
            foundRow["Ostatnia przesłana moneta"] = ud.lastPayment[0];
            string newValue = (Convert.ToInt32(foundRow["Ilość wydanych monet w sumie"]) + Convert.ToInt32(issuedCoins[1])).ToString();
            foundRow["Ilość wydanych monet w sumie"] = newValue; 
        }
       public void addNewRecord(UsersData ud)
        {
            DataRow row = dt.NewRow();
            row["Nazwa klienta"] = ud.userName;
            row["Ostatnia przesłana moneta"] = ud.lastPayment[0];
            row["Ilość wydanych monet w sumie"] = ud.lastPayment[1];
            dt.Rows.Add(row);
        }

        public struct UsersData
        {
            public string userName;
            public List<string> lastPayment;
            public UserCertificate certificate;
            public UserCommitment commitment;
        }

        public struct UserCommitment
        {
            public string vendorName;
            public string basicCoin;
            public string date;
            public string legthOfPayword;
        }

        public struct UserCertificate
        {
            public string brokerName; // nazwa banku
            public string userName; // nazwa usera
            //klucz publiczny usera??????????????????????????????????????????????????????????????????????
            public DateTime expirationDate;// data wygaśnięcia certyfikatu
        }



    }

    
}
