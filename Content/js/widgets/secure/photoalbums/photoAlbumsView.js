define([
    "./photoAlbumsCollection",
    "text!./photoAlbumsTemplate.html",
    "i18n!./nls/photoAlbums",
    "../photoAlbum/photoAlbumModel",
    "./photoAlbumItemView",
    "../../tag/tagView"
], function (
    PhotoAlbumsCollection,
    template,
    i18n,
    PhotoAlbumModel,
    PhotoAlbumView,
    TagView) {
    var PhotoAblumsView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        collection: null,
        modal: null,
        modalModel: null,
        albumModel: null,
        _userId: null,
        masonryContainer: null,
        tagView: null,
        initialize: function (options) {
            this.render();
            this.addTags();
            this.listenTo(this.collection, "reset", this.addAll);
            this.listenTo(this.collection, "add", this.addOne);
            this.masonryContainer = this.$("#user-photoalbums");
            this.collection.fetch();
        },

        events: {
            "click a.addalbum": "addAlbumClick",
            "click a.cancel": "cancelAlbumClick",
            "click button#saveAlbum": "saveAlbumClick"
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            this.$("#albumDatePicker").datetimepicker({
                "pickTime": false
            });

            return this;
        },

        addTags: function () {
            this.tagView = new TagView(null, { "Type": "Albums" });
            this.$(".floatLeft").append(this.tagView.el);
        },

        setupMasonry: function () {
            this.masonryContainer.imagesLoaded(_.bind(function () {
                this.masonryContainer.masonry({
                    itemSelector: ".item",
                    columWidth: 194,
                    isAnimated: true
                });
                $(window).resize();
            }, this));
        },

        showControls: function () {
            this.$(".action-buttons").removeClass("hide");
        },

        addAll: function () {
            this.collection.each(this.initAddOne, this);
            this.setupMasonry();
        },

        initAddOne: function (album) {
            this.$("#no-albums").hide();
            var albumView = new PhotoAlbumView({ "model": album });
            var item = $(albumView.el);
            this.masonryContainer.append(item);
        },

        addOne: function (album) {
            this.$("#no-albums").hide();
            this.listenTo(album, "masonRemove", this.removeOne);
            var albumView = new PhotoAlbumView({ "model": album });
            var item = $(albumView.el);
            this.masonryContainer.append(item).masonry("appended", item);
            this.setupMasonry();
        },

        removeOne: function (album) {
            if (this.collection.length === 0) {
                this.$("#no-albums").show();
            }
            album.$el.fadeOut().removeClass("item masonry-brick");
            this.masonryContainer.masonry("remove", album.$el);
            this.setupMasonry();
        },

        addAlbumClick: function () {
            this.$(".user-albums").addClass("editing");
            this.model = new PhotoAlbumModel({ "Owner": this.options.userId });
            Backbone.Validation.bind(this);
            this.$("#create-album").slideDown();
        },

        cancelAlbumClick: function () {
            this.$(".user-albums").removeClass("editing");
            this.$("#create-album").slideUp();
            this.$("#new-album").find("input[type=text], textarea").val("");
            this.tagView.clear();
        },

        saveAlbumClick: function () {
            var data = this.$("#new-album").serializeObject();
            data.AddedTags = data.AddedTags.split(",");
            this.model.save(data, { validate: true, success: _.bind(this.saveSuccess, this), error: _.bind(this.saveError, this) });
        },

        saveSuccess: function (model, response, options) {
            this.cancelAlbumClick();
            this.collection.add(model);
            this.tagView.clear();
        },

        saveError: function (model, response, options) {
            this.$(".user-albums").addClass("editing");
            this.$("#saveError").removeClass("hide");
            this.tagView.clear();
        }
    });
    return PhotoAblumsView;
});
