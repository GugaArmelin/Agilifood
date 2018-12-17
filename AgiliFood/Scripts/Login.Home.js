$(document).ready(function () {
    $("#status").hide();
    $("#botao-entrar").click(function () {
            $.ajax({
                url: "/Usuario/AutenticacaoDeUsuario",
                data: {
                    Email: $("#txtEmail").val(),
                    Senha: $("#txtSenha").val()
                },
                dataType: "json",
                type: "GET",
                async: true,
                beforeSend: function () {
                    $("#status").html("Estamos autenticando o usuário. Só um instante...");
                $("#status").show();
                },
                success: function (dados) {
                    if (dados.OK) {
                        $("#status").html(dados.Mensagem)
                        setTimeout(function () {
                            window.location.href =
                            "/Usuario/Login"
                        }, 5000);
                        $("#status").show();
                    }
                    else {
                        $("#status").html(dados.Mensagem);
                        $("#status").show();
                    }
                },
                error: function () {
                    $("#status").html(dados.Mensagem);
                    $("#status").show()
                }
            });
    });
});