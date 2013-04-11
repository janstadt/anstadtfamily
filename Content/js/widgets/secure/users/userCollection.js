define(["../user/userModel"], function (userModel) {
    var UserCollection = Backbone.Collection.extend({
        url: "api/users",
        model: userModel,
        exists: function (name, type) {
            return this.find(function (item) {
                return (item.get("Name").toUpperCase().trim() === name.toUpperCase().trim() && item.get("Type") === parseInt(type, 10));
            });
        }
    });
    return UserCollection;
});
