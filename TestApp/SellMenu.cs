using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp
{
    public partial class SellMenu : Form
    {
        private int accountID;
        public SellMenu(int aID)
        {
            this.accountID = aID;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //Sell button should lead the user to the create listing form for a listing to be created.
        {
            this.Hide();
            new CreateListing(accountID).Show();
        }

        private void button2_Click(object sender, EventArgs e) // Logout button should return the user to the login form
        {
            this.Hide();
            new Login().Show();
        }
    }
}
