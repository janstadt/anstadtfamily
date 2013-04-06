define([
    "./photoAlbumInfoView",
    "./photoAlbumModel",
    "../photos/photosView",
    "../photos/photosCollection",
    "../../breadcrumbs/breadcrumbsView",
    "../../breadcrumbs/breadcrumbsModel"
], function (
    PhotoAlbumInfoView,
    PhotoAlbumModel,
    PhotosView,
    PhotoCollection,
    BreadcrumbView,
    asdf) {
    var PhotoAlbumView = Backbone.View.extend({
        photoAlbumInfoView: null,
        photosView: null,
        photoAlbumModel: null,
        photoCollection: null,
        initialize: function (options) {
            this.options = options;
            //new up a user view;
            this.photoAlbumModel = new PhotoAlbumModel({ "Owner": this.options.PageId, "Id": this.options.SubId });
            this.listenTo(this.photoAlbumModel, "change", this.addAlbumInfo);

            this.photoAlbumInfoView = new PhotoAlbumInfoView({ model: this.photoAlbumModel, userId: this.options.PageId });
            //window.application.showLoading(this.photoAlbumInfoView.el);
            return this;
        },

        _setBreadcrumbs: function () {
            var bcModel = new asdf({ "Id": this.options.SubId, "Type": "Albums" });
            var breadCrumbs = new BreadcrumbView({ "model": bcModel });
            this.$el.prepend(breadCrumbs.el);
        },

        addAlbumInfo: function () {
            //window.application.hideLoading(this.photoAlbumInfoView.el);
            $(this.el).append(this.photoAlbumInfoView.el);
            this.stopListening(this.userModel);
            this.addPhotos();
            this._setBreadcrumbs();
        },

        addPhotos: function () {
            //new up user photos
            this.photoCollection = new PhotoCollection([], this.options);
            this.photosView = new PhotosView({ collection: this.photoCollection, userId: this.options.PageId, albumId: this.options.SubId });
            $(this.el).append(this.photosView.el);
            this.displayControls();
        },

        displayControls: function () {
            var user = window.application.user;
            if (user.AccessLevel === 1 || user.Id === this.photoAlbumModel.get("Owner")) {
                this.photosView.showControls();
            }
        }
    });
    return PhotoAlbumView;
});
