var ordemData = new Array();
var ordemALternativasData = new Array();

$(function () {
    //ORDENAÇÃO DE CAMPOS
    $(".CamposContainer").sortable({
        stop: function (event, ui) {
            ordemData = new Array();
            $(".CamposContainer li").each(function (i) {
                $(this).find(".ordem").html((i + 1) + "º");
                var guid = $(this).attr("guid");
                ordemData.push(
                    { id: guid, ordem: i }
                 );
            });

            $(".ordenacaoCamposHidden input").val(JSON.stringify(ordemData));
        }
    });

    $(".CamposContainer").disableSelection();

    //ORDENAÇÃO DE ALTERNATIVAS
    $(".AlternativasContainer").sortable({
        stop: function (event, ui) {
            ordemData = new Array();
            $(".AlternativasContainer li").each(function (i) {
                $(this).find(".ordem").html((i + 1) + "º");
                var guid = $(this).attr("guid");
                ordemData.push(
                    { id: guid, ordem: i }
                 );
            });
            console.log(ordemData);
            $(".ordenacaoAlternativasHidden input").val(JSON.stringify(ordemData));
        }
    });

    $(".AlternativasContainer").disableSelection();
});