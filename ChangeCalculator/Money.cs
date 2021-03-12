using System;
using System.Globalization;

namespace ChangeCalculator
{
    public class Money
    {
        public MoneyType Type { get; set; }
        public double Denomination { get; set; }

        public Money(string type, string value)
        {
            Type = Money.GetMoneyType(type);
            Denomination = Money.GetDenominationAsDouble(value);
        }

        public string GetValueName()
        {
            string valueSign = Denomination < 1 ? "p" : "£";
            string value = (Denomination < 1 ? Denomination * 100 : Denomination).ToString();
            bool suffix = Denomination < 1;

            return suffix ? value + valueSign : valueSign + value;
        }

        public static MoneyType GetMoneyType(string type)
        {
            return type switch
            {
                "Note" => MoneyType.BankNote,
                "Coin" => MoneyType.Coin,
                _ => throw new ArgumentException("Money type not supported", type),
            };
        }

        public static double GetDenominationAsDouble(string value)
        {
            try
            {
                return double.Parse(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new ArgumentException("Money value is not numeric", value);
            }
        }
    }

    public enum MoneyType
    {
        BankNote,
        Coin
    }
}
