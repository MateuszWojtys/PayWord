﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Broker
{
    //Klasa reprezentująca główne okno aplikacji Banku
    public partial class Broker : Form
    {

        Clients clients;
        ClientsData  cd;
        
        //Konstruktor głównego okna aplikacji
        public Broker()
        {
            InitializeComponent();
            clients = new Clients();
            cd = new ClientsData();

            //Wczytanie z pliku XML danych dot. klientów zarejestrowanych w systemie
            clients.readFromXMLUsers(clients.dt, clients.usersData);


            //Nasłuchiwanie na klientów TCP
            Thread tcpServerStartThread = new Thread(new ThreadStart(TcpServerStart));
            tcpServerStartThread.Start();
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

                    
                default:
                    break;
            }

            nclient.Close();
            
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
            byte[] bufor = new byte[1024];
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
            //Zapis do pliku - nazwa pliku to login klienta
            string fileName = urd.login + ".xml";
            System.IO.File.WriteAllText(@"D:\Studia\PKRY\PayWord\" + fileName, sw.ToString());

            

            
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