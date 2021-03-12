using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ChangeCalculator
{
    public sealed class ChangeCalculator
    {
        private string UserName { get; set; }
        private List<Money> Monies { get; set; }
        private List<Product> Products { get; set; }
        private Product ProductToPurchase { get; set; }
        private Money NoteToUse { get; set; }

        static ChangeCalculator() { }

        private ChangeCalculator() { }

        public static ChangeCalculator Instance { get; } = new ChangeCalculator();

        public void BuyProduct()
        {
            GetUserName();

            LoadCurrency();
            LoadProducts();

            SelectProduct();
        }

        private void SelectProduct()
        {
            Console.WriteLine("\r\nPlease select a product to buy (type 1, 2 or 3)\r\n");

            foreach (Product product in Products)
            {
                Console.WriteLine(string.Format("{0} - Price: £{1}", product.Name, product.Price.ToString("0.00").Replace(",", ".")));
            }

            string productNumber = Console.ReadLine();
            ProductToPurchase = Products[int.Parse(productNumber) - 1];

            SelectMoney();
        }

        private void SelectMoney()
        {
            Console.WriteLine("\r\nPlease select a bank note to use for your purchase (type 1, 2, 3..)\r\n");
            List<Money> bankNotes = Monies.Where(money => money.Type == MoneyType.BankNote).ToList();

            foreach (Money money in bankNotes)
            {
                Console.WriteLine(string.Format("£{0}", money.Denomination));
            }

            string noteNumber = Console.ReadLine();
            NoteToUse = bankNotes[int.Parse(noteNumber) - 1];

            ShowChange();
        }

        private void ShowChange()
        {
            List<Change> changeList = new List<Change>();
            double changeRemaining = NoteToUse.Denomination - ProductToPurchase.Price;

            foreach (Money money in Monies.OrderByDescending(money => money.Denomination))
            {
                int amount = (int)Math.Floor(changeRemaining / money.Denomination);

                if (amount > 0) changeList.Add(new Change(money, amount));
                changeRemaining = Math.Round(changeRemaining - amount * money.Denomination, 2);
            }

            Console.WriteLine("\r\nYour change is:");
            foreach (Change change in changeList)
            {
                Console.WriteLine(string.Format("{0} x {1}", change.Amount, change.Money.GetValueName()));
            }

            Console.WriteLine("\r\nThank you for your purchase!");

            Console.WriteLine("\r\nWould you like to make another purchase? y/n");
            string response = Console.ReadLine();
            switch (response)
            {
                case "y":
                    SelectProduct();
                    break;
                default:
                    return;
            }
        }

        private void GetUserName()
        {
            Console.WriteLine("Hi! Please enter your name. ");

            UserName = Console.ReadLine();
            Console.WriteLine(string.Format("\r\nHi! {0}", UserName));
        }

        private void LoadProducts()
        {
            Products = new List<Product>();
            Products.Add(new Product("Developer", 3.14));
            Products.Add(new Product("Lightbulb", 1));
            Products.Add(new Product("Hardware Problem", 4.7));
        }

        private void LoadCurrency()
        {
            Monies = new List<Money>();

            Console.WriteLine("\r\nDo you have a custom currency to load? (A Money.xml file in the same directory as the .exe) y/n");
            string response = Console.ReadLine();
            switch (response)
            {
                case "y":
                    LoadCustomCurrency();
                    break;
                default:
                    LoadDefaultCurrency();
                    break;
            }
        }

        private void LoadDefaultCurrency()
        {
            Monies.Add(new Money("Note", "50"));
            Monies.Add(new Money("Note", "20"));
            Monies.Add(new Money("Note", "10"));
            Monies.Add(new Money("Note", "5"));
            Monies.Add(new Money("Coin", "2"));
            Monies.Add(new Money("Coin", "1"));
            Monies.Add(new Money("Coin", "0.5"));
            Monies.Add(new Money("Coin", "0.2"));
            Monies.Add(new Money("Coin", "0.1"));
            Monies.Add(new Money("Coin", "0.02"));
            Monies.Add(new Money("Coin", "0.01"));
        }

        private void LoadCustomCurrency()
        {
            XDocument xmlMonies = XDocument.Load("Money.xml");
            foreach (XElement xmlMoney in xmlMonies.Descendants("money"))
            {
                string type = xmlMoney.Element("type").Value;
                string value = xmlMoney.Element("value").Value;

                Monies.Add(new Money(type, value));
            }
        }
    }
}
