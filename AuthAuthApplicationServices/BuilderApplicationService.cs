using AuthAuthDomaineService;
using AuthAuthInfrastructure;
using StackExchange.Redis;

namespace AuthAuthApplicationServices;
public class BuilderApplicationService
{
    private AccountManager accountManager;
    private bool _isInfraInitialisation = false;
    public BuilderApplicationService(){}

    public void BuildInMemoryDataBaseConnection()
    {
        accountManager = new(new InMemoryInfrastructure());
        _isInfraInitialisation = true;
    }
    public void BuildRedisDataBaseConnection(string user, string password, string endPoints)
    {
        accountManager = new(new RedisInfrastructure(ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                User = user,
                Password = password,
                EndPoints = { endPoints }
            })));
        _isInfraInitialisation = true;
    }

    public AccountManager Make()
    {
        var errorMessage = "";
        // error checking
        if (_isInfraInitialisation is false) errorMessage += "Infra not initialised\n";

        // throw compated error message if any
        if (errorMessage is not "") throw new BuilderApplicationServiceException(errorMessage);

        //return result
        return this.accountManager;
    }
}

public class BuilderApplicationServiceException : ApplicationException
{
    public BuilderApplicationServiceException(string? message) : base(message)
    {
    }
}
