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
    //Okno pokazujące wynik rejestracji
    public partial class Verify : Form
    {
        public Verify( string text)
        {
            InitializeComponent();
            labelVerify.Text = text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
