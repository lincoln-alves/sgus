function makeid() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < 5; i++)
        text += possible.charAt(Math.floor(Math.random() * possible.length));

    return text;
}

(function ($) {
    function tooltip() {
        var helpButton = $('<span />').addClass('glyphicon glyphicon-question-sign helpDialog');

        var helpeds = $("[data-help]");
        
        helpeds.each(function () {
            var self = $(this);

            var helpText = self.data('help');

            if (helpText !== undefined && helpText !== null && helpText !== '') {
                var popoverOptions = {
                    animation: true,
                    placement: 'right',
                    content: helpText,
                    trigger: 'hover',
                    html: true,
                    template: '<div class="popover helpDialogPopover" role="tooltip"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
                }

                var objId = self.attr('id');

                if (objId === undefined) objId = makeid();

                var containerId = objId + '_helpDialog';

                helpButton.attr('id', containerId);

                $(helpButton.prop('outerHTML')).insertAfter(self);

                $('#' + containerId).popover(popoverOptions);
            }
        });
    }

    $(document).ready(function() {
        tooltip();
    });
})(jQuery);