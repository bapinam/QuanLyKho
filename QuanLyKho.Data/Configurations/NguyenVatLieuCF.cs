using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using QuanLyKho.Utilities.Constants;

namespace QuanLyKho.Data.Configurations
{
    public class NguyenVatLieuCF : IEntityTypeConfiguration<NguyenVatLieu>
    {
        public void Configure(EntityTypeBuilder<NguyenVatLieu> builder)
        {
            builder.ToTable("NguyenVatLieu_Std");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.MaSo).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Ten).IsRequired().HasMaxLength(150).UseCollation(SystemConstants.Collate_AI);
            builder.Property(x => x.HinhAnh).HasDefaultValue(null);
            builder.Property(x => x.MoTa).HasDefaultValue(null);
            builder.Property(x => x.SoLuong).HasDefaultValue(0);
            builder.Property(x => x.MucTonCaoNhat).HasDefaultValue(null);
            builder.Property(x => x.MucTonThapNhat).HasDefaultValue(0);

            builder.HasIndex(p => new { p.MaSo, p.Ten }).IsUnique();

            builder.HasOne(x => x.LoaiNguyenVatLieu).WithMany(x => x.NguyenVatLieus).HasForeignKey(x => x.IdLoaiNVL);
        }
    }
}