(function ($, widgetObject) {
    var fadeOut = function (location) {
        var milliseconds = 1000; //for setTimeout time 

        for (var i = 50; i <= 100; i += 0.2) {
            milliseconds += 10;

            (function (index, time) {
                setTimeout(function () {
                    document.getElementById(location).style.backgroundColor = 'hsl(57,100%,' + index + '%)';
                }, time);
            })(i, milliseconds);
        }
    };

    var inPageAnchor = window.location.href;
    var strLocIndex = inPageAnchor.indexOf("#");
    var strLocation = inPageAnchor.substr(strLocIndex + 1);

    switch (strLocation) {
        case "pcd":
            fadeOut(strLocation);
            break;
        case "st16":
            fadeOut(strLocation);
            break;
        case "st27":
            fadeOut(strLocation);
            break;
        case "qsc":
            fadeOut(strLocation);
            break;
    }
})(jQuery, window.ContactUsWidget = window.ContactUsWidget || {});


