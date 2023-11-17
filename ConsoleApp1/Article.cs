using Spectre.Console;
using System;
using System.Runtime.Remoting.Messaging;

namespace ConsoleApp1
{
    internal class Article
    {
        private string name;
        private double price;
        private bool onSale;
        private int stock;
        private string ean;
        private Status status;
        private Category category;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public double Price
        {
            get { return this.price; }
            set { this.price = value; }
        }
        public int Stock
        {
            get { return this.stock; }
            set { this.stock = value; }
        }
        public string EAN
        {
            get { return this.ean; }
            set { this.ean = value;  }
        }
        public string OnSale
        {
            get 
            {
                switch (this.onSale)
                {
                    case true:
                        return "true";

                    case false:
                        return "false";
                } 
            }
            set 
            {
                switch (value)
                {
                    case "true":
                        this.onSale = true;
                        break;
                    case "false":
                        this.onSale = false;
                        break;
                    default:
                        this.onSale = false;
                        break;
                }
            }
        }
        public string Status
        {
            get
            {
                switch (this.status)
                {
                    case ConsoleApp1.Status.BAD:
                        return "Bad";
                    case ConsoleApp1.Status.MEDIOCRE:
                        return "Mediocre";
                    case ConsoleApp1.Status.MINT_CONDITION:
                        return "Mint Condition";
                    default:
                        return "[grey]Undefined[/]";
                }
            }
            set
            {
                switch (value)
                {
                    case "Bad":
                        this.status = ConsoleApp1.Status.BAD;
                        break;
                    case "Mediocre":
                        this.status = ConsoleApp1.Status.MEDIOCRE;
                        break;
                    case "Mint Condition":
                        this.status = ConsoleApp1.Status.MINT_CONDITION;
                        break;
                }
            }
        }
        public string Category
        {
            get
            {
                switch (this.category)
                {
                    case ConsoleApp1.Category.ANTIQUE:
                        return "Antique";
                    case ConsoleApp1.Category.JEWLERY:
                        return "Jewlery";
                    case ConsoleApp1.Category.VARIOUS:
                        return "Various";
                    default:
                        return "[grey]Undefined[/]";
                }
            }
            set
            {
                switch (value)
                {
                    case "Antique":
                        this.category = ConsoleApp1.Category.ANTIQUE;
                        break;
                    case "Jewlery":
                        this.category = ConsoleApp1.Category.JEWLERY;
                        break;
                    case "Various":
                        this.category = ConsoleApp1.Category.VARIOUS;
                        break;
                }
            }
        }

        public Article(string name, double price, string onSale, string status, string category, string ean, int stock)
        {
            Name = name;
            Price = price;
            OnSale = onSale;
            Status = status;
            Category = category;
            EAN = ean;
            Stock = stock;
        }

        public string[] GetProperties()
        {
            return ["Name", "Price", "On sale", "Condition", "Category", "EAN", "Stock"];
        }
    }
}
