namespace EstateView.Core.Model
{
    public class Transaction
    {
        public Transaction(int year, decimal amount, decimal balance, string description)
        {
            this.Year = year;
            this.Amount = amount;
            this.Balance = balance;
            this.Description = description;
        }

        public int Year { get; private set; }
        public decimal Amount { get; private set; }
        public decimal Balance { get; private set; }
        public string Description { get; set; }
    }
}