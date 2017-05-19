/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
/// <reference path="../../../lib-npm/angular-ui-router.min.js" />
/// <reference path="../../../lib-npm/angular-ui-router.js" />
"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider.state('account', {
            url: "/account",
            templateUrl: "app/Home/account/index.html",
            controller: 'accountCtrl',
            abstract: true
        }).state('account.profile', {
            url: '/profile',
            templateUrl: 'app/Home/account/account.profile.html',
            controller: 'accountDetailCtrl'
        }).state('account.purchasing', {
            url: '/purchasing',
            templateUrl: 'app/Home/account/account.purchasing.html',
            controller: 'accountPurchaseCtrl'
        }).state('account.store', {
            url: "/store",
            templateUrl: "app/Home/account/account.store.html",
            controller: 'accountStoreCtrl'
        });
    });
})();