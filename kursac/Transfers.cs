using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace kursac
{
    class Transfers
    {
        private static int? maxCard = null;
        private static int? maxCategory = null;
        private static int? maxPrice = null;
        private static int? maxTag = null;
        private static int? maxTime = null;

        public int Cardid = 0;
        public bool Is_Received = true;
        public Currency Price = 0;
        public string Tag = "";
        public int Category = 0;
        public DateTime dateTime = DateTime.Now;
        
        public Transfers()
        {

        }

        public Transfers(int _Cardid ,Currency _price,string _tag,int _category,bool _is_received , DateTime _dateTime)
        {
            Cardid = _Cardid;
            Price = _price;
            Tag = _tag;
            Is_Received = _is_received;
            dateTime = _dateTime;
            Category = _category;
        }

        public static void Reset(int index = -1)
        {
            if (Program.FilteringTransfers.Count > 0)
            {
                Transfers _this = Program.FilteringTransfers[Program.FilteringTransfers.Count - 1];
                if (maxPrice == null || index == 0) maxPrice = Program.FilteringTransfers.Max(x => x.Price.ToString().Length);
                else if (index == -1)
                {
                    int lenght = _this.Price.ToString().Length;
                    if (lenght > maxPrice.Value) maxPrice = lenght;
                }
                if (maxTag == null || index == 1) maxTag = Program.FilteringTransfers.Max(x => x.Tag.Length);
                else if (index == -1)
                {
                    int lenght = _this.Tag.Length;
                    if (lenght > maxTag.Value) maxTag = lenght;
                }
                if (maxTime == null || index == 2) maxTime = Program.FilteringTransfers.Max(x => x.dateTime.ToShortDateString().Length);
                else if (index == -1)
                {
                    int lenght = _this.dateTime.ToShortDateString().Length;
                    if (lenght > maxTime.Value) maxTime = lenght;
                }
                if (maxCategory == null || index == 3) maxCategory = Program.FilteringTransfers.Max(x => (x.Is_Received ? Program.Categories : Program.MoneyCategories)[x.Category].Length);
                else if (index == -1)
                {
                    int lenght = (_this.Is_Received ? Program.Categories : Program.MoneyCategories)[_this.Category].Length;
                    if (lenght > maxCategory.Value) maxCategory = lenght;
                }
            }
            else
            {
                maxCard = null;
                maxCategory = null;
                maxPrice = null;
                maxTag = null;
                maxTime = null;
            }
            if (Program.me.Cards.Count > 0)
            {
                if (maxCard == null || index == 4) maxCard = Program.me.Cards.Max(x => x.Name.Length);
                else if (index == -2)
                {
                    Card _this = Program.me.Cards[Program.me.Cards.Count - 1];
                    int lenght = _this.Name.Length;
                    if (lenght > maxCard.Value) maxCard = lenght;
                }
            }
            else
            {
                maxCard = null;
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            List<string> LCategory = (Is_Received ? Program.Categories : Program.MoneyCategories);
            Console.ForegroundColor = (Is_Received ? ConsoleColor.Red : ConsoleColor.Blue);

            str.Append("Card : " + Program.me.Cards[Cardid].Name);
            str.Append(' ', maxCard.Value - Program.me.Cards[Cardid].Name.Length);

            str.Append(" [" + LCategory[Category]);
            str.Append(' ', maxCategory.Value - LCategory[Category].Length);

            str.Append($"] Price = {Price.Value}");
            str.Append(' ', maxPrice.Value - Price.Value.ToString().Length - 3);

            str.Append($"{Currency.CurrencyNames[Price.currency].Name}");

            str.Append($" Tag = {Tag}");
            str.Append(' ', maxTag.Value - Tag.Length);

            str.Append($" Time = {dateTime.ToShortDateString()}");
            str.Append(' ', maxTime.Value - dateTime.ToShortDateString().Length);

            return str.ToString();
        }
    }
}