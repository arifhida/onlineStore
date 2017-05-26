﻿/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
'use strict';
(function () {
    angular.module('app').controller('storeCtrl', [
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
    ]).controller('storeProfileCtrl', [
        '$scope', '$http', '$ngConfirm', function ($scope, $http, $ngConfirm) {
            $scope.logo = 'http://localhost:58969/images/shop.jpg';
            $scope.loading = false;
            $scope.getData = function () {
                $http.get('api/Store/Current')
                    .then(function (response) {
                        $scope.shop = response.data;
                    });
            }
            $scope.getData();
            $scope.Save = function () {
                $scope.loading = true;
                $scope.shop.StoreLogo = $scope.logo;
                $http.post('api/Store/Save', $scope.shop, { headers: { 'Content-Type': 'application/json' } })
                    .then(function (response) {
                        $ngConfirm({
                            title: 'Success',
                            scope: $scope,
                            content: '<strong>{{shop.StoreName}}</strong> updated.',
                            buttons: {
                                OK: function (scope, button) {
                                    scope.loading = false;
                                }
                            }
                        });
                        $scope.loading = false;
                    });
            }
        }
    ]).controller('storeProductListCtrl', [
        '$scope', '$http', '$ngConfirm', function ($scope, $http, $ngConfirm) {
            $scope.getData = function () {
                $http.get().then(function (response) {
                    $scope.productList = response.data;
                });
            }
            $scope.getData();
        }
    ]).controller('storeProductNewCtrl', [
        '$scope', '$http', '$ngConfirm', 'brandData','categoryData', 'storeData',
    function ($scope, $http, $ngConfirm, brandData, categoryData,storeData) {
            $scope.remove = function (item) {
                $scope.images.splice($scope.images.indexOf(item), 1);
            }
            $scope.StoreId = storeData.Id;
            $scope.autocomplete = false;
            $scope.categoryList = categoryData;
            $scope.brandList = brandData;
            $scope.findNodes = function () {
                $scope.autocomplete = true;
            };
            $scope.setCategory = function (item) {
                $scope.categoryName = item.CategoryName;
                $scope.query = item.CategoryName;
                $scope.autocomplete = false;
                $scope.product.CategoryId = item.Id;
            }
            $scope.visible = function (item) {
                return !($scope.query && $scope.query.length > 0
                            && item.CategoryName.indexOf($scope.query) == -1);
            }
            $scope.condition = [
                 { Id: 0, Name: "Brand New" },
                { Id: 1, Name: "Used" }
            ];
            $scope.Availability = [
                { Id: true, Name: "Yes" },
                { Id: false, Name: "No" }
            ];

            $scope.Save = function () {
                $scope.product.Image = $scope.images;
                $http.post('api/Product/Add', $scope.product, { headers: { 'Content-Type': 'application/json' } })
                    .then(function (response) {
                        $ngConfirm('data is successfully saved.');
                    },
                    function (err) {

                    });
            }
        }
    ]);
})();