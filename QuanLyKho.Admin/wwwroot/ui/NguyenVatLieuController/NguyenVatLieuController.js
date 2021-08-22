// Lấy giá trị của mã nguyên vật liệu
function GetNguyenVatLieu(typeMVL) {
    var url = "/QuanLyMaSos/GetAll/"
    $.ajax({
        type: "GET",
        url: url,
        data: { type: typeMVL },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                //Code
                str = '<option value="' + item.ten + '">' + item.ten + '</option>';
                $('#Code').append(str);
            });
        },
        error: function (req, status, error) {
        }
    });
}

//Thêm đóng gói
var t = 0;
function add_element() {
    var str = document.getElementById("Default").value;
    if (!str.trim() || 0 === str.length) {
        toastr.error('Chưa nhập tên đơn vị tính (cơ bản)')
    }
    else {
        t++;
        var html = '<div id="userdata' + t + '" > ' +
            '<div class="form-row">' +
            '<div class="form-group col-md-6">' +
            '<label for="inputEmail4">Tên Đơn Vị Tính</label> ' +
            '<input type="text" class="form-control" name="TenDVT[]" placeholder="Tên đơn vị tính" required> ' +
            '</div> ' +
            '<div class="form-group col-md-6"> ' +
            ' <label for="inputEmail4">Giá Trị Chuyển Đổi</label> ' +
            ' <div style="width:90%; float:left;"> ' +
            ' <input type="number" class="form-control" min="0" max="999999999999" name="GiaTriDVT[]" placeholder="Giá trị chuyển đổi" required> ' +
            ' </div> ' +
            ' <div style="width:10%; float:left;"> ' +
            ' <button onclick="removeE(' + t + ')" class="btn btn-danger btn-sm" style="margin-top:3px; margin-left:5px;" data-toggle="tooltip" data-placement="top" title="Xóa"> ' +
            ' <i class="fa fa-minus-square"></i> ' +
            ' </button>' +
            ' </div>' +
            ' </div>' +
            ' </div>' +
            ' </div>';

        $("#result12").append(html);
        var s = "ShowPack" + t;
        document.getElementById(s).style.display = "none";
    }
}

function removeE(t) {
    var str = "userdata" + t;
    var myobj = document.getElementById(str);
    myobj.remove();
}

document.getElementById("showName").style.display = "none";
document.getElementById("content").style.display = "block";
document.getElementById("ShowMin").style.display = 'none';
document.getElementById("ShowMax").style.display = 'none';
document.getElementById("ShowMinMax").style.display = 'none';

// mở/tắt nhắc nhở
function OnNhacNho() {
    document.getElementById("content").style.display = "block";
    document.getElementById("ShowMin").style.display = 'none';
    document.getElementById("ShowMax").style.display = 'none';
    document.getElementById("ShowMinMax").style.display = 'none';
}

function OffNhacNho() {
    document.getElementById("content").style.display = "none";
    document.getElementById("ShowMin").style.display = 'none';
    document.getElementById("ShowMax").style.display = 'none';
    document.getElementById("ShowMinMax").style.display = 'none';
}

// hình ảnh
var loadFile = function (event) {
    var output = document.getElementById('output');
    output.src = URL.createObjectURL(event.target.files[0]);
    output.onload = function () {
        URL.revokeObjectURL(output.src) // free memory
    }
};

var loadFileEdit = function (event) {
    var hinhEdit = document.getElementById('hinhEdit');
    hinhEdit.src = URL.createObjectURL(event.target.files[0]);
    hinhEdit.onload = function () {
        URL.revokeObjectURL(hinhEdit.src) // free memory
    }
};
//Lấy thông tin loại nguyên vật liệu- >
getLoaiNguyenVatLieu();
function getLoaiNguyenVatLieu() {
    var url = "/LoaiNguyenVatLieus/GetTenLoaiNVL/";
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#GroupType").append(new Option(item.ten, item.id));
                $("#GroupType1").append(new Option(item.ten, item.id));
            });
        },
        error: function (req, status, error) {
        }
    });
}

function getId(id) {
    var url = "/LoaiNguyenVatLieus/GetTenLoaiNVL/";
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                if (item.id != id) {
                    $("#GroupTypeE").append('<option value="' + item.id + '">' + item.ten + '</option>');
                }
            });
        },
        error: function (req, status, error) {
        }
    });
}

//// kiểm tra trước khi thêm nguyên vật liệu
function KiemTraThem() {
    //kiểm tra tên đơn vị tính có trùng nhau không?
    var tenDVT = [];
    var input = document.getElementsByName('TenDVT[]');

    for (var i = 0; i < input.length; i++) {
        var a = input[i];
        tenDVT.push(a.value);
    }
    var ketQua = 0;
    if (tenDVT.length > 0) {
        var cb = $("#Default").val();
        tenDVT.push(cb);
        ketQua = count(tenDVT);
    }

    if (ketQua == 1) {
        toastr.error('Các tên đơn vị tính không được trùng nhau');
        return 1;
    }
    // kiểm tra nhắc nhở
    var nhacNho = $("input[type='radio'].radioBtnClass:checked").val();
    if (nhacNho == "true") {
        var min = $("#Min").val();
        var max = $("#Max").val();
        if (min.length < 1) {
            toastr.error('Vui lòng nhập mức tồn tối thiểu');
            return 1;
        }

        if (max.length < 1) {
            toastr.error('Vui lòng nhập mức tồn tối đa');
            return 1;
        }
        var be = Number(min);
        var lon = Number(max);
        if (be >= lon) {
            toastr.error('Mức tồn tối thiểu phải bé hơn mức tồn tối đa');
            return 1;
        }
    }

    return 0;
}

// kiểm tra trùng lập trong mảng
function count(array_elements) {
    array_elements.sort();

    var current = null;
    var cnt = 0;
    for (var i = 0; i < array_elements.length; i++) {
        if (array_elements[i] != current) {
            if (cnt > 1) {
                return 1;
            }

            current = array_elements[i];
            cnt = 1;
        } else {
            cnt++;
        }
    }
    if (cnt > 1) {
        return 1;
    }

    return 0;
}

// xóa nguyên vật liệu
function GetDelete(id) {
    $("#idDelete").val(id);
}

// kiểm tra tên thêm
var showAdd = document.getElementById("showName");
showAdd.style.display = 'none';
function getNameAdd() {
    var val = document.getElementById("Name").value;
    var btn = document.getElementById("btnthem")
    var url = "/NguyenVatLieus/iTen/";
    $.ajax({
        type: "GET",
        url: url,
        data: { ten: val },
        dataType: "json",
        success: function (msg) {
            if (msg.isSuccessed) {
                showAdd.style.display = 'none';
                btn.style.display = 'block';
            } else {
                showAdd.style.display = 'block';
                btn.style.display = 'none';
            }
        },
        error: function (req, status, error) {
        }
    });
}

// thêm phần tử vào table sau khi tạo thành công
function ThemPhanTu(msg, anh) {
    var html = '<tr id="DeI-' + msg.resultObj.id + '">' +
        '<td id ="Code-' + msg.resultObj.id + '">'
        + msg.resultObj.maSo +
        '</td>' +
        '<td id="Name-' + msg.resultObj.id + '">'
        + msg.resultObj.ten +
        '</td>' +
        '<td id="Image-' + msg.resultObj.id + '">' +
        '<div id="UpdateImage-' + msg.resultObj.id + '">' +
        '   <img src="' + anh + '" style="width:50px; height:50px;" />' +
        '<div />' +
        '</td>' +
        '<td id="Amount-' + msg.resultObj.id + '">' +
        ' <div class="btn-group btn-group-sm">' +
        ' <button type="button" class="btn btn-default">' +
        msg.resultObj.soLuong +
        ' </button>' +
        ' <button type="button" class="btn btn-default dropdown-toggle dropdown-icon" data-toggle="dropdown">' +
        '<span class="sr-only">Toggle Dropdown</span>' +
        ' </button>' +
        '  <div class="dropdown-menu" role="menu">' +
        ' <table class="table table-bordered">' +
        '  <tbody>' +
        '  <tr>' +
        '  <td>' +
        ' <div class="progress progress-xs">' +
        '  <div class="progress-bar progress-bar-danger" style="width: ' + msg.resultObj.phanTramSoLuong + '%"></div>' +
        ' </div>' +
        ' </td>' +
        ' <td style="width: 40px;"><span class="badge bg-danger">' + msg.resultObj.phanTramSoLuong + '%</span></td>' +
        '</tr>' +
        ' </tbody>' +
        ' </table>' +
        '</div>' +
        '</div> ' +
        ' </td> ' +
        '<td id = "Pack-' + msg.resultObj.id + '" > ' +
        '<div class="btn-group btn-group-sm" > ' +
        ' <button type = "button" class="btn btn-warning" id="TextPack-' + msg.resultObj.id + '"> ' +
        msg.resultObj.tenDVTCoBan +
        ' </button> ' +
        '<button type="button" class="btn btn-warning dropdown-toggle dropdown-icon" data-toggle="dropdown">' +
        ' <span class="sr-only">Toggle Dropdown</span>' +
        '</button>' +
        ' <div class="dropdown-menu" role="menu">' +
        '     <a class="dropdown-item" data-toggle="modal" data-target="#examplePack"' +
        'onclick="GetPack(' + msg.resultObj.id + ')">Thao Tác</a>' +
        '  </div>' +
        '</div>' +
        ' </td>' +
        ' <td id="Reminder-' + msg.resultObj.id + '">';

    if (msg.resultObj.nhacNho) {
        html = html + '<div id="Re-' + msg.resultObj.id + '" class="btn-group btn-group-sm">' +
            ' <button type="button" class="btn btn-success">Bật</button>' +
            '<button type="button" class="btn btn-success dropdown-toggle dropdown-icon" data-toggle="dropdown">' +
            '  <span class="sr-only">Toggle Dropdown</span>' +
            ' </button>' +
            ' <div class="dropdown-menu" role="menu">' +
            '  <a class="dropdown-item" data-toggle="modal" data-target="#exampleNhacNho"' +
            ' onclick="GetNhacNho(' + msg.resultObj.id + ')">Thao Tác</a>' + ' </div>' +
            ' </div>'
    }
    else {
        html = html + '<div id="Re-' + msg.resultObj.id + '" class="btn-group btn-group-sm">' +
            ' <button type="button" class="btn btn-danger">Tắt</button>' +
            '<button type="button" class="btn btn-danger dropdown-toggle dropdown-icon" data-toggle="dropdown">' +
            ' <span class="sr-only">Toggle Dropdown</span>' +
            '</button>' +
            '<div class="dropdown-menu" role="menu">' +
            '  <a class="dropdown-item" data-toggle="modal" data-target="#exampleNhacNho"' +
            ' onclick="GetNhacNho(' + msg.resultObj.id + ')">Thao Tác</a>' +
            ' </div>' +
            '</div>'
    }

    html = html + ' </td>' +
        ' <td id="MaterialType-' + msg.resultObj.id + '">' +
        msg.resultObj.tenLoaiNVL +
        ' </td>' +
        ' <td class="text-center py-0 align-middle">' +
        '<div class="btn-group btn-group-sm">' +
        ' <a class="btn btn-success" data-toggle="modal" data-target="#exampleView"' +
        '   onclick="GetView(' + msg.resultObj.id + ')" title="Xem">' +
        '    <i class="fas fa-eye"></i>' +
        '  </a>' +
        ' <a class="btn btn-primary" data-toggle="modal" data-target="#exampleEdit"' +
        '    onclick="GetEdit(' + msg.resultObj.id + ')" title="Sửa">' +
        '     <i class="fas fa-pencil-alt"></i>' +
        '  </a>' +
        ' <a class="btn btn-danger" data-toggle="modal" data-target="#exampleDelete"' +
        '  onclick="GetDelete(' + msg.resultObj.id + ')" title="Xóa">' +
        '  <i class="fas fa-trash"></i>' +
        '  </a>' +
        ' </div>' +
        '</td>' +
        '</tr>';

    $("#addTR").prepend(html);
}

// kiểm tra tên edit
var show = document.getElementById("showNameE");
show.style.display = 'none';
function getName() {
    var val = document.getElementById("NameE").value;
    var btn = document.getElementById("btnthemE")
    var idE = document.getElementById("IdE").value;
    var url = "/NguyenVatLieus/iTen/";
    $.ajax({
        type: "GET",
        url: url,
        data: { ten: val, id: idE },
        dataType: "json",
        success: function (msg) {
            if (msg.isSuccessed) {
                show.style.display = 'none';
                btn.style.display = 'block';

            } else {
                show.style.display = 'block';
                btn.style.display = 'none';
            }
        },
        error: function (req, status, error) {

        }
    });

}

