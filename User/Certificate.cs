using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
    /// Okno wyświetlające dane certyfikatu
    /// </summary>
    public partial class Certificate : Form
    {
        //Konstruktror z parametrem (login klienta)
        public Certificate(string login)
        {
            InitializeComponent();
            //Pobranie odpowiedniego certyfikatu
            List<string> info = getCertificateDatas(login);
            //Wyświtlenie danych z certyfikatu w oknie
            showInfo(info[0], info[1], info[2]);
        }
        /// <summary>
        /// Struktura odpowiadajaca podpisowi cyfrowemu
        /// przechowuje ilosc bajtow(128) oraz bajty odpowiedzialna za sam podpis
        /// </summary>
        public struct Sign
        {
            public int length;
            public byte[] sign;
        }

       
        /// <summary>
        /// struktura odpowiadająca certyfikatowi
        /// </summary>
        public struct UserCertificate
        {
            public string brokerName; // nazwa banku
            public string userName; // nazwa usera
            public RSAParameters publicKey;//klucz publiczny usera
            public DateTime expirationDate;// data wygaśnięcia certyfikatu
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
        /// Pobranie certyfikatu i przypisanie informacji do listy stringów
        /// </summary>
        /// <param name="loginAsFileName"></param> login - nazwa pliku
        /// <returns></returns>
        public List<string> getCertificateDatas(string loginAsFileName)
        {
            List<string> list = new List<string>();
          //Deserializacja - odczytanie odpowiedniego (po loginie) pliku xml i właściwa deserializacja
            XmlSerializer xml = new XmlSerializer(typeof(UserCertificate));
            string x = System.IO.File.ReadAllText(@"D:\Studia\PKRY\PayWord\" + loginAsFileName + ".xml");
            StringReader sr = new StringReader(x);
            UserCertificate tmp = (UserCertificate)xml.Deserialize(sr);
            //Dodanie do listy danych z certyfikatu
            list.Add(tmp.brokerName);
            list.Add(tmp.userName);
            list.Add(tmp.expirationDate.ToString());
            return list;
        }

        
        /// <summary>
        /// zwraca xmla ktory zawiera certyfikat
        /// </summary>
        /// <param name="loginAsFileName"></param>login - nazwa pliku
        /// <returns></returns>
        public string getCertificate(string loginAsFileName)
        {
            
            string x = System.IO.File.ReadAllText(@"D:\Studia\PKRY\PayWord\" + loginAsFileName + ".xml");
            return x;
        }

        /// <summary>
        /// Pobiera z pliku podpis certyfikatu
        /// </summary>
        /// <param name="loginAsFileName"></param> login- nazwa pliku
        /// <returns></returns>
        public Sign getCertificateSign(string loginAsFileName)
        {
            Sign s = new Sign();
            byte[] sign;
            FileStream fs2 = new System.IO.FileStream(@"D:\Studia\PKRY\PayWord\" + "Sign" + loginAsFileName + ".txt", FileMode.Open);
            sign = new byte[fs2.Length];
            fs2.Position = 0;
            fs2.Read(sign, 0, (int)fs2.Length);
            fs2.Close();
            s.length = sign.Length;
            s.sign = sign;
            return s;
        }
        
        /// <summary>
        /// Pokazanie informacji w oknie
        /// </summary>
        /// <param name="broker"></param>nazwa banku
        /// <param name="user"></param>nazwa usera
        /// <param name="date"></param>data
        public void showInfo(string broker, string user, string date)
        {
            labelBank.Text = "Nazwa banku: " + broker;
            labelUser.Text = "Nazwa klienta: " + user;
            labelDate.Text = "Data ważności: " + date;

        }
    }
}
