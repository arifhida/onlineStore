/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />

"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider.state('user', {
            url: "/UserList",
            templateUrl: "app/user/index.html",
            controller: 'userCtrl'
        }).state('userdetail', {
            url: "/User_detail/:username/:Id",
            templateUrl: 'app/user/user.detail.html',
            controller: 'UserDetailCtrl',
            resolve: {
                roledata: function ($http) {
                    return $http.get('http://localhost:58969/api/Role/GetAll').then(function (response) {
                        return response.data;
                    });
                }
            }
        }).state('usernew', {
            url: "/User_New",
            templateUrl: 'app/user/user.new.html',
            controller: 'UserNewCtrl',
            resolve: {
                roledata: function ($http) {
                    return $http.get('http://localhost:58969/api/Role/GetAll').then(function (response) {
                        return response.data;
                    });
                }
            }
        });
    });
})();