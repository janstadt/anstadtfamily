define([
    "text!./photoAlbumTemplate.html",
    "i18n!./nls/photoAlbum",
    "../../tag/tagView"
], function (
    template,
    i18n,
    TagView) {
    var PhotoAblumInfoView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        moment: moment(),
        initialize: function (options) {
            Backbone.Validation.bind(this);
            this.listenTo(this.model, "change", this.render);
            this.model.fetch();
        },

        events: {
            "click a.edit": "edit",
            "click a.cancel": "cancel",
            "click a.save": "save"
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n, "model": this.model.toJSON(), "accessor": this.model.GetAccessor() }));
            this.addTags();
            return this;
        },

        addTags: function () {
            var tags = this.model.get("Tags");
            var tagView = new TagView(null, { "Type": "Albums", "Id": this.model.get("Id"), "AddedTags": tags, "Owner": this.model.get("Owner") });
            this.listenTo(tagView, "TagsUpdated", _.bind(this.updateTags, this));
            this.$("#tags").append(tagView.el);
        },

        updateTags: function (tags) {
            this.model.set({ "Tags": tags }, { silent: true });
        },

        edit: function () {
            this.$(".error").removeClass("error");
            this.$(".photoalbum-info").addClass("editing");
        },

        cancel: function () {
            this.render();
        },

        save: function () {
            this.$("#saveError").addClass("hide");
            this.$(".error").removeClass("error");
            var data = this.$(".photoalbum-info").serializeObject();
            data.Favorite = data.Favorite === "on" ? true : false;
            this.model.save(data, { validate: true, success: _.bind(this.saveSuccess, this), error: _.bind(this.saveError, this) });
        },

        saveSuccess: function (model, response, options) {
            this.$(".photoalbum-info").removeClass("editing");
            this.trigger("finished");
        },

        saveError: function (model, response, options) {
            this.$(".photoalbum-info").addClass("editing");
            this.$("#saveError").removeClass("hide");
        }
    });
    return PhotoAblumInfoView;
});
