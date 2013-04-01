define([], function () {
    var Router = Backbone.Router.extend({
        routes: {
            ":page(/:id/)": "page",
            ":page(/:id)": "page",
            ":page(/:id/:action/)": "page",
            ":page(/:id/:action)": "page",
            ":page(/:id/:subpage/:subId/)": "subpage",
            ":page(/:id/:subpage/:subId)": "subpage",
            "*default": "default"
        },

        start: function () {
            Backbone.history.start();
        }

    });
    return Router;
});