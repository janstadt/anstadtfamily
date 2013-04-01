define([
    "./carouselModel",
    "text!./carouselTemplate.html",
    "i18n!./nls/carousel"
], function (
    carouselModel,
    template,
    i18n
    ) {
    var CarouselView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        _visible: true,
        initialize: function (defaults) {
            this.listenTo(this.model, "change", this.render);
            this.model.fetch();
        },

        visible: function () {
            return this._visible;
        },

        hide: function () {
            if (!this._visible) {
                return;
            }
            $("#" + this.model.get("Id")).slideUp();
            this._visible = false;
        },

        show: function () {
            if (this._visible) {
                return;
            }
            $("#" + this.model.get("Id")).slideDown();
            this._visible = true;
        },

        render: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            $("#" + this.model.get("Id")).carousel();
            this._finished();
            return this;
        },

        _finished: function () {
            if (this._visible) {
                $("#" + this.model.get("Id")).show();
            } else {
                $("#" + this.model.get("Id")).hide();
            }
            this.trigger("finished");
        }
    });
    return CarouselView;
});
