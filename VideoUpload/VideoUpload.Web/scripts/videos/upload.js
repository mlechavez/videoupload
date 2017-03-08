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

    var ajax = new XMLHttpRequest();
    ajax.upload.addEventListener("progress", progressHandler, false);
    ajax.upload.addEventListener("load", completeHandler, false);
    ajax.open("post", form.attr('action'));
    ajax.send(formData);

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
    var appModal = $('#appModal');
    $('#PlateNumber').val("");
    $('#Description').val('');
    $('#Attachments').val('');

    appModal.modal('hide');   
});

$('#appModal').on('hidden.bs.modal', function () {
    var modal = $(this);
    modal.find('.modal-body').empty();
    modal.find('.modal-footer').remove();
});

function progressHandler(event) {
    var percent = Math.floor((event.loaded / event.total) * 100);
    var widthSize = percent.toString() + "%";
    var appModal = $('#appModal');    
    progressBar = appModal.find('.progress-bar');

    progressBar
        .css('width', widthSize)
        .addClass('progress-bar-striped active')
        .html(widthSize);

    $('#btnSubmit').html(
        "<i class='fa fa-spin'></i>Loading..."
    ).addClass('disabled');
}

function completeHandler() {

    var appModal = $('#appModal'),
        title = appModal.find('.modal-title'),
        progressBar = appModal.find('.progress-bar'),
        btnClose = appModal.find('.close'),
        content = appModal.find('.modal-content');

    title.html('Uploaded succesfully');
    btnClose.removeClass('hidden');
    progressBar
        .css('width', '100%')
        .removeClass('active')
        .html('100%');

    $('#btnSubmit').removeClass('disabled').html('Upload');

    var footer = $('<div></div>').addClass('modal-footer');
    var btnOk = $('<button></button>')
                .addClass('btn btn-success pull-right')
                .attr('id', 'btnUploadExit')
                .html('Close');
    footer.append(btnOk);
    content.append(footer);    
}