using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions; // added for input validation (stopping SQL injections)
using TestApp.Control;
using TestApp.Entity;

namespace TestApp
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {

            if (!LoginControl.IsValid(textBox1.Text, textBox2.Text))
            {
                MessageBox.Show("Credentials can only be alphanumeric. Try again.");
                return;// terminate early
            }




            Account acc = LoginControl.ObtainUser(textBox1.Text, textBox2.Text);

            if (acc.getAccountID() == -500) // -500 is the flag value that indicates the user entered in wrong 
            {
                MessageBox.Show("Incorrect Credentials. Try again");
                return;// terminate early
            }



            if (acc.getAccountType().Equals("buyer")) // Take them to buyer menu
            {
                LoginControl.AddLoginToDB(acc);
                this.Hide();
                new BuyMenu(acc).Show();
            }
            else if (acc.getAccountType().Equals("seller")) // Take them to seller menu
            {
                LoginControl.AddLoginToDB(acc);
                this.Hide();
                new SellMenu(acc).Show();//no longer throws login error
            }
            else // This should theoretically never trigger, but for completeness..
            {
                MessageBox.Show("Something went wrong. Click 'Exit' button and try again.");
            }



        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }

    
}
