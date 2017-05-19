/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />

"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider.state('login', {
            url: "/login",
            templateUrl: "app/login/login.html",
            controller: 'loginController'
        });
    });
})();