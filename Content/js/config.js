define([], function() {

	var Config = Backbone.Model.extend({

		initialize: function() {
			this.set(window.config);
		}

	});

	return Config;

});