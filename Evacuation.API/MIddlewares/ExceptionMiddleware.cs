
namespace Evacuations.API.MIddlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("Mid work");
            await next.Invoke(context);
        }
    }
}
