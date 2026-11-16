using System;

namespace DZ8
{
    class Laba8
    {
        private static void Main()
        {
            Task();
            Homework();
        }

        private static void Task()
        {
            BankAccount account1 = new BankAccount(10000m, TypesOfAccount.Current);
            BankAccount account2 = new BankAccount(100m, TypesOfAccount.Current);

            account2.Transfer(account1, 1500m);
            account1.Withdraw(2000m);
            account1.Withdraw(600m);
            account1.Withdraw(700m);
            account1.Deposit(3000m);
            account1.Deposit(4000m);
            account2.Deposit(900m);
            account2.Deposit(3000m);
            account2.Deposit(3000m);
            account2.Deposit(3000m);
            account2.Withdraw(700m);
            account2.Withdraw(900m);

            account2.Dispose();
            account1.Dispose();

        }

        private static void Homework()
        {
            Song mySong = new Song();
            Console.WriteLine(mySong.Title());

            Song song1 = new Song("Нервы", "Нервы");
            Song song2 = new Song("Как на войне", "Агата Кристи", song1);
        }
    }
}
