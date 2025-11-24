using GymManagementBLL;
using GymManagementBLL.AttachmentService;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementPL.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace GymManagementPL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //session repo
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            //booking repo
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            //services
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            builder.Services.AddScoped<IAccountService, AccountService>();

            //Auto Mapper
            builder.Services.AddAutoMapper(config => config.AddProfile<MappingProfile>());

            //Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;//default
                options.Password.RequireLowercase = true;//default
                options.Password.RequireUppercase = true;//default
                
                options.User.RequireUniqueEmail = true; //default is false
                options.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromSeconds(2);
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<GymDbContext>();

            //Cookies  (default options)
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });


            #region  Services For Localization

            builder.Services.AddLocalization();
            //Cache
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            builder.Services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(JsonStringLocalizerFactory));
                });


            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar-EG")
                };
                //options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0], uiCulture: supportedCultures[0]);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            #endregion


            var app = builder.Build();

            #region Data Seeding
            //UnManaged Resources

            using var scope= app.Services.CreateScope();

            var gymDbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager=scope.ServiceProvider.GetRequiredService<RoleManager< IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService< UserManager< ApplicationUser>>();

            GymDataSeeding.SeedData(gymDbContext);
            await  IdentityDataSeeding.SeedData(roleManager, userManager);
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();



            #region Localization
            var supportedCultures = new[] { "en-US", "ar-EG" };
            var localizationOptions = new RequestLocalizationOptions()
                                                //.SetDefaultCulture(supportedCultures[0])
                                                .AddSupportedCultures(supportedCultures)
                                                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            #endregion

            app.Run();
        }
    }
}
