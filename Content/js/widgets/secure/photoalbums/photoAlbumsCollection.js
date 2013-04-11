define(["../photoAlbum/photoAlbumModel"], function (PhotoAlbumModel) {
    var PhotoAlbumsCollection = Backbone.Collection.extend({
        initialize: function (models, options) {
            this.Id = options.PageId;
        },
        url: function () {
            return "api/users/albums/" + this.Id
        },
        model: PhotoAlbumModel,
        SetAccessor: function (model) {
            this._parent = model;
        },
        GetAccessor: function () {
            return this._parent;
        },
    });
    return PhotoAlbumsCollection;
});
