/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider.state('role', {
            url: '/RoleList',
            templateUrl: 'app/role/index.html',
            controller: 'roleCtrl'
        }).state('newrole', {
            url: '/NewRole',
            templateUrl: 'app/role/role.new.html',
            controller: 'rolenewCtrl'
        }).state('roleDetail', {
            url: '/RoleDetail/:rolename/:Id',
            templateUrl: 'app/role/role.edit.html',
            controller: 'roleUpdateCtrl'
        })
    });
})();