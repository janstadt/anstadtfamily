define([
    "text!./tagsTemplate.html",
    "text!./tagTemplate.html",
    "i18n!./nls/tags",
    "./tagCollection",
    "./tagItemView",
    "./tagModel"
], function (
    template,
    tagTemplate,
    i18n,
    TagCollection,
    TagItemView,
    TagModel) {
    var TagsView = Backbone.View.extend({
        template: _.template(template),
        tagTemplate: _.template(tagTemplate),
        i18n: i18n,
        collection: null,
        className: "tag-collection",
        model: null,
        initialize: function (model, defaults) {
            this.collection = new TagCollection();
            this.listenTo(this.collection, "reset", _.bind(this._addTags, this));
            this.collection.fetch();
            this.render();
            this.createTr = this.$(".create-tr");
            this.model = new TagModel();
            Backbone.Validation.bind(this);
        },

        events: {
            "click a.create": "showCreate",
            "click a.cancel": "hideCreate",
            "click a.save": "saveTag"
        },

        showCreate: function () {
            this.createTr.fadeIn();
        },

        hideCreate: function () {
            this.createTr.fadeOut();
        },

        saveTag: function () {
            //validate.
            var tagName = this.$("input[name='Name']").val().trim(),
                tagType = this.$("select[name='Type']").val(),
                isCategory = this.$("input[name='Create-Category']").is(":checked"),
                data = { "Name": tagName, "Type": tagType, "IsCategory": isCategory };
            var exists = this.collection.exists(tagName, tagType);
            if (exists) {
                data.Unique = false;
            } else {
                data.Unique = true;
            }
            this.model = new TagModel(data);
            this.model.save(data, { validate: true, success: _.bind(this.saveSuccess, this), error: _.bind(this.saveError, this) });
        },

        saveSuccess: function () {
            this.createTr.find("input").val("");
            this.hideCreate();
            this._addTag(this.model);
        },

        saveError: function () {
            bootbox.alert(this.i18n.SaveError);
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            return this;
        },

        _addTag: function (model) {
            var tbody = this.$("#tag-list-body");
            var item = new TagItemView({ "model": model });
            tbody.append(item.el);
        },

        _addTags: function () {

            this.collection.forEach(_.bind(function (i) {
                this._addTag(i);
            }, this));
        }
    });
    return TagsView;
});
