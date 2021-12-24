using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using CleanArch.Domain.Settings;
using CleanArch.Identity.Entities;
using CleanArch.Identity.IdentityServer;
using CleanArch.Identity.Services;
using System;
using System.Text;

namespace CleanArch.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // bind settings
            var jwtSettings = new JWTSettings();
            var identityServerSettings = new IdentityServerSettings();
            configuration.Bind("JWTSettings", jwtSettings);
            configuration.Bind("IdentityServerSettings", identityServerSettings);

            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("IdentityConnection"),
                    b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));


            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(config =>
            {
                // With this setup user needs to confirm their email, otherwise invalid user or password exception thrown.
                config.SignIn.RequireConfirmedEmail = true;
            })
            //.AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddDeveloperSigningCredential()

            .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            .AddInMemoryApiResources(IdentityServerConfig.GetApiResources(identityServerSettings.ResourceId))
            .AddInMemoryClients(IdentityServerConfig.GetClients(identityServerSettings.ClientId, identityServerSettings.ClientSecret))
            .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())

            .AddAspNetIdentity<ApplicationUser>();
            //.AddProfileService<IdentityProfileService>();
            ;

            services.AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               })
               .AddJwtBearer(options =>
               {
                   options.RequireHttpsMetadata = false;
                   options.Authority = "https://localhost:44300";

                   // name of the API resource
                   //options.Audience = "app.api.weather";
                   options.Audience = identityServerSettings.ResourceId;

                   options.SaveToken = true;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key)),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });

            // allow CORS
            services.AddSingleton<ICorsPolicyService>((container) => {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();
                return new DefaultCorsPolicyService(logger)
                {
                    AllowAll = true
                };
            });


            #region Services

            //services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IAccountService, AccountService>();
            //services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            //services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();

            #endregion Services
        }
    }
}