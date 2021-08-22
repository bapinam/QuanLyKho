// lấy thông tin loại sản phẩm tìm kiếm
TimKiemLoaiSanPham();
function TimKiemLoaiSanPham() {
    var url = "/LoaiSanPhams/GetTenLoaiSP/";
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#GroupType1").append(new Option(item.ten, item.id));
            });
        },
        error: function (req, status, error) {
        }
    });
}

//model công thức
function GetRecipe(id, admin) {
    $('#showRecipe').remove();
    $('#DeleShow').append('<div class="card-body pb-0" id="showRecipe"></div >');
    var url = "/CongThucs/GetListCongThuc/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                var note = "";
                if (item.note == null) { note = "" } else { note = item.ghiChu }
                var html = '<div class="row" style="float:left;" id="DeleElem-' + item.id + '">' +
                    '<div class="d-flex align-items-stretch flex-column" >' +
                    '<div class="card bg-light d-flex flex-fill">' +
                    ' <div class="card-header text-muted border-bottom-0">' +
                    item.maSo +
                    '       </div>' +
                    '<div class="card-body pt-0">' +
                    '<div class="row">' +
                    ' <div class="">' +
                    '   <h2 class="lead"><b>' + item.ten + '</b></h2>' +
                    '   <p class="text-muted text-sm"><b>Ghi Chú:</b> ' + note + '</p>' +
                    '  <p class="text-muted text-sm"><b>Thành phần:</b></p>' +
                    ' <ul class="ml-4 mb-0 fa-ul text-muted" id="thanhphan">';

                item.ingredientCongThucs.forEach(function (i) {
                    html = html + '<li>- ' + i.ten + ': ' + i.soLuong + ' ' + i.donViTinh + '</li>';
                });
                html = html + '</ul>' +
                    ' </div>' +
                    '</div>' +
                    '</div>' +
                    '  <div class="card-footer">';
                if (admin == 'True') {
   html = html + ' <div class="text-right">' +
                    '  <a class="btn btn-primary btn-sm" data-toggle="modal" data-target="#exampleEditRecipe"' +
                    '    onclick="GetEditRecipe(' + item.id + ',' + id + ')" title="Sửa">' +
                    '    <i class="fas fa-pencil-alt"></i>' +
                    ' </a>&nbsp;&nbsp;&nbsp;' +
                    ' <a class="btn btn-danger btn-sm" data-toggle="modal" data-target="#exampleDelete"' +
                    '     onclick="GetDelete(' + item.id + ',' + id + ')" title="Xóa">' +
                    '    <i class="fas fa-trash"></i>' +
                    ' </a>' +
                    '</div>';
                }
             

                html = html+    ' </div>' +
                    '</div>' +
                    '</div>' +
                    ' </div>';
                $('#showRecipe').append(html);
            });
        },
        error: function (req, status, error) {
        }
    });
}

// thêm công thức
function GetAdd(id) {
    $('#ThemCT').trigger("reset");
    $("#idSP").val(id);
    var my = document.getElementById("ViewModal");
    my.remove();

    leo = '<table class="table table-bordered table-striped" id="ViewModal">' +
        '<thead>' +
        '<tr>' +
        '  <td>Nguyên Vật Liệu</td>' +
        ' <td>ĐVT</td>' +
        '  <td>Số Lượng</td>' +
        '</tr>' +
        '          </thead>' +
        ' <tbody id="BangView">' +
        '</tbody>' +
        '</table >';
    $('#result12').append(leo);

    var my = document.getElementById("table2");
    my.remove();

    var lp = '<div id="table2">' +
        '<table id = "exampleAdd" class="table table-bordered table-striped">' +
        '  <thead>' +
        '    <tr>' +
        '         <th>Mã Số</th>' +
        '         <th>Tên</th>' +
        '          <th>Hình Ảnh</th>' +
        '          <th>Loại Sản Phẩm</th>' +
        '          <th>Thêm</th>' +
        '       </tr>' +
        '   </thead>' +
        '   <tbody id="ThemNL">' +
        '   </tbody>' +
        ' </table>' +
        ' </div >';
    $("#table1").append(lp);

    var my = document.getElementById("exampleWasAdd");
    my.remove();

    var mp = '<table id="exampleWasAdd" class="table table-bordered table-striped">' +
        '<thead>' +
        '<tr>' +
        ' <th>Mã Số</th>' +
        ' <th>Tên</th>' +
        ' <th>Hình Ảnh</th>' +
        ' <th>ĐVT</th>' +
        ' <th>Số Lượng</th>' +
        ' <th>Xóa</th>' +
        ' </tr>' +
        '  </thead>' +
        ' <tbody id="DaThemNL">' +
        '</tbody>' +
        '</table>';
    $("#DSDaThem").append(mp);
    fruits = [];
}

document.getElementById("showName").style.display = "none";

function getNameAdd() {
    var name = $('#Name').val();
    var idSP = $("#idSP").val();
    document.getElementById('btnthem').style.display = "block";
    var url = "/CongThucs/iTen/";
    $.ajax({
        type: "GET",
        url: url,
        data: { ten: name, idSP: idSP },
        dataType: "json",
        success: function (msg) {
            if (msg.isSuccessed) {
                document.getElementById("showName").style.display = "none";
                document.getElementById('btnthem').style.display = "block";
            } else {
                document.getElementById("showName").style.display = "block";
                document.getElementById('btnthem').style.display = "none";
            }
        },
        error: function (req, status, error) {
        }
    });
}

function add_element() {
    var re = document.getElementById("result12");
    $("#exampleNguyenLieu").modal("show");

    GetLoaiNVL(null);
    $(".selectMater").select2({ dropdownParent: "#exampleNguyenLieu" });
    $('.selectMater').select2({
        theme: 'bootstrap4'
    });

    $(".selectMater").select2({
        //here my options
    }).on("select2:opening",
        function () {
            $("#exampleNguyenLieu").removeAttr("tabindex", "-1");
        }).on("select2:close",
            function () {
                $("#exampleNguyenLieu").attr("tabindex", "-1");
            });
}

//< !--Lấy Loại nguyên vật liệu-- >
function GetLoaiNVL(group) {
    var my = document.getElementById("showGr2");
    my.remove();
    $("#showGr").append(
        '<div id="showGr2">' +
        '<select id="MaterialsType" name="LoaiNguyenVatLieu" class="form-control selectMater">' +
        '<option selected>Chọn loại nguyên vật liệu... </option>' +
        ' </select>' +
        '</div>');
    url = "/CongThucs/GetLoaiNguyenVatLieu/";
    $.ajax({
        type: "GET",
        url: url,
        data: { group: group },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#MaterialsType").append('<option value="' + item.id + '"> ' + item.maSo + ' - ' + item.ten + '</option>');
            });

            $('.selectMater').select2({
                theme: 'bootstrap4'
            });
        },
        error: function (req, status, error) {
        }
    });
}

function changeFunc() {
    var selectBox = document.getElementById("GroupTypeMaterials");
    var selectedValue = selectBox.options[selectBox.selectedIndex].value;
    GetLoaiNVL(selectedValue);

    $(".selectMater").select2({ dropdownParent: "#exampleNguyenLieu" });
    $('.selectMater').select2({
        theme: 'bootstrap4'
    });

    $(".selectMater").select2({
        //here my options
    }).on("select2:opening",
        function () {
            $("#exampleNguyenLieu").removeAttr("tabindex", "-1");
        }).on("select2:close",
            function () {
                $("#exampleNguyenLieu").attr("tabindex", "-1");
            });
}



function GetDeleteMaterials(id) {
    var des = fruits.indexOf(id);
    fruits.splice(des, 1);
    var de = "De-" + id;
    var my = document.getElementById(de);
    my.remove();
}

function GetListPack(id) {
    var url = "/CongThucs/GetListDonViTinhs/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (data) {
            data.forEach(function (item) {
                var op = '<option  value="' + item.ten + '">' + item.ten + '</option>';
                var str = "#Se-" + id;
                $(str).append(op);
            });
        }
    });
}

var frmMe = $('#FormMaterials');

frmMe.submit(function (e) {
    var my = document.getElementById("ViewModal");
    my.remove();

    leo = '<table class="table table-bordered table-striped" id="ViewModal">' +
        '<thead>' +
        '<tr>' +
        '  <td>Nguyên Vật Liệu</td>' +
        '  <td>Số Lượng</td>' +
        ' <td>ĐVT</td>' +
        '</tr>' +
        '          </thead>' +
        ' <tbody id="BangView">' +
        '</tbody>' +
        '</table >';
    $('#result12').append(leo);

    fruits.forEach(function (item) {
        var idTen = "#Na-" + item;
        var tenNVL = $(idTen).text();
        var idDVT = "#Se-" + item;
        var dvt = $(idDVT).val();

        var idSL = "#SL-" + item;
        var sl = $(idSL).val();

        htm = '<tr>' +
            '<td>' +
            '<input type="hidden" name="IdNguyenVatLieu[]" value="' + item + '" />'
            + tenNVL +
            '</td> ' +
            '<td>' +
            '<input type="hidden" name="SoLuong[]" value="' + sl + '" />'
            + sl + '</td>' +
            ' <td>' +
            '<input type="hidden" name="DonViTinh[]" value="' + dvt + '" />'
            + dvt + '</td>' +
            '</tr>';

        $('#BangView').append(htm);
    });

    $('#exampleNguyenLieu').modal('hide');

    e.preventDefault();
});

function GetEdit(id) {
    $("#IdProductCT").val(id);
    document.getElementById("ChangeCT").remove();

    var tl = '<select id="ChangeCT" name="IdCongThuc" class="form-control select2bs467"></select >';
    $("#CTShowDe").append(tl);
    var url = "/CongThucs/GetListCongThuc/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#ChangeCT").append(new Option(item.ten, item.id));
            });
        },
        error: function (req, status, error) {
        }
    });

    $(".select2bs467").select2({
        //here my options
    }).on("select2:opening",
        function () {
            $("#exampleEdit").removeAttr("tabindex", "-1");
        }).on("select2:close",
            function () {
                $("#exampleEdit").attr("tabindex", "-1");
            });
}

//< !--Chỉnh sửa-- >-------------------------------------------------------------------
var dataArr = [];
document.getElementById("showNameEdit").style.display = "none";
function GetEditRecipe(id, idSP) {
    XoaView();
    $("#idSP1").val(idSP);
    $("#idCT1").val(id);
    var url = "/CongThucs/GetListIdCongThuc/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            $("#CodeEdit").val(msg.resultObj.maSo);
            $("#NameEdit").val(msg.resultObj.ten);
            $("#NoteEdit").val(msg.resultObj.ghiChu);

            msg.resultObj.ingredientCongThucs.forEach(function (item) {
                var ht = ' <tr id="DeMaterials-' + item.id + '">' +
                    '<td>' + item.ten + '</td>' +
                    '<td>' + item.soLuong + '</td>' +
                    '<td>' + item.donViTinh + '</td>   ' +
                    '<td class="text-center py-0 align-middle">' +
                    '<a class="btn-sm btn-danger" onclick="GetIdDeleteMa(' + item.id + ')" title="Xóa">' +
                    ' <i class="fas fa-times"></i>' +
                    ' </a>' +
                    '</td>' +
                    ' </tr>';
                $("#BangVDaThem").append(ht);
                dataArr.push(1);
            });
        },
        error: function (req, status, error) {
        }
    });
}

function XoaView() {
    dataArr = [];
    $('#BangVDaThem').remove();
    $('#ViewDaThem').append('<tbody id="BangVDaThem"></tbody>');

    $('#BangView12').remove();
    $('#ViewModal12').append('<tbody id="BangView12"></tbody>');

    $('#ThemNL12').remove();
    $('#exampleAdd12').append('<tbody id="ThemNL12"></tbody>');

    fruits12 = [];
}

function XoaTimKiemEdit() {
    $('#ThemNL12').remove();
    $('#exampleAdd12').append('<tbody id="ThemNL12"></tbody>');
}


//++++++++++++++++++++++++++++++++++++++++++++++++++++++
function GetLoaiNVL1(group) {
    var my = document.getElementById("showGr21");
    my.remove();
    $("#showGr1").append(
        '<div id="showGr21">' +
        '<select id="MaterialsType1" name="LoaiNguyenVatLieu" class="form-control selectMater1">' +
        '<option selected>Chọn loại nguyên vật liệu... </option>' +
        ' </select>' +
        '</div>');
    var url = "/CongThucs/GetLoaiNguyenVatLieu/";
    $.ajax({
        type: "GET",
        url: url,
        data: { group: group },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                $("#MaterialsType1").append('<option value="' + item.id + '"> ' + item.maSo + ' - ' + item.ten + '</option>');
            });

            $('.selectMater1').select2({
                theme: 'bootstrap4'
            });
        },
        error: function (req, status, error) {
        }
    });
}

function changeFunc1() {
    var selectBox = document.getElementById("GroupTypeMaterials1");
    var selectedValue = selectBox.options[selectBox.selectedIndex].value;
    GetLoaiNVL1(selectedValue);

    $(".selectMater1").select2({ dropdownParent: "#exampleEditRecipe" });
    $('.selectMater1').select2({
        theme: 'bootstrap4'
    });

    $(".selectMater1").select2({
        //here my options
    }).on("select2:opening",
        function () {
            $("#exampleEditRecipe").removeAttr("tabindex", "-1");
        }).on("select2:close",
            function () {
                $("#exampleEditRecipe").attr("tabindex", "-1");
            });
}



function GetDeleteMaterials12(id) {
    var des = fruits.indexOf(id);
    fruits12.splice(des, 1);
    var de = "De1-" + id;
    var my = document.getElementById(de);
    my.remove();
}

function GetListPack12(id) {
    var url = "/CongThucs/GetListDonViTinhs/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (data) {
            data.forEach(function (item) {
                var op = '<option  value="' + item.ten + '">' + item.ten + '</option>';
                var str = "#Se1-" + id;
                $(str).append(op);
            });
        }
    });
}

function getNameAdd1() {
    var name = $('#NameEdit').val();
    var idSP = $("#idSP1").val();
    var id = $("#idCT1").val();

    document.getElementById('btnthem12').style.display = "block";
    var url = "/CongThucs/iTen/";
    $.ajax({
        type: "GET",
        url: url,
        data: { name: name, idSP: idSP, id: id },
        dataType: "json",
        success: function (msg) {
            if (msg.isSuccessed) {
                document.getElementById("showNameEdit").style.display = "none";
                document.getElementById('btnthem12').style.display = "block";
            } else {
                document.getElementById("showNameEdit").style.display = "block";
                document.getElementById('btnthem12').style.display = "none";
            }
        },
        error: function (req, status, error) {
        }
    });
}