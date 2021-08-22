using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using QuanLyKho.Utilities.Constants;

namespace QuanLyKho.Data.Configurations
{
    public class LoaiNguyenVatLieuCF : IEntityTypeConfiguration<LoaiNguyenVatLieu>
    {
        public void Configure(EntityTypeBuilder<LoaiNguyenVatLieu> builder)
        {
            builder.ToTable("LoaiNguyenVatLieu_Std");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.MaSo).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Ten).IsRequired().HasMaxLength(150).UseCollation(SystemConstants.Collate_AI);
            builder.Property(x => x.NhomLoaiNVL).IsRequired();

            builder.HasIndex(p => new { p.MaSo, p.Ten }).IsUnique();
        }
    }
}