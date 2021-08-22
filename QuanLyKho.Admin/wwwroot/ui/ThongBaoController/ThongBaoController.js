var thongBao = document.getElementById('SoThongBao');
if (soTB == "") {
    soTB.innerHTML = "";
}
function GetShowThongBao(message) {
    var str = '<div class="dropdown-divider"></div>'
        + '<a href = "' + message.DuongDan + '" onclick="UpdateDaXem(' + message.Id + ')" class="dropdown-item">'
        + ' <i class="fas fa-briefcase mr-2" style="color:#FF3333"></i><small>' + message.Ten
        + '</small><span class="float-right text-muted text-sm"><small>Hôm nay</small></span>'
        + '</a>';
    $("#ShowThongBaoNhan").prepend(str);
    var soTB = document.getElementById('SoThongBao');
    var count = document.getElementById('CountThongBao');
    var textContent = soTB.textContent;
    var ketQua = Number(textContent) + 1;
    soTB.innerHTML = ketQua;
    count.innerHTML = ketQua + " tin chưa xem";

    var audio = new Audio('/img/amthanhthongbao.mp3');
    audio.play();
}

function UpdateDaXem(id) {
    var url = "/ThongBaos/UpdateDaXem/";
    $.ajax({
        type: "GET",
        url: url,
        data: { id: id },
        dataType: "json",
        success: function (msg) {
        },
        error: function (req, status, error) {
        }
    });
}
Get10TopThongBao();
function Get10TopThongBao() {
    var url = "/ThongBaos/GetTopThongBao/";
    $.ajax({
        type: "GET",
        url: url,
        dataType: "json",
        success: function (msg) {
            var i = 0;
            var color;
            msg.forEach(function (item) {
                var thoiGian;
                if (item.soNgayNhan > 0) {
                    thoiGian = item.soNgayNhan + " ngày trước";
                } else {
                    thoiGian = "Hôm nay";
                }

                if (item.xem == "Chưa xem") {
                    i++;
                    color = "#FF3333";
                } else {
                    color = "#000000";
                }
                var str = '<div class="dropdown-divider"></div>'
                    + '<a href = "' + item.duongDan + '" onclick="UpdateDaXem(' + item.id + ')" class="dropdown-item">'
                    + ' <i class="fas fa-briefcase mr-2" style="color:' + color + '"></i><small>' + item.ten
                    + '</small><span class="float-right text-muted text-sm"><small>' + thoiGian + '</small></span>'
                    + '</a>';
                $("#ShowThongBaoNhan").append(str);
            });
            if (i > 0) {
                var soTB = document.getElementById('SoThongBao');
                var count = document.getElementById('CountThongBao');
                soTB.innerHTML = i;
                count.innerHTML = i + " tin chưa xem";
            }
        },
        error: function (req, status, error) {
        }
    });
}