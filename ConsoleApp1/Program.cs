using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Spectre.Console;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string option = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("── [bold yellow]Modify a database or work with the database[/] ──────────────────────────────────────────────────────────────────────").AddChoices(new[] { "Modify", "Start working with it" }));
            string result = "null";

            if (option == "Start working with it")
            {
                var controller = new WorkController(7);

                if (Directory.EnumerateFileSystemEntries("databases").Any())
                {
                    controller.StartUp(database: AnsiConsole.Prompt(new SelectionPrompt<string>().Title("──[bold yellow] Select a database [/]───────────────────────────────────────").PageSize(10).AddChoices(controller.fetchDatabases())));
                }
                else
                {
                    AnsiConsole.MarkupLine("──[bold yellow] No database detectect [/]───────────────────────────────────────");
                    controller.StartUp(database: controller.CreateDatabase(AnsiConsole.Ask<string>("[aquamarine1_1]Insert the name of the new database[/]?")));
                }

                while (true)
                {
                    Console.Clear();

                    switch (result)
                    {
                        case "Previous":
                            controller.PreviousPaginationPage();
                            break;
                        case "Next":
                            controller.NextPaginationPage();
                            break;
                        case "Scan":
                            controller.ScanProduct();
                            break;
                        case "Search":
                            controller.SearchProduct();
                            break;
                        case "Remove item":
                            controller.RemoveItem();
                            break;
                        case "Finish purchase":
                            controller.FinishPurchase();
                            break;
                        case "Cancel purchase":
                            controller.CancelPurchase();
                            break;
                        case "Exit":
                            Environment.Exit(0);
                            break;
                        default:
                            break;
                    }

                    controller.RetriveCart();

                    AnsiConsole.Write(controller.CurrentTable());

                    AnsiConsole.MarkupLine(controller.PaginationStatus());

                    result = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(controller.MenuOptions()));

                    Console.Clear();
                }
            }
            else
            {
                var controller = new ModifyController(7);

                if (Directory.EnumerateFileSystemEntries("databases").Any())
                {
                    controller.StartUp(database: AnsiConsole.Prompt(new SelectionPrompt<string>().Title("──[bold yellow] Select a database [/]───────────────────────────────────────").PageSize(10).AddChoices(controller.fetchDatabases())));
                }
                else
                {
                    AnsiConsole.MarkupLine("──[bold yellow] No database detectect [/]───────────────────────────────────────");
                    controller.StartUp(database: controller.CreateDatabase(AnsiConsole.Ask<string>("[aquamarine1_1]Insert the name of the new database[/]?")));
                }

                while (true)
                {
                    Console.Clear();

                    switch (result)
                    {
                        case "Previous":
                            controller.PreviousPaginationPage();
                            break;
                        case "Next":
                            controller.NextPaginationPage();
                            break;
                        case "Filter":
                            string searchCategory = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("── [bold yellow]Select the filter column[/] ──────────────────────────────────────────────────────────────────────").AddChoices(new[] { "By condition", "By sale status", "By category", "By name", "By price" }));
                            if (searchCategory != "By name" && searchCategory != "By price")
                            {
                                controller.TableConstruct(TableConstruct.SEARCH, searchCategory: searchCategory, search: AnsiConsole.Prompt(new MultiSelectionPrompt<string>().Title("── [bold yellow]Select the filter category[/] ────────────────────────────────────────────────────────────────────").AddChoices(controller.SearchOptions(searchCategory))));
                            }
                            else if (searchCategory == "By name")
                            {
                                AnsiConsole.MarkupLine("── [bold yellow]Filter by name[/] ──────────────────────────────────────────────────────────────────────");
                                controller.TableConstruct(TableConstruct.SEARCH, searchCategory: searchCategory, searchName: AnsiConsole.Ask<string>("[aquamarine1_1]Insert the name of the product/s[/]?"));
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("── [bold yellow]Filter by price[/] ──────────────────────────────────────────────────────────────────────");
                                double first = AnsiConsole.Prompt(new TextPrompt<double>("[aquamarine1_1]Price greaeter than?[/]").PromptStyle("green").ValidationErrorMessage("[red]That's not a valid price number[/]").Validate(age => { return age switch { <= 0 => ValidationResult.Error("[red]The price can not be below 1[/]"), _ => ValidationResult.Success(), }; }));
                                double second = AnsiConsole.Prompt(new TextPrompt<double>("[deeppink4_2]Price smaller than?[/]").PromptStyle("green").ValidationErrorMessage("[red]That's not a valid price number[/]").Validate(price => { return price switch { var value when value <= first => ValidationResult.Error("[red]The price can not be below or equal the first price[/]"), _ => ValidationResult.Success(), }; }));
                                controller.TableConstruct(TableConstruct.SEARCH, searchCategory: searchCategory, search: new List<string> { Convert.ToString(first), Convert.ToString(second) });
                            }
                            break;
                        case "Sort":
                            controller.TableConstruct(TableConstruct.FILTER, filter: AnsiConsole.Prompt(new SelectionPrompt<string>().Title("── [bold yellow]Select the order[/] ────────────────────────────────────────────────────────────────────").AddChoices(["By condition", "By sale status", "By category", "By name", "By price"])));
                            break;
                        case "Database actions":
                            switch (AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "Imports", "Exports", "Modify database" })))
                            {
                                case "Imports":
                                    controller.ImportData(AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "Import JSON", "Import XML", "Import XLSX/XLS" })));
                                    break;
                                case "Exports":
                                    controller.ExportData(AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "Export JSON", "Export XML", "Export XLSX/XLS" })));
                                    break;
                                case "Modify database":
                                    AnsiConsole.MarkupLine("── [bold yellow]Modify the XLSX temporal modification file, then press enter[/] ────────────────────────");
                                    controller.EditDatabase();
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "Exit":
                            Environment.Exit(0);
                            break;
                        default:
                            break;
                    }

                    Console.Clear();

                    if (controller.CurrentTable() == null)
                    {
                        controller.TableConstruct(TableConstruct.NONE);
                    }

                    AnsiConsole.Write(controller.CurrentTable());

                    AnsiConsole.MarkupLine(controller.PaginationStatus());

                    result = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(controller.MenuOptions()));
                }

            }
        }
    }
}
