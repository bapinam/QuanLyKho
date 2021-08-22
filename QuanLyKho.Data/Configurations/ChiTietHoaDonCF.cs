using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;

namespace QuanLyKho.Data.Configurations
{
    public class ChiTietHoaDonCF : IEntityTypeConfiguration<ChiTietHoaDon>
    {
        public void Configure(EntityTypeBuilder<ChiTietHoaDon> builder)
        {
            builder.ToTable("ChiTietHoaDon_Inc");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.DonViTinh).IsRequired();
            builder.Property(x => x.SoLuong).IsRequired().HasDefaultValue(1);
            builder.Property(x => x.GiamGia).HasDefaultValue(0);
            builder.Property(x => x.DonGia).HasDefaultValue(0);
            builder.Property(x => x.ThanhTien).HasDefaultValue(0);

            builder.HasOne(x => x.NguyenVatLieu).WithMany(x => x.ChiTietHoaDons).HasForeignKey(x => x.IdNguyenVatLieu);
            builder.HasOne(x => x.HoaDon).WithMany(x => x.ChiTietHoaDons).HasForeignKey(x => x.IdHoaDon);
        }
    }
}