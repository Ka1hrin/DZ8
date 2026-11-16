using System;
using System.Collections.Generic;
using System.IO;

namespace DZ8
{
    public class BankAccount
    {
        private static int _bankId = 0;
        private int _bankNumber;
        private decimal _balance;
        private TypesOfAccount _account;
        private Queue<BankTransaction> _transactions;


        public BankAccount(decimal balance, TypesOfAccount account)
        {
            _bankNumber = BankId();
            Balance = balance;
            Account = account;
            _transactions = new Queue<BankTransaction>();
        }

        public int BankNumber
        {
            get { return _bankNumber; }
        }
        public decimal Balance
        {
            get { return _balance; }
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Баланс не может быть отрицательным!");
                _balance = value;
            }
        }

        public TypesOfAccount Account
        {
            get { return _account; }
            private set
            {
                if (!Enum.IsDefined(typeof(TypesOfAccount), value))
                    throw new ArgumentException("Некорректный тип счета!");
                _account = value;
            }
        }

        #region
        /// <summary>
        /// метод, который переводит деньги с счета на входе на текущий
        /// </summary>
        public void Transfer(BankAccount bankAccount, decimal money)
        {
            if (bankAccount == this)
                throw new ArgumentException("Вы не можете перевести деньги на этот же счет!");
            if (bankAccount == null)
                throw new ArgumentNullException(nameof(bankAccount), "Счет не может быть пустым!");

            bankAccount.Withdraw(money);

            this.Deposit(money);

            Console.WriteLine($"Перевод {money} руб. выполнен успешно!");
        }

        /// <summary>
        /// Метод считает уникальный номер счета
        /// </summary>
        private static int BankId()
        {
            return ++_bankId;
        }

        #endregion
        /// <summary>
        /// Метод проверяет баланс и снимает деньги со счета
        /// </summary>
        public decimal Withdraw(decimal value)
        {

            if (value > _balance)
                throw new ArgumentException("На счете недостаточно средств");
            if (value <= 0)
                throw new ArgumentException("Число не может быть отрицательным!");
            _balance -= value;
            BankTransaction bankTransaction = new BankTransaction(-value);
            _transactions.Enqueue(bankTransaction);
            
            return _balance;
        }

        /// <summary>
        /// Метод пополняет счет
        /// </summary>
        public decimal Deposit(decimal value)
        {
            if (value <= 0)
                throw new ArgumentException("Число не может быть отрицательным!");
            _balance += value;
            
            BankTransaction bankTransaction = new BankTransaction(value);
            _transactions.Enqueue(bankTransaction);

            return _balance;
        }

        /// <summary>
        /// Метод, который записывет все опрерации в файл
        /// using - необходим, чтобы файл в случае ошибки все равно был закрыт
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            using (StreamWriter file = new StreamWriter("Transactions.txt", true))
            {
                while (_transactions.Count > 0)
                {
                    BankTransaction item = _transactions.Dequeue();
                    file.WriteLine($"Номер счета: {_bankNumber}, Дата: {item.Created}, Сумма: {item.Amount}");
                }
            }

            Console.WriteLine($"История транзакций счета {_bankNumber} сохранена в файл Transactions.txt");

        }

        #region
        /// <summary>
        /// Метод печатает информацию о счете
        /// </summary>
        public void PrintInfo()
        {
            Console.WriteLine($"Номер счета: {_bankNumber}, тип счета: {BankHelp.EnumName(_account)}, баланс: {_balance}\n");
        }
    }
    internal static class BankHelp
    {
        /// <summary>
        /// Метод переводит тип TypesOfAccount в строку 
        /// </summary>
        public static string EnumName(TypesOfAccount account)
        {
            return account switch
            {
                TypesOfAccount.Current => "Текущий счет",
                TypesOfAccount.Savings => "Накопительный счет",
                _ => "Неизвестный тип счета!"
            };
        }
    }
    #endregion
}
