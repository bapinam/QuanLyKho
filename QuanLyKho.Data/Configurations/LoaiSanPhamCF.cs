using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using QuanLyKho.Utilities.Constants;

namespace QuanLyKho.Data.Configurations
{
    internal class LoaiSanPhamCF : IEntityTypeConfiguration<LoaiSanPham>
    {
        public void Configure(EntityTypeBuilder<LoaiSanPham> builder)
        {
            builder.ToTable("LoaiSanPham_Std");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.MaSo).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Ten).IsRequired().HasMaxLength(150).UseCollation(SystemConstants.Collate_AI);
            builder.HasIndex(p => new { p.MaSo, p.Ten }).IsUnique();

            builder.HasOne(x => x.NhomSanPham).WithMany(x => x.LoaiSanPhams).HasForeignKey(x => x.IdNhomSanPham);
        }
    }
}