(function ($, myObject) {
    var controls = {
        videoUrl: $('#videoSourceUrl'),
        videoPlayer: document.getElementById("vPlayer"),
        postedVideo: $('#postUrl'),
    };

    myObject.init = function () {
        bindUIActions();
    };

    var bindUIActions = function () {
        controls.videoPlayer.onplay = function () {
            var hasPlayed = controls.postedVideo.data('hasplayed');

            if (hasPlayed === "False") {

                var postID = controls.postedVideo.data('id');
                var url = controls.postedVideo.data('url');
                var uploader = controls.postedVideo.data('uploader');
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
                            controls.postedVideo.data('hasplayed', data.success);
                            var value = controls.postedVideo.data('hasplayed');
                        }
                    }
                });
            }
        };
    };
}(jQuery, window.WatchVideoWidget = window.WatchVideoWidget || {}));

WatchVideoWidget.init();