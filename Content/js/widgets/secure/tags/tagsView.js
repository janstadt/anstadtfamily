define([
    "text!./tagsTemplate.html",
    "text!./tagTemplate.html",
    "i18n!./nls/tags",
    "./tagCollection",
    "./tagItemView"
], function (
    template,
    tagTemplate,
    i18n,
    TagCollection,
    TagItemView) {
    var TagsView = Backbone.View.extend({
        template: _.template(template),
        tagTemplate: _.template(tagTemplate),
        i18n: i18n,
        collection: null,
        className: "tag-collection",
        initialize: function (model, defaults) {
            this.collection = new TagCollection();
            this.listenTo(this.collection, "reset", _.bind(this._addTags, this));
            this.collection.fetch();
            this.render();
            this.createTr = this.$(".create-tr");
        },

        events: {
            "click a.create": "showCreate",
            "click a.cancel": "hideCreate"
        },

        showCreate: function () {
            this.createTr.slideDown();
        },

        hideCreate: function () {
            this.createTr.slideUp();
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            return this;
        },

        _addTags: function () {
            var tbody = this.$("#tag-list-body");
            this.collection.forEach(_.bind(function (i) {
                var item = new TagItemView({ "model": i });
                tbody.append(item.el);
            }, this));
            //this.collection.forEach(fuction (item) {
            //tbody.append(temp({"model": item}));
            //});
        }
    });
    return TagsView;
});
