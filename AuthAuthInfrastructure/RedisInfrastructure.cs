using AuthAuthDomaineService;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AuthAuthInfrastructure;
/// <summary>
/// 
/// </summary>
/// <see href="https://docs.redis.com/latest/rs/references/client_references/client_csharp/"/>
/// <see href="https://stackexchange.github.io/StackExchange.Redis/"/>
public class RedisInfrastructure : IAccountDataInfrastructure
{
    public ConnectionMultiplexer redis;
    protected const string setKeyContact = "ALL_USER";

    public RedisInfrastructure() {}
    public RedisInfrastructure(ConnectionMultiplexer redisDataConnection) => redis = redisDataConnection;

    public bool CreateInfraAccount(Account account)
    {
        CheckIsInitialisedRedisDatabase();
        var db = redis.GetDatabase();
        var creationTask = db.StringSetAsync(account.Contact, Account.Serialise(account));
        creationTask.Wait();
        if(creationTask.Result is true) db.SetAddAsync(setKeyContact, account.Contact);
        return creationTask.Result;
    }
    public bool DeleteInfraAccountAdministratively(Account account)
    {
        CheckIsInitialisedRedisDatabase();
        var db = redis.GetDatabase();
        var deleteTask = db.StringGetDeleteAsync(account.Contact);
        deleteTask.Wait();
        bool succesfullDeleting = deleteTask.Result != "";
        if (succesfullDeleting) db.SetRemove(setKeyContact, account.Contact);
        return succesfullDeleting;
    }
    public bool DeleteInfraAccountByFullAccountLogin(Account account)
    {
        CheckIsInitialisedRedisDatabase();
        var db = redis.GetDatabase();
        string? accountEncriptedData = db.StringGet(account.Contact);
        Account dataAccount = Account.Deserialize(accountEncriptedData);
        if (account.Equals(dataAccount)) return DeleteInfraAccountAdministratively(account);;
        return false;

    }
    public List<Account?> GetAllAccount()
    {
        CheckIsInitialisedRedisDatabase();
        var db = redis.GetDatabase();
        var allContact = db.SetMembers(setKeyContact);
        List<Account?> allAccount = allContact.Select(contact => GetInfraAccountsByMatchingContact(new Account(contact, "",""))
                                                                                                  ?.First())
                                              .ToList();
        return allAccount;
    }
    public IEnumerable<Account?> GetInfraAccountsByMatchingContact(Account account)
    {
        CheckIsInitialisedRedisDatabase();
        var db = redis.GetDatabase();
        string accountEncriptedData = db.StringGet(account.Contact).ToString();
        if (accountEncriptedData is null or "") return new List<Account> { };
        Account dataAccount = Account.Deserialize(accountEncriptedData);
        return new List<Account> { dataAccount };
    }
    public IEnumerable<Account?> GetInfraAccountsByMatchingLogin(Account account) => throw new NotImplementedException("Depreciated function");

    protected void CheckIsInitialisedRedisDatabase()
    {
        if (this.redis is null) throw new RedisInfrastructureException("Not intialised redis data storage");
    }
}

public class RedisInfrastructureException : Exception
{
    public RedisInfrastructureException(string? message) : base(message)
    {
    }
} 
