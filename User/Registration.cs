using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace User
{
    //Okienko pozwalające na wpisanie danych potrzebnych do rejestracji
    public partial class Registration : Form
    {

        // struktura odzwierciedlająca dane użytkownika do rejestracji
        public struct UserRegistrationData
        {
            public string name; //imie
            public string lastName; //nazwisko
            public string creditCard; // nr karty kredytowej
            public string login; // login
            public string password; // hasło
        }

        //Konstruktor okienka rejestracji
        public Registration()
        {
            InitializeComponent();
        }

        //Pozwala na sprawdzenie poprawności danych wpisanych przez usera (głównie czy pola nie są puste)
        // Sprawdzenie czy powtorzone haslo jest takie samo jak haslo poprzednie
        // zwraca true jak jest wszystko ok, false jak nie
        public bool checkRegistrationData()
        {
            bool check = true;
            if (textBoxImie.Text == "")
            {
                textBoxImie.Text = "Wypełnij pole!";
                check = false;
            }
            if (textBoxNazwisko.Text == "")
            {
                textBoxNazwisko.Text = "Wypełnij pole!";
                check = false;
            }
            if (textBoxKartaKredytowa.Text == "")
            {
                textBoxKartaKredytowa.Text = "Wypełnij pole!";
                check = false;
            }
            if (textBoxLogin.Text == "")
            {
                textBoxLogin.Text = "Wypełnij pole!";
                check = false;
            }
            if (textBoxHaslo.Text == "")
            {
                textBoxHaslo.Text = "Wypełnij pole!";
                check = false;
            }
            if(textBoxHaslo.Text != textBoxHaslo2.Text)
            {
                textBoxHaslo.Text = "Błąd!";
                textBoxHaslo2.Text = "Błąd!";
                check = false;
            }

            return check;
        }

        //Metoda pozwalająca na pobranie danych z textBoxow - stworzenie nowej struktury UserRegistrationData
        private UserRegistrationData getRegistrationData()
        {
            UserRegistrationData urd = new UserRegistrationData();
   
            urd.name = textBoxImie.Text;
            urd.lastName = textBoxNazwisko.Text;
            urd.creditCard = textBoxKartaKredytowa.Text;
            urd.login = textBoxLogin.Text;
            urd.password = textBoxHaslo.Text;
            return urd;
        }


        //Zamyka okienko po naciśnięciu Anuluj
        private void buttonAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Pozwala na rejestracje - wysłanie do banku ........????????????????????????????? Trzeba dokonczyc
        private void buttonZarejestruj_Click(object sender, EventArgs e)
        {
            bool tmp = checkRegistrationData();
            if (tmp == true)
            {
                UserRegistrationData urd = getRegistrationData();
                Console.WriteLine(urd.name);
            }
            
        }
    }
}
