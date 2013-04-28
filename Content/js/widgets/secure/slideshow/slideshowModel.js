define([], function () {
    var SlideshowModel = Backbone.Model.extend({
        urlRoot: "api/photos/slideshow/",
        idAttribute: "Id"
    });
    return SlideshowModel;
});
