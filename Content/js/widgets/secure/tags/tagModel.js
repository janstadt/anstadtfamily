define([], function () {
    _.extend(Backbone.Validation.validators, {
        unique: function (value, attr, customValue, model) {
            if (!model.get("Unique")) {
                return "Tag already exits.";
            }
        }
    });
    var TagModel = Backbone.Model.extend({
        urlRoot: "api/tags/tag",
        idAttribute: "Id",
        validation: {
            Name: {
                required: true,
                unique: true
            }
        }
    });
    return TagModel;
});
