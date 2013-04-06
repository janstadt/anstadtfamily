define([], function () {
    var BreadcrumbModel = Backbone.Model.extend({
        urlRoot: function () {
            return "api/" + this.get("Type") + "/breadcrumb/";
        },
        idAttribute: "Id"
    });
    return BreadcrumbModel;
});
