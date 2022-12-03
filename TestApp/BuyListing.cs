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
using System.Text.RegularExpressions;
using System.IO;

namespace TestApp
{
    public partial class BuyListing : Form
    {
        public BuyListing()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) //Card Number Input box
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e) //Card expiration MM/YY
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e) //Card CCV #
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e) // Card Holder Name
        {

        }

        private void button1_Click(object sender, EventArgs e) // Submit button should remove a listing from the menu (and database?) and return the user to the menu
        {
            //input validation
            Regex r = new Regex("[0-9 ]+");
            //TEXTBOX 1 ERROR: it accepts alphabet characters : (
            if (!r.IsMatch(textBox1.Text)) // Card number is 16 integers long and only numeric
            {
                MessageBox.Show("Card number only requires numbers. Try again.");
                return;// terminate early
            }
            if (textBox1.Text.Length != 16)
            {
                MessageBox.Show("The card number consists of a sixteen number length string Ex: 1111222233334444. Try again.");
                return;
            }
            r = new Regex("^[0-9/]+"); //card expiration should only be numbers but for our purposes allowing '/' may also work
            if (!r.IsMatch(textBox2.Text))
            {
                MessageBox.Show("Card expiration should be in the form MM/YY. Try again.");
                return;// terminate early
            }
            if (textBox2.Text.Length != 5)
            {
                MessageBox.Show("The expiration date is in MM/YY format. Try again.");
                return;
            }
            r = new Regex("^[0-9]+"); //CCV is only three integers and only numeric 
            if (!r.IsMatch(textBox3.Text))
            {
                MessageBox.Show("The CCV is only numbers. Try again.");
                return;// terminate early
            }
            if (textBox3.Text.Length != 3)
            {
                MessageBox.Show("The CCV should only be a length of three numbers.");
                return;
            }
            r = new Regex("^[a-zA-Z]+"); //Cardholder names need only accept the alphabet. 
            if (!r.IsMatch(textBox4.Text))
            {
                MessageBox.Show("Cardholder name should only be in letters of the alphabet, no special characters. Try again.");
                return;// terminate early
            }
            if (textBox4.Text.Length > 50)
            {
                MessageBox.Show("Character Limit is 50 letters.");
                return;
            }

            this.Hide();
            new BuyMenu().Show();

        }

        private void label2_Click(object sender, EventArgs e) //pull listing address
        {

        }

        private void label3_Click(object sender, EventArgs e) // pull listing cost
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e) //pull listing photo to present it
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
