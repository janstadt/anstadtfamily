define([
    "./userView",
    "./userModel",
    "text!./usersTemplate.html",
    "text!./userListTemplate.html",
    "./userCollection",
    "i18n!./nls/users",
    "../../modal/modalView",
    "../../modal/modalModel"
], function (
    UserView,
    UserModel,
    template,
    userListTemplate,
    UserCollection,
    i18n,
    ModalView,
    ModalModel) {
    var UsersView = Backbone.View.extend({
        model: null,
        userView: null,
        i18n: i18n,
        className: "user-collection",
        template: _.template(template),
        userListTemplate: _.template(userListTemplate),
        initialize: function (options) {
            this.options = options;
            this.render();
            this.userList = this.$el.find("#users-list-body");
            this.collection = new UserCollection();
            this.listenTo(this.collection, "reset", _.bind(this.addAll, this));
            this.collection.fetch();
            return this;
        },

        events: {
            "click tr": "editUser",
            "click a.create": "createUser"
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            return this;
        },

        createUser: function (evt) {
            evt.preventDefault();
            
        },

        updateUser: function (model) {

        },

        editUser: function (evt) {
            evt.preventDefault();
            var id = evt.currentTarget.id;
            var model = this.collection.find(function (item) { return id === item.get("Id"); });
            var uView = new UserView({ "model": model });
            this.listenTo(uView, "updated", this.updateUser);

            var mModel = new ModalModel({ "Id": "userModal", "Content": uView.el, "Title": model.toJSON().Name });
            this.modal = new ModalView({ "model": mModel });
            this.$("#userModalContainer").html(this.modal.el);
            this.modal.show();
        },

        addOne: function (model) {
            this.userList.append(this.userListTemplate({ "model": model.toJSON() }));
        },

        addAll: function () {
            this.collection.each(_.bind(this.addOne, this));
        }
    });
    return UsersView;
});
