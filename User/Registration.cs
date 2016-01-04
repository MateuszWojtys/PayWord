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
    //Okienko pozwalające na wpisanie danych potrzebnych do rejestracji
    public partial class Registration : Form
    {

        // struktura odzwierciedlająca dane użytkownika do rejestracji
        public struct UserRegistrationData
        {
            public string name; //imie
            public string lastName; //nazwisko
            public string creditCard; // nr karty kredytowej
            public string login; // login
            public string password; // hasło
        }

        //Konstruktor okienka rejestracji
        public Registration()
        {
            InitializeComponent();
            labelBlad.Visible = false;
        }

        //Pozwala na sprawdzenie poprawności danych wpisanych przez usera (głównie czy pola nie są puste)
        // Sprawdzenie czy powtorzone haslo jest takie samo jak haslo poprzednie
        // zwraca true jak jest wszystko ok, false jak nie
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

        //Metoda pozwalająca na pobranie danych z textBoxow - stworzenie nowej struktury UserRegistrationData
        private UserRegistrationData getRegistrationData()
        {
            UserRegistrationData urd = new UserRegistrationData();
   
            urd.name = textBoxImie.Text;
            urd.lastName = textBoxNazwisko.Text;
            urd.creditCard = textBoxKartaKredytowa.Text;
            urd.login = textBoxLogin.Text;
            urd.password = textBoxHaslo.Text;
            return urd;
        }


        //Zamyka okienko po naciśnięciu Anuluj
        private void buttonAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Pozwala na rejestracje - wysłanie do banku 
        private void buttonZarejestruj_Click(object sender, EventArgs e)
        {
            bool tmp = checkRegistrationData();
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

        //Wysyłanie do banku pliku xml z danymi podanymi przez użytkownika jako parametr
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
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                labelBlad.Visible = true;
            }
        }

        //Tworzenie hasha ze stringa
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
