define([
    "./contactUsModel",
    "text!./contactUsTemplate.html",
    "i18n!./nls/contactUs"
], function (
    contactUsModel,
    template,
    i18n
    ) {
    var ContactUsView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        child: false,

        initialize: function (defaults) {
            this.model = new contactUsModel();
            Backbone.Validation.bind(this);
            this._number();
            this.render();
        },

        events: {
            "click #submit": "submit"
        },

        _number: function () {
            this.model.set({ "First": Math.floor(Math.random() * 10) + 1 });
            this.model.set({ "Second": Math.floor(Math.random() * 10) + 1 });
            this.model.set({ "Total": this.model.get("First") + this.model.get("Second") });
        },

        render: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            return this;
        },

        submit: function (e) {
            e.preventDefault();
            var data = this.$("#contactUs form").serializeObject();
            this.model.save(data, { validate: true, success: _.bind(this.success, this), error: _.bind(this.error, this) });
        },

        success: function (model, response, options) {

        },

        error: function (model, response, options) {

        }
    });
    return ContactUsView;
});
