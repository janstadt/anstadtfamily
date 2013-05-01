define([
    "./headerModel",
    "text!./headerTemplate.html",
    "text!./categoriesTemplate.html",
    "i18n!./nls/header",
    "../modal/modalView",
    "../modal/modalModel",
    "../login/loginView",
    "../login/loginModel",
    "../carousel/carouselView"
], function (
    headerModel,
    template,
    categoriesTemplate,
    i18n,
    modalView,
    modalModel,
    loginView,
    loginModel,
    carouselView) {
    var HeaderView = Backbone.View.extend({
        template: _.template(template),
        categoriesTemplate: _.template(categoriesTemplate),
        i18n: i18n,
        model: null,
        modal: null,
        login: null,
        loginModel: null,
        carouselView: null,
        initialize: function (defaults) {
            this.model = new headerModel();
            if (defaults) {
                this.model.set(defaults);
            }
            this.render();

            this._setupLogin();
            this._setupSession();
            this._setupCarousel();
        },

        events: {
            //"click #categorieslink": "toggleCategories",
            // "mouseleave #categories": "toggleCategories",
            "click #logout": "logout"
        },

        setupAffix: function () {
            this.$("#navigation").height(this.$("#navigation-inner").height());
            this.affix();
        },

        affix: function () {
            this.$("#navigation-inner").affix({ offset: { left: 0, top: function () {
                var mast, slider, height;
                height = $(".masthead").outerHeight() + $("#header-slide").outerHeight();
                return height;
            }
            }
            });
        },

        _setupSession: function () {
            this.loginModel.on("change", this._updateLinks, this);
        },

        _setupCarousel: function () {
            this.carouselView = new carouselView({ "Id": "header-carousel", "ShowIndicator": true, "ShowNavigation": true });
            $("#header-slide").html(this.carouselView.el);
            this.listenTo(this.carouselView, "finished", this.setupAffix);
        },

        _setupLogin: function () {
            this.loginModel = new loginModel();
            this.login = new loginView({ "model": this.loginModel, "child": true });

            this.loginModel.on("loggedIn", this.loggedIn, this);
            this.loginModel.on("loggedOut", this.loggedOut, this);

            var mModel = new modalModel({ "Id": "loginModal", "Content": this.login.el, "Title": this.i18n.Login });
            this.modal = new modalView({ "model": mModel });
            $("#login-modal").html(this.modal.el);

            this.modal.setCallbacks({
                callbacks: {
                    "shown": this._focusOnUser
                },
                scope: this
            });
        },

        _focusOnUser: function () {
            this.login.$("#username").focus();
        },

        _updateLinks: function () {
            var json = this.loginModel.toJSON();
            json.Password = "";
            window.application.user = json;
            if (json.LoginStatus !== 1) {
                this.loggedOut();
            } else {
                this.loggedIn();
            }

            this._addCategories();
        },

        _addCategories: function () {
            var ul = this.$(".categories-dropdown");
            ul.empty();
            var categories = this.loginModel.toJSON().Categories;

            _.forEach(categories, _.bind(function (category) {
                ul.append(this.categoriesTemplate(category));
            }, this));
        },

        select: function (page) {
            this.$("#nav-list > li").removeClass("active");
            this.$("#nav-list > li#header-" + page.name).addClass("active");
            if (page.hideCarousel) {
                this.carouselView.hide();
            } else {
                this.carouselView.show();
            }
        },

        render: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            return this;
        },

        logout: function () {
            this.login.logout();
        },

        loggedIn: function () {
            //for now hide modal. add some user link to header.
            this.$("#loginLi").addClass("hide");
            this.$("#logoutLi").removeClass("hide");
            var accessLevel = this.loginModel.get("AccessLevel");
            if (accessLevel == 1 || accessLevel == 2) {
                this.$("#admin").removeClass("hide");
            }
            var href = this.$("#settings").attr("href");
            href = href + "/" + this.loginModel.get("Id");
            this.$("#settings").attr("href", href);
            this.modal.hide();
            this.$("#userInfo").text(this.loginModel.get("Username"));
        },

        loggedOut: function () {
            this.$("#logoutLi").addClass("hide");
            this.$("#admin").addClass("hide");
            this.$("#loginLi").removeClass("hide");
        },

        toggleCategories: function (evt) {
            var list = $(this.el).find("#categories");
            list.toggleClass("hideState");
        }
    });
    return HeaderView;
});
