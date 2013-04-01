define([], function () {

    var CarouselModel = Backbone.Model.extend({
        defaults: {
            "ShowIndicator": false,
            "ShowNavigation": false,
            Items: [],
        },
        urlRoot: "api/albums/slideshows"
    });
    return CarouselModel;
});
