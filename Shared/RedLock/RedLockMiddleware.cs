using RedLockNet.SERedis;
using System.Text.Json;

namespace Shared.RedLock
{
    internal class RedLockMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(
            HttpContext context,
            RedLockFactory redLockFactory,
            HybridCache hybridCache)
        {
            var method = context.Request.Method.ToUpperInvariant();

            if (method is not ("POST" or "PUT" or "DELETE"))
            {
                await _next(context);
                return;
            }

            var keyValue = await ExtractKeyFromBody(context);

            if (keyValue is null)
            {
                await _next(context);
                return;
            }

            string lockKey = $"redlock:{context.Request.Path}:{keyValue}";

            using var redLock = await redLockFactory.CreateLockAsync(
                lockKey,
                expiryTime: TimeSpan.FromSeconds(20),
                waitTime: TimeSpan.FromSeconds(5),
                retryTime: TimeSpan.FromMilliseconds(200)
            );

            if (!redLock.IsAcquired)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "This operation already for other process."
                });
                return;
            }

            await _next(context);

            string cacheKey = $"resource:{context.Request.Path}:{keyValue}";
            await hybridCache.RemoveAsync(cacheKey);
            await hybridCache.RemoveAsync("keyColaborator");
        }

        private static async Task<string?> ExtractKeyFromBody(HttpContext context)
        {
            if (!context.Request.ContentType?.Contains("application/json") ?? true)
                return null;

            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(body))
                return null;

            using var json = JsonDocument.Parse(body);

            var possibleKeys = new[] { "id", "key", "code", "codigo", "uuid", "primaryKey", "email" };

            foreach (var key in possibleKeys)
            {
                if (json.RootElement.TryGetProperty(key, out var prop))
                    return prop.ToString();
            }

            return null;
        }
    }
}