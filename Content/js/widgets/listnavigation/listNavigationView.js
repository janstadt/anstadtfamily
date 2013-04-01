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
        className: "row",
        justification: null,
        initialize: function (defaults) {
            this.collection = new ListNavigationCollection();
            if (defaults) {
                this.justification = defaults.justification;
                this._items = defaults.Items;
                this.collection.add(this._items);
            }
            this.render();
            this.listenTo(this.collection, "change", this.render);
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
            this.$(".list-sidenav").append(item.el);
        },

        _renderWidget: function (item) {
            var self = this,
                widget = item.get("Widget"),
                selected = item.get("Selected");
            if (widget && selected) {
                require([widget.Path], function (Widget) {
                    $(self.el).find("#widget-placeholder").hide().html(new Widget(widget.Model).el).fadeIn();
                });
            }
        },

        render: function () {
            $(this.el).html(this.template({ "i18n": this.i18n, "model": { "justification": this.justification} }));
            this._addListItems();
            return this;
        }

    });
    return listNavigationView;
});
