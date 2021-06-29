using BlazorAuth.Areas.Identity;
using BlazorAuth.Data;
using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAuth {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment) {
            Configuration = configuration;
            FileProvider = hostingEnvironment.ContentRootFileProvider;
        }

        public IConfiguration Configuration { get; }
        public IFileProvider FileProvider { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDevExpressControls();
            services.AddRazorPages();
                        /*.AddDefaultDashboardController(configurator => {
                             // Register Dashboard Storage
                             configurator.SetDashboardStorage(new DashboardFileStorage(FileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath));
                             // Create a sample JSON data source
                             DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
                             DashboardJsonDataSource jsonDataSourceUrl = new DashboardJsonDataSource("JSON Data Source (URL)");
                             jsonDataSourceUrl.JsonSource = new UriJsonSource(
                                     new Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json"));
                             jsonDataSourceUrl.RootElement = "Customers";
                             dataSourceStorage.RegisterDataSource("jsonDataSourceUrl", jsonDataSourceUrl.SaveToXml());
                             configurator.SetDataSourceStorage(dataSourceStorage);
                         });*/


            services.AddHttpContextAccessor();

            services.AddSingleton<DashboardConfigurator, MultiTenantDashboardConfigurator>();


            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton<WeatherForecastService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseDevExpressControls();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboard");
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
