define(["./portfolioModel"], function (PortfolioModel) {
    var PortfolioCollection = Backbone.Collection.extend({
        initialize: function (models, options) {
            this.Id = options.PageId;
        },
        url: function () {
            return "api/albums/portfolio/" + (this.Id || "")
        },
        model: PortfolioModel
    });
    return PortfolioCollection;
});
