using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.IdentityServer
{
    public class IdentityServerConfiguration
    {
        public static IEnumerable<IdentityResource> Ids => new IdentityResource[] { new IdentityResources.OpenId(), new IdentityResources.Profile(), new IdentityResources.Email() };

        public static IEnumerable<ApiResource> Apis => new ApiResource[] { new ApiResource("api.khoahoc", "Khóa Học Trực Tuyến Api") };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "client_api",
                    ClientName = "Config Identity",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.Email,
                        "api.khoahoc"
                    },
                    RequireConsent = true,
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                  //  RedirectUris = { "https://localhost:44342/signin-oidc" },
                    //PostLogoutRedirectUris =  {   "http://openidclientdemocom:8001/signout-callback-oidc"}
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 3600 * 24, //86400,
                    IdentityTokenLifetime = 3600 * 24, //86400,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    Enabled = true
                },
                new Client
                {
                    ClientId = "client_angular",
                    ClientName = "Config angular",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api.khoahoc"
                    },
                    RequireConsent = true,
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    //  RedirectUris = { "https://localhost:44342/signin-oidc" },
                    //PostLogoutRedirectUris =  {   "http://openidclientdemocom:8001/signout-callback-oidc"}
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 3600 * 24, //86400,
                    IdentityTokenLifetime = 3600 * 24, //86400,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    Enabled = true
                }
            };
    }
}