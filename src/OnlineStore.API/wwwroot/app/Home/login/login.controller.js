/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').controller('loginController',
        ['$scope', '$http', 'authenticate', function ($scope, $http, authenticate) {
            $scope.loginData = {
                Username: '',
                Password: ''
            };
            $scope.Gender = [
                { Id: 1, Name: "Male" },
                { Id: 2, Name: "Female" }
            ];
            
            $scope.loading = false;
            $scope.creatingUser = false;
            $scope.login = function () {
                $scope.loading = true;               
                authenticate.login($scope.loginData, function (response, $state) {
                    if (response.status == 200) {
                        $scope.status == response.status;
                        $state.go('main');
                    } else {
                        $scope.status == response.status;
                        $scope.error = response.data;
                    }
                    $scope.loading = false;
                });
            }
            $scope.CreateUser = function () {                
                $scope.creatingUser = true;                
                authenticate.register($scope.regData, function (response, $state) {
                    if (response.status == 200) {
                        $scope.status == response.status;
                        $state.go('main');
                    } else {
                        $scope.status == response.status;
                        $scope.error = response.data;
                    }
                    $scope.creatingUser = false;
                });
            }
        }]);
})();