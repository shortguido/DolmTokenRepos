$(document).ready(() => {
    $("#email").blur(() => {
        $.ajax({
            url: "/user/checkMail",
            method: "GET",
            data: { email: $("#email").val() }
        })
            .done((dataFromServer) => {
                if (dataFromServer === true) {
                    $("#mailMessage").css("visibility", "visible");
                    $("#email").addClass("redBorder");
                } else {
                    $("#mailMessage").css("visibility", "hidden");
                    $("#email").removeClass("redBorder");
                }
            })
            .fail(() => {
                alert("Server/URL nicht erreichbar");
            });
    });
});