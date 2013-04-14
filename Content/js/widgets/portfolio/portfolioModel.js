define([], function () {
    var PortfolioModel = Backbone.Model.extend({
        urlRoot: "api/albums/portfolio",
        idAttribute: "Id"
    });
    return PortfolioModel;
});
