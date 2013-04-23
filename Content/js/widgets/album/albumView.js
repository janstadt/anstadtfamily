define([
    "./albumModel",
    "text!./albumTemplate.html",
    "i18n!./nls/album"
], function (
    albumModel,
    template,
    i18n) {
    var AlbumView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        content: null,
        initialize: function (defaults) {
            this.model = new albumModel({ "Title": defaults.SubId, "Type": defaults.PageId });
            this.model.fetch({ success: _.bind(this.success, this), error: _.bind(this.error, this) });
        },
        success: function (model, response) {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            return this;
        },
        error: function (model, response) {
            var asdf = "asdf";
        }

    });
    return AlbumView;
});
