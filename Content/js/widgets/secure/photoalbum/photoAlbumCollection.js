define(["./photoAlbumModel"], function (PhotoAlbumModel) {
    var PhotoAlbumCollection = Backbone.Collection.extend({
        model: PhotoAlbumModel,
        SetAccessor: function (model) {
            this._parent = model;
        },
        GetAccessor: function (model) {
            return this._parent;
        }
    });
    return PhotoAlbumCollection;
});
