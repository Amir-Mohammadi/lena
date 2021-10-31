using core.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using core.Data;
using StackExchange.Redis;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Builder;
using System.Linq;
using System.IO.Compression;
using System.Reflection;
using Microsoft.OpenApi.Models;
using System.IO;
using core.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using core.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Threading.Tasks;
using core.ExceptionHandler;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System;
namespace core.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static void ConfigureSiteSetting(this IServiceCollection services, IConfiguration configuration)
    {
      services.Configure<SiteSetting>(configuration.GetSection(nameof(SiteSetting)));
    }
    public static void AddContext(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
    }
    public static void AddCors(this IServiceCollection services,
                               IConfiguration configuration)
    {
      var siteSetting = configuration.GetSection(nameof(SiteSetting)).Get<SiteSetting>();
      var withOrigins = siteSetting.WithOrigins.Split(",");
      services.AddCors(options =>
      {
        options.AddPolicy(siteSetting.AllowSpecificOrigins,
              builder =>
              {
                builder.WithOrigins(withOrigins)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
              });
      });
    }
    public static IServiceCollection AddSwaggerGen(this IServiceCollection services, string apiVersion)
    {
      return services.AddSwaggerGen(c =>
             {
               var assemblyName = Assembly.GetEntryAssembly().GetName().Name;
               c.SwaggerDoc(apiVersion, new OpenApiInfo { Title = assemblyName, Version = "v1" });
               var filePath = Path.Combine(AppContext.BaseDirectory, assemblyName + ".xml");
               c.IncludeXmlComments(filePath);
               c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
               {
                 Description = "JWT Authorization header using  the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                 Name = "Authorization",
                 In = ParameterLocation.Header,
                 Type = SecuritySchemeType.ApiKey,
                 Scheme = "Bearer"
               });
               c.SchemaFilter<EnumSchemaFilter>();
               c.OperationFilter<SecurityRequirementsOperationFilter>();
             }
            );
    }
    public static void AddRedisService(this IServiceCollection services, IConfiguration configuration)
    {
      var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisServer"));
      services.AddScoped(s => redis.GetDatabase());
    }
    public static void AddBrotliAndGzipResponseCompression(this IServiceCollection services)
    {
      services.AddResponseCompression(options =>
        {
          options.Providers.Add<BrotliCompressionProvider>();
          options.Providers.Add<GzipCompressionProvider>();
          options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
              new[] { "image/svg+xml" });
        });
      services.Configure<GzipCompressionProviderOptions>(options =>
       {
         options.Level = CompressionLevel.Fastest;
       });
    }
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
      var tokenSetting = configuration.GetSection(nameof(SiteSetting)).Get<SiteSetting>().TokenSetting;
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        var secretkey = Encoding.UTF8.GetBytes(tokenSetting.SecretKey);
        var encryptionkey = Encoding.UTF8.GetBytes(tokenSetting.EncryptKey);
        var validationParameters = new TokenValidationParameters
        {
          ClockSkew = TimeSpan.Zero,
          RequireSignedTokens = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(secretkey),
          RequireExpirationTime = true,
          ValidateLifetime = true,
          ValidateAudience = true,
          ValidAudience = tokenSetting.Audience,
          ValidateIssuer = true,
          ValidIssuer = tokenSetting.Issuer,
          TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey),
          NameClaimType = ClaimTypes.NameIdentifier
        };
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = validationParameters;
        options.Events = new JwtBearerEvents
        {
          OnAuthenticationFailed = context =>
                {
                  if (context.Exception != null)
                  {
                    var errorFactory = context.HttpContext.RequestServices.GetRequiredService<IErrorFactory>();
                    context.Fail(errorFactory.AccessDenied());
                  }
                  return Task.CompletedTask;
                },
          OnTokenValidated = async context =>
                {
                  var errorFactory = context.HttpContext.RequestServices.GetRequiredService<IErrorFactory>();
                  var tokenMangerContext = context.HttpContext.RequestServices.GetRequiredService<ITokenManagerService>();
                  var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                  var userClaim = claimsIdentity.FindFirst("user-id");
                  var securityStamp = claimsIdentity.FindFirst("security-stamp");
                  var checkLoggedUser = await tokenMangerContext.IsActiveToken();
                  var IsValidUser = await tokenMangerContext.CheckSecurityStamp(userClaim.Value, securityStamp.Value);
                  if (!checkLoggedUser)
                    context.Fail(errorFactory.InvalidToken());
                  if (!IsValidUser)
                    context.Fail(errorFactory.ThisUserIsAltered());
                },
          OnChallenge = context =>
                {
                  var errorFactory = context.HttpContext.RequestServices.GetRequiredService<IErrorFactory>();
                  if (context.AuthenticateFailure != null)
                    return Task.FromException(errorFactory.AccessDenied());
                  else
                    return Task.FromException(errorFactory.AccessDenied());
                }
        };
      });
    }
    public static void AddCustomApiVersioning(this IServiceCollection services, int majorVersion, int minorVersion)
    {
      services.AddApiVersioning(options =>
      {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(majorVersion: majorVersion, minorVersion: minorVersion);
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
      });
    }
  }
}