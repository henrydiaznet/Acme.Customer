using Acme.Customers.API.Authentication.Contracts;

namespace Acme.Customers.IntegrationTests.Mocks;

public class MockApiKeyValidator: IApiKeyValidator
{
    public bool IsValid(string apiKey)
    {
        return true;
    }
}