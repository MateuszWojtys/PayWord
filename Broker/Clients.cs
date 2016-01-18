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
    
    /// <summary>
    /// Klasa reprezentująca klientów Banku
    /// </summary>
    public class Clients
    {
       //Lista przechowująca dane dot. danych rejestracyjnych userów
       public List<UserRegistrationData> usersRegistrationDatas;

        //Lista przechowująca pełne dane dot. userów
       public List<UserData> usersData;

       //DataTable przechowująca dane o użytkownikach
       public DataTable dt;
        //Lista przechowująca hasła użytkowników do systemu
       ArrayList passwords = new ArrayList();
        /// <summary>
        /// Struktura odpowiadająca za raport przesyłany przez Sprzedawce do Banku -
        /// - zawiera info dot. platnosci klienta
        /// </summary>
       public struct UserReport
       {
           public UserCertificate uc;
           public string[] lastPayment;
       }


        
        
        /// <summary>
       /// Konstruktor  - inicjowane są obie powyższe listy
        /// </summary>
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
            public RSAParameters publicKey;//klucz publiczny usera
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

        
        /// <summary>
        /// Metoda pozwalająca zweryfikować login i hash z hasła podczas logowania
        /// </summary>
        /// <param name="login"></param> // login podany podczas logowania
        /// <param name="passwordHash"></param>// hash z hasła - otrzymany od Usera
        /// <returns></returns>
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

        
        /// <summary>
        /// Przypisanie wartosci z pliku tekstowego zaweirajacego hashe z haseł do listy
        /// </summary>
        public void getDataFromFile()
        {
            passwords = readData("Passwords.txt");
        }


        
        /// <summary>
        /// pobranie wartosci z pliku tekstowego i dodanie do listy
        /// </summary>
        /// <param name="fileName"></param> nazwa pliku do odczytu
        /// <returns></returns>
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

        
        /// <summary>
        /// metoda pozwalająca na oddzielenie slow, ktore odzdzielone sa spacja
        /// </summary>
        /// <param name="line"></param>// dany ciag znakow do podzielenia
        /// <returns></returns>
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
        
        /// <summary>
        /// Zapisywanie do XMla danych  użytkowników 
        /// </summary>
        /// <param name="dt"></param> dane użytkowników
        public void writeToXMLUsers(DataTable dt)
        {
            dt.WriteXml("Users.xml");
        }


        
        /// <summary>
        /// Odczytywanie danych  uzytkownikow z xml 
        /// </summary>
        /// <param name="dt"></param> dane użytkowników
        public void readFromXMLUsers( DataTable dt)
        {
            dt.ReadXml("Users.xml");
        }

        
        /// <summary>
        /// Metoda tworząca DataTable (kolumny) , która będzie przechowywać dane o użytkownikach
        /// </summary>
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

        
        /// <summary>
        /// Dodanie do pliku loginu i hashu z hasła nowego klienta (rejestracja)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public void addToPasswordList(string login, string password)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"D:\Studia\PKRY\PayWord\Broker\Broker\bin\Debug\passwords.txt", true))
            {
                file.WriteLine(login + " " + password);
            }  
           
        }

        
        /// <summary>
        /// Metoda pozwalająca na dodanie nowego rekordu
        /// </summary>
        /// <param name="name"></param> nazwa użytkownika
        /// <param name="login"></param>login użytkownika
        /// <param name="hash"></param>hash z hasła użytkownika
        /// <param name="creditCard"></param>numer karty kredytowej użytkownika
        /// <param name="date"></param>data ważności certyfikatu
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
      
        /// <summary>
        /// metoda tworzaca i zwracajaca certyfikat
        /// </summary>
        /// <param name="urd"></param>// dane podane podczas rejestracji
        /// <returns></returns>
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

        
        /// <summary>
        /// Tworzenie danych użytkownika
        /// </summary>
        /// <param name="urd"></param>dane podane podczas rejestracji
        /// <param name="uc"></param> certyfikat uzytkownika
        /// <returns></returns>
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
