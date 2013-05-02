define([
    "./albumModel",
    "text!./albumTemplate.html",
    "text!./photoTemplate.html",
    "i18n!./nls/album"
], function (
    albumModel,
    template,
    photoTemplate,
    i18n) {
    var AlbumView = Backbone.View.extend({
        template: _.template(template),
        photoTemplate: _.template(photoTemplate),
        i18n: i18n,
        model: null,
        content: null,
        initialize: function (defaults) {
            this.model = new albumModel({ "Title": defaults.SubId, "Type": defaults.PageId });
            this.model.fetch({ success: _.bind(this.success, this), error: _.bind(this.error, this) });
            this.bootstrap();
        },
        bootstrap: function () {
            window.application.showLoading(this.$el);
            return this;
        },
        success: function (model, response) {
            window.application.hideLoading(this.$el, _.bind(this.addPhotos, this));
            return this;
        },

        error: function (model, response) {
            var asdf = "asdf";
        },
        addPhotos: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            this.masonryContainer = this.$(".photos");
            var photos = this.model.get("Photos");
            _.each(photos, _.bind(this.addPhoto, this));
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

        addPhoto: function (photo) {
            var item = $(this.photoTemplate({ "model": photo, "i18n": this.i18n }));
            this.setupMasonry();
            this.masonryContainer.append(item).masonry("appended", item);
        }
    });
    return AlbumView;
});
