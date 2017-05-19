"use strict";
(function () {
    angular.module('authentication').controller('loginController', [
        '$scope', 'authenticate', function ($scope, authenticate) {
            $scope.loginData = {
                Username: '',
                Password: ''
            };
            $scope.loading = false;            
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
                    console.log(response);
                    $scope.loading = false;
                });
            }

        }
    ]).controller('logoutController', [
        '$scope', 'authenticate', '$state', '$rootScope', function ($scope, authenticate, $state, $rootScope) {
            $scope.auth = false;
            $scope.isAuthenticated = function () {
                $scope.auth = authenticate.isAuthenticate();
                return authenticate.isAuthenticate();
            }

            $scope.logout = function () {
                authenticate.logout();
                $rootScope.globals = null;
                $state.go('login');
            }
        }
    ]);
})();