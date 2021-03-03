using FluentValidation.AspNetCore;
using IdentityModel.Client;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("BackendApi").ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                return handler;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Events = new CookieAuthenticationEvents
                    {
                        // this event is fired everytime the cookie has been validated by the cookie middleware,
                        // so basically during every authenticated request
                        // the decryption of the cookie has already happened so we have access to the user claims
                        // and cookie properties - expiration, etc..
                        OnValidatePrincipal = async x =>
                        {
                            // since our cookie lifetime is based on the access token one,
                            // check if we're more than halfway of the cookie lifetime
                            var now = DateTimeOffset.UtcNow;
                            var timeElapsed = now.Subtract(x.Properties.IssuedUtc!.Value);
                            var timeRemaining = x.Properties.ExpiresUtc!.Value.Subtract(now);

                            if (timeElapsed > timeRemaining)
                            {
                                var identity = (ClaimsIdentity)x.Principal.Identity;
                                var accessTokenClaim = identity.FindFirst("access_token");
                                var refreshTokenClaim = identity.FindFirst("refresh_token");
                                // if we have to refresh, grab the refresh token from the claims, and request
                                // new access token and refresh token
                                var refreshToken = refreshTokenClaim.Value;
                                var response = await new HttpClient().RequestRefreshTokenAsync(new RefreshTokenRequest
                                {
                                    Address = Configuration["Authorization:AuthorityUrl"],
                                    ClientId = Configuration["Authorization:ClientId"],
                                    ClientSecret = Configuration["Authorization:ClientSecret"],
                                    RefreshToken = refreshToken,
                                });

                                if (!response.IsError)
                                {
                                    // everything went right, remove old tokens and add new ones
                                    identity.RemoveClaim(accessTokenClaim);
                                    identity.RemoveClaim(refreshTokenClaim);

                                    identity.AddClaims(new[]
                                    {
                                        new Claim("access_token", response.AccessToken),
                                        new Claim("refresh_token", response.RefreshToken)
                                    });

                                    // indicate to the cookie middleware to renew the session cookie
                                    // the new lifetime will be the same as the old one, so the alignment
                                    // between cookie and access token is preserved
                                    x.ShouldRenew = true;
                                }
                            }
                        }
                    };
                    options.LoginPath = new PathString("/Account/Login");
                    options.ReturnUrlParameter = "RequestPath";
                    options.SlidingExpiration = true;
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = Configuration["Authorization:AuthorityUrl"];
                    options.RequireHttpsMetadata = false;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.ClientId = Configuration["Authorization:ClientId"];
                    options.ClientSecret = Configuration["Authorization:ClientSecret"];
                    options.SaveTokens = true;

                    options.Scope.Add("openid");
                    options.Scope.Add("email");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("api.khoahoc");

                    options.Events = new OpenIdConnectEvents
                    {
                        // that event is called after the OIDC middleware received the auhorisation code,
                        // redeemed it for an access token and a refresh token,
                        // and validated the identity token
                        OnTokenValidated = x =>
                        {
                            // store both access and refresh token in the claims - hence in the cookie
                            var identity = (ClaimsIdentity)x.Principal.Identity;
                            identity.AddClaims(new[]
                            {
                                new Claim("access_token", x.TokenEndpointResponse.AccessToken),
                                new Claim("refresh_token", x.TokenEndpointResponse.RefreshToken)
                            });

                            // so that we don't issue a session cookie but one with a fixed expiration
                            x.Properties.IsPersistent = true;

                            // align expiration of the cookie with expiration of the
                            // access token
                            var accessToken = new JwtSecurityToken(x.TokenEndpointResponse.AccessToken);
                            x.Properties.ExpiresUtc = accessToken.ValidTo;

                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IBaseApiClient, BaseApiClient>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddRecaptcha(new RecaptchaOptions
            {
                SiteKey = Configuration["Recaptcha:SiteKey"],
                SecretKey = Configuration["Recaptcha:SecretKey"]
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            var builder = services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>()); ;
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (env == Environments.Development)
            {
                builder.AddRazorRuntimeCompilation();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseForwardedHeaders();
                app.UseHsts();
            }
            app.UseErrorMiddleware();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}