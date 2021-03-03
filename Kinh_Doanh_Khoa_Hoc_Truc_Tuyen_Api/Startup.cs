using FluentValidation.AspNetCore;
using IdentityServer4.Services;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.HubConfig;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.IdentityServer;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Services;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api
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
            //1. Setup entity framework
            services.AddDbContext<EKhoaHocDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                    , o => o.MigrationsAssembly("Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain")));
            //2. Setup idetntity
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<EKhoaHocDbContext>().AddDefaultTokenProviders();
            services.AddTransient<DbInitializer>();
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            });

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
               .AddInMemoryApiResources(IdentityServerConfiguration.Apis)
               .AddInMemoryClients(IdentityServerConfiguration.Clients)
               .AddInMemoryIdentityResources(IdentityServerConfiguration.Ids)
               .AddProfileService<IdentityProfileService>()
               .AddAspNetIdentity<AppUser>()
               .AddDeveloperSigningCredential();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.Authority = "https://localhost:44342/";
                o.RequireHttpsMetadata = false;
                o.Audience = "api.khoahoc";
                /* This block was missing */
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notification"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
            services.AddTransient<IProfileService, IdentityProfileService>();
            services.AddTransient<IStorageService, StorageService>();
            services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("KhoaHocPolicy", builder =>
                {
                    //builder.AllowAnyMethod()
                    //    .AllowAnyHeader()
                    //    .WithOrigins("*;http://localhost:4200;https://localhost:44352;http://localhost:6717")
                    //    .AllowCredentials();
                    builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
                        .SetIsOriginAllowed((host) => true);
                });
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Project Khóa Học Trực Tuyến",
                    Description = "Website Quản Lý Khóa Học Trực Tuyến Online",
                    Contact = new OpenApiContact
                    {
                        Name = "Trần Bảo Long",
                        Email = "lockhanhlong007@gmail.com",
                        Url = new Uri("https://github.com/lockhanhlong007")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Trần Bảo Long - Github",
                        Url = new Uri("https://github.com/lockhanhlong007/Kinh_Doanh_Khoa_Hoc_Truc_Tuyen")
                    }
                });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme, Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>{ "api.khoahoc" }
                    }
                });
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserCreateRequestValidator>());
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorMiddleware();

            app.UseCors("KhoaHocPolicy");

            app.UseRouting();

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Khóa Học Trực Tuyến Api");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/notification");
            });
        }
    }
}