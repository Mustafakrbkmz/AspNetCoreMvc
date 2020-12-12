using AspNetCoreMvc2.Introduction.Identity;
using AspNetCoreMvc2.Introduction.Models;
using AspNetCoreMvc2.Introduction.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetCoreMvc2.Introduction
{
    public class Startup
    {

        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //var connection = @"Server=(localdb)\mssqllocaldb;Database=SchoolDB;Trusted_Connection=true";
            services.AddDbContext<SchoolContext>(options => options.UseSqlServer(_configuration["DbConnection"]));
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(_configuration["DbConnection"]));
            services.AddIdentity<AppIdentityUser, AppIdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {

                options.Password.RequireDigit = true; //Parolada sayı olsun mu?
                options.Password.RequireLowercase = true; //Küçük harf zorunluluğu olsun mu
                options.Password.RequiredLength = 6; // minimum 6 karakter uzunluk
                options.Password.RequireNonAlphanumeric = true; // Alfa nümerik karakter olsun mu
                options.Password.RequireUppercase = true; // Büyük harf zorunluluğu olsun mu

                options.Lockout.MaxFailedAccessAttempts = 5; // Şifre denemesi max 5 kere yanlış olunca kitlesin
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Kaç dakika kitlensin --5dk
                options.Lockout.AllowedForNewUsers = true; // Bu kurallar yeni kullanıcı içinde geçerli olsun mu?

                options.User.RequireUniqueEmail = true; //Her kullanıcının benzersiz bir e-postasına sahip olmasını gerektirir.
                options.SignIn.RequireConfirmedEmail = true; //Yeni login olduğunda mail ile doğrulama istensin mi
                options.SignIn.RequireConfirmedPhoneNumber = false; //Telefon ile doğrulama olsun mu?

                //Detaylar için https://docs.microsoft.com/tr-tr/aspnet/core/security/authenRequireUniqueEmailcation/identity-configuration?view=aspnetcore-3.1

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Security/Login"; // Kullanıcının login olacağı path i set ediyoruz.
                options.LogoutPath = "/Security/Logout";
                options.AccessDeniedPath = "/Security/AccessDenied"; // Kişinin erişim yetkisi olmadığında yönlendirileceği action
                options.SlidingExpiration = true; // cookie 20dk ise kullanıcı 15. dakikada giriş yaparsa o 20 dk sıfırlansın.
                options.Cookie = new Microsoft.AspNetCore.Http.CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".AspNetCoreDemo.Security.Cookie",
                    Path = "/", //cookie leri tutacağı yerde direkt route a koyacak
                    SameSite = SameSiteMode.Lax, // Bizim belirttiğimiz noktalar dışında istekte bulunmasını engellemek için .Strict verebiliriz.
                    // Biz aynı domainde bir postman vasıtasıyla erişebilirz diye lax yapıyoruz.
                    SecurePolicy = CookieSecurePolicy.SameAsRequest // requestteki confg ile bizdeki confg aynı olacağını sağlamak için.
                };

                }
            );


            services.AddScoped<ICalculator, Calculator18>();//Hangi hesaplama türüne göre çalıştığımızı burdan belirterek controller tarafında hiçbir değişiklik yapmadan istediğimizi elde edeceğiz
            services.AddSession(); // session servisini ekliyoruz.
            services.AddDistributedMemoryCache(); //Session bilgisinin nerede tutulacağını belirliyoruz.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            //env.EnvironmentName = EnvironmentName.Production;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/error");
            //}
            app.UseSession(); //UseMvc middleware' in altına eklersek session indexi hataya düşüyor. 

            app.UseAuthentication(); //cookie ile ilgili.

            app.UseMvc(ConfigureRoutes); // ConfigureRoute methodu içerisinde birden fazla route tanımlayıp methodu burda verdik.

        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{Controller=Home}/{Action=Index}/{id?}");
            routeBuilder.MapRoute("MyRoute", "Mustafa/{Controller=Home}/{Action=Index3}/{id?}");
            routeBuilder.MapRoute("areas", "{area:exists}/{controller=Home}/{action=index}/{id?}");
        }
    }
}
