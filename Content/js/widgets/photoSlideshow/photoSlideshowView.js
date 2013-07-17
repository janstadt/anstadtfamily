define([
    "text!./photoSlideshowTemplate.html",
    "i18n!./nls/photoSlideshow",
    "./slideshowItemView",
    "text!./photoSlideshowIndicatorsTemplate.html",
], function (
    template,
    i18n,
    slideshowItemView,
    indicatorsTemplate) {
    var PhotoSlideshowView = Backbone.View.extend({
        template: _.template(template),
        indicatorsTemplate: _.template(indicatorsTemplate),
        i18n: i18n,
        content: null,
        initialize: function (model, defaults) {
            this.render();
            this.carouselContainer = this.$(".carousel-inner");
            this.addAll();
        },
        render: function () {
            var model = this.model.toJSON();
            $(this.el).html(this.template({ "model": model, "i18n": this.i18n }));
            this.$("#" + model.Id).carousel();
            return this;
        },
        overrideClass: function (className) {
            this.$("#" + this.model.get("Id")).addClass(className);
        },
        addAll: function () {
            this.setupIndicators();
            this.collection.each(this.addOne, this);
        },
        setupIndicators: function () {
            this.$("#carouselIndicators").append(this.indicatorsTemplate({ "length": this.collection.toJSON().length, "model": this.model.toJSON() }));
        },
        addOne: function (item) {
            var itemView = new slideshowItemView({ "model": item });
            this.carouselContainer.append(itemView.el);
        }
    });
    return PhotoSlideshowView;
});
