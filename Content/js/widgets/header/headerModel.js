define([], function () {

    var HeaderModel = Backbone.Model.extend({
        defaults: {
            "selectedTab" : "home",
            "slideshow" : []
        },
        url: ""
    });
    return HeaderModel;
});
