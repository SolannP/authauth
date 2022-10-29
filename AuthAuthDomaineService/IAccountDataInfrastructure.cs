using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAuthDomaineService;
public interface IAccountDataInfrastructure
{
    /// <summary>
    /// Provide a way to get an account stored in data storage using the login data of input
    /// </summary>
    /// <param name="account">The account object (search by Login value only)</param>
    /// <returns>List of all account having the same Login</returns>
    [Obsolete]
    public IEnumerable<Account?> GetInfraAccountsByMatchingLogin(Account account);
    /// <summary>
    ///  Provide a way to get an account stored in data storage using the contact data of input
    /// </summary>
    /// <param name="account">The account object to look for (search by Contact value only)</param>
    /// <returns></returns>
    public IEnumerable<Account?> GetInfraAccountsByMatchingContact(Account account);
    /// <summary>
    /// Create a new account into the data storage
    /// </summary>
    /// <param name="account">The account to create</param>
    /// <returns>Succes if created, false otherwise</returns>
    bool CreateInfraAccount(Account account);
    /// <summary>
    /// Delete an account only of the account is exactly in the data storage
    /// </summary>
    /// <param name="account">>The account object to delete</param>
    /// <returns>Succes if deleted, false otherwise</returns>
    bool DeleteInfraAccountByFullAccountLogin(Account account);
    /// <summary>
    /// Super user method to delete an account based on the Contact only
    /// </summary>
    /// <param name="account">The account object to delete, only contact vlaue is important</param>
    /// <returns>Succes if deleted, false otherwise</returns>
    bool DeleteInfraAccountAdministratively(Account account);
    /// <summary>
    /// Provide all account of the data storage
    /// </summary>
    /// <returns>A list of all account</returns>
    List<Account?> GetAllAccount();
    
}
