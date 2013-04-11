define([], function () {

    var SessionModel = Backbone.Model.extend({
        url: "/api/secure/current"
    });
    return SessionModel;
});
