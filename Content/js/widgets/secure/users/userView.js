define([
    "./userModel",
    "text!./userTemplate.html",
    "i18n!./nls/users"
], function (
    UserModel,
    template,
    i18n) {
    var UserView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        initialize: function (options) {
            Backbone.Validation.bind(this);
            this.render();
        },

        events: {
            "click a.edit": "edit",
            "click a.cancel": "cancel",
            "click a.save": "save",
            "click a.addalbum": "addAlbum"
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n, "model": this.model.toJSON() }));
            return this;
        },

        edit: function () {
            this.$(".error").removeClass("error");
            this.$(".user-info").addClass("editing");
        },

        cancel: function () {
            this.render();
        },

        save: function () {
            this.$("#saveError").addClass("hide");
            this.$(".error").removeClass("error");
            var data = this.$(".user-info").serializeObject();
            this.model.save(data, { validate: true, success: _.bind(this.saveSuccess, this), error: _.bind(this.saveError, this) });
        },

        addAlbum: function () {

        },

        saveSuccess: function (model, response, options) {
            this.$(".user-info").removeClass("editing");
            this.trigger("finished");
        },

        saveError: function (model, response, options) {
            this.$(".user-info").addClass("editing");
            this.$("#saveError").removeClass("hide");
        }
    });
    return UserView;
});
