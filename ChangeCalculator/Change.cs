namespace ChangeCalculator
{
    public class Change
    {
        public Money Money { get; set; }
        public int Amount { get; set; }

        public Change(Money money, int amount)
        {
            Money = money;
            Amount = amount;
        }
    }
}
