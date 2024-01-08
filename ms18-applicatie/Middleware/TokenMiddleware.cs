namespace Maasgroep.Middleware;

using Maasgroep.Database.Interfaces;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ITokenStoreRepository tokenStore, IMemberRepository members)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ");

        if (authHeader != null && authHeader.Length > 0 && authHeader[0].ToLower() == "bearer")
        {
            // Seems like we've received a bearer token!
            var token = String.Join(' ', authHeader.Skip(1)); // Token without the word "bearer"
            var memberId = tokenStore.GetMemberIdFromToken(token);
            if (memberId != null)
            {
                var member = members.GetModel((long)memberId);
                context.Items["CurrentUser"] = member;
                context.Items["Token"] = token;    
            }
        }

        await _next(context);
    }
}