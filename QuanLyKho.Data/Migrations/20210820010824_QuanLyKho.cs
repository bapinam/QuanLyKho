using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyKho.Data.Migrations
{
    public partial class QuanLyKho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                },
                comment: "Vai Trò");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CanCuocCongDan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ho = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<bool>(type: "bit", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhTrangLamViec = table.Column<int>(type: "int", nullable: false),
                    LoaiTaiKhoan = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                },
                comment: "Người Dùng");

            migrationBuilder.CreateTable(
                name: "ChiMuc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayNghiMacDinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoNgayLamViec = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiMuc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoaiNguyenVatLieu_Std",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AI"),
                    NhomLoaiNVL = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiNguyenVatLieu_Std", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhaCungCap_Std",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SoThue = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AI"),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCap_Std", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NhomSanPham_Std",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AI")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhomSanPham_Std", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuanLyMaSo_Std",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DinhDau = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MaLoai = table.Column<int>(type: "int", nullable: false),
                    ViTri = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuanLyMaSo_Std", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "KeHoachCheBien_Inc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 123, DateTimeKind.Local).AddTicks(3837)),
                    NgayBatDauDuKien = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 123, DateTimeKind.Local).AddTicks(4333)),
                    NgayKetThucDuKien = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 123, DateTimeKind.Local).AddTicks(4640)),
                    GhiChu = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NhanKeHoach = table.Column<bool>(type: "bit", nullable: false),
                    LenDatHang = table.Column<bool>(type: "bit", nullable: false),
                    IdNguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNguoiNhan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChiMuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeHoachCheBien_Inc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeHoachCheBien_Inc_AspNetUsers_IdNguoiNhan",
                        column: x => x.IdNguoiNhan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_KeHoachCheBien_Inc_AspNetUsers_IdNguoiTao",
                        column: x => x.IdNguoiTao,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_KeHoachCheBien_Inc_ChiMuc_IdChiMuc",
                        column: x => x.IdChiMuc,
                        principalTable: "ChiMuc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "KeHoachDatHang_Inc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DatHang = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 136, DateTimeKind.Local).AddTicks(2857)),
                    NgayBatDauDuKien = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 136, DateTimeKind.Local).AddTicks(3347)),
                    NgayKetThucDuKien = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 136, DateTimeKind.Local).AddTicks(3663)),
                    GhiChu = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    NhanKeHoach = table.Column<bool>(type: "bit", nullable: false),
                    IdNguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNguoiNhan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChiMuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeHoachDatHang_Inc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeHoachDatHang_Inc_AspNetUsers_IdNguoiNhan",
                        column: x => x.IdNguoiNhan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_KeHoachDatHang_Inc_AspNetUsers_IdNguoiTao",
                        column: x => x.IdNguoiTao,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_KeHoachDatHang_Inc_ChiMuc_IdChiMuc",
                        column: x => x.IdChiMuc,
                        principalTable: "ChiMuc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "NgayNghiTuChon_Inc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayNghi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdChiMuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NgayNghiTuChon_Inc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NgayNghiTuChon_Inc_ChiMuc_IdChiMuc",
                        column: x => x.IdChiMuc,
                        principalTable: "ChiMuc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ThongBao_Inc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuongDan = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Xem = table.Column<bool>(type: "bit", nullable: false),
                    NgayNhan = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 166, DateTimeKind.Local).AddTicks(2247)),
                    LoaiThongBao = table.Column<int>(type: "int", nullable: false),
                    MaKeHoach = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IdNguoiNhan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNguoiGui = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChiMuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBao_Inc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongBao_Inc_AspNetUsers_IdNguoiGui",
                        column: x => x.IdNguoiGui,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ThongBao_Inc_AspNetUsers_IdNguoiNhan",
                        column: x => x.IdNguoiNhan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ThongBao_Inc_ChiMuc_IdChiMuc",
                        column: x => x.IdChiMuc,
                        principalTable: "ChiMuc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "NguyenVatLieu_Std",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AI"),
                    SoLuong = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    MucTonThapNhat = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    MucTonCaoNhat = table.Column<long>(type: "bigint", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NhacNho = table.Column<bool>(type: "bit", nullable: false),
                    IdLoaiNVL = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguyenVatLieu_Std", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NguyenVatLieu_Std_LoaiNguyenVatLieu_Std_IdLoaiNVL",
                        column: x => x.IdLoaiNVL,
                        principalTable: "LoaiNguyenVatLieu_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSanPham_Std",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AI"),
                    IdNhomSanPham = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiSanPham_Std", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoaiSanPham_Std_NhomSanPham_Std_IdNhomSanPham",
                        column: x => x.IdNhomSanPham,
                        principalTable: "NhomSanPham_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PhieuCheBien_Inc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdKeHoach = table.Column<long>(type: "bigint", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 174, DateTimeKind.Local).AddTicks(5450)),
                    NgayHoanThanh = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 174, DateTimeKind.Local).AddTicks(6013)),
                    TrangThaiPhieu = table.Column<int>(type: "int", nullable: false),
                    IdNguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNguoiDuyet = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdChiMuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuCheBien_Inc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhieuCheBien_Inc_AspNetUsers_IdNguoiDuyet",
                        column: x => x.IdNguoiDuyet,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhieuCheBien_Inc_AspNetUsers_IdNguoiTao",
                        column: x => x.IdNguoiTao,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PhieuCheBien_Inc_ChiMuc_IdChiMuc",
                        column: x => x.IdChiMuc,
                        principalTable: "ChiMuc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PhieuCheBien_Inc_KeHoachCheBien_Inc_IdKeHoach",
                        column: x => x.IdKeHoach,
                        principalTable: "KeHoachCheBien_Inc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon_Inc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoHoaDon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaLuuTru = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NgayMua = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 118, DateTimeKind.Local).AddTicks(1142)),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 8, 20, 8, 8, 23, 116, DateTimeKind.Local).AddTicks(6397)),
                    ThueSuat = table.Column<int>(type: "int", nullable: false),
                    ThanhToanHoaDon = table.Column<int>(type: "int", nullable: false),
                    SoTienDaTra = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IdNCC = table.Column<int>(type: "int", nullable: false),
                    IdKeHoach = table.Column<long>(type: "bigint", nullable: false),
                    IdNguoiTao = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChiMuc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon_Inc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoaDon_Inc_AspNetUsers_IdNguoiTao",
                        column: x => x.IdNguoiTao,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HoaDon_Inc_ChiMuc_IdChiMuc",
                        column: x => x.IdChiMuc,
                        principalTable: "ChiMuc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HoaDon_Inc_KeHoachDatHang_Inc_IdKeHoach",
                        column: x => x.IdKeHoach,
                        principalTable: "KeHoachDatHang_Inc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_HoaDon_Inc_NhaCungCap_Std_IdNCC",
                        column: x => x.IdNCC,
                        principalTable: "NhaCungCap_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDatHang_Inc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoLuong = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SoLuongDaThem = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DonGiaGoiY = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThaiChiTiet = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IdNhaCungCap = table.Column<int>(type: "int", nullable: true),
                    IdKeHoachDatHang = table.Column<long>(type: "bigint", nullable: false),
                    IdNguyenVatLieu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDatHang_Inc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietDatHang_Inc_KeHoachDatHang_Inc_IdKeHoachDatHang",
                        column: x => x.IdKeHoachDatHang,
                        principalTable: "KeHoachDatHang_Inc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ChiTietDatHang_Inc_NguyenVatLieu_Std_IdNguyenVatLieu",
                        column: x => x.IdNguyenVatLieu,
                        principalTable: "NguyenVatLieu_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ChiTietDatHang_Inc_NhaCungCap_Std_IdNhaCungCap",
                        column: x => x.IdNhaCungCap,
                        principalTable: "NhaCungCap_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SanPham_Std",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "SQL_Latin1_General_CP1_CI_AI"),
                    SoLuong = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    MucTonThapNhat = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    MucTonCaoNhat = table.Column<long>(type: "bigint", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NhacNho = table.Column<bool>(type: "bit", nullable: false),
                    IdLoaiSanPham = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham_Std", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SanPham_Std_LoaiSanPham_Std_IdLoaiSanPham",
                        column: x => x.IdLoaiSanPham,
                        principalTable: "LoaiSanPham_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDon_Inc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonViTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    GiamGia = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IdNguyenVatLieu = table.Column<int>(type: "int", nullable: false),
                    IdHoaDon = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietHoaDon_Inc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_Inc_HoaDon_Inc_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "HoaDon_Inc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ChiTietHoaDon_Inc_NguyenVatLieu_Std_IdNguyenVatLieu",
                        column: x => x.IdNguyenVatLieu,
                        principalTable: "NguyenVatLieu_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CongThuc_Std",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DinhDau = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    GhiChu = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IdSanPham = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CongThuc_Std", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CongThuc_Std_SanPham_Std_IdSanPham",
                        column: x => x.IdSanPham,
                        principalTable: "SanPham_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DonViTinh_Std",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GiaTriChuyenDoi = table.Column<long>(type: "bigint", nullable: false),
                    CoBan = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LoaiDongGoi = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IdSanPham = table.Column<int>(type: "int", nullable: true),
                    IdNguyenVatLieu = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonViTinh_Std", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonViTinh_Std_NguyenVatLieu_Std_IdNguyenVatLieu",
                        column: x => x.IdNguyenVatLieu,
                        principalTable: "NguyenVatLieu_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonViTinh_Std_SanPham_Std_IdSanPham",
                        column: x => x.IdSanPham,
                        principalTable: "SanPham_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietCheBien_Inc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoLuong = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SoLuongDaThem = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrangThaiChiTiet = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IdCongThuc = table.Column<int>(type: "int", nullable: false),
                    IdKeHoachCheBien = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietCheBien_Inc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietCheBien_Inc_CongThuc_Std_IdCongThuc",
                        column: x => x.IdCongThuc,
                        principalTable: "CongThuc_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ChiTietCheBien_Inc_KeHoachCheBien_Inc_IdKeHoachCheBien",
                        column: x => x.IdKeHoachCheBien,
                        principalTable: "KeHoachCheBien_Inc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietCongThuc_Std",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoLuong = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DonViTinh = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdCongThuc = table.Column<int>(type: "int", nullable: false),
                    IdNguyenVatLieu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietCongThuc_Std", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietCongThuc_Std_CongThuc_Std_IdCongThuc",
                        column: x => x.IdCongThuc,
                        principalTable: "CongThuc_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ChiTietCongThuc_Std_NguyenVatLieu_Std_IdNguyenVatLieu",
                        column: x => x.IdNguyenVatLieu,
                        principalTable: "NguyenVatLieu_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPhieuCheBien_Inc",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoLuong = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DonViTinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCongThuc = table.Column<int>(type: "int", nullable: false),
                    IdPhieu = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietPhieuCheBien_Inc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuCheBien_Inc_CongThuc_Std_IdCongThuc",
                        column: x => x.IdCongThuc,
                        principalTable: "CongThuc_Std",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ChiTietPhieuCheBien_Inc_PhieuCheBien_Inc_IdPhieu",
                        column: x => x.IdPhieu,
                        principalTable: "PhieuCheBien_Inc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "MoTa", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("88a28f0b-99cd-4893-ab70-0189c8c7fec5"), "e5539c30-9de5-4770-ab4b-55c2500e5b89", "Vai trò Administrator Hệ Thống", "AdminHeThong", "AdminHeThong" },
                    { new Guid("c7cbdfd3-bdda-4a4a-b2ae-5475f7400f56"), "85febe73-e2dc-44ae-a36b-6b4da7553615", "Vai trò Administrator", "Admin", "Admin" },
                    { new Guid("1ed2a1cf-5d3d-471f-baf0-3b72d7161969"), "13d8c4be-c91c-4bc8-a51f-869e866b2c3a", "Vai trò Quản lý đặt hàng", "QuanLyDatHang", "QuanLyDatHang" },
                    { new Guid("b2d0f535-7053-4d6c-9c3f-28e892858683"), "13e1de8d-8a2e-4164-84bd-fdd26cbec1e9", "Vai trò Quản lý chế biến", "QuanLyCheBien", "QuanLyCheBien" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "CanCuocCongDan", "ConcurrencyStamp", "DiaChi", "Email", "EmailConfirmed", "GioiTinh", "HinhAnh", "Ho", "LoaiTaiKhoan", "LockoutEnabled", "LockoutEnd", "MaSo", "NgaySinh", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Ten", "TinhTrangLamViec", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("0275d5a7-da4a-41c3-85ed-15e53cd1b7a0"), 0, "0123456789", "d3dd4a28-2cb8-43b3-83c5-9fe99bb37403", "Cần Thơ", "khoaluan@gmail.com", true, true, null, "Lương Nhựt", 2, false, null, "AdminHeThong", new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "khoaluan@gmail.com", "adminHeThong", "AQAAAAEAACcQAAAAED5tnqCA2mE4Mv4g8AkGwsP7AJVfSMOA6DyTDsf/5Sit5BuoQt/DHSGBf1dBKJRiEQ==", null, false, "", "Nam", 0, false, "adminHeThong" },
                    { new Guid("53e27774-4d9e-47bc-a7dc-b4faa6b9e140"), 0, "0987654321", "5ed375e4-5224-41a3-9e0d-d38f540b8bdc", "Cần Thơ", "admin@gmail.com", true, true, null, "Lương Nhựt", 1, false, null, "Admin", new DateTime(2020, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "admin", "AQAAAAEAACcQAAAAEPN6KFNtErhcN7CYEJOmi+kVrZYaylPl7weUrILtH1gWpjMVQqa5LVyXRx4dCqwhww==", null, false, "", "Nam", 0, false, "admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("88a28f0b-99cd-4893-ab70-0189c8c7fec5"), new Guid("0275d5a7-da4a-41c3-85ed-15e53cd1b7a0") });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("c7cbdfd3-bdda-4a4a-b2ae-5475f7400f56"), new Guid("53e27774-4d9e-47bc-a7dc-b4faa6b9e140") });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MaSo_CanCuocCongDan",
                table: "AspNetUsers",
                columns: new[] { "MaSo", "CanCuocCongDan" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChiMuc_Ten",
                table: "ChiMuc",
                column: "Ten",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCheBien_Inc_IdCongThuc",
                table: "ChiTietCheBien_Inc",
                column: "IdCongThuc");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCheBien_Inc_IdKeHoachCheBien",
                table: "ChiTietCheBien_Inc",
                column: "IdKeHoachCheBien");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCongThuc_Std_IdCongThuc",
                table: "ChiTietCongThuc_Std",
                column: "IdCongThuc");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietCongThuc_Std_IdNguyenVatLieu",
                table: "ChiTietCongThuc_Std",
                column: "IdNguyenVatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatHang_Inc_IdKeHoachDatHang",
                table: "ChiTietDatHang_Inc",
                column: "IdKeHoachDatHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatHang_Inc_IdNguyenVatLieu",
                table: "ChiTietDatHang_Inc",
                column: "IdNguyenVatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatHang_Inc_IdNhaCungCap",
                table: "ChiTietDatHang_Inc",
                column: "IdNhaCungCap");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_Inc_IdHoaDon",
                table: "ChiTietHoaDon_Inc",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_Inc_IdNguyenVatLieu",
                table: "ChiTietHoaDon_Inc",
                column: "IdNguyenVatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuCheBien_Inc_IdCongThuc",
                table: "ChiTietPhieuCheBien_Inc",
                column: "IdCongThuc");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPhieuCheBien_Inc_IdPhieu",
                table: "ChiTietPhieuCheBien_Inc",
                column: "IdPhieu");

            migrationBuilder.CreateIndex(
                name: "IX_CongThuc_Std_IdSanPham",
                table: "CongThuc_Std",
                column: "IdSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_CongThuc_Std_MaSo_Ten",
                table: "CongThuc_Std",
                columns: new[] { "MaSo", "Ten" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonViTinh_Std_IdNguyenVatLieu",
                table: "DonViTinh_Std",
                column: "IdNguyenVatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_DonViTinh_Std_IdSanPham",
                table: "DonViTinh_Std",
                column: "IdSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_Inc_IdChiMuc",
                table: "HoaDon_Inc",
                column: "IdChiMuc");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_Inc_IdKeHoach",
                table: "HoaDon_Inc",
                column: "IdKeHoach");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_Inc_IdNCC",
                table: "HoaDon_Inc",
                column: "IdNCC");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_Inc_IdNguoiTao",
                table: "HoaDon_Inc",
                column: "IdNguoiTao");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_Inc_MaLuuTru",
                table: "HoaDon_Inc",
                column: "MaLuuTru",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeHoachCheBien_Inc_IdChiMuc",
                table: "KeHoachCheBien_Inc",
                column: "IdChiMuc");

            migrationBuilder.CreateIndex(
                name: "IX_KeHoachCheBien_Inc_IdNguoiNhan",
                table: "KeHoachCheBien_Inc",
                column: "IdNguoiNhan");

            migrationBuilder.CreateIndex(
                name: "IX_KeHoachCheBien_Inc_IdNguoiTao",
                table: "KeHoachCheBien_Inc",
                column: "IdNguoiTao");

            migrationBuilder.CreateIndex(
                name: "IX_KeHoachCheBien_Inc_MaSo",
                table: "KeHoachCheBien_Inc",
                column: "MaSo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeHoachDatHang_Inc_IdChiMuc",
                table: "KeHoachDatHang_Inc",
                column: "IdChiMuc");

            migrationBuilder.CreateIndex(
                name: "IX_KeHoachDatHang_Inc_IdNguoiNhan",
                table: "KeHoachDatHang_Inc",
                column: "IdNguoiNhan");

            migrationBuilder.CreateIndex(
                name: "IX_KeHoachDatHang_Inc_IdNguoiTao",
                table: "KeHoachDatHang_Inc",
                column: "IdNguoiTao");

            migrationBuilder.CreateIndex(
                name: "IX_KeHoachDatHang_Inc_MaSo",
                table: "KeHoachDatHang_Inc",
                column: "MaSo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoaiNguyenVatLieu_Std_MaSo_Ten",
                table: "LoaiNguyenVatLieu_Std",
                columns: new[] { "MaSo", "Ten" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoaiSanPham_Std_IdNhomSanPham",
                table: "LoaiSanPham_Std",
                column: "IdNhomSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_LoaiSanPham_Std_MaSo_Ten",
                table: "LoaiSanPham_Std",
                columns: new[] { "MaSo", "Ten" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NgayNghiTuChon_Inc_IdChiMuc",
                table: "NgayNghiTuChon_Inc",
                column: "IdChiMuc");

            migrationBuilder.CreateIndex(
                name: "IX_NguyenVatLieu_Std_IdLoaiNVL",
                table: "NguyenVatLieu_Std",
                column: "IdLoaiNVL");

            migrationBuilder.CreateIndex(
                name: "IX_NguyenVatLieu_Std_MaSo_Ten",
                table: "NguyenVatLieu_Std",
                columns: new[] { "MaSo", "Ten" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhaCungCap_Std_MaSo",
                table: "NhaCungCap_Std",
                column: "MaSo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhomSanPham_Std_MaSo_Ten",
                table: "NhomSanPham_Std",
                columns: new[] { "MaSo", "Ten" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuCheBien_Inc_IdChiMuc",
                table: "PhieuCheBien_Inc",
                column: "IdChiMuc");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuCheBien_Inc_IdKeHoach",
                table: "PhieuCheBien_Inc",
                column: "IdKeHoach");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuCheBien_Inc_IdNguoiDuyet",
                table: "PhieuCheBien_Inc",
                column: "IdNguoiDuyet");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuCheBien_Inc_IdNguoiTao",
                table: "PhieuCheBien_Inc",
                column: "IdNguoiTao");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuCheBien_Inc_MaSo",
                table: "PhieuCheBien_Inc",
                column: "MaSo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuanLyMaSo_Std_Ten",
                table: "QuanLyMaSo_Std",
                column: "Ten",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_Std_IdLoaiSanPham",
                table: "SanPham_Std",
                column: "IdLoaiSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_Std_MaSo_Ten",
                table: "SanPham_Std",
                columns: new[] { "MaSo", "Ten" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThongBao_Inc_IdChiMuc",
                table: "ThongBao_Inc",
                column: "IdChiMuc");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBao_Inc_IdNguoiGui",
                table: "ThongBao_Inc",
                column: "IdNguoiGui");

            migrationBuilder.CreateIndex(
                name: "IX_ThongBao_Inc_IdNguoiNhan",
                table: "ThongBao_Inc",
                column: "IdNguoiNhan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChiTietCheBien_Inc");

            migrationBuilder.DropTable(
                name: "ChiTietCongThuc_Std");

            migrationBuilder.DropTable(
                name: "ChiTietDatHang_Inc");

            migrationBuilder.DropTable(
                name: "ChiTietHoaDon_Inc");

            migrationBuilder.DropTable(
                name: "ChiTietPhieuCheBien_Inc");

            migrationBuilder.DropTable(
                name: "DonViTinh_Std");

            migrationBuilder.DropTable(
                name: "NgayNghiTuChon_Inc");

            migrationBuilder.DropTable(
                name: "QuanLyMaSo_Std");

            migrationBuilder.DropTable(
                name: "ThongBao_Inc");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "HoaDon_Inc");

            migrationBuilder.DropTable(
                name: "CongThuc_Std");

            migrationBuilder.DropTable(
                name: "PhieuCheBien_Inc");

            migrationBuilder.DropTable(
                name: "NguyenVatLieu_Std");

            migrationBuilder.DropTable(
                name: "KeHoachDatHang_Inc");

            migrationBuilder.DropTable(
                name: "NhaCungCap_Std");

            migrationBuilder.DropTable(
                name: "SanPham_Std");

            migrationBuilder.DropTable(
                name: "KeHoachCheBien_Inc");

            migrationBuilder.DropTable(
                name: "LoaiNguyenVatLieu_Std");

            migrationBuilder.DropTable(
                name: "LoaiSanPham_Std");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ChiMuc");

            migrationBuilder.DropTable(
                name: "NhomSanPham_Std");
        }
    }
}