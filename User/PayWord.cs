using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Threading;
namespace User
{
    public class PayWord
    {

        //Metoda pozwalająca na wygenerowanie łańcucha monet z parametrem, który mówi o długości łańcucha (ilość monet)
        public void generatePayWord(int payWordLength)
        {
           //Tworzenie klasy odpowiedzialnej za algorytm MD5
           MD5 md5 = MD5.Create();

           //Tworzenie listy - łańcucha monet
           List<string> payWord = new List<string>();

           //Wylosowanie liczby
           string firstNumber = getRandomNumber();
           //Utworzenie hasha z liczby
           string hash = getMD5Hash(md5, firstNumber);
           //Dodanie do łańcucha monety
           payWord.Add(hash);

           Console.WriteLine("Oto hash  :  " + hash + " z " + firstNumber);
           Thread.Sleep(100);

            //Tworzenie i dodawanie kolejnych monet
            for(int i=0; i<payWordLength; i++)
            {
                string tmp = hash;
                hash = getMD5Hash(md5, tmp);
                Console.WriteLine("Oto hash  :  " + hash + " z " + tmp);
                payWord.Add(hash);
            }
            
        }

        //Metoda pozwalająca na wylosowanie pierwszej liczby do łańcucha - liczba jest zwracana
        static string getRandomNumber()
        {
            string randomNumber = null;
            Random rnd = new Random();

            //Stworzenie losowej liczby jako stringa, który składa się z 10 kolejnych liczb losowych (od 0 do 9)
            for (int i = 0; i < 10; i++ )
            {
                randomNumber = randomNumber + rnd.Next(10).ToString();
            }

            return randomNumber;
        }

        //Metoda pozwalająca na uzyskanie hasha z stringa
        public  string getMD5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
