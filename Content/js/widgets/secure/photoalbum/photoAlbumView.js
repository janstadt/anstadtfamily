define([
    "./photoAlbumInfoView",
    "./photoAlbumModel",
    "../photos/photosView",
    "../photos/photosCollection",
    "../../breadcrumbs/breadcrumbsView"
], function (
    PhotoAlbumInfoView,
    PhotoAlbumModel,
    PhotosView,
    PhotoCollection,
    BreadcrumbView) {
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
            var crumbs = [];
            var breadCrumbs = new BreadcrumbView();
            var links = [];
            links[0] = {
                Url: "/#/user/" + this.options.PageId,
                Text: "Your Albums",
                Class: ""
            };
            links[1] = {
                Url : "#",
                Class: "active",
                Text: this.photoAlbumModel.get("Title")
            };
            breadCrumbs.addCrumbs(links);
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
