using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace CleanArch.Identity.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static List<ApiResource> GetApiResources(string resourceId)
        {
            return new List<ApiResource> { new ApiResource(resourceId) };
        }

        public static List<ApiScope> GetApiScopes()
        {
            return new List<ApiScope> { new ApiScope(name: "myapi.full") };
        }

        public static IEnumerable<Client> GetClients(string clientId, string clientSecret)
        {
            return new List<Client>
            {
                //new Client
                //{
                //    ClientName = "Client Application1",
                //    ClientId = "t8agr5xKt4$3",
                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    ClientSecrets = { new Secret("eb300de4-add9-42f4-a3ac-abd3c60f1919".Sha256()) },
                //    AllowedScopes = new List<string> { "app.api.whatever.read", "app.api.whatever.write" }
                //},
                new Client
                {
                    ClientId = clientId,

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowOfflineAccess = true,
                    //AccessTokenLifetime = 300, // IS4 minimum is 300 seconds (5 minutes), can't be less than 300 seconds.
                    //RefreshTokenUsage = TokenUsage.ReUse,
                    //AbsoluteRefreshTokenLifetime = 12 * 2592000, //360 days
                    //RefreshTokenExpiration = TokenExpiration.Absolute,
                    //AllowedCorsOrigins = { "https://localhost:44358/" }, // Add Cors Origins.
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(clientSecret.Sha256())
                    },
                    // scopes that client has access to
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    },
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }
    }
}