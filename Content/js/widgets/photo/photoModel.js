define([], function () {
    var PhotoModel = Backbone.Model.extend({
        urlRoot: "api/photos/photo",
        idAttribute: "Id",
        SetAccessor: function (model) {
            this._parent = model;
        },
        GetAccessor: function (model) {
            return this._parent;
        }
    });
    return PhotoModel;
});
