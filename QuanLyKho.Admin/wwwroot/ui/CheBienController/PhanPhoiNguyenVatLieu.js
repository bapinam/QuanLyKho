GetAllKeHoachCheBien(" ", " ");
function GetAllKeHoachCheBien(ngay, tukhoa) {
    var bundle =
    {
        Ngay: ngay,
        TuKhoa: tukhoa
    };
    $(".delete-kehoach").remove();
    $.ajax({
        type: "GET",
        url: "/CheBiens/GetAllKeHoachCheBienChuaHoanThanh",
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
                    + '              onclick="TaoPhieuCheBien(' + item.id + ',\'' + item.maSo + '\')" title="Tạo Phiếu Chế Biến">'
                    + '               <i class="fas fa-sitemap"></i>'
                    + '           </a>'
                    //+ '           <a class="btn btn-success" data-toggle="modal" data-target="#exampleSplitBills"'
                    //+ '               onclick="SplitBills(' + item.id + ')" title="Tách Kế Hoạch">'
                    //+ '                <i class="fas fa-scissors"></i>'
                    //+ '            </a>'
                    + '            <a class="btn btn-danger" data-toggle="modal" data-target="#HuyKeHoachCheBien"'
                    + '               onclick="HuyKeHoachCheBien(' + item.id + ')" title="Hủy Kế Hoạch">'
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
                var str = ' <tr class="delete-kehoach">'
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
    GetAllKeHoachCheBien(ngay, tukhoa);
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

function TaoPhieuCheBien(id, code) {
    arrayQuaTon = [];
    DeleteTableCrateNVL();
    $("#IdPlan").val(id);
    $("#CodeOrder").val(code);
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
                        + '<tr><th colspan="6" class="text-center">Mã Công Thức: ' + item.maCongThuc
                        + '<br />Mã Sản Phẩm: ' + item.maSanPham
                        + '<br />Tên Sản Phẩm: ' + item.tenSanPham
                        + '</th > <tr>'
                        + '    <tr>'
                        + '           <th>Mã Số</th>'
                        + '        <th>Tên</th>'
                        + '            <th>ĐVT</th>'
                        + '            <th>Số Lượng Cần</th>'
                        + '            <th>Số Lượng Tồn</th>'
                        + '            <th class="text-center">Tổng Phát</th>'
                        + '        </tr>'
                        + '    </thead>'
                        + '   <tbody id="tableAddNVLPP">';
                    var stt = 1;
                    item.danhSachNguyenVatLieus.forEach(function (i) {
                        var amountNew = i.soLuongHienCoNVL / i.giaTriChuyenDoi;
                        str = str + ' <tr>'
                            + '<td>' + i.maNguyenVatLieu + '</td>'
                            + ' <td>' + i.tenNguyenVatLieu + '</td>'
                            + '<td>' + i.donViTinh + '</td>'
                            + '<td id="SLCT-' + item.idChiTiet + '-' + stt + '">' + i.soLuongCanCT + '</td>'
                            + '<td id="SLNVL-' + item.idChiTiet + '-' + stt + '">' + amountNew + '</td>'
                            + '<input id="Value-' + item.idChiTiet + '-' + stt + '" type="hidden" value="' + i.giaTriChuyenDoi + '" />'
                            + '<td id="SLTP-' + item.idChiTiet + '-' + stt + '">0</td>'
                            + '</tr>';
                        stt++;
                    });

                    str = str + '<tr>'
                        + '   <td colspan="2" class="text-center"><label>Số Lượng Sản Phẩm</label></td>'
                        + '  <td>' + item.donViTinh + '</td>'
                        + '  <td colspan="3">'
                        + '<input type="hidden" name="IdCongThuc[]" value="' + item.idCongThuc + '" />'
                        + '<input type="hidden" name="DonViTinh[]" value="' + item.donViTinh + '" />'
                        + '<input type="hidden" name="IdChiTiet[]" value="' + item.idChiTiet + '" />'
                        + '<input type="number" min="0" max="' + max + '" class="form-control" name="SoLuong[]" id="AmountNumber-' + item.idChiTiet + '"'
                        + '  oninput="ClickSoLuong(' + item.idChiTiet + ',' + stt + ')"   required /> '
                        + '<small>Tối đa: ' + max + '</small>'
                        + '</td>'
                        + '</tr>';

                    str = str + '<tr><td colspan="6"></td><tr>';

                    str = str + ' </tbody>';
                    $("#DeleteTableNVLcc").append(str);
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

///< !--Tổng Phát số lượng-- >
var arrayQuaTon = [];
function ClickSoLuong(idProcess, stt) {
    ////SLCT-,SLNVL-,SLTP-
    for (var i = 1; i < stt; i++) {
        var tp = "SLTP-" + idProcess + "-" + i;
        var nvl = "SLNVL-" + idProcess + "-" + i;
        var ct = "SLCT-" + idProcess + "-" + i;
        var slCT = document.getElementById(ct).innerText;
        var slNVL = document.getElementById(nvl).innerText;
        var slA = "#AmountNumber-" + idProcess;
        var amount = $(slA).val();

        // var va = "#Value-" + idProcess + "-" + i;
        // var value = $(va).val();

        var result = slCT * amount;

        var hang = idProcess + "-" + i;
        if (result > slNVL) {
            document.getElementById(tp).innerHTML = "<span style='color: red;'>" + result + "</span>";
            if (arrayQuaTon.indexOf(hang) === -1) {
                arrayQuaTon.push(hang);
            }
        } else {
            document.getElementById(tp).innerHTML = result;
            if (arrayQuaTon.indexOf(hang) != -1) {
                var pos = arrayQuaTon.indexOf(hang);
                arrayQuaTon.splice(pos, 1);
            }
        }
    }
}

// hủy kết hoạch chế biến
function HuyKeHoachCheBien(id) {
    $(".HuyKHCheBienShow").remove();
    var url = "/CheBiens/GetAllDanhSachChebien";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            msg.forEach(function (item) {
                var str = '<tr class="HuyKHCheBienShow">'
                    + '<td>' + item.maSoSanPham + '</td>'
                    + '<td>' + item.tenSanPham + '</td>'
                    + '<td>' + item.maSoCongThuc + '</td>'
                    + '<td>' + item.soLuong + '</td>'
                    + '<td>' + item.soLuongDaThem + '</td>'
                    + '<td>' + item.donViTinh + '</td>'
                    + '<td class="text-center">'
                    + '<div class="btn-group btn-group-sm">'
                    + '          <a class="btn btn-danger" data-toggle="modal" data-target="#modalHoiHuyCheBien" id="HuyCBNone-' + item.idChiTiet + '"'
                    + '              onclick="HuyChitietCB(' + item.idChiTiet + ')" title="Hủy Chế Biến">'
                    + '               Hủy'
                    + '           </a>'
                    + '            <a class="btn btn-primary" href="#" style="display: none;" id="HuyCB-' + item.idChiTiet + '" title = "Đã Hủy" > '
                    + '              Đã Hủy'
                    + '            </a>'
                    + '        </div>'
                    + '</td>'
                    + ' </tr >';
                $("#ShowViewHuyKeHoachCheBien").append(str);
            });
        },
        error: function (req, status, error) {
        }
    });
}

function HuyChitietCB(id) {
    $("#idHoiHuyCheBien").val(id);
}

function ajaxHuyCheBien() {
    var id = $("#idHoiHuyCheBien").val();
    var url = "/CheBiens/HuyChiTietCheBien";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            var str = "HuyCB-" + id;
            toastr.success("Hủy thành công");
            if (msg.isSuccessed) {
                $("#modalHoiHuyCheBien").modal("hide");
                var str = "HuyCB-" + id;
                document.getElementById(str).style.display = "block";

                var strNone = "HuyCBNone-" + id;
                document.getElementById(strNone).style.display = "none";
            } else {
                toastr.error(msg.message);
            }
        },
        error: function (req, status, error) {
            toastr.error("Hủy thất bại");
        }
    });
}