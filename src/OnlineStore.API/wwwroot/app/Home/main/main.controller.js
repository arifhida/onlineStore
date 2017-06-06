/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').controller('mainController',
        ['$scope', '$http', '$location', '$state', 'authenticate',
            function ($scope, $http, $location, $state, authenticate) {
                $scope.getCategory = function () {
                    $http.get('api/Category/GetAll').then(function (response) {
                        $scope.categoryList = response.data;
                    });
                }

                $scope.getData = function () {
                    $http.get('api/Product').then(function (response) {
                        $scope.productList = response.data;
                    });
                }
                $scope.getData();

                $scope.getCategory();
                var searchObject = $location.search();
                if (searchObject.q) {
                    authenticate.logout();
                    $state.go(searchObject.q);
                }

            
        }]);
})();