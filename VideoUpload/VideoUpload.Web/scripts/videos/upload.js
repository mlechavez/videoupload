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

function progressHandler(event) {
    var percent = (event.loaded / event.total) * 100;
    var appModal = $('#appModal');

    progressBar = appModal.find('.progress-bar');

    progressBar
        .css('width', percent)
        .addClass('progress-bar-striped active')
        .html(percent);

    $('#btnSubmit').html(
        "<i class='fa fa-spin'></i>Loading..."
    ).addClass('disabled');
}
function completeHandler() {

    var appModal = $('#appModal'),
        title = appModal.find('.modal-title'),
        progressBar = appModal.find('.progress-bar'),
        btnClose = appModal.find('.close');

    title.html('Uploaded succesfully');
    btnClose.removeClass('hidden');
    progressBar
        .css('width', '100%')
        .removeClass('active')
        .html('100%');
    $('#btnSubmit').removeClass('disabled').html('Upload');

}