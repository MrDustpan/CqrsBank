using CqrsBank.Infrastructure;

namespace CqrsBank.Domain.Commands
{
  public class DeleteAccount : ICommand
  {
    public int Id { get; set; }
  }
}