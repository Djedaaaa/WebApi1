using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Extensions
{
    // Extension classes are static
    public static class GeneralExtensions
    {
        // HttpContext provides info about current request that came on Controller.
        // When using HttpContext class in other parts of application, example: in PostController, it will be possible to use this extension method as: UserId = HttpContext.GetUserId()
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
                return string.Empty;

            // Returns the userId from the token
            return httpContext.User.Claims.Single(x => x.Type == "id").Value;

        }
    }
}
