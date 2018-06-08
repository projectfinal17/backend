using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace API
{
    public class Startup
    {
        private readonly int? _httpsPort;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();
            Configuration = builder.Build();

            // Get the HTTPS port (only in development)
            if (env.IsDevelopment())
            {
                var launchJsonConfig = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("Properties\\launchSettings.json")
                    .Build();
                _httpsPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<DatabaseContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("SecurityConnection"));
                opt.UseOpenIddict();
            });
            // Map some of the default claim names to the proper OpenID Connect claim names
            services.Configure<IdentityOptions>(opt =>
            {
                opt.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                opt.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                opt.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            // Add OpenIddict services
            services.AddOpenIddict<Guid>(opt =>
            {
                opt.AddEntityFrameworkCoreStores<DatabaseContext>();
                opt.AddMvcBinders();

                opt.EnableTokenEndpoint("/token");
                opt.AllowPasswordFlow();
            });


            // Add ASP.NET Core Identity
            services.AddIdentity<UserEntity, RoleEntity>()
                .AddEntityFrameworkStores<DatabaseContext, Guid>()
                .AddDefaultTokenProviders();

            // enable CORS 
            services.AddCors();

            // Add MVC
            services.AddMvc(opt =>
            {
                // Require HTTPS for all controllers
                opt.SslPort = _httpsPort;
                opt.Filters.Add(typeof(RequireHttpsAttribute));
            });

            // add repositoires 

            //services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();


            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUserCustomerRepository, UserCutomerRepository>();

            services.AddScoped(typeof(IGenericRepository<,,>), typeof(GenericRepository<,,>));
            services.AddScoped(typeof(IAccessiblePageRepository), typeof(AccessiblePageRepository));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));

            // Add AutoMappers
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Add some test data in development
            var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<RoleEntity>>();
            CreateDefaultRoles(roleManager).Wait();
            // Add default data for Page table
            var databaseContext = app.ApplicationServices.GetRequiredService<DatabaseContext>();
            CreateDefaultPages(databaseContext).Wait();

            if (env.IsDevelopment())
            {
                // Add test roles and users

                var userManager = app.ApplicationServices.GetRequiredService<UserManager<UserEntity>>();
                AddTestUsers(roleManager, userManager).Wait();

            }

            // Use UseCors
            app.UseCors(
               options => options.WithOrigins("*")
               .AllowAnyMethod()
               .AllowAnyHeader()
            );

            app.UseOAuthValidation();
            app.UseOpenIddict();
            app.UseMvc();
        }
        private static async Task CreateDefaultRoles(RoleManager<RoleEntity> roleManager)
        {
            await roleManager.CreateAsync(new RoleEntity("OWNER"));
            await roleManager.CreateAsync(new RoleEntity("USER"));
            //await roleManager.CreateAsync(new RoleEntity("SALESMAN"));
            //await roleManager.CreateAsync(new RoleEntity("WAREHOUSE"));
        }
        private static async Task CreateDefaultPages(DatabaseContext context)
        {
            var entity = context.Set<AccessiblePageEntity>();
            string[] pageNames = new string[]  {"Dashboard",  "Demo" , "Setting" , "AccessiblePage" , "User" };

            for (int i = 0; i < pageNames.Length; i++)
            {
                var pageName = pageNames[i];
                var currentPage = await entity.SingleOrDefaultAsync(p => p.Name == pageName);
                if (currentPage == null)
                {
                    var newPage = new AccessiblePageEntity
                    {
                        Name = pageName,
                        Index = i,
                        ValidRoleNames = "[\"OWNER\"]"
                    };
                    entity.Add(newPage);
                }
            }
            await context.SaveChangesAsync();
        }

        private static async Task AddTestUsers(RoleManager<RoleEntity> roleManager, UserManager<UserEntity> userManager)
        {
            // Add a default user
            var user = new UserEntity
            {
                Email = "admin@mailinator.com",
                UserName = "admin@mailinator.com",
                FirstName = "Admin",
                LastName = "Testerman",
                JobTitle = "IT Guy"
            };

            await userManager.CreateAsync(user, "Admin@123");

            // Put the user in the admin role
            await userManager.AddToRoleAsync(user, "OWNER");

            await userManager.UpdateAsync(user);
        }

        private static async Task AdProductionUsers(RoleManager<RoleEntity> roleManager, UserManager<UserEntity> userManager)
        {
            // Create a admin role
            await roleManager.CreateAsync(new RoleEntity("Owner"));

            //Create a new user
            var user = new UserEntity
            {
                Email = "adminprod@mailinator.com",
                UserName = "adminprod@mailinator.com",
                FirstName = "Admin",
                LastName = "Prod",
                JobTitle = "Director"
            };

            await userManager.CreateAsync(user, "Admin@123");

            // Put the user in the admin role
            await userManager.AddToRoleAsync(user, "Admin");

            await userManager.UpdateAsync(user);
        }
    }
}
