define(["../../photo/photoModel"], function (PhotosModel) {
    var PhotosCollection = Backbone.Collection.extend({
        initialize: function (models, options) {
            this.Id = options.PageId;
            this.SubId = options.SubId;
        },
        url: function () {
            return "api/albums/photos/" + this.SubId
        },
        model: PhotosModel,
        SetAccessor: function (model) {
            this._parent = model;
        },
        GetAccessor: function () {
            return this._parent;
        },
        UpdateMain: function (model) {
            this.forEach(function (item) {
                if (item.get("Id") !== model.get("Id")) {
                    item.set({ "MainImage": false });
                }
            });
        }
    });
    return PhotosCollection;
});
