define([
    "./slideshowCollection",
    "text!./slideshowTemplate.html",
    "i18n!./nls/slideshow",
    "../../modal/modalView",
    "../../modal/modalModel",
    "../../upload/uploadView",
    "../../upload/uploadModel",
    "./slideshowItemView"
], function (
    SlideshowCollection,
    template,
    i18n,
    ModalView,
    ModalModel,
    UploadView,
    UploadModel,
    SlideshowItemView) {
    var SlideshowView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        collection: null,
        modal: null,
        modalModel: null,
        className: "slideshow-collection",
        initialize: function (options) {
            this.render();
            this.collection = new SlideshowCollection();
            this.listenTo(this.collection, "reset", this.addAll);
            this.listenTo(this.collection, "add", this.addOne);
            this.collection.fetch();
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            return this;
        },

        events: {
            "click a.create": "addClick"
        },

        addAll: function () {
            this.collection.each(this.addOne, this);
        },

        addOne: function (photo) {
            var item = new SlideshowItemView({ "model": photo });
            this.$("#slidewhos-list-body").append(item.el);
        },

        addClick: function () {
            var mModel = new ModalModel({ "Id": "slideshowModal", "Content": "", "Title": this.i18n.AddPhotos });
            this.modal = new ModalView({ "model": mModel });
            $("#slideshowModalContainer").html(this.modal.el);

            var uploadModel = new UploadModel({ "Url": "/api/photos/slideshow", "HideWatermark": true });
            var uploadView = new UploadView({ "model": uploadModel });

            //hide modal when finished
            this.listenTo(uploadView, "finished", _.bind(this.hideModal, this));
            this.listenTo(uploadView, "add", _.bind(this.uploaded, this));

            this.modal.setContent(uploadView.el);
            uploadView.initFileUpload();
            this.modal.show();
        },

        uploaded: function (photo) {
            this.collection.add(photo.result.files[0]);
        },

        hideModal: function () {
            this.modal.hide();
        }
    });
    return SlideshowView;
});
