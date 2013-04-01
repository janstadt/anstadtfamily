define([
    "../photoAlbum/photoAlbumModel",
    "text!./photoAlbumItemTemplate.html",
    "i18n!./nls/photoAlbums",
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
        favoritesModel: null,
        attributes: function () {
            return {
                class: "item",
                id: this.model.get("Id")
            };
        },
        //className: "item", //for masonry.js
        initialize: function (options) {
            this.render();
            this.favoritesModel = new FavoritesModel({ "id": this.model.toJSON().Id, "type": "albums" });
        },

        events: {
            "click a#unfavoriteLink": "unFavoriteClick",
            "click a#favoriteLink": "favoriteClick",
            "click a#deleteLink": "deleteClick"
        },

        render: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            //this.el.id = this.model.toJSON().Id;
            return this;
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
            this.$("#favorite").toggleClass("hide");
        }
    });
    return PhotoAblumView;
});
