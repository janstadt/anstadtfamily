define([], function () {
    var TagModel = Backbone.Model.extend({
        urlRoot: "api/tags/tag",
        idAttribute: "Id"
    });
    return TagModel;
});
