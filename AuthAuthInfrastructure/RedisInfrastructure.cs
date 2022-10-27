using AuthAuthDomaineService;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAuthInfrastructure;
public class RedisInfrastructure : IAccountDataInfrastructure
{
    ConnectionMultiplexer redis;

    public bool CreateInfraAccount(Account account)
    {
        var db = redis.GetDatabase();
        var stateAction = db.SetAdd(account.Contact, "");
        return stateAction;
    }
    public bool DeleteInfraAccountAdministratively(Account account) => throw new NotImplementedException();
    public bool DeleteInfraAccountByFullAccountLogin(Account account) => throw new NotImplementedException();
    public List<Account?> GetAllInfraLogin() => throw new NotImplementedException();
    public IEnumerable<Account?> GetInfraAccountsByMatchingContact(Account account) => throw new NotImplementedException();
    public IEnumerable<Account?> GetInfraAccountsByMatchingLogin(Account account) => throw new NotImplementedException();
}
