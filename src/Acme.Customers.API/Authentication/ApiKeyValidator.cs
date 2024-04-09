using Acme.Customers.API.Authentication.Contracts;

namespace Acme.Customers.API.Authentication;

public class ApiKeyValidator: IApiKeyValidator
{
    private readonly string _apiKey;

    public ApiKeyValidator(IConfiguration configuration)
    {
        _apiKey = configuration.GetValue<string>(Constants.ApiKeyConfiguration);
    }
    
    public bool IsValid(string apiKey)
    {
        return string.Equals(_apiKey, apiKey, StringComparison.InvariantCultureIgnoreCase);
    }
}