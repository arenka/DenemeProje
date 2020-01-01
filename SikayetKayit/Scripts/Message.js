$(function () {
    $('#modalDelete').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget);
        var sikayetid = btn.data("sikayet-id");

        $('#modalDelete_Body').load("/Home/Delete/" + sikayetid);
    })
});

$(function () {
    $('#modalDetails').on('show.bs.modal', function (e) {
        var btn = $(e.relatedTarget);
        var sikayetid = btn.data("sikayet-id");

        $('#modalDetails_Body').load("/Home/SikayetDetay/" + sikayetid);
    })
});

function doComment(btn, e, sikayetid, spanid) {

    var button = $(btn);
    var mode = button.data("edit-mode");

    if (e === "edit_clicked") {
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");

            $(spanid).addClass("editable");
            $(spanid).attr("contenteditable", true);
            $(spanid).focus();

        }
        else {
            button.data("edit-mode", false);
            button.removeClass("btn-success");
            button.addClass("btn-warning");

            $(spanid).removeClass("editable");
            $(spanid).attr("contenteditable", false);

            var txt = $(spanid).text();

            $.ajax({
                method: "POST",
                url: "/Home/Edit/" + sikayetid,
                data: { text: txt }
            }).done(function (data) {

                if (data.result) {

                    $('#modalDetails_Body').load("/Home/SikayetDetay/" + sikayetid);

                }
                else {
                    alert("Yorum Güncellenemedi")
                }

            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı")
            });
        }


    }
    else if (e === "delete_clicked") {
        $.ajax({
            method: "POST",
            url: "/Home/Delete/" + sikayetid
        }).done(function (data) {

            if (data.result) {

                $('#modalDetails_Body').load("/Home/List");
            }
            else {
                alert("Yorum Silinemedi");
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı");
        });
    }
}