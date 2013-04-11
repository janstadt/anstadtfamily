define([], function () {
    var UserModel = Backbone.Model.extend({
        urlRoot: "api/users/user",
        idAttribute: "Id",
        validation: {
            Name: {
                required: true
            },
            Email: {
                required: true,
                pattern: "email"
            }
        }
    });
    return UserModel;
});
