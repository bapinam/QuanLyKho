using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using System;

namespace QuanLyKho.Data.Configurations
{
    public class PhieuCheBienCF : IEntityTypeConfiguration<PhieuCheBien>
    {
        public void Configure(EntityTypeBuilder<PhieuCheBien> builder)
        {
            builder.ToTable("PhieuCheBien_Inc");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.MaSo).IsRequired().HasMaxLength(20);
            builder.Property(x => x.NgayTao).HasDefaultValue(DateTime.Now);
            builder.Property(x => x.NgayHoanThanh).HasDefaultValue(DateTime.Now);

            builder.Property(x => x.IdNguoiDuyet).HasDefaultValue(null);

            builder.HasIndex(p => p.MaSo).IsUnique();

            builder.HasOne(x => x.KeHoachCheBien).WithMany(x => x.PhieuCheBiens).HasForeignKey(x => x.IdKeHoach);
            builder.HasOne(x => x.NguoiTao).WithMany(x => x.NguoiTaoPhieus).HasForeignKey(x => x.IdNguoiTao);
            builder.HasOne(x => x.NguoiDuyet).WithMany(x => x.NguoiDuyetPhieus).HasForeignKey(x => x.IdNguoiDuyet);
            builder.HasOne(x => x.ChiMuc).WithMany(x => x.PhieuCheBiens).HasForeignKey(x => x.IdChiMuc);
        }
    }
}