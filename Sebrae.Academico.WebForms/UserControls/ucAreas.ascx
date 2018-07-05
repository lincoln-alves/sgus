<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAreas.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucAreas" %>

<asp:HiddenField ID="txtJsonAreasSelecionadas" ClientIDMode="Static" runat="server" />

<div class="form-group">
    <h4>Pré-requisitos</h4>
    <div class="panel panel-default" style="overflow: inherit">
        <div class="panel-body">
            <div class="form-group">
                <span data-toggle="tooltip" data-title="Selecionar tudo" class="pull-right">
                    <button type="button" class="btn btn-default pull-right" onclick="SelectAllAreas()" style="width: 40px; margin-bottom: 10px;">
                        <span class="glyphicon glyphicon-plus" style="line-height: 18px; padding-bottom: 4px;"></span>
                    </button>
                </span>
                <asp:Label ID="Label1" runat="server" Text="Áreas" AssociatedControlID="txtArea"></asp:Label>
                <div class="clearfix"></div>
                <asp:TextBox ID="txtArea" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <span data-toggle="tooltip" data-title="Selecionar subáreas marcadas" class="pull-right">
                    <button type="button" class="btn btn-default pull-right" onclick="SelectSubareas()" style="width: 40px; margin-bottom: 10px;">
                        <span class="glyphicon glyphicon-plus" style="line-height: 18px; padding-bottom: 4px;"></span>
                    </button>
                </span>
                <asp:Label ID="Label2" runat="server" Text="Subáreas" AssociatedControlID="listSubareas"></asp:Label>
                (segure CRTL e clique para selecionar vários)
                <div class="clearfix"></div>
                <asp:ListBox ID="listSubareas" runat="server" Height="150" ClientIDMode="Static" SelectionMode="multiple" CssClass="form-control"></asp:ListBox>
            </div>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Subáreas selecionadas" AssociatedControlID="listSubareasSelecionadas"></asp:Label>
                (segure CRTL e clique para selecionar vários)
                <div class="row">
                    <div class="col-xs-12 col-md-10 col-lg-11">
                        <asp:ListBox ID="listSubareasSelecionadas" Height="150" runat="server" ClientIDMode="Static" SelectionMode="multiple" CssClass="form-control"></asp:ListBox>
                    </div>
                    <div class="col-xs-12 col-md-2 col-lg-1">
                        <span data-toggle="tooltip" data-title="Remover áreas selecionadas">
                            <button type="button" class="btn btn-default btn-block btn-remove-area" onclick="RemoveSelectedSubarea()">
                                <strong>>
                                </strong>
                            </button>
                        </span>
                        <br />
                        <span data-toggle="tooltip" data-title="Remover todas as áreas">
                            <button type="button" class="btn btn-default btn-block btn-remove-area" onclick="RemoveAllSelectedSubareas()">
                                <strong>>>
                                </strong>
                            </button>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">

    // JSON de todas as áreas. Super importante! Pecisa estar no escopo global pra ser consultado pelos métodos abaixo.
    var areas;

    $(document).ready(function () {
        // Inicializar as áreas.
        areas = $('#txtJsonAreasSelecionadas').val() !== ''
            ? JSON.parse($('#txtJsonAreasSelecionadas').val())
            : <%= ViewState["_Areas"] ?? "[]" %>;

        // Atualizar o HTML completo dos listbox de áreas.
        UpdateAreas();
    });

    // Método mestre para atualizar todos os dados e o DOM de áreas.
    function UpdateAreas() {
        ListAreas();
        ListSubareas();
        ListSelectedSubareas();

        // Coloca o JSON de áreas em um hidden input para preservar os dados em postbacks.
        if(areas.length > 0)
            $('#txtJsonAreasSelecionadas').val(JSON.stringify(areas));
    }

    // Criar e atualizar o aucotomplete de áreas.
    function ListAreas() {
        // Converte para um objeto que pode ser lido pelo Autocomplete.
        // Linq.js é muito bom!
        var areasAutocomplete = areas.length === 0 ? '' : Enumerable.From(areas)
            .Where(function(a) { return Enumerable.From(a.subAreas).Any(function(s) { return s.selecionada === false }) })
            .Select(function(x) {
                return {
                    'data': x['id'].toString(),
                    'value': x['id'] + ' - ' + x['nome'],
                    'exibirUf': false
                };
            })
            .ToArray();

        if (document.getElementById('txtArea_autocomplete') !== null ) {
            // Caso o input de autocomplete já tenha sido criado, remove o HTML dele
            // pra inserir novamente na criação do autocomplete abaixo. Isso força a
            // atualização do autocomplete.
            $($('#txtArea_autocomplete').parent()).remove();
        }

        // Define o autocomplete e manda o callback para atualizar o HTML na seleção de alguma área.
        AutoCompleteDefine(areasAutocomplete, '#txtArea', false, false, true, UpdateAreas);
    }

    // Criar e atualizar o listbox de subáreas.
    function ListSubareas() {
        var listBoxSubareas = $('#listSubareas');

        // Limpar subáreas.
        listBoxSubareas.html('');

        // Caso possua área selecionada, preenche o HTML das subáreas.
        var area = GetSelectedArea();

        if (area != null) {
            // Obtém as subáreas não selecionadas.
            var subAreas = Enumerable.From(area.subAreas)
                .Where(function(x) { return x.selecionada !== true })
                .ToArray();

            for (var i = 0; i < subAreas.length; i++) {
                listBoxSubareas.append(GetHtmlOption(subAreas[i]));
            }
        }
    }

    // Criar e atualizar o listbox de subáreas selecionadas.
    function ListSelectedSubareas() {
        var listBoxSelectedSubareas = $('#listSubareasSelecionadas');

        // Limpar subáreas selecionadas.
        listBoxSelectedSubareas.html('');

        var selectedAreas = GetSelectedSubareas();

        for (var i = 0; i < selectedAreas.length; i++) {
            listBoxSelectedSubareas.append(GetHtmlOptionGroup(selectedAreas[i]));
        }
    }

    // Selecionar todas as áreas disponíveis.
    function SelectAllAreas() {
        for (var a = 0; a < areas.length; a++) {
            for (var s = 0; s < areas[a].subAreas.length; s++) {
                areas[a].subAreas[s].selecionada = true;
            }
        }

        UpdateAreas();
    }

    // Selecionar as subáreas marcadas.
    function SelectSubareas() {
        var area = GetSelectedArea();

        if (area != null) {
            $('#listSubareas :selected').each(function(i, selected) {
                var id = parseInt(selected.value);
                
                // Alterar as subáreas selecionadas.
                var selectedSubArea =
                    Enumerable.From(area.subAreas)
                        .Where(function(x) { return x.id === id })
                        .FirstOrDefault();

                // Selecionar subárea.
                selectedSubArea.selecionada = true;
            });

            // Atualiza tudo.
            UpdateAreas();
        }
    }


    function RemoveSelectedSubarea() {
        $('#listSubareasSelecionadas :selected').each(function(i, selected) {
            var id = parseInt(selected.value);

            var subArea = GetSubareaById(id);

            // Selecionar subárea.
            subArea.selecionada = false;
        });

        UpdateAreas();
    }

    // Remover todas as subáreas selecionadas.
    function RemoveAllSelectedSubareas() {
        var selectedSubAreas = Enumerable.From(areas)
            .SelectMany(function(a) { return a.subAreas })
            .Where(function(s) { return s.selecionada === true })
            .ToArray();

        for (var i = 0; i < selectedSubAreas.length; i++) {
            selectedSubAreas[i].selecionada = false;
        }

        // Atualiza tudo.
        UpdateAreas();
    }

    function GetSelectedArea() {
        var autocompleteAreasId = $('#txtArea').val();

        if (isInteger(autocompleteAreasId))
            return GetAreaById(parseInt(autocompleteAreasId));

        return null;
    }

    function GetSelectedSubareas() {
        return Enumerable.From(areas)
            .Where(function(x) {
                return Enumerable.From(x.subAreas).Any(function(s) { return s.selecionada === true; });
            })
            .Select(function(x) {
                return {
                    id: x.id,
                    nome: x.nome,
                    subAreas: Enumerable.From(x.subAreas)
                        .Where(function(s) { return s.selecionada === true })
                        .Select(function(s) { return { id: s.id, nome: s.nome } })
                        .ToArray()
                }
            })
            .ToArray();
    }

    function GetHtmlOption(subArea) {
        return '<option value="' + subArea.id + '">' + subArea.id + ' - ' + subArea.nome + '</option>';
    }

    function GetHtmlOptionGroup(area) {
        var optGroupArea = $('<optgroup label="ÁREA: ' + area.id + ' - ' + area.nome + '"></optgroup>');

        for (var opt = 0; opt < area.subAreas.length; opt++) {
            var subArea = area.subAreas[opt];

            optGroupArea.append(GetHtmlOption(subArea));
        }

        return optGroupArea;
    }

    function GetAreaById(areaId) {
        return Enumerable.From(areas)
            .Where(function(x) { return x.id === areaId })
            .FirstOrDefault();
    }

    function GetSubareaById(subAreaId) {
        return Enumerable.From(areas)
            .SelectMany(function(a) { return a.subAreas })
            .Where(function(x) { return x.id === subAreaId })
            .FirstOrDefault();
    }

    // Função helper simples para verificar se um objeto pode ser convertido em inteiro.
    function isInteger(value) {
        return !isNaN(value) && (function(x) { return (x | 0) === x; })(parseFloat(value));
    }

</script>