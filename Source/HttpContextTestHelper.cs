using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AspNet_UnitTests
{
    public static class HttpContextTestHelper
    {
        public const string AdminUserIdString = "00000000000000000000000000000000";

        public static HttpContext BuildAuthorizedHttpContextMock(HttpContext? httpContext = null)
        {
            IList<Claim> claims = new List<Claim>
            {
                new Claim("http://localhost/Claims/Name", "Admin"),
                new Claim("http://localhost/Claims/UserId", AdminUserIdString),
                new Claim("http://localhost/Claims/IsAdmin", "1"),
                new Claim("nbf", "1594905946"),
                new Claim("exp", "1594949146"),
                new Claim("iat", "1594905946"),
            };

            var claimsIdentityMock = new Mock<ClaimsIdentity>();
            claimsIdentityMock.Setup(x => x.Claims).Returns(claims);

            httpContext ??= new DefaultHttpContext();
            httpContext.User.AddIdentity(claimsIdentityMock.Object);

            return httpContext;
        }
    }
}
