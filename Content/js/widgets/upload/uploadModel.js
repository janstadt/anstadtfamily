define([], function () {
    var UploadModel = Backbone.Model.extend({
        defaults: {
            "Url": null
        }
    });
    return UploadModel;
});
