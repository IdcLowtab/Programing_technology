namespace Bank;

/// <summary>
/// Демонстрация сценариев банковского приложения.
/// </summary>
public class Program
{
    /// <summary>
    /// Прогоняет основные сценарии работы со счетами.
    /// </summary>
    /// <param name="args">Аргументы командной строки. Не используются.</param>
    static void Main(string[] args)
    {

        BankAccount account1 = new BankAccount("Yana1", 100000);
        BankAccount account2 = new BankAccount("Lena", 10);
        Console.WriteLine($"account {account1.Balance} №{account1.Number} {account1.Owner}");
        Console.WriteLine($"account {account2.Balance} №{account2.Number} {account2.Owner}");

        account1.MakeDeposit(2000000, DateTime.UtcNow, ":)");
        Console.WriteLine(account1.Balance);
        account1.MakeWithdrawal(200, DateTime.UtcNow, ":(");
        Console.WriteLine(account1.Balance);
        Console.WriteLine(account1.GetAccountHistory());
        try
        {
            account2.MakeWithdrawal(10000, DateTime.UtcNow, ":(");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
        InterestEarningAccount interest = new InterestEarningAccount("Yana", 1000m);
        interest.PerformMonthAndTransactions();
        Console.WriteLine(interest.GetAccountHistory());
        //interestEarning.MakeDeposit(1000m, DateTime.UtcNow, ";)");
        //interestEarning.MakeWithdrawal(10m, DateTime.UtcNow, ";(");
        //interestEarning.PerformMonthAndTransactions();
        //Console.WriteLine(interestEarning); // auto send ToString()


        LineOfCreditAccount lineOfCredit = new LineOfCreditAccount("Yana", 0, 1000m);
        lineOfCredit.MakeWithdrawal(500m, DateTime.UtcNow, "credit");

        GiftCardAccount giftcart = new GiftCardAccount("Yana", 1000m, 5000m);

        List<BankAccount> accounts = new List<BankAccount>();
        accounts.Add(account1);
        accounts.Add(interest);
        accounts.Add(lineOfCredit);
        accounts.Add(giftcart);

        foreach (BankAccount account in accounts)
        {
            Console.WriteLine(account);
            account.PerformMonthAndTransactions();
            Console.WriteLine(account.GetAccountHistory());
        }

        try
        {
            lineOfCredit.MakeWithdrawal(600m, DateTime.UtcNow, "credit");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
        }
        Console.WriteLine(lineOfCredit.GetAccountHistory());

        Console.WriteLine("=== Задача 1. Статусы клиента ===");
        BankAccount regular = new BankAccount("Regular", 20000m, 0, ClientStatus.Regular);
        BankAccount premium = new BankAccount("Premium", 20000m, 0, ClientStatus.Premium);
        BankAccount vip = new BankAccount("VIP", 20000m, 0, ClientStatus.VIP);

        regular.PerformMonthAndTransactions();
        premium.PerformMonthAndTransactions();
        vip.PerformMonthAndTransactions();

        Console.WriteLine($"Regular: было 20000, стало {regular.Balance}");
        Console.WriteLine($"Premium: было 20000, стало {premium.Balance}");
        Console.WriteLine($"VIP: было 20000, стало {vip.Balance}");
        Console.WriteLine(vip.GetAccountHistory());

        Console.WriteLine("=== Задача 1. Кэшбэк VIP ===");
        vip.MakeWithdrawal(1000m, DateTime.UtcNow, "shopping");
        Console.WriteLine($"VIP после списания 1000: {vip.Balance}");
        Console.WriteLine(vip.GetAccountHistory());

        Console.WriteLine("=== Задача 3. Перевод между счетами ===");
        decimal beforeFirst = account1.Balance;
        decimal beforeSecond = account2.Balance;
        account1.Transfer(account2, 500m, DateTime.UtcNow, "transfer");
        Console.WriteLine($"Первый счёт: было {beforeFirst}, стало {account1.Balance}");
        Console.WriteLine($"Второй счёт: было {beforeSecond}, стало {account2.Balance}");

        Console.WriteLine("=== Задача 2. Журнал аудита ===");
        Console.WriteLine(AuditLog.Instance.GetReport());
    }
}