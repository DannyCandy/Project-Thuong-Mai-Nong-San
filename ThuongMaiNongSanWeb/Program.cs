using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Data;
using WebApp.Repositories;
using WebApp.HelperClass;
using Microsoft.AspNetCore.Authorization;
using WebApp.Authorization.Requirements.Handlers;
using WebApp.Authorization.Requirements;
using WebApp.Services;
using WebApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApp.MobileApp.Repositories.Abstract;
using WebApp.MobileApp.Repositories.Domain;

namespace WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Cài đặt nhận các request từ bên ngoài thiết bị
            builder.WebHost.ConfigureKestrel(ops =>
            {
                ops.ListenAnyIP(5000);
                ops.ListenAnyIP(7062, listenOps =>
                {
                    listenOps.UseHttps();
                });
            });

            builder.Services.AddDbContext<AgriculturalProductsContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Custom Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AgriculturalProductsContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();

            builder.Services.AddRazorPages();

            //Add ClaimsPrincipal để hiển thị tên người dùng thay vì hello email
            builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

            builder.Services.AddSingleton<IVNPayService, VNPayService>();
            builder.Services.AddTransient<ITokenService, TokenService>();

            //Bổ sung xác thực
            builder.Services.AddAuthentication(opts =>
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme
            )
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = false;  // Tắt yêu cầu HTTPS trong môi trường phát triển
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });
            builder.Services.AddAuthorization(ops =>
            {
                ops.AddPolicy("JwtDefault",
                new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
            });

            // Bổ sung role để tùy chỉnh giao diện
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireMember", policy => policy.RequireRole("Member"));
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequirePoster", policy => policy.RequireRole("Poster"));
            });


            //Bổ sung role để phân quyền đại lý liệu đã được kiểm duyệt hay chưa
            builder.Services.AddScoped<IAuthorizationHandler, IsAgencyHandler>();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CheckingAgency", policy =>
                {
                    policy.Requirements.Add(new IsAgencyRequirement());
                });
            });


            //Dùng session để xử lý giỏ hàng
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddScoped<ISanPhamRepository, EFSanPhamRepository>();
            builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();
            builder.Services.AddScoped<IChungNhanRepository, EFChungNhanRepository>();
            builder.Services.AddScoped<IDaiLyRepository, EFDaiLyRepository>();
            builder.Services.AddScoped<IOrderHistory, EFOrderHistory>();
            builder.Services.AddScoped<IOrderDetailRepository, EFOrderDetailRepository>();

            builder.Services.AddScoped<IUserRepository, EFUserRepository>();
            builder.Services.AddTransient<IEmailService, EmailService>();

            /*// Add service to use localstorage
            builder.Services.AddTransient<LocalStorageService>();*/
            builder.Services.Configure<SMTPConfig>(builder.Configuration.GetSection("SMTPConfig"));
            // Add services to the container.
            builder.Services.AddControllersWithViews();
                
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //Thêm vào để Identity hoạt động
            app.MapRazorPages();
            app.UseAuthentication(); ;

            //Dùng session để xử lý giỏ hàng
            app.UseSession();

            app.UseAuthorization();

            //Route to Admin page
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areasAdmin",
                  pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}"
                );
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areasManager",
                  pattern: "{area:exists}/{controller=Manager}/{action=Index}/{id?}"
                );
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            //Khoi tao role
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Member", "Manager", "Poster" };
                foreach (var item in roles)
                {
                    if (!await roleManager.RoleExistsAsync(item))
                    {
                        await roleManager.CreateAsync(new IdentityRole(item));
                    }
                }
            }

            app.Use(async (context, next) =>
            {
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AgriculturalProductsContext>();
                    var today = DateTime.UtcNow.Date;

                    // Kiểm tra trong session hoặc cookie
                    if (!context.Request.Cookies.ContainsKey("VisitedToday"))
                    {
                        var visitorStat = await dbContext.VisitorStatistics.FirstOrDefaultAsync(v => v.Date == today);
                        if (visitorStat == null)
                        {
                            visitorStat = new VisitorStatistic
                            {
                                Date = today,
                                VisitorCount = 1
                            };
                            dbContext.VisitorStatistics.Add(visitorStat);
                        }
                        else
                        {
                            visitorStat.VisitorCount += 1;
                        }

                        await dbContext.SaveChangesAsync();

                        // Lưu trạng thái vào cookie để không tăng nữa trong ngày
                        context.Response.Cookies.Append("VisitedToday", "true", new CookieOptions
                        {
                            Expires = DateTime.UtcNow.AddHours(24) // Hết hạn sau 24h
                        });
                    }
                }

                await next();
            });

            app.Run();
        }
    }
}