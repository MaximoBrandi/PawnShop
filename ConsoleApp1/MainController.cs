using Newtonsoft.Json;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    internal class MainController : TableController
    {
        protected int pagination;

        public MainController(int paginationLimit) : base (paginationLimit)
        {
            this.pagination = 0;
        }

        public bool AvailableDatabases()
        {
            if (Directory.EnumerateFileSystemEntries("databases").Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string[] fetchDatabases() {
            return Array.ConvertAll(Directory.GetFiles("databases", "*.json"), i => i.Split('\\').Last());
        }
        public void NextPaginationPage()
        {
            this.pagination++;
        }
        public void PreviousPaginationPage()
        {
            this.pagination--;
        }
        public Table CurrentTable()
        {
            if (this.tables != null)
            {
                return this.tables[this.pagination];
            }
            else
            {
                TableConstruct(ConsoleApp1.TableConstruct.NONE);
                return this.tables[this.pagination];
            }
        }
        public string PaginationStatus()
        {
            return "Table[bold yellow] "+(this.pagination+1)+"[/] of " + "[bold yellow]" + this.tables.Count + "[/]";
        }
        public string CreateDatabase(string databaseName)
        {
            File.WriteAllText("exports/" + databaseName + ".json", JsonConvert.SerializeObject(new Shop(databaseName, new List<Article>())));

            return databaseName + ".json";
        }
        public void StartUp(bool availableDatabases = true, string database = null)
        {
            if (!Directory.Exists("exports") || !Directory.Exists("databases"))
            {
                Directory.CreateDirectory("exports"); Directory.CreateDirectory("databases");
            }

            this.shop = JsonConvert.DeserializeObject<Shop>(File.ReadAllText("databases/" + database)); ;
        }
    }
}
