/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />

"use strict";
(function () {
    angular.module('app').controller('brandCtrl', [
        '$scope', '$http', function ($scope, $http) {
            $scope.q = '';
            $scope.page = 0;
            $scope.totalPage = 0;
            $scope.pageSize = 10;
            $scope.getData = function () {
                $http.get('http://localhost:58969/api/Brand/GetBrand', { headers: { 'Content-Type': 'application/json', 'q': $scope.q, 'Pagination': $scope.page + ',' + $scope.pageSize } }).then(
                    function (response) {
                        var pagination = JSON.parse(response.headers('Pagination'))
                        $scope.page = pagination.CurrentPage;
                        $scope.totalPage = pagination.TotalPages;
                        $scope.pageSize = pagination.ItemsPerPage;
                        $scope.brandList = response.data;
                    });
            }
            $scope.getData();


        }]).controller('newbrandCtrl', [
            '$scope', '$http', '$state', function ($scope, $http, $state) {
                $scope.loading = false;

                $scope.Save = function () {
                    $scope.loading = false;
                    $http.post('http://localhost:58969/api/Brand/AddBrand', $scope.data, { headers: { 'Content-Type': 'application/json' } })
                        .then(function (response) {
                            $state.go('brand');
                        });
                }
            }
        ]);
})();