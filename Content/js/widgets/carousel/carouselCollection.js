define(["./carouselModel"], function (slideshowModel) {
    var SlideshowCollection = Backbone.Collection.extend({
        url: "api/photos/slideshow",
        model: slideshowModel,
        initialize: function (models, defaults) {
            if (defaults) {
                
            }
        }
    });
    return SlideshowCollection;
});
