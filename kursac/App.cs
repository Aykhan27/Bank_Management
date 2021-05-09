using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace kursac
{
    class App
    {
        public static void SetTransfer(Rect rect , Transfers transfer)
        {
            while (true)
            {
                Window.Draw(rect);
                switch (Window.Draw_Choose(rect.ChangeWidth(-rect.width / 2), Program.TransferCategories))
                {
                    case 0:
                        string str_price = Window.Draw_Read_Line(new Rect(rect.left + rect.width / 2 + 2, rect.top + 2, rect.width / 2 - 4, 2), "Enter price");
                        if (new Regex(@"^\d+$").Match(str_price).Success)
                        {
                            transfer.Price = int.Parse(str_price);
                            Transfers.Reset(0);
                        }
                        else Errors.Draw("Price not correct!");
                        break;
                    case 1:
                        string str_tag = Window.Draw_Read_Line(new Rect(rect.left + rect.width / 2 + 2, rect.top + 2, rect.width / 2 - 4, 2), "Enter tag");
                        transfer.Tag = str_tag;
                        Transfers.Reset(1);
                        break;
                    case 2:
                        if (DateTime.TryParse(Window.Draw_Read_Line(new Rect(rect.left + rect.width / 2 + 2, rect.top + 2, rect.width / 2 - 4, 2), "Set Data as (MM/DD/YYYY)"), out DateTime new_time)) transfer.dateTime = new_time;
                        else Errors.Draw("Data not correct!");
                        Transfers.Reset(2);
                        break;
                    case 3:
                        var Categories = (transfer.Is_Received ? Program.Categories : Program.MoneyCategories);
                        int category = Window.Draw_Choose(new Rect(rect.left + rect.width / 2 + 1, rect.top, rect.width / 2, rect.height), Categories.Concat(new List<string>() { "Add New" }));
                        Window.Clear(new Rect(rect.left + rect.width / 2 + 1, rect.top + 1, rect.width / 2 - 1, rect.height - 2));
                        if (category == Categories.Count)
                        {
                            Window.Draw(rect);
                            Categories.Add(Window.Draw_Read_Line(new Rect(rect.left + rect.width / 2 + 2, rect.top + 2, rect.width / 2 - 5, 2), "Enter name of new category"));
                        }
                        transfer.Category = category;
                        Transfers.Reset(3);
                        break;
                    case 4:
                        return;
                }
                Window.Clear(new Rect(rect.left + rect.width / 2, rect.top, rect.width / 2 + 1, rect.height));
            }
        }

        public static void ScrollMenu(Rect rect)
        {
            if (Program.FilteringTransfers.Count > 0)
            {
                Window.Draw(rect);

                for (int choose = 0, start = 0, end = Program.FilteringTransfers.Count > rect.height - 1 ? rect.height - 1 : Program.FilteringTransfers.Count; true;)
                {
                    for (int i = start; i < end; i++)
                    {
                        Transfers item = Program.FilteringTransfers[i];
                        Console.SetCursorPosition(rect.left + 1, rect.top + 1 + (i - start));
                        Console.Write(item);
                        Console.ResetColor();
                        if (i == choose)
                        {
                            Console.Write(" <--         \b\b\b\b\b\b\b\b");
                            Console.Write(i + 1);
                        }
                        else Console.Write("                      ");
                    }
                    Console.SetCursorPosition(0,0);
                    Console.Write($"ch={choose} s={start} e={end}");
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.DownArrow:
                            if (choose + 1 < Program.FilteringTransfers.Count())
                            {
                                if (++choose >= end)
                                {
                                    start++;
                                    end++;
                                }
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if (choose > 0)
                            {
                                if (--choose < start)
                                {
                                    start--;
                                    end--;
                                }
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            for (int i = 0; i < 10; i++)
                            {
                                if (choose + 1 < Program.FilteringTransfers.Count())
                                {
                                    if (++choose >= end)
                                    {
                                        start++;
                                        end++;
                                    }
                                }
                                else break;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            for (int i = 0; i < 10; i++)
                            {
                                if (choose > 0)
                                {
                                    if (--choose < start)
                                    {
                                        start--;
                                        end--;
                                    }
                                }
                                else break;
                            }
                            break;
                        case ConsoleKey.Enter:
                            Window.Clear(rect);
                            SetTransfer(rect, Program.FilteringTransfers.ElementAt(choose));
                            return;
                        case ConsoleKey.Escape:
                            return;
                    }
                }
            }
            else Errors.Add("You dont have a transfers!");
        }

        public static void MainMenu(Rect rect , ref int choose)
        {
            while (true)
            {
                for (int i = 0; i < Program.Offers.Count; i++)
                {
                    Window.Draw_At_Center(rect.ChangeLeft(i * (rect.width + 3)), Program.Offers[i], i == choose ? ConsoleColor.Yellow : ConsoleColor.Gray);
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.RightArrow:
                        if (choose + 1 < Program.Offers.Count) choose++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (choose > 0) choose--;
                        break;
                    case ConsoleKey.Enter:
                        return;
                }
            }
        }

        public static void TransferMenu(Rect rect, bool is_recived, List<string> Categories)
        {
            if (Program.me.Cards.Count > 0)
            {
                string str_price = Window.Draw_Read_Line(new Rect(rect.left + 2, rect.top + 2, rect.width / 2 - 4, 2), "Enter price");
                if (new Regex(@"^\d+$").Match(str_price).Success)
                {
                    string str_tag = Window.Draw_Read_Line(new Rect(rect.left + 2, rect.top + 6, rect.width / 2 - 4, 2), "Enter tag");
                    DateTime dateTime = DateTime.Now;
                    if (Window.Draw_Bool(new Rect(rect.left + 2, rect.top + 9, rect.width / 2 - 4, 10), "Do you want to change data"))
                    {
                        if (DateTime.TryParse(Window.Draw_Read_Line(new Rect(rect.left + 2, rect.top + 21, rect.width / 2 - 4, 2), "Set Data as (MM/DD/YYYY)"), out DateTime new_time)) dateTime = new_time;
                        else
                        {
                            Errors.Add("Data not correct!");
                            return;
                        }
                    }
                    int category = Window.Draw_Choose(new Rect(rect.left + rect.width / 2, rect.top, rect.width / 2 + 1, rect.height), Categories.Concat(new List<string>() { "Add New" }));
                    if (category == Categories.Count)
                    {
                        Window.Clear(new Rect(rect.left + rect.width / 2, rect.top, rect.width / 2 + 1, rect.height / 2 + 1));
                        Window.Draw(rect);
                        Categories.Add(Window.Draw_Read_Line(new Rect(rect.left + rect.width / 2 + 2, rect.top + 2, rect.width / 2 - 4, 2), "Enter name of new category"));
                    }
                    Window.Clear(new Rect(rect.left + rect.width / 2 + 1, rect.top + 1, rect.width / 2 - 2, rect.height - 2));
                    int cur = Window.Draw_Choose(new Rect(rect.left + rect.width / 2, rect.top, rect.width / 2 + 1, rect.height), Program.CurrencyCategories);
                    Window.Clear(new Rect(rect.left + rect.width / 2 + 1, rect.top + 1, rect.width / 2 - 2, rect.height - 2));
                    int card = Window.Draw_Choose(new Rect(rect.left + rect.width / 2, rect.top, rect.width / 2 + 1, rect.height), Program.me.GetCardNames());
                    Transfers item = new Transfers(card, new Currency(int.Parse(str_price), cur), str_tag, category, is_recived, dateTime);
                    Program.transfers.Add(item);
                    if (Program.min < item.dateTime && item.dateTime < Program.max) Program.FilteringTransfers.Add(item);
    
                    Transfers.Reset();
                }
                else Errors.Add("Price not correct!");
            }
            else Errors.Add("You dont have a card!");
        }

        public static void SetError(Rect rect, string str = "")
        {
            Window.Clear(rect);
            Window.Draw(rect,ConsoleColor.Gray);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(rect.left + 1, rect.top + 1);
            Console.Write(str);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void SetData(Rect rect)
        {
            switch (Window.Draw_Choose(rect, Program.DataCategories))
            {
                case 0:
                    Program.min = DateTime.Today;
                    Program.max = DateTime.Today.AddDays(1);
                    break;
                case 1:
                    DateTime startWeek = DateTime.Today.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek);
                    DateTime endWeek = startWeek.AddDays(7); ;
                    Program.min = startWeek;
                    Program.max = endWeek;
                    break;
                case 2:
                    DateTime startMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    DateTime endMonth = startMonth.AddMonths(1);
                    Program.min = startMonth;
                    Program.max = endMonth;
                    break;
                case 3:
                    DateTime startYear = new DateTime(DateTime.Today.Year, 1, 1);
                    DateTime endYear = startYear.AddYears(1);
                    Program.min = startYear;
                    Program.max = endYear;
                    break;
                case 4:
                    Program.min = DateTime.MinValue;
                    Program.max = DateTime.MaxValue;
                    break;
                case 5:
                    string str_min = Window.Draw_Read_Line(new Rect(rect.left + rect.width + 2, rect.top + 2, rect.width - 4, 2), "Enter a min Data as (MM/DD/YYYY)");
                    if (DateTime.TryParse(str_min, out DateTime new_min))
                    {
                        Program.min = new_min;
                    }
                    else
                    {
                        Errors.Add("Data not correct!");
                        return;
                    }

                    string str_max = Window.Draw_Read_Line(new Rect(rect.left + rect.width + 2, rect.top + 6, rect.width - 4, 2), "Enter a max Data as (MM/DD/YYYY)");
                    if (DateTime.TryParse(str_max, out DateTime new_max))
                    {
                        Program.min = new_max;
                    }
                    else
                    {
                        Errors.Add("Data not correct!");
                        return;
                    }
                    break;
            }
            Program.FilteringTransfers = Program.transfers.Where(x => Program.min <= x.dateTime && x.dateTime < Program.max).ToList();
        }

        public static void Graph(Rect rect)
        {
            Window.Draw(rect);

            float DecE = 0;
            float DecI = 0;
            int[] categories = new int[Program.Categories.Count];
            int[] Mcategories = new int[Program.MoneyCategories.Count];
            int Cmaxsize = Program.Categories.Max(x => x.Length);
            int Mmaxsize = Program.MoneyCategories.Max(x => x.Length);

            foreach (var item in Program.FilteringTransfers)
            {
                if (item.Is_Received)
                {
                    categories[item.Category] += Currency.GetConvertMoney(item.Price, Program.me.Balance.currency);
                    DecE += Currency.GetConvertMoney(item.Price, Program.me.Balance.currency);
                }
                else
                {
                    Mcategories[item.Category] += Currency.GetConvertMoney(item.Price, Program.me.Balance.currency);
                    DecI += Currency.GetConvertMoney(item.Price, Program.me.Balance.currency);
                }
            }
            DecE = DecE == 0 ? 0 : 100f / DecE;
            DecI = DecI == 0 ? 0 : 100f / DecI;

            for (int i = 1,start = 0; start < rect.height && start < Program.Categories.Count; start++, i++)
            {
                int val = categories[start];
                Console.SetCursorPosition(rect.left + 1 , rect.top + i);
                Console.Write($"[{Program.Categories[start]}{new string(' ', Cmaxsize - Program.Categories[start].Length)}] - ");
                for (int l = 0; l < DecE * val / 10; l++)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(' ');
                }
                Console.ResetColor(); 
                Console.SetCursorPosition(rect.left + Cmaxsize + 16 , rect.top + i);
                Console.Write($" {Convert.ToInt32(DecE * val)}%");
                Console.SetCursorPosition(rect.left + Cmaxsize + 22 , rect.top + i);
                Console.Write(val);
            }

            for (int i = 1,start = 0; start < rect.height && start < Program.MoneyCategories.Count; start++, i++)
            {
                int val = Mcategories[start];
                Console.SetCursorPosition(rect.left + rect.width / 2 + 1, rect.top + i);
                Console.Write($"[{Program.MoneyCategories[start]}{new string(' ', Mmaxsize - Program.MoneyCategories[start].Length)}] - ");
                for (int l = 0; l < DecI * val / 10; l++)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write(' ');
                }
                Console.ResetColor();
                Console.SetCursorPosition(rect.left + rect.width / 2 + Mmaxsize + 16, rect.top + i);
                Console.Write($" {Convert.ToInt32(DecI * val)}%");
                Console.SetCursorPosition(rect.left + rect.width / 2 + Mmaxsize + 22, rect.top + i);
                Console.Write(val);
            }
            Console.ReadKey();
        }
    }
}