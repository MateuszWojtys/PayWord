using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace User
{
    //Okno wyświetlające dane certyfikatu
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
        public struct Sign
        {
            public int length;
            public byte[] sign;
        }
        //struktura odpowiadająca certyfikatowi
        public struct UserCertificate
        {
            public string brokerName; // nazwa banku
            public string userName; // nazwa usera
            public RSAParameters publicKey;//klucz publiczny usera??????????????????????????????????????????????????????????????????????
            public DateTime expirationDate;// data wygaśnięcia certyfikatu
        }

        //Pobranie certyfikatu i przypisanie informacji do listy stringów
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

        //zwraca xmla ktory zawiera certyfikat
        public string getCertificate(string loginAsFileName)
        {
            
            string x = System.IO.File.ReadAllText(@"D:\Studia\PKRY\PayWord\" + loginAsFileName + ".xml");
            return x;
        }

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
        //Pokazanie informacji w oknie
        public void showInfo(string broker, string user, string date)
        {
            labelBank.Text = "Nazwa banku: " + broker;
            labelUser.Text = "Nazwa klienta: " + user;
            labelDate.Text = "Data ważności: " + date;

        }
    }
}
