using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpUtilsAndExtensionMethods.AspNet
{
    public static class HttpContextExtensions
    {
        public static async Task<JwtBearerOptions> GetJwtBearerOptionsFromAuthenticationMiddleware(this HttpContext httpContext)
        {
            var authenticationMiddlewareHandlerProvider = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            var jwtBearerHandler = await authenticationMiddlewareHandlerProvider.GetHandlerAsync(null!, JwtBearerDefaults.AuthenticationScheme) as JwtBearerHandler;
            return jwtBearerHandler!.Options;
        }
    }
}
