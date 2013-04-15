define([
    "../photoAlbum/photoAlbumModel",
    "text!./photoItemTemplate.html",
    "i18n!./nls/photos",
    "../../../shared/models/favoritesModel"
], function (
    PhotoAlbumModel,
    template,
    i18n,
    FavoritesModel) {
    var PhotoAblumView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        className: "item", //for masonry.js
        initialize: function (options) {
            this.render();
            this.favoritesModel = new FavoritesModel({ "id": this.model.toJSON().Id, "type": "photos" });
            this.listenTo(this.model, "change:MainImage", this.toggleMain);
        },

        events: {
            "click a#unfavoriteLink": "unFavoriteClick",
            "click a#favoriteLink": "favoriteClick",
            "click a#deleteLink": "deleteClick",
            "click span.comments": "commentClick",
            "click a#deselectMainImage": "deselectMainClick",
            "click a#selectMainImage": "selectMainClick"
        },

        render: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n, "accessor": this.model.GetAccessor() }));
            return this;
        },

        selectMainClick: function (evt) {
            evt.preventDefault();
            this.model.save({ "MainImage": true }, { "success": _.bind(this.mainSuccess, this), "error": _.bind(this.mainError, this), "silent": true });
        },

        deselectMainClick: function (evt) {
            evt.preventDefault();
            this.model.save({ "MainImage": false }, { "success": _.bind(this.mainSuccess, this), "error": _.bind(this.mainError, this), "silent": true });
        },

        toggleMain: function () {
            //            if (this.model.get("MainImage")) {
            //                this.$("#deselectMainImage").removeClass("hide");
            //                this.$("#selectMainImage").addClass("hide");
            //            } else {
            //                this.$("#deselectMainImage").addClass("hide");
            //                this.$("#selectMainImage").removeClass("hide");
            //            }
            this.$el.html("");
            this.render();
        },

        mainSuccess: function (model) {
            this.toggleMain();
            this.model.trigger("mainUpdate", model);
        },

        mainError: function (model) {
        },

        commentClick: function (evt) {
            evt.preventDefault();
            alert('add comment.');
        },

        unFavoriteClick: function (evt) {
            evt.preventDefault();
            this.favoritesModel.destroy({ "success": _.bind(this.favoritesSuccess, this) });
        },

        favoriteClick: function (evt) {
            evt.preventDefault();
            this.favoritesModel.save(null, { "success": _.bind(this.favoritesSuccess, this) });
        },

        deleteClick: function (evt) {
            bootbox.confirm(_.template(this.i18n.ConfirmDelete, this.model.toJSON()), _.bind(function (confirm) {
                if (confirm) {
                    this.model.destroy({ "success": _.bind(this.deleteSuccess, this), "error": _.bind(this.deleteError, this) });
                }
            }, this));
        },

        deleteSuccess: function () {
            this.model.trigger("masonRemove", this);
        },

        deleteError: function () {
            bootbox.alert(this.i18n.Error);
        },

        favoritesSuccess: function () {
            var favorite = this.model.get("Favorite");
            this.model.set({ "Favorite": !favorite });
            this.toggleMain();
        }
    });
    return PhotoAblumView;
});
