define(["./tagModel"], function (tagModel) {
    var TagCollection = Backbone.Collection.extend({
        url: "api/tags",
        model: tagModel
    });
    return TagCollection;
});
