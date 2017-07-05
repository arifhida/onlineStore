/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
/// <reference path="../../../lib-npm/angular-ui-router.min.js" />
/// <reference path="../../../lib-npm/angular-ui-router.js" />
"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {        
        $stateProvider.state('main', {
            url: "/",
            templateUrl: "app/Home/main/main.html",
            controller: "mainController"
        }).state('itemDetail', {
            url: "/:Id/Detail",
            templateUrl: 'app/Home/main/item.detail.html',
            controller: 'detailCtrl'
        });
    });
})();