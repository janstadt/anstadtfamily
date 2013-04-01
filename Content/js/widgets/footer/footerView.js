define([
    "text!./footerTemplate.html",
    "i18n!./nls/footer"
], function (
    template,
    i18n) {
    var FooterView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        initialize: function (defaults) {
            this.render();
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            return this;
        }

    });
    return FooterView;
});
