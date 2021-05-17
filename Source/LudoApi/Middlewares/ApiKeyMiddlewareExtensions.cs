using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoApi.Middlewares
{
    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseAPIKey(this IApplicationBuilder builder, string apiKey)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>(apiKey);
        }
    }
}
