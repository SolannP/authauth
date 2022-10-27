using AuthAuthInfrastructure;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing._2_Integration;

public class IntegrationTestInfraDataBase
{
    [SetUp]
    public void Setup()
    {
    }


    [Test(Author = "S.PUYGRENIER", Description = "Dabase exist")]

    public void ActiveDatabaseRedisBeforeTest()
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                User = "default",
                Password = "hNhwPLIOOGpZgkds1wGDGjjYysHxeOSX",
                EndPoints = { "redis-12870.c300.eu-central-1-1.ec2.cloud.redislabs.com:12870" }
            });
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


}

