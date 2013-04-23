define([
    "./portfolioModel",
    "text!./portfolioItemTemplate.html",
    "text!./albumTemplate.html",
    "i18n!./nls/portfolio"
], function (
    portfolioModel,
    template,
    albumTemplate,
    i18n) {
    var PortfolioItemView = Backbone.View.extend({
        template: _.template(template),
        albumTemplate: _.template(albumTemplate),
        i18n: i18n,
        model: null,
        content: null,
        masonryContainer: null,
        initialize: function (defaults) {
            this.render();
            this.masonryContainer = this.$(".photo-albums." + this.model.get("Id"));
            this.addItems();
        },
        render: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            return this;
        },
        addItems: function () {
            var albums = this.model.get("Albums");
            _.each(albums, _.bind(this.addItem, this));
        },

        setupMasonry: function () {
            this.masonryContainer.imagesLoaded(_.bind(function () {
                this.masonryContainer.masonry({
                    itemSelector: ".item",
                    columWidth: 270,
                    isAnimated: true
                });
                $(window).resize();
            }, this));
        },

        addItem: function (album) {
            var item = $(this.albumTemplate({ "model": album, "albumId": this.model.get("Id"), "i18n": this.i18n }));
            this.setupMasonry();
            this.masonryContainer.append(item).masonry("appended", item);
        }
    });
    return PortfolioItemView;
});
