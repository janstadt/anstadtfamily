define([
    "text!./footerTemplate.html",
    "i18n!./nls/footer",
    "../modal/modalView",
    "../modal/modalModel",
    "../contactus/contactUsView"
], function (
    template,
    i18n,
    modalView,
    modalModel,
    contactUsView) {
    var FooterView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        contactUsView: null,
        initialize: function (defaults) {
            this.render();
            this.contact();
        },

        //        events: {
        //            "click a#contactUs": "contact"
        //        },

        contact: function () {
            this.contactUsView = new contactUsView();
            var mModel = new modalModel({ "Id": "contactUsModal", "Content": this.contactUsView.el, "Title": this.i18n.ContactUs });
            this.modal = new modalView({ "model": mModel });
            this.$("#contactUs-modal").html(this.modal.el);
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            return this;
        }

    });
    return FooterView;
});
