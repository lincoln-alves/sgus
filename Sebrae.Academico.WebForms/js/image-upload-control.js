$(function () {
    $('.image-upload-control').each(function () {
        var self = $(this);

        var img = self.find('img');
        var file = self.find('[type=file]');
        var del = self.find('[type=checkbox]');

        var url_img = img.attr("src");

        var new_btn = $('<div class="image-thumbnail-button" ><div class="glyphicon glyphicon-remove action-delete" title="Remover imagem"/><div class="action-label" /><img class="fake-img"/></div>');
        var action_label = new_btn.find(".action-label");
        var fake_image = new_btn.find(".fake-img");
        var del_btn = new_btn.find(".action-delete");

        if (url_img) {
            fake_image.attr("src", url_img);
            action_label.hide();
            del_btn.show();
        } else {
            action_label.html("Upload imagem");
            del_btn.hide();
        }

        self.children().not("label").hide();
        self.append(new_btn);

        /* EVENTS */

        fake_image.click(function () {
            fake_image.hide();
            file.click();
        });

        file.change(function () {

            if (this.files && this.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    fake_image.attr('src', e.target.result)
                        .height(150);
                    action_label.hide();
                    fake_image.show();
                    del_btn.show();
                };

                reader.readAsDataURL(this.files[0]);
            }

            fake_image.hide();

            var name = file.val().split("\\").pop();
            action_label.html("Imagem selecionada: " + name);
            action_label.show();
        });

        action_label.click(function () {
            file.click();
        });

        del_btn.click(function () {
            del.prop('checked', 'checked');
            del_btn.hide();
            fake_image.hide();
            action_label.html("Imagem removida");
            action_label.show();
        });

    });
});