/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />
'use strict';
(function () {
    angular.module('app').controller('storeCtrl', [
        '$scope', '$http', '$ngConfirm', function ($scope, $http, $ngConfirm) {
            $scope.q = '';
            $scope.page = 0;
            $scope.totalPage = 0;
            $scope.pageSize = 10;
            $scope.getData = function () {
                $http.get('http://localhost:58969/api/Store/GetData', { headers: { 'q': $scope.q, 'Pagination': $scope.page + ',' + $scope.pageSize } }).then(function (response) {
                    if (response.status == 200) {
                        $scope.storeList = response.data;
                        var pagination = JSON.parse(response.headers('Pagination'))
                        $scope.page = pagination.CurrentPage;
                        $scope.totalPage = pagination.TotalPages;
                        $scope.pageSize = pagination.ItemsPerPage;
                    } else {
                        console.log(response);
                    }

                });
            }
            $scope.getData();
            $scope.delete = function (store) {
                $scope.deleteStore = store;
                $ngConfirm({
                    title: 'Delete?',
                    content: 'Are you sure want to delete <strong>{{ deleteStore.StoreName }}</strong>?',
                    scope: $scope,
                    buttons: {
                        Delete: {
                            text: 'Delete',
                            btnClass: 'btn-danger',
                            action: function (scope, button) {
                                $http.delete('http://localhost:58969/api/Store/' + store.Id)
                                    .then(function (response) {
                                        scope.getData();
                                    }, function (error) {
                                        $ngConfirm('Data not found');
                                    });
                            }
                        },
                        Cancel: function (scope, button) {

                        }

                    }
                });
            }
        }
    ]).controller('storeDetailCtrl', [
        '$scope', '$http', '$ngConfirm', '$stateParams', '$state',
        function ($scope, $http, $ngConfirm, $stateParams, $state) {
            var param = $stateParams.Id;
            $scope.getData = function () {
                $http.get('http://localhost:58969/api/Store/' + param)
                    .then(function (response) {
                        $scope.data = response.data.Store;
                        $scope.user = response.data;
                    }, function (response) {

                    });
            }
            $scope.getData();
        }
    ]);
})();