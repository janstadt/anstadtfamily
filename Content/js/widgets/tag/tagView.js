define([
    "text!./tagTemplate.html",
    "i18n!./nls/tag",
    "./tagModel",
    "tagit"
], function (
    template,
    i18n,
    TagModel) {
    var TagView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        model: null,
        className: "control-group",
        initialize: function (model, defaults) {
            if (defaults) {
                this.tagName = defaults.TagName || "div";
            }
            this.model = new TagModel(defaults);
            this.model.setTypeUrl();
            this.listenTo(this.model, "change", _.bind(this.insertTags, this));
            this.render();
            this.addExistingTags(defaults.AddedTags);
            this.model.fetch();
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n }));
            return this;
        },

        addExistingTags: function (tags) {
            if (tags) {
                var tagNames = _.map(tags, function (tag) { return tag.Name; });
                var list = this.$("#tagInput");
                list.val(tagNames.join(","));
            }
        },

        insertTags: function () {
            var list = this.$(".tagList");
            var tags = this.model.get("Available");
            tags = _.map(tags, function (item) { return item.Name; });
            list.tagit({
                availableTags: tags,
                singleField: true,
                singleFieldNode: this.$("#tagInput"),
                beforeTagAdded: _.bind(this.addTag, this),
                beforeTagRemoved: _.bind(this.removeTag, this),
                allowSpaces: true
            });
        },

        clear: function () {
            this.$(".tagList").tagit("removeAll");
        },

        _addTagId: function (item) {
            var tags = this.model.get("AddedTags");
            var tag = _.find(tags, function (i) {
                return i.Name == item.tagLabel;
            });
            $(item.tag).attr("id", tag.Id);
        },

        addTag: function (event, ui) {
            if (ui.duringInitialization) {
                this._addTagId(ui);
            }
            else {
                if (!this.model.isNew() && !this.model.exists(ui.tagLabel)) {
                    //call save on the model for this item. 
                    var tag = new TagModel();
                    var data = { "ParentId": this.model.get("Id"), "Owner": this.model.get("Owner"), "Name": ui.tagLabel, "Type": this.model.get("Type") };
                    tag.save(data, { success: _.bind(this.success, this, ui), error: _.bind(this.error, this) });

                }
            }
        },
        removeTag: function (event, ui) {
            if (!this.model.isNew()) {
                if (this.model.exists(ui.tagLabel)) {
                    //call save on the model for this item.
                    var id = $(ui.tag).attr("id");
                    var tag = new TagModel({ "ParentId": this.model.get("Id"), "Id": id, "Owner": this.model.get("Owner"), "Name": ui.tagLabel, "Type": this.model.get("Type") });
                    tag.destroy({ success: _.bind(this.success, this, ui), error: _.bind(this.error, this) });
                }
                //call destroy on the model for this item.
            }
        },
        success: function (args, model, response) {
            var item = model.toJSON();
            if (this.model.exists(item.Name)) {
                this.model.remove(item);
            }
            else {
                this.model.add(item);
                this._addTagId(args);
            }
            this.trigger("TagsUpdated", this.model.get("AddedTags"));
        },

        error: function (model, response) {
            this.model.remove(model.toJSON());
        }
    });
    return TagView;
});
