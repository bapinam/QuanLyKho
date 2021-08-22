using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using System;

namespace QuanLyKho.Data.Configurations
{
    public class KeHoachCheBienCF : IEntityTypeConfiguration<KeHoachCheBien>
    {
        public void Configure(EntityTypeBuilder<KeHoachCheBien> builder)
        {
            builder.ToTable("KeHoachCheBien_Inc");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.MaSo).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Ten).IsRequired().HasMaxLength(50);
            builder.Property(x => x.NgayTao).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.NgayBatDauDuKien).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.NgayKetThucDuKien).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.GhiChu).HasMaxLength(250);
            builder.HasIndex(x => x.MaSo).IsUnique();

            builder.HasOne(x => x.NguoiTao).WithMany(x => x.NguoiTaoKeHoachCheBiens).HasForeignKey(x => x.IdNguoiTao);
            builder.HasOne(x => x.NguoiNhan).WithMany(x => x.NguoiNhanKeHoachCheBiens).HasForeignKey(x => x.IdNguoiNhan);
            builder.HasOne(x => x.ChiMuc).WithMany(x => x.KeHoachCheBiens).HasForeignKey(x => x.IdChiMuc);
        }
    }
}