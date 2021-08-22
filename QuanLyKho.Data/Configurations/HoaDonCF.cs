using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using System;

namespace QuanLyKho.Data.Configurations
{
    public class HoaDonCF : IEntityTypeConfiguration<HoaDon>
    {
        public void Configure(EntityTypeBuilder<HoaDon> builder)
        {
            builder.ToTable("HoaDon_Inc");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.SoHoaDon).IsRequired().HasMaxLength(20);
            builder.Property(x => x.MaLuuTru).IsRequired().HasMaxLength(20);
            builder.Property(x => x.ThueSuat).HasDefaultValue(null);
            builder.Property(x => x.NgayTao).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.NgayMua).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.TongTien).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.SoTienDaTra).HasDefaultValue(0);

            builder.HasIndex(p => p.MaLuuTru).IsUnique();

            builder.HasOne(x => x.NhaCungCap).WithMany(x => x.HoaDons).HasForeignKey(x => x.IdNCC);
            builder.HasOne(x => x.KeHoachDatHang).WithMany(x => x.HoaDons).HasForeignKey(x => x.IdKeHoach);
            builder.HasOne(x => x.NguoiTao).WithMany(x => x.HoaDons).HasForeignKey(x => x.IdNguoiTao);
            builder.HasOne(x => x.ChiMuc).WithMany(x => x.HoaDons).HasForeignKey(x => x.IdChiMuc);
        }
    }
}