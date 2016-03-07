using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CqrsBank.Infrastructure
{
  public class SqlConnectionFactory : IDbConnectionFactory
  {
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfigurationService config)
    {
      _connectionString = config.GetDefaultConnectionString();
    }

    public SqlConnectionFactory(string connectionString)
    {
      _connectionString = connectionString;
    }

    public IDbConnection GetOpenConnection()
    {
      var cn = new SqlConnection(_connectionString);
      cn.Open();
      return cn;
    }

    public async Task<IDbConnection> GetOpenConnectionAsync()
    {
      var cn = new SqlConnection(_connectionString);
      await cn.OpenAsync();
      return cn;
    }
  }

  public interface IDbConnectionFactory
  {
    IDbConnection GetOpenConnection();
    Task<IDbConnection> GetOpenConnectionAsync();
  }
}