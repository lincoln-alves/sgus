<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCategorias.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucCategorias" %>
<asp:TreeView ID="trvCategorias" runat="server" ShowCheckBoxes="All" CssClass="checkbox_tree" OnTreeNodeCheckChanged="trvCategorias_TreeNodeCheckChanged">
</asp:TreeView>

<asp:HiddenField ID="hdnIsRadio" runat="server" Value="0" />
<asp:HiddenField ID="hdnIdCategoriaRadio" runat="server" Value="0" />
<asp:LinkButton runat="server" ID="lnkCategoria" OnClick="lnkCategoria_OnClick" />

<div runat="server" ID="divSelecionarTodos">
</div>

<script type="text/javascript">
    $(function () {
        $("input[type='checkbox']").click(function () {
            var idCheckbox = $(this).attr('id');
            var retiraPrefixo = idCheckbox.replace('<%= trvCategorias.ClientID %>', '');
            var id = retiraPrefixo.replace('CheckBox', '');
            if ($(this).prop("checked")) {
                $('#<%= trvCategorias.ClientID %>' + id + 'Nodes table tr td input[type="checkbox"]').prop("checked", true);
            } else {
                $('#<%= trvCategorias.ClientID %>' + id + 'Nodes table tr td input[type="checkbox"]').prop("checked", false);
            }
        });

        var isRadio = <%= (IsRadio).ToString().ToLower() %>;
        var hdnIdCategoriaRadio = $('#<%= hdnIdCategoriaRadio.ClientID %>');
        
        // Caso seja Radio, mantém a seleção.
        if (isRadio) {
            if (hdnIdCategoriaRadio.val() !== "0") {
                $('#<%= trvCategorias.ClientID %> input:radio[value=' + hdnIdCategoriaRadio.val() + ']').attr('checked', true);
            }

            $('input[name="<%= trvCategorias.ClientID %>_rbCategoriaConteudo"]')
                .mousedown(function() {
                    var radio = $(this);

                    hdnIdCategoriaRadio.val(radio.val());
                })
                .mouseup(function() {
                    $('#<%= lnkCategoria.ClientID %>')[0].click();
                });
        }

        // Criar a funcionalidade de selecionar todas;
        // Só faz sentido se não for Radio.
        if (!isRadio && document.getElementById('<%= divSelecionarTodos.ClientID %>') !== null)
            $.markAll('<%= divSelecionarTodos.ClientID %>', '<%= trvCategorias.ClientID%>', 'Marcar Todas', 'Desmarcar Todas');
    });

    function <%= trvCategorias.ClientID %>_postBackByObject() {
        var checkBox = window.event != window.undefined ? window.event.srcElement : clickedElement.target;

        if (checkBox.tagName.toLowerCase() === "input" && checkBox.type.toLowerCase() === "checkbox") {
            MostarLoading();

            __doPostBack("<%= trvCategorias.ClientID %>", "");
        }
    }

    function <%= trvCategorias.ClientID %>_OnTreeClick(evt) {
        var src = window.event !== window.undefined ? window.event.srcElement : evt.target;
        var isChkBoxClick = (src.tagName.toLowerCase() === "input" && src.type === "checkbox");

        if (isChkBoxClick) {
            var parentTable = <%= trvCategorias.ClientID %>_GetParentByTagName("table", src);
            var nxtSibling = parentTable.nextSibling;

            // Checa se o proximo sibling nao é nulo e se é um nó.
            if (nxtSibling && nxtSibling.nodeType === 1) {
                //Verifica se o nó é uma div (Se for, ele é um container)
                if (nxtSibling.tagName.toLowerCase() === "div") {
                    <%= trvCategorias.ClientID %>_CheckUncheckChildren(parentTable.nextSibling, src.checked);
                }
            }

            <%= trvCategorias.ClientID %>_CheckUncheckParents(src, src.checked);
        }
    }

    function <%= trvCategorias.ClientID %>_CheckUncheckChildren(childContainer, check) {
        var childChkBoxes = childContainer.getElementsByTagName("input");
        var childChkBoxCount = childChkBoxes.length;
        for (var i = 0; i < childChkBoxCount; i++) {
            childChkBoxes[i].checked = check;
        }
    }

    function <%= trvCategorias.ClientID %>_CheckUncheckParents(srcChild, check) {
        var parentDiv = <%= trvCategorias.ClientID %>_GetParentByTagName("div", srcChild);
        var parentNodeTable = parentDiv.previousSibling;

        if (parentNodeTable) {
            var checkUncheckSwitch;

            if (check) {
                checkUncheckSwitch = true;
            }
            else {
                var isAllSiblingsUnChecked = <%= trvCategorias.ClientID %>_AreAllSiblingsChecked(srcChild);
                if (!isAllSiblingsUnChecked)
                    checkUncheckSwitch = true;
                else
                    checkUncheckSwitch = false;
            }

            var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
            if (inpElemsInParentTable.length > 0) {
                var parentNodeChkBox = inpElemsInParentTable[0];
                parentNodeChkBox.checked = checkUncheckSwitch;
                // Recursividade para checkar todos os nodePai, do node.
                <%= trvCategorias.ClientID %>_CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
            }
        }
    }

    function <%= trvCategorias.ClientID %>_AreAllSiblingsChecked(chkBox) {
        var parentDiv = <%= trvCategorias.ClientID %>_GetParentByTagName("div", chkBox);
        var childCount = parentDiv.childNodes.length;
        for (var i = 0; i < childCount; i++) {
            if (parentDiv.childNodes[i].nodeType == 1) {
                if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                    var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                    if (!prevChkBox.checked) {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    function <%= trvCategorias.ClientID %>_GetParentByTagName(parentTagName, childElementObj) {
        var parent = childElementObj.parentNode;
        while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
            parent = parent.parentNode;
        }
        return parent;
    }
</script>
