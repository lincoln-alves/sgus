$(function () {
    dynamicForm();
});

function dynamicForm(){
    var forms = $('.dynamic-graph');

    forms.each(function () {
        var form = $(this);
        var fields = form.find('input, select');
        var img = form.find('img');

        fields.each(function () {
            $(this).attr('name',$(this).attr('id'));
        });

        fields.change(function () {
            img.attr("src", form.data('action') + "?" + fields.serialize());
        });

    });
}