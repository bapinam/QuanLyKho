using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using System;

namespace QuanLyKho.Data.Configurations
{
    internal class ThongBaoCF : IEntityTypeConfiguration<ThongBao>
    {
        public void Configure(EntityTypeBuilder<ThongBao> builder)
        {
            builder.ToTable("ThongBao_Inc");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Ten).IsRequired();
            builder.Property(x => x.DuongDan).IsRequired().HasMaxLength(250);
            builder.Property(x => x.NgayNhan).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.MaKeHoach).HasMaxLength(20);

            builder.HasOne(x => x.NguoiGui).WithMany(x => x.NguoiGuiThongBaos).HasForeignKey(x => x.IdNguoiGui);
            builder.HasOne(x => x.NguoiNhan).WithMany(x => x.NguoiNhanThongBaos).HasForeignKey(x => x.IdNguoiNhan);
            builder.HasOne(x => x.ChiMuc).WithMany(x => x.ThongBaos).HasForeignKey(x => x.IdChiMuc);
        }
    }
}