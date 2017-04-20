/// <reference path="../jquery-3.1.1.js" />

var hasPlayed = $('#postUrl').data('hasplayed');

var vPlayer = $('#vPlayer').get(0);

if (hasPlayed === "False") {

    if (vPlayer.play()) {
    var postID = $('#postUrl').data('id');
    var url = $('#postUrl').data('url');
    var uploader = $('#postUrl').data('uploader');
    var currentLink = window.location.href;
    currentLink = currentLink.replace("watch", "details");    

    $.ajax({
        type: 'post',
        url: url,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ 
            postID: postID,
            userName: uploader,
            details: currentLink
        }),
        success: function (data) {

        }
    });
    }
}