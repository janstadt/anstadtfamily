define([], function () {
    var TemplateLoaderView = Backbone.View.extend({
        template: null,
        initialize: function (options) {
            if (options && options.Template) {
                var self = this,
                temp = _.template("text!./templates/<%- template %>.html", { "template": options.Template });
                require([temp], function (template) {
                    self.template = _.template(template);
                    self.render();
                });
            }
        },

        render: function () {
            $(this.el).html(this.template(this.options));
            return this;
        }
    });
    return TemplateLoaderView;
});
