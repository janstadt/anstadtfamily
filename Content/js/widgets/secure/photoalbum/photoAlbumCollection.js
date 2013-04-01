define(["./photoAlbumModel"], function (PhotoAlbumModel) {
    var PhotoAlbumCollection = Backbone.Collection.extend({
        model: PhotoAlbumModel
    });
    return PhotoAlbumCollection;
});
