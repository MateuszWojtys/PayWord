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
using System.Xml.Serialization;

namespace Vendor
{
    public partial class Vendor : Form
    {
        Users users;
        List<Users.UsersData> usersData;
        List<string[]> issuedCoins;
        List<Report.UserReport> mainReport;
        List<string> userNames;

        
        private RSAParameters getBrokerPublicKey()
        {
            RSAParameters brokerPublicKey;
            string publicKey = System.IO.File.ReadAllText(@"D:\Studia\PKRY\PayWord\BrokerPublicKey.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(RSAParameters));
            StringReader sr = new StringReader(publicKey);
            brokerPublicKey = (RSAParameters) serializer.Deserialize(sr);
            return brokerPublicKey;
        }

        private void updateIssuedCoins(string name, string value)
        {
            string[] tmp = new string[2];
            tmp[0] = name;
            tmp[1] = value;
            issuedCoins.Add(tmp);
        }

        private void generateReport()
        {
            for(int i = 0; i < usersData.Count; i++)
            {
                
                Report.UserReport tmp = new Report.UserReport();
                tmp.lastPayment = new string[2];
                tmp.lastPayment[0] = usersData[i].lastPayment[0];
                tmp.lastPayment[1] = usersData[i].lastPayment[1];
                tmp.uc = usersData[i].certificate;
                
                mainReport.Add(tmp);
            }
        }

        public Vendor()
        {
            InitializeComponent();
            
            users = new Users();
            usersData = new List<Users.UsersData>();
            issuedCoins = new List<string[]>();
            mainReport = new List<Report.UserReport>();
            userNames = new List<string>();
            setSourceForDTV(users.dt);
            //Nasłuchiwanie na klientów TCP
            Thread tcpServerStartThread = new Thread(new ThreadStart(TcpServerStart));
            tcpServerStartThread.Start();
        }

        //Metoda odpowiadająca za nasłuchiwanie na klientów TCP
        private void TcpServerStart()
        {
            
            //Stworzenie listenera dla dowolnych IP na porcie 5000
            TcpListener tcplistener = new TcpListener(IPAddress.Any, 5002);
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
            string userLogin;
            //Obsługa w zależności od nagłówka odebranej wiadomości
            switch (prot)
            {

                //Obsługa gdy odebrany zostanie nagłówek odpowiedzialny za wynik weryfikacji logowania
                case "#Commitment":
                     userLogin = br.ReadString();

                    string xml = br.ReadString();


                    StringReader sr = new StringReader(xml);
                    int signCertificateLength  = br.ReadInt32();
                    byte[] signCertificate;
                    signCertificate = br.ReadBytes(signCertificateLength);


                    //Dekompresja odbieranych danych
                    GZipStream GZipStream = new GZipStream(stream, CompressionMode.Decompress);
                    byte[] bufor = new byte[3000];
                    GZipStream.Read(bufor, 0, bufor.Length);

                    //Przejście z bitów na stringi
                    StringReader StringReader = new StringReader(UnicodeEncoding.Unicode.GetString(bufor, 0, bufor.Length));

                    //Deserializacja odebranego pliku XML - tworzenie zdarzeń
                    XmlSerializer Serializer = new XmlSerializer(typeof(Users.UserCommitment));
                    Users.UserCommitment com = (Users.UserCommitment)Serializer.Deserialize(StringReader);
                    
                    

                    

                    XmlSerializer Serializer1 = new XmlSerializer(typeof(Users.UserCertificate));
                    Users.UserCertificate uc = (Users.UserCertificate)Serializer1.Deserialize(sr);

                    int comLength = br.ReadInt32();
                    byte[] comSign = br.ReadBytes(comLength);
                    //sprawdzenie podpisu commitment - podpisany przez usera
                    bool verCommit = verifyCommitmentSign(com, comSign, uc);
                    //Sprawdzenie podpisu certyfikatu - podpisany przez bank
                    bool verCert = verifyCertificateSign(uc, signCertificate);
                    if (verCert == true && verCommit == true)
                    {
                        if (userNames.Count == 0)
                        {
                            createUserData(uc, com);
                        }
                        else
                        {
                            for (int i = 0; i < userNames.Count; i++)
                            {
                                if (userNames[i] == uc.userName)
                                {
                                    updateUserData(uc, com, findUserData(userNames[i]));
                                }
                                else
                                {
                                    createUserData(uc, com);
                                }
                            }
                        }

                        string s = uc.userName;
                        DataRow foundRow = users.dt.Rows.Find(s);

                        if (foundRow != null)
                        {
                            string[] tmp = new string[2];
                            /*for (int p = 0; p < issuedCoins.Count; p++)
                            {
                                string[] tmp2 = new string[2];
                                tmp2 = issuedCoins[p];
                                if (tmp2[0] == userLogin)
                                {
                                    tmp = issuedCoins[p];
                                }
                            }*/
                            tmp[0] = s;
                            tmp[1] = "0";
                            users.updateDataTable(findUserData(userLogin), tmp);
                        }
                        else
                        {
                            users.addNewRecord(findUserData(userLogin));

                        }

                        refreshDataGridView(users.dt);
                    }
                    else
                    {
                        // Trzeba cos zrobic jak hash sie nie zgadza
                    }
                    break;

                    
                case "#Payment":
                    List<string> payment = new List<string>();
                    userLogin = br.ReadString();
                    payment.Add(br.ReadString());
                    payment.Add(br.ReadString());
                    sendResponse(verifyPayment(userLogin, payment), payment[0]);
                    string[]  temp = new string[2];
                    for (int i = 0; i < issuedCoins.Count; i++ )
                    {
                        string[] tmp2 = new string[2];
                        tmp2 = issuedCoins[i];
                        if(tmp2[0] == userLogin )
                        {
                            temp = issuedCoins[i];
                        }
                    }
                    users.updateDataTable(findUserData(userLogin), temp);
                    refreshDataGridView(users.dt);
                    break;
                


                default:
                    break;
            }

            Console.WriteLine("OSTATNIA PLATNOSC " + usersData[0].lastPayment[0]);
            nclient.Close();

        }

        private bool verifyCommitmentSign(Users.UserCommitment com, byte[] sign, Users.UserCertificate uc)
        {
            
            XmlSerializer xml = new XmlSerializer(typeof(Users.UserCommitment));
            StringWriter comXML = new StringWriter();
            xml.Serialize(comXML, com);
            UTF8Encoding enc = new UTF8Encoding();
            byte[] hashedData = null;
            byte[] wiadomosctmp = enc.GetBytes(comXML.ToString());

            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            SHA1Managed hash = new SHA1Managed();
            RSAParameters tmpRSA = uc.publicKey;
            rsaCSP.ImportParameters(tmpRSA);
            hashedData = hash.ComputeHash(wiadomosctmp);
            Console.WriteLine(rsaCSP.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA1"), sign).ToString());
            return rsaCSP.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA1"), sign);
            
        }
        private bool verifyCertificateSign(Users.UserCertificate uc, byte[] sign)
        {
            
            XmlSerializer xml = new XmlSerializer(typeof(Users.UserCertificate));
            StringWriter certXML = new StringWriter();
            xml.Serialize(certXML, uc);

            UTF8Encoding enc = new UTF8Encoding();
            byte[] hashedData = null;
            byte[] wiadomosctmp = enc.GetBytes(certXML.ToString());

            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            SHA1Managed hash = new SHA1Managed();
            RSAParameters tmp = getBrokerPublicKey();
            rsaCSP.ImportParameters(tmp);
            hashedData = hash.ComputeHash(wiadomosctmp);
            Console.WriteLine(rsaCSP.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA1"), sign).ToString());
            return rsaCSP.VerifyHash(hashedData, CryptoConfig.MapNameToOID("SHA1"),sign);
            
            
        }
        private void sendResponse(bool verify, string coin)
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 5001);
            NetworkStream stream = client.GetStream();
            BinaryWriter br = new BinaryWriter(stream);
            br.Write(Protocol.PAYMENTVERIFY);
            br.Write(verify);
            br.Write(coin);
            stream.Close();
            client.Close();
            

        }
        private void createUserData(Users.UserCertificate uc, Users.UserCommitment com)
        {
            Users.UsersData tmp = new Users.UsersData();
            tmp.lastPayment = new List<string>(); 
            tmp.userName = uc.userName;
            tmp.certificate = uc;
            tmp.commitment = com;
            //tmp.lastPayment.Clear();
            // moneta
            tmp.lastPayment.Add(com.basicCoin);
            //nuemr platnosci
            tmp.lastPayment.Add("0");
            usersData.Add(tmp);
            userNames.Add(tmp.userName);
        }

        private void updateUserData(Users.UserCertificate uc, Users.UserCommitment com, Users.UsersData  ud)
        {
            for (int i = 0; i < usersData.Count; i++)
            {
                if(usersData[i].userName == ud.userName)
                {
                    usersData.RemoveAt(i);
                }
            }
            ud.lastPayment.Clear();
            ud.userName = uc.userName;
            ud.certificate = uc;
            ud.commitment = com;
            ud.lastPayment.Add(com.basicCoin);
            ud.lastPayment.Add("0");
            usersData.Add(ud);
        }

        private bool verifyPayment(string userLogin, List<string> payment)
        {
            Users.UsersData userData = new Users.UsersData();
            userData.lastPayment = new List<string>();
            userData = findUserData(userLogin);
            
            MD5 md5 = MD5.Create();
            
            int difference = (Int32.Parse(payment[1]) - Int32.Parse(userData.lastPayment[1]));
            string tmp = payment[0]; // odebrana moneta
            string hash = null;
            int value = 0;
            for(int i=0; i<difference; i++)
            {
                hash = getMD5Hash(md5, tmp);
                tmp = hash;
                value++;
            }
            bool verify = false;
            if (hash == userData.lastPayment[0])
            {
                verify = true;
                userData.lastPayment[0] = payment[0];
                userData.lastPayment[1] = payment[1];

                updateIssuedCoins(userLogin, value.ToString());
                for (int i = 0; i < usersData.Count; i++)
                {
                    
                    if (userData.userName == usersData[i].userName)
                    {
                        
                        usersData[i] = userData;
                        
                    }
                }
            }

            else
            {
                verify = false;
            }
            return verify;
        }

        private Users.UsersData findUserData(string userName)
        {
            Users.UsersData tmp = new Users.UsersData();
            tmp.lastPayment = new List<string>();
            for(int i = 0; i<usersData.Count; i++)
            {
                if(usersData[i].userName == userName)
                {
                    tmp.userName = usersData[i].userName;
                    tmp.certificate.brokerName = usersData[i].certificate.brokerName;
                    tmp.certificate.expirationDate = usersData[i].certificate.expirationDate;
                    tmp.certificate.userName = usersData[i].certificate.userName;
                    tmp.commitment.basicCoin = usersData[i].commitment.basicCoin;
                    tmp.commitment.date = usersData[i].commitment.date;
                    tmp.commitment.legthOfPayword = usersData[i].commitment.legthOfPayword;
                    tmp.commitment.vendorName = usersData[i].commitment.vendorName;
                    tmp.lastPayment.Add( usersData[i].lastPayment[0]);
                    tmp.lastPayment.Add( usersData[i].lastPayment[1]);
                }   
            }
                return tmp;
        }

        
        //Tworzenie hasha ze stringa
        public string getMD5Hash(MD5 md5Hash, string input)
        {

            // Konwersja stringa na bajty i uzyskanie hasha
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            //przejscie z bajtow z powrotem na stringa (heksadecymalny)
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // zwraca hash - stringa
            return sBuilder.ToString();
        }

        private void setSourceForDTV(DataTable dt)
        {
            dataGridViewUsersData.DataSource = dt;
        }

        public void refreshDataGridView(DataTable dt)
        {
            Func<int> del = delegate()
            {
                dataGridViewUsersData.DataSource = null;
                dataGridViewUsersData.DataSource = dt;

                return 0;
            };
            Invoke(del);
        }

        //Przeciązona metoda zamykania okna
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            System.Environment.Exit(1);
        }

        private void buttonShowReport_Click(object sender, EventArgs e)
        {
            generateReport();
            Report r = new Report(mainReport, userNames);
            r.Show();
           
        }

        private void buttonSendReport_Click(object sender, EventArgs e)
        {
            generateReport();
            sendReport(mainReport);
        }

        private void sendReport(List<Report.UserReport> ur)
        {
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 5000);
            NetworkStream stream = client.GetStream();
            BinaryWriter bw = new BinaryWriter(stream);
            bw.Write(Protocol.REPORT);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Report.UserReport>));
            StringWriter sw = new StringWriter();
            xmlSerializer.Serialize(sw, ur);
            byte[] bufor = UnicodeEncoding.Unicode.GetBytes(sw.ToString());
            MemoryStream ms = new MemoryStream();
            GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true);
            gzip.Write(bufor, 0, bufor.Length);
            gzip.Flush();
            gzip.Close();
            stream.Write(ms.ToArray(), 0, (int)ms.Length);
            ms.Close();

            stream.Close();
            client.Close();

        }
    }
}
