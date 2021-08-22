using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using System;

namespace QuanLyKho.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // admin hệ thống
            var roleId = new Guid("88A28F0B-99CD-4893-AB70-0189C8C7FEC5");
            var adminId = new Guid("0275D5A7-DA4A-41C3-85ED-15E53CD1B7A0");

            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleId,
                Name = "AdminHeThong",
                NormalizedName = "AdminHeThong",
                MoTa = "Vai trò Administrator Hệ Thống"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminId,
                MaSo = "AdminHeThong",
                CanCuocCongDan = "0123456789",
                UserName = "adminHeThong",
                NormalizedUserName = "adminHeThong",
                Email = "khoaluan@gmail.com",
                NormalizedEmail = "khoaluan@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123456"),
                SecurityStamp = string.Empty,
                Ho = "Lương Nhựt",
                Ten = "Nam",
                NgaySinh = new DateTime(2020, 01, 31),
                GioiTinh = true,
                DiaChi = "Cần Thơ",
                TinhTrangLamViec = TinhTrangLamViec.DangLamViec,
                LoaiTaiKhoan = LoaiTaiKhoan.AdminHeThong
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

            // admin web
            var roleIdWeb = new Guid("C7CBDFD3-BDDA-4A4A-B2AE-5475F7400F56");
            var adminIdWeb = new Guid("53E27774-4D9E-47BC-A7DC-B4FAA6B9E140");

            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = roleIdWeb,
                Name = "Admin",
                NormalizedName = "Admin",
                MoTa = "Vai trò Administrator"
            });

            var hasherWeb = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = adminIdWeb,
                MaSo = "Admin",
                CanCuocCongDan = "0987654321",
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasherWeb.HashPassword(null, "123456"),
                SecurityStamp = string.Empty,
                Ho = "Lương Nhựt",
                Ten = "Nam",
                NgaySinh = new DateTime(2020, 01, 31),
                GioiTinh = true,
                DiaChi = "Cần Thơ",
                TinhTrangLamViec = TinhTrangLamViec.DangLamViec,
                LoaiTaiKhoan = LoaiTaiKhoan.Admin
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleIdWeb,
                UserId = adminIdWeb
            });
            // admin web
            var vaiTro_id1 = new Guid("1ED2A1CF-5D3D-471F-BAF0-3B72D7161969");
            var vaiTro_id2 = new Guid("B2D0F535-7053-4D6C-9C3F-28E892858683");

            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = vaiTro_id1,
                Name = "QuanLyDatHang",
                NormalizedName = "QuanLyDatHang",
                MoTa = "Vai trò Quản lý đặt hàng"
            },
            new AppRole
            {
                Id = vaiTro_id2,
                Name = "QuanLyCheBien",
                NormalizedName = "QuanLyCheBien",
                MoTa = "Vai trò Quản lý chế biến"
            });

            // dữ liệu hồ sơ
            //modelBuilder.Entity<LoaiNguyenVatLieu>().HasData(
            //   new LoaiNguyenVatLieu() { Id = 1, MaSo = "LoaiNVL1", Ten = "Bột", NhomLoaiNVL = NhomLoaiNVL.NguyenLieu },
            //   new LoaiNguyenVatLieu() { Id = 2, MaSo = "LoaiNVL2", Ten = "Trái Cây", NhomLoaiNVL = NhomLoaiNVL.NguyenLieu }
            //   );

            //modelBuilder.Entity<NguyenVatLieu>().HasData(
            //   new NguyenVatLieu()
            //   {
            //       Id = 1,
            //       MaSo = "NVL1",
            //       Ten = "Bột gạo",
            //       MucTonThapNhat = 1,
            //       MucTonCaoNhat = 100,
            //       SoLuong = 1000,
            //       MoTa = "Bột gạo dùng để làm bánh",
            //       IdLoaiNVL = 1
            //   },
            //   new NguyenVatLieu()
            //   {
            //       Id = 2,
            //       MaSo = "NVL2",
            //       Ten = "Cam",
            //       MucTonThapNhat = 10,
            //       MucTonCaoNhat = 100,
            //       SoLuong = 10,
            //       MoTa = "Bột gạo dùng để làm bánh",
            //       IdLoaiNVL = 2
            //   }
            //   );

            //modelBuilder.Entity<NhomSanPham>().HasData(
            // new NhomSanPham()
            // {
            //     Id = 1,
            //     MaSo = "NhomLSP1",
            //     Ten = "Thực Phẩm",
            // }
            // );

            //modelBuilder.Entity<LoaiSanPham>().HasData(
            //  new LoaiSanPham()
            //  {
            //      Id = 1,
            //      MaSo = "LSP1",
            //      Ten = "Bánh",
            //      IdNhomSanPham = 1
            //  }
            //  );

            //modelBuilder.Entity<SanPham>().HasData(
            //   new SanPham()
            //   {
            //       Id = 1,
            //       MaSo = "SP1",
            //       Ten = "Bánh Cam",
            //       MucTonCaoNhat = 10,
            //       MucTonThapNhat = 0,
            //       SoLuong = 100,
            //       MoTa = "Bánh làm từ cam vắt",
            //       IdLoaiSanPham = 1
            //   }
            //   );

            //modelBuilder.Entity<DonViTinh>().HasData(
            //  new DonViTinh()
            //  {
            //      Id = 1,
            //      Ten = "g",
            //      GiaTriChuyenDoi = 1,
            //      CoBan = true,
            //      LoaiDongGoi = LoaiDongGoi.NguyenVatLieu,
            //      IdNguyenVatLieu = 1
            //  },
            //    new DonViTinh()
            //    {
            //        Id = 2,
            //        Ten = "kg",
            //        GiaTriChuyenDoi = 1000,
            //        CoBan = false,
            //        LoaiDongGoi = LoaiDongGoi.NguyenVatLieu,
            //        IdNguyenVatLieu = 1
            //    },
            //    new DonViTinh()
            //    {
            //        Id = 3,
            //        Ten = "Trái",
            //        GiaTriChuyenDoi = 1,
            //        CoBan = true,
            //        LoaiDongGoi = LoaiDongGoi.NguyenVatLieu,
            //        IdNguyenVatLieu = 2
            //    },
            //     new DonViTinh()
            //     {
            //         Id = 4,
            //         Ten = "Cái",
            //         GiaTriChuyenDoi = 1,
            //         CoBan = true,
            //         LoaiDongGoi = LoaiDongGoi.SanPham,
            //         IdSanPham = 1
            //     },
            //     new DonViTinh()
            //     {
            //         Id = 5,
            //         Ten = "Bịt",
            //         GiaTriChuyenDoi = 6,
            //         CoBan = false,
            //         LoaiDongGoi = LoaiDongGoi.SanPham,
            //         IdSanPham = 1
            //     }
            //  );

            //modelBuilder.Entity<NhaCungCap>().HasData(
            //  new NhaCungCap()
            //  {
            //      Id = 1,
            //      MaSo = "NCC1",
            //      SoThue = "011",
            //      Ten = "Công Ty ABO",
            //      SoDienThoai = "0869696969",
            //      Email = "nhacungcap@gmail.com",
            //      DiaChi = "Cần Thơ"
            //  }
            //  );

            //modelBuilder.Entity<CongThuc>().HasData(
            // new CongThuc()
            // {
            //     Id = 1,
            //     MaSo = "CTCB1",
            //     Ten = "Làm bánh Cam",
            //     DinhDau = true,
            //     IdSanPham = 1
            // }
            // );

            //modelBuilder.Entity<ChiTietCongThuc>().HasData(
            // new ChiTietCongThuc()
            // {
            //     Id = 1,
            //     SoLuong = 1,
            //     DonViTinh = "kg",
            //     IdCongThuc = 1,
            //     IdNguyenVatLieu = 1
            // },
            //  new ChiTietCongThuc()
            //  {
            //      Id = 2,
            //      SoLuong = 1,
            //      DonViTinh = "Trái",
            //      IdCongThuc = 1,
            //      IdNguyenVatLieu = 2
            //  }
            // );
        }
    }
}