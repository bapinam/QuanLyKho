ThongTinPhieuCheBienChoNhapSanPham(" ", " ");
function ThongTinPhieuCheBienChoNhapSanPham(ngay, tukhoa) {
    var bundle =
    {
        Ngay: ngay,
        TuKhoa: tukhoa
    };
    $(".delete-phieuCho").remove();
    $.ajax({
        type: "GET",
        url: "/CheBiens/GetPhieuCheBienDangCho",
        data: bundle,
        success: function (msg) {
            var i = 0;
            msg.forEach(function (item) {
                var str = ' <tr class="delete-phieuCho" id="phieuChoDE-' + item.id + '">'
                    + '<td>' + item.maSo + '</td>'
                    + '<td>' + item.maKeHoach + '</td>'
                    + '<td>' + item.ngayTao + '</td>'
                    + '<td>' + item.nguoiTao + '</td>'
                    + '<td>' + item.nguoiNhan + '</td>'
                    + '<td class="text-center">'
                    + '<div class="btn-group btn-group-sm">'
                    + '          <a class="btn btn-primary" data-toggle="modal" data-target="#modalCapNhat"'
                    + '              onclick="CapNhatPhieuChoCb(' + item.id + ',\'' + item.maSo + '\')" title="Phiếu chế biến hoàn thành">'
                    + '               <i class="fas fa-check"></i>'
                    + '           </a>'
                    + '           <a class="btn btn-success" data-toggle="modal" data-target="#xemthongtinPhIEUcb"'
                    + '               onclick="XemPhieuCheBien(' + item.id + ')" title="Xem phiếu chế biến">'
                    + '                <i class="fas fa-eye"></i>'
                    + '            </a>'
                    + '            <a class="btn btn-danger" data-toggle="modal" data-target="#modalHuyPhieu"'
                    + '               onclick="HuyPhieuCheBien(' + item.id + ',\'' + item.maSo + '\')" title="Hủy Phiếu Chế Biến">'
                    + '               <i class="fas fa-ban"></i>'
                    + '            </a>'
                    + '        </div>'
                    + '    </td>'
                    + '</tr>';
                $("#phieuChoCB").append(str);
                i++;
                document.getElementById("PhieuCountCho").innerHTML = i;
            });
            if (i == 0) {
                document.getElementById("PhieuCountCho").innerHTML = "0";
                var str = ' <tr class="delete-phieuCho">'
                    + '<td colspan="6" >Không tìm thấy dữ liệu</td>'
                    + '</tr>';
                $("#phieuChoCB").append(str);
            }
        },
        error: function (data) {
        }
    });
}

function CapNhatPhieuChoCb(id, maSo) {
    document.getElementById("exampleModalLabelCB").innerHTML = "Bạn có muốn đánh dấu hoàn thành phiếu chế biến " + maSo + " ?";
    $("#idPhieuCheBienDanhDau").val(id);
}

function ajaxHoanThanhPhieuCheBien() {
    var id = $("#idPhieuCheBienDanhDau").val();
    var userName = $("#idNguoiDuyetPhieu").val();
    $.ajax({
        type: "GET",
        url: "/CheBiens/CapNhatTrangThaiCheBien",
        data: { id: id, userName: userName },
        success: function (msg) {
            if (msg.isSuccessed) {
                toastr.success("Cập nhật thành công");
                $("#modalCapNhat").modal("hide");
                var str = "#phieuChoDE-" + id;
                $(str).remove();
                var text = document.getElementById("PhieuCountCho").innerText;
                var soLuong = text - 1;
                document.getElementById("PhieuCountCho").innerHTML = soLuong;

                var strChuoi = '<tr>'
                    + ' <td>' + msg.resultObj.maPhieu
                    + '      </td >'
                    + '           <td>' + msg.resultObj.maKeHoach
                    + '    </td>'
                    + '       <td>' + msg.resultObj.ngayTao
                    + '       </td>'
                    + '        <td>' + msg.resultObj.nguoiTao
                    + '        </td>'
                    + '        <td>' + msg.resultObj.nguoiNhan
                    + '        </td>'
                    + '       <td>' + msg.resultObj.trangThai
                    + '        </td>'
                    + '        <td class="text-center py-0 align-middle">'
                    + '            <div class="btn-group btn-group-sm">'
                    + '                <a class="btn btn-success" data-toggle="modal" data-target="#xemthongtinPhIEUcb"'
                    + '                   onclick="XemPhieuCheBien(' + msg.resultObj.id + ')" title="Xem phiếu chế biến">'
                    + '                    <i class="fas fa-eye"></i>'
                    + '              </a>'
                    + '           </div>'
                    + '       </td>'
                    + '    </tr >';

                $("#XemThongTinPCBTB1").prepend(strChuoi);

                // gửi thông điệp
                var user = msg.resultObj.userName;
                var message =
                {
                    Id: msg.resultObj.idThongBao,
                    Ten: "Cập nhật phiếu chế biến: " + msg.resultObj.maPhieu,
                    DuongDan: "/NhiemVuPhieuCheBien/Index",
                };
                connectionPhieuCheBien.invoke("SendPhieuCheBien", user, message).catch(function (err) {
                    return console.error(err.toString());
                });
            } else {
                toastr.error(msg.message);
            }
        },
        error: function (data) {
            toastr.error("Cập nhật thất bại");
        }
    });
}

function XemPhieuCheBien(id) {
    //maPhieuCB .maKHCB.ngtaoCB.ngayTaoCB.nguoiNhanCB.nguoiCapNhatCB
    $(".showPhieuCheBienXem").remove();
    $.ajax({
        type: "GET",
        url: "/CheBiens/GetIdInforCheBien",
        data: { id: id },
        success: function (msg) {
            document.getElementById("maPhieuCB").innerHTML = msg.resultObj.maSo;
            document.getElementById("maKHCB").innerHTML = msg.resultObj.maKeHoach;
            document.getElementById("ngtaoCB").innerHTML = msg.resultObj.nguoiTao;
            document.getElementById("ngayTaoCB").innerHTML = msg.resultObj.ngayTao;
            document.getElementById("nguoiNhanCB").innerHTML = msg.resultObj.nguoiNhan;
            document.getElementById("nguoiCapNhatCB").innerHTML = msg.resultObj.nguoiDuyet;
            document.getElementById("ngayHoanThanhCB").innerHTML = msg.resultObj.ngayHoanThanh;

            msg.resultObj.danhSachPhieuChiTietCheBiens.forEach(function (item) {
                var str = ' <tr class="showPhieuCheBienXem">'
                    + '<td>' + item.maSanPham + '</td>'
                    + '<td>' + item.tenSanPham + '</td>'
                    + '<td>' + item.maCongThuc + '</td>'
                    + '<td>' + item.soLuong + '</td>'
                    + '<td>' + item.donViTinh + '</td>'
                    + '</tr>';
                $("#ShowViewKH12CB").append(str);
            });
        },
        error: function (data) {
        }
    });
}

// hủy phiếu chế biến
function HuyPhieuCheBien(id, maSo) {
    document.getElementById("exampleModalLabelCBHuy").innerHTML = "Bạn có muốn hủy phiếu chế biến " + maSo + " ?";
    $("#idPhieuCheBienHuy").val(id);
}

function ajaxHuyPhieu() {
    var id = $("#idPhieuCheBienHuy").val();
    var nguoiDuyet = $("#idNguoiDuyetPhieuHuy").val();

    $.ajax({
        type: "GET",
        url: "/CheBiens/HuyPhieuCheBien",
        data: { id: id, nguoiDuyet: nguoiDuyet },
        success: function (msg) {
            if (msg.isSuccessed) {
                toastr.success("Hủy thành công");
                $("#modalHuyPhieu").modal("hide");
                var str = "#phieuChoDE-" + id;
                $(str).remove();
                var text = document.getElementById("PhieuCountCho").innerText;
                var soLuong = text - 1;
                document.getElementById("PhieuCountCho").innerHTML = soLuong;
            

                var strChuoi = '<tr>'
                    + ' <td>' + msg.resultObj.maPhieu
                    + '      </td >'
                    + '           <td>' + msg.resultObj.maKeHoach
                    + '    </td>'
                    + '       <td>' + msg.resultObj.ngayTao
                    + '       </td>'
                    + '        <td>' + msg.resultObj.nguoiTao
                    + '        </td>'
                    + '        <td>' + msg.resultObj.nguoiNhan
                    + '        </td>'
                    + '       <td>' + msg.resultObj.trangThai
                    + '        </td>'
                    + '        <td class="text-center py-0 align-middle">'
                    + '            <div class="btn-group btn-group-sm">'
                    + '                <a class="btn btn-success" data-toggle="modal" data-target="#xemthongtinPhIEUcb"'
                    + '                   onclick="XemPhieuCheBien(' + msg.resultObj.id + ')" title="Xem phiếu chế biến">'
                    + '                    <i class="fas fa-eye"></i>'
                    + '              </a>'
                    + '           </div>'
                    + '       </td>'
                    + '    </tr >';

                $("#XemThongTinPCBTB1").prepend(strChuoi);
                // gửi thông điệp
                var user = msg.resultObj.userName;
                var message =
                {
                    Id: msg.resultObj.idThongBao,
                    Ten: "Hủy phiếu chế biến: " + msg.resultObj.maPhieu,
                    DuongDan: "/NhiemVuPhieuCheBien/Index",
                };
                connectionPhieuCheBien.invoke("SendPhieuCheBien", user, message).catch(function (err) {
                    return console.error(err.toString());
                });
               
            } else {
                toastr.error(msg.message);
            }
        },
        error: function (data) {
            toastr.error("Hủy thất bại");
        }
    });
}

// tìm kiếm
// ngày tìm kiếm kế hoạch
resetLaiNgayPhanHoi();
function resetLaiNgayPhanHoi() {
    $(function () {
        $('#NgayPhanHoi').daterangepicker({
            opens: 'left',
            defaultDate: '',
            minDate: 0,
        }, function (start, end, label) {
            console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
        }).val('');
    });
    $('#NgayPhanHoi').val('');
}

var frmCapNhatPhieuTK = $("#frmCapNhatPhieuTK");
frmCapNhatPhieuTK.submit(function (e) {
    $(".delete-phieuCho").remove();
    var ngay = $("#NgayPhanHoi").val();
    var tukhoa = $("#TuKhoaTKCapNhatPhieu").val();
    ThongTinPhieuCheBienChoNhapSanPham(ngay, tukhoa);
    e.preventDefault();
});

// tìm kiếm chê sbieesn hoàn thành
// ngày tìm kiếm kế hoạch
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