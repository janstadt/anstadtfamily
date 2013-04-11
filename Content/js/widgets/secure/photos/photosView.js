define([
    "text!./photosTemplate.html",
    "i18n!./nls/photos",
    "../../photo/photoModel",
    "./photoItemView",
    "text!./photoItemTemplate.html",
     "../../modal/modalView",
    "../../modal/modalModel",
    "../../upload/uploadView",
    "../../upload/uploadModel"
], function (
    template,
    i18n,
    PhotoModel,
    PhotoItemView,
    PhotoItemTemplate,
    ModalView,
    ModalModel,
    UploadView,
    UploadModel) {
    var PhotoPhotosView = Backbone.View.extend({
        template: _.template(template),
        photoTemplate: _.template(PhotoItemTemplate),
        i18n: i18n,
        collection: null,
        modal: null,
        modalModel: null,
        albumModel: null,
        _albmunId: null,
        _ownerId: null,
        masonryContainer: null,
        initialize: function (options) {
            this.options = options;
            this.render();
             if(this.options) {
                this._albumId = this.options.albumId;
                this._ownerId = this.options.userId;
            }
            this.listenTo(this.collection, "reset", this.addAll);
            this.listenTo(this.collection, "add", this.addOne);
            this.masonryContainer = this.$("#user-photos");
            this.collection.fetch();
        },
        
        events: {
            "click a.addphotos": "addPhotoClick",
            "click a.cancel": "cancelPhotoClick",
            "click button#savePhoto": "savePhotoClick",
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n, "accessor" : this.collection.GetAccessor() }));
            this.$("#albumDatePicker").datetimepicker({
                "pickTime": false
            });
            return this;
        },

        setupMasonry: function () {
            this.masonryContainer = this.$("#user-photos");
            this.masonryContainer.imagesLoaded( _.bind(function () {
                this.masonryContainer.masonry({
                    itemSelector: ".item",
                    columWidth: 194,
                    isAnimated: true
                });
            }, this));
        },

        addAll: function () {
            this.collection.each(this.addOne, this);
        },

        addOne: function (photo) {
            this.$("#no-photos").hide();
            this.setupMasonry();
            photo.SetAccessor(this.collection.GetAccessor());
            this.listenTo(photo, "masonRemove", this.removeOne);
            var photoView = new PhotoItemView({"model": photo});
            var item = $(photoView.el);
            this.masonryContainer.append(item).masonry("appended", item);
        },

        removeOne: function (photo) {
             if (this.collection.length === 0) {
                this.$("#no-albums").show();
            }
            photo.$el.fadeOut().removeClass("item masonry-brick");
            this.masonryContainer.masonry("remove", photo.$el);
            this.setupMasonry();
        },

        uploaded: function (photo) {
            this.collection.add(photo.result.files[0]);
        },

        addPhotoClick: function () {
            this.$(".user-photos").addClass("editing");
            var mModel = new ModalModel({ "Id": "photosModal", "Content": "", "Title": this.i18n.AddPhotos });
            this.modal = new ModalView({ "model": mModel });
            $("#photos-modal").html(this.modal.el);

            var data = [
                {
                    "name": "AlbumId",
                    "value": this._albumId
                },
                {
                    "name": "Owner",
                    "value" : this._ownerId
                }
            ];

            var uploadModel = new UploadModel({"Url" : "/api/photos", "Data" : data});
            var uploadView = new UploadView({"model": uploadModel});
            
            //hide modal when finished
            this.listenTo(uploadView, "finished", _.bind(this.hideModal, this)); 

            this.listenTo(uploadView, "add", _.bind(this.uploaded, this));
                        
            this.modal.setContent(uploadView.el);
            this.modal.setCallbacks({
                callbacks : {
                    "hidden": this.cancelPhotoClick
                },
                scope: this
            });
            uploadView.initFileUpload();
        },

        hideModal: function () {
            this.modal.hide();
            this.cancelPhotoClick();
        },

        cancelPhotoClick: function () {
            this.$(".user-photos").removeClass("editing");
        },

        savePhotoClick: function () {
            var data = this.$("#new-photo").serializeObject();
            this.model.save(data, {validate: true, success: _.bind(this.saveSuccess, this), error: _.bind(this.saveError, this) });
        },

        saveSuccess: function (model, response, options) {
            this.cancelPhotoClick();
            this.collection.add(model);
        },

        saveError: function (model, response, options) {
            this.$(".user-photos").addClass("editing");
            this.$("#saveError").removeClass("hide");
        }
    });
    return PhotoPhotosView;
});
