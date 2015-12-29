using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace User
{
    class LoginData
    {
        ArrayList usersData = new ArrayList();

        public LoginData()
        {
            getDataFromFile();
        }

        //Sprawdzenie poprawności loginu i hasła - może powinno być po stronie banku?????????????????
        public bool checkUserData(string login, string password)
        {
            bool verify = false;

            //Stworzenie hasha z podanego przez usera hasła
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            string passwordHash = sBuilder.ToString();
           
            //odszukanie na liscie loginu i sprawdzenie poprawnosci hasha
            List<string> list = new List<string>();
            foreach (string line in usersData)
            {
                list = parse(line.Trim());
                if (list[0] == login)
                {
                    
                    if(list[1] == passwordHash)
                    {
                        Console.WriteLine("Zalogowano");
                        verify = true;
                        break;
                    }                       
                }
            }
            // zwraca true jak jest ok, false jak nie
            return verify;
        }

        //Przypisanie wartosci z pliku tekstowego do listy
        private void getDataFromFile()
        {
            usersData = readData("Users.txt");
        }


        //pobranie wartosci z pliku tekstowego i dodanie do listy
        private static ArrayList readData(string fileName)
        {
            ArrayList data = new ArrayList();
            string[] lines = System.IO.File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                data.Add(line);
            }
            return data;
        }

        //metoda pozwalająca na oddzielenie slow, ktore odzdzielone sa spacja
        private static List<string> parse(string line)
        {
            List<string> datas = new List<string>();
            string[] temp = line.Split(' ');
            foreach (string tmpChar in temp)
            {
                datas.Add(tmpChar);

            }
            return datas;
        }
    }
}
