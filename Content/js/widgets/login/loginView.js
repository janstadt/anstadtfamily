define([
    "./loginModel",
    "text!./loginTemplate.html",
    "i18n!./nls/login"//,
//    "../modal/modalView",
//    "../modal/modalModel"
], function (
    loginModel,
    template,
    i18n//,
//modalView, modalModel
    ) {
    var LoginView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        child: false,

        initialize: function (defaults) {
            if (!this.model) {
                this.model = new loginModel();
            }
            Backbone.Validation.bind(this);
            if (defaults) {
                this.child = defaults.child;
                this.model.set(defaults.ReturnUrl);
            }
            this.model.on("change", this.render, this);
            this.model.fetch();
        },

        events: {
            "click #signIn": "submit",
            "keypress input[type='password'], input[type='text']": "onEnter"
        },

        render: function () {
            var modelJson = this.model.toJSON();
            if (modelJson.LoginStatus === 1 && !this.child) {
                application.router.navigate("#");
            }
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            return this;
        },

        submit: function (e) {
            e.preventDefault();
            this.$("#loginError").addClass("hide");
            var data = this.$("#login form").serializeObject();
            this.model.save(data, { validate: true, success: _.bind(this.success, this), error: _.bind(this.error, this) });
        },

        onEnter: function (e) {
            if (e.which === 13) {
                this.submit(e);
            }
        },

        success: function (model, response, options) {
            if (!this.child) {
                model.trigger("loggedIn");
                var returnUrl = this.model.get("ReturnUrl") || "#/";
                application.router.navigate(returnUrl);
            }
        },

        logout: function () {
            this.model.trigger("loggedOut");
            this.model.destroy({ success: this.logoutSuccess });
        },

        logoutSuccess: function (model, response) {
            application.router.navigate("#/");
            window.location.reload();
        },

        error: function (model, response, options) {
            //map response.status to invalidState;
            var error = this.$("#loginError");
            switch (response.status) {
                case 423:
                    error.html(this.i18n.userLockedOut);
                    break;
                case 403:
                    error.html(this.i18n.invalidUser);
                    break;
                default:
                    error.html(this.i18n.generalError);
                    break;
            }
            error.removeClass("hide");
        }
    });
    return LoginView;
});
