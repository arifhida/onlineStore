/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />
/// <reference path="../../lib-npm/angular-ui-router.min.js" />
/// <reference path="../../lib-npm/angular-ui-router.js" />
"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        $urlRouterProvider.otherwise("main");        
        $locationProvider.html5Mode(true);
    });
})();