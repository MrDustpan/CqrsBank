using System.Configuration;

namespace CqrsBank.Infrastructure
{
  public class ConfigurationService : IConfigurationService
  {
    public string GetDefaultConnectionString()
    {
      return ConfigurationManager.ConnectionStrings["CqrsBank"].ConnectionString;
    }
  }

  public interface IConfigurationService
  {
    string GetDefaultConnectionString();
  }
}