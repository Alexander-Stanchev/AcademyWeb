using demo_db.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Academy.Web.Utilities.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch(EntityAlreadyExistsException ex)
            {
                context.Response.Redirect("/home/invalid");
            }
            catch(IncorrectPermissionsException ex)
            {
                context.Response.Redirect("/home/forbidden");
            }
        }
    }
}
