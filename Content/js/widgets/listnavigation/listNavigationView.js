define([
    "text!./listNavigationTemplate.html",
    "i18n!./nls/listNavigation",
    "./listNavigationCollection",
    "./listItemView",
    "./listNavigationModel"
], function (
    template,
    i18n,
    ListNavigationCollection,
    ListItemView,
    ListNavigationModel) {
    var listNavigationView = Backbone.View.extend({
        template: _.template(template),
        i18n: i18n,
        collection: null,
        _items: null,
        justification: null,
        initialize: function (defaults) {
            this.collection = new ListNavigationCollection();
            if (defaults) {
                this.defaults = defaults;
                this.justification = defaults.justification;
                this._items = defaults.Items;
            }
            this._overrideSelected();
            this.render();
            this.listenTo(this.collection, "change", this.render);
        },

        //A little routing crap in the view. Hate it but oh well.
        _overrideSelected: function () {
            if (this.defaults.PageId) {
                var initialList = this._items;
                var override = _.findWhere(this._items, { "Selected": true, "Id": this.defaults.PageId });
                if (!override) {
                    var pageId = this.defaults.PageId;
                    this.items = _.map(this._items, function (item) { item.Selected = item.Id === pageId; return item; });
                    //If none are selected, select default.
                    if (!_.findWhere(this._items, { "Selected": true })) {
                        this._items = initialList;
                    }
                }
            }
            this.collection.add(this._items);
        },

        _addListItems: function () {
            var that = this,
                listItem,
                model;
            this.collection.forEach(function (item) {
                listItem = new ListItemView({ "model": item });
                that._renderListItem(listItem);
                that._renderWidget(item);
            });
        },

        _renderListItem: function (item) {
            this.$(".navigationList").append(item.el);
        },

        _renderWidget: function (item) {
            var self = this,
                widget = item.get("Widget"),
                selected = item.get("Selected");
            if (widget && selected) {
                require([widget.Path], function (Widget) {
                    $(self.el).find(".list-widget-placeholder").hide().html(new Widget(widget.Model).el).fadeIn();
                });
            }
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n, "model": this.defaults }));
            this._addListItems();
            return this;
        }

    });
    return listNavigationView;
});
