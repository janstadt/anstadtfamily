define([
    "./carouselCollection",
    "./carouselItemView",
    "text!./carouselTemplate.html",
    "text!./carouselIndicatorsTemplate.html",
    "i18n!./nls/carousel"
], function (
    carouselCollection,
    carouselItemView,
    template,
    indicatorsTemplate,
    i18n
    ) {
    var CarouselView = Backbone.View.extend({
        template: _.template(template),
        indicatorsTemplate: _.template(indicatorsTemplate),
        i18n: i18n,
        collection: null,
        carouselId: null,
        carouselContainer: null,
        _visible: true,
        initialize: function (defaults) {
            this.defaults = defaults;
            this.collection = new carouselCollection([], this.defaults);
            this.render();
            this.carouselContainer = this.$(".carousel-inner");
            this.listenTo(this.collection, "reset", this.addAll);
            this.collection.fetch();
        },

        visible: function () {
            return this._visible;
        },

        hide: function () {
            if (!this._visible) {
                return;
            }
            this.$("#" + this.defaults.Id).slideUp();
            this._visible = false;
        },

        show: function () {
            if (this._visible) {
                return;
            }
            this.$("#" + this.defaults.Id).slideDown();
            this._visible = true;
        },

        addOne: function (item) {
            var itemView = new carouselItemView({ "model": item });
            this.carouselContainer.append(itemView.el);
        },

        addAll: function () {
            this.setupIndicators();
            this.collection.at(0).set({ "First": true }, { "silent": true });
            this.collection.each(this.addOne, this);
        },

        setupIndicators: function () {
            if (this.defaults.ShowIndicator) {
                this.$("#carouselIndicators").append(this.indicatorsTemplate({ "length": this.collection.toJSON().length, "model": this.defaults }));
            }
        },

        render: function () {
            $(this.el).html(this.template({ "model": this.defaults, "i18n": this.i18n }));
            this.$("#" + this.defaults.Id).carousel();
            this._finished();
            return this;
        },

        _finished: function () {
            if (this._visible) {
                $("#" + this.defaults.Id).show();
            } else {
                $("#" + this.defaults.Id).hide();
            }
            this.trigger("finished");
        }
    });
    return CarouselView;
});
