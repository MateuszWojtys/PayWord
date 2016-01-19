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
    /// <summary>
    /// Klasa odpowiadajaca aplikacji Usera
    /// </summary>
    public partial class User : Form
    {
        PayWord payWord = new PayWord();
        //Lista monet
        PayWord.StructPayWord spw = new PayWord.StructPayWord();
        RSACryptoServiceProvider rsaCSP;
        public RSAParameters publicKey;
        public RSAParameters privateKey;

        //string przechowujący login podany podczas logowania - wykorzystywane do pobierania informacji o certyfikacie
        string mainLogin = null;

        /// <summary>
        /// Struktura odpowiedzialna za commitment użytkownika wysyłany do sklepu
        /// </summary>
        public struct UserCommitment
        {
            public string vendorName;//nazwa sklepu
            public string basicCoin;//podstawowa moneta payword'a
            public string date; //data wysłania
            public string legthOfPayword;//długość payword'a

        }

        
        /// <summary>
        /// Konstruktor głównego okna
        /// </summary>
        public User()
        {
            InitializeComponent();
            //Wygaszenie niepotrzebnych elementów, które nie powinny być dostępne przez poprawnym zalogowaniem
            dontShowElements();
            rsaCSP = new RSACryptoServiceProvider();
            
            //Nasłuchiwanie na klientów TCP
            Thread tcpServerStartThread = new Thread(new ThreadStart(TcpServerStart));
            tcpServerStartThread.Start();
        }

        

        
        /// <summary>
        /// Metoda odpowiadająca za nasłuchiwanie na klientów TCP
        /// </summary>
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

        
        /// <summary>
        /// /Metoda odpowiedzialna za obsługę klienta
        /// </summary>
        /// <param name="client"></param>
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
                    connectAndSendToLogger("-", "Wynik weryfikacji logowania użytkownika" + login +" jest równy " + verify.ToString());
                    if(verify == true)
                    {
                        readKeys();
                    }
                    
                    //Pokazanie przycisków itp. dla klienta zalogowanego w przypadku poprawnego wyniku weryfikacji
                    setFormForLoggedUser(verify, login);
                    
                    break;
                //Obsługa gdy odebrany zostanie nagłówek odpowiedzialny za wynik rejestracji
                case "#RegistrationVerify":

                    connectAndSendToLogger("-", "Wynik rejesetracji w systemi jest pozytywny");
                    //Pokazywane jest okno, które informuje i poprawnym wyniku weryfikacji
                    Verify rv = new Verify("Zarejestrowano pomyślnie");
                    rv.ShowDialog();
                    break;
                    //Obsluga gdy odebrany zostanie wynik weryfikacji płatności
                case "#PaymentVerify":
                    bool ver = br.ReadBoolean();
                    string coin = br.ReadString();
                    Verify v;
                    if (ver == true)
                    {
                         v = new Verify("Płatność monetą: " + coin + " została przyjęta");
                    }
                    else
                    {
                         v = new Verify("Płatność monetą: " + coin + " nie została przyjęta");
                    }
                    v.ShowDialog();
                    connectAndSendToLogger("-", "Wynik weryfikacji płatności monetą " + coin + " jest równy " + ver);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Wczytywanie własnych kluczy
        /// </summary>
        private void readKeys()
        {
            
            string[] str = System.IO.File.ReadAllLines(@"D:\Studia\PKRY\PayWord\Klucze\" + mainLogin + "BrokerKeys.txt");
            rsaCSP = new RSACryptoServiceProvider();
            rsaCSP.FromXmlString(str[0]);
            publicKey = rsaCSP.ExportParameters(false);
            privateKey = rsaCSP.ExportParameters(true);
            connectAndSendToLogger("-", "Wczytałem klucze użytkownika " + mainLogin);
        }
        
        /// <summary>
        /// Metoda pozwalająca na wygaszenie elementów - wygaszane są gdy klient nie jest zalogowany
        /// </summary>
        private void dontShowElements()
        {
            buttonWyloguj.Visible = false;
            buttonZaplac.Visible = false;
            labelLogowanie.Visible = false;
            linkLabelCertificate.Visible = false;
            buttonCoinsGeneration.Visible = false;
            comboBoxCoins.Visible = false;
            numericUpDownCoinsNumber.Visible = false;
            buttonCommitment.Visible = false;
        }
        
        
        /// <summary>
        /// Metoda łącząca się z Loggerem i wysylajaca Log
        /// </summary>
        public void connectAndSendToLogger(string destination, string message)
        {
            try
            {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 6000);
            NetworkStream stream = client.GetStream();
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(DateTime.Now.ToString() );
            bw.Write("User");
            bw.Write(destination);
            bw.Write(message);
            bw.Close();
            stream.Close();
            client.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }

      /// <summary>
      /// Generowanie Commitment do Sprzedawcy
      /// </summary>
      /// <returns></returns>
        private UserCommitment generateCommitmentToVendor()
      {
          UserCommitment com = new UserCommitment();
          com.vendorName = "Sprzedawca";
          com.basicCoin = spw.basicCoin;
          com.date = DateTime.Now.ToString();
          com.legthOfPayword = spw.payingCoins.Count.ToString();
          connectAndSendToLogger("-", "Wygenerowany został commitment użytkownika " + mainLogin);
          return com;
      }

        
        /// <summary>
        /// Przycisk odpowiedzialny za dokonywanie płatności - przesyłanie monet do sprzedawcy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonZaplac_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 5002);
            NetworkStream stream = client.GetStream();
            BinaryWriter bw = new BinaryWriter(stream);
            List<string> payment = makePayment();
            
                bw.Write(Protocol.PAYMENT);
                Certificate c = new Certificate(mainLogin);
                bw.Write(c.getCertificateDatas(mainLogin)[1]);
                bw.Write(payment[0]);
                bw.Write(payment[1]);
                stream.Close();
            
            
            client.Close();
            connectAndSendToLogger("Vendor", "Wysłana została platnosc nr " + payment[1] + " moneta " + payment[0]);
            comboBoxCoins.Items.Remove(comboBoxCoins.SelectedItem);
            comboBoxCoins.Text = "Wybierz monetę";
        }

        /// <summary>
        /// Wybieranie monety do płątności
        /// </summary>
        /// <returns></returns>
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

                string s = comboBoxCoins.Items[0].ToString();
                string[] x = s.Split(' ');
                x = x[0].Split('.');
                int p = Convert.ToInt32(x[0]);
                if(p < Convert.ToInt32(coinNumber))
                {
                    comboBoxCoins.Items.RemoveAt(0);
                }
                
            }

            return payment;
            

        }
        /// <summary>
        /// Wysyłanie commitment so Sprzedawcy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCommitment_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 5002);
            NetworkStream stream = client.GetStream();
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(Protocol.COMMITMENT);

            Certificate c = new Certificate(mainLogin);
            bw.Write(c.getCertificateDatas(mainLogin)[1]);
            bw.Write(c.getCertificate(mainLogin));
            Certificate.Sign sign = c.getCertificateSign(mainLogin);
            
            //podpis certyfikatu
            bw.Write(sign.length); //  dlugosc podpisu
            bw.Write(sign.sign); // podpis

            // commitment
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserCommitment));
            StringWriter sw = new StringWriter();
            UserCommitment tmpCom = generateCommitmentToVendor();
            xmlSerializer.Serialize(sw, tmpCom);
            byte[] bufor = UnicodeEncoding.Unicode.GetBytes(sw.ToString());
            MemoryStream ms = new MemoryStream();
            GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true);
            gzip.Write(bufor, 0, bufor.Length);
            gzip.Flush();
            gzip.Close();
            stream.Write(ms.ToArray(), 0, (int)ms.Length);
            ms.Close();

            byte[] commitmentSign =  createSign(sw.ToString());
            connectAndSendToLogger("-", "Wygenerowany commitment został podpisany przy użyciu klucza prywatnego użytkownika " + mainLogin);
            bw.Write(commitmentSign.Length);
            bw.Write(commitmentSign);
            client.Close();
            connectAndSendToLogger("Vendor", "Do sprzedawcy wysłany został commitment użytkownika " + mainLogin);
            buttonZaplac.Visible = true;
        }

        /// <summary>
        /// Tworzenie podpisu
        /// </summary>
        /// <param name="s"></param>wiadomosc do podpisaania
        /// <returns></returns>
        private byte[] createSign(string s)
        {

            UTF8Encoding enc = new UTF8Encoding();
            byte[] data = enc.GetBytes(s);

            SHA1Managed hash = new SHA1Managed();
            byte[] hashedData = null;
            hashedData = hash.ComputeHash(data);
            //RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            rsaCSP.ImportParameters(privateKey);
            return rsaCSP.SignHash(hashedData, CryptoConfig.MapNameToOID("SHA1"));
        }

        
        /// <summary>
        /// Metoda odpowiedzialna za logowanie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            //Pobranie wartosci z textBoxow - login i hasło
            string login = textBoxLogin.Text;
            string password = textBoxHasło.Text;//textBoxHasło.Text;

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
            connectAndSendToLogger("Broker", "Do banku wysłany został login" + login + " wraz z hashem " + getHashFromPassword(password) + " z hasła");
        }

        
        /// <summary>
        /// Stworzenie hasha z hasła
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
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
            connectAndSendToLogger("-", "Wygenerowany został hash z podanego hasła: " + password);
            return passwordHash;
            
        }

        
        /// <summary>
        /// Ustawienie okna dla zalogowanego użytkownika, jeśli wynik weryfikacji jest poprawny,
        /// w przeciwnym razie  klient proszony jest o podanie loginu i hasła jeszcze raz
        /// </summary>
        /// <param name="checking"></param> wynik weryfikacji
        /// <param name="login"></param> login
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
                    buttonZaplac.Visible = false;
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
                setTextBox(textBoxHasło, " ");
                
            }
        }

        

        
        /// <summary>
        /// Otwiera okienko do rejestracji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelRejestracja_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Pokazuje okno do rejestracji
            Registration registration = new Registration();
            registration.ShowDialog();
        }

         
        /// <summary>
        /// Pozwala na ustawianie tekstu w textboxie 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="s"></param>
        public void setTextBox(TextBox t, string s)
        {
            Func<int> del = delegate()
            {
                t.Text = s;
                return 0;
            };
            Invoke(del);
        }

        
        /// <summary>
        /// Pozwala na ustawianie tekstu w labelach
        /// </summary>
        /// <param name="l"></param>wybrany label
        /// <param name="s"></param>tekst do ustawienia
        public void setLabel(Label l, string s)
        {
            Func<int> del = delegate()
            {
                l.Text = s;
                return 0;
            };
            Invoke(del);
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
        /// Przycisk wylogowujący z sytemu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWyloguj_Click(object sender, EventArgs e)
        {

            //Okno powraca do stanu przed zalogowaniem, odpowiednie przyciski są zakrywane i odkrywane
            Func<int> del = delegate()
            {
                textBoxHasło.Clear();
                textBoxLogin.Clear();
                labelCoins.Visible = false;
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
                buttonCommitment.Visible = false;
                comboBoxCoins.Items.Clear();
                connectAndSendToLogger("-","Użytkownik " + mainLogin + " wylogował się");
                return 0;
            };
            Invoke(del);
        }

        
        /// <summary>
        /// Pokazanie okna z danymi cetryfikatu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelCertificate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            Certificate c = new Certificate(mainLogin);
            c.Show();
        }

        
        /// <summary>
        /// Przycisk pozwalający na wygenerowanie monet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            buttonCommitment.Visible = true;
        }

         private void textBoxHasło_TextChanged(object sender, EventArgs e)
        {

        }
    }
   
}
