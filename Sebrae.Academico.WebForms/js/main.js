$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('#LupaUsuario_txtNomeUsuarioSelecionado').addClass('hidden');

    $('.navbar').each(function () {
        var self = $(this);
        var header = self.find(".dropdown-caller");
        var menu = self.find(".dropdown-menu");
        header.click(function () {
            if (menu.hasClass("open")) { menu.mouseleave(); return false; };
            menu.addClass("open");
            menu.fadeIn(150);
            self.find('li').css("marginTop", "-120px").each(function (index, target) {
                $(this).delay(33 * index).animate({ marginTop: "10px" }, 75);
            });
        });
        menu.mouseleave(function () {
            var delay;

            $(self.find('li').get().reverse()).each(function (index, target) {
                delay = 20 * index;
                $(this).delay(delay).animate({ marginTop: "-120px" }, 75);
            });
            menu.removeClass("open");
            menu.delay(delay + 40).fadeOut(150);
        });

    });

    // Controle do menu-lateral
    var sidebar = $('.side-bar');
    var contentArea = $('.conteudo_busca');
    $('.side-bar-icon').click(function () {
        var postBack = this.dataset.postback === "1";

        if (sidebar.hasClass('closed')) {
            $.cookie('menu_state', 'open');

            if (postBack) {
                location.reload();
            } else {
                sidebar.delay(40).animate({ marginLeft: '0%' }, 250);
                contentArea.animate({ width: '75%' }, 250);
            }
        } else {
            $.cookie('menu_state', 'close');
            if (postBack) {
                location.reload();
            } else {
                sidebar.animate({ marginLeft: '-25%' }, 250);
                contentArea.delay(40).animate({ width: '100%' }, 250);
            }
        }

        $(this).toggleClass('closed');
        sidebar.toggleClass('closed');
    });

    if ($.cookie('menu_state') == 'close') {
        $('.side-bar-icon').addClass('closed');
        sidebar.addClass('closed');
        sidebar.css({ marginLeft: '-25%' });
        contentArea.css({ width: '100%' });
    }

    $(".mostrarload").each(function () {
        $(this).data("default_value", $(this).val());

        $(this).find('input').on('click', function () {
            MostarLoading();
        });

        $(this).change(function () {
            if ($(this).val() !== $(this).data("default_value")) { MostarLoading(); }
        });
    });
});

var target = document.getElementById('divCarregando');

function ConfirmarExclusao() {
    return ConfirmarExclusao("Deseja Realmente Excluir este Registro?");
}

function ConfirmarExclusao(mensagem) {
    if (mensagem == null)
        mensagem = "Deseja Realmente Excluir este Registro?";

    if (confirm(mensagem)) {
        MostarLoading();
        return true;
    } else {
        return false;
    }
}

var opts = {
    lines: 13, // The number of lines to draw
    length: 14, // The length of each line
    width: 5, // The line thickness
    radius: 14, // The radius of the inner circle
    corners: 1, // Corner roundness (0..1)
    rotate: 0, // The rotation offset
    direction: 1, // 1: clockwise, -1: counterclockwise
    color: '#fff', // #rgb or #rrggbb or array of colors
    speed: 1, // Rounds per second
    trail: 60, // Afterglow percentage
    shadow: false, // Whether to render a shadow
    hwaccel: false, // Whether to use hardware acceleration
    className: 'spinner', // The CSS class to assign to the spinner
    zIndex: 2e9, // The z-index (defaults to 2000000000)
    top: 'auto', // Top position relative to parent in px
    left: 'auto' // Left position relative to parent in px
};

function MostarLoading() {
    $("#divCarregando").show();
    new Spinner(opts).spin(target);
}