using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAuthDomaineService;
public interface IAccountDataInfrastructure
{
    public IEnumerable<Account?> GetInfraAccountsByMatchingLogin(Account account);
    public IEnumerable<Account?> GetInfraAccountsByMatchingContact(Account account);
    bool CreateInfraAccount(Account account);
    bool DeleteInfraAccountByFullAccountLogin(Account account);
    bool DeleteInfraAccountAdministratively(Account account);
    List<Account?> GetAllInfraLogin();
    
}
