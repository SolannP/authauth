using AuthAuthDomaineService;
using AuthAuthInfrastructure;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing._3_System;
public class SystemTestAccountManagerRedis
{
    AccountManager accountManager = new(new RedisInfrastructure(ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                User = "default",
                Password = "hNhwPLIOOGpZgkds1wGDGjjYysHxeOSX",
                EndPoints = { "redis-12870.c300.eu-central-1-1.ec2.cloud.redislabs.com:12870" }
            })));

    TestDataCreator dataCreator;

    [SetUp]
    public void InitListOfUserToDelete()
    {
        dataCreator = new TestDataCreator();
    }

    [TearDown]
    public void DeleteAllCreatedUserForTest()
    {
        foreach (string contact in dataCreator.allCreatedContactForTest)
        {
            accountManager.AdministrativDeleteAccount(contact);
        }
    }
    [Test(Author = "S.PUYGRENIER")]
    public void CreationOfAccount()
    {
        var randomLogin = dataCreator.CreateNewLoginOrContact();
        var randomPassword = Guid.NewGuid().ToString();

        var anotehrLogin = dataCreator.CreateNewLoginOrContact();


        AccountCreationStatus status = accountManager.Create(randomLogin, randomPassword);
        Assert.That(status, Is.EqualTo(AccountCreationStatus.Created));

        AccountCreationStatus statusOnAlreadyExistingUser;
        statusOnAlreadyExistingUser = accountManager.Create(randomLogin, randomPassword);
        Assert.That(statusOnAlreadyExistingUser, Is.EqualTo(AccountCreationStatus.AlreadyExisting));

        AccountCreationStatus statusOnExistingUserWithSameLogin;
        statusOnExistingUserWithSameLogin = accountManager.Create(randomLogin, Guid.NewGuid().ToString());
        Assert.That(statusOnExistingUserWithSameLogin, Is.EqualTo(AccountCreationStatus.AlreadyExisting));

        AccountCreationStatus statusOnNonExistingUserWithSamePassword;
        statusOnNonExistingUserWithSamePassword = accountManager.Create(anotehrLogin, randomPassword);
        Assert.That(statusOnNonExistingUserWithSamePassword, Is.EqualTo(AccountCreationStatus.Created));
    }

    [Test(Author = "S.PUYGRENIER")]
    public void CorrectPassword()
    {
        var randomLogin = dataCreator.CreateNewLoginOrContact();
        var randomPassword = Guid.NewGuid().ToString();

        var anotherLogin = dataCreator.CreateNewLoginOrContact();
        var thirdLogin = dataCreator.CreateNewLoginOrContact();

        accountManager.Create(randomLogin, randomPassword);
        AccountAccesStatus statusOnExistingAccountWithCorrectIdInput = accountManager.IsCorrectPassword(randomLogin, randomPassword);
        Assert.That(statusOnExistingAccountWithCorrectIdInput, Is.EqualTo(AccountAccesStatus.CorrectCredential));

        AccountAccesStatus statusOnExistingAccountWithIncorectInput = accountManager.IsCorrectPassword(randomLogin, Guid.NewGuid().ToString());
        Assert.That(statusOnExistingAccountWithIncorectInput, Is.EqualTo(AccountAccesStatus.IncorectCredential));

        AccountAccesStatus statusOnInexistingAccount = accountManager.IsCorrectPassword(thirdLogin, Guid.NewGuid().ToString());
        Assert.That(statusOnInexistingAccount, Is.EqualTo(AccountAccesStatus.IncorectCredential));

        AccountAccesStatus statusOnExistingAccountWithIncorectPassword = accountManager.IsCorrectPassword(anotherLogin, Guid.NewGuid().ToString());
        Assert.That(statusOnExistingAccountWithIncorectPassword, Is.EqualTo(AccountAccesStatus.IncorectCredential));

    }

    [Test(Author = "S.PUYGRENIER")]
    public void DeleteAccount()
    {
        var randomLogin = dataCreator.CreateNewLoginOrContact();
        var randomPassword = Guid.NewGuid().ToString();
        var anotherLogin = dataCreator.CreateNewLoginOrContact();
        var anotherPassword = Guid.NewGuid().ToString();

        accountManager.Create(randomLogin, randomPassword);
        accountManager.Create(anotherLogin, anotherPassword);

        AccountDeleteStatus statusOnDeletingExistingAccount = accountManager.DeleteAccount(randomLogin, randomPassword);
        Assert.That(statusOnDeletingExistingAccount, Is.EqualTo(AccountDeleteStatus.CorrectDeletion));

        AccountDeleteStatus statusOnDeletingInexistingAccount = accountManager.DeleteAccount(randomLogin, randomPassword);
        Assert.That(statusOnDeletingInexistingAccount, Is.EqualTo(AccountDeleteStatus.InexistingAccount));

        AccountDeleteStatus statusOnDeletingAccountWithWrongPassword = accountManager.DeleteAccount(anotherLogin, randomPassword);
        Assert.That(statusOnDeletingAccountWithWrongPassword, Is.EqualTo(AccountDeleteStatus.InexistingAccount));

    }

    [Test(Author = "S.PUYGRENIER")]
    public void DeleteAccountAsAdmin()
    {
        var randomLogin = dataCreator.CreateNewLoginOrContact();
        var randomPassword = Guid.NewGuid().ToString();
        var anotherLogin = dataCreator.CreateNewLoginOrContact();
        var anotherPassword = Guid.NewGuid().ToString();
        var contact = dataCreator.CreateNewLoginOrContact();
        var login = dataCreator.CreateNewLoginOrContact();
        var password = Guid.NewGuid().ToString();

        accountManager.Create(randomLogin, randomPassword);
        accountManager.Create(anotherLogin, anotherPassword);
        accountManager.Create(contact, login, password);

        AccountDeleteStatus statusOnDeletingExistingAccount = accountManager.AdministrativDeleteAccount(randomLogin);
        Assert.That(statusOnDeletingExistingAccount, Is.EqualTo(AccountDeleteStatus.CorrectDeletion));

        AccountDeleteStatus statusOnExistingAccountUsingContactWhenProvided = accountManager.AdministrativDeleteAccount(contact);
        Assert.That(statusOnExistingAccountUsingContactWhenProvided, Is.EqualTo(AccountDeleteStatus.CorrectDeletion));
    }

    [Test(Author = "S.PUYGRENIER")]
    public void GetAllAccount()
    {
        var firstContact = dataCreator.CreateNewLoginOrContact("test");
        var firstLogin = dataCreator.CreateNewLoginOrContact("serge@photo.com");
        var firstPassword = "viva la vida";
        var secondLogin = dataCreator.CreateNewLoginOrContact("bruno.mars69@yahou.fr");
        var secondPassword = "FAUXmot2p4ss!";
        var thridLogin = dataCreator.CreateNewLoginOrContact("test@gmx.fr");
        var thridPassword = "thridPassword";


        accountManager.Create(firstContact, firstLogin, firstPassword);
        accountManager.Create(secondLogin, secondPassword);
        accountManager.Create(thridLogin, thridPassword);

        var allAccounts = accountManager.GetAllAccounts();
        Assert.That(allAccounts.Count, Is.EqualTo(3));
        Assert.That(allAccounts.Where(account => account!.Equals(firstLogin)), Is.Not.Null);
        Assert.That(allAccounts.Where(account => account!.Equals(thridLogin)), Is.Not.Null);

        Assert.Pass($"Data : {allAccounts.Aggregate("", (curent, next) => curent += Account.Serialise(next))}");
    }



    //[Test]
    /*
    public async Task TemporaryPasswordBehaviorAsync()
    {
    }
    */

}


public class TestDataCreator
{
    public TestDataCreator() {}
    public List<string> allCreatedContactForTest = new List<string> { };
    public string CreateNewLoginOrContact()
    {
        var dataKey = Guid.NewGuid().ToString();
        return CreateNewLoginOrContact(dataKey);
    }
    public string CreateNewLoginOrContact(string data)
    {
        allCreatedContactForTest.Add(data);
        return data;
    }

}
