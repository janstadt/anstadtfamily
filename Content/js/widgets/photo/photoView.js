define([
    "./photoModel",
    "text!./photoTemplate.html",
    "i18n!./nls/photo"
], function (
   photoModel,
    template,
    i18n) {
    var PhotoView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        content: null,
        initialize: function (defaults) {
            this.model = new photoModel();
            this.model.fetch({ success: _.bind(this.success, this), error: _.bind(this.error, this) });
        },
        success: function (model, response) {
            var asdf = "asdf";
        },
        error: function (model, response) {
            var asdf = "asdf";
        }

    });
    return PhotoView;
});
