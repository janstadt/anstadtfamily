define([
    "text!./tagTemplate.html",
    "i18n!./nls/tags",
    "./tagCollection"
], function (
    template,
    i18n,
    TagCollection) {
    var TagItemView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        tagName: "tr",
        className: "tag-item",
        collection: null,
        initialize: function () {
            this.render();
        },

        events: {
            "dblclick div": "edit",
            "click a.delete": "deleteItem",
            "click a.save": "saveItem",
            "click a.cancel": "cancel"
        },

        render: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON(), "i18n": this.i18n }));
            return this;
        },

        deleteItem: function () {
            bootbox.confirm(this.i18n.ConfirmDelete, _.bind(function (confirm) {
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
            bootbox.alert(this.i18n.DeleteError);
        },

        saveSuccess: function () {
            this.render();
            this.cancel();
        },

        saveError: function () {
            bootbox.alert(this.i18n.SaveError);
        },

        saveItem: function () {
            var name = this.$(".tagInput").val(),
                type = this.$("select").val(),
                category = this.$(".tagCategory").is(":checked");
            this.model.save({ "Name": name, "Type": type, "IsCategory": category }, { "success": _.bind(this.saveSuccess, this), "error": _.bind(this.saveError, this) });
        },

        cancel: function () {
            $(".tag-item").removeClass("editing");
        },

        edit: function () {
            this.cancel();
            this.$el.addClass("editing");
        }
    });
    return TagItemView;
});
