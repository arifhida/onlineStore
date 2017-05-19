/// <reference path="../lib/angular/angular.min.js" />
/// <reference path="../lib/angular/angular.js" />
"use strict";
(function () {
    angular.module('authentication', ['ui.router', 'LocalStorageModule', 'angular-jwt']);
    angular.module('app', ['authentication', 'checklist-model', 'ui.tree', 'ngTable', 'cp.ngConfirm']);
    angular.module('app').controller('navController', function ($scope, $rootScope, localStorageService) {
        $scope.isAuthenticated = function () {
            var isAuth = localStorageService.get('authentication');
            return (isAuth == null);
        }
    });
    angular.module('app').run(function ($rootScope, $state, localStorageService) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
            $rootScope.globals = localStorageService.get('authentication');
            if (!$rootScope.globals && toState.name != 'login') {
                event.preventDefault();
                return $state.go('login');
            }
        });
    });
})();