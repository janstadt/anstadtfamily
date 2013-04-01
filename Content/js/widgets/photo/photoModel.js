define([], function () {
    var PhotoModel = Backbone.Model.extend({
        urlRoot: "api/photos/photo",
        idAttribute: "Id"
    });
    return PhotoModel;
});
