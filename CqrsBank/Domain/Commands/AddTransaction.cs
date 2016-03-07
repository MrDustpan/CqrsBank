using CqrsBank.Infrastructure;

namespace CqrsBank.Domain.Commands
{
  public class AddTransaction : ICommand
  {
    public int AccountId { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
  }
}