using AuthAuthDomaineService;

namespace AuthAuthInfrastructure;
/// <summary>
/// This class is aimed to provide mocking possibilities for test
/// </summary>
public class InMemoryInfrastructure : IAccountDataInfrastructure
{
    private readonly List<Account?> allAccounts = new();

    public bool CreateInfraAccount(Account account)
    {
        allAccounts.Add(account);
        return true;
    }

    public bool DeleteInfraAccountByFullAccountLogin(Account account) => allAccounts.Remove(account);

    public bool DeleteInfraAccountAdministratively(Account account)
    {
        Account? accountToDelete = allAccounts.Find(matchinAccount => matchinAccount.EqualsContact(account));
        return allAccounts.Remove(accountToDelete);
    }

    public IEnumerable<Account?> GetInfraAccountsByMatchingLogin(Account accountToVerify) => allAccounts.Where(account => accountToVerify.EqualsLogin(account));

    public List<Account?> GetAllInfraLogin() => allAccounts;

    public IEnumerable<Account?> GetInfraAccountsByMatchingContact(Account accountToVerify) => allAccounts.Where(account => accountToVerify.EqualsContact(account));
}
