/// <reference path="../jquery-3.1.1.js" />

var vUrl = $('#videoSourceUrl').data('url');
var vPlayer = document.getElementById("vPlayer"); //$('#vPlayer').get(0);

//console.log(vUrl);
//var req = new XMLHttpRequest();
//req.open('GET', vUrl, true);
//req.responseType = 'blob';

//req.onload = function () {
//    if (this.status === 200) {
//        console.log('ok');
//        var videoBlob = this.response;
//        var vid = URL.createObjectURL(videoBlob);
//        vPlayer.src = vid;
//    } else {
//        console.log('not ok');
//    }
//}

//I use onplay event to handle IE browser
//Tested in IE 11 only not below
//since html 5 video won't play in browser below IE 11
vPlayer.onplay = function () {
    var hasPlayed = $('#postUrl').data('hasplayed');
    
    if (hasPlayed === "False") {
       
        var postID = $('#postUrl').data('id');
        var url = $('#postUrl').data('url');
        var uploader = $('#postUrl').data('uploader');
        var currentLink = window.location.href;
        currentLink = currentLink.replace("watch", "archive");

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
                if (data.success) {
                    $('#postUrl').data('hasplayed', data.success);
                    var value = $('#postUrl').data('hasplayed');                    
                }                
            }
        });
    } 
};