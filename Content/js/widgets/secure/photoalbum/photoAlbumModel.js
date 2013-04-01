define([], function () {
    var PhotoAlbumModel = Backbone.Model.extend({
        urlRoot: "api/albums/album",
        idAttribute: "Id",
        validation: {
            Title: {
                required: true
            }
        }
    });
    return PhotoAlbumModel;
});
