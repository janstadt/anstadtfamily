define([], function () {
    var AlbumModel = Backbone.Model.extend({
        urlRoot: function () {
            return "api/albums/album/" + this.get("Title") + "/?Type=" + this.get("Type") 
        }
    });
    return AlbumModel;
});
