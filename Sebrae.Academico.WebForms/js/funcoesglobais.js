function EhNumerico(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function EhNumericoOuVirgula(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        if (charCode != 44) //Considerar virgula
            return false;
    }
    return true;
}

function adicionarItensListBox(botao) {
    var container = $(botao).parents('.containerSeletorListBox');

    var listTodos = $(container).find('.list1');
    var listSelecionados = $(container).find('.list2');

    transferirItensListBoxes(listTodos, listSelecionados);

    guardarSelecaoItensListBox(listSelecionados, $(container).find('input[type="hidden"]'));
}

function removerItensListBox(botao) {
    var container = $(botao).parents('.containerSeletorListBox');

    var listTodos = $(container).find('.list1');
    var listSelecionados = $(container).find('.list2');

    transferirItensListBoxes(listSelecionados, listTodos);

    guardarSelecaoItensListBox(listSelecionados, $(container).find('input[type="hidden"]')); 
}
function adicionarTodos(botao) {
    var itens = $(botao).parent().parent().find('select')[0];

    if ($(itens).find('option').length) {
        $(itens)
            .find('option')
            .prop('selected', true);

        $(botao).parent().find('input')[0].click();
    } else {
        var selecionados = $(botao).parent().parent().find('select')[1];

        if ($(selecionados).find('option').length) {
            $(selecionados)
                .find('option')
                .prop('selected', true);

            $(botao).parent().find('input')[1].click();
        }
    }

}

function transferirItensListBoxes($listBoxDe, $listBoxPara) {
    var itens = $($listBoxDe).find('option:selected').removeAttr('selected');
    $($listBoxPara).append(itens);
}

function guardarSelecaoItensListBox($listBox, $campoHidden) {
    var itens = [];
    
    $($listBox).find('option').each(function(i, elem) {
        var valor = $(elem).attr('value');
        itens.push(valor);
    });

    $($campoHidden).val(itens);
}