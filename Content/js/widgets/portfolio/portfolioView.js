define([
    "./portfolioCollection",
    "text!./portfolioTemplate.html",
    "i18n!./nls/portfolio",
    "./portfolioItemView"
], function (
    portfolioCollection,
    landingTemplate,
    i18n,
    portfolioItemView) {
    var PortfolioView = Backbone.View.extend({
        landingTemplate: _.template(landingTemplate),
        i18n: i18n,
        collection: null,
        content: null,
        initialize: function (defaults) {
            this.collection = new portfolioCollection([], defaults);
            this.render();
            this.collection.fetch({ success: _.bind(this.success, this), error: _.bind(this.error, this) });
        },
        render: function () {
            $(this.el).html(this.landingTemplate({ "i18n": this.i18n }));
            return this;
        },

        addOne: function (model) {
            var pView = new portfolioItemView({ "model": model });
            this.$("#portfolioItems").append(pView.el);
        },

        success: function (collection, response) {
            this.collection.forEach(this.addOne);
            $(window).resize();
        },
        error: function (model, response) {
            var asdf = "asdf";
        }

    });
    return PortfolioView;
});