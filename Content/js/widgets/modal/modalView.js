define([
    "./modalModel",
    "text!./modalTemplate.html",
    "i18n!./nls/modal"
], function (
    modalModel,
    template,
    i18n) {
    var ModalView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        content: null,
        initialize: function (defaults) {
            this.render();
            this.setContent(this.model.get("Content"));
        },

        setCallbacks: function (options) {
            if (options && options.callbacks) {
                var modalContainer = this.$("#" + this.model.get("Id"));
                for (var callback in options.callbacks) {
                    modalContainer.on(callback, function () {
                        options.callbacks[callback].apply(options.scope);
                    });
                }
            }
        },

        overrideSize: function () {
            this.$el.addClass("modalOverride");
        },

        setBodySize: function (width, height) {
            this.$("#modalBody").animate({ "width": width, "height": height }, 200);
        },

        setContent: function (content) {
            this.$("#modalBody").html(content);
        },

        hide: function () {
            this.$("#" + this.model.get("Id")).modal("hide");
        },

        show: function () {
            this.$("#" + this.model.get("Id")).modal("show");
        },

        render: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            return this;
        }
    });
    return ModalView;
});
