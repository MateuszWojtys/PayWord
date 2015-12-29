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
    public partial class Registration : Form
    {

        public struct UserRegistrationData
        {
            public string name;
            public string lastName;
            public string creditCard;
            public string login;
            public string password;
        }

        public Registration()
        {
            InitializeComponent();
        }

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


        private void buttonAnuluj_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
