using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class ModifyController : MainController
    {
        public ModifyController(int paginationLimit) : base(paginationLimit)
        {
        }
        public string[] SearchOptions(string searchCategory)
        {
            switch (searchCategory)
            {
                case "By condition":
                    return ["Mint Condition", "Mediocre", "Bad"];
                case "By sale status":
                    return ["Yes", "No"];
                case "By category":
                    return ["Antique", "Jewlery", "Various"];
                default:
                    return [""];
            }
        }

        public string[] MenuOptions()
        {
            if (tables.Count > 1)
            {
                if (tables.ElementAtOrDefault(pagination + 1) != null && tables.ElementAtOrDefault(pagination - 1) != null)
                {
                    return ["Previous", "Next", "Filter", "Sort", "Database actions", "Exit"];
                }
                else if (tables.ElementAtOrDefault(pagination + 1) == null)
                {
                    return ["Previous", "Filter", "Sort", "Database actions", "Exit"];
                }
                else
                {
                    return ["Next", "Filter", "Sort", "Database actions", "Exit"];
                }
            }
            else
            {
                return ["Filter", "Sort", "Database actions", "Exit"];
            }
        }
    }
}
