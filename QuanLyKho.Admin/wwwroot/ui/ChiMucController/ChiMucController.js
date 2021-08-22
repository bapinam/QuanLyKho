$(function () {
    $('#Date').daterangepicker({
        opens: 'left'
    }, function (start, end, label) {
        console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
    });
});
// Thêm input ngày nghỉ
var divNgay = 0;
function ThemNgayNghi() {
    str = '<div class="form-group" id = "divNgayNghi-' + divNgay + '">'
        + ' <input type="date" class="form-control" name="NgayNghi" style="width:80%; float:left;"'
        + 'id="NgayNhap-' + divNgay + '" onchange = "ChonNgay(' + divNgay + ')" required> '
        + '     <button type="button" class="btn btn-danger btn-sm" style="width: 15%; margin-top:2px; margin-left:2%"'
        + 'onclick = "XoaInputNgayNghi(' + divNgay + ')" > '
        + '         <i class="fas fa-minus-square"></i>'
        + '    </button>'
        + '<p id="LoiNgay-' + divNgay + '" style="color:red; float: right; display:none;">Ngày phải nằm trong khoảng đã chọn</p>'
        + '</div>';
    $("#ShowNgayNghi").append(str);
    divNgay++;
}

function ThemNgayNghiDatabase(id, date) {
    str = '<div class="form-group" id ="divNgayNghiDB-' + id + '">'
        + '<input type="hidden" name="IdNgayTuChon" value="' + id + '">'
        + ' <input type="date" class="form-control" style="width:80%; float:left;" name="NgayTuChonCapNhat" value="' + date + '"'
        + 'id="NgayNhapDB-' + id + '" onchange = "ChonNgayDB(' + id + ')" required> '
        + '     <button type="button" class="btn btn-danger btn-sm" style="width: 15%; margin-top:2px; margin-left:2%"'
        + 'onclick = "XoaInputNgayNghiDB(' + id + ')" > '
        + '         <i class="fas fa-minus-square"></i>'
        + '    </button>'
        + '<p id="LoiNgayDB-' + id + '" style="color:red; float: right; display:none;">Ngày phải nằm trong khoảng đã chọn</p>'
        + '</div>';
    $("#ShowNgayNghiEditDB").append(str);

    divNgay++;
}

var divNgayEdit = 0;
function ThemNgayNghiEdit() {
    str = '<div class="form-group" id = "divNgayNghiEdit-' + divNgayEdit + '">'
        + ' <input type="date" class="form-control" name="NgayNghiTuChon" style="width:80%; float:left;"'
        + 'id="NgayNhapEdit-' + divNgayEdit + '" onchange = "ChonNgayEdit(' + divNgayEdit + ')" required> '
        + '     <button type="button" class="btn btn-danger btn-sm" style="width: 15%; margin-top:2px; margin-left:2%"'
        + 'onclick = "XoaInputNgayNghiEdit(' + divNgayEdit + ')" > '
        + '         <i class="fas fa-minus-square"></i>'
        + '    </button>'
        + '<p id="LoiNgayEdit-' + divNgayEdit + '" style="color:red; float: right; display:none;">Ngày phải nằm trong khoảng đã chọn</p>'
        + '</div>';
    $("#ShowNgayNghiEdit").append(str);
    divNgayEdit++;
}
// xóa input ngày nghỉ
function XoaInputNgayNghi(ngay) {
    var xoa = "#divNgayNghi-" + ngay;
    $(xoa).remove();
}
function XoaInputNgayNghiEdit(ngay) {
    var xoa = "#divNgayNghiEdit-" + ngay;
    $(xoa).remove();
}
// Kiểm tra ngày
function ChonNgay(stt) {
    var date = $("#Date").val();
    var thang1 = date.substr(0, 2);
    var ngay1 = date.substr(3, 2);
    var nam1 = date.substr(6, 4);

    var ngayDayDu1 = nam1 + "-" + thang1 + "-" + ngay1;

    var ngay2 = date.substr(16, 2);
    var thang2 = date.substr(13, 2);
    var nam2 = date.substr(19, 4);
    var ngayDayDu2 = nam2 + "-" + thang2 + "-" + ngay2;

    var ngay = "#NgayNhap-" + stt;
    var ngayNhap = $(ngay).val();

    var x = new Date(ngayNhap);
    var y = new Date(ngayDayDu1.trim());
    var z = new Date(ngayDayDu2.trim());

    var str = "LoiNgay-" + stt;

    if (x < y) {
        document.getElementById("btnthem").style.display = "none";
        document.getElementById(str).style.display = "block";
    }
    else {
        if (x > z) {
            document.getElementById("btnthem").style.display = "none";
            document.getElementById(str).style.display = "block";
        } else {
            document.getElementById("btnthem").style.display = "block";
            document.getElementById(str).style.display = "none";
        }
    }
}

// Kiểm tra ngày
function ChonNgayDB(stt) {
    var date = $("#DateE").val();
    var thang1 = date.substr(0, 2);
    var ngay1 = date.substr(3, 2);
    var nam1 = date.substr(6, 4);

    var ngayDayDu1 = nam1 + "-" + thang1 + "-" + ngay1;

    var ngay2 = date.substr(16, 2);
    var thang2 = date.substr(13, 2);
    var nam2 = date.substr(19, 4);
    var ngayDayDu2 = nam2 + "-" + thang2 + "-" + ngay2;

    var ngay = "#NgayNhapDB-" + stt;
    var ngayNhap = $(ngay).val();

    var x = new Date(ngayNhap);
    var y = new Date(ngayDayDu1.trim());
    var z = new Date(ngayDayDu2.trim());

    var str = "LoiNgayDB-" + stt;

    if (x < y) {
        document.getElementById("btnedit").style.display = "none";
        document.getElementById(str).style.display = "block";
    }
    else {
        if (x > z) {
            document.getElementById("btnedit").style.display = "none";
            document.getElementById(str).style.display = "block";
        } else {
            document.getElementById("btnedit").style.display = "block";
            document.getElementById(str).style.display = "none";
        }
    }
}

// Kiểm tra ngày
function ChonNgayEdit(stt) {
    var date = $("#DateE").val();
    var thang1 = date.substr(0, 2);
    var ngay1 = date.substr(3, 2);
    var nam1 = date.substr(6, 4);

    var ngayDayDu1 = nam1 + "-" + thang1 + "-" + ngay1;

    var ngay2 = date.substr(16, 2);
    var thang2 = date.substr(13, 2);
    var nam2 = date.substr(19, 4);
    var ngayDayDu2 = nam2 + "-" + thang2 + "-" + ngay2;

    var ngay = "#NgayNhapEdit-" + stt;
    var ngayNhap = $(ngay).val();

    var x = new Date(ngayNhap);
    var y = new Date(ngayDayDu1.trim());
    var z = new Date(ngayDayDu2.trim());

    var str = "LoiNgayEdit-" + stt;

    if (x < y) {
        document.getElementById("btnedit").style.display = "none";
        document.getElementById(str).style.display = "block";
    }
    else {
        if (x > z) {
            document.getElementById("btnedit").style.display = "none";
            document.getElementById(str).style.display = "block";
        } else {
            document.getElementById("btnedit").style.display = "block";
            document.getElementById(str).style.display = "none";
        }
    }
}
// cập nhật tên chỉ mục
function CapNhatTen() {
    var date = $("#Date").val();
    var thang1 = date.substr(0, 2);
    var nam1 = date.substr(6, 4);

    var thang2 = date.substr(13, 2);
    var nam2 = date.substr(19, 4);
    var str;
    if (nam1 == nam2) {
        if (thang1 == thang2) {
            str = "Tháng " + thang1 + " - Năm " + nam1;
        } else {
            str = "Tháng " + thang1 + " - Năm " + nam1 + " đến Tháng " + thang2 + " - Năm " + nam2;
        }
    } else {
        str = "Tháng " + thang1 + " - Năm " + nam1 + " đến Tháng " + thang2 + " - Năm " + nam2;
    }

    $.ajax({
        type: "GET",
        url: "/ChiMucs/GetName",
        data: { ten: str },
        success: function (msg) {
            $("#Name").val(msg.resultObj);
            $("#NameHidden").val(msg.resultObj);
        },
        error: function (data) {
        }
    });
}

//xóa ngày nghỉ
function XoaInputNgayNghiDB(id) {
    $("#idDeleteNgayNghi").val(id);
    $("#exampleDeleteNgayNghi").modal("show");
}
function GetDelete(id) {
    $("#idDelete").val(id);
}

function SetCheckBoxEditFalse() {
    document.getElementById("customCheckboxEdit1").checked = false;
    document.getElementById("customCheckboxEdit2").checked = false;
    document.getElementById("customCheckboxEdit3").checked = false;
    document.getElementById("customCheckboxEdit4").checked = false;
    document.getElementById("customCheckboxEdit5").checked = false;
    document.getElementById("customCheckboxEdit6").checked = false;
    document.getElementById("customCheckboxEdit7").checked = false;
}

function SetCheckBox() {
    document.getElementById("customCheckbox1").checked = true;
    document.getElementById("customCheckbox2").checked = false;
    document.getElementById("customCheckbox3").checked = false;
    document.getElementById("customCheckbox4").checked = false;
    document.getElementById("customCheckbox5").checked = false;
    document.getElementById("customCheckbox6").checked = false;
    document.getElementById("customCheckbox7").checked = false;
}

