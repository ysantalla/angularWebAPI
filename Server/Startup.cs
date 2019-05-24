using Server.Services;
using Server.Services.Interfaces;
using Server;
using Server.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Pomelo.EntityFrameworkCore.MySql;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private readonly string EnabledCORS = "_EnabledCORS";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => 
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.User.RequireUniqueEmail = true;

                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options => 
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                                                            {
                                                                ValidateIssuer = true,
                                                                ValidateAudience = true,
                                                                ValidateIssuerSigningKey = true,
                                                                ValidateLifetime = true,

                                                                ValidIssuer = Configuration["JWT:ValidIssuer"],
                                                                ValidAudience = Configuration["JWT:ValidAudience"],
                                                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"])),
                                                            };
                });
            
                
            services.AddMvc();

            ConfigureCustomServices(services);

            services.AddCors(options =>
            {
                options.AddPolicy(EnabledCORS,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                    });
            });

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                }); 
                
                c.SwaggerDoc("v1", new Info 
                { 
                    Title = "RestFull API", 
                    Version = "v1",
                    Contact = new Contact { Name="Raydel Alvarez Ramírez", Email="raydel@gmail.com" }
                });
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../Client/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseCors(EnabledCORS);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<ICitizenshipService, CitizenshipService>();
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGuestService, GuestService>();

            services.AddSingleton<ITokenService, TokenService>();            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
