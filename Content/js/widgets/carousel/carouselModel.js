define([], function () {

    var CarouselModel = Backbone.Model.extend({
        defaults: {
            "ShowIndicator": true,
            "ShowNavigation": true
        }
    });
    return CarouselModel;
});
