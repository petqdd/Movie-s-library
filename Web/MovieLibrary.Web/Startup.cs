namespace MovieLibrary.Web
{
    using System.Linq;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MovieLibrary.Common;
    using MovieLibrary.Data;
    using MovieLibrary.Data.Common;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Data.Repositories;
    using MovieLibrary.Data.Seeding;
    using MovieLibrary.Services;
    using MovieLibrary.Services.Common;
    using MovieLibrary.Services.Data;
    using MovieLibrary.Services.Mapping;
    using MovieLibrary.Services.Messaging;
    using MovieLibrary.Services.Models;
    using MovieLibrary.Web.Hubs;
    using MovieLibrary.Web.Services;
    using MovieLibrary.Web.ViewModels;
    
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });
            //services.AddAuthentication().AddFacebook(options =>
            //{
            //    options.AppId = "2862113450734530";
            //    options.AppSecret = "1054e98b01b99946dcd4c81307b625b7";
            //    });
            services.AddSignalR();
            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });
            services.AddSingleton(this.configuration);
            services.Configure<ReCaptchaSettings>(this.configuration.GetSection("GooglereCAPTCHA"));
            services.AddTransient<ReCaptchaService>();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            services.AddTransient<IEmailSender>(x => new SendGridEmailSender("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"));

            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IMoviesService, MoviesService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<ICommentsService, CommentService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IRatingsService, RatingsService>();
            services.AddTransient<IArtistService, ArtistService>();
            services.AddTransient<IImdbScraperService, ImdbScraperService>();
            services.AddTransient<ISearchService, SearchService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var userManager = app.ApplicationServices
                                 .CreateScope()
                                 .ServiceProvider
                                 .GetRequiredService<UserManager<ApplicationUser>>();
            if (!userManager.Users.Any(x => x.UserName == "admin@abv.bg"))
            {
                var newUser = new ApplicationUser
                {
                    UserName = "admin@abv.bg",
                    Email = "admin@abv.bg",
                    EmailConfirmed = true,
                };

                userManager.CreateAsync(newUser, "adminn").GetAwaiter().GetResult();
                userManager.AddToRoleAsync(newUser, GlobalConstants.AdministratorRoleName);
            }

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<ChatHub>("/chat");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
