/// <reference path="../jquery-3.1.1.js" />

$('#frmUpload').on('submit', function (e) {
    e.preventDefault();
    var form = $(this);

    var files = $('#Attachments').get(0).files;
    var formData = new FormData();

    for (var i = 0; i < files.length; i++) {
        formData.append("Attachments", files[i]);
    }

    $("input[type='text'").each(function (index, value) {
        formData.append($(value).attr('name'), $(value).val());
    });

    formData.append('Description', $('#Description').val());

    $('#appModal').modal({
        keyboard: false,
        backdrop: 'static'
    });
    $('#appModal').modal('show');

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
    }

    xhr.upload.addEventListener("progress", function (event) {
        var percent = Math.floor((event.loaded / event.total) * 100);
        var widthSize = percent.toString() + "%";
        var appModal = $('#appModal');
        progressBar = appModal.find('.progress-bar');
        

        progressBar
            .css('width', widthSize)
            .addClass('progress-bar-striped active')
            .html(widthSize);

        if (widthSize === "100%") {
            $('#appModal').find('.modal-title').html('Your video is now being converted. Pls do not interrupt');
        }

        $('#btnSubmit').html(
            "<i class='fa fa-spin'></i>Loading..."
        ).addClass('disabled');
    });


    //xhr.upload.addEventListener("load", completeHandler); //ignore this because of onreadystatechange function
    //xhr.upload.addEventListener("error", failedHandler); //no event handler function made that's why it's commented


    xhr.open("post", form.attr('action'), true);
    xhr.send(formData);

    function completeHandler(message) {

        var appModal = $('#appModal'),
            title = appModal.find('.modal-title'),
            progressBar = appModal.find('.progress-bar'),

            content = appModal.find('.modal-content');

        title.html(message);

        progressBar
            .css('width', '100%')
            .removeClass('active')
            .html('100%');

        $('#btnSubmit').removeClass('disabled').html('Upload');

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
    };
    function invalidFormat(message) {
        var appModal = $('#appModal'),
            title = appModal.find('.modal-title'),
            body = appModal.find('.modal-body'),
            content = appModal.find('.modal-content');

        title.html('Upload failed');
        body.empty();

        body.append('<p class="text-center">' + message + "</p>");

        $('#btnSubmit').removeClass('disabled').html('Upload');

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
    };
});

$('#appModal').on('show.bs.modal', function () {
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

$('#appModal .modal-content').on('click', '#btnUploadExit', function () {

    var self = $(this),
        status = self.data('status');

    if (status === "success") {
        
        var currentUrl = location.href.toLowerCase();

        location.href = currentUrl.replace("upload", "");                
        
    } else {
        $('#appModal').modal('hide');
    }
});

$('#appModal').on('hidden.bs.modal', function () {
    var modal = $(this);
    modal.find('.modal-body').empty();
    modal.find('.modal-footer').remove();
});



