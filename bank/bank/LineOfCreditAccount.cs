using System;
using System.Collections.Generic;
using System.Text;

namespace Bank;

/// <summary>
/// Кредитный счёт: позволяет уходить в минус до предела кредитного лимита.
/// </summary>
public class LineOfCreditAccount : BankAccount
{
    /// <summary>
    /// Создаёт кредитный счёт.
    /// </summary>
    /// <param name="name">Имя владельца счёта.</param>
    /// <param name="initialBalance">Начальный остаток.</param>
    /// <param name="creditLimit">Максимальный размер кредита, на который можно уйти в минус.</param>
    /// <param name="status">Статус клиента, влияющий на комиссии и кэшбэк.</param>
    public LineOfCreditAccount(string name, decimal initialBalance, decimal creditLimit, ClientStatus status = ClientStatus.Regular)
        : base(name, initialBalance, -creditLimit, status)
    {

    }

}