using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;

namespace QuanLyKho.Data.Configurations
{
    public class ChiMucCF : IEntityTypeConfiguration<ChiMuc>
    {
        public void Configure(EntityTypeBuilder<ChiMuc> builder)
        {
            builder.ToTable("ChiMuc");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Ten).IsRequired().HasMaxLength(50);
            builder.Property(x => x.NgayBatDau).IsRequired();
            builder.Property(x => x.NgayKetThuc).IsRequired();
            builder.Property(x => x.SoNgayLamViec).HasDefaultValue(0);

            builder.HasIndex(p => p.Ten).IsUnique();
        }
    }
}