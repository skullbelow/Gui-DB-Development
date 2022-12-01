using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace TestApp
{
    public partial class SellMenu : Form
    {
        private int account;
        public SellMenu(int aID)
        {
            this.account = aID;
            InitializeComponent();


            //after input validation we need to save the input as a new entry in the Listing table
            SQLiteConnection con = new SQLiteConnection(@"data source = nAccountDb.db");
            con.Open();
            SQLiteCommand cmnd = new SQLiteCommand();
            cmnd.Connection = con;
            cmnd.Connection = con;
        }

        private void button1_Click(object sender, EventArgs e) //Sell button should lead the user to the create listing form for a listing to be created.
        {
            this.Hide();
            new CreateListing(account).Show();
        }

        private void button2_Click(object sender, EventArgs e) // Logout button should return the user to the login form
        {
            this.Hide();
            new Login().Show();
        }
    }
}
