using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Npoi.Mapper;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace ConsoleApp1
{
    internal class DatabaseController
    {
        protected Shop shop;

        public void ImportData(string ImportType)
        {
            switch (ImportType)
            {
                case "Import JSON":
                    AnsiConsole.MarkupLine("── [bold yellow]Import JSON[/] ──────────────────────────────────────────────────────────────────────");
                    this.shop.ArticlesOnSale = new List<List<Article>> { shop.ArticlesOnSale, JsonConvert.DeserializeObject<Shop>(File.ReadAllText(AnsiConsole.Ask<string>("[aquamarine1_1]Insert the JSON Path or URL[/]?"))).ArticlesOnSale }.SelectMany(sublist => sublist).ToList();
                    break;
                case "Import XML":
                    AnsiConsole.MarkupLine("── [bold yellow]Import XML[/] ──────────────────────────────────────────────────────────────────────");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(File.ReadAllText(AnsiConsole.Ask<string>("[aquamarine1_1]Insert the XML Path or URL[/]?")));
                    this.shop.ArticlesOnSale = new List<List<Article>> { shop.ArticlesOnSale, JObject.Parse(JsonConvert.SerializeXmlNode(doc)).SelectToken("root", false).ToObject<Shop>().ArticlesOnSale }.SelectMany(sublist => sublist).ToList();
                    break;
                case "Import XLSX/XLS":
                    AnsiConsole.MarkupLine("── [bold yellow]Import XLSX/XLS[/] ──────────────────────────────────────────────────────────────────────");
                    this.shop.ArticlesOnSale = new List<List<Article>> { shop.ArticlesOnSale, XLSImport(new Mapper(AnsiConsole.Ask<string>("[aquamarine1_1]Insert the XML Path or URL[/]?")).Take<ArticleImport>("Articles").Select(x => x.Value).ToList()) }.SelectMany(sublist => sublist).ToList();
                    break;
                default:
                    break;
            }
        }
        public void ExportData(string ExportType)
        {
            switch (ExportType)
            {
                case "Export JSON":
                    File.WriteAllText("exports/" + this.shop.Name + ".json", JsonConvert.SerializeObject(this.shop));
                    break;
                case "Export XML":
                    XNode node = JsonConvert.DeserializeXNode(JsonConvert.SerializeObject(shop), "root");
                    File.WriteAllText("exports/" + this.shop.Name + ".xml", node.ToString());
                    break;
                case "Export XLSX/XLS":
                    //var objects = ...
                    var mappeh = new Mapper();
                    mappeh.Put(new[] { this.shop }, "Shop", true);
                    mappeh.Put(this.shop.ArticlesOnSale, "Articles", false);
                    mappeh.Save("exports/" + this.shop.Name + ".xlsx");
                    break;
                default:
                    break;
            }
        }
        public void CreateTemporaryEditFile()
        {
            var mapper = new Mapper();
            mapper.Put(new[] { this.shop }, "Shop", true);
            mapper.Put(this.shop.ArticlesOnSale, "Articles", false);
            mapper.Save("exports/" + this.shop.Name + " temporal edit file.xlsx");
        }
        public void ImportTemporaryEditFile()
        {
            this.shop.ArticlesOnSale = XLSImport(new Mapper("exports/" + this.shop.Name + " temporal edit file.xlsx").Take<ArticleImport>("Articles").Select(x => x.Value).ToList());
            File.Delete("exports/" + this.shop.Name + " temporal edit file.xlsx");
        }
        private List<Article> XLSImport(List<ArticleImport> ImportList)
        {
            List<Article> ArticlesXLSM = new List<Article>();

            foreach (var ArticleImport in new Mapper("exports/" + this.shop.Name + " temporal edit file.xlsx").Take<ArticleImport>("Articles").Select(x => x.Value).ToList())
            {
                if (Convert.ToDouble(ArticleImport.Price) == 0)
                {
                    break;
                }
                ArticlesXLSM.Add(new Article(ArticleImport.Name, Convert.ToDouble(ArticleImport.Price), ArticleImport.OnSale, ArticleImport.Status, ArticleImport.Category, ArticleImport.EAN, Convert.ToInt32(ArticleImport.Stock)));
            }

            return ArticlesXLSM;
        }
        public void EditDatabase()
        {
            CreateTemporaryEditFile();
            Console.ReadKey();
            ImportTemporaryEditFile();
        }
    }
}
