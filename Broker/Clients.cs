using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Broker
{
    // Klasa reprezentująca klientów
    public class Clients
    {
       //Lista przechowująca dane dot. danych rejestracyjnych userów
       public List<UserRegistrationData> usersRegistrationDatas;

        //Lista przechowująca pełne dane dot. userów
       public List<UserData> usersData;

       //DataTable przechowująca dane o użytkownikach
       public DataTable dt;

        //Konstruktor  - inicjowane są obie powyższe listy
       public  Clients()
        {   
             usersRegistrationDatas = new List<UserRegistrationData>();
             usersData = new List<UserData>();

             // Stworzenie DataTable - utworzenie odpowiednich kolumn
             createDataTable();
             
        }
        
        //struktura odpowiadająca certyfikatowi
        public struct UserCertificate
        {
            public string brokerName; // nazwa banku
            public string userName; // nazwa usera
            //klucz publiczny usera??????????????????????????????????????????????????????????????????????
            public DateTime expirationDate;// data wygaśnięcia certyfikatu
        }


        // struktura odzwierciedlająca dane użytkownika do rejestracji
        public struct UserRegistrationData
        {
            public string name; //imie
            public string lastName; //nazwisko
            public string creditCard; // nr karty kredytowej
            public string login; // login
            public string password; // hasło
        }

        // struktura odzwierciedlająca pełne dane użytkownika
        public struct UserData
        {
            public UserCertificate certificate;
            public UserRegistrationData urd;
        }


        //Zapisywanie do XMla danych  użytkowników PRZEROBIC!!!!!!!!!!!!!!!!!!!!!!!!
        public void writeToXMLUsers(DataTable dt)
        {
            dt.WriteXml("Users.xml");
        }


        //Odczytywanie danych  uzytkownikow z xml PRZEROBIC !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public void readFromXMLUsers( DataTable dt, List<UserData> users)
        {
            dt.ReadXml("Users.xml");
        }

        //Metoda tworząca DataTable (kolumny) , która będzie przechowywać dane o użytkownikach
        private void createDataTable()
        {
            dt = new DataTable("Klienci banku");
            DataColumn name = new DataColumn("Nazwa klienta", typeof(string));
            DataColumn login = new DataColumn("Login", typeof(string));
            DataColumn passwordHash = new DataColumn("Hash z hasła", typeof(string));
            DataColumn creditCard = new DataColumn("Numer karty kredytowej", typeof(string));
            DataColumn certificate = new DataColumn("Certyfikat", typeof(bool));
            DataColumn certificateDate = new DataColumn("Data wygaśnięcia certyfikatu", typeof(DateTime));
            dt.Columns.Add(name);
            dt.Columns.Add(login);
            dt.Columns.Add(passwordHash);
            dt.Columns.Add(creditCard);
            dt.Columns.Add(certificate);
            dt.Columns.Add(certificateDate);
        }

        //Metoda pozwalająca na dodanie nowego rekordu
        public void addNewData(int id, string name, string login, string hash, string creditCard, bool cert, DateTime date)
        {
            DataRow row = dt.NewRow();
            row["Nazwa klienta"] = name;
            row["Login"] = login;
            row["Hash z hasła"] = hash;
            row["Numer karty kredytowej"] = creditCard;
            row["Certyfikat"] = cert;
            row["Data wygaśnięcia certyfikatu"] = date;
            dt.Rows.Add(row);

        }
        //metoda tworzaca i zwracajaca certyfikat
        public UserCertificate createCertificate(UserRegistrationData urd)
        {
            UserCertificate uc = new UserCertificate();
            uc.brokerName = "Bank";
            uc.userName = urd.name + " " + urd.lastName;

            //Obliczanie daty wygaśnięcia certufikatu - miesiąc od obecnej daty
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            uc.expirationDate = new DateTime(year, month + 1, day);
            return uc;
        }

        //Tworzenie danych użytkownika
        public UserData createUserData(UserRegistrationData urd, UserCertificate uc)
        {
            UserData ud = new UserData();
            ud.urd = urd;
            ud.certificate = uc;
            usersData.Add(ud);
            return ud;
        }
    }
}
