define([
    "./photoAlbumInfoView",
    "./photoAlbumModel",
    "../photos/photosView",
    "../photos/photosCollection",
    "../../breadcrumbs/breadcrumbsView",
    "../../breadcrumbs/breadcrumbsModel",
    "../../login/sessionModel"
], function (
    PhotoAlbumInfoView,
    PhotoAlbumModel,
    PhotosView,
    PhotoCollection,
    BreadcrumbView,
    BreadcrumbModel,
    SessionModel) {
    var PhotoAlbumView = Backbone.View.extend({
        photoAlbumInfoView: null,
        photosView: null,
        photoAlbumModel: null,
        photoCollection: null,
        initialize: function (options) {
            this.options = options;
            //new up a user view;
            this.sessionModel = new SessionModel();
            this.listenTo(this.sessionModel, "change", this.setAccessor);
            this.sessionModel.fetch();


            return this;
        },

        setAccessor: function () {
            //Set the person who is accessing the object to the model.
            this.photoAlbumModel = new PhotoAlbumModel({ "Owner": this.options.PageId, "Id": this.options.SubId });
            this.photoAlbumModel.SetAccessor(this.sessionModel.toJSON());

            this.listenTo(this.photoAlbumModel, "change", this.addAlbumInfo);
            this.photoAlbumInfoView = new PhotoAlbumInfoView({ model: this.photoAlbumModel, userId: this.options.PageId });
        },

        _setBreadcrumbs: function () {
            var bcModel = new BreadcrumbModel({ "Id": this.options.SubId, "Type": "Albums" });
            var breadCrumbs = new BreadcrumbView({ "model": bcModel });
            this.$el.prepend(breadCrumbs.el);
        },

        addAlbumInfo: function () {
            $(this.el).append(this.photoAlbumInfoView.el);
            this.stopListening(this.userModel);
            this.addPhotos();
            this._setBreadcrumbs();
        },

        addPhotos: function () {
            //new up user photos
            this.photoCollection = new PhotoCollection([], this.options);
            this.photoCollection.SetAccessor(this.sessionModel.toJSON());
            this.photosView = new PhotosView({ collection: this.photoCollection, userId: this.options.PageId, albumId: this.options.SubId });
            $(this.el).append(this.photosView.el);
            //this.displayControls();
        }//,

//        displayControls: function () {
//            var user = window.application.user;
//            if (user.AccessLevel === 1 || user.AccessLevel === 2) {
//                this.photosView.showControls();
//            }
//        }
    });
    return PhotoAlbumView;
});
