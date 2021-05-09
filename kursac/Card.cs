using System;
using System.Net;
using System.Collections.Generic;

namespace kursac
{
    class Card
    {
        public string Name;
        public Currency Balance;

        public Card(Currency _Currency,string _Name)
        {
            Name = _Name;
            Balance = _Currency;
        }
    }

    class Storoge
    {
        public string Name;
        public double Value;

        public Storoge() { }        

        public Storoge(string _Name, double _Value)
        {
            Name = _Name;
            Value = _Value;
        }
    }

    class Currency
    {
        public static List<Storoge> CurrencyNames = new List<Storoge> {
        new Storoge("AZN",0),
        new Storoge("USD",0),
        new Storoge("EUR",0)
        };

        public int Value;
        public int currency;

        public static object JsonConvert { get; private set; }

        public Currency(int _Value) => Value = _Value;

        public Currency(int _Value, int _Currency)
        {
            Value = _Value;
            currency = _Currency;
        }

        public static implicit operator Currency(int _Value) => new Currency(_Value);

        public static Currency operator +(Currency _this, Currency _second)
        {
            return new Currency(_this.Value + GetConvertMoney(_second, _this.currency), _this.currency);
        }
        public static Currency operator -(Currency _this, Currency _second) => new Currency(_this.Value - GetConvertMoney(_second, _this.currency), _this.currency);

        public static int GetConvertMoney(Currency _this,int toCurrency )
        {
            return Convert.ToInt32(_this.Value / CurrencyNames[_this.currency].Value * CurrencyNames[toCurrency].Value);
        }

        public static void ConvertMoney(ref Currency _this, int toCurrency)
        {
            _this.Value = Convert.ToInt32(_this.Value / CurrencyNames[_this.currency].Value * CurrencyNames[toCurrency].Value);
            _this.currency = toCurrency;
        }

        public static void ResetCurencies()
        {
            var key = "d78c53cd7b3b6660c60c";
            try
            {
                WebClient webClient = new WebClient();
                var url = "https://free.currconv.com/api/v7/convert?apiKey=" + key;
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(webClient.DownloadString(url + "&q=AZN_AZN,AZN_USD"));
                CurrencyNames[0].Value = result["results"]["AZN_AZN"]["val"];
                CurrencyNames[1].Value = result["results"]["AZN_USD"]["val"];
                result = Newtonsoft.Json.JsonConvert.DeserializeObject(webClient.DownloadString(url + "&q=AZN_EUR"));
                CurrencyNames[2].Value = result["results"]["AZN_EUR"]["val"];
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File = Cards.cs -> class = Currency -> foonction = ResetCurencies");
                Console.ResetColor();
                Console.WriteLine("Gleb Если тут я написан значит есть ошибка!!!");
                Console.WriteLine("1. Вы находитесь в \"Step It\"");
                Console.WriteLine("2. Не подключон инернет");
                Console.WriteLine("3. Ключ(" +  key + ") не работает");
                Console.WriteLine();
                Console.WriteLine("Made by Ruha 3 casa");
                Environment.Exit(0);
            }
        }

        public override string ToString()
        {
            return Value + " " + CurrencyNames[currency].Name;
        }
    }
}