/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').controller('mainController',
        ['$scope', '$http', '$location', '$state', 'authenticate','PagerService','localStorageService',
            function ($scope, $http, $location, $state, authenticate, PagerService, localStorageService) {
                $scope.getCategory = function () {
                    $http.get('api/Category/GetAll').then(function (response) {
                        $scope.categoryList = response.data;
                    });
                }
                $scope.q = '';
                $scope.page = 0;
                $scope.pageSize = 10;
                $scope.category = null;
                $scope.getData = function () {
                    $scope.category = null;
                    $scope.categories = [];
                    $http.get('api/Product', { headers: { 'q': $scope.q, 'Pagination': $scope.page + ',' + $scope.pageSize } }).then(function (response) {
                        $scope.productList = response.data;
                        var pagination = JSON.parse(response.headers('Pagination'))
                        $scope.page = pagination.CurrentPage;
                        $scope.totalPage = pagination.TotalPages;
                        $scope.pageSize = pagination.ItemsPerPage;
                        $scope.pages = PagerService.GetPager($scope.page, $scope.totalPage, $scope.pageSize).pages;
                    });
                }
                if (localStorage.getItem('cart')) {
                    $scope.cart = localStorage.getItem('cart')
                } else {
                    $scope.cart = {};
                }
                $scope.navClick = function (item) {
                    $scope.categories = [];
                    $scope.category = item;
                    getCategory(item);
                    $http.get('api/Product/Category/' + item.Id).then(function (response) {
                        $scope.productList = response.data;
                        var pagination = JSON.parse(response.headers('Pagination'))
                        $scope.page = pagination.CurrentPage;
                        $scope.totalPage = pagination.TotalPages;
                        $scope.pageSize = pagination.ItemsPerPage;
                        $scope.pages = PagerService.GetPager($scope.page, $scope.totalPage, $scope.pageSize).pages;
                    });
                    console.log($scope.categories);
                    
                }
                $scope.categories = [];
                $scope.getData();

                $scope.getCategory();
                var searchObject = $location.search();
                if (searchObject.q) {
                    authenticate.logout();
                    $state.go(searchObject.q);
                }
                $scope.setPage = function (page) {
                    $scope.page = page;
                }
                function getCategory(node) {
                    $scope.categories.push(node.Id);                    
                    if (node.Children) {
                        for (var i = 0; i < node.Children.length; i++) {
                            getCategory(node.Children[i]);
                        }                       
                    }
                }

                $scope.addToChart = function (item) {

                }
            
        }]);
})();