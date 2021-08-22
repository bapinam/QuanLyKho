using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;

namespace QuanLyKho.Data.Configurations
{
    public class CongThucCF : IEntityTypeConfiguration<CongThuc>
    {
        public void Configure(EntityTypeBuilder<CongThuc> builder)
        {
            builder.ToTable("CongThuc_Std");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.MaSo).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Ten).IsRequired().HasMaxLength(50);
            builder.Property(x => x.DinhDau).HasDefaultValue(false);
            builder.Property(x => x.GhiChu).HasMaxLength(250);
            builder.HasIndex(p => new { p.MaSo, p.Ten }).IsUnique();

            builder.HasOne(x => x.SanPham).WithMany(x => x.CongThucs).HasForeignKey(x => x.IdSanPham);
        }
    }
}