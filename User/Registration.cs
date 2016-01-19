using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace User
{

    
    /// <summary>
    /// Okienko pozwalające na wpisanie danych potrzebnych do rejestracji
    /// </summary>
    public partial class Registration : Form
    {

        // <summary>
        /// struktura odzwierciedlająca dane użytkownika do rejestracji
        /// </summary>
        public struct UserRegistrationData
        {
            public string name; //imie
            public string lastName; //nazwisko
            public string creditCard; // nr karty kredytowej
            public string login; // login
            public string password; // hasło
            public RSAParameters publicKey; // klucz publiczny
        }

        

        

        //Konstruktor okienka rejestracji
        public Registration()
        {
            InitializeComponent();
            labelBlad.Visible = false;
            
        }


        /// <summary>
        /// Pozwala na sprawdzenie poprawności danych wpisanych przez usera (głównie czy pola nie są puste)
        /// Sprawdzenie czy powtorzone haslo jest takie samo jak haslo poprzednie
        /// zwraca true jak jest wszystko ok, false jak nie
        /// </summary>
        /// <returns></returns>
        public bool checkRegistrationData()
        {
            bool check = true;
            if (textBoxImie.Text == "")
            {
                textBoxImie.Text = "Wypełnij pole!";
                check = false;
            }
            if (textBoxNazwisko.Text == "")
            {
                textBoxNazwisko.Text = "Wypełnij pole!";
                check = false;
            }
            if (textBoxKartaKredytowa.Text == "")
            {
                textBoxKartaKredytowa.Text = "Wypełnij pole!";
                check = false;
            }
            if (textBoxLogin.Text == "")
            {
                textBoxLogin.Text = "Wypełnij pole!";
                check = false;
            }
            if (textBoxHaslo.Text == "")
            {
                textBoxHaslo.Text = "Wypełnij pole!";
                check = false;
            }
            if(textBoxHaslo.Text != textBoxHaslo2.Text)
            {
                textBoxHaslo.Text = "Błąd!";
                textBoxHaslo2.Text = "Błąd!";
                check = false;
            }

            return check;
        }

   
        /// <summary>
        /// Metoda pozwalająca na pobranie danych z textBoxow - stworzenie nowej struktury UserRegistrationData
        /// </summary>
        /// <returns></returns>
        private UserRegistrationData getRegistrationData()
        {
           
        
            UserRegistrationData urd = new UserRegistrationData();
            urd.name = textBoxImie.Text;
            urd.lastName = textBoxNazwisko.Text;
            urd.creditCard = textBoxKartaKredytowa.Text;
            urd.login = textBoxLogin.Text;
            urd.password = textBoxHaslo.Text;

            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            string str = rsaCSP.ToXmlString(false);
            string str1 = rsaCSP.ToXmlString(true);
            string[] tmp1 = new string[2];
            tmp1[0] = str;
            string[] tmp2 = new string[1];
            tmp2[0] = str1;
            System.IO.File.WriteAllLines(@"D:\Studia\PKRY\PayWord\Klucze\" + urd.login + "PublicKey.txt", tmp1);
            System.IO.File.WriteAllLines(@"D:\Studia\PKRY\PayWord\Klucze\" + urd.login + "BrokerKeys.txt", tmp2);
            connectAndSendToLogger("-", "Wygenerowana została para kluczy dla użytkownika " + urd.login);
            urd.publicKey = rsaCSP.ExportParameters(false);
            connectAndSendToLogger("-", "Klucz publiczny użytkownika " + urd.login + " został udostępniony");
            return urd;
        }


        //Zamyka okienko po naciśnięciu Anuluj
        /// <summary>
        /// //Zamyka okienko po naciśnięciu Anuluj
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Metoda łącząca się z Loggerem i wysylajaca Log
        /// </summary>
        public void connectAndSendToLogger(string destination, string message)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect("127.0.0.1", 6000);
                NetworkStream stream = client.GetStream();
                BinaryWriter bw = new BinaryWriter(stream);
                bw.Write(DateTime.Now.ToString());
                bw.Write("User");
                bw.Write(destination);
                bw.Write(message);
                bw.Close();
                stream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        /// <summary>
        /// Pozwala na rejestracje - wysłanie do banku 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonZarejestruj_Click(object sender, EventArgs e)
        {
            bool tmp = checkRegistrationData();
            connectAndSendToLogger("-", "Sprawdzona została poprawność wprowadzonych danych do rejestracji");
            if (tmp == true)
            {
                UserRegistrationData urd = getRegistrationData();
                MD5 md5 = MD5.Create();
                //tworzony jest hash z podanego hasła i to on jest wysyłany do banku
                urd.password = getMD5Hash(md5, urd.password);
                //Dane wysyłane są do banku jako plik xml
                sendUrdAsXML(urd);
                
            }
            
        }

        
        /// <summary>
        /// Wysyłanie do banku pliku xml z danymi podanymi przez użytkownika 
        /// </summary>
        /// <param name="urd"></param>dane potrzebne do rejestracji
        private void sendUrdAsXML(UserRegistrationData urd)
        {
            try
            {
                //Tworzenie połączenia
                TcpClient client = new TcpClient();
                client.Connect("127.0.0.1", 5000);
                NetworkStream stream = client.GetStream();
                BinaryWriter bw = new BinaryWriter(stream);
                //Wysłanie nagłówka wiadomości
                bw.Write(Protocol.REGISTRATION);
                //Serializacja i wysłanie xml-a
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserRegistrationData));
                StringWriter sw = new StringWriter();
                xmlSerializer.Serialize(sw, urd);
                byte[] bufor = UnicodeEncoding.Unicode.GetBytes(sw.ToString());
                MemoryStream MemoryStream = new MemoryStream();
                GZipStream GZipStream = new GZipStream(MemoryStream, CompressionMode.Compress, true);
                GZipStream.Write(bufor, 0, bufor.Length);
                GZipStream.Flush();
                GZipStream.Close();
                stream.Write(MemoryStream.ToArray(), 0, (int)MemoryStream.Length);
                MemoryStream.Close();
                stream.Close();
                this.Close();
                connectAndSendToLogger("Broker", "Dane rejestracyjne zostały wysłane do Brokera");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                labelBlad.Visible = true;
            }
        }

        
        /// <summary>
        /// Tworzenie hasha ze stringa
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param> string z ktorego ma byc stworozny hash
        /// <returns></returns>
        public string getMD5Hash(MD5 md5Hash, string input)
        {

            // Konwersja stringa na bajty i uzyskanie hasha
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            //przejscie z bajtow z powrotem na stringa (heksadecymalny)
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // zwraca hash - stringa
            return sBuilder.ToString();
        }
    }
}
