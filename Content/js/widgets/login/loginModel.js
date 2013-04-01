define(["i18n!./nls/login"], function (i18n) {

    var LoginModel = Backbone.Model.extend({
        i18n: i18n,
        defaults: {
            Username: null,
            Password: null,
            Id: null
        },

        idAttribute: "Id",

        initialize: function () {
            this._setValidation();
        },

        _setValidation : function () {
            this.validation = {
                Username: {
                required: true,
                msg: this.i18n.invalidUsername
                },
                Password: {
                    required: true,
                    msg: this.i18n.invalidPassword
                }
            };
        },

        url: "/api/secure/login"
    });
    return LoginModel;
});
