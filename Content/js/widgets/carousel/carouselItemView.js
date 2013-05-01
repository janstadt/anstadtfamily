define([
    "text!./carouselItemTemplate.html",
    "i18n!./nls/carousel"
], function (
    template,
    i18n) {
    var CarouselItemView = Backbone.View.extend({
        model: null,
        i18n: i18n,
        className: function () {
            return this.model.get("First") ? "item active" : "item"; 
        },
        attributes: function () {
            return { id: this.model.get("Id") };
        },
        template: _.template(template),
        initialize: function (model, options) {
            this.render();
        },
        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n, "model": this.model.toJSON() }));
            return this;
        }
    });
    return CarouselItemView;
});
