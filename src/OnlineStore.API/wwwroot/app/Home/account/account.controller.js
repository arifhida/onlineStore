/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').controller('accountCtrl', [
        '$scope', '$http', function ($scope, $http) {
            $scope.getUser = function () {
                $http.get('api/User/CurrentUser')
                .then(function (response) {
                    $scope.User = response.data;
                    $scope.User.BirthDate = new Date(response.data.BirthDate);
                    console.log($scope.User);
                });
            }
            $scope.Gender = [
                { Id: 1, Name: "Male" },
                { Id: 2, Name: "Female" }
            ];
            $scope.getUser();
        }
    ]).controller('accountDetailCtrl', [
        '$scope', '$http', function ($scope, $http) {
            $scope.profile = 'http://localhost:58969/images/user.png';
            $scope.getUser = function () {
                $http.get('api/User/CurrentUser')
                .then(function (response) {
                    $scope.User = response.data;
                    $scope.User.BirthDate = new Date(response.data.BirthDate);
                    $scope.profile = $scope.User.Photo !== null ? $scope.User.Photo : $scope.profile;
                    console.log($scope.User);
                    $scope.error = $scope.User.isConfirmed == false ? 'Please Confirm your email.' : null;
                    $scope.status = $scope.User.isConfirmed == false ? 401 : 200;
                });
            };

            
            $scope.loading = false;
            $scope.Gender = [
                { Id: 1, Name: "Male" },
                { Id: 2, Name: "Female" }
            ];
            $scope.getUser();
            $scope.Save = function () {
                $scope.loading = true;
                $http.post('api/Customer/UpdateProfile', $scope.User, { headers: { 'Content-Type': 'application/json' } })
                    .then(function (response) {
                        $scope.status == response.status;
                        $scope.error = null;
                        $scope.loading = false;
                    }, function (error) {
                        scope.status == error.status;
                        $scope.error = error.data;
                        $scope.loading = false;
                    });
            }
            $scope.address = {
                Id: 0,
                Phone: '',
                Address: '',
                District: '',
                City: '',
                Province: '',
                PostalCode: ''
            };

            $scope.getAddress = function () {
                $http.get('api/Address/GetAddress')
                    .then(function (response) {
                        if (response.status == 200) {
                            $scope.cust = response.data;
                        } else {
                            $scope.cust = angular.copy($scope.address);
                        }
                        console.log(response.data);
                        console.log($scope.cust);
                    }, function (err) {
                        $scope.cust = angular.copy($scope.address);
                    });
            }
            $scope.getAddress();

            $scope.SaveAddress = function () {
                if ($scope.cust.Id == 0) {
                    $http.post('api/Address/Add', $scope.cust, { headers: { 'Content-Type': 'application/json' } })
                        .then(function (response) {
                            $scope.error == null;
                        }, function (error) {
                            $scope.error == error.data;
                        });
                } else {
                    $http.put('api/Address/' + $scope.cust.Id, $scope.cust, { headers: { 'Content-Type': 'application/json' } })
                        .then(function (response) {
                            $scope.error == null;
                        }, function (error) {
                            console.log(error);
                            $scope.status == error.status;
                            $scope.error == error.data;
                        });
                }
            }
        }
    ]).controller('accountPurchaseCtrl', [
        '$scope', '$http', function ($scope, $http) {

        }
    ]).controller('accountStoreCtrl', [
        '$scope', '$http','$state', function ($scope, $http, $state) {
			$scope.logo = 'http://localhost:58969/images/shop.jpg';
			$scope.loading = false;
			$scope.Save = function () {
			    $scope.loading = true;

			}
        }
    ])
})();