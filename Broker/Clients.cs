using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

        ArrayList passwords = new ArrayList();
        //Konstruktor  - inicjowane są obie powyższe listy
       public  Clients()
        {
           //Pobranie danych o zarejestrowanych już klientach w systemie
             getDataFromFile();
           //Stworzenie list w których przechowywane będą odpowiednie dane
             usersRegistrationDatas = new List<UserRegistrationData>();
             usersData = new List<UserData>();

             // Stworzenie DataTable - utworzenie odpowiednich kolumn
             createDataTable();
             
        }
        
        //Struktura odpowiadająca certyfikatowi
        public struct UserCertificate
        {
            public string brokerName; // nazwa banku
            public string userName; // nazwa usera
            public RSAParameters publicKey;//klucz publiczny usera??????????????????????????????????????????????????????????????????????
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
            public RSAParameters publicKey; // klucz publiczny
        }

        // struktura odzwierciedlająca pełne dane użytkownika
        public struct UserData
        {
            public UserCertificate certificate; //certyfikat
            public UserRegistrationData urd; //dane podane podczas rejestracji
        }

        //Metoda pozwalająca zweryfikować login i hash z hasła podczas logowania
        public bool checkUserData(string login, string passwordHash)
        {
            bool verify = false;

            //odszukanie na liscie loginu i sprawdzenie poprawnosci hasha
            List<string> list = new List<string>();
            foreach (string line in passwords)
            {
                list = parse(line.Trim());
                if (list[0] == login)
                {

                    if (list[1] == passwordHash)
                    {
                        Console.WriteLine("Zalogowano");
                        verify = true;
                        break;
                    }
                }
            }
            return verify;
        }

        //Przypisanie wartosci z pliku tekstowego do listy
        public void getDataFromFile()
        {
            passwords = readData("Passwords.txt");
        }


        //pobranie wartosci z pliku tekstowego i dodanie do listy
        private static ArrayList readData(string fileName)
        {
            ArrayList data = new ArrayList();
            string[] lines = System.IO.File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                data.Add(line);
            }
            return data;
        }

        //metoda pozwalająca na oddzielenie slow, ktore odzdzielone sa spacja
        private static List<string> parse(string line)
        {
            List<string> datas = new List<string>();
            string[] temp = line.Split(' ');
            foreach (string tmpChar in temp)
            {
                datas.Add(tmpChar);

            }
            return datas;
        }
        //Zapisywanie do XMla danych  użytkowników 
        public void writeToXMLUsers(DataTable dt)
        {
            dt.WriteXml("Users.xml");
        }


        //Odczytywanie danych  uzytkownikow z xml 
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
            
            DataColumn certificateDate = new DataColumn("Data wygaśnięcia certyfikatu", typeof(DateTime));
            dt.Columns.Add(name);
            dt.Columns.Add(login);
            dt.Columns.Add(passwordHash);
            dt.Columns.Add(creditCard);
            
            dt.Columns.Add(certificateDate);
        }

        //Dodanie do pliku loginu i hashu z hasła nowego klienta (rejestracja)
        public void addToPasswordList(string login, string password)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Studia\PKRY\PayWord\Broker\Broker\bin\Debug\passwords.txt", true))
            {
                file.WriteLine(login + " " + password);
            }  
           
        }

        //Metoda pozwalająca na dodanie nowego rekordu
        public void addNewData(string name, string login, string hash, string creditCard,  DateTime date)
        {
            DataRow row = dt.NewRow();
            row["Nazwa klienta"] = name;
            row["Login"] = login;
            row["Hash z hasła"] = hash;
            row["Numer karty kredytowej"] = creditCard;
            
            row["Data wygaśnięcia certyfikatu"] = date;
            dt.Rows.Add(row);

        }
        //metoda tworzaca i zwracajaca certyfikat
        public UserCertificate createCertificate(UserRegistrationData urd)
        {
            UserCertificate uc = new UserCertificate();
            uc.brokerName = "Bank";
            uc.userName = urd.name + " " + urd.lastName;
            uc.publicKey = urd.publicKey;

            //Obliczanie daty wygaśnięcia certyfikatu - miesiąc od obecnej daty
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
