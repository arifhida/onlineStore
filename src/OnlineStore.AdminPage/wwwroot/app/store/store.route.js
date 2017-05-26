/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider.state('store', {
            url: '/StoreList',
            templateUrl: 'app/store/index.html',
            controller: 'storeCtrl'
        }).state('storeDetail', {
            url: '/storeDetail/:username/:Id',
            templateUrl: 'app/store/store.detail.html',
            controller: 'storeDetailCtrl'
        });
    });
})();