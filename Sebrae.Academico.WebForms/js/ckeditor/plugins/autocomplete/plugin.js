CKEDITOR.plugins.add('autocomplete',
{
    init: function (editor) {

        var autocompleteCommand = editor.addCommand('autocomplete', {
            exec: function (editor) {
                var dummyElement = editor.document.createElement('span');
                editor.insertElement(dummyElement);
                
                var x = 0;
                var y = 0;

                var obj = dummyElement.$;

                while (obj.offsetParent) {
                    x += obj.offsetLeft;
                    y += obj.offsetTop;
                    obj = obj.offsetParent;
                }
                x += obj.offsetLeft;
                y += obj.offsetTop;

                // Compensando o SCROLL do editor
                if (editor.window.$.scrollY) {
                    y -= editor.window.$.scrollY;
                }

                dummyElement.remove();
                editor.contextMenu.show(editor.document.getBody(), null, x, y);
            }
        });
    },
    afterInit: function (editor) {

        editor.ui.addButton('autocomplete',
        {
            icon: null
        });


        editor.on('key', function (evt) {
            if (evt.data.keyCode == CKEDITOR.SHIFT + 51) {
                editor.execCommand('autocomplete');
            }
        });
        
        var firstExecution = true;
        var dataElement = {};

        editor.addCommand('reloadSuggestionBox', {
            exec: function (editor, param) {
                if (editor.contextMenu) {
                    dataElement = {};
                    editor.addMenuGroup('suggestionBoxGroup');

                    var sugs = [];

                    // Se for passado um padrâmetro quer dizer que existem múltiplas sugestões assim o Suggestions deve ser carregado com base nisso
                    if (typeof param != undefined && typeof (Suggestions) == "object" && Suggestions[param]) {
                        sugs = Suggestions[param];
                    } else if (Suggestions) {
                        sugs = Suggestions;
                    }

                    $.each(sugs, function (i, s) {
                        var suggestionBoxItem = "suggestionBoxItem" + i;
                        dataElement[suggestionBoxItem] = CKEDITOR.TRISTATE_OFF;
                        editor.addMenuItem(suggestionBoxItem,
                            {
                                id: s.id,
                                label: s.label,
                                group: 'suggestionBoxGroup',
                                onClick: function () {
                                    var data = editor.getData();
                                    var selection = editor.getSelection();
                                    var element = selection.getStartElement();
                                    var ranges = selection.getRanges();
                                    ranges[0].setStart(element.getFirst(), 0);
                                    ranges[0].setEnd(element.getFirst(), 0);

                                    // Guarda a posição que estava para que possamos voltar após o insertHTML
                                    var returnY = editor.window.getScrollPosition().y;

                                    editor.insertHtml(this.id + '&nbsp;');

                                    // Volta a posição de inserção pois o CKEditor está fazendo com que o editor seja "scrollado" para o fim do editor                                    
                                    b = jQuery(editor.document.$);
                                    b.scrollTop(returnY);                                    

                                },
                            });
                    });                    

                    if (firstExecution == true) {
                        editor.contextMenu.addListener(function (element) {
                            return dataElement;
                        });
                        firstExecution = false;
                    }
                }
            }
        });        

        delete editor._.menuItems.paste;
    },
});