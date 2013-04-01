define(["./photoModel"], function (PhotoModel) {
    var PhotoCollection = Backbone.Collection.extend({
        model: PhotoModel
    });
    return PhotoCollection;
});
