using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Entity
{
    public class Purchase
    {
        private int listing; // listing ID Number
        private int buyer;   // buyer account ID Number
        private int price; // cost of property

        public Purchase(int listing, int buyer, int price)
        {
            setListing(listing);
            setBuyer(buyer);
            setPrice(price);
        }
        public int getListing()
        {
            return this.listing;
        }
        public void setListing(int listing)
        {
            if (listing > -1)
                this.listing = listing;
        }
        public int getBuyer()
        {
            return this.buyer;
        }
        public void setBuyer(int buyer)
        {
            if (buyer > -1)
                this.buyer = buyer;
        }
        public int getPrice()
        {
            return this.price;
        }
        public void setPrice(int price)
        {
            if (price > 0)
                this.price = price;
        }

    }
}
