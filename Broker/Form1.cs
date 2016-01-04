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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Broker
{
    public partial class Form1 : Form
    {

        Clients clients;
        ClientsData  cd;
        

        public Form1()
        {
            InitializeComponent();
            clients = new Clients();
            cd = new ClientsData();
            clients.readFromXMLUsers(clients.dt, clients.usersData);
            //Nasłuchiwanie na klientów TCP
            Thread tcpServerStartThread = new Thread(new ThreadStart(TcpServerStart));
            tcpServerStartThread.Start();
        }

        


        private void TcpServerStart()
        {
            //Stworzenie listenera dla dowolnych IP na porcie 5004
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

        private void tcpHandler(object client)
        {
            TcpClient nclient = (TcpClient)client;

            //Stworzenie strumienia od klienta
            NetworkStream stream = nclient.GetStream();

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
            nclient.Close();
            
        }

        //Metoda odpowiedzialna za odbieranie i tworzenie nowego klienta w systemie banku
        public void receiveNewUser(Clients.UserRegistrationData urd)
        {
            clients.createCertificate(urd);
            Clients.UserData ud = clients.createUserData(urd, clients.createCertificate(urd));
            //Ponizej trzeba troche zmienic !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! te 1 i true
            clients.addNewData(1, ud.certificate.userName, ud.urd.login, ud.urd.password, ud.urd.creditCard, true, ud.certificate.expirationDate);
            cd.refreshDataGridView(clients.dt);
            
            

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
            cd.setSourceForDTV(clients.dt);
            cd.Show();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
       
        
    }
}
