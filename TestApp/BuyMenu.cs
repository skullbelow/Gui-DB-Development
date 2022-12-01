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
    public partial class BuyMenu : Form
    {
        public BuyMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)// buy button should take a buyer to the buy form of a selected listing
        {
            this.Hide();
            new BuyListing().Show();
        }

        private void button2_Click(object sender, EventArgs e)//logout button should return the user to the home page
        {
            this.Hide();
            new Login().Show();
        }
    }
}
