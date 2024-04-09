using Acme.Customers.API.Authentication.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Acme.Customers.API.Authentication;

public class ApiKeyAuthFilter: IAsyncAuthorizationFilter
{
    private readonly IApiKeyValidator _apiKeyValidator;

    public ApiKeyAuthFilter(IApiKeyValidator apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(Constants.ApiKeyHeaderName, out var apiKey))
        {
            context.Result = new UnauthorizedObjectResult($"Expected header is missing: {Constants.ApiKeyHeaderName}");
            return Task.CompletedTask;
        }

        if (!_apiKeyValidator.IsValid(apiKey))
        {
            context.Result = new UnauthorizedObjectResult("Invalid API key");
            return Task.CompletedTask;
        }
        
        return Task.CompletedTask;
    }
}