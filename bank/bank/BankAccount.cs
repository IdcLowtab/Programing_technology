using System.Text;
using static Bank.AuditLog;

namespace Bank;


// потомок класса object => можно переопределить
// виртуальные методы, находящиеся в object

/// <summary>
/// Базовый счёт: хранит историю операций и умеет пополняться и списывать деньги.
/// </summary>
public class BankAccount
{
    // Все ставки и пороги зашиты в код прямо здесь.
    // Поменять их можно только переписыванием класса.
    private const decimal RegularServiceFee = 50m;
    private const decimal PremiumRateOverThreshold = 0.01m;
    private const decimal VipRateOverThreshold = 0.03m;
    private const decimal ExcessThreshold = 10_000m;
    private const decimal VipCashbackRate = 0.05m;

    private readonly decimal _minimumBalance;
    static private int s_accountNuberSeed = 1000000000;

    /// <summary>
    /// Номер счёта, который банк выдаёт автоматически при создании счёта.
    /// </summary>
    public string Number { get; }

    /// <summary>
    /// Имя владельца счёта.
    /// </summary>
    public string Owner { get; private set; }

    /// <summary>
    /// Статус клиента, от которого зависит комиссия за обслуживание и кэшбэк.
    /// </summary>
    public ClientStatus Status { get; }

    /// <summary>
    /// Текущий остаток на счёте.
    /// </summary>
    public decimal Balance
    {
        get
        {
            decimal balance = 0;
            foreach (var item in _allTransactions)
            {
                balance += item.Amount;
            }

            return balance;
        }
    }

    private List<Transaction> _allTransactions = new List<Transaction>();

    /// <summary>
    /// Создаёт новый счёт и зачисляет на него начальный остаток.
    /// </summary>
    /// <param name="name">Имя владельца счёта.</param>
    /// <param name="initialBalance">Начальный остаток. Ноль и отрицательные значения игнорируются.</param>
    public BankAccount(string name, decimal initialBalance) : this(name, initialBalance, 0)
    {

    }

    /// <summary>
    /// Создаёт новый счёт с минимальным остатком и статусом клиента.
    /// </summary>
    /// <param name="name">Имя владельца счёта.</param>
    /// <param name="initialBalance">Начальный остаток. Ноль и отрицательные значения игнорируются.</param>
    /// <param name="minimumBalance">Минимальный остаток, ниже которого списание запрещено.</param>
    /// <param name="status">Статус клиента, влияющий на комиссии и кэшбэк.</param>
    public BankAccount(string name, decimal initialBalance, decimal minimumBalance, ClientStatus status = ClientStatus.Regular)
    {

        Owner = name; // this.Owner = name
        Number = s_accountNuberSeed.ToString();
        s_accountNuberSeed++;

        Status = status;
        _minimumBalance = minimumBalance;

        if (initialBalance > 0)
            MakeDeposit(initialBalance, DateTime.UtcNow, "Inital balance");
    }

    /// <summary>
    /// Пополняет счёт и пишет операцию в журнал аудита.
    /// </summary>
    /// <param name="amount">Сумма пополнения. Должна быть положительной.</param>
    /// <param name="date">Дата операции.</param>
    /// <param name="note">Комментарий к операции.</param>
    /// <param name="operationType">Тип операции для журнала аудита.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Бросается, если <paramref name="amount"/> меньше или равен нулю.
    /// </exception>
    public void MakeDeposit(decimal amount, DateTime date, string note, AuditOperationType operationType = AuditOperationType.Deposit)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
        }

        var deposit = new Transaction(amount, date, note);
        _allTransactions.Add(deposit);
        AuditLog.Instance.Write(Number, operationType, amount, date);
    }

    /// <summary>
    /// Списывает деньги со счёта и начисляет кэшбэк клиенту со статусом VIP.
    /// </summary>
    /// <param name="amount">Сумма списания. Должна быть положительной.</param>
    /// <param name="date">Дата операции.</param>
    /// <param name="note">Комментарий к операции.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Бросается, если <paramref name="amount"/> меньше или равен нулю.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Бросается, если после списания остаток уходит ниже минимального.
    /// </exception>
    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        Transaction? overdraftTransaction = CheckWithdrawalLimit(Balance - amount < _minimumBalance);
        Transaction? withdrawal = new(-amount, date, note);

        _allTransactions.Add(withdrawal);
        AuditLog.Instance.Write(Number, AuditOperationType.Withdrawal, amount, date);

        if (overdraftTransaction is not null)
            _allTransactions.Add(overdraftTransaction);

        // Кэшбэк привязан к методу списания, а не к конкретной операции,
        // поэтому начисляется и на комиссию за обслуживание.
        if (Status == ClientStatus.VIP)
        {
            decimal cashback = amount * VipCashbackRate;
            MakeDeposit(cashback, date, $"Cashback {VipCashbackRate * 100}% for withdrawal {amount}");
        }
    }

    /// <summary>
    /// Проверяет, допустимо ли списание за пределы минимального остатка.
    /// </summary>
    /// <param name="isOvesdrawn">Признак того, что остаток уходит ниже минимального.</param>
    /// <returns>
    /// Транзакция для оформления овердрафта либо <see langword="null"/>, если овердрафт не требуется.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Бросается, если списание недопустимо.
    /// </exception>
    protected virtual Transaction? CheckWithdrawalLimit(bool isOvesdrawn)
    {
        if (isOvesdrawn)
        {
            throw new InvalidOperationException("Not sufficient rubls for this withdrawal");
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Возвращает историю операций по счёту в виде таблицы.
    /// </summary>
    /// <returns>Текст с историей операций.</returns>
    public string GetAccountHistory()
    {
        var report = new StringBuilder();
        decimal balance = 0;
        report.Append("Data\t\tAmount\tBalance\tNote");
        foreach (var item in _allTransactions)
        {
            balance += item.Amount;
            report.AppendLine($"" +
                $"{item.Date.ToShortDateString()}\t" +
                $"{item.Amount}\t{balance}\t{item.Note}");
        }
        return report.ToString();
    }

    /// <summary>
    /// Начисляет комиссию за обслуживание в зависимости от статуса клиента.
    /// </summary>
    public virtual void PerformMonthAndTransactions()
    {
        decimal commission = CalculateServiceCommission();
        if (commission <= 0)
            return;

        try
        {
            MakeWithdrawal(commission, DateTime.UtcNow, $"Service commission ({Status})");
        }
        catch (InvalidOperationException)
        {
            // Денег на счёте не хватило, комиссию не списываем.
            // Счёт сам про себя не знает, что пополнения не будет.
        }
    }

    /// <summary>
    /// Считает комиссию за обслуживание для текущего статуса клиента.
    /// </summary>
    /// <returns>Сумма комиссии в рублях.</returns>
    private decimal CalculateServiceCommission()
    {
        return Status switch
        {
            ClientStatus.Regular => RegularServiceFee,
            ClientStatus.Premium => Balance > ExcessThreshold
                ? (Balance - ExcessThreshold) * PremiumRateOverThreshold
                : 0m,
            ClientStatus.VIP => Balance > ExcessThreshold
                ? (Balance - ExcessThreshold) * VipRateOverThreshold
                : 0m,
            _ => 0m
        };
    }

    /// <summary>
    /// Переводит сумму со счёта на другой счёт.
    /// </summary>
    /// <param name="destination">Счёт-получатель.</param>
    /// <param name="amount">Сумма перевода. Должна быть положительной.</param>
    /// <param name="date">Дата операции.</param>
    /// <param name="note">Комментарий к операции.</param>
    /// <exception cref="ArgumentNullException">
    /// Бросается, если <paramref name="destination"/> не задан.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Бросается, если <paramref name="amount"/> меньше или равен нулю.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Бросается, если списание со счёта недопустимо.
    /// </exception>
    public void Transfer(BankAccount destination, decimal amount, DateTime date, string note)
    {
        ArgumentNullException.ThrowIfNull(destination);

        MakeWithdrawal(amount, date, note);

        try
        {
            destination.MakeDeposit(amount, date, note);
        }
        catch (InvalidOperationException)
        {
            // Зачисление не получилось — возвращаем деньги обратно отправителю.
            // Откат — это тоже операция, и она тоже может не получиться.
            MakeDeposit(amount, date, $"Rollback of failed transfer to {destination.Number}");
            throw;
        }
    }

    // Ключевое слово virtual позволяет в дочернем классе
    // предоставить другую реализацию
    // Метода PerformMonthAndTransactions
    // переопределяем метод базового класса - класса object
    // toString возвращает строку с информацией об объекте
    /// <summary>
    /// Возвращает описание счёта: тип, владелец, номер и остаток.
    /// </summary>
    /// <returns>Текстовое описание счёта.</returns>
    public override string ToString()
    => $"Type:{GetType().Name}\tOwner:{Owner}\tNumber of account:{Number}\tBalance:{Balance}";
}