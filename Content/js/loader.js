require.config({
    paths: {
        text: "shared/lib/text",
        libs: "shared/lib",
        plugins: "shared/plugins",
        jquery: "shared/lib/jquery-1.7.2.min",
        underscore: "shared/lib/underscore",
        backbone: "shared/lib/backbone",
        i18n: "shared/lib/i18n",
        use: "shared/plugins/use",
        "jquery.ui": "shared/lib/jquery-ui-1.8.24",
        'jquery.ui.widget': 'shared/lib/jquery.file.upload/js/vendor/jquery.ui.widget',
        'tmpl': "shared/lib/jquery.file.upload/js/tmpl.min",
        "tagit": "shared/lib/tag-it.min",
        'load-image' : "shared/lib/jquery.file.upload/js/load-image.min",
        'canvas-to-blob' : "shared/lib/jquery.file.upload/js/canvas-to-blob.min",
        'jquery.iframe-transport': 'shared/lib/jquery.file.upload/js/jquery.iframe-transport',
        'jquery.fileupload': 'shared/lib/jquery.file.upload/js/jquery.fileupload',
        'jquery.fileupload-fp': 'shared/lib/jquery.file.upload/js/jquery.fileupload-fp',
        'jquery.fileupload-ui': 'shared/lib/jquery.file.upload/js/jquery.fileupload-ui'
    },

    use: {
        backbone: {
            deps: ["use!underscore", "jquery"],
            attach: "Backbone"
        },

        underscore: {
            attach: "_"
        }
    }
});

require(["./application"], function (Application) {

    $(document).ready(function () {

        var application = new Application();

        window.application = application;

        application.start();

    });

});
