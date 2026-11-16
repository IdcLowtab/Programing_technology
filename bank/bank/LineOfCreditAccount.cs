namespace bank;
/// <summary>
/// Представляет банковский счет с кредитной линией,
/// позволяя овердрафт до определенного лимита и применяя комиссию при перерасходе
/// </summary>
public class LineOfCreditAccount : BankAccount
{
    private decimal _creditLimit;
    /// <summary>
    /// Инициализирует новый экземпляр класса LineOfCreditAccount
    /// с указанным именем владельца, начальным балансом и кредитным лимитом.
    /// </summary>
    /// <param name="name"> имя человека для транзакции</param>
    /// <param name="initialBalance"> начальный баланс</param>
    /// <param name="creditLimit"> кредитный лимит</param>
    public LineOfCreditAccount(string name, decimal initialBalance, decimal creditLimit)
        : base(name, initialBalance, -creditLimit)
    {
        _creditLimit = creditLimit;
    }

    // Разрешаем овердрафт по кредитной линии, но применяем комиссию при перерасходе
    /// <summary>
    /// Проверяет, превышен ли кредитный лимит при снятии средств.
    /// </summary>
    /// <param name="isOverdrawn">превышение лимита</param>
    /// <returns>транзакция с комиссией или null</returns>
    protected override Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        // если не овердрафт — всё в порядке
        if (!isOverdrawn) return null;

        // при овердрафте взимаем фиксированную плату
        return new Transaction(-20m, DateTime.UtcNow, "Apply overdraft fee");
    }
}
