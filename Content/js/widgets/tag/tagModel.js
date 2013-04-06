define([], function () {
    var TagModel = Backbone.Model.extend({
        urlRoot: "api/tags/tag/",
        setTypeUrl: function () {
            this.url = "api/" + this.get("Type") + "/tags";
        },

        idAttribute: "Id",
        exists: function (item) {
            var items = this.get("AddedTags");
            return _.find(items, function (i) {
                return i.Name === item;
            });
        },
        add: function (item) {
            //            var items = this.get("Available");
            //            items.push(item);
            //            this.set("Available", items);
            var addedTags = this.get("AddedTags");
            addedTags.push(item);
            this.set("AddedTags", addedTags);
            this.trigger("change");
        },
        remove: function (item) {
            //var items = this.get("Available");
            var addedItems = this.get("AddedTags");
            //this.set("Available", _.without(items, item));
            var add = _.find(addedItems, function (i) {
                return i.Name = item.Name;
            });
            this.set("AddedTags", _.without(addedItems, add));
        }
    });
    return TagModel;
});
