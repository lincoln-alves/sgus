jQuery.markAll = function (appendElement, markContainer, markText, unmarkText) {
	// Inicializar botão.
	var button = $('<button type="button"></button>');
	button.attr('data-container', markContainer);
	button.addClass('markall');

	// Caso todas as opções estejam marcadas, altera o contexto do botão para "Desmarcar todas".
	if ($('#' + markContainer).find('input:checkbox:not(:checked)').size() === 0) {
		button.attr('data-type', '2');
		button.text(unmarkText);
	} else {
		button.attr('data-type', '1');
		button.text(markText);
	}

	// Cria o evento do click do botão.
	button.on('click', function () {
		var marcar = true;

	    var btn = $(this);

	    if (btn.attr('data-type') === '1') {
	    	btn.attr('data-type', '2');
	    	btn.text(unmarkText);
		} else {
	    	btn.attr('data-type', '1');
	    	btn.text(markText);
			marcar = false;
		}

		// Marca ou desmarca os objetos de acordo.
	    $.each($('#' + this.dataset.container).find('input'), function () {
	        var check = $(this);

            // Não marca opções desativadas.
	        if (check.attr('disabled') == undefined)
	            check.prop('checked', marcar);
		});
	});

	// Insere o botão dentro um determinado elemento.
	$('#' + appendElement).append(button);
}