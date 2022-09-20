using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Gruppeoppgave_1.Models;
using Microsoft.Extensions.Logging;
using Gruppeoppgave_1.DAL;
using Gruppeoppgave_1.Controllers;
using Newtonsoft.Json;
using System;

namespace Gruppeoppgave_1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                // håndterer rekusjonsfeil som kommer av mange-til-mange relasjonene i databasen
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite("Data Source=Database.db"));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IZipCodeRepository, ZipCodeRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IPortRepository, PortRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRouteTimeRepository, RouteTimeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(1800);
                options.Cookie.IsEssential = true;
            });
            services.AddDistributedMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddFile("Logs/Log.txt");
                DBInit.Initialize(app);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSession();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // må ha med denne for at SPA routing skal funke
                endpoints.MapFallbackToFile("/index.html");
            });
        }
    }
}
