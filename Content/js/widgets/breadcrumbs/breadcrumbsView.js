define([
    "text!./breadcrumbsTemplate.html",
    "text!./breadcrumbTemplate.html",
    "i18n!./nls/breadcrumbs",
    "./breadcrumbsModel"
], function (
    template,
    crumbTemplate,
    i18n,
    BreadcrumbModel) {
    var BreadcrumbsView = Backbone.View.extend({
        template: _.template(template),
        crumbTemplate: _.template(crumbTemplate),
        i18n: i18n,
        model: null,
        initialize: function (model, defaults) {
            this.render();
            if (defaults && defaults.Crumbs) {
                this.addCrumbs(defaults.Crumbs);
            }
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            return this;
        },

        addCrumbs: function (crumbs) {
            var ul = this.$(".breadcrumb"),
            self = this;
            _.each(crumbs, function (crumb) {
                ul.append(self.crumbTemplate(crumb));
            });
        }

    });
    return BreadcrumbsView;
});
