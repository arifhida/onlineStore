/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app')
        .controller('roleCtrl', [
            '$scope', '$http', '$state', '$ngConfirm',
            function ($scope, $http, $state, $ngConfirm) {
                $scope.loading = false;
                $scope.q = '';
                $scope.page = 0;
                $scope.totalPage = 0;
                $scope.pageSize = 10;
                $scope.getData = function () {
                    $http.get('http://localhost:58969/api/Role/GetData', { headers: { 'Content-Type': 'application/json', 'q': $scope.q, 'Pagination': $scope.page + ',' + $scope.pageSize } }).then(function (response) {
                        $scope.roleList = response.data;
                        var pagination = JSON.parse(response.headers('Pagination'))
                        $scope.page = pagination.CurrentPage;
                        $scope.totalPage = pagination.TotalPages;
                        $scope.pageSize = pagination.ItemsPerPage;
                    });
                }
                $scope.getData();
                $scope.delete = function (role) {
                    $scope.deleteRole = role;
                    $ngConfirm({
                        title: 'Delete?',
                        content: 'Are you sure want to delete <strong>{{deleteRole.RoleName}}</strong>?',
                        scope: $scope,
                        buttons: {
                            Delete: {
                                text: 'Delete',
                                btnClass: 'btn-danger',
                                action: function (scope, button) {
                                    $http.delete('http://localhost:58969/api/Role/' + role.Id)
                                        .then(function (response) {
                                            $scope.getData();
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
        ])
    .controller('rolenewCtrl', [
        '$scope', '$http', '$state', '$ngConfirm',
        function ($scope, $http, $state, $ngConfirm) {
            $scope.loading = false;
            $scope.Save = function () {
                $scope.loading = true;
                $http.post('http://localhost:58969/api/Role/Add', $scope.data, { headers: { 'Content-Type': 'application/json' } })
                .then(function (response) {
                    $state.go('role');
                }, function (error) {

                });
            }
        }
    ])
    .controller('roleUpdateCtrl', [
        '$scope', '$http', '$state','$stateParams', '$ngConfirm',
        function ($scope, $http, $state, $stateParams, $ngConfirm) {
            var param = $stateParams.Id;
            $http.get('http://localhost:58969/api/Role/' + param)
                .then(function (response) {
                    $scope.data = response.data;
                });
            $scope.loading = false;
            $scope.Save = function () {
                $scope.loading = true;
                $http.put('http://localhost:58969/api/Role/' + $scope.data.Id, $scope.data, { headers: { 'Content-Type': 'application/json' } })
                .then(function (response) {
                    $state.go('role');
                }, function (error) {

                });
            }
        }
    ]);
})();