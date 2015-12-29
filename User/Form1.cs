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


namespace User
{
    public partial class Form1 : Form
    {
        PayWord payWord = new PayWord();
        LoginData logData = new LoginData();

        public Form1()
        {
            InitializeComponent();
            disposeElements();
            //c.generatePayWord(10);
        }

        private void disposeElements()
        {
            buttonWyslij.Dispose();
            labelLogowanie.Dispose();
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
        private void buttonWyslij_Click(object sender, EventArgs e)
        {
            connectAndSendToLogger();
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxHasło.Text;
            bool checking = logData.checkUserData(login, password);


            if (checking == true)
            {
                
                labelLogowanie.Text = ("Zalogowano jako " + login);
                
                labelLogowanie.Show();
                labelLogin.Dispose();
                labelHasło.Dispose();
                textBoxHasło.Dispose();
                textBoxLogin.Dispose();
                buttonLogIn.Dispose();
            }
            else
            {
                textBoxLogin.Text = "Błąd! Spróbuj jeszcze raz";
                textBoxHasło.Text = "Błąd! Spróbuj jeszcze raz";
            }
        }

        private void textBoxHasło_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabelRejestracja_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration registration = new Registration();
            registration.ShowDialog();
        }
    }
}
