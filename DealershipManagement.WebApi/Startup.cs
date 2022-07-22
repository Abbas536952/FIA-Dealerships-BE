using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using DealershipManagement.DataAccess;
using DealershipManagement.DataTransferObjects.ApplicationConstants;
using DealershipManagement.Entities.Account;
using DealershipManagement.Services;
using DealershipManagement.WebAPI.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace DealershipManagement.WebAPI
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
            services.AddControllers(config =>
            {
                config.AddExceptionFilter();
            });

            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddControllers().AddNewtonsoftJson();

            services.AddDbContext<DealershipManagementDbContext>(opts =>
               opts.UseSqlServer(Configuration.GetConnectionString("DealershipManagementDb")));

            //Swagger configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FIA Dealership Swagger Doc",
                    Version = "v1",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });

            //setting up Identity dependencies.
            var identity = services.AddIdentity<UserAccount, SystemRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.AllowedUserNameCharacters = String.Empty;
            }).AddDefaultTokenProviders();

            identity.AddUserManager<UserManager<UserAccount>>();
            identity.AddUserStore<UserStore<UserAccount, SystemRole, DealershipManagementDbContext, string, IdentityUserClaim<string>, UserRole, 
                IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>>();
            identity.AddRoles<SystemRole>();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromDays(3));

            //authentication scheme
            var secretKey = Encoding.ASCII.GetBytes(Configuration[AuthenticationConstants.JwtSecret]);
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration[AuthenticationConstants.Issuer],
                    ValidAudience = Configuration[AuthenticationConstants.Audience],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ClockSkew = TimeSpan.FromMinutes(0),
                };
            });

            services.AddHttpContextAccessor();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Autofac dependencies resolver
            builder.RegisterModule(new ServiceDependencyModule(Configuration));

            builder.RegisterModule(new global::Template.Repository.Dependencies.AutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!string.IsNullOrEmpty(Configuration["Environment"]) && Configuration["Environment"] != "PROD")
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseApiVersioning();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FIA Dealership Management");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
