/*
Cool way of newing up an application. Would be nice to eventually move to an xml file on the server. 
The header and footer get loaded once via the headerView and footerView widgets. 
{
    how the page transitions. driven off of the pages.index property
    transition: {
        forward: -> moving forward on the page.
        back: -> moving back on the page.
    }
    pages: -> collection of pages that get loaded dynamically based on the route.
        page: {Object}
        {
            name: -> {string} name of the page which is derived from the route. #/user -> page name is 'user'
            index: -> {int} where the page falls for transitions
            isDefault: -> {bool} should this page be displayed if there is no route?
            template: -> {string} the page parent template. exists in /templates directory. 
            widgets: -> {array} array of widgets to be loaded onto the page.
                widget: -> a widget that gets loaded onto the page.
                    name: -> {string} name of the widget. will be used when loading a widget. it will be [name]/[name]View.js for the widget.
                    directory: -> {string} name of the directory in which the widget lives. This will override the first name param in the case where we have a controller widget: [directory]/[name]View.js.
                    location: -> {string} location in the parent page template to put the widget.
                    defaults: -> {Object} the default properties/options passed into the widget upon creation.
            hideCarousel: -> {bool} should the page have the carousel on it? Happens in application.js after a page has been rendered.
        }
}
*/

window.config = {
    transition: {
        forward: {
            type: "opacity",
            duration: {
                start: 0,
                end: 1
            }
        },
        back: {
            type: "opacity",
            duration: {
                start: 0,
                end: 1
            }
        }
    },
    pages: [
        {
            name: 'home',
            index: 0,
            isDefault: true,
            template: "homeTemplate",
            widgets: []
        },
        {
            name: 'about',
            template: "aboutTemplate",
            widgets: []
        },
        {
            name: 'portfolio',
            index: 1,
            template: "portfolioTemplate",
            widgets: [],
            hideCarousel: true
        },
        {
            name: 'rates',
            index: 4,
            template: "ratesTemplate",
            widgets: [
                {
                    name: "listNavigation",
                    location: "#ratesContainer",
                    defaults: {
                        justification: "left",
                        Items: [
                            {
                                Id: "weddings",
                                Label: "Weddings",
                                Widget: null,
                                Selected: true
                            },
                            {
                                Id: "babies",
                                Label: "Babies",
                                Widget: {
                                    Path: "widgets/templateLoader/templateLoaderView",
                                    Model: {
                                        Template: "babyRatesTemplate"
                                    }
                                },
                                Selected: false
                            },
                            {
                                Id: "dress",
                                Label: "Trash The Dress",
                                Widget: null,
                                Selected: false
                            }
                        ]
                    }
                }
            ]
        },
        {
            name: 'contact',
            index: 5,
            template: "contactTemplate",
            widgets: [
                {
                    name: "listNavigation",
                    location: "#contactsContainer",
                    defaults: {
                        justification: "left",
                        ShowHeader: true,
                        Title: "Settings",
                        Items: [
                            {
                                Id: "blaa",
                                HasLabel: true,
                                Label: "blaa",
                                Widget: null,
                                Selected: true
                            },
                            {
                                Id: "bee",
                                HasLabel: true,
                                Label: "bee",
                                Widget: null,
                                Selected: false
                            },
                            {
                                Id: "blaablaa",
                                HasLabel: true,
                                Label: "blaablaa",
                                Widget: null,
                                Selected: false
                            }
                        ]
                    }
                }
            ]
        },
        {
            name: 'blog',
            template: "blogTemplate",
            widgets: []
        },
    //BACKEND PAGES
        {
        name: 'admin',
        template: "adminTemplate",
        index: 10,
        hideCarousel: true,
        widgets: [
                {
                    name: "listNavigation",
                    location: "#adminContainer",
                    defaults: {
                        ShowHeader: true,
                        Title: "Settings",
                        Items: [
                            {
                                Id: "users",
                                Label: "Site Users",
                                Widget: {
                                    Path: "widgets/secure/users/usersView"
                                },
                                Selected: true,
                                Url: "/admin/users"
                            },
                            {
                                Id: "tags",
                                Label: "Album Tags",
                                Widget: {
                                    Path: "widgets/secure/tags/tagsView"
                                },
                                Selected: false,
                                Url: "/admin/tags"
                            }
                        ]
                    }
                }
            ]
    },
        {
            name: 'user', //User page where people can update their info and add albums.
            template: "userTemplate",
            hideCarousel: true,
            widgets: [
                        {
                            name: "user",
                            directory: "secure/user",
                            location: "#userContainer"
                        }
                ]
        },
        {
            name: 'photoalbum', //Deceiving name, but its the backend version of the album.
            template: "photoAlbumTemplate",
            hideCarousel: true,
            widgets: [
                        {
                            name: "photoAlbum",
                            directory: "secure/photoalbum",
                            location: "#photoAlbumContainer"
                        }
                ]
        }
    ]
}