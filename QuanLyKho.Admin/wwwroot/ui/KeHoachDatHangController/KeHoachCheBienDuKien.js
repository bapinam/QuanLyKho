$(function () {
    $('#tableKHCBDK').DataTable({
        "paging": false,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
    });
});

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
                    + ' <td class="text-center">'
                    + '<div class="icheck-primary">'
                    + '       <input type="checkbox" name="TaoKeHoachCheBien[]" value="' + item.id + '" class="checkbutton">'
                    + '           </div>'
                    + '       </td>'
                    + '<td>' + item.maSo + '</td>'
                    + '<td>' + item.ten + '</td>'
                    + '<td>' + item.ngayDuKienBatDau + '</td>'
                    + '<td>' + item.nguoiTao + '</td>'
                    + '<td>' + item.nhanKeHoach + '</td>'
                    + '</tr>';
                $("#CheBienDK").append(str);
            });
        },
        error: function (req, status, error) {
        }
    });
}