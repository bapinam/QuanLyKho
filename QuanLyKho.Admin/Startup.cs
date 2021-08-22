using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuanLyKho.ApiIntegration.ChiMucApiClient;
using QuanLyKho.ApiIntegration.CongThucApiClient;
using QuanLyKho.ApiIntegration.DonViTinhApiClient;
using QuanLyKho.ApiIntegration.KeHoachCheBienApiClient;
using QuanLyKho.ApiIntegration.LoaiNguyenVatLieuApiClient;
using QuanLyKho.ApiIntegration.LoaiSanPhamApiClient;
using QuanLyKho.ApiIntegration.NguyenVatLieuApiClient;
using QuanLyKho.ApiIntegration.NhaCungCapApiClient;
using QuanLyKho.ApiIntegration.NhomSanPhamApiClient;
using QuanLyKho.ApiIntegration.QuanLyMaSoApiClient;
using QuanLyKho.ApiIntegration.SanPhamApiClient;
using QuanLyKho.ApiIntegration.UserApiClient;
using QuanLyKho.Data.Entities;
using QuanLyKho.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyKho.Admin.SignalRChat;
using QuanLyKho.ApiIntegration.ThongBaoApiClient;
using QuanLyKho.ApiIntegration.KeHoachDatHangApiClient;
using QuanLyKho.ApiIntegration.CheBienApiClient;
using QuanLyKho.ApiIntegration.DatHangApiClient;
using Hangfire;
using Hangfire.MemoryStorage;
using QuanLyKho.Admin.HangFire;

namespace QuanLyKho.Admin
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
            services.AddHttpClient();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index"; // nếu chưa đăng nhập thì nó về trang login
                    options.AccessDeniedPath = "/Home/Index/";
                });

            services.AddControllersWithViews();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(240);
            });

            //DI
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserApiClient, UserApiClient>();
            services.AddScoped<IChiMucApiClient, ChiMucApiClient>();
            services.AddScoped<IQuanLyMaSoApiClient, QuanLyMaSoApiClient>();
            services.AddScoped<INhaCungCapApiClient, NhaCungCapApiClient>();
            services.AddScoped<ILoaiSanPhamApiClient, LoaiSanPhamApiClient>();
            services.AddScoped<ISanPhamApiClient, SanPhamApiClient>();
            services.AddScoped<ILoaiNguyenVatLieuApiClient, LoaiNguyenVatLieuApiClient>();
            services.AddScoped<INhomSanPhamApiClient, NhomSanPhamApiClient>();
            services.AddScoped<INguyenVatLieuApiClient, NguyenVatLieuApiClient>();
            services.AddScoped<IDonViTinhApiClient, DonViTinhApiClient>();
            services.AddScoped<ICongThucApiClient, CongThucApiClient>();
            services.AddScoped<IKeHoachCheBienApiClient, KeHoachCheBienApiClient>();
            services.AddScoped<IThongBaoApiClient, ThongBaoApiClient>();
            services.AddScoped<IKeHoachDatHangApiClient, KeHoachDatHangApiClient>();
            services.AddScoped<ICheBienApiClient, CheBienApiClient>();
            services.AddScoped<IDatHangApiClient, DatHangApiClient>();

            services.AddScoped<IDeleteNotify, DeleteNotify>();

            // hang fire
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage());

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            // SignalR
            services.AddSignalR();
            //---
            IMvcBuilder builder = services.AddRazorPages();
            var environment = Environment.GetEnvironmentVariable(SystemConstants.Environment);

#if DEBUG
            if (environment == Environments.Development)
            {
                builder.AddRazorRuntimeCompilation();
            }
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IBackgroundJobClient backgroundJobs,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Profile}/{id?}");
                endpoints.MapHub<NhanKeHoachCheBienHub>("/nhanKeHoachCheBienHub");
                endpoints.MapHub<NhanKeHoachDatHangHub>("/nhanKeHoachDatHangHub");
                endpoints.MapHub<NhanPhieuCheBien>("/nhanPhieuCheBienHub");
                endpoints.MapHub<NhacNhoHub>("/nhacNhoHub");
            });

            app.UseHangfireDashboard();
            backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            recurringJobManager.AddOrUpdate
            (
                "Run every minute",
                () => serviceProvider.GetService<IDeleteNotify>().Delete(),
                "* * * * *"
            );
        }
    }
}