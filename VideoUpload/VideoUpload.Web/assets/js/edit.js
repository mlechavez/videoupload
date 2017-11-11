(function ($, widgetObject, tinymce) {
    
    widgetObject.init = function () {
        tinymce.init({
            selector: "#Description",
            branding: false,
            menubar: false,
            toolbar: false, 
            statusbar: false,
            content_css: '../../dist/css/app.min.css'
        });
    };

})(jQuery, window.EditVideoWidget = window.EditVideoWidget || {}, tinymce);

EditVideoWidget.init();