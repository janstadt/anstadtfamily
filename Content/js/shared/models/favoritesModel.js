define([], function () {
    var FavoritesModel = Backbone.Model.extend({
        urlRoot: function () {
            return "/api/" + this.get("type") + "/favorite";
        }
    });
    return FavoritesModel;
});
