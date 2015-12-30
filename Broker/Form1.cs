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
        public Form1()
        {
            InitializeComponent();
            Thread tcpServerStartThread = new Thread(new ThreadStart(TcpServerStart));
            tcpServerStartThread.Start();
        }

        // struktura odzwierciedlająca dane użytkownika do rejestracji
        public struct UserRegistrationData
        {
            public string name; //imie
            public string lastName; //nazwisko
            public string creditCard; // nr karty kredytowej
            public string login; // login
            public string password; // hasło
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
            XmlSerializer Serializer = new XmlSerializer(typeof(UserRegistrationData));
            UserRegistrationData urd = (UserRegistrationData)Serializer.Deserialize(StringReader);

            GZipStream.Close();
            stream.Close();
            nclient.Close();
            
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            System.Environment.Exit(1);

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
