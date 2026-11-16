namespace bank;
/// <summary>
/// Представляет банковский счет подарочной карты, который позволяет делать ежемесячные депозиты на счет.
/// </summary>
public class GiftCardAccount : BankAccount
{
    private readonly decimal _monthlyDeposit = 0m;

    // monthlyDeposit - параметр по умолчанию, по умолчанию принимает 0,
    // при создании new GiftCardAccount("Ulyana", 1000); - monthlyDeposit = 0
    // new GiftCardAccount("Ulyana", 1000, 5000) => monthlyDeposit = 5000
    /// <summary>
    /// Инициализирует новый экземпляр класса GiftCardAccount с
    /// указанным именем владельца, начальным балансом и необязательным ежемесячным депозитом.
    /// </summary>
    /// <param name="name"> имя человека для транзакции</param>
    /// <param name="initialBalance"> начальный баланс</param>
    /// <param name="monthlyDeposit"> ежемесячный депозит</param>
    public GiftCardAccount(string name, decimal initialBalance, decimal monthlyDeposit = 0)
        : base(name, initialBalance)
        => _monthlyDeposit = monthlyDeposit;
    /// <summary>
    /// Переопределяет метод PerformMonthAndTransactions() базового класса BankAccount
    /// для выполнения ежемесячного депозита на счет подарочной карты.
    /// </summary>
    public override void PerformMonthAndTransactions()
    {
        if (_monthlyDeposit != 0)
        {
            MakeDeposit(_monthlyDeposit, DateTime.UtcNow, "Add monthly deposit");
        }
    }
    /// <summary>
    /// Переопределяет метод ToString() базового класса
    /// BankAccount для предоставления строкового представления
    /// </summary>
    /// <returns>строку с информацией о счете подарочной карты</returns>
    public override string ToString() => base.ToString() + $"monthly deposit: {_monthlyDeposit}";
}
