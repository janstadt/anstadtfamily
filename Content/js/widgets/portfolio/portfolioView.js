define([
    "./portfolioModel",
    "text!./portfolioLandingTemplate.html",
    "i18n!./nls/portfolio"
], function (
    portfolioModel,
    landingTemplate,
    i18n) {
    var PortfolioView = Backbone.View.extend({
        landingTemplate: _.template(landingTemplate),
        i18n: i18n,
        model: null,
        content: null,
        initialize: function (defaults) {
            this.model = new portfolioModel({ "Id": defaults.PageId });
            this.render();
            if (!this.model.isNew()) {
                this.model.fetch({ success: _.bind(this.success, this), error: _.bind(this.error, this) });    
            }
        },
        render: function () {
            $(this.el).html(this.landingTemplate({ "model": this.model.toJSON(), "i18n": this.i18n }));
            return this;
        },
        success: function (model, response) {
            var asdf = "asdf";
        },
        error: function (model, response) {
            var asdf = "asdf";
        }

    });
    return PortfolioView;
});
