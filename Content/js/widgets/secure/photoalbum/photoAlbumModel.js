define([], function () {
    var PhotoAlbumModel = Backbone.Model.extend({
        urlRoot: "api/albums/album",
        idAttribute: "Id",
        validation: {
            Title: {
                required: true
            }
        },
        SetAccessor: function (model) {
            this._parent = model;
        },
        GetAccessor: function (model) {
            return this._parent;
        }
    });
    return PhotoAlbumModel;
});
