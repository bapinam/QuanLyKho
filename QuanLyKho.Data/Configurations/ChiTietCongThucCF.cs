using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;

namespace QuanLyKho.Data.Configurations
{
    public class ChiTietCongThucCF : IEntityTypeConfiguration<ChiTietCongThuc>
    {
        public void Configure(EntityTypeBuilder<ChiTietCongThuc> builder)
        {
            builder.ToTable("ChiTietCongThuc_Std");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.DonViTinh).IsRequired().HasMaxLength(50);
            builder.Property(x => x.SoLuong).IsRequired().HasDefaultValue(0);

            builder.HasOne(x => x.CongThuc).WithMany(x => x.ChiTietCongThucs).HasForeignKey(x => x.IdCongThuc);
            builder.HasOne(x => x.NguyenVatLieu).WithMany(x => x.ChiTietCongThucs).HasForeignKey(x => x.IdNguyenVatLieu);
        }
    }
}