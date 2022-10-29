using AuthAuthDomaineService;
using AuthAuthInfrastructure;
using Google.Protobuf.WellKnownTypes;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing._2_Integration;

public class IntegrationTestInfraRedis
{
    ConnectionMultiplexer redis;
    [SetUp]
    public void Setup()
    {
        this.redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                User = "default",
                Password = "hNhwPLIOOGpZgkds1wGDGjjYysHxeOSX",
                EndPoints = { "redis-12870.c300.eu-central-1-1.ec2.cloud.redislabs.com:12870" }
            });
    }


    [Test(Author = "S.PUYGRENIER", Description = "Dabase exist")]

    public void ActiveDatabaseRedisBeforeTest()
    {

        var pongMessage = "";
        var result = Task.Run(async () =>
        {
            var db = redis.GetDatabase();
            var pong = await db.PingAsync();
            pongMessage = pong.ToString();
        });
        result.Wait();

        Assert.That(pongMessage, Is.Not.EqualTo(""));
        Assert.Pass($"Data : {pongMessage}");
    }


    [Test(Author = "S.PUYGRENIER", Description = "Dabase exist")]
    public void CreateReadDeleteEntry()
    {
        var randomLogin = "GorgetteDu69";
        var randomPassword = "gorgette1950";
        Account account = new Account(randomLogin, randomPassword);
        ;

        var db = redis.GetDatabase();
        string data = "";
        var succesStatus = false;
        string logData = "";

        // CREATE : Add data to base
        var creationTask = db.StringSetAsync(account.Contact, Account.Serialise(account));
        var recordAllUSer = db.SetAddAsync("ALL_USER", account.Contact);
        Task.WaitAll(recordAllUSer, creationTask);
        Assert.True(creationTask.Result);

        // READ: Load data from base
        var loadDataTask = db.StringGetAsync(account.Contact);
        loadDataTask.Wait();
        Assert.True(loadDataTask.Result.ToString().SequenceEqual("{\"Contact\":\"GorgetteDu69\",\"Login\":\"H1m1RiQ8sd9Spz64u8iFY+D2txC/kUlMP4iDXpdct7WbRa1imPPUKmH7NKuOqOCbji348BCTPQb3IItuOphrmA==\",\"Password\":\"uGFKCqEHL2KqITjBPxwaSkV1Du37/QsAP4ebW2pa8x2T7/pnGuj5hUTnhVEnWmMBDyjEeuERVwoqgt+MmOI5uA==\"}"));
        logData += $"Load is : {loadDataTask.Result} \n";

        // DELETE: delete data from base
        var deleteTask = db.StringGetDeleteAsync(account.Contact);
        var deleteFromAllUser = db.SetRemoveAsync("ALL_USER", account.Contact);
        Task.WaitAll(deleteTask, deleteFromAllUser);

        var checkDeleteTask = db.StringGetDeleteAsync(account.Contact);
        checkDeleteTask.Wait();
        Assert.True(checkDeleteTask.Result.IsNull);
        logData += $"Key deletion is : {checkDeleteTask.Result} \n";

        Assert.Pass(logData);

    }

    [Test(Author = "S.PUYGRENIER", Description = "CRD action on account")]
    public void CreateReadDeleteAccount()
    {
        var db = redis.GetDatabase();

        var randomLogin = "GorgetteDu69";
        var randomPassword = "gorgette1950";
        Account account = new Account(randomLogin, randomPassword);

        RedisInfrastructure redisInfrastructure = new RedisInfrastructure();
        redisInfrastructure.redis = this.redis;

        // CREATE : Add data to base
        // init
        redisInfrastructure.CreateInfraAccount(account);
        // action
        var loadDataTask = db.StringGetAsync(account.Contact);
        loadDataTask.Wait();
        // verify
        Assert.True(loadDataTask.Result.ToString().SequenceEqual("{\"Contact\":\"GorgetteDu69\",\"Login\":\"H1m1RiQ8sd9Spz64u8iFY+D2txC/kUlMP4iDXpdct7WbRa1imPPUKmH7NKuOqOCbji348BCTPQb3IItuOphrmA==\",\"Password\":\"uGFKCqEHL2KqITjBPxwaSkV1Du37/QsAP4ebW2pa8x2T7/pnGuj5hUTnhVEnWmMBDyjEeuERVwoqgt+MmOI5uA==\"}"));


        // READ: Load data from base
        // init
        // action
        var resultAccount = redisInfrastructure.GetInfraAccountsByMatchingContact(account).First();
        // verify
        Assert.True(resultAccount.Equals(account));

        // DELETE: delete data from base using exact same account
        // init
        // action
        redisInfrastructure.DeleteInfraAccountByFullAccountLogin(account);
        // verify
        var checkDeleteTask = db.StringGetDeleteAsync(account.Contact);
        checkDeleteTask.Wait();
        Assert.True(checkDeleteTask.Result.IsNull);

    }

    [Test(Author = "S.PUYGRENIER", Description = "Delete using admin account ")]
    public void CreatDeleteAsAdmin()
    {
        var db = redis.GetDatabase();

        var randomLogin = "userToDeleteAsAdmin";
        var randomPassword = "a random password of the user";

        Account account = new Account(randomLogin, randomPassword);
        Account accountDeletionForAdmin = new Account(randomLogin, "","");

        RedisInfrastructure redisInfrastructure = new RedisInfrastructure();
        redisInfrastructure.redis = this.redis;

        // CREATE : Add data to base
        // init
        redisInfrastructure.CreateInfraAccount(account);
        // action
        var loadDataTask = db.StringGetAsync(account.Contact);
        loadDataTask.Wait();
        // verify
        Assert.True(loadDataTask.Result.ToString().SequenceEqual("{\"Contact\":\"userToDeleteAsAdmin\",\"Login\":\"xsDp6nyH5Yr3dcn/Fe6RZuQ3QAhKXEJinCDx7weMwlcbJ+DNumKkhb5tGyKsuBMH/guc3YM9StQFHvvtKot9rQ==\",\"Password\":\"PXwoG6Jr/qncjkVgg1y5HwDmfOqIOm7dz1U3cn/PiFEQU9bafayJv2bn/+EmywJOYU44aEoRjVCGaXZmH6QU5w==\"}"));

        
        // DELETE: delete data from base using partial account object
        // init
        // action
        redisInfrastructure.DeleteInfraAccountAdministratively(accountDeletionForAdmin);
        // verify
        var checkDeleteTask = db.StringGetDeleteAsync(account.Contact);
        checkDeleteTask.Wait();
        Assert.True(checkDeleteTask.Result.IsNull);
        
    }
}
