define([
	'./router',
	'./config',
    './widgets/footer/footerView',
    './widgets/header/headerView',
    'i18n!./nls/global'
], function (
	Router,
	Config,
    Footer,
    Header,
    i18n
    ) {

    var Application = Backbone.View.extend({

        router: new Router(),
        config: new Config(),
        header: null,
        footer: null,

        el: $("#pageElements"),

        initialize: function () {
            this.router.on('route:page', this.onPageRoute, this);
            this.router.on('route:subpage', this.onSubPageRoute, this);
            this.router.on('route:default', this.onDefaultRoute, this);
            this.renderHeader();
            this.renderFooter();
        },

        showLoading: function (element) {
            if (typeof element === 'undefined' || element === null) {
                return;
            }
            $(element).prepend('<div class="loadSpinner"><div class="loadSpinnerIcon">Loading...</div></div>');
        },

        hideLoading: function (element, callback) {
            if (typeof element === 'undefined' || element === null) {
                return;
            }

            var $element = $(element);
            var children = $element.children();

            if (children.length > 0) {
                var $firstChild = $(children[0]);
                if ($firstChild.hasClass('loadSpinner')) {
                    $firstChild.fadeOut(500, function () { $(this).remove(); if (callback) { callback(); } });
                }
            }
        },

        start: function () {
            this.router.start();
        },

        pageIndex: 0,

        onDefaultRoute: function () {
            var defaultPage = _.find(this.config.get('pages'), function (thePage) {
                return thePage.isDefault;
            });

            if (defaultPage) {
                defaultPage.i18n = this.i18n;
                this.renderPage(defaultPage);
            }
            else {
                $(this.el).html('<h1>Bad Config<h1>');
            }
        },

        _determineTransition: function (nextPage) {
            var returnTransition;
            if (nextPage.index > this.pageIndex || 0) {
                returnTransition = "forward";
            } else {
                returnTransition = "back";
            }
            this.pageIndex = nextPage.index;
            return returnTransition;
        },

        _getPage: function (page) {
            var selectedPage = _.find(this.config.get('pages'), function (thePage) {
                return thePage.name.toLowerCase() === page.toLowerCase();
            });
            page.i18n = this.i18n;
            return selectedPage;
        },

        onSubPageRoute: function (page, id, subpage, subId, params) {

            //SubPage route will grab the subpage as the page and pass everything else as params to each widget.
            var selected = this._getPage(subpage);

            if (selected) {
                selected.params = params || {}; //pass all the child widgets the params if it exists.
                selected.params["PageId"] = id; //pass all the child widgets the id if it exists.
                selected.params["SubPage"] = subpage;
                selected.params["SubId"] = subId;
                selected.params["PageName"] = page;
                this.renderPage(selected);
            } else {
                $(this.el).html('<h1>Not Found<h1>');
            }
        },

        onPageRoute: function (page, id, action, subPage, params) {
            var selected = this._getPage(page);

            if (selected) {
                selected.params = params || {}; //pass all the child widgets the params if it exists.
                selected.params["PageId"] = id; //pass all the child widgets the id if it exists.
                this.renderPage(selected);
            } else {
                $(this.el).html('<h1>Not Found<h1>');
            }
        },

        renderPage: function (page) {
            var pageTemplate,
                that = this,
                widgetPathTemplate,
                direction = this._determineTransition(page),
                transition = this.config.get('transition');

            pageTemplate = _.template("text!./templates/<%- template %>.html", page);
            require([pageTemplate], function (template) {

                if (transition) {
                    var start = {},
                        end = {},
                        key = transition[direction].type;

                    start[key] = transition[direction].duration.start;
                    end[key] = transition[direction].duration.end;

                    $(that.el).removeAttr("style").animate(start, 400, "swing", function () {
                        $(this).html(function (index, oldhtml) {
                            that.requireWidget(page);
                            return template;
                        }).animate(end, 400, "swing", function () {
                            $(this).removeAttr("style");
                        });
                    });
                }
                else {
                    $(that.el).html(template);
                }


            });
            this._selectTab(page);
        },

        requireWidget: function (page) {
            //Render widgets dynamically.
            var that = this;
            widgetPathTemplate = _.template('widgets/<%= directory %>/<%- name %>View');
            _.each(page.widgets, function (widget) {
                //user can specify a directory to load OR by default we will default to the name of the widget.
                widget.directory = widget.directory || widget.name;
                require([widgetPathTemplate(widget)], function (Widget) {
                    widget.defaults = _.extend(widget.defaults || {}, page.params);
                    that.renderWidget(Widget, widget.name, widget.location, widget.defaults);
                });
            });
        },

        renderFooter: function () {
            this.footer = new Footer();
            $("#footer").html(this.footer.el);
        },

        renderHeader: function (page) {
            this.header = new Header();
            $("#header").html(this.header.el);
        },

        _selectTab: function (page) {
            this.header.select(page);
        },

        renderWidget: function (Widget, name, location, defaults) {
            this.$(location).append(new Widget(defaults).el);
        }
    });
    return Application;
});