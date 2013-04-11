define([
    "./userView",
    "../user/userModel",
    "text!./usersTemplate.html",
    "text!./userListTemplate.html",
    "./userCollection",
    "i18n!./nls/users",
    "../../modal/modalView",
    "../../modal/modalModel",
    "../../login/sessionModel"
], function (
    UserView,
    UserModel,
    template,
    userListTemplate,
    UserCollection,
    i18n,
    ModalView,
    ModalModel,
    SessionModel) {
    var UsersView = Backbone.View.extend({
        model: null,
        modal: null,
        sessionModel: null,
        userView: null,
        i18n: i18n,
        className: "user-collection",
        template: _.template(template),
        userListTemplate: _.template(userListTemplate),
        initialize: function (options) {
            this.options = options;

            this.render();
            this.sessionModel = new SessionModel();
            this.listenTo(this.sessionModel, "change", this.setAccessor);
            this.sessionModel.fetch();
            return this;
        },

        setAccessor: function () {
            this.userList = this.$el.find("#users-list-body");
            this.collection = new UserCollection();
            this.listenTo(this.collection, "reset", _.bind(this.addAll, this));
            this.collection.fetch();
        },

        events: {
            "click tbody > tr": "editUser",
            "click a.create": "createUser"
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            return this;
        },

        createUser: function (evt) {
            evt.preventDefault();
            var model = new UserModel({ "AccessLevel": 2, "IsNew": true });
            model.SetAccessor(this.sessionModel.toJSON());
            var uView = new UserView({ "model": model });
            this.listenTo(uView, "finished", this.userAdded);

            var mModel = new ModalModel({ "Id": "userModal", "Content": uView.el, "Title": i18n.Create });
            this.modal = new ModalView({ "model": mModel });
            this.$("#userModalContainer").html(this.modal.el);
            this.modal.show();
        },

        userAdded: function (model) {
            this.modal.hide();
            this.collection.add(model);
            this.addAll();
        },

        userUpdated: function (model) {
            this.modal.hide();
            this.addAll();
            //update item in row.
        },

        editUser: function (evt) {
            if (evt.target.nodeName !== "A") {
                evt.preventDefault();
                var id = evt.currentTarget.id;
                var model = this.collection.find(function (item) { return id === item.get("Id"); });
                model.SetAccessor(this.sessionModel.toJSON());
                var uView = new UserView({ "model": model });
                this.listenTo(uView, "finished", this.userUpdated);

                var mModel = new ModalModel({ "Id": "userModal", "Content": uView.el, "Title": model.toJSON().Username });
                this.modal = new ModalView({ "model": mModel });
                this.$("#userModalContainer").html(this.modal.el);
                this.modal.show();
            }
        },

        addOne: function (model) {
            this.userList.append(this.userListTemplate({ "model": model.toJSON(), "i18n": this.i18n }));
        },

        addAll: function () {
            this.userList.html("");
            this.collection.each(_.bind(this.addOne, this));
        }
    });
    return UsersView;
});
