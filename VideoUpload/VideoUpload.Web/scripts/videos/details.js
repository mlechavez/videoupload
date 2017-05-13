$('#appModal').on('show.bs.modal', function (event) {

    var vPlayer = $('#vPlayer').get(0);

    if (vPlayer.play()) {
        vPlayer.pause()
    }

    var button = $(event.relatedTarget);
    var isapproved = button.data('isapproved');
    var url = button.data('url');
    var postID = button.data('postid');

    var appModal = $(this);
    var body = appModal.find('.modal-body');    
    
    appModal.find('.modal-title').text('Approval');

    var p = $('<p></p>');

    if (isapproved) {        
        p.html('Are you sure you want to approve this video?').addClass('text-center');
        body.append(p);
    } else {        
        p.html('Are you sure you want to disapprove this video?').addClass('text-center');
        body.append(p);
    }
        
    var footer = $('<div></div>').addClass('modal-footer');
    
    appModal.find('.modal-content').append(footer)
    var btnApproval = $("<button></button>")
                        .attr({
                            'id': 'btnApproved',
                            'data-postid': postID, 
                            'data-value': isapproved,
                            'data-url': url
                        }).text('OK').addClass('btn btn-default text-right');

    var btnClose = $("<button></button>").attr('id', 'btnClose').text('Close').addClass('btn btn-default text-right');
    footer.append(btnApproval).append(btnClose);
});

$('#appModal .modal-content').on('click', '#btnClose', function () {
    var appModal = $('#appModal');
    appModal.modal('hide');
});

$('#appModal').on('hidden.bs.modal', function () {
    var modal = $(this);
    modal.find('.modal-body').empty();
    modal.find('.modal-footer').remove();
});

$('#appModal .modal-content').on('click', '#btnApproved', function () {
    var appModal = $('#appModal');
    var btn = $(this);
    var url = btn.data('url');
    var value = btn.data('value');
    var postID = btn.data('postid');
    

    appModal.find('#btnClose').addClass('hidden');
    btn.html('<span>Please wait..</span>').addClass('disabled');   

    $.ajax({
        type: 'post',
        url: url,
        dataType: 'json',
        data: {
            isapproved: value,
            postID : postID
        },
        success: function (result) {

            //create another button for reloading the page
            var btnReload = $('<button></button>').attr('id', 'btnReload').addClass('btn btn-default text-right').text('Reload the page');
            //append to footer
            appModal.find('.modal-footer').append(btnReload)
            var body = appModal.find('.modal-body');
            body.empty();

            var p = $('<p></p>').addClass('text-center');

            btn.addClass('hidden');

            if (result.success) {
                                               
                p.text(result.message);
                
            } else {
                //if you go this far, It's either you could not retrieve the post
                // or it's been approved/disapproved but the email is not working.
                
                p.text(result.message);
            }

            body.append(p);
        }
    });
});

$('#appModal .modal-content').on('click', '#btnReload', function () {
    location.reload();
});

