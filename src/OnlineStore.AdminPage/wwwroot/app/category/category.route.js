/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />

"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider.state('category', {
            url: '/category',
            templateUrl: 'app/category/index.html',
            controller: 'categoryController',
            resolve: {
                initialData: function ($http) {
                    return $http.get('http://localhost:58969/api/Category/GetAll').then(function (response) {
                        return response.data;
                    });
                }
            }
        });
    });
})();