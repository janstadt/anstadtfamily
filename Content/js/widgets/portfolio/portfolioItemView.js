define([
    "./portfolioModel",
    "text!./portfolioItemTemplate.html",
    "i18n!./nls/portfolio"
], function (
    portfolioModel,
    template,
    i18n) {
    var PortfolioItemView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        content: null,
        initialize: function (defaults) {
            this.render();
        },
        render: function () {
            $(this.el).html(this.template({ "model": this.model, "i18n": this.i18n }));
            return this;
        }
    });
    return PortfolioItemView;
});
