/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
/// <reference path="../../../lib-npm/angular-ui-router.min.js" />
/// <reference path="../../../lib-npm/angular-ui-router.js" />
"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider.state('store', {
            url: "/store",
            templateUrl: "app/Home/store/index.html",
            controller: 'storeCtrl',
            abstract: true
        }).state('store.profile', {
            url: "/profile",
            templateUrl: "app/Home/store/store.profile.html",
            controller: 'storeProfileCtrl'
        }).state('store.productlist', {
            url: '/productList',
            templateUrl: 'app/Home/store/store.productlist.html'
        }).state('store.productNew', {
            url: '/product.new',
            templateUrl: 'app/Home/store/store.product.new.html',
            controller: 'storeProductNewCtrl',
            resolve: {
                brandData: function ($http) {
                    return $http.get('api/Brand/GetAll').then(function (response) {
                        return response.data;
                    });
                },
                categoryData: function ($http) {
                    return $http.get('api/Category/GetCategory').then(function (response) {
                        return response.data;
                    });
                },
                storeData: function ($http) {
                    return $http.get('api/Store/Current').then(function (response) {
                        return response.data;
                    });
                }
            }
        });
    });
})();