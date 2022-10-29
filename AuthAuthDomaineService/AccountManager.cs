using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AuthAuthDomaineService;
public class AccountManager
{
    IAccountDataInfrastructure data;
    IMessageSending sendingService;

    public AccountManager(IAccountDataInfrastructure data) => this.data = data;

    /// <summary>
    /// Create an Account
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns>A enum <see cref="AccountCreationStatus"/> like AlreadyExisting,Error or Created</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public AccountCreationStatus Create(string login, string password)
    {
        return Create(login, login, password);
    }

    /// <summary>
    ///  Create an Account
    /// </summary>
    /// <param name="contact">a way to contact user</param>
    /// <param name="login">login credential for connection</param>
    /// <param name="password">password credential for connection</param>
    /// <returns>A enum <see cref="AccountCreationStatus"/> like AlreadyExisting,Error or Created</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public AccountCreationStatus Create(string contact,string login, string password)
    {
        bool succesfullCreation = false;
        Account account = new(contact,login, password);
        var matchingAccount = data.GetInfraAccountsByMatchingContact(account);
        if (matchingAccount?.Count() > 1)
            throw new ArgumentOutOfRangeException($"More than one account already exist for login: {login}");
        if (matchingAccount?.Any() ?? false)
            return AccountCreationStatus.AlreadyExisting;
        try
        {
            succesfullCreation = data.CreateInfraAccount(account);
        }
        catch { return AccountCreationStatus.Error; }
        if (succesfullCreation is not true)
            return AccountCreationStatus.Error;
        return AccountCreationStatus.Created;
    }

    /// <summary>
    /// Check Password Data Of Account
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns>A enum <see cref="AccountAccesStatus"/> like IncorectCredential, CorrectCredential or Error</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public AccountAccesStatus IsCorrectPassword(string login, string password)
    {
        IEnumerable<Account?> matchingAccounts;
        Account accountToCheck = new(login, password);
        try { matchingAccounts = data.GetInfraAccountsByMatchingContact(accountToCheck);}
        catch { return AccountAccesStatus.Error; }
        if (matchingAccounts?.Count() > 1) throw new ArgumentOutOfRangeException($"More than one account already exist for login: {login}");
        if (matchingAccounts?.Count() is 0) return AccountAccesStatus.IncorectCredential ;
        if(matchingAccounts!.Contains(accountToCheck) is not true) return AccountAccesStatus.IncorectCredential;
        return AccountAccesStatus.CorrectCredential;
    }

    public AccountDeleteStatus DeleteAccount(string login, string password)
    {
        bool succesfullDelete= false;
        IEnumerable<Account?> matchingAccounts;
        Account accountToDelete = new(login, password);
        try{ matchingAccounts = data.GetInfraAccountsByMatchingContact(accountToDelete); }
        catch { return AccountDeleteStatus.Error; }
        if (matchingAccounts?.Count() > 1) throw new ArgumentOutOfRangeException($"More than one account already exist for login: {login}");
        if (matchingAccounts?.Count() is 0) return AccountDeleteStatus.InexistingAccount;
        
        if (matchingAccounts!.Contains(accountToDelete) is not true) return AccountDeleteStatus.InexistingAccount;
        
        try{ succesfullDelete = data.DeleteInfraAccountByFullAccountLogin(accountToDelete); }
        catch { return AccountDeleteStatus.Error; };

        if (succesfullDelete is not true) return AccountDeleteStatus.Error;
        return AccountDeleteStatus.CorrectDeletion;
    }

    public List<Account?> GetAllAccounts()
    {
        return data.GetAllAccount();
    }

    public AccountDeleteStatus AdministrativDeleteAccount(string contact)
    {
        bool succesfullDelete= false;
        IEnumerable<Account?> matchingAccounts;
        Account accountToDelete = new(contact, "no maters because admin deleting...", "no maters because admin deleting...");

        try{ succesfullDelete = data.DeleteInfraAccountAdministratively(accountToDelete); }
        catch { return AccountDeleteStatus.Error; };
        
        if (succesfullDelete is not true) return AccountDeleteStatus.Error;
        return AccountDeleteStatus.CorrectDeletion;
    }

    public async Task<(AccountResetPasswordMessageStatus statusOnResetRequestWithEnexistingAccount, string message)> AskTemporaryResetPasswordPossibility(string contactMail)
    {
        IEnumerable<Account?> accounts = data.GetInfraAccountsByMatchingContact(new Account(contactMail, "", ""));
        if (accounts.Count() is 0)
            return (AccountResetPasswordMessageStatus.InexistingAccount, "Not any account matching");
        throw new NotImplementedException("not impl");
        //return (AccountResetPasswordMessageStatus.MessageSend, " ");
        //var TODO =  sendingService.sendMessageToContact('',0,  new MailMessage { });
    }
}
