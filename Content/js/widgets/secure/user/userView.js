define([
    "./userInfoView",
    "./userModel",
    "../photoAlbums/photoAlbumsView",
    "../photoAlbums/photoAlbumsCollection",
    "../../login/sessionModel"
], function (
    UserInfoView,
    UserModel,
    PhotoAlbumsView,
    PhotoAlbumsCollection,
    SessionModel) {
    var UserView = Backbone.View.extend({
        model: null,
        sessionModel: null,
        userInfoView: null,
        photoAlbumsView: null,
        initialize: function (options) {
            this.options = options;
            this.sessionModel = new SessionModel();
            this.listenTo(this.sessionModel, "change", this.setAccessor);
            this.sessionModel.fetch();
            return this;
        },

        addUserInfo: function () {
            $(this.el).append(this.userInfoView.el);
            this.getPhotos();
            //get the user info once and then stop listening to it.
            this.stopListening(this.userModel);
        },

        setAccessor: function () {
            //Set the person who is accessing the object to the model.
            this.userModel = new UserModel({ "Id": this.options.PageId });
            this.userModel.SetAccessor(this.sessionModel.toJSON());

            this.listenTo(this.userModel, "change", this.addUserInfo);
            this.userInfoView = new UserInfoView({ model: this.userModel, userId: this.options.PageId });
        },

        getPhotos: function () {
            //new up user photos
            this.photoAlbumsCollection = new PhotoAlbumsCollection([], this.options);
            this.photoAlbumsCollection.SetAccessor(this.sessionModel.toJSON());
            //this.listenTo(this.photoAlbumsCollection, "reset", this.displayControls);
            this.photoAlbumsView = new PhotoAlbumsView({ collection: this.photoAlbumsCollection, userId: this.options.PageId });
            $(this.el).append(this.photoAlbumsView.el);
        },

        displayControls: function () {
            var accessLevel = this.userModel.get("AccessLevel");
            if (accessLevel === 1 || accessLevel === 2 || accessLevel === 5) {
                this.photoAlbumsView.showControls();
            }
        }
    });
    return UserView;
});
