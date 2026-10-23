namespace Bank;

// record - Состояние объектов этого класса нельзя изменить

/// <summary>
/// Запись об одной банковской операции.
/// </summary>
/// <param name="Amount">Сумма операции со знаком: положительная — пополнение, отрицательная — списание.</param>
/// <param name="Date">Дата и время операции.</param>
/// <param name="Note">Комментарий к операции.</param>
public record Transaction(decimal Amount, DateTime Date, string Note);

//internal record Transaction
//{
//    public decimal Amount { get; }
//    public DateTime Date { get; }
//    public string Note { get; }
//    public Transaction(decimal Amount, DateTime Date, string Note)
//    {
//        this.Amount = Amount;
//        this.Note = Note;
//        this.Date = Date;
//    }
//}