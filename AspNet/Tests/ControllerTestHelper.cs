using AspNet5_0;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace CSharpUtilsAndExtensionMethods.AspNet.Tests
{
    public static class ControllerTestHelper
    {
        public static ControllerContext BuildAuthorizedControllerContextMock(HttpContext? httpContext = null)
        {
            var httpContextMock = HttpContextTestHelper.BuildAuthorizedHttpContextMock(httpContext);
            return new ControllerContext(new ActionContext(httpContextMock, new RouteData(), new ControllerActionDescriptor()));
        }

        public static ControllerContext BuildAnonymousControllerContextMock()
        {
            var httpContextMock = new DefaultHttpContext();
            return new ControllerContext(new ActionContext(httpContextMock, new RouteData(), new ControllerActionDescriptor()));
        }
    }
}
