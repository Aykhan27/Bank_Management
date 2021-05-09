using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace kursac
{
    class Accaunt
    {
        public Currency Balance = 0;
        public Currency Expense = 0;
        public Currency Income = 0;
        public List<Card> Cards = new List<Card> { new Card(new Currency(0,0), "my card") };

        public List<string> GetCardNames()
        {
            List<string> _new = new List<string>(Cards.Count);
            foreach (var item in Cards)
            {
                _new.Add(item.Name);
            }
            return _new;
        }

        public void Reset()
        {
            Balance.Value = 0;
            Expense.Value = 0;
            Income.Value = 0;
            for (int i = 0; i < Program.me.Cards.Count; i++)
            {
                var item = Cards[i];
                item.Balance.Value = 0;
                foreach (var Titem in Program.FilteringTransfers)
                {
                    if (Titem.Cardid == i)
                    {
                        if (Titem.Is_Received)
                        {
                            item.Balance -= Titem.Price;
                            Expense += Titem.Price;
                        }
                        else
                        {
                            item.Balance += Titem.Price;
                            Income += Titem.Price;
                        }
                    }
                }
                Balance += item.Balance;
            }
        }

        public void EditCard(Rect rect)
        {
            Window.Draw(rect);
            switch (Window.Draw_Choose(rect.ChangeWidth(-rect.width / 2 - 1), Program.CardCategories))
            {
                case 0:
                    string str_name = Window.Draw_Read_Line(new Rect(rect.left + rect.width / 2 + 2, rect.top + 2, rect.width / 2 - 3, 2), "Enter Card Name");
                    int cur = Window.Draw_Choose(new Rect(rect.left + rect.width / 2 + 2, rect.top + 6, rect.width / 2 - 3,rect.height - 6 - 1), Program.CurrencyCategories);
                    Cards.Add(new Card(new Currency(0,cur), str_name));
                    break;
                case 1:
                    Window.Clear(new Rect(rect.left + 1, rect.top + 1, rect.width / 2 - 2, rect.height / 2 - 2));
                    switch (Window.Draw_Choose(rect.ChangeWidth(-rect.width / 2 - 1), Program.CardEditCategories))
                    {
                        case 0:
                            Window.Clear(new Rect(rect.left + 1, rect.top + 1, rect.width / 2 - 2, rect.height / 2 - 2));
                            int index = Window.Draw_Choose(rect.ChangeWidth(-rect.width / 2 - 1), GetCardNames());
                            Cards[index].Name = Window.Draw_Read_Line(new Rect(rect.left + rect.width / 2 + 2, rect.top + 2, rect.width / 2 - 4, 2), "Enter Card Name");
                            break;
                        case 1:
                            Window.Clear(new Rect(rect.left + 1, rect.top + 1, rect.width / 2 - 2, rect.height / 2 - 2));
                            index = Window.Draw_Choose(rect.ChangeWidth(-rect.width / 2 - 1), GetCardNames());
                            int to_currency = Window.Draw_Choose(new Rect(rect.left + rect.width / 2 + 1, rect.top , rect.width / 2, rect.height), Program.CurrencyCategories);
                            Currency.ConvertMoney(ref Cards[index].Balance, to_currency);
                            break;
                    }
                    break;
                case 2:
                    if (Cards.Count > 0)
                    {
                        int index = Window.Draw_Choose(rect.ChangeLeft(rect.width / 2 + 1).ChangeWidth(-rect.width / 2 - 1), GetCardNames());
                        Cards.RemoveAt(index);
                        for (int i = 0; i < Program.transfers.Count; i++)
                            if (Program.transfers[i].Cardid == index)
                                Program.transfers.RemoveAt(i--);
                        Program.FilteringTransfers = Program.transfers.Where(x => Program.min <= x.dateTime && x.dateTime < Program.max).ToList();
                        Transfers.Reset();
                    }
                    else Errors.Add("You dont have a card!");
                    break;

            }     
        }

        public void EditCurrencyes(Rect rect)
        {
            switch(Window.Draw_Choose(rect.ChangeWidth(-rect.width / 2 - 1),Program.CurrencyEditCategories))
            {
                case 0:
                    Currency.ResetCurencies();
                    break;
                case 1:
                    int cur = Window.Draw_Choose(rect.ChangeLeft(rect.width / 2).ChangeWidth(-rect.width / 2), Program.CurrencyCategories);                   
                    Currency.ConvertMoney(ref Balance,cur);
                    Currency.ConvertMoney(ref Expense,cur);
                    Currency.ConvertMoney(ref Income,cur);
                    break;
            }
        }

        public void Draw(Rect rect)
        {
            Window.Draw(rect);
            Console.SetCursorPosition(rect.left + 2,rect.top + rect.height / 2);
            Console.Write($"Balance : {Balance}    Expense : {Expense}    Income : {Income}     {Program.min.ToLongDateString()}  -  {Program.max.ToLongDateString()}");
            Console.SetCursorPosition(rect.left + 2,rect.top + rect.height / 2 + 1);
            foreach (var item in Cards) Console.Write($"Card : {item.Name} = {item.Balance}    ");
        }
    }
}