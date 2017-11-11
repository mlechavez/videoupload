(function ($, widgetObject) {
    var controls = {
        frmSearch : $('#search-form')
    };

    widgetObject.init = function () {
        setTimeZoneCookie();        
        bindUiActions();
    };    

    var bindUiActions = function () {
        controls.frmSearch.on('submit', function () {

            if ($('#v').val().trim()) return true;

            return false;
        });
    };

    var setTimeZoneCookie = function() {
        var timeZone_cookie = "timezoneoffset";

        // checks if the cookie exists if not create one
        if (!$.cookie(timeZone_cookie)) {

            //checks if the browser supports cookie
            var test = "testCookie";
            $.cookie(test, true);

            //browser supports cookie
            if ($.cookie(test)) {

                //delete the test cookie
                $.cookie(test, null);

                //create a new cookie
                $.cookie(timeZone_cookie, new Date().getTimezoneOffset());
                location.reload();
            }
        } else {
            var storedCookie = parseInt($.cookie(timeZone_cookie));
            var currentCookie = new Date().getTimezoneOffset();

            if (storedCookie !== currentCookie) {
                $.cookie(timeZone_cookie, new Date().getTimezoneOffset());

                location.reload();
            }
        }
    }

})(jQuery, window.SetTimeCookieWidget = window.SetTimeCookieWidget || {});

SetTimeCookieWidget.init();



