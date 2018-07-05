var ordemData = new Array();

$(function () {
    $(".EtapasContainer").sortable({
        items: "> li:not(:first)",
        stop: function (event, ui) {
            ordemData = new Array();
            $(".EtapasContainer li").each(function (i) {
                if (i == 0) {
                    $(this).find(".ordem").html("Inscrição");
                } else {
                    $(this).find(".ordem").html((i) + "º Etapa");
                }
                var guid = $(this).attr("guid");
                var ordemBanco = $(this).data("ordem");
                ordemData.push(
                    { id: guid, ordem: i, ordemBanco: ordemBanco }
                 );
            });

            $(".ordenacaoHidden input").val(JSON.stringify(ordemData));
        }
    });

    $(".EtapasContainer").disableSelection();
});