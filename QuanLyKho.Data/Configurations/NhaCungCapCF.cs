using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using QuanLyKho.Utilities.Constants;

namespace QuanLyKho.Data.Configurations
{
    public class NhaCungCapCF : IEntityTypeConfiguration<NhaCungCap>
    {
        public void Configure(EntityTypeBuilder<NhaCungCap> builder)
        {
            builder.ToTable("NhaCungCap_Std");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.MaSo).IsRequired().HasMaxLength(20);
            builder.Property(x => x.SoThue).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Ten).IsRequired().HasMaxLength(150).UseCollation(SystemConstants.Collate_AI);
            builder.Property(x => x.SoDienThoai).HasDefaultValue(null);
            builder.Property(x => x.Email).HasDefaultValue(null).IsUnicode(false);
            builder.Property(x => x.DiaChi).IsRequired().HasMaxLength(250);

            builder.HasIndex(p => p.MaSo).IsUnique();
        }
    }
}