using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAuthDomaineService;

// All Status possible for each command 
# region AccountStatusCommande
public enum AccountCreationStatus
{
    Created,
    AlreadyExisting,
    Error,// Please throw Excetion instead
}
public enum AccountAccesStatus
{
    CorrectCredential,
    IncorectCredential,
    Error,// Please throw Excetion instead
}

public enum AccountDeleteStatus
{
    CorrectDeletion,
    InexistingAccount,
    Error,// Please throw Excetion instead
}

public enum AccountResetPasswordMessageStatus
{
    MessageSend,
    InexistingAccount
}

public enum AccountPasswordResetStatus
{
    AlreadyConnectedSinceLastRequest,
    NewPasswordSet
}

#endregion
