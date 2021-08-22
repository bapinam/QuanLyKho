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
    var url = "/KeHoachDatHangs/GetAllKeHoachTheoThang/";
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
//---------------------------------------Thêm ----------------------------------
//Tìm kiếm loại NVL
function TimKiemLoaiNVL() {
    $("#LoaiNVL option").remove();
    var str = '<option selected>Chọn dữ liệu...</option>';
    $("#LoaiNVL").append(str);

    var id = $("#NhomNVL").val();
    var url = "/KeHoachDatHangs/GetLoaiNVLKH/";
    $.ajax({
        type: "GET",
        url: url,
        data: { nhomNVL: id },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#LoaiNVL").append(new Option(item.ten, item.id));
            });
        },
        error: function (req, status, error) {
        }
    });
}

// tìm kiếm nguyên vật liệu
var frmTKKHCB = $("#frmTKKHCB");
frmTKKHCB.submit(function (e) {
    var id = $("#LoaiNVL").val();
    var tuKhoa = $("#TimKiemNVL").val();
    var url = "/KeHoachDatHangs/GetNguyenVatLieuKH/";
    $(".Piko").remove();
    var str = "";
    if (id == "Chọn dữ liệu...") {
        toastr.error('Vui lòng chọn loại nguyên vật liệu');
    } else {
        $.ajax({
            type: "GET",
            url: url,
            data: { tuKhoa: tuKhoa, idLoaiNVL: id },
            dataType: "json",
            success: function (msg) {
                var i = 0;
                msg.forEach(function (item) {
                    i++;
                    var trangThai;
                    if (item.trangThaiTonKho) {
                        trangThai = '<p style="color: red;">Vượt mức tồn</p>';
                    } else {
                        trangThai = "Bình thường"
                    }
                    str = str + '<tr class="Piko"> '
                        + '<td>' + item.maSo + '</td>'
                        + '<td>' + item.ten + '</td>'
                        + '<td>' + item.soLuong + '</td>'
                        + '<td>' + trangThai + '</td>'
                        + '<td>'
                        + '<a class="btn btn-success btn-sm"'
                        + '     onclick="GetAddNVL(' + item.id + ',\'' + item.maSo + '\',\'' + item.ten + '\')" title="Thêm">'
                        + '      <i class="fas fa-plus"></i>'
                        + '   </a>'
                        + '</td>'
                        + '</tr>';
                });
                if (i == 0) {
                    str = '<p class="Piko" style="color: red;">Không tìm thấy kết quả</p>'
                }
                $("#timKiemNVLtable").append(str);
            },
            error: function (req, status, error) {
            }
        });
    }
    e.preventDefault();
});

// thêm nguyên vật liệu
var arrayThemNguyenVatLieu = [];
function GetAddNVL(id, maSo, ten) {
    if (arrayThemNguyenVatLieu.indexOf(id) === -1) {
        ThemNguyenVatLieu(id, maSo, ten);
    }
    else {
        toastr.error('Nguyên vật liệu đã được thêm');
    }
}
function ThemNguyenVatLieu(id, maSo, ten) {
    arrayThemNguyenVatLieu.push(id);
    var str = ' <tr class="NguyenVatLieuThemClass" id="ThemCTKH-' + id + '">'
        + '<td>' + maSo + ' </br> <small>' + ten + '</small></td>'
        + '<td>'
        + '<select id="DonViTinhThem-' + id + '" name="DonViTinh[]" class="form-control selectMater" required>'
        + '</select>'
        + '</td>'
        + '<td>'
        + ' <input type="number" min="0" class="form-control" name="SoLuong[]" id="SoLuongThem-' + id + '" placeholder="Số lượng" required>'
        + ' </td>'
        + ' <td>'
        + '   <input type="hidden" class="form-control" name="IdNguyenVatLieu[]" value="' + id + '" >'
        + '   <input type="text" class="form-control" name="GhiChuDatHang[]" placeholder="Ghi chú">'
        + ' </td>'
        //+ ' <td id="showNCC-' + id + '">'
        //+ '   <input type="hidden" class="form-control" name="IdNhaCungCap[]" value="0"  id="NCC-' + id + '" >'
        //+ ' </td>'
        + ' <td class="text-center py-0 align-middle">'
        + ' <div class="btn-group btn-group-sm">'
        + '    <a class="btn btn-primary" data-toggle="modal" data-target="#exampleViewChuyenDoi"'
        + '      onclick="GetViewChuyenDoi(' + id + ')" title="Xem chuyển đổi">'
        + '      <i class="fas fa-eye"></i>'
        + '  </a>'
        + '  <a class="btn btn-danger"'
        + '    onclick="GetDeleteNguyenVatLieu(' + id + ')" title="Xóa">'
        + '      <i class="fas fa-trash"></i>'
        + '</a>'
        + '</div>'
        + '</td>'
        + '</tr>';

    $("#tableAddru").append(str);
    TimDonViTinhKH(id);
}

function TimDonViTinhKH(id) {
    var url = "/KeHoachDatHangs/GetDonViTinhKHDH/";
    var str = "#DonViTinhThem-" + id;
    $.ajax({
        type: "GET",
        url: url,
        data: { idNVL: id },
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

function GetDeleteNguyenVatLieu(id) {
    var str = "#ThemCTKH-" + id;
    $(str).remove();
    var pos = arrayThemNguyenVatLieu.indexOf(id);
    arrayThemNguyenVatLieu.splice(pos, 1);
}

$(function () {
    $('#NgayKetThucDuKien').daterangepicker({
        opens: 'left'
    }, function (start, end, label) {
        console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
    });
});

// chuyển đổi đơn vị tính
function GetViewChuyenDoi(id) {
    $(".chuoiChuyenDoi").remove();
    var str = "#SoLuongThem-" + id;
    var str1 = "#DonViTinhThem-" + id + " option:selected";
    var slNhap = $(str).val();
    var dvtNhap = $(str1).text();
    var soLuong = 0;
    $("#PackSL").val(slNhap);
    var url = "/KeHoachDatHangs/GetShowDonViTinhKHDH/";
    $.ajax({
        type: "GET",
        url: url,
        data: { idNVL: id },
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

// tìm kiếm nhân viên để giao việc
var frmTKNV = $("#frmTKNV");
frmTKNV.submit(function (e) {
    var url = "/KeHoachDatHangs/NhanVienKHDatHang/";
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

/////////////////////////Xem thông tin kế hoạch
/// show modal kế hoạch
function ShowModalView(id) {
    $("#exampleViewKH").modal("show");
    var url = "/KeHoachDatHangs/GetInfoKeHoachById/";
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
            msg.resultObj.danhSachDatHangs.forEach(function (item) {
                var ghiChu = "";
                if (item.ghiChu == null) {
                    ghiChu = "";
                } else {
                    ghiChu = item.ghiChu;
                }
                var str = '<tr class="xemdANHsACHsP">'
                    + '<td>' + item.maSoNguyenVatLieu + '</td>'
                    + '<td>' + item.tenNguyenVatLieu + '</td>'
                    + '<td>' + item.soLuong + '</td>'
                    + '<td>' + item.donViTinh + '</td>'
                    + '<td>' + item.maSoNCC + '</td>'
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
    $("#exampleXoaKeHoachDH").modal("show");
}

////-------------------------------SỬA----------------------------
//Tìm kiếm loại NVL
function TimKiemLoaiNVLEdit() {
    $("#LoaiNVLEdit option").remove();
    var str = '<option selected>Chọn dữ liệu...</option>';
    $("#LoaiNVLEdit").append(str);

    var id = $("#NhomNVLEdit").val();
    var url = "/KeHoachDatHangs/GetLoaiNVLKH/";
    $.ajax({
        type: "GET",
        url: url,
        data: { nhomNVL: id },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#LoaiNVLEdit").append(new Option(item.ten, item.id));
            });
        },
        error: function (req, status, error) {
        }
    });
}

// tìm kiếm nguyên vật liệu
var frmTKKHCBEdit = $("#frmTKKHCBEdit");
frmTKKHCBEdit.submit(function (e) {
    var id = $("#LoaiNVLEdit").val();
    var tuKhoa = $("#TimKiemNVLEdit").val();
    var url = "/KeHoachDatHangs/GetNguyenVatLieuKH/";
    $(".PikoEdit").remove();
    var str = "";
    if (id == "Chọn dữ liệu...") {
        toastr.error('Vui lòng chọn loại nguyên vật liệu');
    } else {
        $.ajax({
            type: "GET",
            url: url,
            data: { tuKhoa: tuKhoa, idLoaiNVL: id },
            dataType: "json",
            success: function (msg) {
                var i = 0;
                msg.forEach(function (item) {
                    i++;
                    var trangThai;
                    if (item.trangThaiTonKho) {
                        trangThai = '<p style="color: red;">Vượt mức tồn</p>';
                    } else {
                        trangThai = "Bình thường"
                    }
                    str = str + '<tr class="PikoEdit"> '
                        + '<td>' + item.maSo + '</td>'
                        + '<td>' + item.ten + '</td>'
                        + '<td>' + item.soLuong + '</td>'
                        + '<td>' + trangThai + '</td>'
                        + '<td>'
                        + '<a class="btn btn-success btn-sm"'
                        + '     onclick="GetAddNVLEdit(' + item.id + ',\'' + item.maSo + '\',\'' + item.ten + '\')" title="Thêm">'
                        + '      <i class="fas fa-plus"></i>'
                        + '   </a>'
                        + '</td>'
                        + '</tr>';
                });
                if (i == 0) {
                    str = '<p class="PikoEdit" style="color: red;">Không tìm thấy kết quả</p>'
                }
                $("#timKiemNVLtableEdit").append(str);
            },
            error: function (req, status, error) {
            }
        });
    }
    e.preventDefault();
});

// thêm nguyên vật liệu
var arrayThemNguyenVatLieuEdit = [];
function GetAddNVLEdit(id, maSo, ten) {
    if (arrayThemNguyenVatLieuEdit.indexOf(id) === -1) {
        ThemNguyenVatLieuEdit(id, maSo, ten);
    }
    else {
        toastr.error('Nguyên vật liệu đã được thêm');
    }
}
function ThemNguyenVatLieuEdit(id, maSo, ten) {
    arrayThemNguyenVatLieuEdit.push(id);
    var str = ' <tr class="NguyenVatLieuThemClassEdit" id="ThemCTKHEdit-' + id + '">'
        + '<td>' + maSo + ' </br> <small>' + ten + '</small></td>'
        + '<td>'
        + '<select id="DonViTinhThemEdit-' + id + '" name="DonViTinhThem[]" class="form-control selectMater" required>'
        + '</select>'
        + '</td>'
        + '<td>'
        + ' <input type="number" min="0" class="form-control" name="SoLuongThem[]" id="SoLuongThemEdit-' + id + '" placeholder="Số lượng" required>'
        + ' </td>'
        + ' <td>'
        + '   <input type="hidden" class="form-control" name="IdNguyenVatLieuThem[]" value="' + id + '" >'
        + '   <input type="text" class="form-control" name="GhiChuDatHangThem[]" placeholder="Ghi chú">'
        + ' </td>'
        //+ ' <td id="showNCCEdit-' + id + '">'
        //+ '   <input type="hidden" class="form-control" name="IdNhaCungCapThem[]" value="0"  id="NCCEdit-' + id + '" >'
        //+ ' </td>'
        + ' <td class="text-center py-0 align-middle">'
        + ' <div class="btn-group btn-group-sm">'
        + '    <a class="btn btn-primary" data-toggle="modal" data-target="#exampleViewChuyenDoiEdit"'
        + '      onclick="GetViewChuyenDoiEdit(' + id + ')" title="Xem chuyển đổi">'
        + '      <i class="fas fa-eye"></i>'
        + '  </a>'
        + '  <a class="btn btn-danger"'
        + '    onclick="GetDeleteNguyenVatLieuEdit(' + id + ')" title="Xóa">'
        + '      <i class="fas fa-trash"></i>'
        + '</a>'
        + '</div>'
        + '</td>'
        + '</tr>';

    $("#tableAddruEdit").append(str);
    TimDonViTinhKHEdit(id);
}

function TimDonViTinhKHEdit(id) {
    var url = "/KeHoachDatHangs/GetDonViTinhKHDH/";
    var str = "#DonViTinhThemEdit-" + id;
    $.ajax({
        type: "GET",
        url: url,
        data: { idNVL: id },
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

function GetDeleteNguyenVatLieuEdit(id) {
    var str = "#ThemCTKHEdit-" + id;
    $(str).remove();
    var pos = arrayThemNguyenVatLieuEdit.indexOf(id);
    arrayThemNguyenVatLieuEdit.splice(pos, 1);
}

$(function () {
    $('#NgayKetThucDuKienEdit').daterangepicker({
        opens: 'left'
    }, function (start, end, label) {
        console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
    });
});

// chuyển đổi đơn vị tính
function GetViewChuyenDoiEdit(id) {
    $(".chuoiChuyenDoiEdit").remove();
    var str = "#SoLuongThemEdit-" + id;
    var str1 = "#DonViTinhThemEdit-" + id + " option:selected";
    var slNhap = $(str).val();
    var dvtNhap = $(str1).text();
    var soLuong = 0;
    $("#PackSLEdit").val(slNhap);
    var url = "/KeHoachDatHangs/GetShowDonViTinhKHDH/";
    $.ajax({
        type: "GET",
        url: url,
        data: { idNVL: id },
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

// tìm kiếm nhân viên để giao việc
var frmTKNVEdit = $("#frmTKNVEdit");
frmTKNVEdit.submit(function (e) {
    var url = "/KeHoachDatHangs/NhanVienKHDatHang/";
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

//chỉnh sửa
function ChinhSuaKeHoach() {
    var id = $("#idKeHoachXem").val();
    $("#idKHSua").val(id);
    $("#exampleViewKH").modal("hide");
    $("#modalSua").modal("show");
    $(".NguyenVatLieuThemClassEdit").remove();
    arrayThemNguyenVatLieuEdit = [];
    document.getElementById('ShowNVienGVEdit').innerHTML = "";
    $("#IdNguoiNhanEdit").val('');
    var url = "/KeHoachDatHangs/GetInfoKeHoachById/";
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
            msg.resultObj.danhSachDatHangs.forEach(function (item) {
                arrayThemNguyenVatLieuEdit.push(item.idNguyenVatLieu);
                var ghiChu = "";
                if (item.ghiChu == null) {
                    ghiChu = "";
                } else {
                    ghiChu = item.ghiChu;
                }
                var str = ' <tr class="NguyenVatLieuThemClassEdit" id="ThemCTKHEdit-' + item.idNguyenVatLieu + '">'
                    + '<td>' + item.maSoNguyenVatLieu + '</br> <small>Sản phẩm: ' + item.tenNguyenVatLieu + '</small></td>'
                    + '<td>'
                    + '<select id="DonViTinhThemEdit-' + item.idNguyenVatLieu + '" name="DonViTinh[]" class="form-control selectMater" required>'
                    + '</select>'
                    + '</td>'
                    + '<td>'
                    + ' <input type="number" min="0" class="form-control" name="SoLuong[]" id="SoLuongThemEdit-' + item.idNguyenVatLieu + '"'
                    + 'value="' + item.soLuong + '" placeholder = "Số lượng" required > '
                    + ' </td>'
                    + ' <td>'
                    + '   <input type="hidden" class="form-control" name="IdChiTiet[]" value="' + item.idChiTiet + '" />'
                    + '   <input type="hidden" class="form-control" name="IdnguyenVatLieu[]" value="' + item.idNguyenVatLieu + '" />'
                    + '   <input type="text" class="form-control" name="GhiChuDatHang[]" value="' + ghiChu + '" placeholder="Ghi chú">'
                    + ' </td>'
                    //+ ' <td id="showNCCEdit-' + item.idNguyenVatLieu + '">' + item.maSoNCC
                    //+ '   <input type="hidden" class="form-control" name="IdNhaCungCap[]" value="' + item.idNhaCungCap + '"  id="NCCEdit-' + item.idNguyenVatLieu + '" >'
                    //+ ' </td>'
                    + ' <td class="text-center py-0 align-middle">'
                    + ' <div class="btn-group btn-group-sm">'
                    + '    <a class="btn btn-primary" data-toggle="modal" data-target="#exampleViewChuyenDoiEdit"'
                    + '      onclick="GetViewChuyenDoiEdit(' + item.idNguyenVatLieu + ')" title="Xem chuyển đổi">'
                    + '      <i class="fas fa-eye"></i>'
                    + '  </a>'
                    + '  <a class="btn btn-danger"'
                    + '    onclick="GetDeleteCongThucUpdate(' + item.idChiTiet + ',' + item.idNguyenVatLieu + ')" title="Xóa">'
                    + '      <i class="fas fa-trash"></i>'
                    + '</a>'
                    + '</div>'
                    + '</td>'
                    + '</tr>';

                $("#tableAddruEdit").append(str);
                TimDonViTinhKHUpdate(item.idNguyenVatLieu, '' + item.donViTinh + '');
            });
        },
        error: function (req, status, error) {
            toastr.error('Xóa thất bại');
        }
    });
}

// tìm kiếm đơn vị tính show ra của cái cập nhật có sẵn
function TimDonViTinhKHUpdate(id, donViTinh) {
    var url = "/KeHoachDatHangs/GetDonViTinhKHDH/";
    var str = "#DonViTinhThemEdit-" + id;
    $(str).append(new Option(donViTinh, donViTinh));
    $.ajax({
        type: "GET",
        url: url,
        data: { idNVL: id },
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
function GetViewShowDSKHDH(id) {
    ShowModalView(id);
    document.getElementById("XoaKeHoachXem").style.display = "none";
    document.getElementById("SuaKeHoachXem").style.display = "none";
}