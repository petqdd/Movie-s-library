namespace Sandbox
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    using MovieLibrary.Data;
    using MovieLibrary.Data.Common;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;
    using MovieLibrary.Data.Repositories;
    using MovieLibrary.Data.Seeding;
    using MovieLibrary.Services.Data;
    using MovieLibrary.Services.Messaging;

    using CommandLine;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class Program
    {
        public static int Main(string[] args)
        {
            int value = 1234;
            string id = value.ToString("D" + 7.ToString());
            string test = "2344".PadLeft(7, '0');
            string test2 = "4".PadLeft(7, '0');
            string test3 = "43334".PadLeft(7, '0');
            string test4 = "423444".PadLeft(7, '0');
            string test5 = "44".PadLeft(7, '0');
            string test6 = "1234567".PadLeft(7, '0');

            

            Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

            // Seed data on application startup
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                serviceProvider = serviceScope.ServiceProvider;

                return Parser.Default.ParseArguments<SandboxOptions>(args).MapResult(
                    opts => SandboxCode(opts, serviceProvider).GetAwaiter().GetResult(),
                    _ => 255);
            }
        }

        private static async Task<int> SandboxCode(SandboxOptions options, IServiceProvider serviceProvider)
        {
            var sw = Stopwatch.StartNew();

            var settingsService = serviceProvider.GetService<ISettingsService>();
            Console.WriteLine($"Count of settings: {settingsService.GetCount()}");

            Console.WriteLine(sw.Elapsed);
            return await Task.FromResult(0);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseLoggerFactory(new LoggerFactory()));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
        }
    }
}
