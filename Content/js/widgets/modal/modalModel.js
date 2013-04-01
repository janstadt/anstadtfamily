define(["i18n!./nls/modal"], function (i18n) {
    var ModalModel = Backbone.Model.extend({
        i18n: i18n,
        defaults: {
            Id: null,
            Content: null,
            Title: null,
            ShowFooter: false,
            ClassName: ""
        }
    });
    return ModalModel;
});
