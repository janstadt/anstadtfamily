define([], function () {
    var SlideshowCollection = Backbone.Collection.extend({
        url: "api/photos/slideshow"
    });
    return SlideshowCollection;
});
