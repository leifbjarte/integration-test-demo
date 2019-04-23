using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.ErrorHandling
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (SomeCustomError ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
