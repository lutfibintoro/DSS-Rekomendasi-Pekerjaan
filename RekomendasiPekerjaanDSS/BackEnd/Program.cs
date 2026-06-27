using BackEnd.Common;
using BackEnd.Common.Auth;
using BackEnd.Common.Errors;
using BackEnd.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BackEnd;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ---------- Database ----------
        var connStr = builder.Configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("ConnectionStrings:Default not configured");
        builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(connStr, npg => npg.EnableRetryOnFailure(5)));

        // ---------- MediatR (CQRS) ----------
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // ---------- FluentValidation ----------
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        // ---------- Auth (Basic) ----------
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<CurrentUser>();
        builder.Services
            .AddAuthentication(BasicAuthHandler.SchemeName)
            .AddScheme<BasicAuthOptions, BasicAuthHandler>(BasicAuthHandler.SchemeName, _ => { });
        builder.Services.AddAuthorization();

        // ---------- FastEndpoints ----------
        builder.Services.AddFastEndpoints();
        builder.Services.SwaggerDocument(o =>
        {
            o.DocumentSettings = s =>
            {
                s.Title = "DSS Rekomendasi Pekerjaan API";
                s.Version = "v1";
            };
        });

        // ---------- Global exception handler ----------
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        var app = builder.Build();

        // ---------- Pipeline ----------
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseFastEndpoints();
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwaggerGen();
        //}

        // Seed master kriteria kalau belum ada (idempotent, fallback kalau init SQL tidak jalan)
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            try
            {
                DbSeeder.SeedKriteria(db);
            }
            catch (Exception ex)
            {
                app.Logger.LogWarning(ex, "DbSeeder gagal (mungkin init SQL sudah jalan): {Msg}", ex.Message);
            }
        }

        app.Run();
    }
}