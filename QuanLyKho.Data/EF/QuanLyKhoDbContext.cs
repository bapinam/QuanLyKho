using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Configurations;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions;
using System;

namespace QuanLyKho.Date.EF
{
    public class QuanLyKhoDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public QuanLyKhoDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure using Fluent API
            modelBuilder.ApplyConfiguration(new ChiTietCheBienCF());
            modelBuilder.ApplyConfiguration(new ChiTietCongThucCF());
            modelBuilder.ApplyConfiguration(new ChiTietDatHangCF());
            modelBuilder.ApplyConfiguration(new ChiTietHoaDonCF());
            modelBuilder.ApplyConfiguration(new CongThucCF());
            modelBuilder.ApplyConfiguration(new DonViTinhCF());
            modelBuilder.ApplyConfiguration(new HoaDonCF());
            modelBuilder.ApplyConfiguration(new KeHoachCheBienCF());
            modelBuilder.ApplyConfiguration(new KeHoachDatHangCF());
            modelBuilder.ApplyConfiguration(new LoaiNguyenVatLieuCF());
            modelBuilder.ApplyConfiguration(new LoaiSanPhamCF());
            modelBuilder.ApplyConfiguration(new NguyenVatLieuCF());
            modelBuilder.ApplyConfiguration(new NhaCungCapCF());
            modelBuilder.ApplyConfiguration(new SanPhamCF());
            modelBuilder.ApplyConfiguration(new NhomSanPhamCF());
            modelBuilder.ApplyConfiguration(new QuanLyMaSoCF());
            modelBuilder.ApplyConfiguration(new ThongBaoCF());
            modelBuilder.ApplyConfiguration(new PhieuCheBienCF());
            modelBuilder.ApplyConfiguration(new ChiTietPhieuCheBienCF());
            modelBuilder.ApplyConfiguration(new ChiMucCF());
            modelBuilder.ApplyConfiguration(new NgayNghiTuChonCF());

            modelBuilder.ApplyConfiguration(new AppUserCF());
            modelBuilder.ApplyConfiguration(new AppRoleCF());

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

            //Data seeding
            modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ChiMuc> ChiMucs { get; set; }
        public DbSet<PhieuCheBien> PhieuCheBiens { get; set; }
        public DbSet<ChiTietPhieuCheBien> ChiTietPhieuCheBiens { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<QuanLyMaSo> QuanLyMaSos { get; set; }
        public DbSet<ChiTietCheBien> ChiTietCheBiens { get; set; }
        public DbSet<ChiTietCongThuc> ChiTietCongThucs { get; set; }
        public DbSet<ChiTietDatHang> ChiTietDatHangs { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<CongThuc> CongThucs { get; set; }
        public DbSet<DonViTinh> DonViTinhs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<KeHoachCheBien> KeHoachCheBiens { get; set; }
        public DbSet<KeHoachDatHang> KeHoachDatHangs { get; set; }
        public DbSet<LoaiNguyenVatLieu> LoaiNguyenVatLieus { get; set; }
        public DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public DbSet<NguyenVatLieu> NguyenVatLieus { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<NgayNghiTuChon> NgayNghiTuChons { get; set; }
        public DbSet<NhomSanPham> NhomSanPhams { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
    }
}