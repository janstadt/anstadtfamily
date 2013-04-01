define([
    "./uploadModel",
    "text!./uploadTemplate.html",
    "i18n!./nls/upload",
    "jquery.ui.widget",
    'tmpl',
    'load-image',
    'canvas-to-blob',
    'jquery.fileupload-fp',
    'jquery.fileupload-ui',
    "jquery.fileupload",
    "jquery.iframe-transport"
], function (
    uploadModel,
    template,
    i18n) {
    var UploadView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        content: null,
        initialize: function (defaults) {
            this.render();
        },

        render: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            return this;
        },

        initFileUpload: function () {
            var fileUpload = $(this.el).find("#fileupload"),
            self = this;
            fileUpload.fileupload({
                url: this.model.get("Url"),
                dropZone: $(this.el),
                formData: _.bind(function () {
                    var watermark = this.$("#watermark").is(":checked"),
                        data = this.model.get("Data") || [];
                    data.push({ name: "Watermark", value: watermark});
                    return data;
                }, this),
                process: [
                    {
                        action: 'load',
                        fileTypes: /^image\/(gif|jpeg|png)$/
                    },
                    {
                        action: 'save'
                    }
                ]
            }).bind("fileuploaddone", _.bind(this.successCallback, this))
              .bind("fileuploadfail", _.bind(this.errorCallback, this))
              .bind("fileuploadstop", _.bind(this.finishedCallback, this));
        },

        errorCallback: function (e, data) {
            //alert('error');
        },

        successCallback: function (e, data) {
            //alert('success');
            this.trigger("add", data);
        },

        finishedCallback: function (e, data) {
            //alert('finished');
            this.trigger("finished");
        }
    });
    return UploadView;
});
