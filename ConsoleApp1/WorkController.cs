using NPOI.OpenXmlFormats.Shared;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class WorkController : MainController
    {
        private Cart cart;

        public WorkController (int paginationLimit) : base (paginationLimit)
        {
            cart = new Cart();
        }

        public void ScanProduct()
        {
            string EANCode = AnsiConsole.Ask<string>("[aquamarine1_1]Insert the EAN code of the product[/]");
            Article scanedArticle = shop.ScanEAN(EANCode);

            string option = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("── [bold yellow]Add to cart?[/] ──────────────────────────────────────────────────────────────────────").AddChoices(new[] { "Add", "Restock" }));

            switch (option)
            {
                case "Add":
                    cart.Add(scanedArticle);
                    break;
                case "Restock":
                    int quantity = Convert.ToInt32(AnsiConsole.Prompt(new SelectionPrompt<string>().Title("── [bold yellow]Quantity?[/] ──────────────────────────────────────────────────────────────────────")));

                    shop.ReStock(scanedArticle, quantity);
                    break;
                default:
                    break;
            }
        }

        public Table CurrentTable()
        {
            if (this.tables != null)
            {
                return this.tables[this.pagination];
            }
            else
            {
                TableConstruct(ConsoleApp1.TableConstruct.NONE, emptyCart: true);
                return this.tables[this.pagination];
            }
        }

        public void SearchProduct()
        {
            List<Article> ArticleSearch = new List<Article>();
            string EANCode = AnsiConsole.Ask<string>("[aquamarine1_1]Insert the EAN code of the product[/]");
            ArticleSearch.Add(shop.ScanEAN(EANCode));

            TableConstruct(ConsoleApp1.TableConstruct.NONE, Articles: ArticleSearch);

            AnsiConsole.Write(this.CurrentTable());

            Console.ReadKey();
            Console.Clear();
        }

        public void RetriveCart()
        {

            if (this.cart.Articles.Count > 0)
            {
                TableConstruct(ConsoleApp1.TableConstruct.NONE, Articles: cart.Articles);
            }
            else
            {
                TableConstruct(ConsoleApp1.TableConstruct.NONE, emptyCart: true) ;
            }
        }

        public void FinishPurchase()
        {
            foreach (var item in cart.Articles)
            {
                shop.DeStock(item);
            }

            cart.Articles.Clear();
        }

        public void CancelPurchase()
        {
            cart.Articles.Clear();
        }

        public void RemoveItem()
        {
            string EANCode = AnsiConsole.Ask<string>("[aquamarine1_1]Insert the EAN code of the product[/]");
            cart.Articles.Remove(shop.ScanEAN(EANCode));
        }

        public string[] MenuOptions()
        {
            if (tables.Count > 1)
            {
                if (tables.ElementAtOrDefault(pagination + 1) != null && tables.ElementAtOrDefault(pagination - 1) != null)
                {
                    return ["Previous", "Next", "Scan", "Search", "Remove item", "Finish purhcase", "Cancel purchase", "Exit"];
                }
                else if (tables.ElementAtOrDefault(pagination + 1) == null)
                {
                    return ["Previous", "Filter", "Scan", "Search", "Remove item", "Finish purhcase", "Cancel purchase", "Exit"];
                }
                else
                {
                    return ["Next", "Scan", "Search", "Remove item", "Finish purhcase", "Cancel purchase", "Exit"];
                }
            }
            else
            {
                if (tables[0].Rows.Count == 0)
                {
                    return ["Scan", "Search", "Exit"];
                }
                else
                {
                    return ["Scan", "Search", "Remove item", "Finish purhcase", "Cancel purchase", "Exit"];
                }
            }
        }
    }
}
