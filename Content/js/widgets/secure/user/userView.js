define([
    "./userInfoView",
    "./userModel",
    "../photoAlbums/photoAlbumsView",
    "../photoAlbums/photoAlbumsCollection"
], function (
    UserInfoView,
    UserModel,
    PhotoAlbumsView,
    PhotoAlbumsCollection) {
    var UserView = Backbone.View.extend({
        model: null,
        userInfoView: null,
        photoAlbumsView: null,
        initialize: function (options) {
            this.options = options;
            //new up a user view;
            this.userModel = new UserModel({ "Id": this.options.PageId });
            this.listenTo(this.userModel, "change", this.addUserInfo);

            this.userInfoView = new UserInfoView({ model: this.userModel, userId: this.options.PageId });
            return this;
        },

        addUserInfo: function () {
            $(this.el).append(this.userInfoView.el);
            this.getPhotos();
            //get the user info once and then stop listening to it.
            this.stopListening(this.userModel);
        },

        getPhotos: function () {
            //new up user photos
            this.photoAlbumsCollection = new PhotoAlbumsCollection([], this.options);
            this.listenTo(this.photoAlbumsCollection, "reset", this.displayControls);

            this.photoAlbumsView = new PhotoAlbumsView({ collection: this.photoAlbumsCollection, userId: this.options.PageId });
            $(this.el).append(this.photoAlbumsView.el);
        },

        displayControls: function () {
            var accessLevel = this.userModel.get("AccessLevel");
            if (accessLevel === 1 || accessLevel === 2) {
                this.photoAlbumsView.showControls();
            }
        }
    });
    return UserView;
});
