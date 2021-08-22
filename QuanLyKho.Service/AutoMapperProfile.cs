using AutoMapper;
using QuanLyKho.ViewModels.User;
using QuanLyKho.Data.Entities;
using QuanLyKho.ViewModels.LoaiNguyenVatLieu;
using QuanLyKho.ViewModels.LoaiSanPham;
using QuanLyKho.ViewModels.NhaCungCap;
using QuanLyKho.ViewModels.NhomSanPham;

namespace QuanLyKho.Service.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //User
            CreateMap<RegisterRequest, AppUser>();
            CreateMap<UserUpdateRequest, AppUser>();
            CreateMap<UpdateInfor, AppUser>();
            CreateMap<AppUser, UserVm>();
            CreateMap<AppUser, UserNameVm>();
            CreateMap<AppUser, GetByIdListUser>();

            //Nha Cung Cấp
            CreateMap<TaoNhaCungCap, NhaCungCap>();
            CreateMap<CapNhatNhaCungCap, NhaCungCap>();
            CreateMap<NhaCungCap, NhaCungCapModel>();

            //Loại Nguyên Vật Liệu
            CreateMap<TaoLoaiNguyenVatLieu, LoaiNguyenVatLieu>();
            CreateMap<CapNhatLoaiNguyenVatLieu, LoaiNguyenVatLieu>();

            //Nhóm Sản Phẩm
            CreateMap<TaoNhomSanPham, NhomSanPham>();
            CreateMap<CapNhatNhomSanPham, NhomSanPham>();
            CreateMap<NhomSanPham, NhomSanPhamModel>();

            //Loại Sản Phẩm
            CreateMap<TaoLoaiSanPham, LoaiSanPham>();
            CreateMap<CapNhatLoaiSanPham, LoaiSanPham>();
        }
    }
}