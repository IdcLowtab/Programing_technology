using System.Text;

namespace Bank;

/// <summary>
/// Тип операции, попадающей в журнал аудита.
/// </summary>
public enum AuditOperationType
{
    /// <summary>
    /// Пополнение счёта.
    /// </summary>
    Deposit = 0,

    /// <summary>
    /// Списание со счёта.
    /// </summary>
    Withdrawal = 1,

    /// <summary>
    /// Начисление процентов.
    /// </summary>
    Interest = 2
}

/// <summary>
/// Запись журнала аудита.
/// </summary>
/// <param name="AccountNumber">Номер счёта, по которому прошла операция.</param>
/// <param name="Type">Тип операции.</param>
/// <param name="Amount">Сумма операции в рублях.</param>
/// <param name="Date">Дата и время операции.</param>
public record AuditRecord(string AccountNumber, AuditOperationType Type, decimal Amount, DateTime Date);

/// <summary>
/// Журнал аудита: список операций, выполненных по всем счетам приложения.
/// </summary>
public class AuditLog
{
    private static readonly AuditLog s_instance = new AuditLog();
    private readonly List<AuditRecord> _records = new List<AuditRecord>();

    private AuditLog()
    {
    }

    /// <summary>
    /// Единственный экземпляр журнала аудита на всё приложение.
    /// </summary>
    public static AuditLog Instance => s_instance;

    /// <summary>
    /// Все записи журнала в порядке добавления.
    /// </summary>
    public IReadOnlyList<AuditRecord> Records => _records;

    /// <summary>
    /// Записывает операцию в журнал аудита.
    /// </summary>
    /// <param name="accountNumber">Номер счёта, по которому прошла операция.</param>
    /// <param name="type">Тип операции.</param>
    /// <param name="amount">Сумма операции в рублях.</param>
    /// <param name="date">Дата и время операции.</param>
    public void Write(string accountNumber, AuditOperationType type, decimal amount, DateTime date)
    {
        _records.Add(new AuditRecord(accountNumber, type, amount, date));
    }

    /// <summary>
    /// Возвращает журнал аудита в виде текстовой таблицы.
    /// </summary>
    /// <returns>Текст со всеми записями журнала.</returns>
    public string GetReport()
    {
        var report = new StringBuilder();
        report.AppendLine("Account\tType\tAmount\tDate");
        foreach (var record in _records)
        {
            report.AppendLine(
                $"{record.AccountNumber}\t" +
                $"{record.Type}\t" +
                $"{record.Amount}\t" +
                $"{record.Date:yyyy-MM-dd HH:mm:ss}");
        }

        return report.ToString();
    }
}