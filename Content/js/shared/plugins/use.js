/* RequireJS Use Plugin v0.1.0
* Copyright 2012, Tim Branyen (@tbranyen)
* use.js may be freely distributed under the MIT license.
*/
define(function () {
    var buildMap = {};

    return {
        version: "0.1.0",

        // Invoked by the AMD builder, passed the path to resolve, the require
        // function, done callback, and the configuration options.
        //
        // Configuration format
        // --------------------------------------------------------------------------
        //
        // The string property used in attach will resolve to window[stringProp]
        // Functions are evaluated in the scope of the window and passed all
        // arguments.
        //
        // require.config({
        //   use: {
        //     "libs/underscore": {
        //       attach: "_"
        //     },
        //  
        //     "libs/backbone": {
        //       deps: ["use!underscore", "jquery"],
        //       attach: function(_, $) {
        //         return this.Backbone.noConflict();
        //       }
        //     }
        //   }
        // });
        //
        load: function (name, req, load, config) {
            var module = config.use && config.use[name];

            // No module to load so return early.
            if (!module) {
                return load();
            }

            if (config.isBuild) {
                var fs = require.nodeRequire("fs");
                var url = req.toUrl(name);

                if (/\.js$/.test(url) === false) {
                    url += ".js";
                }

                buildMap[name] = {
                    content: fs.readFileSync(url, "utf8"),
                    attach: module.attach
                };

                return load();
            }

            // Read the current module configuration for any dependencies that are
            // required to run this particular non-AMD module.
            req(module.deps || [], function () {
                // Require this module
                req([name], function () {
                    // Attach property
                    var attach = config.use[name].attach;

                    // Return the correct attached object
                    if (typeof attach == "function") {
                        return load(attach.apply(window, arguments));
                    }

                    // Use window for now (maybe this?)
                    return load(window[attach]);
                });
            });
        },

        write: function (pluginName, moduleName, write, config) {
            if (moduleName in buildMap) {
                var module = buildMap[moduleName];
                var content = module.content;
                var attach = module.attach;
                var def = "define(function() { " + content + ";";

                if (module.attach) {
                    if (typeof attach == "function") {
                        // convert the attach fn to a string
                        var fn = attach.toString();

                        // append the file and the logic (body of the attach fn) to attach it.
                        def += fn.substring(fn.indexOf("{") + 1, fn.lastIndexOf("}")).trim();
                    } else {
                        def += "return " + attach + ";";
                    }
                }

                // close off the define
                def += "});\n";

                // write module
                write.asModule(pluginName + "!" + moduleName, def);
            }
        }

    };

});