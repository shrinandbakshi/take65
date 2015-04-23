'use strict';

//WL.Event.subscribe("auth.login", onLogin);
//alert("liveid : " + _authConfig.live.client_id);
WL.init({
    client_id: _authConfig.live.client_id,
    redirect_uri: window.location.origin + _authConfig.live.redirect_url,
    scope: ["wl.signin", "wl.basic", "wl.emails", "wl.contacts_emails"],
    response_type: "code"
});
//WL.ui({
//    name: "signin",
//    element: "signin"
//});

function onLogin(session) {
    if (!session.error) {
        WL.api({
            path: "me",
            method: "GET"
        }).then(
            function (response) {
                // console.info("Hello, " + response.first_name + " " + response.last_name + "!");
            },
            function (responseFailed) {
                console.error("Error calling API: " + responseFailed.error.message);
            }
        );
    }
    else {
        console.error("Error signing in: " + session.error_description);
    }
}

function onGoogleApiLoad() {
    gapi.client.setApiKey(_googleConfig.api_key);
}

// Declare app level module which depends on filters, and services
var App = angular.module('App', [
    'XSockets',
    'ngRoute',
    'ngAnimate',
    //'ngMobile',
    'App.controllers',
    'App.filters',
    'App.services',
    'App.directives',
    'angular-underscore'
]).run(['$rootScope', function ($rootScope) {
    $rootScope.now = new Date();

    if ((typeof _userName === 'string' || typeof _userEmail === 'string') && _currentEnvironment == 'sandbox') {
        Candy.init(_chatCoreUrl, {
            core: { debug: false, autojoin: [_chatRoom] },
            view: { resources: '/Img/Default/' }
        }); //take65@conference.kwwcws / tak65@conference.amazona-kd5nje2

        Candy.Core.connect(_chatUrl, null, _userName || _userEmail.split('@')[0]);
    }
}]).config(function ($routeProvider, $locationProvider) {
    $routeProvider
        .when('/', {})
          .otherwise({
              redirectTo: '/'
          });

    $locationProvider.html5Mode(false);
});
