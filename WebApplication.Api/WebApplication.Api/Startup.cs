using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebApplication.Core.Domain.Context;
using WebApplication.Core.Repositories;
using WebApplication.Infrastructure.AutoMapper;
using WebApplication.Infrastructure.Repositories;
using WebApplication.Infrastructure.Services.User;
using WebApplication.Infrastructure.Services.User.JwtToken;
using WebApplication.Infrastructure.Services.Voivodeship;
using WebApplication.Infrastructure.Settings;

namespace WebApplication.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Wstrzykiwanie zależności
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtHandler, JwtHandler>();
            services.AddScoped<IVoivodeshipRepository, VoivodeshipRepository>();
            services.AddScoped<IVoivodeshipService, VoivodeshipService>();

            // Łączenie się z bazą danych MS SQL
            services.AddDbContext<DataBaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DevConnection"), b => b.MigrationsAssembly("WebApplication.Api"))
            );

            // Konfiguracja Jwt token
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"])),
                    ValidateIssuer = false,     // Podmiot zdolny do wystawienia tokenu. UWAGA: ValidateIssuer != ValidIssuer
                    ValidateAudience = false    // Strony mogące kożystać z serwisu. UWAGA: ValidateAudience != ValidAudience
                };
            });

            // Konfiguracja AutoMappera
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
