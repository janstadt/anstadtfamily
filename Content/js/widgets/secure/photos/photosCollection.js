define(["../../photo/photoModel"], function (PhotosModel) {
    var PhotosCollection = Backbone.Collection.extend({
        initialize: function (models, options) {
            this.Id = options.PageId;
            this.SubId = options.SubId;
        },
        url: function () {
            return "api/albums/photos/" + this.SubId
        },
        model: PhotosModel
    });
    return PhotosCollection;
});
