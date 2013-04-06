define(["./tagModel"], function (tagModel) {
    var TagCollection = Backbone.Collection.extend({
        url: "api/tags",
        model: tagModel,
        exists: function (name, type) {
            return this.find(function (item) {
                return (item.get("Name").toUpperCase().trim() === name.toUpperCase().trim() && item.get("Type") === parseInt(type, 10));
            });
        }
    });
    return TagCollection;
});
