using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;

namespace QuanLyKho.Data.Configurations
{
    public class QuanLyMaSoCF : IEntityTypeConfiguration<QuanLyMaSo>
    {
        public void Configure(EntityTypeBuilder<QuanLyMaSo> builder)
        {
            builder.ToTable("QuanLyMaSo_Std");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Ten).IsRequired().HasMaxLength(20);
            builder.Property(x => x.DinhDau).HasDefaultValue(false);
            builder.Property(x => x.ViTri).HasDefaultValue(0);

            builder.HasIndex(p => p.Ten).IsUnique();
        }
    }
}