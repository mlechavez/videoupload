/// <reference path="../tinymce/tinymce.js" />


tinymce.init({
    selector: '#Description',    
    setup: function (ed) {
        
        //ed.onSaveContent.add(function (i, o) {
        //    o.content = o.content.replace(/&#39/g, "&apos");
        //});        
    }
});