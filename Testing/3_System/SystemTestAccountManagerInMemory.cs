using AuthAuthDomaineService;
using AuthAuthInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing._3_System;
public class SystemTestAccountManagerInMemory
{
    AccountManager accountManager = new(new InMemoryInfrastructure());

    [Test(Author = "S.PUYGRENIER")]
    public void CreationOfAccount()
    {
        var randomLogin = Guid.NewGuid().ToString();
        var randomPassword = Guid.NewGuid().ToString();


        AccountCreationStatus status = accountManager.Create(randomLogin, randomPassword);
        Assert.That(status, Is.EqualTo(AccountCreationStatus.Created));

        AccountCreationStatus statusOnAlreadyExistingUser;
        statusOnAlreadyExistingUser = accountManager.Create(randomLogin, randomPassword);
        Assert.That(statusOnAlreadyExistingUser, Is.EqualTo(AccountCreationStatus.AlreadyExisting));

        AccountCreationStatus statusOnExistingUserWithSameLogin;
        statusOnExistingUserWithSameLogin = accountManager.Create(randomLogin, Guid.NewGuid().ToString());
        Assert.That(statusOnExistingUserWithSameLogin, Is.EqualTo(AccountCreationStatus.AlreadyExisting));

        AccountCreationStatus statusOnNonExistingUserWithSamePassword;
        statusOnNonExistingUserWithSamePassword = accountManager.Create(Guid.NewGuid().ToString(), randomPassword);
        Assert.That(statusOnNonExistingUserWithSamePassword, Is.EqualTo(AccountCreationStatus.Created));

    }

    [Test(Author = "S.PUYGRENIER")]
    public void CorrectPassword()
    {
        var randomLogin = Guid.NewGuid().ToString();
        var randomPassword = Guid.NewGuid().ToString();


        this.accountManager.Create(randomLogin, randomPassword);
        AccountAccesStatus statusOnExistingAccountWithCorrectIdInput = accountManager.IsCorrectPassword(randomLogin, randomPassword);
        Assert.That(statusOnExistingAccountWithCorrectIdInput, Is.EqualTo(AccountAccesStatus.CorrectCredential));

        AccountAccesStatus statusOnExistingAccountWithIncorectInput = accountManager.IsCorrectPassword(randomLogin, Guid.NewGuid().ToString());
        Assert.That(statusOnExistingAccountWithIncorectInput, Is.EqualTo(AccountAccesStatus.IncorectCredential));

        AccountAccesStatus statusOnInexistingAccount = accountManager.IsCorrectPassword(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        Assert.That(statusOnInexistingAccount, Is.EqualTo(AccountAccesStatus.IncorectCredential));

        AccountAccesStatus statusOnExistingAccountWithIncorectPassword = accountManager.IsCorrectPassword(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        Assert.That(statusOnExistingAccountWithIncorectPassword, Is.EqualTo(AccountAccesStatus.IncorectCredential));

    }

    [Test(Author = "S.PUYGRENIER")]
    public void DeleteAccount()
    {
        var randomLogin = Guid.NewGuid().ToString();
        var randomPassword = Guid.NewGuid().ToString();
        var anotherLogin = Guid.NewGuid().ToString();
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
        var randomLogin = Guid.NewGuid().ToString();
        var randomPassword = Guid.NewGuid().ToString();
        var anotherLogin = Guid.NewGuid().ToString();
        var anotherPassword = Guid.NewGuid().ToString();
        var contact = Guid.NewGuid().ToString();
        var login = Guid.NewGuid().ToString();
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
        var firstContact = "test";
        var firstLogin = "serge@photo.com";
        var firstPassword = "viva la vida";
        var secondLogin = "bruno.mars69@yahou.fr";
        var secondPassword = "FAUXmot2p4ss!";
        var thridLogin = "test@gmx.fr";
        var thridPassword = "thridPassword";

        var allAccountsBeforeCreation = accountManager.GetAllAccounts().Count();
        accountManager.Create(firstContact, firstLogin, firstPassword);
        accountManager.Create(secondLogin, secondPassword);
        accountManager.Create(thridLogin, thridPassword);

        var allAccounts = accountManager.GetAllAccounts();
        Assert.That(allAccounts.Count, Is.EqualTo(allAccountsBeforeCreation + 3));
        Assert.That(allAccounts.Where(account => account!.Equals(firstLogin)), Is.Not.Null);
        Assert.That(allAccounts.Where(account => account!.Equals(thridLogin)), Is.Not.Null);

        Assert.Pass($"Data : {allAccounts.Aggregate("", (curent, next) => curent += Account.Serialise(next))}");
    }

    //[Test]
    public async Task TemporaryPasswordBehaviorAsync()
    {
        /*
        var randomContact = Guid.NewGuid().ToString();
        var randomLogin = Guid.NewGuid().ToString();
        var randomPassword = Guid.NewGuid().ToString();

        var inexistingContact = Guid.NewGuid().ToString();
        AccountManager accountManager = new(new InMemoryInfrastructure());

        accountManager.Create(randomContact, randomLogin, randomPassword);

        Task<(AccountResetPasswordMessageStatus statusOnResetRequestWithEnexistingContact, string message)> SendMailToUserAsync = accountManager.AskTemporaryResetPasswordPossibility(inexistingContact);
        var CommandResult = await SendMailToUserAsync;
        Assert.That(CommandResult.statusOnResetRequestWithEnexistingContact, Is.EqualTo(AccountResetPasswordMessageStatus.InexistingAccount));
        */
        /*
        (AccountResetPasswordMessageStatus statusOnResetRequestWithExistingAccount,string message) = accountManager.AskTemporaryResetPasswordPossibility(randomContact);
        Assert.That(statusOnResetRequestWithExistingAccount, Is.EqualTo(AccountResetPasswordMessageStatus.MessageSend));
        var statusOnAccountStillWorkingWithoutSettingNewPassword =  accountManager.IsCorrectPassword(randomLogin, randomPassword);
        Assert.That(statusOnAccountStillWorkingWithoutSettingNewPassword, Is.EqualTo(AccountAccesStatus.CorrectCredential));
        AccountPasswordResetStatus statusOnSetingPasswordIfUserConnectedAsUsualBeforeSetNewPassword = accountManager.SetNewPasswordAfterRequestReset(randomContact, newPassword);
        Assert.That(statusOnSetingPasswordIfUserConnectedAsUsualBeforeSetNewPassword, Is.EqualTo(AccountPasswordResetStatus.AlreadyConnectedSinceLastRequest));

        accountManager.AskTemporaryResetPasswordPossibility(randomContact);
        AccountPasswordResetStatus statusOnSettingPasswordRightAfterUserAsk = accountManager.SetNewPasswordAfterRequestReset(randomContact, temporaryPassword,newPassword);
        Assert.That(statusOnSetingPasswordIfUserConnectedAsUsualBeforeSetNewPassword, Is.EqualTo(AccountPasswordResetStatus.NewPasswordSet));*/

        //string newPassword = "superSTR0NGp4sswd!<3";

    }
}
