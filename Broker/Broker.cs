using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;


namespace Broker
{
    
    /// <summary>
    /// Klasa reprezentująca główne okno aplikacji Banku
    /// </summary>
    public partial class Broker : Form
    {

        Clients clients;
        ClientsData  cd;
        Report report;
        RSACryptoServiceProvider rsaCSP; 
        RSAParameters publicKey; // klucz publiczny banku
        RSAParameters privateKey; // klucz prywatny banku
        
        /// <summary>
        /// Konstruktor głównego okna aplikacji
        /// </summary>
        public Broker()
        {
            InitializeComponent();
            rsaCSP = new RSACryptoServiceProvider();
            
            //wczytywanie własnych kluczy - publiczny i prywatny
            readKeys();
            clients = new Clients();
            cd = new ClientsData();
            report = new Report();
            //Wczytanie z pliku XML danych dot. klientów zarejestrowanych w systemie
            clients.readFromXMLUsers(clients.dt);

            //Nasłuchiwanie na klientów TCP
            Thread tcpServerStartThread = new Thread(new ThreadStart(TcpServerStart));
            tcpServerStartThread.Start();
        }

        #region Obsługa Tcp-klientów
        /// <summary>
        /// Metoda odpowiadająca za nasłuchiwanie na klientów TCP
        /// </summary>
        private void TcpServerStart()
        {
            //Stworzenie listenera dla dowolnych IP na porcie 5000
            TcpListener tcplistener = new TcpListener(IPAddress.Any, 5000);
            tcplistener.Start();

            //Przyjmowanie klientów
            while (true)
            {
                //Akceptacja klienta
                TcpClient client = tcplistener.AcceptTcpClient();

                //Uruchomienie wątku dla klienta
                Thread tcpHandlerThread = new Thread(new ParameterizedThreadStart(tcpHandler));
                tcpHandlerThread.Start(client);

            }
        }

        
        /// <summary>
        /// Metoda odpowiedzialna za obsługę klienta
        /// </summary>
        /// <param name="client"></param> parametrem jest zaakceptowany klient
        private void tcpHandler(object client)
        {
            TcpClient nclient = (TcpClient)client;

            //Stworzenie strumienia od klienta
            NetworkStream stream = nclient.GetStream();
            BinaryReader br = new BinaryReader(stream);
            string prot = br.ReadString();

            //Obsługa w zależności od nagłówka odebranej wiadomości
            switch (prot)
            {
                    //Obsługa żądania zalogowanie do systemu
                case "#LogIn":
                    // odczytanie loginu
                    string login = br.ReadString();
                    //odczytanie hasha z hasła
                    string passwordHash = br.ReadString();
                    //weryfikacja poprawności loginu i hasła
                    bool verify = clients.checkUserData(login, passwordHash);
                    //Wysłanie do klienta wyniku weryfikacji wraz z loginem podanym przez klienta w żądaniu
                    sendLogInVerify(login, verify);
                    break;
                    //Obsługa żądania rejestracji w systemie
                case "#Registration":
                    //Odebranie informacji przesłanych przez klienta  w żądaniu
                    getRegistrationData(stream);
                    //ponowne wczytanie listy haseł
                    clients.getDataFromFile();
                    //Wysłanie wyniku rejestracji
                    sendRegistrationVerify();
                    break;

                    //Obsługa odebrania raportu od sprzedawcy
                case "#Report":
                    //Dekompresja odbieranych danych
                    GZipStream GZipStream = new GZipStream(stream, CompressionMode.Decompress);
                    byte[] bufor = new byte[5000];
                    GZipStream.Read(bufor, 0, bufor.Length);
                    //Przejście z bitów na stringi
                    StringReader StringReader = new StringReader(UnicodeEncoding.Unicode.GetString(bufor, 0, bufor.Length));
                    //Właściwa deserializacja
                    XmlSerializer xml = new XmlSerializer(typeof(List<Clients.UserReport>));
                    List<Clients.UserReport> ur = (List<Clients.UserReport>)xml.Deserialize(StringReader);
                    // Dodanie raportu do listy
                    report.allReports.Add(ur);
                    report.addToComboBox();
                    break;

                default:
                    break;
            }

            nclient.Close();
            
        }

        #endregion

        #region Klucze

        /// <summary>
        /// Nieużywana funkcja, pozwala wygenerowac klucze i zapisac do pliku
        /// </summary>
        private void generateKeys()
        {
            rsaCSP = new RSACryptoServiceProvider();
            string str = rsaCSP.ToXmlString(false);
            string str1 = rsaCSP.ToXmlString(true);
            string[] tmp1 = new string[2];
            tmp1[0] = str;
            string[] tmp2 = new string[1];
            tmp2[0] = str1;
            System.IO.File.WriteAllLines(@"D:\Studia\PKRY\PayWord\Klucze\BrokerPublicKey.txt", tmp1);
            System.IO.File.WriteAllLines(@"D:\Studia\PKRY\PayWord\Klucze\BrokerKeys.txt", tmp2);
        }

        /// <summary>
        /// Wczytywanie własnych kluczy
        /// </summary>
        private void readKeys()
        {

            string[] str = System.IO.File.ReadAllLines(@"D:\Studia\PKRY\PayWord\Klucze\BrokerKeys.txt");
            rsaCSP = new RSACryptoServiceProvider();
            rsaCSP.FromXmlString(str[0]);
            publicKey = rsaCSP.ExportParameters(false);
            privateKey = rsaCSP.ExportParameters(true);

        }
  

        

        #endregion


        #region Rejestracja nowego użytkownika i certyfikat
        /// <summary>
        /// Metoda pozwalająca na odebranie informacji przesyłanych przez klienta podczas rejestracji
        /// </summary>
        /// <param name="stream"></param>
        private void getRegistrationData(NetworkStream stream)
        {
            //Dekompresja odbieranych danych
            GZipStream GZipStream = new GZipStream(stream, CompressionMode.Decompress);
            byte[] bufor = new byte[3000];
            GZipStream.Read(bufor, 0, bufor.Length);

            //Przejście z bitów na stringi
            StringReader StringReader = new StringReader(UnicodeEncoding.Unicode.GetString(bufor, 0, bufor.Length));

            //Deserializacja odebranego pliku XML - tworzenie zdarzeń
            XmlSerializer Serializer = new XmlSerializer(typeof(Clients.UserRegistrationData));
            Clients.UserRegistrationData urd = (Clients.UserRegistrationData)Serializer.Deserialize(StringReader);
            receiveNewUser(urd);

            //Zapisywanie do XMLa danych uzytkownikow
            clients.writeToXMLUsers(clients.dt);


            GZipStream.Close();
            stream.Close();
            
        }

       
        /// <summary>
        /// Metoda odpowiedzialna za odbieranie i tworzenie nowego klienta w systemie banku podczas rejestracji 
        /// </summary>
        /// <param name="urd"></param> // dane podane podczas rejestracji w strukturze UserRegistrationData
        public void receiveNewUser(Clients.UserRegistrationData urd)
        {
            //Tworzony jest certyfikat dla klienta
            Clients.UserCertificate tmp = clients.createCertificate(urd);
            //"Wysyłany" jest certyfikat dla klienta - zapis do pliku, do którego ma dostęp klient
            sendCertificateToUser(tmp, urd);
            //Stworzenie UserData, która odzwierciedla dane klienta
            Clients.UserData ud = clients.createUserData(urd, clients.createCertificate(urd));
            //Dodanie nowego klienta do wyświetlanej tabeli
            clients.addNewData(ud.certificate.userName, ud.urd.login, ud.urd.password, ud.urd.creditCard, ud.certificate.expirationDate);
            //Dodanie loginy i hasha z hasła do pliku 
            clients.addToPasswordList(ud.urd.login, ud.urd.password);
            //Odświeżenie tabeli
            cd = new ClientsData();
            cd.refreshDataGridView(clients.dt);
        }

        /// <summary>
        /// Metoda pozwalająca wysłać do Klienta wynik rejestracji - wysyłany jest tylko nagłówek RegistrationVerify
        /// </summary>
        private void sendRegistrationVerify()
        {
            try
            {
                TcpClient tmpClient = new TcpClient();
                tmpClient.Connect("127.0.0.1", 5001);
                NetworkStream tmpStream = tmpClient.GetStream();
                BinaryWriter tmpBr = new BinaryWriter(tmpStream);
                tmpBr.Write(Protocol.REGISTRATIONVERIFY);
                tmpStream.Close();
                tmpClient.Close();
            }
            catch(Exception e)
            {
                //Można bląd wyslac do loggera????????????????????????????????????????????
            }
        }


        /// <summary>
        /// Metoda ktora 'wysyla' klientowi jego certyfikat  -  dla potrzeb symulacji certyfikat, 
        /// zapisywany jest do pliku, do ktorego ma dostep rowniez klient
        /// </summary>
        /// <param name="uc"></param> // certyfikat Usera
        /// <param name="urd"></param> // dane podane podczas rejestracji
        private void sendCertificateToUser(Clients.UserCertificate uc, Clients.UserRegistrationData urd)
        {
            //Serializacja do Xml-a
            XmlSerializer xml = new XmlSerializer(typeof(Clients.UserCertificate));
            StringWriter sw = new StringWriter();
            xml.Serialize(sw, uc);
            // certyfikat jako xml jako string
            string certicicateInXML = sw.ToString();
            //Zapis do pliku - nazwa pliku to login klienta
            string fileName = urd.login ;
            System.IO.File.WriteAllText(@"D:\Studia\PKRY\PayWord\" + fileName + ".xml", certicicateInXML);
            //Podpisywanie certyfikatu przez Bank
            byte[] sign = createSign(certicicateInXML);
            //Zapis podpisu do pliku
            FileStream fs = new System.IO.FileStream(@"D:\Studia\PKRY\PayWord\" + "Sign" + fileName + ".txt", FileMode.Create);
            fs.Write(sign, 0, sign.Length);
            fs.Close();
        }

        /// <summary>
        /// Tworzenie podpisu danej wiadomosci(stringa) - wykorzystywane podczas podpisu certyfikatu
        /// </summary>
        /// <param name="s"></param> // wiadomosc do podpisania
        /// <returns></returns>
        private byte[] createSign(string s)
        {
            UTF8Encoding enc = new UTF8Encoding();
            byte[] data = enc.GetBytes(s);
            SHA1Managed hash = new SHA1Managed();
            byte[] hashedData = null;
            hashedData = hash.ComputeHash(data);
            rsaCSP.ImportParameters(privateKey);
            return rsaCSP.SignHash(hashedData, CryptoConfig.MapNameToOID("SHA1"));  
        }


        #endregion



        /// <summary>
        /// Wysłanie wyniku weryfikacji logowania
        /// </summary>
        /// <param name="login"></param> // wysyłany jest login przy użyciu jakiego logował się klient
        /// <param name="verify"></param> // wynik weryfikacji
        private void sendLogInVerify(string login, bool verify)
        {
            TcpClient tmpClient = new TcpClient();
            tmpClient.Connect("127.0.0.1", 5001);
            NetworkStream tmpStream = tmpClient.GetStream();
            BinaryWriter tmpBr = new BinaryWriter(tmpStream);
            //Nagłówek
            tmpBr.Write(Protocol.LOGINVERIFY);
            //Login podany podczas żadania logowania
            tmpBr.Write(login);
            //Wynik weryfikacji
            tmpBr.Write(verify);
            tmpStream.Close();
            tmpClient.Close();
        }



        /// <summary>
        /// Przeciążona metoda zamykania okna 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            System.Environment.Exit(1);
        }

        
     
        /// <summary>
        /// Metoda pokazujaca okno z danymi uzytkownikow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelDataUsers_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {  
            cd = new ClientsData();
            //Ustawienie źródła dla wyświetlanej tabeli
            cd.setSourceForDTV(clients.dt);
            cd.Show();
            
        }

        private void linkLabelReports_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            report.Show();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
       
        
    }
}
