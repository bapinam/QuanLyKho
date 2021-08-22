using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuanLyKho.Data.Entities;
using QuanLyKho.Date.EF;
using QuanLyKho.Service.AutoMapper;
using QuanLyKho.Service.CheBienService;
using QuanLyKho.Service.ChiMucService;
using QuanLyKho.Service.Common;
using QuanLyKho.Service.CongThucService;
using QuanLyKho.Service.DatHangService;
using QuanLyKho.Service.DonViTinhService;
using QuanLyKho.Service.KeHoachCheBienService;
using QuanLyKho.Service.KeHoachDatHangService;
using QuanLyKho.Service.LoaiNguyenVatLieuService;
using QuanLyKho.Service.LoaiSanPhamService;
using QuanLyKho.Service.NguyenVatLieuService;
using QuanLyKho.Service.NhaCungCapService;
using QuanLyKho.Service.NhomSanPhamService;
using QuanLyKho.Service.QuanLyMaSoService;
using QuanLyKho.Service.SanPhamService;
using QuanLyKho.Service.ThongBaoService;
using QuanLyKho.Service.UserService;
using QuanLyKho.Utilities.Constants;
using System.Collections.Generic;

namespace QuanLyKho.API
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
            services.AddDbContext<QuanLyKhoDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString(SystemConstants.MainConectionString)));

            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<QuanLyKhoDbContext>()
                .AddDefaultTokenProviders();

            //Declare DI
            services.AddScoped<IDatHangService, DatHangService>();
            services.AddScoped<ICheBienService, CheBienService>();
            services.AddScoped<IKeHoachDatHangService, KeHoachDatHangService>();
            services.AddScoped<IThongBaoService, ThongBaoService>();
            services.AddScoped<IKeHoachCheBienService, KeHoachCheBienService>();
            services.AddScoped<IDonViTinhService, DonViTinhService>();
            services.AddScoped<INguyenVatLieuService, NguyenVatLieuService>();
            services.AddScoped<ICongThucService, CongThucService>();
            services.AddScoped<ILoaiNguyenVatLieuService, LoaiNguyenVatLieuService>();
            services.AddScoped<INhomSanPhamService, NhomSanPhamService>();
            services.AddScoped<ISanPhamService, SanPhamService>();
            services.AddScoped<ILoaiSanPhamService, LoaiSanPhamService>();
            services.AddScoped<INhaCungCapService, NhaCungCapService>();
            services.AddScoped<IQuanLyMaSoService, QuanLyMaSoService>();
            services.AddScoped<IChiMucService, ChiMucService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddScoped<RoleManager<AppRole>, RoleManager<AppRole>>();

            services.AddScoped<IFileStorageService, FileStorageService>();

            services.AddControllers();

            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QuanLyKho.API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });
            });

            // nếu có token thì nó tự giải mã ra, ko đúng sẽ trả về 401
            string issuer = Configuration.GetValue<string>(SystemConstants.Token.Issuer);
            string signingKey = Configuration.GetValue<string>(SystemConstants.Token.Key);
            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = System.TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuanLyKho.API v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
              {
                  endpoints.MapControllers();
              });
        }
    }
}