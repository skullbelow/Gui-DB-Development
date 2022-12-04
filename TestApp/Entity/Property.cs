using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Entity
{
    public class Property
    {
        private int listingID;
        private int price;

        public Property(int ListID, int Price)
        {
            listingID = ListID;
            price = Price;
        }
        public int getListingID()
        {
            return listingID;
        }
        public void setListingID(int nlistingID)
        {
            listingID = nlistingID;
        }
        public void setPrice(int nPrice)
        {
            price = nPrice;
        }
        public int getPrice()
        {
            return price;
        }

    }
}
