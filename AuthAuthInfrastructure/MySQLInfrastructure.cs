using AuthAuthDomaineService;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAuthInfrastructure;

[Obsolete("Not any existing bdd for mysql", true)]
public class MySQLInfrastructure : IAccountDataInfrastructure
{
    private string connectionString;
    public MySQLInfrastructure(string connString) => this.connectionString = connString;

    public bool BddCanConnect()
    {
        bool result = false;
        var connexion = new MySqlConnection(connectionString);
        try
        {
            connexion.Open();
            result = true;
            connexion.Close();
        }
        catch
        {
            result = false;
        }
        return result;
    }

    public bool CreateAccount(Account account) => throw new NotImplementedException();
    public bool CreateInfraAccount(Account account) => throw new NotImplementedException();
    public bool DeleteAccountByFullAccountLogin(Account account) => throw new NotImplementedException();
    public bool DeleteInfraAccountAdministratively(Account account) => throw new NotImplementedException();
    public bool DeleteInfraAccountByFullAccountLogin(Account account) => throw new NotImplementedException();
    public IEnumerable<Account?> GetAccountsByLogin(Account account) => throw new NotImplementedException();
    public List<Account?> GetAllAccount() => throw new NotImplementedException();
    public IEnumerable<Account?> GetInfraAccountsByMatchingContact(Account account) => throw new NotImplementedException();
    public IEnumerable<Account?> GetInfraAccountsByMatchingLogin(Account account) => throw new NotImplementedException();
}
