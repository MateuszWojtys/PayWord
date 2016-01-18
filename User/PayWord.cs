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
        public struct StructPayWord
        {
            public string basicCoin;
            public List<string> payingCoins;
        }

        
        /// <summary>
        /// Metoda pozwalająca na wygenerowanie łańcucha monet z parametrem, który mówi o długości łańcucha (ilość monet)
        /// </summary>
        /// <param name="payWordLength"></param> ilosc monet
        /// <returns></returns>
        public StructPayWord generatePayWord(int payWordLength)
        {
           //Tworzenie klasy odpowiedzialnej za algorytm MD5
           MD5 md5 = MD5.Create();

           //Tworzenie listy - łańcucha monet
           StructPayWord spw = new StructPayWord();
           spw.basicCoin = null;
           spw.payingCoins = new List<string>();
           //Wylosowanie liczby
           string firstNumber = getRandomNumber();
           //Utworzenie hasha z liczby
           string hash = getMD5Hash(md5, firstNumber);
           //Dodanie do łańcucha monety
           spw.payingCoins.Add(hash); 

           Console.WriteLine("Oto hash  :  " + hash + " z " + firstNumber);
           Thread.Sleep(100);
           Console.WriteLine("Ilosc monet przekazywanych: " + payWordLength);
            //Tworzenie i dodawanie kolejnych monet
            for(int i=0; i<payWordLength; i++)
            {
                string tmp;
                if(i == payWordLength-1)
                {
                     tmp = hash;
                    hash = getMD5Hash(md5, tmp);
                    spw.basicCoin = hash;
                    Console.WriteLine("Oto hash  :  " + hash + " z " + tmp);
                }
                else
                {
                 tmp = hash;
                hash = getMD5Hash(md5, tmp);
                Console.WriteLine("Oto hash  :  " + hash + " z " + tmp);
                spw.payingCoins.Add(hash);
                Console.WriteLine(i);
                }

            }
            // Odwrocenie kolejnosci monet
            for (int i = 0; i < spw.payingCoins.Count; i ++ )
            {
                Console.WriteLine("Moneta " + i + " " + spw.payingCoins[i]);
            }
                spw.payingCoins = rotateList(spw.payingCoins);
                for (int i = 0; i < spw.payingCoins.Count; i++)
                {
                    Console.WriteLine("Po odwroceniu moneta " + i + " " + spw.payingCoins[i]);
                }
            return spw;
        }

        /// <summary>
        /// Metoda odwracajaca elementy w liscie
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private List<string> rotateList(List<string> input)
        {
            List<string> output = new List<string>();
            for (int i = input.Count; i > 0; i--)
            {
                output.Add(input[i-1]);
            }

            return output;

        }
      
        /// <summary>
        /// Metoda pozwalająca na wylosowanie pierwszej liczby do łańcucha - liczba jest zwracana
        /// </summary>
        /// <returns></returns>
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

        
        /// <summary>
        /// Metoda pozwalająca na uzyskanie hasha z stringa
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public  string getMD5Hash(MD5 md5Hash, string input)
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

    }
}
