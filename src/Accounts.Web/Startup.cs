using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgrammerGrammar.Accounts.Core;
using ProgrammerGrammar.Accounts.Infrastructure;
using ProgrammerGrammar.Accounts.Infrastructure.Data;

namespace ProgrammerGrammar.Accounts.Web
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
            services
                .AddAutoMapper(Assembly.GetExecutingAssembly(), Assembly.Load("Accounts.Core"), Assembly.Load("Accounts.Infrastructure"))
                .AddMediatR(Assembly.GetExecutingAssembly());

            services
                .AddCore()
                .AddInfrastructure(Configuration);

            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AccountsDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "/login";
                    options.UserInteraction.LogoutUrl = "/logout";
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddApiAuthorization<IdentityUser, AccountsDbContext>();

            services
                .AddAuthentication()
                .AddIdentityServerJwt();

            services
                .AddOidcStateDataFormatterCache()
                .AddDistributedMemoryCache();

            services
                .AddControllers();

            services
                .AddSpaStaticFiles(configuration => configuration.RootPath = "dist");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AccountsDbContext db)
        {
            db.Database.Migrate();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // for development only
            // chrome requires secure cookies w/ http, so let's make it lax
            if (env.IsDevelopment())
                app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
            else
                app.UseHttpsRedirection();

            app.UseStaticFiles();
            if (!env.IsDevelopment())
                app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "./";

                if (env.IsDevelopment())
                    spa.UseProxyToSpaDevelopmentServer("http://accounts.webapp:4200");
            });
        }
    }
}