(function ($, MyObject, tinymce) {
    tinymce.init({
        selector: "#Description",
        menubar: false,
        toolbar: false,
        content_css: '../../dist/css/styles.min.css'
    });

    var controls = {
        frmUpload: $('#frmUpload'),
        appModal: $('#appModal'),
        txtFile: $('#Attachments'),
        btnSubmit: $('#btnSubmit'),
        modalContent: $('#appModal .modal-content')
    };

    MyObject.init = function () {
        bindUIActions();
    };

    var bindUIActions = function () {

        controls.frmUpload.on('submit', function (e) {
            e.preventDefault();
            var form = $(this);

            var files = controls.txtFile.get(0).files;

            var formData = new FormData();

            for (var i = 0; i < files.length; i++) {
                formData.append("Attachments", files[i]);
            }

            $("input[type='text'").each(function (index, value) {
                formData.append($(value).attr('name'), $(value).val());
            });

            //since I use tinyMce, for me to get the raw content of the editor use tinyMCE.activeEditor.getContent({ format : 'raw'})
            //formData.append('Description', $('#Description').val()); // This has been commented since I used the tinyMCE

            formData.append('Description', tinyMCE.activeEditor.getContent({ format: 'raw' }));

            controls.appModal.modal({
                keyboard: false,
                backdrop: 'static'
            });

            controls.appModal.modal('show');

            var xhr = new XMLHttpRequest();

            xhr.onreadystatechange = function () {
                if (this.readyState === 4 && this.status === 200) {
                    var data = JSON.parse(this.responseText);
                    if (data.success) {
                        completeHandler(data.message);
                    } else {
                        invalidFormat(data.message);
                    }
                }
            };

            xhr.upload.addEventListener("progress", function (event) {
                var quotient = event.loaded / event.total;
                var percent = Math.floor(quotient * 100);
                var widthSize = percent.toString() + "%";
                

                progressBar = controls.appModal.find('.progress-bar');

                progressBar
                    .css('width', widthSize)
                    .addClass('progress-bar-striped active')
                    .html(widthSize);

                if (widthSize === "100%") {
                    controls.appModal.find('.modal-title').html('Your video is now being converted. Please do not interrupt');
                }

                $('#btnSubmit').html(
                    "<i class='fa fa-spin'></i>Loading..."
                ).addClass('disabled');
            });

            xhr.open("post", form.attr('action'), true);
            xhr.send(formData);

            function completeHandler(message) {

                var title = controls.appModal.find('.modal-title'),
                    progressBar = controls.appModal.find('.progress-bar'),

                    content = controls.appModal.find('.modal-content');

                title.html(message);

                progressBar
                    .css('width', '100%')
                    .removeClass('active')
                    .html('100%');

                controls.btnSubmit.removeClass('disabled').html('Upload');

                var footer = $('<div></div>').addClass('modal-footer');
                var btnOk = $('<button></button>')
                            .addClass('btn btn-primary pull-right')
                            .attr({
                                'id': 'btnUploadExit',
                                'data-status': 'success'
                            })
                            .html('Close');
                footer.append(btnOk);
                content.append(footer);
            }

            function invalidFormat(message) {
                var title = controls.appModal.find('.modal-title'),
                    body = controls.appModal.find('.modal-body'),
                    content = controls.appModal.find('.modal-content');

                title.html('Upload failed');
                body.empty();

                body.append('<p class="text-center">' + message + "</p>");

                controls.btnSubmit.removeClass('disabled').html('Upload');

                var footer = $('<div></div>').addClass('modal-footer');
                var btnOk = $('<button></button>')
                            .addClass('btn btn-primary pull-right')
                            .attr({
                                'id': 'btnUploadExit',
                                'data-status': 'failed'
                            })
                            .html('Close');
                footer.append(btnOk);
                content.append(footer);
            }
        });

        controls.appModal.on('show.bs.modal', function () {
            var self = $(this),
                title = self.find('.modal-title'),
                btnClose = self.find('.close'),
                body = self.find('.modal-body');

            title.html('Upload in progress');
            btnClose.addClass('hidden');
            body.empty();

            //create a progress bar
            var progress = $('<div></div>');
            progress.addClass('progress');
            var progressBar = $('<div></div>');
            progressBar.addClass('progress-bar');
            progress.append(progressBar);
            //append to the body
            body.append(progress);
        });

        controls.modalContent.on('click', '#btnUploadExit', function () {

            var self = $(this),
                status = self.data('status');

            if (status === "success") {

                var currentUrl = location.href.toLowerCase();

                //TODO: check this when you change Route attribute of the list action to the videos controller
                location.href = currentUrl.replace("/videos/upload", "");

            } else {
                controls.appModal.modal('hide');
            }
        });

        controls.appModal.on('hidden.bs.modal', function () {
            var modal = $(this);
            modal.find('.modal-body').empty();
            modal.find('.modal-footer').remove();
        });
    };

}(jQuery, window.UploadWidget = window.UploadWidget || {}, tinymce));

UploadWidget.init();