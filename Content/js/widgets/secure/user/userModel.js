define([], function () {
    var UserModel = Backbone.Model.extend({
        urlRoot: "api/users/user",
        idAttribute: "Id",
        SetAccessor: function (model) {
            this._parent = model;
        },
        GetAccessor: function (model) {
            return this._parent;
        },
        validation: {
            Username: {
                required: true
            },
            Name: {
                required: true
            },
            Email: {
                required: true,
                pattern: "email"
            },
            Phone: {
                required: false,
                pattern: /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/,
                msg: "Phone number must be valid"
            },
            Password: {
                required: function (value, attr, computedState) {
                    var aLevel = this.get("AccessLevel");
                    if (aLevel === 2 || aLevel === 3) {
                        return true;
                    }
                    return false;
                }
            },
            PasswordConfirm: {
                equalTo: "Password"
            }
        }
    });
    return UserModel;
});
