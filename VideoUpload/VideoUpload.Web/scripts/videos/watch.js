/// <reference path="../jquery-3.1.1.js" />

var hasPlayed = $('#postUrl').data('hasPlayed');
var vPlayer = $('#vPlayer').get(0);

if (!hasPlayed) {
    if (vPlayer.play()) {
    var postID = $('#postUrl').data('id');
    var url = $('#postUrl').data('url');
    var userName = $('#postUrl').data('userName');
    var currentLink = window.location.href;
    currentLink = currentLink.replace("watch", "details");    

    $.ajax({
        type: 'post',
        url: url,
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ 
            id: postID,
            userName: userName,
            details: currentLink
        }),
        success: function (data) {

        }
    });
    }
}