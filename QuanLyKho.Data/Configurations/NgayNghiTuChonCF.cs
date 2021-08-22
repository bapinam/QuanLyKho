using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyKho.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Data.Configurations
{
    public class NgayNghiTuChonCF : IEntityTypeConfiguration<NgayNghiTuChon>
    {
        public void Configure(EntityTypeBuilder<NgayNghiTuChon> builder)
        {
            builder.ToTable("NgayNghiTuChon_Inc");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.NgayNghi).IsRequired();

            builder.HasOne(x => x.ChiMuc).WithMany(x => x.NgayNghiTuChons).HasForeignKey(x => x.IdChiMuc);
        }
    }
}