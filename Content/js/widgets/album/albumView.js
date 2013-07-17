define([
    "./albumModel",
    "text!./albumTemplate.html",
    "text!./photoTemplate.html",
    "i18n!./nls/album",
    "../photoSlideshow/photosCollection",
    "../photoSlideshow/photoSlideshowView",
    "../modal/modalView",
    "../modal/modalModel"
], function (
    albumModel,
    template,
    photoTemplate,
    i18n,
    slideshowCollection,
    slideshowView,
    modalView,
    modalModel) {
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

        events: {
            "click a.item.masonry-brick": "photoClick"
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
            this.initSlideshow();
        },

        initSlideshow: function () {
            var id = this.model.get("Id"),
            mModel = new modalModel({ "Id": "photosModal-" + id, "Title": this.model.get("Title") });
            this.modal = new modalView({ "model": mModel });
            this.modal.overrideSize();
            // TODO: figure out how to make the transition a bit more clean.
            //            this.modal.setCallbacks({
            //                callbacks: {
            //                    "slide": this._slide
            //                },
            //                scope: this
            //            });
            $("#photos-modal").html(this.modal.el);
            this.photoCollection = new slideshowCollection(this.model.get("Photos"), { "Id": id });

            //this.modal.setContent(uploadView.el);
        },

        //        _slide: function () {
        //            var nextImage = this.modal.$el.find(".item.active").next().find("img"),
        //            image = new Image(),
        //            modal = this.modal;
        //            image.onload = function () {
        //                modal.setBodySize(this.width, this.height);
        //            }
        //            image.src = nextImage[0].src;
        //        },

        photoClick: function (evt) {
            evt.preventDefault();
            var id = evt.currentTarget.id;
            this.photoCollection.setFirst(id);
            var slideshow = new slideshowView({ "model": this.model, "collection": this.photoCollection });
            slideshow.overrideClass("albumSlideshow");
            this.modal.setContent(slideshow.el);
            this.modal.show();
            //this.photoCollection.find({"Id: evt.id}).set({ "First": true }, { "silent": true });
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
