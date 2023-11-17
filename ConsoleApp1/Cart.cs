using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Cart
    {
        private List<Article> articles;
        private double totalPrice;
        private double totalDiscount;
        private double total;
        private List<Discount> discounts;

        public List<Article> Articles
        {
            get { return this.articles; }
            set { this.articles = value; }
        }
        public double TotalPrice
        {
            get { return this.totalPrice; }
            set { this.totalPrice = value; }
        }
        public double TotalDiscount
        {
            get { return this.totalDiscount; }
            set { this.totalDiscount = value; }
        }
        public double Total
        {
            get { return this.total; }
            set { this.total = value; }
        }
        public List<Discount> Discounts
        {
            get { return this.discounts; }
            set { this.discounts = value; }
        }

        public void Add(Article article)
        {
            articles.Add(article);
        }

        public void Remove(Article article)
        {
            articles.Remove(article);
        }

        public void CalculateTotalPrice()
        {
            double totalPriceCalculation = 0;
            foreach (Article article in articles)
            {
                totalPriceCalculation += article.Price;
            }
            this.totalPrice = totalPriceCalculation;
        }

        public void CalculateTotalDiscount()
        {
            double totalDiscount = 0;
            foreach (Discount discount in discounts)
            {
                totalDiscount += discount.DiscountPercentage;
            }

            double discountPercentage = totalDiscount / totalPrice * 100;
            double discountAmount = totalPrice * (discountPercentage / 100);
            double discountedPrice = totalPrice - discountAmount;
            this.totalDiscount = discountedPrice;
        }

        public void CalculateTotal()
        {
            this.total = totalPrice - totalDiscount;
        }

        public Cart()
        {
            Articles = new List<Article>();
            Discounts = new List<Discount>();
        }
    }
}
