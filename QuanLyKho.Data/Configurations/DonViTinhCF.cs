using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;

namespace QuanLyKho.Data.Configurations
{
    public class DonViTinhCF : IEntityTypeConfiguration<DonViTinh>
    {
        public void Configure(EntityTypeBuilder<DonViTinh> builder)
        {
            builder.ToTable("DonViTinh_Std");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Ten).IsRequired().HasMaxLength(50);
            builder.Property(x => x.GiaTriChuyenDoi).IsRequired();
            builder.Property(x => x.LoaiDongGoi).IsRequired().HasDefaultValue(LoaiDongGoi.NguyenVatLieu);
            builder.Property(x => x.CoBan).HasDefaultValue(false);

            builder.Property(x => x.IdNguyenVatLieu).HasDefaultValue(null);
            builder.Property(x => x.IdSanPham).HasDefaultValue(null);
            builder.HasOne(x => x.NguyenVatLieu).WithMany(x => x.DonViTinhs).HasForeignKey(x => x.IdNguyenVatLieu);
            builder.HasOne(x => x.SanPham).WithMany(x => x.DonViTinhs).HasForeignKey(x => x.IdSanPham);
        }
    }
}