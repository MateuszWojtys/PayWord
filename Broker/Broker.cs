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
    //Klasa reprezentująca główne okno aplikacji Banku
    public partial class Broker : Form
    {

        Clients clients;
        ClientsData  cd;
        List<List<UserReport>> allReports;
        RSACryptoServiceProvider rsaCSP; 
        RSAParameters publicKey;
        RSAParameters privateKey;
        //Konstruktor głównego okna aplikacji
        public Broker()
        {
            InitializeComponent();
            rsaCSP = new RSACryptoServiceProvider();
            generateOwnKeys();
            clients = new Clients();
            cd = new ClientsData();
            allReports = new List<List<UserReport>>();
            //Wczytanie z pliku XML danych dot. klientów zarejestrowanych w systemie
            clients.readFromXMLUsers(clients.dt, clients.usersData);


            //Nasłuchiwanie na klientów TCP
            Thread tcpServerStartThread = new Thread(new ThreadStart(TcpServerStart));
            tcpServerStartThread.Start();
        }

        private void generateOwnKeys()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string str = rsa.ToXmlString(true);
            string[] tmp = new string[1];
            tmp[0] = str;
            privateKey =  rsa.ExportParameters(true); // prywatny
            publicKey = rsa.ExportParameters(false); // publiczny
            savePublicKey(publicKey);
        }

        private void savePublicKey(RSAParameters publicKey)
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(RSAParameters));
            StringWriter StringWriter = new StringWriter();
            Serializer.Serialize(StringWriter, publicKey);
            System.IO.File.WriteAllText(@"D:\Studia\PKRY\PayWord\BrokerPublicKey.xml", StringWriter.ToString());
        }
        //Metoda odpowiadająca za nasłuchiwanie na klientów TCP
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

        //Metoda odpowiedzialna za obsługę klienta
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
                    byte[] bufor = new byte[1024];
                    GZipStream.Read(bufor, 0, bufor.Length);

                    //Przejście z bitów na stringi
                    StringReader StringReader = new StringReader(UnicodeEncoding.Unicode.GetString(bufor, 0, bufor.Length));

                    XmlSerializer xml = new XmlSerializer(typeof(List<UserReport>));
                    List<UserReport> ur = (List<UserReport>)xml.Deserialize(StringReader);

                    
                    allReports.Add(ur);
                    break;

                    
                default:
                    break;
            }

            nclient.Close();
            
        }

        public struct UserReport
        {
            public Clients.UserCertificate uc;
            public string[] lastPayment;
        }

        //Metoda pozwalająca wysłać do wynik rejestracji - wysyłany jest tylko nagłówek RegistrationVerify
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

        //Metoda pozwalająca na odebranie informacji przesyłanych przez klienta podczas rejestracji
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

        //Wysłanie wyniku weryfikacji logowania
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

        //Metoda odpowiedzialna za odbieranie i tworzenie nowego klienta w systemie banku, przekazywane są dane podane podczas rejestracji
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
            cd = new ClientsData();
            cd.refreshDataGridView(clients.dt);
            
            

        }
        
        // Metoda ktora 'wysyla' klientowi jego certyfikat  -  dla potrzeb symulacji certyfikat, zapisywany jest do pliku, do ktorego ma dostep rowniez klient
        private void sendCertificateToUser(Clients.UserCertificate uc, Clients.UserRegistrationData urd)
        {
            //Serializacja do Xml-a
            XmlSerializer xml = new XmlSerializer(typeof(Clients.UserCertificate));
            StringWriter sw = new StringWriter();
            xml.Serialize(sw, uc);
            string certicicateInXML = sw.ToString();
            //Zapis do pliku - nazwa pliku to login klienta
            string fileName = urd.login ;
            System.IO.File.WriteAllText(@"D:\Studia\PKRY\PayWord\" + fileName + ".xml", certicicateInXML);

            byte[] sign = createSign(certicicateInXML);

            FileStream fs = new System.IO.FileStream(@"D:\Studia\PKRY\PayWord\" + "Sign" + fileName + ".txt", FileMode.Create);
            fs.Write(sign, 0, sign.Length);
            fs.Close();
            

            
        }

        private byte[] createSign(string s)
        {
            UTF8Encoding enc = new UTF8Encoding();
            byte[] data = enc.GetBytes(s);

            SHA1Managed hash = new SHA1Managed();
            byte[] hashedData = null;
            hashedData = hash.ComputeHash(data);
            // RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            rsaCSP.ImportParameters(privateKey);
            return rsaCSP.SignHash(hashedData, CryptoConfig.MapNameToOID("SHA1"));

            /*byte[] sign;
            //Generate a public/private key pair.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //Create an RSAPKCS1SignatureFormatter object and pass it the 
            //RSACryptoServiceProvider to transfer the private key.
            RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(RSA);
            SHA1 sha1 = SHA1.Create();

            //Set the hash algorithm to SHA1.
            RSAFormatter.SetHashAlgorithm("SHA1");

            //Create a signature for HashValue and assign it to 
            //SignedHashValue.
            sign = RSAFormatter.CreateSignature(sha1.ComputeHash(data));*/
            
        }

        //Przeciązona metoda zamykania okna
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            System.Environment.Exit(1);

        }

        
        //Metoda pokazujaca okno z danymi uzytkownikow
        private void linkLabelDataUsers_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {  
            cd = new ClientsData();
            //Ustawienie źródła dla wyświetlanej tabeli
            cd.setSourceForDTV(clients.dt);
            cd.Show();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
       
        
    }
}
