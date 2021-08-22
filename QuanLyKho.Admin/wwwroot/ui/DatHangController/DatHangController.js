GetAllKeHoachDatHangChuaHoanThanh(" ", " ")
function GetAllKeHoachDatHangChuaHoanThanh(ngay, tukhoa) {
    var bundle =
    {
        Ngay: ngay,
        TuKhoa: tukhoa
    };
    $(".delete-kehoach").remove(); $.ajax({
        type: "GET",
        url: "/DatHangs/GetAllKeHoachDatHangChuaHoanThanh",
        data: bundle,
        success: function (msg) {
            var i = 0;
            msg.forEach(function (item) {
                var str = ' <tr class="delete-kehoach" id="keHoachDE-' + item.id + '">'
                    + '<td>' + item.maSo + '</td>'
                    + '<td>' + item.ten + '</td>'
                    + '<td>' + item.ngayTao + '</td>'
                    + '<td>' + item.nguoiTao + '</td>'
                    + '<td>' + item.nguoiNhan + '</td>'
                    + '<td>' + item.trangThai + '</td>'
                    + '<td class="text-center">'
                    + '<div class="btn-group btn-group-sm">'
                    + '          <a class="btn btn-primary" data-toggle="modal" data-target="#modalThem"'
                    + '              onclick="NhapHoaDon(' + item.id + ')" title="Nhập hóa đơn">'
                    + '               <i class="fas fa-cart-plus"></i>'
                    + '           </a>'
                    + '            <a class="btn btn-danger" data-toggle="modal" data-target="#HuyKeHoachDatHang"'
                    + '               onclick="HuyKeHoachDatHang(' + item.id + ')" title="Hủy Kế Hoạch">'
                    + '               <i class="fas fa-ban"></i>'
                    + '            </a>'
                    + '        </div>'
                    + '    </td>'
                    + '</tr>';
                $("#phanPhoiTableShow").append(str);
                i++;
                document.getElementById("SoHDCTT").innerHTML = i;
            });
            if (i == 0) {
                document.getElementById("SoHDCTT").innerHTML = "0";
                var str = ' <tr class="delete-kehoach text-center">'
                    + '<td colspan="7" >Không tìm thấy dữ liệu</td>'
                    + '</tr>';
                $("#phanPhoiTableShow").append(str);
            }
        },
        error: function (data) {
        }
    });
}

var frmTKKH = $("#frmTKKH");
frmTKKH.submit(function (e) {
    $(".delete-kehoach").remove();
    var ngay = $("#NgayTKKH").val();
    var tukhoa = $("#tuKhoaTKKH").val();
    GetAllKeHoachDatHangChuaHoanThanh(ngay, tukhoa);
    e.preventDefault();
});
// ngày tìm kiếm kế hoạch
resetLaiNgayTKKH();
function resetLaiNgayTKKH() {
    $(function () {
        $('#NgayTKKH').daterangepicker({
            opens: 'left',
            defaultDate: '',
            minDate: 0,
        }, function (start, end, label) {
            console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
        }).val('');
    });
    $('#NgayTKKH').val('');
}

// tạo phiếu chế biến ----------------------------------------------------------------------------------------

var arrItem = [];
function NhapHoaDon(id) {
    DeleteTableBill();
    $("#IdOrderPlan").val(id);
    var url = "/DatHangs/GetDanhSachDatHang";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            arrItem = [];
            msg.forEach(function (item) {
                var max = item.soLuong - item.soLuongDaThem;

                if (max != 0) {
                    arrItem.push(item.idChiTiet);

                    var str = '<tr>'
                        + '<td>'
                        + '<input value="' + item.idChiTiet + '" type="hidden"  name="IdChiTiet[]" />'
                        + '<input value="' + item.idNguyenVatLieu + '" type="hidden"  name="IdNguyenVatLieu[]" />'
                        + '<input value="0" type="hidden"  name="TongTien" id="tongTienHoaDon"/> '
                        + item.maSo + ' </td>'
                        + ' <td>' + item.ten + '</td>'
                        + ' <td>'
                        + '<input value="' + item.donViTinh + '" type="hidden"  name="DonViTinh[]" />'
                        + item.donViTinh + '</td>'
                        + ' <td>'
                        + '      <input class="form-control" type="hidden" value="' + item.soLuong + '"  name="SoLuong[]" />'
                        + '    <input class="form-control" type="number" min="0" max="' + max + '" id="SoLuongShow-' + id + '-' + item.idChiTiet + '"'
                        + 'onchange="SoLuong(' + id + ',' + item.idChiTiet + ')" value="0" name="SoLuongDaThem[]" />'
                        + '<small>Tối đa: ' + max + '</small> '
                        + '  </td>'
                        + '  <td>'
                        + '      <input class="form-control" type="text" id="numberPrice' + id + '-' + item.idChiTiet + '" onkeyup="price(' + id + ',' + item.idChiTiet + ')"  min="0" value="0" required />'
                        + '      <input class="form-control" type="hidden" id="numberPriceShow' + id + '-' + item.idChiTiet + '"  min="0" value="0"  name="DonGia[]" />'
                        + '   </td>'
                        + '  <td>'
                        + '       <input class="form-control" type="number" min="0" max="100" value="0" name="GiamGia[]"'
                        + 'onchange="GiamGia(' + id + ',' + item.idChiTiet + ')" id="GiamGiaShow-' + id + '-' + item.idChiTiet + '" required />'
                        + '   </td>'
                        + ' <td id="ThanhTienCreate-' + id + '-' + item.idChiTiet + '">0'
                        + '  </td>'
                        + ' </tr>';
                    $("#tableAddNVL").append(str);
                }
            });

            var str12 = '<tr>'
                + '<td colspan="5" class="text-center"><b>Cộng Tiền:  </b></td>'
                + '<td colspan="2" class="text-center" id="showTongTien">0</td>'
                + '      </tr >';
            $("#tableAddNVL").append(str12);

            var tienthue = '<tr>'
                + '<td colspan="5" class="text-center"><b>Tiền Thuế GTGT:  </b></td>'
                + '<td colspan="2" class="text-center" id="showTienThue">0</td>'
                + '      </tr >';
            $("#tableAddNVL").append(tienthue);

            var str1234 = '<tr>'
                + '<td colspan="5" class="text-center"><b>Tổng Tiền Thanh Toán:  </b></td>'
                + '<td colspan="2" class="text-center" id="showTongTienTT">0'
                + ' </td> '
                + '</tr>';
            $("#tableAddNVL").append(str1234);
        },
        error: function (req, status, error) {
        }
    });
}

function DeleteTableBill() {
    $("#tableAddNVL").remove();
    $("#table34NCC").append('<tbody id="tableAddNVL"></tbody>');
}

function isNumber(n) { return /^-?[\d.]+(?:e-?\d+)?$/.test(n); }

function price(id, idmater) {
    var txt = "numberPrice" + id + "-" + idmater;
    document.getElementById(txt).onblur = function () {
        this.value = parseFloat(this.value.replace(/,/g, ""))
            .toFixed(2)
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    var mystring = document.getElementById(txt).value;
    var kq = isNumber(mystring);
    if (!kq) {
        document.getElementById(txt).value = 0;
    }

    var pr = "numberPriceShow" + id + "-" + idmater;
    document.getElementById(pr).value = mystring;

    SoLuong(id, idmater);
}

function checkThue() {
    var mystring = document.getElementById("Thue").value;
    var kq = isNumber(mystring);
    if (!kq) {
        document.getElementById("Thue").value = "";
        document.getElementById("showTienThue").innerHTML = 0;
        mystring = 0;
    }

    var tong = document.getElementById("showTongTien").innerText;
    var tongTien = tong.split('.').join('')
    tongTien = tongTien.replace('.00', '');
    var tien = tongTien.substring(0, tongTien.length - 1);

    if (tien === "") {
        tien = 0;
    }
    var tongThue = parseInt(tien) * parseInt(mystring) / 100;
    var tthue = tongThue;
    tthue = tthue.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
    document.getElementById("showTienThue").innerHTML = tthue;

    var tongTT = parseInt(tien) + parseInt(tongThue);
    $("#tongTienHoaDon").val(tongTT);
    tongTT = tongTT.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
    document.getElementById("showTongTienTT").innerHTML = tongTT;
}
function checkSHD() {
    var mystring = document.getElementById("SoHoaDon").value;
    var kq = isNumber(mystring);
    if (!kq) {
        document.getElementById("SoHoaDon").value = "";
    }
}

function checkAmoutPaid() {
    var txt = "AmountPaidShow";
    document.getElementById(txt).onblur = function () {
        this.value = parseFloat(this.value.replace(/,/g, ""))
            .toFixed(2)
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    var mystring = document.getElementById(txt).value;
    var kq = isNumber(mystring);
    if (!kq) {
        document.getElementById(txt).value = 0;
    }

    var pr = "AmountPaid";
    document.getElementById(pr).value = mystring;
}

function SoLuong(id, idMaterials) {
    var sl = "#SoLuongShow-" + id + "-" + idMaterials;
    var gg = "#GiamGiaShow-" + id + "-" + idMaterials;
    var price = "#numberPriceShow" + id + "-" + idMaterials;
    var thanhTien = "ThanhTienCreate-" + id + "-" + idMaterials;
    var soLuong = $(sl).val();
    var giamGia = $(gg).val();
    var giaTien = $(price).val();
    if (soLuong === "") {
        soLuong = 0;
    }

    if (giamGia === "") {
        giamGia = 0;
    }

    if (giaTien === "") {
        giaTien = 0;
    }
    var kq;
    if (giamGia == 0) {
        kq = soLuong * giaTien;
    } else {
        var tongTien = soLuong * giaTien;

        var phamTram = tongTien * giamGia / 100;
        kq = tongTien - phamTram;
    }

    document.getElementById(thanhTien).innerHTML = kq;

    var thue = $("#Thue").val();

    if (thue === "") {
        thue = 0;
    }

    var tong = 0;
    arrItem.forEach(function myFunction(item) {
        var thanhTienTong12 = "ThanhTienCreate-" + id + "-" + item;
        var txt = document.getElementById(thanhTienTong12).innerText;
        tong += parseInt(txt);
    });

    var tongGoc = tong;
    tong = tong.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
    document.getElementById("showTongTien").innerHTML = tong;

    var tongThue = parseInt(tongGoc) * parseInt(thue) / 100;
    var tthue = tongThue;
    tthue = tthue.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
    document.getElementById("showTienThue").innerHTML = tthue
        ;

    var tongTT = parseInt(tongGoc) + parseInt(tongThue);
    $("#tongTienHoaDon").val(tongTT);
    tongTT = tongTT.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
    document.getElementById("showTongTienTT").innerHTML = tongTT;
}

function GiamGia(id, idMaterials) {
    SoLuong(id, idMaterials);
}

/// nhà cung cấp
function GetNhaCungCap() {
    $("#modalThem").modal("hide");
}
var frmTKNCCBill = $("#frmTKNCCBill");
frmTKNCCBill.submit(function (e) {
    var key = $("#keywordNCC").val();
    var url = "/DatHangs/NhaCungCapDatHang";
    $.ajax({
        type: "GET",
        url: url,
        data: { tuKhoa: key },
        dataType: "json",
        success: function (msg) {
            DeleteViewSupp();
            var i = 0;
            msg.forEach(function (item) {
                var str = ' <tr>'
                    + '<td>' + item.maSo + '</td>'
                    + '  <td>' + item.ten + '</td>'
                    + ' <td>' + item.diaChi + '</td>'
                    + '  <td class="text-center">'
                    + '     <div class="btn-group btn-group-sm">'
                    + '          <a class="btn btn-primary"'
                    + '             onclick="GetAddNCCBill(' + item.id + ',\'' + item.maSo + '\')" title="Thêm">'
                    + '              <i class="fas fa-plus"></i>'
                    + '          </a>'
                    + '       </div>'
                    + '     </td>'
                    + ' </tr>';
                $("#tableAddNCCBill").append(str); i++;
            });
            if (i == 0) {
                toastr.warning('Không tìm thấy kết quả');
            }
        },
        error: function (req, status, error) {
        }
    });
    e.preventDefault();
});

function DeleteViewSupp() {
    $('#tableAddNCCBill').remove();
    $("#table34NCCBill").append('<tbody id="tableAddNCCBill"><tbody>');
}

function GetAddNCCBill(id, code) {
    document.getElementById("NameNhaCungCap").innerHTML = code;
    $("#IdSupliers").val(id);
    $("#exampleNhaCungCap").modal("hide");
    $("#modalThem").modal("show");
}
// danh sách đặt hàng đang chờ trả tiền-------------------------------------
DatHangChuaTraTien(" ", " ")
function DatHangChuaTraTien(ngay, tukhoa) {
    var bundle =
    {
        Ngay: ngay,
        TuKhoa: tukhoa
    };
    $(".delete-datHangCTT").remove(); $.ajax({
        type: "GET",
        url: "/DatHangs/GetAllPhieuNhapChuaTra",
        data: bundle,
        success: function (msg) {
            var i = 0;
            msg.forEach(function (item) {
                var str = ' <tr class="delete-datHangCTT" id="hoadonchuatraDE-' + item.id + '">'
                    + '<td>' + item.maLuuTru + '</td>'
                    + '<td>' + item.maKeHoach + '</td>'
                    + '<td>' + item.ngayTao + '</td>'
                    + '<td>' + item.nguoiTao + '</td>'
                    + '<td id="trangThaicapnhat-' + item.id + '">' + item.trangThai + '</td>'
                    + '<td class="text-center">'
                    + '<div class="btn-group btn-group-sm">'
                    + '          <a class="btn btn-success" data-toggle="modal" data-target="#modalXem"'
                    + '              onclick="XemHoaDon(' + item.id + ')" title="Xem phiếu nhập hóa đơn">'
                    + '               <i class="fas fa-eye"></i>'
                    + '           </a>'
                    + '            <a class="btn btn-primary" data-toggle="modal" data-target="#exampleEditMoeny"'
                    + '               onclick="CapNhatHoaDon(' + item.id + ')" title="Cập nhật thanh toán">'
                    + '               <i class="fas fa-pencil-alt"></i>'
                    + '            </a>'
                    + '        </div>'
                    + '    </td>'
                    + '</tr>';
                $("#dathangCTTShow").append(str);
                i++;
                document.getElementById("HoaDonCTT").innerHTML = i;
            });
            if (i == 0) {
                document.getElementById("HoaDonCTT").innerHTML = "0";
                var str = ' <tr class="delete-datHangCTT text-center">'
                    + '<td colspan="6" >Không tìm thấy dữ liệu</td>'
                    + '</tr>';
                $("#dathangCTTShow").append(str);
            }
        },
        error: function (data) {
        }
    });
}

var frmHDCTT = $("#frmHDCTT");
frmHDCTT.submit(function (e) {
    $(".delete-kehoach").remove();
    var ngay = $("#NgayTKHDCT").val();
    var tukhoa = $("#tuKhoaHDCTT").val();
    DatHangChuaTraTien(ngay, tukhoa);
    e.preventDefault();
});
// ngày tìm kiếm ngày tạo phiếu
resetLaiNgayHDCT();
function resetLaiNgayHDCT() {
    $(function () {
        $('#NgayTKHDCT').daterangepicker({
            opens: 'left',
            defaultDate: '',
            minDate: 0,
        }, function (start, end, label) {
            console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
        }).val('');
    });
    $('#NgayTKHDCT').val('');
}

/// danh sách phiếu đã thành toán xong ---------------------------------
// ngày tìm kiếm ngày tạo phiếu
resetLaiNgayToanBoPhieu();
function resetLaiNgayToanBoPhieu() {
    $(function () {
        $('#NgayToanBoPhieu').daterangepicker({
            opens: 'left',
            defaultDate: '',
            minDate: 0,
        }, function (start, end, label) {
            console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
        }).val('');
    });
    $('#NgayToanBoPhieu').val('');
}
///XEM HÓA ĐƠN
function XemHoaDon(id) {
    //maLuuTruDH.maKHDH.soHoaDonDH.nguoiTaoDH.ngayTaoDH.ngayMuaDH.trangThaiDH.nhaCungCapDH
    $(".showPhieuDatHangXem").remove();
    $.ajax({
        type: "GET",
        url: "/DatHangs/GetChiTietPhieuNhap",
        data: { id: id },
        success: function (msg) {
            document.getElementById("maLuuTruDH").innerHTML = msg.resultObj.maLuuTru;
            document.getElementById("maKHDH").innerHTML = msg.resultObj.maKeHoach;
            document.getElementById("soHoaDonDH").innerHTML = msg.resultObj.soHoaDon;
            document.getElementById("nguoiTaoDH").innerHTML = msg.resultObj.nguoiTao;
            document.getElementById("ngayTaoDH").innerHTML = msg.resultObj.ngayTao;
            document.getElementById("ngayMuaDH").innerHTML = msg.resultObj.ngayMua;
            document.getElementById("trangThaiDH").innerHTML = msg.resultObj.trangThaiTraTien;
            document.getElementById("nhaCungCapDH").innerHTML = msg.resultObj.maNhaCungCap;
            var tongTien = 0;
            msg.resultObj.danhSachNguyenLieuNhapHDs.forEach(function (item) {
                var thanhTieNVL = item.thanhTien.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });

                var str = ' <tr class="showPhieuDatHangXem">'
                    + '<td>' + item.maNguyenVatLieu + '</td>'
                    + '<td>' + item.tenNguyenVatLieu + '</td>'
                    + '<td>' + item.donViTinh + '</td>'
                    + '<td>' + item.soLuong + '</td>'
                    + '<td>' + item.donGia + '</td>'
                    + '<td>' + item.giamGia + '</td>'
                    + '<td>' + thanhTieNVL + '</td>'
                    + '</tr>';
                $("#ShowViewKH12DH").append(str);
                tongTien = tongTien + item.thanhTien;
            });
            var tienThue = tongTien * msg.resultObj.thueSuat / 100;
            var chuoi = '<tr class="showPhieuDatHangXem">'
                + '<td colspan="2" class="text-center"><b>Cộng tiền bảng: </b></td>'
                + '<td colspan="5">' + tongTien + '</td>'
                + '</tr>';
            tongTienTT = tienThue.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });

            chuoi = chuoi + '<tr class="showPhieuDatHangXem">'
                + '<td colspan="2" class="text-center"><b>Thuế suất GTGT: ' + msg.resultObj.thueSuat + ' %</b></td>'
                + '<td colspan="5"><b>Tiền thuế GTGT:</b>&nbsp;&nbsp;&nbsp; ' + tienThue + '</td>'
                + '</tr>';

            var tongTienTT = msg.resultObj.tongTienThanhToan.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });

            chuoi = chuoi + '<tr class="showPhieuDatHangXem">'
                + '<td colspan="2" class="text-center"><b>Tổng cộng tiền thanh toán: </b></td>'
                + '<td colspan="5">' + tongTienTT + '</td>'
                + '</tr>';
            chuoi = chuoi + '<tr class="showPhieuDatHangXem">'
                + '<td colspan="2" class="text-center"><b>Số tiền bằng chữ: </b></td>'
                + '<td colspan="5">' + msg.resultObj.tienBangChu + '</td>'
                + '</tr>';
            $("#ShowViewKH12DH").append(chuoi);
        },
        error: function (data) {
        }
    });
}

// cập nhật tiền thanh toán hóa đơn
function CapNhatHoaDon(id) {
    $("#IdUpdateMoneyHD").val(id);
    var url = "/DatHangs/GetTienDaThanhToan";

    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            if (msg.isSuccessed) {
                var tienCanThanhToan = msg.resultObj.tienCanThanhToan.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                var tienDaTra = msg.resultObj.tienDaTra.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                $("#paymentMoenyCTT").val(tienCanThanhToan);
                $("#paymentMoeny").val(tienDaTra);
            } else {
                toastr.erro('Không tìm thấy kết quả');
            }
        }
        ,
        error: function (req, status, error) {
        }
    });
}

function priceUpdateTT() {
    var txt = "moenyNewShow";
    document.getElementById(txt).onblur = function () {
        this.value = parseFloat(this.value.replace(/,/g, ""))
            .toFixed(2)
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    var mystring = document.getElementById(txt).value;
    var kq = isNumber(mystring);
    if (!kq) {
        document.getElementById(txt).value = 0;
    }

    var pr = "moenyNew";
    document.getElementById(pr).value = mystring;
}

function UpdateTienHoaDon() {
    var url = "/DatHangs/UpdateThanhToan";
    var id = $("#IdUpdateMoneyHD").val();
    var tienDaTra = $("#moenyNew").val();
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id, tienDaTra: tienDaTra },
        dataType: "json",
        success: function (msg) {
            if (msg.isSuccessed) {
                if (msg.resultObj.trangThai == "Đã thanh toán") {
                    var str = "#hoadonchuatraDE-" + id;
                    $(str).remove();
                    var str = ' <tr ' + id + '">'
                        + ' <td>' + msg.resultObj.maLuuTru + '</td>'
                        + ' <td>' + msg.resultObj.maKeHoach + '</td>'
                        + ' <td>' + msg.resultObj.ngayTao + '</td>'
                        + ' <td>' + msg.resultObj.nguoiTao + '</td>'
                        + ' <td>' + msg.resultObj.trangThai + '</td>'
                        + '  <td class="text-center">'
                        + '<div class="btn-group btn-group-sm">'
                        +'<a class="btn btn-success" data-toggle="modal" data-target="#modalXem"'
                         + 'onclick = "XemHoaDon('+id+')" title = "Xem phiếu nhập hóa đơn">'
                       +' <i class="fas fa-eye"></i>'
                       +'           </a>'
                        + '     </div>'
                        + '    </td>'
                        + ' </tr >';

                    $("#XemThongTinPCBTB1").prepend(str);
                } else {
                    var chuoi = "trangThaicapnhat-" + id;
                    document.getElementById(chuoi).innerHTML = msg.resultObj.trangThai;
                }
                $("#moenyNewShow").val('');
                $("#moenyNew").val('');
                toastr.success('Cập nhật thành công');
                $("#exampleEditMoeny").modal("hide");
            } else {
                toastr.erro('Cập nhật thất bại');
            }
        }
        ,
        error: function (req, status, error) {
            toastr.erro('Cập nhật thất bại');
        }
    });
}
/*_____________________________________-*/
function HuyKeHoachDatHang(id) {
    $(".HuyKHDatHangShow").remove();
    var url = "/DatHangs/GetDanhSachDatHang";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                var str = '<tr class="HuyKHDatHangShow">'
                    + '<td>' + item.maSo + '</td>'
                    + '<td>' + item.ten + '</td>'
                    + '<td>' + item.soLuong + '</td>'
                    + '<td>' + item.soLuongDaThem + '</td>'
                    + '<td>' + item.donViTinh + '</td>'
                    + '<td class="text-center">'
                    + '<div class="btn-group btn-group-sm">'
                    + '          <a class="btn btn-danger" data-toggle="modal" data-target="#modalHoiHuyDatHang" id="HuyDHNone-' + item.idChiTiet + '"'
                    + '              onclick="HuyChitietDH(' + item.idChiTiet + ','+id+')" title="Hủy Chế Biến">'
                    + '               Hủy'
                    + '           </a>'
                    + '            <a class="btn btn-primary" href="#" style="display: none;" id="HuyDH-' + item.idChiTiet + '" title = "Đã Hủy" > '
                    + '              Đã Hủy'
                    + '            </a>'
                    + '        </div>'
                    + '</td>'
                    + ' </tr >';
                $("#ShowViewHuyKeHoachDatHang").append(str);
            });
        },
        error: function (req, status, error) {
        }
    });

}
function HuyChitietDH(idChiTiet,id) {
    $("#idHoiHuyDatHang").val(idChiTiet);
    $("#idKeHoachDHHuy").val(id);

}

function ajaxHuyDatHang() {
    var id = $("#idHoiHuyDatHang").val();
    var url = "/DatHangs/HuyChiTietDatHang";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            toastr.success("Hủy thành công");
            if (msg.isSuccessed) {
                $("#modalHoiHuyDatHang").modal("hide");
                var str = "HuyDH-" + id;
                document.getElementById(str).style.display = "block";

                var strNone = "HuyDHNone-" + id;
                document.getElementById(strNone).style.display = "none";

                if (msg.resultObj == "true") {
                   var idKH= $("#idKeHoachDHHuy").val();

                    var de = '#keHoachDE-' + idKH;
                    $(de).remove();
                    $("#HuyKeHoachDatHang").modal("hide");

                    var text = document.getElementById("SoHDCTT").innerText;
                    var soLuong = text - 1;
                    document.getElementById("SoHDCTT").innerHTML = soLuong; 
                }
            } else {
                toastr.error(msg.message);
            }
        },
        error: function (req, status, error) {
            toastr.error("Hủy thất bại");
        }
    });
}