define(["./slideshowModel"], function (slideshowModel) {
    var SlideshowCollection = Backbone.Collection.extend({
        url: "api/photos/slideshow",
        model: slideshowModel
    });
    return SlideshowCollection;
});
