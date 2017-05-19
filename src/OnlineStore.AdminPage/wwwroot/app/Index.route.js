/// <reference path="../scripts/app.js" />
/// <reference path="../lib/angular/angular.min.js" />
/// <reference path="../lib/angular/angular.js" />
/// <reference path="../lib-npm/angular-ui-router.min.js" />
/// <reference path="../lib-npm/angular-ui-router.js" />
"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        $urlRouterProvider.otherwise("main");
        $stateProvider
        .state('main', {
            url: "/",
            templateUrl: "app/main/main.html",
            controller: 'mainController'
        });
        $locationProvider.html5Mode(true);
    });
})();