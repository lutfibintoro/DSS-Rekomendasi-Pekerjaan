using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using BackEnd.Domain;
using BackEnd.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackEnd.Common.Auth;

public class BasicAuthOptions : AuthenticationSchemeOptions { }

/// <summary>
/// Authentication handler "Basic" — memparse header Authorization: Basic base64(email:password),
/// memvalidasi kredensial ke DB, lalu membuat ClaimsPrincipal.
/// Setelah ini sukses, HttpContext.User terisi sehingga middleware authorization bisa jalan.
/// </summary>
public class BasicAuthHandler : AuthenticationHandler<BasicAuthOptions>
{
    public const string SchemeName = "Basic";

    public BasicAuthHandler(
        IOptionsMonitor<BasicAuthOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder) { }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var auth))
            return AuthenticateResult.NoResult();

        var raw = auth.ToString();
        if (!raw.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.NoResult();

        string email, password;
        try
        {
            var decoded = System.Text.Encoding.UTF8.GetString(
                Convert.FromBase64String(raw["Basic ".Length..]));
            var idx = decoded.IndexOf(':');
            if (idx < 0) return AuthenticateResult.Fail("Malformed Basic header");
            email = decoded[..idx];
            password = decoded[(idx + 1)..];
        }
        catch
        {
            return AuthenticateResult.Fail("Malformed Basic header");
        }

        var db = Context.RequestServices.GetRequiredService<AppDbContext>();
        var user = await db.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return AuthenticateResult.Fail("Invalid credentials");

        // Set juga untuk CurrentUser via HttpContext.Items
        Context.Items["CurrentUser"] = user;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
        };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);
        return AuthenticateResult.Success(ticket);
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = 401;
        Response.Headers.WWWAuthenticate = "Basic realm=\"DSS\"";
        return Task.CompletedTask;
    }
}