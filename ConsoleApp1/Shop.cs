using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class Shop
    {
        private string name;
        private List<Article> articlesOnSale;

        public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}
        public List<Article> ArticlesOnSale
        {
            get { return this.articlesOnSale; }
            set { this.articlesOnSale = value; }
        }

        public void Add(Article article)
        {
            articlesOnSale.Add(article);
        }

        public Article ScanEAN(string EAN)
        {
            return articlesOnSale.Where(Article => Article.EAN == EAN).First();
        }

        public void ReStock(Article article, int amount = 1)
        {
            article.Stock += amount;
        }

        public void DeStock(Article article, int amount = 1)
        {
            article.Stock -= amount;
        }

        public void Remove(Article article)
        {
            articlesOnSale.Remove(article);
        }

        public List<Article> Search(string Name)
        {
            return articlesOnSale.Where(Article => Article.Name == Name).ToList();
        }
        public Shop(string name, List<Article> articlesOnSale)
        {
            Name = name;
            ArticlesOnSale = articlesOnSale;
        }

    }
}
