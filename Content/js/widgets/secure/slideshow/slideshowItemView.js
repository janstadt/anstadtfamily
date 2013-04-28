define([
    "./slideshowModel",
    "text!./slideshowTemplate.html",
    "i18n!./nls/slideshow"
], function (
    SlideshowModel,
    template,
    i18n) {
    var SlideshowItemView = Backbone.View.extend({
        model: null,
        i18n: i18n,
        template: _.template(template),
        initialize: function (model, options) {
            this.model = model;
            this.render();
        },

        events: {
            "click a.delete": "deleteClick"
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            return this;
        },
        
        deleteClick: function (evt) {
            bootbox.confirm(_.template(this.i18n.ConfirmDelete, this.model.toJSON()), _.bind(function (confirm) {
                if (confirm) {
                    this.model.destroy({ "success": _.bind(this.deleteSuccess, this), "error": _.bind(this.deleteError, this) });
                }
            }, this));
        },
        
        deleteSuccess: function () {
            this.$el.fadeOut(500, function () {
                $(this).remove();
            });
        },

        deleteError: function () {
            bootbox.alert(this.i18n.Error);
        }
    });
    return SlideshowItemView;
});
