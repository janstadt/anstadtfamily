define(["../photo/photoModel"], function (PhotoModel) {
    var PhotoCollection = Backbone.Collection.extend({
        model: PhotoModel,
        setFirst: function (id) {
            this.each(function (item) {
                if (item.get("Id") !== id) {
                    item.set({"First": false}, {"silent": true});
                } else {
                    item.set({ "First": true }, { "silent": true });
                }
            });
        }
    });
    return PhotoCollection;
});
