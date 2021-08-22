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

// xem kế hoạch
function GetView(id) {
    var url = "/KeHoachCheBiens/GetInfoKeHoachById/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
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

// nhận kế hoạch
function GetNhanKeHoach(id) {
    $("#idNhanKeHoach").val(id);
}

function ajaxNhanKeHoach() {
    var url = "/NhiemVuKeHoachCheBien/UpdateNhanKeHoach/";
    var id = $("#idNhanKeHoach").val();
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
            if (msg.isSuccessed) {
                $("#exampleNhanKeHoach").modal("hide");
                var str = "NhanKH-" + id;
                document.getElementById(str).innerHTML = "Đã nhận";
            } else {
                toastr.error(msg.message);
            }
        },
        error: function (req, status, error) {
            toastr.error('Nhận thất bại');
        }
    });
}