using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Entity
{
    public class Listing
    {
        private int id;
        private int sellerid;
        private string address;
        private double price;

        public Listing(int Id, int Sellerid, string Address, double Price)
        {
            if (Id > 0)
                id = Id;
            if (Sellerid > 0)
                sellerid = Sellerid;
            address = Address;
            if (Price > 0)
                price = Price;
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

        public void setPrice(double Price)
        {
            price = Price;
        }

    }
}
