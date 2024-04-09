using System.Net;
using Acme.Customers.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Acme.Customers.API.Filters;

public class AcmeExceptionFilter: ExceptionFilterAttribute 
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is OrderException)
        {
            var result = new ObjectResult(context.Exception.Message)
            {
                StatusCode = (int) HttpStatusCode.InternalServerError
            };
            
            context.Result = result;
        }
    }
}