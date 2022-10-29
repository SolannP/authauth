using AuthAuthDomaineService;
using AuthAuthInfrastructure;
using Newtonsoft.Json;

namespace Testing;

public class UnitTestAccount
{
    [SetUp]
    public void Setup()
    {
    }
    [Test(Author = "S.PUYGRENIER")]
    public void EqualityOfAccountForTwoParameters()
    {
        var randomLogin = Guid.NewGuid().ToString();
        var randomPassword = Guid.NewGuid().ToString();

        var anotherLogin = Guid.NewGuid().ToString();
        var anotherPassword = Guid.NewGuid().ToString();

        var firstAccount = new Account(randomLogin, randomPassword);
        var almostLikeFirstAccountExpectForPassword = new Account(randomLogin, anotherPassword);
        var almostLikeFirstAccountExpectForLogin  = new Account(anotherLogin, randomPassword);
        var secondAccount = new Account(anotherLogin, anotherPassword);

        Assert.That(firstAccount, Is.EqualTo(firstAccount));
        Assert.That(firstAccount, Is.Not.EqualTo(secondAccount));
        Assert.That(firstAccount, Is.Not.EqualTo(almostLikeFirstAccountExpectForLogin));
        Assert.That(firstAccount, Is.Not.EqualTo(almostLikeFirstAccountExpectForPassword));
    }
    [Test(Author = "S.PUYGRENIER")]
    public void EqualityOfAccountForThreeParameters()
    {
        var randomContact = Guid.NewGuid().ToString();
        var randomLogin = Guid.NewGuid().ToString();
        var randomPassword = Guid.NewGuid().ToString();

        var anotherContact = Guid.NewGuid().ToString();
        var anotherLogin = Guid.NewGuid().ToString();
        var anotherPassword = Guid.NewGuid().ToString();

        var firstAccount = new Account(randomContact,randomLogin, randomPassword);
        var almostLikeFirstAccountExpectForContact = new Account(anotherContact,randomLogin, randomPassword);
        var almostLikeFirstAccountExpectForLogin = new Account(randomContact, anotherLogin, randomPassword);
        var almostLikeFirstAccountExpectForPassword = new Account(randomContact, randomLogin, anotherPassword);
        var secondAccount = new Account(anotherContact,anotherLogin, anotherPassword);

        Assert.That(firstAccount, Is.EqualTo(firstAccount));
        Assert.That(firstAccount, Is.Not.EqualTo(secondAccount));
        Assert.That(firstAccount, Is.Not.EqualTo(almostLikeFirstAccountExpectForLogin));
        Assert.That(firstAccount, Is.Not.EqualTo(almostLikeFirstAccountExpectForPassword));
    }

    [Test(Author = "S.PUYGRENIER")]
    public void SerialiseObject()
    {
        var randomLogin = "GorgetteDu69";
        var randomPassword = "gorgette1950";
        Account account = new Account(randomLogin, randomPassword);
        string dataSerialised = Account.Serialise(account);

        Assert.True(dataSerialised.Contains("GorgetteDu69"));
        //login
        Assert.True(dataSerialised.Contains("H1m1RiQ8sd9Spz64u8iFY+D2txC/kUlMP4iDXpdct7WbRa1imPPUKmH7NKuOqOCbji348BCTPQb3IItuOphrmA=="));
        //ppassword
        Assert.True(dataSerialised.Contains("uGFKCqEHL2KqITjBPxwaSkV1Du37/QsAP4ebW2pa8x2T7/pnGuj5hUTnhVEnWmMBDyjEeuERVwoqgt+MmOI5uA=="));

        Assert.Pass(Account.Serialise(account));
    }

    [Test(Author = "S.PUYGRENIER")]
    public void DeserialiseObject()
    {
        var randomLogin = "GorgetteDu69";
        var randomPassword = "gorgette1950";
        Account account = new Account(randomLogin, randomPassword);
        
        string serializedData = "{\"Contact\":\"GorgetteDu69\",\"Login\":\"H1m1RiQ8sd9Spz64u8iFY+D2txC/kUlMP4iDXpdct7WbRa1imPPUKmH7NKuOqOCbji348BCTPQb3IItuOphrmA==\",\"Password\":\"uGFKCqEHL2KqITjBPxwaSkV1Du37/QsAP4ebW2pa8x2T7/pnGuj5hUTnhVEnWmMBDyjEeuERVwoqgt+MmOI5uA==\"}";
        Account desierializedAccount = Account.Deserialize(serializedData);

        Assert.True(account.Equals(desierializedAccount));

    }
}