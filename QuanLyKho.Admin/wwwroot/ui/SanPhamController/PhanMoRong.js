//cập nhật đóng gói___-----------------------------------------
//ẩn
document.getElementById("SaveDG").style.display = "none";
document.getElementById("PackDefaultDG").style.display = "none";
document.getElementById("ShowEnterT").style.display = "none";
document.getElementById("ShowTenDongGoi").style.display = "none";

function GetEditDG() {
    document.getElementById("EditDG").style.display = "none";
    document.getElementById("PackShowDG").style.display = "none";

    document.getElementById("SaveDG").style.display = "block";
    document.getElementById("PackDefaultDG").style.display = "block";
}

document.getElementById("FormAddDongGoi").style.display = "none";
document.getElementById("ShowTenDongGoi").style.display = "none";

function MoRong() {
    $('#FormAddDongGoi').trigger("reset");
    document.getElementById("btn-dg").style.display = "block";
    document.getElementById("FormAddDongGoi").style.display = "block";
    document.getElementById("ShowTenDongGoi").style.display = "none";
}

function ThuGon() {
    $('#FormAddDongGoi').trigger("reset");
    document.getElementById("FormAddDongGoi").style.display = "none";
    document.getElementById("ShowTenDongGoi").style.display = "none";
    document.getElementById("btn-dg").style.display = "block";
}

// hiển thị đơn vị tính đã thêm
function GetDonViTinh(id, loaiDongGoi) {
    $('#FormAddDongGoi').trigger("reset");
    document.getElementById("btn-dg").style.display = "block";
    document.getElementById("ShowTenDongGoi").style.display = "none";
    document.getElementById("FormAddDongGoi").style.display = "none";

    document.getElementById("EditDG").style.display = "block";
    document.getElementById("PackShowDG").style.display = "block";

    document.getElementById("SaveDG").style.display = "none";
    document.getElementById("PackDefaultDG").style.display = "none";
    document.getElementById("ShowEnterT").style.display = "none";

    var myobj = document.getElementById("dgTR");
    myobj.remove();

    var h = '<tbody id="dgTR"></tbody>';
    $("#tableDG").append(h);

    $('#IdDG').val(id);
    var url = "/DonViTinhs/GetDonViTinh/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id, loaiDongGoi: loaiDongGoi },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                if (item.coBan) {
                    $('#PackShowDG').val(item.ten);
                    $('#PackDefaultDG').val(item.ten);
                    $('#idDVTCB').val(item.id);
                    $('#idSPDVT').val(item.idSP);
                } else {
                    var str = '<tr id="PackEdit-' + item.id + '">' +
                        '<td id = "Name-' + item.id + '" >' +
                        ' <div class="form-group"> ' +
                        '  <label for="inputEmail4">Tên Đơn Vị Tính</label>' +
                        '<input type="hidden" id="DG-' + item.id + '" value="' + item.id + '" />' +
                        '    <input type="text" class="form-control" id="NameP' + item.id + '" onkeyup="CheckNameDGTuChon(' + item.id + ')" ' +
                        'placeholder="Tên đơn vị tính" value="' + item.ten + '"  required>' +
                        '<span class="text-danger" id="ShowTenDVTTC-' + item.id + '" style="display:none;">Tên đơn vị tính đã tồn tại</span>' +
                        '   </div>' +
                        ' </td>' +
                        '   <td id="Value-' + item.id + '">' +
                        '       <div class="form-group">' +
                        '            <label for="inputEmail4">Giá Trị Chuyển Đổi</label>' +
                        '            <input type="text" class="form-control" id="ValueP' + item.id + '" ' +
                        'value="' + item.giaTriChuyenDoi + '" placeholder="Giá trị chuyển đổi" required>' +
                        '        </div>' +
                        '     </td>' +
                        '     <td class="text-center py-0 align-middle">' +
                        '        <div class="btn-group btn-group-sm">' +
                        '             <a class="btn btn-success" ' +
                        '                onclick="GetEditDG1(' + item.id + ')" title="Sửa" id="luuSuaDVT">' +
                        '                 <i class="fas fa-check"></i>' +
                        '            </a>' +
                        '            <a class="btn btn-danger" ' +
                        '              onclick="GetDeleteDG(' + item.id + ')" title="Xóa">' +
                        '               <i class="fas fa-trash"></i>' +
                        '           </a>' +
                        '       </div>' +
                        '   </td>' +
                        ' </tr>';
                    $("#dgTR").append(str);
                }
            });
        },
        error: function (req, status, error) {
        }
    });
}
// kiểm tra sự tồn tại của đóng gói khi thêm
function CheckNameDongGoiThem() {
    var idSPDVT = $('#IdDG').val();
    var ten = $('#NameDGShowThem').val();

    var url = "/DonViTinhs/iTen/";
    $.ajax({
        type: "GET",
        url: url,
        data: { ten: ten, id: idSPDVT, idDVT: null, loaiDongGoi: true },
        dataType: "json",
        success: function (msg) {
            if (!msg.isSuccessed) {
                document.getElementById("ShowTenDongGoi").style.display = 'none';
                document.getElementById("btn-dg").style.display = 'block';
            } else {
                document.getElementById("ShowTenDongGoi").style.display = 'block';
                document.getElementById("btn-dg").style.display = 'none';
            }
        },
        error: function (req, status, error) {
        }
    });
}
// kiểm tra sự tồn tại của đóng gói
function CheckNameDG() {
    var idDVT = $('#idDVTCB').val();
    var idSPDVT = $('#idSPDVT').val();
    var val = document.getElementById("PackDefaultDG").value;
    var btnDG = document.getElementById("SaveDG");
    var enter = document.getElementById("ShowEnterT");
    var url = "/DonViTinhs/iTen/";
    $.ajax({
        type: "GET",
        url: url,
        data: { ten: val, id: idSPDVT, idDVT: idDVT, loaiDongGoi: true },
        dataType: "json",
        success: function (msg) {
            if (!msg.isSuccessed) {
                enter.style.display = 'none';
                btnDG.style.display = 'block';
            } else {
                enter.style.display = 'block';
                btnDG.style.display = 'none';
            }
        },
        error: function (req, status, error) {
        }
    });
}

// kiểm tra sự tồn tại của đóng gói
function CheckNameDGTuChon(idDVT) {
    var na = '#NameP' + idDVT;
    var te = $(na).val();
    var idLoaiE = $('#IdDG').val();
    var tenStr = "ShowTenDVTTC-" + idDVT;
    var url = "/DonViTinhs/iTen/";
    $.ajax({
        type: "GET",
        url: url,
        data: { ten: te, id: idLoaiE, idDVT: idDVT, loaiDongGoi: true },
        dataType: "json",
        success: function (msg) {
            if (!msg.isSuccessed) {
                document.getElementById(tenStr).style.display = "none";
                document.getElementById("luuSuaDVT").style.display = "block";
            } else {
                document.getElementById(tenStr).style.display = "block";
                document.getElementById("luuSuaDVT").style.display = "none";
            }
        },
        error: function (req, status, error) {
        }
    });
}

// xóa đóng gói
function GetDeleteDG(id) {
    $('#idDeDG').val(id);
    $('#dgDelete').modal('show');
}

//____________________________________________________________________________________
// nhắc nhở
function OnReUpdate() {
    document.getElementById("showNhacNhoUpdate").style.display = "block";
}

function OffReUpate() {
    document.getElementById("showNhacNhoUpdate").style.display = "none";
}
function KiemTraNhacNhoUpdate() {
    // kiểm tra nhắc nhở
    var nhacNho = $("input[type='radio'].updateNhacNho:checked").val();
    if (nhacNho == "true") {
        var min = $("#MinTUpdate").val();
        var max = $("#MaxTUpdate").val();
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