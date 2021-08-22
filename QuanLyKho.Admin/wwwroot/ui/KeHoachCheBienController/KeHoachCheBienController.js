let calendar;
$(function () {
    /* initialize the external events
     -----------------------------------------------------------------*/
    function ini_events(ele) {
        ele.each(function () {
            var eventObject = {
                title: $.trim($(this).text())
            }
            $(this).data('eventObject', eventObject)

            // make the event draggable using jQuery UI
            $(this).draggable({
                zIndex: 1070,
                revert: true,
                revertDuration: 0
            })
        })
    }

    ini_events($('#external-events div.external-event'))

    var Calendar = FullCalendar.Calendar;

    var calendarEl = document.getElementById('calendar');

    calendar = new Calendar(calendarEl, {
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        events: [],
        themeSystem: 'bootstrap',
        editable: false,
        droppable: true,
        eventClick: function (info) {
            var trangThai = info.event.extendedProps.description;
            var nhanKeHoach = info.event.extendedProps.department;
            if (trangThai == "ChuaHoanThanh" && nhanKeHoach == false) {
                document.getElementById("XoaKeHoachXem").style.display = "block";
                document.getElementById("SuaKeHoachXem").style.display = "block";
            } else {
                document.getElementById("XoaKeHoachXem").style.display = "none";
                document.getElementById("SuaKeHoachXem").style.display = "none";
            }

            ShowModalView(info.event.id);
        },
        dateClick: function (info) {
            var date = new Date(info.dateStr);
            var thang = (date.getMonth() + 1).toString();
            if (thang.length < 2) {
                thang = "0" + thang;
            }
            var ngay = date.getDate().toString();
            if (ngay.length < 2) {
                ngay = "0" + ngay;
            }
            var nam = date.getFullYear();
            var chuoi = thang + '/' + ngay + '/' + nam + ' - ' + thang + '/' + ngay + '/' + nam;
            $("#NgayKetThucDuKien").val(chuoi);
            $("#modalThem").modal("show");
        }
    });

    calendar.render();
});

function AddEvent(data) {
    calendar.addEvent(data);
}

function Remove(id) {
    calendar.getEventById(id).remove();
}
function callback() {
    $('.fc-next-button').click(function () {
        var date = calendar.getDate();
        var thang = date.getMonth() + 1;
        var ngay = new Date(date);
        var nam = ngay.getFullYear();
        ClearAllEvent();
        GetAllKeHoachTheoThang(thang, nam);
    });
    $('.fc-prev-button').click(function () {
        var date = calendar.getDate();
        var thang = date.getMonth() + 1;
        var ngay = new Date(date);
        var nam = ngay.getFullYear();
        ClearAllEvent();
        GetAllKeHoachTheoThang(thang, nam);
    });
}
setTimeout(function () {
    callback();
}, 1500);

function ClearAllEvent() {
    var listEvent = calendar.getEvents();
    listEvent.forEach(event => {
        event.remove()
    });
}
function GetDateNow() {
    var today = new Date();
    var thang = today.getMonth() + 1;
    var nam = today.getFullYear();
    GetAllKeHoachTheoThang(thang, nam);
}
//---------------------Lấy giá trị theo tháng
setTimeout(function () {
    GetDateNow();
}, 1500);
function GetAllKeHoachTheoThang(thang, nam) {
    var url = "/KeHoachCheBiens/GetAllKeHoachTheoThang/";
    $.ajax({
        type: "GET",
        url: url,
        data: { thang: thang, nam: nam },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                var moTa = '';
                var mau = '#00c0ef'
                var ten = item.maSo + ": " + item.ten;
                if (item.trangThai == "ChuaHoanThanh" && item.nhanKeHoach == false) {
                    moTa = 'ChuaHoanThanh';
                    mau = '#f39c12';
                }
                if (item.trangThai == "ChuaHoanThanh" && item.nhanKeHoach == true) {
                    moTa = 'ChuaHoanThanh';
                    mau = '#79378B';
                }
                if (item.trangThai == "HoanThanh") {
                    moTa = 'HoanThanh';
                    mau = '#00a65a';
                }
                if (item.trangThai == "BiHuy") {
                    moTa = 'BiHuy';
                    mau = '#FF0000';
                }

                AddEvent({
                    id: item.id,
                    title: ten,
                    description: moTa,
                    extendedProps: {
                        department: item.nhanKeHoach
                    },
                    start: new Date(item.ngayBatDauDuKien),
                    end: new Date(item.ngayKetThucDuKien),
                    backgroundColor: mau,
                    borderColor: mau,
                    allDay: true
                });
            });
        },
        error: function (req, status, error) {
        }
    });
}
//___________________________________________________________________________________///___________________
//THÊM
// lấy thông tin nhóm sản phẩm
ThongTinNhomSanPhamTimKiem();
function ThongTinNhomSanPhamTimKiem() {
    var url = "/KeHoachCheBiens/GetNhomSanPhamKH/";
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#NhomSP").append(new Option(item.ten, item.id));
            });
        },
        error: function (req, status, error) {
        }
    });
}

function TimKiemLoaiSanPham() {
    $("#LoaiSP option").remove();
    var str = '<option selected>Chọn dữ liệu...</option>';
    $("#LoaiSP").append(str);

    var id = $("#NhomSP").val();
    var url = "/KeHoachCheBiens/GetLoaiSanPhamKH/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#LoaiSP").append(new Option(item.ten, item.id));
            });
        },
        error: function (req, status, error) {
        }
    });
}

var frmTKKHCB = $("#frmTKKHCB");
frmTKKHCB.submit(function (e) {
    var id = $("#LoaiSP").val();
    var tuKhoa = $("#TimKiemSanPham").val();
    var url = "/KeHoachCheBiens/GetCongThucSanPham/";
    $(".Piko").remove();
    var str = "";
    if (id == "Chọn dữ liệu...") {
        toastr.error('Vui lòng chọn loại sản phẩm');
    } else {
        $.ajax({
            type: "GET",
            url: url,
            data: { tuKhoa: tuKhoa, idLoaiSanPham: id },
            dataType: "json",
            success: function (msg) {
                $("#bodyIdTable").remove();
                $("#showbodyIdTable").append('<div id="bodyIdTable"></div>');
                var i = 0;
                msg.forEach(function (item) {
                    i++;
                    str = str + '<table id="example246456" class="table table-bordered table-striped table-hover">'
                        + '<thead>'
                        + '   <tr>'
                        + '   <th colspan="4" class="text-center">' + item.maSo + ': ' + item.ten + '</th>'
                        + '   </tr>'
                        + '    <tr>'
                        + '      <th>Mã Số</th>'
                        + '     <th>Tên Công Thức</th>'
                        + '    <th>Trạng Thái Tồn</th>'
                        + '  <th class="text-center">Thêm</th>'
                        + '    </tr>'
                        + '    </thead>'
                        + ' <tbody id="tableAddruThem">';

                    item.congThucSanPhamKHs.forEach(function (it) {
                        str = str + '   <tr>'
                            + '       <td>' + it.maSo + '</td>'
                            + '       <td>' + it.ten + '</td>';

                        if (item.trangThaiTonKho) {
                            str = str + '        <td style="color:red;">Vượt mức tồn</td>';
                        } else {
                            str = str + '        <td>Bình thường</td>';
                        }

                        str = str + '       <td>'
                            + '<a class="btn btn-success btn-sm"'
                            + '     onclick="GetAddCT(' + it.id + ',\'' + it.maSo + '\',' + item.id + ',\'' + item.ten + '\')" title="Thêm">'
                            + '      <i class="fas fa-plus"></i>'
                            + '   </a>'
                            + '</td>'
                            + '   </tr>';
                    });

                    str = str + '</tbody>'
                        + '</table>';
                });
                if (i == 0) {
                    str = '<p class="Piko" style="color: red;">Không tìm thấy kết quả</p>'
                }
                $("#bodyIdTable").append(str);
            },
            error: function (req, status, error) {
            }
        });
    }

    e.preventDefault();
});
// thêm công thức vào thêm kế hoạch
var arrayThemCongThuc = [];
function GetAddCT(idCT, maSo, idSP, tenSP) {
    if (arrayThemCongThuc.indexOf(idCT) === -1) {
        ThemCongThuc(idCT, maSo, idSP, tenSP);
    }
    else {
        toastr.error('Công thức đã được thêm');
    }
}
function ThemCongThuc(idCT, maSo, idSP, tenSP) {
    arrayThemCongThuc.push(idCT);
    var str = ' <tr class="CongThucThemClass" id="ThemCTKH-' + idCT + '">'
        + '<td>' + maSo + '</br> <small>Sản phẩm: ' + tenSP + '</small></td>'
        + '<td>'
        + '<select id="DonViTinhThem-' + idCT + '" name="DonViTinh[]" class="form-control selectMater" required>'
        + '</select>'
        + '</td>'
        + '<td>'
        + ' <input type="number" min="0" class="form-control" name="SoLuong[]" id="SoLuongThem-' + idCT + '" placeholder="Số lượng" required>'
        + ' </td>'
        + ' <td>'
        + '   <input type="hidden" class="form-control" name="IdCongThuc[]" value="' + idCT + '" >'
        + '   <input type="text" class="form-control" name="GhiChuCheBien[]" placeholder="Ghi chú">'
        + ' </td>'
        + ' <td class="text-center py-0 align-middle">'
        + ' <div class="btn-group btn-group-sm">'
        + '    <a class="btn btn-primary" data-toggle="modal" data-target="#exampleViewChuyenDoi"'
        + '      onclick="GetViewChuyenDoi(' + idCT + ',' + idSP + ')" title="Xem chuyển đổi">'
        + '      <i class="fas fa-eye"></i>'
        + '  </a>'
        + '  <a class="btn btn-danger"'
        + '    onclick="GetDeleteCongThuc(' + idCT + ')" title="Xóa">'
        + '      <i class="fas fa-trash"></i>'
        + '</a>'
        + '</div>'
        + '</td>'
        + '</tr>';

    $("#tableAddru").append(str);
    TimDonViTinhKH(idCT, idSP);
}

function TimDonViTinhKH(idCT, idSP) {
    var url = "/KeHoachCheBiens/GetDonViTinhKHCB/";
    var str = "#DonViTinhThem-" + idCT;
    $.ajax({
        type: "GET",
        url: url,
        data: { idSanPham: idSP },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $(str).append(new Option(item.ten, item.ten));
            });
        },
        error: function (req, status, error) {
        }
    });
}

function GetDeleteCongThuc(idCT) {
    var str = "#ThemCTKH-" + idCT;
    $(str).remove();
    var pos = arrayThemCongThuc.indexOf(idCT);
    arrayThemCongThuc.splice(pos, 1);
}

$(function () {
    $('#NgayKetThucDuKien').daterangepicker({
        opens: 'left'
    }, function (start, end, label) {
        console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
    });
});

// tìm kiếm nhân viên để giao việc
var frmTKNV = $("#frmTKNV");
frmTKNV.submit(function (e) {
    var url = "/KeHoachCheBiens/NhanVienKHCheBien/";
    var tuKhoa = $("#TimKiemNV").val();
    $(".nhanvienXoa").remove();
    $.ajax({
        type: "GET",
        url: url,
        data: { tuKhoa: tuKhoa },
        dataType: "json",
        success: function (msg) {
            var i = 0;
            msg.forEach(function (item) {
                i++;
                var str = '<tr class="nhanvienXoa">'
                    + '<td>' + item.maSo + '</td>'
                    + '     <td>' + item.canCuocCongDan + '</td>'
                    + '     <td>' + item.ho + '</td>'
                    + '       <td>' + item.ten + '</td>'
                    + '        <td>'
                    + '<a class="btn btn-success btn-sm"'
                    + '     onclick="GetAddNhanVienGV(\'' + item.id + '\',\'' + item.maSo + '\')" title="Thêm">'
                    + '      <i class="fas fa-plus"></i>'
                    + '   </a>'
                    + '         </td>'
                    + '     </tr >';
                $("#tableNVIEN").append(str);
            });
            if (i == 0) {
                $("#tableNVIEN").append('<p class="nhanvienXoa" style="color: red;">Không tìm thấy kết quả</p>');
            }
        },
        error: function (req, status, error) {
        }
    });
    e.preventDefault();
});

function GetAddNhanVienGV(id, maSo) {
    $("#IdNguoiNhan").val(id);
    document.getElementById("ShowNVienGV").innerHTML = maSo;
    $("#exampleGiaoViec").modal("hide");
}

// chuyển đổi đơn vị tính
function GetViewChuyenDoi(idCT, idSP) {
    $(".chuoiChuyenDoi").remove();
    var str = "#SoLuongThem-" + idCT;
    var str1 = "#DonViTinhThem-" + idCT + " option:selected";
    var slNhap = $(str).val();
    var dvtNhap = $(str1).text();
    var soLuong = 0;
    $("#PackSL").val(slNhap);
    var url = "/KeHoachCheBiens/GetShowDonViTinhKHCB/";
    $.ajax({
        type: "GET",
        url: url,
        data: { idSanPham: idSP },
        dataType: "json",
        success: function (msg) {
            var tenCB = "";
            msg.forEach(function (it) {
                if (it.coBan == true) {
                    tenCB = it.ten;
                }
            });
            msg.forEach(function (item) {
                if (item.coBan) {
                    $("#PackMD").val(item.ten);
                } else {
                    var chuoi = '<tr class="chuoiChuyenDoi">'
                        + '<td>' + item.ten + '</td>';
                    chuoi = chuoi + '<td>' + item.giaTriChuyenDoi + '</br> <small>1 ' + item.ten + ' = ' + item.giaTriChuyenDoi + ' ' + tenCB + '</small></td>';

                    soLuong = slNhap * item.giaTriChuyenDoi;
                    if (item.ten == dvtNhap) {
                        chuoi = chuoi + '<td style="color:red;">' + soLuong + '</td>';
                    } else {
                        chuoi = chuoi + '<td>' + soLuong + '</td>';
                    }
                    chuoi = chuoi + '</tr>';
                    $("#tableNPacketert").append(chuoi);
                }
            });
        },
        error: function (req, status, error) {
        }
    });
}

/// show modal kế hoạch
function ShowModalView(id) {
    $("#exampleViewKH").modal("show");
    var url = "/KeHoachCheBiens/GetInfoKeHoachById/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            $("#idKeHoachXem").val(id);
            var ngayDK = msg.resultObj.ngayDuKienBatDau + ' - ' + msg.resultObj.ngayDuKienKetThuc;
            document.getElementById('codeViewKH').innerHTML = msg.resultObj.maSo;
            document.getElementById('nameViewKH').innerHTML = msg.resultObj.ten;
            document.getElementById('createDateViewKH').innerHTML = msg.resultObj.ngayTao;
            document.getElementById('dateDKViewKH').innerHTML = ngayDK;
            document.getElementById('nguoitaoViewKH').innerHTML = msg.resultObj.nguoiTao;
            document.getElementById('nguoinhanViewKH').innerHTML = msg.resultObj.nguoiNhan;
            document.getElementById('trangthaiNhanViewKH').innerHTML = msg.resultObj.nhanKeHoach;
            document.getElementById('trangthaiViewKH').innerHTML = msg.resultObj.trangThai;
            document.getElementById('ghichuViewKH').innerHTML = msg.resultObj.ghiChu;
            $(".xemdANHsACHsP").remove();
            msg.resultObj.danhSachCheBiens.forEach(function (item) {
                var ghiChu = "";
                if (item.ghiChu == null) {
                    ghiChu = "";
                } else {
                    ghiChu = item.ghiChu;
                }
                var str = '<tr class="xemdANHsACHsP">'
                    + '<td>' + item.maSoSanPham + '</td>'
                    + '<td>' + item.tenSanPham + '</td>'
                    + '<td>' + item.maSoCongThuc + '</td>'
                    + '<td>' + item.soLuong + '</td>'
                    + '<td>' + item.donViTinh + '</td>'
                    + '<td>' + item.trangThai + '</td>'
                    + '<td>' + ghiChu + '</td>'
                    + '</tr>';
                $("#ShowViewKH12").append(str);
            });
        },
        error: function (req, status, error) {
        }
    });
}

// xóa kế hoạch
function XoaKeHoach() {
    var id = $("#idKeHoachXem").val();
    $("#idKeHoachXoa").val(id);
    $("#exampleViewKH").modal("hide");
    $("#exampleXoaKeHoachCB").modal("show");
}

//___________________________________________________________________________________///___________________
//SỬA
// lấy thông tin nhóm sản phẩm
ThongTinNhomSanPhamTimKiemEdit();
function ThongTinNhomSanPhamTimKiemEdit() {
    var url = "/KeHoachCheBiens/GetNhomSanPhamKH/";
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#NhomSPEdit").append(new Option(item.ten, item.id));
            });
        },
        error: function (req, status, error) {
        }
    });
}

function TimKiemLoaiSanPhamEdit() {
    $("#LoaiSPEdit option").remove();
    var str = '<option selected>Chọn dữ liệu...</option>';
    $("#LoaiSPEdit").append(str);

    var id = $("#NhomSPEdit").val();
    var url = "/KeHoachCheBiens/GetLoaiSanPhamKH/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#LoaiSPEdit").append(new Option(item.ten, item.id));
            });
        },
        error: function (req, status, error) {
        }
    });
}

var frmTKKHCBEdit = $("#frmTKKHCBEdit");
frmTKKHCBEdit.submit(function (e) {
    var id = $("#LoaiSPEdit").val();
    var tuKhoa = $("#TimKiemSanPhamEdit").val();
    var url = "/KeHoachCheBiens/GetCongThucSanPham/";
    $(".PikoEdit").remove();
    var str = "";
    if (id == "Chọn dữ liệu...") {
        toastr.error('Vui lòng chọn loại sản phẩm');
    } else {
        $.ajax({
            type: "GET",
            url: url,
            data: { tuKhoa: tuKhoa, idLoaiSanPham: id },
            dataType: "json",
            success: function (msg) {
                $("#bodyIdTableEdit").remove();
                $("#showbodyIdTableEdit").append('<div id="bodyIdTableEdit"></div>');
                var i = 0;
                msg.forEach(function (item) {
                    i++;
                    str = str + '<table id="example246456Edit" class="table table-bordered table-striped table-hover">'
                        + '<thead>'
                        + '   <tr>'
                        + '   <th colspan="4" class="text-center">' + item.maSo + ': ' + item.ten + '</th>'
                        + '   </tr>'
                        + '    <tr>'
                        + '      <th>Mã Số</th>'
                        + '     <th>Tên Công Thức</th>'
                        + '    <th>Trạng Thái Tồn</th>'
                        + '  <th class="text-center">Thêm</th>'
                        + '    </tr>'
                        + '    </thead>'
                        + ' <tbody id="tableAddruThemEdit">';

                    item.congThucSanPhamKHs.forEach(function (it) {
                        str = str + '   <tr>'
                            + '       <td>' + it.maSo + '</td>'
                            + '       <td>' + it.ten + '</td>';

                        if (item.trangThaiTonKho) {
                            str = str + '        <td style="color:red;">Vượt mức tồn</td>';
                        } else {
                            str = str + '        <td>Bình thường</td>';
                        }

                        str = str + '       <td>'
                            + '<a class="btn btn-success btn-sm"'
                            + '     onclick="GetAddCTEdit(' + it.id + ',\'' + it.maSo + '\',' + item.id + ',\'' + item.ten + '\')" title="Thêm">'
                            + '      <i class="fas fa-plus"></i>'
                            + '   </a>'
                            + '</td>'
                            + '   </tr>';
                    });

                    str = str + '</tbody>'
                        + '</table>';
                });
                if (i == 0) {
                    str = '<p class="PikoEdit" style="color: red;">Không tìm thấy kết quả</p>';
                }
                $("#bodyIdTableEdit").append(str);
            },
            error: function (req, status, error) {
            }
        });
    }

    e.preventDefault();
});

// thêm công thức vào thêm kế hoạch sửa
var arrayThemCongThucEdit = [];
function GetAddCTEdit(idCT, maSo, idSP, tenSP) {
    if (arrayThemCongThucEdit.indexOf(idCT) === -1) {
        ThemCongThucEdit(idCT, maSo, idSP, tenSP);
    }
    else {
        toastr.error('Công thức đã được thêm');
    }
}
function ThemCongThucEdit(idCT, maSo, idSP, tenSP) {
    arrayThemCongThucEdit.push(idCT);
    var str = ' <tr class="CongThucThemClassEdit" id="ThemCTKHEdit-' + idCT + '">'
        + '<td>' + maSo + '</br> <small>Sản phẩm: ' + tenSP + '</small></td>'
        + '<td>'
        + '<select id="DonViTinhThemEdit-' + idCT + '" name="DonViTinhThem[]" class="form-control selectMater" required>'
        + '</select>'
        + '</td>'
        + '<td>'
        + ' <input type="number" min="0" class="form-control" name="SoLuongThem[]" id="SoLuongThemEdit-' + idCT + '" placeholder="Số lượng" required>'
        + ' </td>'
        + ' <td>'
        + '   <input type="hidden" class="form-control" name="IdCongThucThem[]" value="' + idCT + '" >'
        + '   <input type="text" class="form-control" name="GhiChuCheBienThem[]" placeholder="Ghi chú">'
        + ' </td>'
        + ' <td class="text-center py-0 align-middle">'
        + ' <div class="btn-group btn-group-sm">'
        + '    <a class="btn btn-primary" data-toggle="modal" data-target="#exampleViewChuyenDoiEdit"'
        + '      onclick="GetViewChuyenDoiEdit(' + idCT + ',' + idSP + ')" title="Xem chuyển đổi">'
        + '      <i class="fas fa-eye"></i>'
        + '  </a>'
        + '  <a class="btn btn-danger"'
        + '    onclick="GetDeleteCongThucEdit(' + idCT + ')" title="Xóa">'
        + '      <i class="fas fa-trash"></i>'
        + '</a>'
        + '</div>'
        + '</td>'
        + '</tr>';

    $("#tableAddruEdit").append(str);
    TimDonViTinhKHEdit(idCT, idSP);
}

function TimDonViTinhKHEdit(idCT, idSP) {
    var url = "/KeHoachCheBiens/GetDonViTinhKHCB/";
    var str = "#DonViTinhThemEdit-" + idCT;
    $.ajax({
        type: "GET",
        url: url,
        data: { idSanPham: idSP },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $(str).append(new Option(item.ten, item.ten));
            });
        },
        error: function (req, status, error) {
        }
    });
}

function GetDeleteCongThucEdit(idCT) {
    var str = "#ThemCTKHEdit-" + idCT;
    $(str).remove();
    var pos = arrayThemCongThucEdit.indexOf(idCT);
    arrayThemCongThucEdit.splice(pos, 1);
}

$(function () {
    $('#NgayKetThucDuKienEdit').daterangepicker({
        opens: 'left'
    }, function (start, end, label) {
        console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
    });
});
// tìm kiếm nhân viên để giao việc
var frmTKNVEdit = $("#frmTKNVEdit");
frmTKNVEdit.submit(function (e) {
    var url = "/KeHoachCheBiens/NhanVienKHCheBien/";
    var tuKhoa = $("#TimKiemNVEdit").val();
    $(".nhanvienXoaEdit").remove();
    $.ajax({
        type: "GET",
        url: url,
        data: { tuKhoa: tuKhoa },
        dataType: "json",
        success: function (msg) {
            var i = 0;
            msg.forEach(function (item) {
                i++;
                var str = '<tr class="nhanvienXoaEdit">'
                    + '<td>' + item.maSo + '</td>'
                    + '     <td>' + item.canCuocCongDan + '</td>'
                    + '     <td>' + item.ho + '</td>'
                    + '       <td>' + item.ten + '</td>'
                    + '        <td>'
                    + '<a class="btn btn-success btn-sm"'
                    + '     onclick="GetAddNhanVienGVEdit(\'' + item.id + '\',\'' + item.maSo + '\')" title="Thêm">'
                    + '      <i class="fas fa-plus"></i>'
                    + '   </a>'
                    + '         </td>'
                    + '     </tr >';
                $("#tableNVIENEdit").append(str);
            });
            if (i == 0) {
                $("#tableNVIENEdit").append('<p class="nhanvienXoaEdit" style="color: red;">Không tìm thấy kết quả</p>');
            }
        },
        error: function (req, status, error) {
        }
    });
    e.preventDefault();
});

function GetAddNhanVienGVEdit(id, maSo) {
    $("#IdNguoiNhanEdit").val(id);
    document.getElementById("ShowNVienGVEdit").innerHTML = maSo;
    $("#exampleGiaoViecEdit").modal("hide");
}

// chuyển đổi đơn vị tính
function GetViewChuyenDoiEdit(idCT, idSP) {
    $(".chuoiChuyenDoiEdit").remove();
    var str = "#SoLuongThemEdit-" + idCT;
    var str1 = "#DonViTinhThemEdit-" + idCT + " option:selected";
    var slNhap = $(str).val();
    var dvtNhap = $(str1).text();
    var soLuong = 0;
    $("#PackSLEdit").val(slNhap);
    var url = "/KeHoachCheBiens/GetShowDonViTinhKHCB/";
    $.ajax({
        type: "GET",
        url: url,
        data: { idSanPham: idSP },
        dataType: "json",
        success: function (msg) {
            var tenCB = "";
            msg.forEach(function (it) {
                if (it.coBan == true) {
                    tenCB = it.ten;
                }
            });
            msg.forEach(function (item) {
                if (item.coBan) {
                    $("#PackMDEdit").val(item.ten);
                } else {
                    var chuoi = '<tr class="chuoiChuyenDoiEdit">'
                        + '<td>' + item.ten + '</td>';
                    chuoi = chuoi + '<td>' + item.giaTriChuyenDoi + '</br> <small>1 ' + item.ten + ' = ' + item.giaTriChuyenDoi + ' ' + tenCB + '</small></td>';

                    soLuong = slNhap * item.giaTriChuyenDoi;
                    if (item.ten == dvtNhap) {
                        chuoi = chuoi + '<td style="color:red;">' + soLuong + '</td>';
                    } else {
                        chuoi = chuoi + '<td>' + soLuong + '</td>';
                    }
                    chuoi = chuoi + '</tr>';
                    $("#tableNPacketertEdit").append(chuoi);
                }
            });
        },
        error: function (req, status, error) {
        }
    });
}

//chỉnh sửa
function ChinhSuaKeHoach() {
    var id = $("#idKeHoachXem").val();
    $("#idKHSua").val(id);
    $("#exampleViewKH").modal("hide");
    $("#modalSua").modal("show");
    $(".CongThucThemClassEdit").remove();
    arrayThemCongThucEdit = [];
    document.getElementById('ShowNVienGVEdit').innerHTML = "";
    $("#IdNguoiNhanEdit").val('');
    var url = "/KeHoachCheBiens/GetInfoKeHoachById/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            $("#CodeChonEdit").val(msg.resultObj.maSo);
            $("#NameEdit").val(msg.resultObj.ten);
            $("#NoteEdit").val(msg.resultObj.ghiChu);
            var ngayDK = msg.resultObj.ngayDuKienBatDauEdit + ' - ' + msg.resultObj.ngayDuKienKetThucEdit;
            $("#NgayKetThucDuKienEdit").val(ngayDK);
            document.getElementById('ShowNVienGVEdit').innerHTML = msg.resultObj.nguoiNhan;
            $("#IdNguoiNhanEdit").val(msg.resultObj.idNguoiNhan);
            /////
            msg.resultObj.danhSachCheBiens.forEach(function (item) {
                arrayThemCongThucEdit.push(item.idCongThuc);
                var ghiChu = "";
                if (item.ghiChu == null) {
                    ghiChu = "";
                } else {
                    ghiChu = item.ghiChu;
                }
                var str = ' <tr class="CongThucThemClassEdit" id="ThemCTKHEdit-' + item.idCongThuc + '">'
                    + '<td>' + item.maSoCongThuc + '</br> <small>Sản phẩm: ' + item.tenSanPham + '</small></td>'
                    + '<td>'
                    + '<select id="DonViTinhThemEdit-' + item.idCongThuc + '" name="DonViTinh[]" class="form-control selectMater" required>'
                    + '</select>'
                    + '</td>'
                    + '<td>'
                    + ' <input type="number" min="0" class="form-control" name="SoLuong[]" id="SoLuongThemEdit-' + item.idCongThuc + '"'
                    + 'value="' + item.soLuong + '" placeholder = "Số lượng" required > '
                    + ' </td>'
                    + ' <td>'
                    + '   <input type="hidden" class="form-control" name="IdChiTiet[]" value="' + item.idChiTiet + '" />'
                    + '   <input type="hidden" class="form-control" name="IdCongThuc[]" value="' + item.idCongThuc + '" />'
                    + '   <input type="text" class="form-control" name="GhiChuCheBien[]" value="' + ghiChu + '" placeholder="Ghi chú">'
                    + ' </td>'
                    + ' <td class="text-center py-0 align-middle">'
                    + ' <div class="btn-group btn-group-sm">'
                    + '    <a class="btn btn-primary" data-toggle="modal" data-target="#exampleViewChuyenDoiEdit"'
                    + '      onclick="GetViewChuyenDoiEdit(' + item.idCongThuc + ',' + item.idSanPham + ')" title="Xem chuyển đổi">'
                    + '      <i class="fas fa-eye"></i>'
                    + '  </a>'
                    + '  <a class="btn btn-danger"'
                    + '    onclick="GetDeleteCongThucUpdate(' + item.idChiTiet + ',' + item.idCongThuc + ')" title="Xóa">'
                    + '      <i class="fas fa-trash"></i>'
                    + '</a>'
                    + '</div>'
                    + '</td>'
                    + '</tr>';

                $("#tableAddruEdit").append(str);
                TimDonViTinhKHUpdate(item.idCongThuc, '' + item.donViTinh + '', item.idSanPham);
            });
        },
        error: function (req, status, error) {
            toastr.error('Xóa thất bại');
        }
    });
}

function TimDonViTinhKHUpdate(idCT, donViTinh, idSP) {
    var url = "/KeHoachCheBiens/GetDonViTinhKHCB/";
    var str = "#DonViTinhThemEdit-" + idCT;
    $(str).append(new Option(donViTinh, donViTinh));
    $.ajax({
        type: "GET",
        url: url,
        data: { idSanPham: idSP },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                if (item.ten != donViTinh) {
                    $(str).append(new Option(item.ten, item.ten));
                }
            });
        },
        error: function (req, status, error) {
        }
    });
}

// ngày dự kiến danh sách
$(function () {
    $('#NgayDuKienDanhSach').daterangepicker({
        opens: 'left',
        defaultDate: '',
        minDate: 0,
    }, function (start, end, label) {
        console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
    }).val('');
});
$('#NgayDuKienDanhSach').val('');
// show danh dách kế hoạch
function GetViewShowDSKHCB(id) {
    ShowModalView(id);
    document.getElementById("XoaKeHoachXem").style.display = "none";
    document.getElementById("SuaKeHoachXem").style.display = "none";
}