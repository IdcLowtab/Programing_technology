using System;
using System.Collections.Generic;
using System.Text;

namespace Bank;

/// <summary>
/// Подарочная карта: ежемесячно пополняется на фиксированную сумму.
/// </summary>
public class GiftCardAccount : BankAccount
{
    private readonly decimal _monthlyDeposit = 0m;

    /// <summary>
    /// Создаёт подарочную карту.
    /// </summary>
    /// <param name="name">Имя владельца карты.</param>
    /// <param name="initialBalance">Начальный остаток.</param>
    /// <param name="monthlyDeposit">Сумма ежемесячного пополнения.</param>
    /// <param name="status">Статус клиента, влияющий на комиссии и кэшбэк.</param>
    public GiftCardAccount(string name, decimal initialBalance, decimal monthlyDeposit = 0, ClientStatus status = ClientStatus.Regular)
        : base(name, initialBalance, 0, status)
        => _monthlyDeposit = monthlyDeposit;

    /// <summary>
    /// Пополняет карту и учитывает комиссию базового счёта.
    /// </summary>
    public override void PerformMonthAndTransactions()
    {
        // Без этой строки комиссия базового счёта не начисляется вообще.
        base.PerformMonthAndTransactions();

        if (_monthlyDeposit != 0)
        {
            MakeDeposit(_monthlyDeposit, DateTime.UtcNow, "Add monthly deposit");
        }
    }

    /// <summary>
    /// Возвращает описание счёта с указанием ежемесячного пополнения.
    /// </summary>
    /// <returns>Текстовое описание счёта.</returns>
    public override string ToString() => base.ToString() + $"monthly deposit: {_monthlyDeposit}";
}