using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;

namespace QuanLyKho.Data.Configurations
{
    public class ChiTietCheBienCF : IEntityTypeConfiguration<ChiTietCheBien>
    {
        public void Configure(EntityTypeBuilder<ChiTietCheBien> builder)
        {
            builder.ToTable("ChiTietCheBien_Inc");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.DonViTinh).IsRequired().HasMaxLength(50);
            builder.Property(x => x.SoLuong).HasDefaultValue(0);
            builder.Property(x => x.SoLuongDaThem).HasDefaultValue(0);
            builder.Property(x => x.GhiChu).HasMaxLength(250);

            builder.HasOne(x => x.CongThuc).WithMany(x => x.ChiTietCheBiens).HasForeignKey(x => x.IdCongThuc);
            builder.HasOne(x => x.KeHoachCheBien).WithMany(x => x.ChiTietCheBiens).HasForeignKey(x => x.IdKeHoachCheBien);
        }
    }
}