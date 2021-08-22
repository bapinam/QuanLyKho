using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;

namespace QuanLyKho.Data.Configurations
{
    public class ChiTietPhieuCheBienCF : IEntityTypeConfiguration<ChiTietPhieuCheBien>
    {
        public void Configure(EntityTypeBuilder<ChiTietPhieuCheBien> builder)
        {
            builder.ToTable("ChiTietPhieuCheBien_Inc");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.SoLuong).HasDefaultValue(0);

            builder.HasOne(x => x.CongThuc).WithMany(x => x.ChiTietPhieuCheBiens).HasForeignKey(x => x.IdCongThuc);
            builder.HasOne(x => x.PhieuCheBien).WithMany(x => x.ChiTietPhieuCheBiens).HasForeignKey(x => x.IdPhieu);
        }
    }
}