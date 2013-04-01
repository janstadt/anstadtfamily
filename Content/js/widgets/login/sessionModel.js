define([], function () {

    var SessionModel = Backbone.Model.extend({
        url: "/api/secure/session"
    });
    return SessionModel;
});
