define([
    "text!./listItemTemplate.html"
], function (
    template
) {
    var listItemView = Backbone.View.extend({
        tagName: "li",
        className: "control-group",
        events: {
            "click .navigationList li a": "select"
        },
        template: _.template(template),
        initialize: function (defaults) {
            this.render();
        },

        select: function (evt) {
            evt.preventDefault();
            this.model.collection.setSelected(this.model);
            application.router.navigate(this.model.get("Url"));
        },

        render: function () {
            $(this.el).html(this.template({ "model": this.model.toJSON() }));
            if (this.model.get("Selected")) {
                $(this.el).addClass("active");
            }
            return this;
        }

    });
    return listItemView;
});
