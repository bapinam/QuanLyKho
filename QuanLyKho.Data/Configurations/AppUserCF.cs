using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;

namespace QuanLyKho.Data.Configurations
{
    public class AppUserCF : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers").HasComment("Người Dùng");
            builder.Property(x => x.MaSo).IsRequired().HasMaxLength(20);
            builder.Property(x => x.CanCuocCongDan).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Ten).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Ho).IsRequired().HasMaxLength(50);
            builder.Property(x => x.NgaySinh).IsRequired();
            builder.Property(x => x.DiaChi).IsRequired().HasMaxLength(250);
            builder.Property(x => x.HinhAnh).HasDefaultValue(null);
            builder.Property(x => x.LoaiTaiKhoan).HasDefaultValue(LoaiTaiKhoan.NhanVien);

            builder.HasIndex(p => new { p.MaSo, p.CanCuocCongDan }).IsUnique();
        }
    }
}