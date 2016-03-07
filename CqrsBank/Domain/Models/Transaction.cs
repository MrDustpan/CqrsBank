using CqrsBank.Domain.Commands;

namespace CqrsBank.Domain.Models
{
  public class Transaction
  {
    public int Id { get; private set; }
    public int AccountId { get; private set; }
    public string Description { get; private set; }
    public decimal Amount { get; private set; }
    public virtual BankAccount Account { get; private set; }

    public Transaction()
    {
    }

    public Transaction(AddTransaction command)
    {
      Description = command.Description;
      Amount = command.Amount;
      AccountId = command.AccountId;
    }
  }
}