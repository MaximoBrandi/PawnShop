using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Discount
    {
        private string name;
        private double discountPercentage;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public double DiscountPercentage
        {
            get { return this.discountPercentage; }
            set { this.discountPercentage = value; }
        }

        public Discount(string name, double discountPercentage)
        {
            Name = name;
            DiscountPercentage = discountPercentage;
        }
    }
}
