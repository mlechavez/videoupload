(function ($, widgetObject, tinymce) {
    
    widgetObject.init = function () {
        tinymce.init({
            selector: "#Description",
            menubar: false,
            toolbar: false,
            branding: false,            
            content_css: '../../dist/css/app.min.css'
        });
    };

})(jQuery, window.EditVideoWidget = window.EditVideoWidget || {}, tinymce);

EditVideoWidget.init();