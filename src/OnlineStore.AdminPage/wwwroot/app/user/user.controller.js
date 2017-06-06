/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').controller('userCtrl', [
        '$scope', '$http', '$ngConfirm', function ($scope, $http, $ngConfirm) {
            $scope.q = '';
            $scope.page = 0;
            $scope.totalPage = 0;
            $scope.pageSize = 10;
            $scope.getData = function () {
                $http.get('http://localhost:58969/api/User/GetData', { headers: { 'q': $scope.q, 'Pagination': $scope.page + ',' + $scope.pageSize } }).then(function (response) {
                    if (response.status == 200) {
                        $scope.Userlist = response.data;
                        var pagination = JSON.parse(response.headers('Pagination'))
                        $scope.page = pagination.CurrentPage;
                        $scope.totalPage = pagination.TotalPages;
                        $scope.pageSize = pagination.ItemsPerPage;
                    } else {
                        console.log(response);
                    }
                });
            }
            $scope.Delete = function (user) {
                $scope.deleteUser = user;
                $ngConfirm({
                    title: 'Delete?',
                    content: 'Are you sure want to delete <strong>{{deleteUser.UserName}}</strong>?',
                    scope: $scope,
                    buttons: {
                        Delete: {
                            text: 'Delete',
                            btnClass: 'btn-danger',
                            action: function (scope, button) {
                                $http.delete('http://localhost:58969/api/User/' + user.Id)
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
            $scope.getData();
        }
    ]).controller('UserDetailCtrl', [
        '$scope', '$http', '$stateParams', '$state', 'roledata', function ($scope, $http, $stateParams, $state, roledata) {
            $scope.Gender = [
                { Id: 1, Name: "Male" },
                { Id: 2, Name: "Female" }
            ];
            $scope.roles = roledata;
            $scope.Deleted = [];
            var param = $stateParams.Id;
            $http.get('http://localhost:58969/api/User/GetUserById?UserId=' + param).then(function (response) {
                console.log(response);
                $scope.data = response.data;
                $scope.data.BirthDate = new Date(response.data.BirthDate);
            });
            $scope.loading = false;
            $scope.Save = function () {
                $scope.loading = true;
                $http.put('http://localhost:58969/api/User/' + param, $scope.data).then(function (response) {
                    $state.go('user');
                });              
                
            }
            $scope.change = function (role) {
                if ($scope.data != null) {
                    var p = new Object();
                    var exist = false;
                    for (var i = 0; i < $scope.data.Roles.length; i++) {
                        if ($scope.data.Roles[i].Role.Id == role.Id && $scope.data.Roles[i].Id !== 0) {
                            exist = true;
                            $scope.data.Roles[i].Delete = true;
                        } else if ($scope.data.Roles[i].Role.Id == role.Id && $scope.data.Roles[i].Id == 0) {
                            $scope.data.Roles.splice(i, 1);
                            exist = true;
                        }

                    } if (exist == false) {
                        p.Id = 0;
                        p.Role = role;
                        p.Delete = false;
                        $scope.data.Roles.push(p);
                    }
                    console.log($scope.data.Roles);
                }                
            }

            $scope.checked = function (Id, Roles) {
                if (angular.isArray(Roles)) {
                    for (var i = 0; i < Roles.length; i++) {
                        if (Roles[i].Role.Id == Id && Roles[i].Delete == false)
                            return true;
                    }
                } else {
                    return false;
                }
            }
        }
    ]).controller('UserNewCtrl', [
        '$scope', '$http', '$state', 'roledata', function ($scope, $http, $state, roledata) {
            $scope.Gender = [
               { Id: 1, Name: "Male" },
               { Id: 2, Name: "Female" }
            ];
            $scope.original = {
                Id: 0,
                UserName: '',
                FullName: '',
                Email: '',
                Password: '',
                Gender: '',
                BirthDate: new Date(),
                Roles: []
            };
            $scope.data = angular.copy($scope.original);
            $scope.roles = roledata;
            $scope.loading = false;
            $scope.Save = function () {
                $scope.loading = true;
                $http.post('http://localhost:58969/api/User/Add', $scope.data, { headers: { 'Content-Type': 'application/json' } })
                    .then(function (response) {
                        $state.go('user');
                    });
            }
            $scope.click = function (role) {
                if ($scope.data.Roles) {
                    var exist = false;
                    for (var i = 0; i < $scope.data.Roles.length; i++) {
                        if ($scope.data.Roles[i].Role.Id == role.Id) {
                            exist = true;                            
                            $scope.data.Roles.splice(i, 1);
                        }
                    }
                    if (!exist) {
                        var p = new Object();
                        p.Id = 0;
                        p.Role = role;
                        $scope.data.Roles.push(p);
                    }
                }
                console.log($scope.data.Roles);
            }
        }
    ]);
})();