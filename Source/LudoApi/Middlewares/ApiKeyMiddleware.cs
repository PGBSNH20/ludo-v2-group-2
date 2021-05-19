using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoApi.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;

        public ApiKeyMiddleware(RequestDelegate next, string apiKey)
        {
            _next = next;
            _apiKey = apiKey;
        }

        public async Task Invoke(HttpContext context)
        {
            // Logic to check api key.
            if (!context.Request.Headers.ContainsKey("apikey"))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Missing API key!");
                return;
            }

            if (!ValidateApiKey(context))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API key!");
                return;
            }

            //await _next.Invoke(context);
            await _next(context);
        }

        private bool ValidateApiKey(HttpContext context)
        {
            return _apiKey == context.Request.Headers["apikey"];
        }
    }
}
