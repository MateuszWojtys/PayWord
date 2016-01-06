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
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.IO.Compression;


namespace User
{
    public partial class User : Form
    {
        //Lista monet
        PayWord.StructPayWord spw = new PayWord.StructPayWord();

        PayWord payWord = new PayWord();
        
        //string przechowująca login podany podczas logowania - wykorzystywane do pobierania informacji o certyfikacie
        string mainLogin = null;

        //Konstruktor głównego okna
        public User()
        {
            InitializeComponent();

            //Wygaszenie niepotrzebnych elementów, które nie powinny być dostępne przez poprawnym zalogowaniem
            dontShowElements();

         

            //Nasłuchiwanie na klientów TCP
            Thread tcpServerStartThread = new Thread(new ThreadStart(TcpServerStart));
            tcpServerStartThread.Start();
        }


        //Metoda odpowiadająca za nasłuchiwanie na klientów TCP
        private void TcpServerStart()
        {
            //Stworzenie listenera dla dowolnych IP na porcie 5001
            TcpListener tcplistener = new TcpListener(IPAddress.Any, 5001);
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
            string prot = br.ReadString(); //odebranie nagłówka

            //Obsługa w zależności od nagłówka odebranej wiadomości
            switch (prot)
            {
                    //Obsługa gdy odebrany zostanie nagłówek odpowiedzialny za wynik weryfikacji logowania
                case "#LogInVerify":
                    string login = br.ReadString();
                    bool verify = br.ReadBoolean();
                    //Pokazanie przycisków itp. dla klienta zalogowanego w przypadku poprawnego wyniku weryfikacji
                    setFormForLoggedUser(verify, login);
                    break;
                //Obsługa gdy odebrany zostanie nagłówek odpowiedzialny za wynik rejestracji
                case "#RegistrationVerify":
                    //Pokazywane jest okno, które informuje i poprawnym wyniku weryfikacji
                    Verify rv = new Verify("Zarejestrowano pomyślnie");
                    rv.ShowDialog();
                    break;

                case "#PaymentVerify":
                    bool ver = br.ReadBoolean();
                    string coin = br.ReadString();
                    Verify v = new Verify("Płatność monetą: " + coin +" została przyjęta");
                    v.ShowDialog();

                    break;


                   

                default:
                    break;
            }
        }

        //Metoda pozwalająca na wygaszenie elementów - wygaszane są gdy klient nie jest zalogowany
        private void dontShowElements()
        {
            buttonWyloguj.Visible = false;
            buttonZaplac.Visible = false;
            labelLogowanie.Visible = false;
            linkLabelCertificate.Visible = false;
            buttonCoinsGeneration.Visible = false;
            comboBoxCoins.Visible = false;
            numericUpDownCoinsNumber.Visible = false;
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

      public struct UserCommitment
      {
          public string vendorName;
          public string basicCoin;
          public string date;
          public string legthOfPayword;
      }

        private UserCommitment generateCommitmentToVendor()
      {
          
          UserCommitment com = new UserCommitment();
          com.vendorName = "Sprzedawca";
          com.basicCoin = spw.basicCoin;
          com.date = DateTime.Now.ToString();
          com.legthOfPayword = spw.payingCoins.Count.ToString();
          return com;
      }

        //Przycisk odpowiedzialny za dokonywanie płatności - przesyłanie monet do sprzedawcy
        private void buttonZaplac_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 5002);
            NetworkStream stream = client.GetStream();
            BinaryWriter bw = new BinaryWriter(stream);
            List<string> payment = makePayment();
            if(payment[1] == "1")
            {
                bw.Write(Protocol.FIRSTPAYMENT);
                
                Certificate c = new Certificate(mainLogin);
                bw.Write(c.getCertificateDatas(mainLogin)[1]);
                bw.Write(c.getCertificate(mainLogin));
                
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserCommitment));
                StringWriter sw = new StringWriter();
                xmlSerializer.Serialize(sw, generateCommitmentToVendor());
                byte[] bufor = UnicodeEncoding.Unicode.GetBytes(sw.ToString());
                MemoryStream ms = new MemoryStream();
                GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true);
                gzip.Write(bufor, 0, bufor.Length);
                gzip.Flush();
                gzip.Close();
                stream.Write(ms.ToArray(), 0, (int)ms.Length);
                ms.Close();
                
                bw.Write(payment[0]);
                bw.Write(payment[1]);
                stream.Close();
            }
            else
            {
                bw.Write(Protocol.PAYMENT);
                Certificate c = new Certificate(mainLogin);
                bw.Write(c.getCertificateDatas(mainLogin)[1]);
                bw.Write(payment[0]);
                bw.Write(payment[1]);
                stream.Close();
            }
            
            client.Close();

            comboBoxCoins.Items.Remove(comboBoxCoins.SelectedItem);
            comboBoxCoins.Text = "Wybierz monetę";
        }

        private List<string> makePayment()
        {
            List<string> payment = new List<string>();
            string text = comboBoxCoins.Text;
            if(text == "Wybierz monetę")
            {
                //WYBierz monete!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }
            else
            {
                
                List<string> datas = new List<string>();
                string[] temp = text.Split(' ');
                foreach (string tmpChar in temp)
                {
                     datas.Add(tmpChar);
                }
                string[] tmp = datas[0].Split('.');

                string coinNumber = tmp[0];
                
                string coin = datas[1];
                payment.Add(coin);
                payment.Add(coinNumber);
            }

            return payment;
            

        }
        //Metoda odpowiedzialna za logowanie
        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            //Pobranie wartosci z textBoxow - login i hasło
            string login = textBoxLogin.Text;
            string password = textBoxHasło.Text;

            mainLogin = textBoxLogin.Text;
            //Połącznie z bankiem
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 5000);
            NetworkStream stream = client.GetStream();
            BinaryWriter bw = new BinaryWriter(stream);
            //Wysłanie nagłówka
            bw.Write(Protocol.LOGIN);
            //Wysłanie loginu
            bw.Write(login);
            //Wysłanie hasha z hasła
            bw.Write(getHashFromPassword(password));
            stream.Close();
            client.Close();    
        }

        //Stworzenie hasha z hasła
        public string getHashFromPassword(string password)
        {
            //Stworzenie hasha z podanego przez usera hasła
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string passwordHash = sBuilder.ToString();
            return passwordHash;
        }

        //Ustawienie okna dla zalogowanego użytkownika, jeśli wynik weryfikacji jest poprawny, w przeciwnym razie
        // klient proszony jest o podanie loginu i hasła jeszcze raz
        private void setFormForLoggedUser(bool checking, string login)
        {
            //Odkrycie elementów i udostepnienie funkcjonalności po poprawnym zalogowaniu
            if (checking == true)
            {
                setLabel(labelLogowanie, "Zalogowano jako " + login);
                Func<int> del = delegate()
                {
                    numericUpDownCoinsNumber.Visible = true;
                    linkLabelCertificate.Visible = true;
                    labelLogowanie.Visible = true;
                    buttonZaplac.Visible = true;
                    buttonWyloguj.Visible = true;
                    buttonCoinsGeneration.Visible = true;
                    comboBoxCoins.Visible = true;
                    labelLogin.Visible = false;
                    labelHasło.Visible = false;
                    textBoxHasło.Visible = false;
                    textBoxLogin.Visible = false;
                    buttonLogIn.Visible = false;
                    linkLabelRejestracja.Visible = false;
                    return 0;
                };
                Invoke(del);

            }
            else
            {
                setTextBox(textBoxLogin, "Błąd! Spróbuj jeszcze raz");
                setTextBox(textBoxHasło, "Błąd! Spróbuj jeszcze raz");
                
            }
        }

        private void textBoxHasło_TextChanged(object sender, EventArgs e)
        {

        }

        //Otwiera okienko do rejestracji
        private void linkLabelRejestracja_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Pokazuje okno do rejestracji
            Registration registration = new Registration();
            registration.ShowDialog();
        }

        //Pozwala na ustawianie tekstu w textboxie 
        public void setTextBox(TextBox t, string s)
        {
            Func<int> del = delegate()
            {
                t.Text = s;
                return 0;
            };
            Invoke(del);
        }

        //Pozwala na ustawianie tekstu w labelach
        public void setLabel(Label l, string s)
        {
            Func<int> del = delegate()
            {
                l.Text = s;
                return 0;
            };
            Invoke(del);
        }

        //Przeciążona metoda zamykania
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            System.Environment.Exit(1);

        }

        //Przycisk wylogowujący z sytemu
        private void buttonWyloguj_Click(object sender, EventArgs e)
        {

            //On=kno powraca do stanu przed zalogowaniem, odpowiednie przyciski są zakrywane i odkrywane
            Func<int> del = delegate()
            {
                textBoxHasło.Clear();
                textBoxLogin.Clear();
                linkLabelCertificate.Visible = false;
                labelLogowanie.Visible = false;
                buttonZaplac.Visible = false;
                labelLogin.Visible = true;
                labelHasło.Visible = true;
                textBoxHasło.Visible = true;
                textBoxLogin.Visible = true;
                buttonLogIn.Visible = true;
                linkLabelRejestracja.Visible = true;
                buttonWyloguj.Visible = false;
                buttonCoinsGeneration.Visible = false;
                comboBoxCoins.Visible = false;
                numericUpDownCoinsNumber.Visible = false;
                return 0;
            };
            Invoke(del);
        }

        //Pokazanie okna z danymi cetryfikatu
        private void linkLabelCertificate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Certificate c = new Certificate(mainLogin);
            c.Show();
        }

        //Przycis pozwalający na wygenerowanie monet
        private void buttonCoinsGeneration_Click(object sender, EventArgs e)
        {
            
            
            int numberOfCoins = (int)numericUpDownCoinsNumber.Value;

            //Tworzenie monet z parametrem ilości tworoznych monet
            if(numberOfCoins == 0 || numberOfCoins <0)
            {
                //OKienko błąd!@!!!!!!!!!!!!!!!!!!!!!!!!!! wybierz ilosc mnet wieksza od 0
            }
            else
            {
                spw = payWord.generatePayWord(numberOfCoins);
                labelCoins.Text = "Wygenerowano " + numberOfCoins + " monet/y.";
            }

            //Dodawanie stworzonych monet do comboBoxa w okienku
            for (int i = 0; i < spw.payingCoins.Count ; i++)
            {
                comboBoxCoins.Items.Add((i+1).ToString() + ". " + spw.payingCoins[i]);
            }

        }
    }
}
