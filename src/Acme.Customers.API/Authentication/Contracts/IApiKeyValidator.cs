namespace Acme.Customers.API.Authentication.Contracts;

public interface IApiKeyValidator
{
    bool IsValid(string apiKey);
}