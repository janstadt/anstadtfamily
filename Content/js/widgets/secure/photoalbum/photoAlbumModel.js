define([], function () {
    var PhotoAlbumModel = Backbone.Model.extend({
        urlRoot: "api/albums/photoalbum",
        idAttribute: "Id",
        validation: {
            Title: {
                required: true
            },
            Date: {
                required: true
            },
            Description: {
                required: true
            }
    },
    SetAccessor: function (model) {
        this._parent = model;
    },
    GetAccessor: function () {
        return this._parent;
    }
});
return PhotoAlbumModel;
});
