$(function () {
    $(".TrilhaTutorialContainer").sortable({
        items: "li",
        stop: function (event, ui) {
            var ordemData = new Array(),
                lastOrderByCategory = {};

            $(".TrilhaTutorialContainer li").each(function (i) {
                var guid = $(this).attr("guid"),
                    category = $(this).attr("category");

                if (!ordemData.hasOwnProperty(category)) {
                    ordemData[category] = new Array();
                    lastOrderByCategory[category] = 0;
                }                
                ordemData[category].push(
                    { id: guid, ordem: lastOrderByCategory[category] }
                );
                lastOrderByCategory[category]++;
            });
            console.log(ordemData);
            $(".ordenacaoHidden input").val(JSON.stringify(ordemData));
        }
    });

    $(".TrilhaTutorialContainer").disableSelection();
});