$('#appModal').on('show.bs.modal', function (event) {    
    var button = $(event.relatedTarget);
    var isapproved = button.data('isapproved');
    
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
                            'data-value': isapproved,
                            'data-url' : '/videos/approval'
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
    var url = $(this).data('url');
    var value = $(this).data('value');
    $({
        type: 'post',
        url: url,
        dataType: 'json',
        data: { isapproved: value },
        success: function (result) {

        }
    });
    console.log(data);
});

