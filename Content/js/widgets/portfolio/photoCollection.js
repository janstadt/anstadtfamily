define(["./photoModel"], function (PhotoModel) {
    var PhotoCollection = Backbone.Collection.extend({
        model: PhotoModel,
        SetAccessor: function (model) {
            this._parent = model;
        },
        GetAccessor: function () {
            return this._parent;
        }
    });
    return PhotoCollection;
});