define(["../photoAlbum/photoAlbumModel"], function (PhotoAlbumModel) {
    var PhotoAlbumsCollection = Backbone.Collection.extend({
        initialize: function (models, options) {
            this.Id = options.PageId;
        },
        url: function () {
            return "api/user/albums/" + this.Id
        },
        model: PhotoAlbumModel
    });
    return PhotoAlbumsCollection;
});
