using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CqrsBank.Domain.Models;

namespace CqrsBank.Infrastructure
{
  public class AccountContext : DbContext
  {
    public AccountContext() : this("CqrsBank")
    {
    }

    public AccountContext(string connectionStringName) 
      : base(connectionStringName)
    {
      // Migrations...no thank you
      Database.SetInitializer<AccountContext>(null);
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      // Plural table names...no thank you
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

      modelBuilder.Entity<BankAccount>()
        .HasMany(x => x.Transactions)
        .WithRequired(x => x.Account)
        .HasForeignKey(x => x.AccountId);
    }

    public virtual DbSet<BankAccount> Accounts { get; set; }
  }
}