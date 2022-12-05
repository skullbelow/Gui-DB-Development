using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp.Entity
{
    public class Listing
    {
        private int id;
        private int sellerid;
        private string address;
        private int price;

        // These are used to make displaying in Sell Menu form more simple 
        public PictureBox pictureBox;
        public TextBox adressTextBox;
        public TextBox costTextBox;
        public RadioButton radioButton;
        public TextBox rooms;
        public TextBox bathRooms;


        public Listing(int Id, int Sellerid, string Address, int Price, PictureBox pb, TextBox aTB, TextBox cTB, RadioButton rb=null, TextBox rms = null, TextBox bthRms=null)
        {
            if (Id > 0)
                id = Id;
            if (Sellerid > 0)
                sellerid = Sellerid;
            address = Address;
            if (Price > 0)
                price = Price;

            this.pictureBox = pb;
            this.adressTextBox = aTB;
            this.costTextBox = cTB;

            if(rb != null)
            {
                this.radioButton = rb;
            }
            if (rms != null)
            {
                this.rooms = rms;
            }
            if (bthRms != null)
            {
                this.bathRooms = bthRms;
            }

        }

        public int getId()
        {
            return id;
        }

        public void setId(int Id)
        {
            id = Id;
        }

        public int getSellerid()
        {
            return sellerid;
        }

        public void setSellerid(int Sellerid)
        {
            sellerid = Sellerid;
        }

        public string getAddress()
        {
            return address;
        }

        public void setAddress(string Address)
        {
            address = Address;
        }

        public double getPrice()
        {
            return price;
        }

        public void setPrice(int Price)
        {
            price = Price;
        }

    }
}
