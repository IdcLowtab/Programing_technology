namespace bank;
/// <summary>
/// Представляет банковский счет с начислением процентов,
/// который начисляет проценты на баланс, если он превышает определенный порог.
/// </summary>
public class InterestEarningAccount : BankAccount
{
    /// <summary>
    /// Инициализирует новый экземпляр класса InterestEarningAccount
    /// с указанным именем владельца и начальным балансом.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="initialBalance"></param>
    public InterestEarningAccount(string name, decimal initialBalance)
         : base(name, initialBalance)
    {

    }
    // override позволяет в дочернем классе определить новую реализацию
    // метода PerformMonthAndTransactions
    /// <summary>
    /// Переопределяет метод PerformMonthAndTransactions()
    /// базового класса BankAccount
    /// </summary>
    public override void PerformMonthAndTransactions()
    {
        if (Balance > 500m)
        {
            decimal interest = Balance * 0.02m;
            MakeDeposit(interest, DateTime.UtcNow, "Apply month interest");
        }

    }
}