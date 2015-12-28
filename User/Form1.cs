﻿using System;
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


namespace User
{
    public partial class Form1 : Form
    {
        PayWord c = new PayWord();
        public Form1()
        {
            InitializeComponent();
            c.generatePayWord(10);
        }


        
        //Metoda łącząca się z Loggerem
        public void connectAndSendToLogger()
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 5000);
            NetworkStream stream = client.GetStream();
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(DateTime.Now.ToString() );
            bw.Write("User");
            bw.Write("Bank");
            bw.Write("Testowy tekst ");
            bw.Close();
            stream.Close();
            client.Close();
            
        }

      
        //Przycisk odpowiedzialny za wysyłanie
        private void button1_Click(object sender, EventArgs e)
        {
            connectAndSendToLogger();
        }
    }
}
