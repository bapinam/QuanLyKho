using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;

namespace QuanLyKho.Data.Configurations
{
    public class ChiTietDatHangCF : IEntityTypeConfiguration<ChiTietDatHang>
    {
        public void Configure(EntityTypeBuilder<ChiTietDatHang> builder)
        {
            builder.ToTable("ChiTietDatHang_Inc");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.SoLuong).HasDefaultValue(0).IsRequired();
            builder.Property(x => x.SoLuongDaThem).HasDefaultValue(0);
            builder.Property(x => x.DonViTinh).HasMaxLength(50).IsRequired();
            builder.Property(x => x.DonGiaGoiY).HasDefaultValue(null);
            builder.Property(x => x.GhiChu).HasMaxLength(250);
            builder.Property(x => x.IdNhaCungCap).HasDefaultValue(null);

            builder.HasOne(x => x.NguyenVatLieu).WithMany(x => x.ChiTietDatHangs).HasForeignKey(x => x.IdNguyenVatLieu);
            builder.HasOne(x => x.NhaCungCap).WithMany(x => x.ChiTietDatHangs).HasForeignKey(x => x.IdNhaCungCap);
            builder.HasOne(x => x.KeHoachDatHang).WithMany(x => x.ChiTietDatHangs)
                .HasForeignKey(x => x.IdKeHoachDatHang);
        }
    }
}