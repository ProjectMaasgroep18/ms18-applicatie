namespace Maasgroep.Middleware;

using Maasgroep.Services;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;

    public TokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IMemberService memberService)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ");

        if (authHeader != null && authHeader.Count() > 0 && authHeader[0].ToLower() == "bearer")
        {
            // Seems like we've received a bearer token!
            var token = String.Join(' ', authHeader.Skip(1)); // Token without the word "bearer"
            context.Items["CurrentUser"] = memberService.GetMemberByToken(token);
            if (memberService.CurrentMember != null)
                Console.WriteLine("Authenticated request by: " + memberService.CurrentMember.Name);
        }

        await _next(context);
    }
}