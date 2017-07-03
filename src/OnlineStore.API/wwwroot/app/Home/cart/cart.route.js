/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />


"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider.state('cart', {
            url: '/cart',
            templateUrl: 'app/Home/cart/index.html',
            controller: 'cartCtrl'
        });
    });
})();