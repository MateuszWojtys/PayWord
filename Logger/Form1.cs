using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;

namespace Logger
{
    public partial class Form1 : Form
    {
        private int  logNumber=0; //Ilosc odebranych logow
        private int portTCP = 5000; // port TCP na którym nasłuchuje TCPListener aplikacji
        DataTable dt; // DataTable przechowujaca logi
      
        //Struktura odzwierciedlająca log 
        public struct Log
        {
            public string date;
            public string source;
            public string destination;
            public string message;
        }
        
        //Updatowanie (źródła DataGridView) wyświetlanej tabeli
        private void updateDataGridView()
        {
            Func<int> del = delegate()
            {
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.AutoResizeColumns();
                return 0;
            };
            Invoke(del);
        }
       

        //Konstruktor głównego okna aplikacji - Loggera
        public Form1()
        {
            InitializeComponent();
            makeTable();
            
            Thread tcpServerStartThread = new Thread(new ThreadStart(TCPServerStart));
            tcpServerStartThread.Start();
          
        }

        //Właściwe uruchomienie serwera
        private void TCPServerStart()
        {
            //Stworzenie listenera dla dowolnych IP na porcie portTCP
            TcpListener tcplistener = new TcpListener(IPAddress.Any, portTCP);
            tcplistener.Start();

            

            //Przyjmowanie klientów
            while (true)
            {
                //Akceptacja klienta
                TcpClient client = tcplistener.AcceptTcpClient();

                //Uruchomienie wątku dla klienta
                Thread tcpHandlerThread = new Thread(new ParameterizedThreadStart(TCPHandler));
                tcpHandlerThread.Start(client);
                
            }

            
        }

        //Obsługa klienta - odbieranie zdarzenia
        private void TCPHandler(object client)
        {
            TcpClient nclient = (TcpClient)client;

            //Stworzenie strumienia od klienta
            NetworkStream stream = nclient.GetStream();
            
            addLog(getLog(stream));
            
            //Zamkniecie strumieni
            stream.Close();
            nclient.Close();

        }

        //Stworzenia tabeli do wyświetlania wiadomości
        public void makeTable()
        {
            dt = new DataTable("Powiadomienia");

            //Tworzenie kolumn tabeli
            DataColumn logID = new DataColumn("ID", typeof(int));
            DataColumn date = new DataColumn("Data", typeof(string));
            DataColumn source = new DataColumn("Nadawca", typeof(string));
            DataColumn destination = new DataColumn("Odbiorca", typeof(string));
            DataColumn message = new DataColumn("Wiadomość", typeof(string));
            
            //Dodawanie kolumn do tabeli
            dt.Columns.Add(logID);
            dt.Columns.Add(date);
            dt.Columns.Add(source);
            dt.Columns.Add(destination);
            dt.Columns.Add(message);
            
            //Ustawienie jako źródła do wyświetlania DataTable dt
            
            dataGridView1.DataSource = dt;

        }

        //Dodanie wiersza (logu) do tabeli
        public void addLog(Log l)
        {
            logNumber++;
            DataRow dr = dt.NewRow();
            dr["ID"] = logNumber;
            dr["Data"] = l.date;
            dr["Nadawca"] = l.source;
            dr["Odbiorca"] = l.destination;
            dr["Wiadomość"] = l.message;

            dt.Rows.Add(dr);

            updateDataGridView();
            
        }


        //Odbieranie logu - informacji od innej aplikacji
        public Log getLog(NetworkStream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            Log l = new Log();
            l. date = br.ReadString();
            l.source = br.ReadString();
            l.destination = br.ReadString();
            l.message = br.ReadString();
            br.Close();
            return l;
        }

        //Przeciązona metoda zamykania okienka
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
