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
        public bool checkUserData(string login, string password)
        {
            bool verify = false;
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            string passwordHash = sBuilder.ToString();
           

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

            return verify;
        }

        private void getDataFromFile()
        {
            usersData = readData("Users.txt");
        }


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
