using NPOI.SS.Formula.Functions;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class TableController : DatabaseController
    {
        protected int paginationLimit { get; set; }
        protected  List<Table> tables { get; set; }

        public TableController(int paginationLimit)
        {
            this.paginationLimit = paginationLimit;
        }
        public void TableConstruct(TableConstruct typeConstruct, string filter = null, List<string> search = null, string searchCategory = null, string searchName = null, List<Article> Articles = null, bool emptyCart = false)
        {
            int loop = 0; var table = new Table(); List<Article> rows = ((Articles == null && emptyCart != true) ? shop.ArticlesOnSale : (emptyCart == true) ? new List<Article>() : Articles); this.tables = new List<Table>();

            if (searchCategory != null)
            {
                if (searchName != null)
                {
                    rows = SearchTables(searchCategory, rows, searchName: searchName);
                }
                else
                {
                    rows = SearchTables(searchCategory, rows, search: search);
                }
            }
            else if (filter != null)
            {
                rows = FilterTables(filter, rows);
            }

            if (rows.Count != 0)
            {
                Article last = rows.Last();

                foreach (var row in rows)
                {
                    loop++;
                    if (loop > this.paginationLimit || loop == 1 || row.Equals(last))
                    {
                        if (loop == 1)
                        {
                            // Create a table
                            table = new Table().Title("[italic darkcyan]" + shop.Name + "[/]").Border(TableBorder.Horizontal).BorderColor(Color.LightSeaGreen);

                            foreach (var column in rows.First().GetProperties())
                            {
                                table.AddColumn(new TableColumn(column).Centered().Padding(2, 2));
                            }
                        }
                        table.AddRow(row.Name, "[bold yellow]" + Convert.ToString(row.Price) + "[/]", ((Convert.ToBoolean(row.OnSale) ? "[bold green] On sale [/]" : "[bold olive] Sold [/]")), (row.Status switch { "Bad" => "[bold red] Bad [/]", "Mediocre" => "[bold yellow] Mediocre [/]", "Mint Condition" => "[bold green] Mint Condition [/]" }), Convert.ToString(row.Category), row.EAN, Convert.ToString(row.Stock));

                        if (loop > this.paginationLimit)
                        {
                            loop = 0;

                            this.tables.Add(table);

                            // Create a table
                            table = new Table().Title("[italic darkcyan]" + shop.Name + "[/]").Border(TableBorder.Horizontal).BorderColor(Color.LightSeaGreen);

                            foreach (var column in rows.First().GetProperties())
                            {
                                table.AddColumn(new TableColumn(column).Centered().Padding(2, 2));
                            }
                        }
                        if (row.Equals(last))
                        {
                            loop = 0;

                            this.tables.Add(table);
                        }
                    }
                    else
                    {
                        table.AddRow(row.Name, "[bold yellow]" + Convert.ToString(row.Price) + "[/]", ((Convert.ToBoolean(row.OnSale) ? "[bold green] On sale [/]" : "[bold olive] Sold [/]")), (row.Status switch { "Bad" => "[bold red] Bad [/]", "Mediocre" => "[bold yellow] Mediocre [/]", "Mint Condition" => "[bold green] Mint Condition [/]" }), Convert.ToString(row.Category), row.EAN, Convert.ToString(row.Stock));
                    }
                }
            }
            else
            {
                this.tables.Add(new Table());
            }
        }
        private List<Article> FilterTables(string filter, List<Article> Articles)
        {
            switch (filter)
            {
                case "By condition":
                    Articles.Sort((x, y) => x.Status.CompareTo(y.Status));
                    break;
                case "By sale status":
                    Articles.Sort((x, y) => x.OnSale.CompareTo(y.OnSale));
                    break;
                case "By category":
                    Articles.Sort((x, y) => x.Category.CompareTo(y.Category));
                    break;
                case "By name":
                    Articles.Sort((x, y) => x.Name.CompareTo(y.Name));
                    break;
                case "By price":
                    Articles.Sort((x, y) => x.Price.CompareTo(y.Price));
                    break;
                case "By stock":
                    Articles.Sort((x, y) => x.Stock.CompareTo(y.Stock));
                    break;
            }

            return Articles;
        }
        private List<Article> SearchTables(string searchCategory, List<Article> Articles, List<string> search = null, string searchName = null)
        {
            List<List<Article>> lists;

            switch (searchCategory)
            {
                case "By condition":
                    lists = new List<List<Article>>();
                    foreach (var condition in search)
                    {
                        lists.Add(Articles.Where(article => article.Status == condition).ToList());
                    }
                    return lists.SelectMany(sublist => sublist).ToList();
                case "By sale status":
                    lists = new List<List<Article>>();
                    foreach (var status in search)
                    {
                        lists.Add(Articles.Where(article => Convert.ToBoolean(article.OnSale) == (status == "Yes" ? true : false)).ToList());
                    }
                    return lists.SelectMany(sublist => sublist).ToList();
                case "By category":
                    lists = new List<List<Article>>();
                    foreach (var category in search)
                    {
                        lists.Add(Articles.Where(article => article.Category == category).ToList());
                    }
                    return lists.SelectMany(sublist => sublist).ToList();
                case "By name":
                    return Articles.Where(article => article.Name.Contains(searchName) == true).ToList();
                case "By price":
                    return Articles.Where(article => (Convert.ToDouble(search.First()) < article.Price && article.Price < Convert.ToDouble(search.Last()))).ToList();
                default:
                    return Articles;
            }
        }
    }
}
