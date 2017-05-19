/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').controller('ctrlLeftNav',
        ['$scope', '$http', function ($scope, $http) {
            $scope.getCategory = function () {
                $http.get('api/Category/GetAll').then(function (response) {
                    $scope.categoryList = response.data;
                });
            }
            $scope.getCategory();
        }]).controller('ActNavCtrl', [
            '$scope', '$http', '$state', '$rootScope', 'localStorageService', 'authenticate',
            function ($scope, $http, $state, $rootScope, localStorageService, authenticate) {
                $scope.isConfirmed = false;
                $scope.isAuthenticated = function () {
                    var isAuth = localStorageService.get('authentication');
                    $scope.isConfirmed = (isAuth !== null) ? isAuth.Confirmed : false;
                    return (isAuth !== null);
                }
                
                $scope.logout = function () {
                    authenticate.logout();
                    $rootScope.globals = null;
                    $state.go('main');
                }
            }
        ]);
})();