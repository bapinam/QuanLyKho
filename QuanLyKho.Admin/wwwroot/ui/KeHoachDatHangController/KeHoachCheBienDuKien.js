////$(function () {
////    $('#tableKHCBDK').DataTable({
////        "paging": false,
////        "lengthChange": false,
////        "searching": false,
////        "ordering": true,
////        "info": true,
////        "autoWidth": false,
////        "responsive": true,
////    });
////});

KeHoachCheBienDuKien();
function KeHoachCheBienDuKien() {
    var url = "/KeHoachDatHangs/KeHoachCheBienDuKien/";
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                var str = '<tr>'
                    + '<td>' + item.maSo + '</td>'
                    + '<td>' + item.ten + '</td>'
                    + '<td>' + item.ngayTao + '</td>'
                    + '<td>' + item.nguoiTao + '</td>'
                    + '<td class="text-center py-0 align-middle">'
                    + '<input type="hidden" value="0" id="idCheBien-' + item.id + '">'
                    + '<button type="button" id="chebienDKDaDu-' + item.id + '" class="btn btn-primary btn-sm">Đã đủ hàng</button>'
                    + '<button type="button" id="chebienDKCDhang-' + item.id + '" class="btn btn-danger btn-sm">Chưa đủ hàng</button>'
                    + '<button type="button" id="chebienDaDatHang-' + item.id + '" class="btn btn-success btn-sm">Đã lên kế hoạch</button>'
                    + '</td>'
                    + '<td class="text-center py-0 align-middle">'
                    + '<a class="btn btn-success btn-sm" data-toggle="modal" data-target="#exampleView"'
                    + 'onclick = "GetViewCheBienDk(' + item.id + ')" title = "Xem" >'
                    + '  <i class="fas fa-eye"></i>'
                    + ' </a >'
                    + '</td>'
                    + '</tr>';
                $("#CheBienDK").append(str);

                GetTrangThaiKeHoach(item.id);
            });
        },
        error: function (req, status, error) {
        }
    });
}
function GetViewCheBienDk(id) {
    $("#IdKeHoachCheBienLuu").val(id);
    var str = "#idCheBien-" + id;
    var status = $(str).val();
    if (status == 1) {
        GetThongTinKeHoachCheBienXem(id);
    } else {
        GetDanhSachNguyenVatLieuCB(id);
    }
}

function GetThongTinKeHoachCheBienXem(id) {
    $("#exampleViewKHDuHang").modal("show");
    var url = "/KeHoachCheBiens/GetInfoKeHoachById/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            $("#idKeHoachXem").val(id);
            var ngayDK = msg.resultObj.ngayDuKienBatDau + ' - ' + msg.resultObj.ngayDuKienKetThuc;
            document.getElementById('codeViewKHCB').innerHTML = msg.resultObj.maSo;
            document.getElementById('nameViewKHCB').innerHTML = msg.resultObj.ten;
            document.getElementById('createDateViewKHCB').innerHTML = msg.resultObj.ngayTao;
            document.getElementById('dateDKViewKHCB').innerHTML = ngayDK;
            document.getElementById('nguoitaoViewKHCB').innerHTML = msg.resultObj.nguoiTao;
            document.getElementById('nguoinhanViewKHCB').innerHTML = msg.resultObj.nguoiNhan;
            document.getElementById('trangthaiNhanViewKHCB').innerHTML = msg.resultObj.nhanKeHoach;
            document.getElementById('trangthaiViewKHCB').innerHTML = msg.resultObj.trangThai;
            document.getElementById('ghichuViewKHCB').innerHTML = msg.resultObj.ghiChu;
            $(".xemdANHsACHsPCB").remove();
            msg.resultObj.danhSachCheBiens.forEach(function (item) {
                var ghiChu = "";
                if (item.ghiChu == null) {
                    ghiChu = "";
                } else {
                    ghiChu = item.ghiChu;
                }
                var str = '<tr class="xemdANHsACHsPCB">'
                    + '<td>' + item.maSoSanPham + '</td>'
                    + '<td>' + item.tenSanPham + '</td>'
                    + '<td>' + item.maSoCongThuc + '</td>'
                    + '<td>' + item.soLuong + '</td>'
                    + '<td>' + item.donViTinh + '</td>'
                    + '<td>' + item.trangThai + '</td>'
                    + '<td>' + ghiChu + '</td>'
                    + '</tr>';
                $("#ShowViewKH12CB").append(str);
            });
        },
        error: function (req, status, error) {
        }
    });
}
function GetTrangThaiKeHoach(id) {
    var url = "/KeHoachDatHangs/GetTrangThaiCheBien/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) { //chebienDaDatHang
            var chuaDu = "chebienDKCDhang-" + id;
            var daDu = "chebienDKDaDu-" + id;
            var datHang = "chebienDaDatHang-" + id;
            if (msg.isSuccessed) {
                document.getElementById(chuaDu).style.display = "none";
                document.getElementById(daDu).style.display = "block";
                document.getElementById(datHang).style.display = "none";
                var str = "#idCheBien-" + id;
                $(str).val(1);
            } else {
                if (msg.message == "Chưa đủ hàng") {
                    document.getElementById(daDu).style.display = "none";
                    document.getElementById(chuaDu).style.display = "block";
                    document.getElementById(datHang).style.display = "none";

                    var str = "#idCheBien-" + id;
                    $(str).val(0);
                } else {
                    document.getElementById(daDu).style.display = "none";
                    document.getElementById(chuaDu).style.display = "none";
                    document.getElementById(datHang).style.display = "block";

                    var str = "#idCheBien-" + id;
                    $(str).val(1);
                }
            }
        },
        error: function (req, status, error) {
        }
    });
}

function GetDanhSachNguyenVatLieuCB(id) {
    DeleteTableCrateNVL();
    $("#modalThemKHDHChuaDu").modal("show");
    var url = "/CheBiens/GetCongThucKeHoach";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                var max = item.soLuong - item.soLuongDaThem;
                if (max != 0) {
                    var str = '<thead>'
                        + '<tr><th colspan="8" class="text-center">Mã Công Thức: ' + item.maCongThuc
                        + '<br />Mã Sản Phẩm: ' + item.maSanPham
                        + '<br />Tên Sản Phẩm: ' + item.tenSanPham
                        + '<br />Số Lượng Sản Phẩm: ' + max
                        + '</th > <tr>'
                        + '    <tr>'
                        + '           <th>Mã Số</th>'
                        + '        <th>Tên</th>'
                        + '            <th>ĐVT</th>'
                        + '            <th>Số Lượng Cần</th>'
                        + '            <th>Số Lượng Tồn</th>'
                        + '            <th>Số Lượng Nhập</th>'
                        + '            <th>Số Lượng Đặt</th>'
                        + '            <th>ĐVT Đặt</th>'
                        + '        </tr>'
                        + '    </thead>'
                        + '   <tbody id="tableAddNVLPP">';
                    var stt = 1;
                    item.danhSachNguyenVatLieus.forEach(function (i) {
                        var amountNew = i.soLuongHienCoNVL / i.giaTriChuyenDoi;
                        var soLuongKiemTra = i.soLuongCanCT * max;

                        str = str + ' <tr>'
                            + '<td>' + i.maNguyenVatLieu + '</td>'
                            + ' <td>' + i.tenNguyenVatLieu + '</td>'
                            + '<td id="DVTThemCB-' + i.idNguyenVatLieu + '-' + item.idChiTiet + '">' + i.donViTinh + '</td>'
                            + '<td id="SLCT-' + item.idChiTiet + '-' + stt + '">' + i.soLuongCanCT + '</td>'
                            + '<td id="SLNVL-' + item.idChiTiet + '-' + stt + '">' + amountNew + '</td>'
                            + '<input id="Value-' + i.idNguyenVatLieu + '-' + item.idChiTiet + '" type="hidden" value="' + i.giaTriChuyenDoi + '" />';

                        if (soLuongKiemTra > amountNew) {
                            var soLuongSeDat = soLuongKiemTra - amountNew;
                            str = str
                                + '<td id="SLDH-' + item.idChiTiet + '-' + stt + '">'
                                + ' <input type="number" min="' + soLuongSeDat + '" class="form-control" oninput="CapNhatSLDVT(' + i.idNguyenVatLieu + ',' + item.idChiTiet + ')"  id="SoLuongNhap-' + i.idNguyenVatLieu + '-' + item.idChiTiet + '" placeholder="Số lượng" required>'
                                + '<small>Tối thiểu: ' + soLuongSeDat + ' ' + i.donViTinh + '</small></td>'
                                + '<td id="SLDH-' + item.idChiTiet + '-' + stt + '">'
                                + ' <input type="text" min="0" name="SoLuong[]" class="form-control" value="0"  id="SoLuongShow-' + i.idNguyenVatLieu + '-' + item.idChiTiet + '" disabled>' + '</td>'
                                + ' <input type="hidden" min="0" name="SoLuong[]" id="SoLuongThem-' + i.idNguyenVatLieu + '-' + item.idChiTiet + '">'
                                + ' <input type="hidden" name="IdNguyenVatLieu[]" value="' + i.idNguyenVatLieu + '">'
                                + '</td>'
                                + '<td id="SLDVT-' + item.idChiTiet + '-' + stt + '">'
                                + ' <input type="hidden" value="' + i.donViTinh + '" name="DonViTinh[]" id="DonViLuu-' + i.idNguyenVatLieu + '-' + item.idChiTiet + '">'
                                + '<select id="DonViTinhThemEditLuu-' + i.idNguyenVatLieu + '-' + item.idChiTiet + '" onchange="ChonDonViTinh(' + i.idNguyenVatLieu + ',' + item.idChiTiet +')" class="form-control selectMater" required>'
                                + '<option value="' + i.giaTriChuyenDoi + '-' + i.donViTinh + '">' + i.donViTinh + '</option>'
                                + '</select>'
                                + '</td> ';
                            TimDonViTinhKHUpdateLuu(i.idNguyenVatLieu, item.idChiTiet, '' + i.donViTinh + '');
                        } else {
                            str = str + '<td colspan="3"  style="color: blue;">Đã đủ hàng</td> ';
                        }

                        str = str + '</tr>';
                        stt++;
                    });

                    str = str + ' </tbody>';
                    $("#DeleteTableNVLcc").append(str);
                }
            });
        },
        error: function (req, status, error) {
        }
    });
}
function TimDonViTinhKHUpdateLuu(id, idChiTiet, donViTinh) {
    var str = "#DonViTinhThemEditLuu-" + id + '-' + idChiTiet;
    var url = "/KeHoachDatHangs/GetDonViTinhKHDH/";
    $.ajax({
        type: "GET",
        url: url,
        data: { idNVL: id },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                if (item.ten != donViTinh) {
                    var chuoi12 = '<option value="' + item.giaTriChuyenDoi + '-' + item.ten + '">'
                        + item.ten + '</option> ';
                    $(str).append(chuoi12);
                }
            });
        },
        error: function (req, status, error) {
        }
    });
}

function DeleteTableCrateNVL() {
    $("#DeleteTableNVLcc").remove();
    $("#tableRow").append('<table id="DeleteTableNVLcc" class="table table-bordered table-striped table-hover table-responsive-xl">'
        + '</table> ');
}
function ChonDonViTinh(id, idChiTiet) {
    CapNhatSLDVT(id, idChiTiet);
    var str1 = "#DonViLuu-" + id + '-' + idChiTiet;
    var tenSelect = "#DonViTinhThemEditLuu-" + id + '-' + idChiTiet;
    var chuyenDoi = $(tenSelect).val();

    var splitted = chuyenDoi.split("-");
    var tenDoi = splitted[1];
    $(str1).val(tenDoi);
}
function CapNhatSLDVT(id, idChiTiet) {
    var tenSelect = "#DonViTinhThemEditLuu-" + id + '-' + idChiTiet;
    var chuyenDoi = $(tenSelect).val();

    var splitted = chuyenDoi.split("-");
    var giaTri = splitted[0];
    var tenDoi = splitted[1];
    var str = "#DVTThemCB-" + id + '-' + idChiTiet;
    var tenGoc = $(str).text();

    var str1 = "#SoLuongNhap-" + id + '-' + idChiTiet;
    var soLuongNhap = $(str1).val();
    var str2An = "#SoLuongThem-" + id + '-' + idChiTiet;
    var str2Hien = "#SoLuongShow-" + id + '-' + idChiTiet;

    if (tenDoi == tenGoc) {
        $(str2An).val(soLuongNhap);
        $(str2Hien).val(soLuongNhap);
    } else {
        var gt = "#Value-" + id + '-' + idChiTiet;
        var giaTriCDGoc = $(gt).val();

        var soLuongCoBan = giaTriCDGoc * soLuongNhap;
        //(a - a%b)/b
        var soLuongMoi = 0;
        if (soLuongCoBan % giaTri == 0) {
            soLuongMoi = soLuongCoBan / giaTri;
        } else {
            soLuongMoi = (soLuongCoBan - soLuongCoBan % giaTri) / giaTri + 1;
        }

        $(str2An).val(soLuongMoi);
        $(str2Hien).val(soLuongMoi);
    }
}