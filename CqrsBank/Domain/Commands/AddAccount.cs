using CqrsBank.Infrastructure;

namespace CqrsBank.Domain.Commands
{
  public class AddAccount : ICommand
  {
    public string Name { get; set; }
    public decimal OpeningBalance { get; set; }
  }
}