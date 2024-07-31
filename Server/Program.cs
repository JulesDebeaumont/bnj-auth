using Server.Db;
using Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

public partial class Program
{
    private static readonly string AppName = "ArkusAuth";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        RunBeforeStart(builder);
        RegisterServices(builder);

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<MainContext>();
            context.Database.Migrate();
            await DevSeed.RunAsync(context);
        }


        app.UseHttpsRedirection();
        app.UseCors(GetCorsModeByEnv(builder.Configuration));
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static void RunBeforeStart(WebApplicationBuilder builder)
    {
        EnsureSecretsAreOk(builder.Configuration);
        FileStorageService.EnsureStorageDirectoryAreCreated(builder.Configuration);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    private static void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddDbContext<MainContext>(options =>
            options.UseNpgsql(builder.Configuration[$"{AppName}DbCredentials"]));
        builder.Services.AddTransient<FileStorageService>();
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = AuthService.GetAuthServiceTokenValidationParameters(builder.Configuration);
                });

        builder.Services.AddAuthorization();
        builder.Services.AddCors(options =>
            {
                options.AddPolicy("Prod", builder =>
                {
                    // TODO
                });
                options.AddPolicy("Dev", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
    }

    private static void EnsureSecretsAreOk(IConfiguration config)
    {
        var secretsToCheck = new string[]
        {
            $"{AppName}DbCredentials",
            $"{AppName}JwtPrivateKey",
            $"{AppName}PathMainStorage",
            $"{AppName}CorsPolicy"
        };
        foreach (string secret in secretsToCheck)
        {
            if (config[secret] is null)
            {
                throw new Exception($"Secret '{secret}' is not defined!");
            }
        }
    }

    private static string GetCorsModeByEnv(IConfiguration config)
    {
        return config[$"{AppName}CorsPolicy"] ?? "Prod";
    }

}

