using Gip.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Gip.Controllers;
using Gip.Services.Interfaces;
using Gip.Services;

namespace Gip
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
            services.AddDbContext<gipDatabaseContext>();

            //Zet hieronder in comment om in te login zonder email confirmation (aangeduid met comment)
            services.AddIdentity<ApplicationUser, IdentityRole>(/**/options => 
            {
                options.SignIn.RequireConfirmedEmail = true;
            }/**/)
                .AddEntityFrameworkStores<gipDatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews();
            //datatables
            services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddControllersWithViews().AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            
            services.AddTransient<MailHandler>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAdministrationService, AdministrationService>();
            services.AddTransient<IFieldOfStudyService, FieldOfStudyService>();
            services.AddTransient<ILectorService, LectorService>();
            services.AddTransient<ILokaalService, LokaalService>();
            services.AddTransient<IPlannerService, PlannerService>();
            services.AddTransient<IVakService, VakService>();
            services.AddTransient<IDataService, DataService>();
            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //Wie is het?
            app.UseAuthentication();

            //Wat mag hij doen?
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
