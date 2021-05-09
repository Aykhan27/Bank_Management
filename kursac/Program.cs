using System;
using System.Collections.Generic;
using System.Linq;

namespace kursac
{
    static class Program
    {
        public static int count = 10;

        static void Addtransfers()
        {
            Random random = new Random();                                                                                                                                                                ////
            for (int i = 0; i < count; i++)
            {
                bool t = random.Next(0, 100) > 55 ? true : false;
                transfers.Add(new Transfers(0, new Currency(random.Next(1, 2000), random.Next(1, Currency.CurrencyNames.Count)), random.Next(1, 2000).ToString(), random.Next(0, t ? Categories.Count : MoneyCategories.Count), t, DateTime.Now.AddDays(random.Next(-365, 365))));
                if (transfers[i].Is_Received)
                {
                    me.Expense += transfers[i].Price;
                    me.Cards[0].Balance -= transfers[i].Price;
                }
                else
                {
                    me.Income += transfers[i].Price;
                    me.Cards[0].Balance += transfers[i].Price;
                }
            }
            FilteringTransfers = transfers.Where(x => min <= x.dateTime && x.dateTime < max).ToList();
            Transfers.Reset();
        }

        static void Main()
        {
            Currency.ResetCurencies();

            Addtransfers();                                                          

            Rect info = new Rect(8, 2, 2 * (Offers.Count + 1) + Offers.Count * Offers.Max(x => (x.Length + 2) < 16 ? 16 : x.Length) + 1 , 3);
            Rect menu = new Rect(info.left, info.top + info.height + 1, info.width, 8);
            Rect window = new Rect(menu.left, menu.top + menu.height + 1, menu.width, 35);
            Rect option = new Rect(menu.left + 3, menu.top + (menu.height - 4) / 2, (menu.width - 2 * (Offers.Count + 1) - 2) / Offers.Count, 4);
            Errors.rect = new Rect(menu.left, window.top + window.height + 1, menu.width, 2);

            Console.SetBufferSize(menu.width + 16, 500);
            Console.SetWindowSize(menu.width + 16, info.top + info.height + menu.height + window.height + Errors.rect.height + 6);
            
            for (int choose = 0; true;)
            {
                me.Reset();

                Window.Draw(window, ConsoleColor.DarkGray);

                me.Draw(info);

                Window.Draw(menu);

                App.MainMenu(option, ref choose);

                Window.Draw(window);

                switch (choose)
                {
                    case 0:
                        App.TransferMenu(window, false, MoneyCategories);
                        break;
                    case 1:
                        App.TransferMenu(window, true, Categories);
                        break;
                    case 2:
                        me.EditCard(window);
                        break;
                    case 3:
                        App.SetData(new Rect(window.left, window.top, window.width / 2, window.height));
                        break;
                    case 4:
                        App.Graph(window);
                        break;
                    case 5:
                        App.ScrollMenu(window);
                        break;
                    case 6:
                        me.EditCurrencyes(window);
                        break;
                    case 7:
                        Window.Draw(window, ConsoleColor.DarkGray);
                        Errors.Draw("Goodbye See You Later.", ConsoleColor.Green);
                        while (true) Console.ReadKey(true);
                }
                Console.Clear();
                Errors.Draw();
            }
        }

        public static Accaunt me = new Accaunt();

        public static DateTime min = DateTime.MinValue;
        public static DateTime max = DateTime.MaxValue;

        public static List<string> Offers = new List<string>{
               "$Get$", "$Spend$", "Edit Card", "Chage Data","View Graph","Show Transfers","View $Currency","Exit"
            };
        public static List<string> Categories = new List<string>{
                "Bills","Car","Clothes","Communications","Eating out","Enterteimant","Fodd","Gifts","Health","House","Pets","Sports","Taxi","Toiletry","Transport"
            };
        public static List<string> DataCategories = new List<string> {
                "Day","Week","Month","Year","All","Choose Data"
            };
        public static List<string> CardCategories = new List<string>{
            "Add Card","Manage Card","Delete Card"
        };
        public static List<string> MoneyCategories = new List<string>{
                "Deposits","Salary","Savings"
            };
        public static List<string> TransferCategories = new List<string> {
                "Change Price","Change Tag","Change Data","Change Categories","Exit"
            };
        public static List<string> CurrencyCategories = new List<string>{
            "AZN","USD","EUR"
        };
        public static List<string> CardEditCategories = new List<string>{
            "Change Name","Change Currency"
        };
        public static List<string> CurrencyEditCategories = new List<string>{
            "Get Actual Rates","Change Base Currency"
        };

        public static List<Transfers> transfers = new List<Transfers>(count + 10);
        public static List<Transfers> FilteringTransfers = new List<Transfers>(count + 10);
    }
}