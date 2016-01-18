using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Vendor
{
    /// <summary>
    /// Klasa odpowiedzialna za użytkownikó u Sprzedawcy
    /// </summary>
    public class Users
    {
        //Struktura odzwierciedlajaca dane użytkownika
        public struct UsersData
        {
            public string userName; // nazwa usera
            public List<string> lastPayment;// ostatnia platnosc
            public UserCertificate certificate;// cetryfikat usera
            public UserCommitment commitment;// commitment usera
        }

        /// <summary>
        /// Struktura odzwierciedlajaca commitment użytkownika
        /// </summary>
        public struct UserCommitment
        {
            public string vendorName;// nazwa sprzwdawcy
            public string basicCoin;// podstawowa moneta
            public string date;//data
            public string legthOfPayword;// dlugosc paywaroda
        }

        /// <summary>
        /// Struktura odzwierciedlajaca certyfikat usera
        /// </summary>
        public struct UserCertificate
        {
            public string brokerName; // nazwa banku
            public string userName; // nazwa usera
            public RSAParameters publicKey;//klucz publiczny usera
            public DateTime expirationDate;// data wygaśnięcia certyfikatu
        }

        //Tablica przechowujaca dane uzytkownikow
        public DataTable dt;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Users()
         {
             createUsersTable();
         }
        /// <summary>
        /// Metoda tworzaca tabele do przechowywania danych uzytkownikow
        /// </summary>
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

        /// <summary>
        /// Metoda pozwalajaca na aktualizacje danych w tabeli
        /// </summary>
        /// <param name="ud"></param>//dane usera
        /// <param name="issuedCoins"></param>//wydane monety
        public void updateDataTable(UsersData ud, string[] issuedCoins)
        {
            DataRow foundRow = dt.Rows.Find(issuedCoins[0]);
            foundRow["Ostatnia przesłana moneta"] = ud.lastPayment[0];
            string newValue = (Convert.ToInt32(foundRow["Ilość wydanych monet w sumie"]) + Convert.ToInt32(issuedCoins[1])).ToString();
            foundRow["Ilość wydanych monet w sumie"] = newValue; 
        }
        /// <summary>
        /// Metoda pozwalajaca na dodanie nowego wierszza do tabeli
        /// </summary>
        /// <param name="ud"></param>

       public void addNewRecord(UsersData ud)
        {
            DataRow row = dt.NewRow();
            row["Nazwa klienta"] = ud.userName;
            row["Ostatnia przesłana moneta"] = ud.lastPayment[0];
            row["Ilość wydanych monet w sumie"] = ud.lastPayment[1];
            dt.Rows.Add(row);
        }




    }

    
}
