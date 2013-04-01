define(["./listNavigationModel"], function (model) {

    var ListNavigationCollection = Backbone.Collection.extend({
        model: model,
        initialize: function (models, options) {

        },
        setSelected: function (item) {
            this.forEach(function(i){
                i.set({"Selected":false}, {"silent": true});
            });
            item.set({"Selected": true});
        },
    });
    return ListNavigationCollection;
});
