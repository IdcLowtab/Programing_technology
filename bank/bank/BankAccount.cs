using System.Text;

namespace bank;
//BankAccount - потомок класса object => можно переопределить виртуальные методы, такие как ToString(), Equals() и GetHashCode()
/// <summary>
/// Представляет банковский счет с возможностью внесения депозитов,
/// снятия средств и отслеживания истории транзакций.
/// </summary>
public class BankAccount
{
    private readonly decimal _minimumBalance;
    static private int s_accountNumberSeed = 1000000000;
    public string Number { get; }
    public string Owner { get; private set; }
    private List<Transaction> _allTransactions = new List<Transaction>();
    /// <summary>
    /// Возвращает текущий баланс счета,
    /// вычисляемый на основе всех транзакций.
    /// </summary>
    public decimal Balance
    {
        get
        {
            decimal balance = 0;
            foreach (var transaction in _allTransactions)
            {
                balance += transaction.Amount;
            }
            return balance;
        }
    }
    /// <summary>
    /// Инициализирует новый экземпляр класса BankAccount 
    /// с указанным именем владельца и начальным балансом.
    /// </summary>
    /// <param name="name">Имя владельца счета</param>
    /// <param name="initialBalance">Начальный баланс</param>
    public BankAccount(string name, decimal initialBalance) : this(name, initialBalance, 0)
    {

    }
    /// <summary>
    /// Инициализирует новый экземпляр класса BankAccount с указанным именем владельца,
    /// начальным балансом и минимальным балансом.
    /// </summary>
    /// <param name="name">Имя владельца счета</param>
    /// <param name="initialBalance">Начальный баланс</param>
    /// <param name="minimumBalance">Минимальный баланс</param>
    public BankAccount(string name, decimal initialBalance, decimal minimumBalance)
    {
        Owner = name;
        Number = s_accountNumberSeed.ToString();
        s_accountNumberSeed++;

        _minimumBalance = minimumBalance;

        if (initialBalance > 0)
            MakeDeposit(initialBalance, DateTime.UtcNow, "Initial balance");
    }
    /// <summary>
    /// Вносит депозит на счет с указанной суммой, датой и заметкой.
    /// </summary>
    /// <param name="amount">Сумма депозита</param>
    /// <param name="date">Дата депозита</param>
    /// <param name="note">Примечание к депозиту</param>
    /// <exception cref="ArgumentOutOfRangeException">исключение на сумму депозита</exception>
    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
        }
        var deposit = new Transaction(amount, date, note);
        _allTransactions.Add(deposit);
    }
    /// <summary>
    /// Снимает средства со счета с указанной суммой, датой и заметкой.
    /// </summary>
    /// <param name="amount">Сумма снятия</param>
    /// <param name="date">Дата снятия</param>
    /// <param name="note">Примечание к снятию</param>
    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        Transaction? overdraftTransaction = CheckWithdrawalLimit(Balance - amount < _minimumBalance);
        Transaction? withdrawal = new(-amount, date, note);

        _allTransactions.Add(withdrawal);

        if (overdraftTransaction is not null)
            _allTransactions.Add(overdraftTransaction);
    }
    protected virtual Transaction? CheckWithdrawalLimit(bool isOverdrawn)
    {
        if (isOverdrawn)
        {
            throw new InvalidOperationException("Not sufficient rubls for this withdrawal");
        }
        else
        {
            // default - содержит значение по умолчанию,
            // так как тип возвращаемого значения - ссылочный, то
            // default = nul
            return default; // == return null
        }
    }
    /// <summary>
    /// Возвращает строковое представление истории транзакций счета.
    /// </summary>
    /// <returns>  </returns>
    public string GetAccountHistory()
    {
        var report = new StringBuilder();
        decimal balance = 0;
        report.AppendLine("Date\t\tAmount\tBalance\tNote");
        foreach (var item in _allTransactions)
        {
            balance += item.Amount;
            report.AppendLine($"" +
                $"{item.Date.ToShortDateString()}\t" +
                $"{item.Amount}\t{balance}\t{item.Note}");
        }
        return report.ToString();
    }
    // Ключевое слово virtual позволяет в дочернем классе предоставить другую реализацию 
    // Метода PerformMonthAndTransactions
    /// <summary>
    /// Выполняет ежемесячные операции и транзакции на счете.
    /// </summary>
    public virtual void PerformMonthAndTransactions()
    {

    }

    // переопределяем метод базового класса - класса object 
    // toString возвращает строку с информацией об объекте 
    /// <summary>
    /// Возвращает строковое представление объекта BankAccount, включая тип, владельца, номер счета и баланс.
    /// </summary>
    /// <returns>  </returns>
    public override string ToString()
    {
        return $"Type: {GetType().Name}\t" +
            $"Owner: {Owner}\t" +
            $"Number of account: {Number}\t" +
            $"Balance: {Balance}";
    }
}
