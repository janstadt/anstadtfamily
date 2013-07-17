define([],
    function () {
          _.extend(Backbone.Validation.validators, {
              spam: function(value, attr, customValue, model) {
                if(value !== model.get("Total").toString()){
                  return "Incorrect number entered.";
                }
              }
        });
        var ContactUsModel = Backbone.Model.extend({
            url: "/api/secure/contact",
            validation: {
                Name: {
                    required: true
                },
                Email: {
                    pattern: "email",
                    required: true
                },
                Message: {
                    required: true
                },
                Number: {
                    required: true,
                    spam: true
                }
            }
    });
    return ContactUsModel;
});
