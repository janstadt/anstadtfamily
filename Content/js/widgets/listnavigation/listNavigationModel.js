define([], function () {

    var ListNavigationModel = Backbone.Model.extend({
        defaults: {
            Id: null,
            Label: null,
            Widget: null,
            Selected: false
        },

        idAttribute: "Id"
    });
    return ListNavigationModel;
});
