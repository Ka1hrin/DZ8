using System;

namespace DZ8
{
    internal class BankTransaction
    {
        private readonly decimal _amount;
        private readonly DateTime _created;

        public BankTransaction(decimal amount)
        {
            _amount = amount;
            _created = DateTime.Now;
        }

        public decimal Amount
        {
            get { return _amount; }
        }

        public DateTime Created
        {
            get { return _created; }
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Дата пополнения: {_created}, сумма пополнения: {_amount}");
        }
    }
}
