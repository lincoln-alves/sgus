function AutoCompleteDefine(preloadedList, txtAutocomplete) {
    AutoCompleteDefine(preloadedList, txtAutocomplete, false, false, false, false, null);
}

function AutoCompleteDefine(preloadedList, txtAutocomplete, autoPostBack, exibirTodos, exibirSelecione, callback) {
    $(function () {
        if (preloadedList === null || preloadedList === undefined)
            preloadedList = "";

        // Converte o controle original para hidden.
        $(txtAutocomplete).css("display", "none");

        // Criar controles ao redor do txtAutocomplete com o caret pro dropdown.
        var input = $('<input></input>');
        var id = $(txtAutocomplete).attr('id') + '_autocomplete';
        input.attr('id', id);
        input.addClass('form-control');
        input.attr('autocomplete', 'off');

        if (exibirTodos)
            input.attr('placeholder', '-- Todos --');

        if (exibirSelecione)
            input.attr('placeholder', '-- Selecione --');

        // Exibir mensagem caso não haja nenhum item na lista.
        if (preloadedList === "") {
            $(txtAutocomplete).val("");

            var mensagemVazia = $(txtAutocomplete).attr('data-mensagemVazia');

            if (mensagemVazia !== undefined && mensagemVazia !== null && mensagemVazia !== "") {
                input.attr('placeholder', mensagemVazia);
            }
        } else {
            if (preloadedList.length === 0) {
                input.attr('placeholder', 'Nenhum item a ser exibido');
            } else if (!exibirTodos && !exibirSelecione) {
                input.attr('placeholder', '-- Selecione --');
            }
        }
        // Preenche o texto caso haja selecionado.
        if ($(txtAutocomplete).val() !== "") {
            var element = preloadedList[preloadedList.map(function (e) { return e.data; }).indexOf($(txtAutocomplete).val())];

            if (element != null || element != undefined) {
                input.val(element.value);
            }
        }

        var container = $(txtAutocomplete).parent();

        input.addClass('autocomplete-input');

        var div = $('<div></div>');
        div.addClass('input-group');

        var spanCaret = $('<span></span>');
        spanCaret.addClass('input-group-addon autocomplete-caret-container');
        spanCaret.css("border-left", "none");
        spanCaret.on('click', function () { input.focus(); });
        var caret = $('<span></span>');
        caret.addClass('autocomplete-caret');
        spanCaret.append(caret);

        var spanLimpar = $('<span></span>');
        spanLimpar.addClass('input-group-addon autocomplete-caret-container');
        spanLimpar.on('click', function () {
            input.val('');
            $(txtAutocomplete).val('');

            if (autoPostBack) {
                MostarLoading();

                __doPostBack($(txtAutocomplete).attr('id'));
            } else {
                if (callback !== undefined) {
                    callback();
                }
            }

        });
        spanLimpar.css("border-left", "none");
        spanLimpar.css("border-right", "none");
        var btnLimpar = $('<span>Limpar</span>');
        btnLimpar.addClass('label label-theme');
        btnLimpar.css("opacity", "0.5");
        btnLimpar.css("border-radius", "10px");
        btnLimpar.on("mouseout", function () { btnLimpar.css("opacity", "0.5"); });
        btnLimpar.on("mouseover", function () { btnLimpar.css("opacity", "1"); });
        spanLimpar.append(btnLimpar);

        // Verificar se o elemento está desabilitado e desabilita o autocomplete.
        if (!$(txtAutocomplete).is(':disabled')) {
            div.append(input);
            div.append(spanLimpar);

            // Iniciar o Autocomplete caso existam itens a serem exibidos.
            if (preloadedList != null && preloadedList != undefined && preloadedList.length > 0) {
                $(input).autocomplete({
                    // serviceUrl: '/autosuggest/service/url',
                    noCache: true,
                    lookup: preloadedList,
                    lookupFilter: function (suggestion, originalQuery, queryLowerCase) {
                        if (suggestion == null || suggestion.value == null)
                            return true;

                        return new RegExp(queryLowerCase).test(suggestion.value.toLowerCase());
                    },
                    onSelect: function (suggestion) {
                        if (txtAutocomplete != null) {
                            $(txtAutocomplete).val(suggestion.data);

                            if (autoPostBack) {
                                MostarLoading();

                                __doPostBack($(txtAutocomplete).attr('id'));
                            }

                            if (callback !== undefined) {
                                callback();
                            }
                        }
                    },
                    onChange: function () {
                    },
                    orientation: 'auto'
                });

                // Atribuir o valor do texto em data attribute para o txt original para pesquisas que não irão usar o ID do autocomplete.
                $(input).on('keyup', function () {
                    $(txtAutocomplete).val($(this).val());
                });
            }
        } else {
            input.attr("disabled", "disabled");

            spanCaret.css("background-color", "#eeeeee");

            div.append(input);
        }

        div.append(spanCaret);
        container.append(div);
    });
}

function AutocompleteCombobox(ddl) {
    return AutocompleteCombobox(ddl, false, false);
}

function AutocompleteCombobox(ddl, autoPostBack, autoPostBackTodosOuSelecionar) {
    $(function () {
        $('.form-control').css({ 'width': '96%' });
    });
    Ext.onReady(function () {
        // ComboBox transformed from HTML select
        var _autoCompleteCombobox = Ext.create('Ext.form.field.ComboBox', {
            typeAhead: true,
            transform: ddl,
            forceSelection: true,
            listeners: {
                load: function (store, records) {
                    store.insert(0, [{
                        fullName: '&nbsp;',
                        id: null
                    }]);
                }
            },
            doQuery: function (queryString, forceAll) {
                this.expand();
                this.store.clearFilter(!forceAll);

                if (!forceAll) {
                    this.store.filter(this.displayField, new RegExp(Ext.String.escapeRegex(queryString), 'i'));
                }
            }
        });
        _autoCompleteCombobox.on('change', function () {
            if (autoPostBack) {
                if (_autoCompleteCombobox.value > 0 || _autoCompleteCombobox.value < 0) {
                    __doPostBack("ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder1$" + ddl);
                }
                else {
                    var obj = document.getElementById(_autoCompleteCombobox.id).getElementsByTagName('input');
                    var selectedText = obj[1].value;
                    if ((selectedText == '-- Todos --' || selectedText == '::Todos::') && autoPostBackTodosOuSelecionar) {
                        __doPostBack("ctl00$ctl00$ContentPlaceHolder1$ContentPlaceHolder1$" + ddl);
                    }
                }
            }
        });

        _autoCompleteCombobox.on('focus', function () {
            var obj = document.getElementById(_autoCompleteCombobox.id).getElementsByTagName('input')
            var selectedText = obj[1].value;
            if (selectedText == '-- Todos --' || selectedText == '::Todos::' || selectedText == '-- Selecione --' || selectedText == '::Selecione::') {
                obj[1].value = '';
            }
        });
    });
}