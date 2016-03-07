using System.Collections.Generic;
using CqrsBank.Domain.Commands;

namespace CqrsBank.Domain.Models
{
  public class BankAccount
  {
    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Balance { get; private set; }
    public virtual ICollection<Transaction> Transactions { get; private set; } 

    public BankAccount() { }

    public BankAccount(AddAccount command)
    {
      Name = command.Name;
      Balance = command.OpeningBalance;
    }

    public void AddTransaction(AddTransaction command)
    {
      Transactions.Add(new Transaction(command));
      Balance += command.Amount;
    }
  }
}