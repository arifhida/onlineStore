/// <reference path="../lib-npm/angular/angular.js" />
/// <reference path="../lib-npm/angular/angular.min.js" />

"use strict";
(function () {
    angular.module('authentication', ['ui.router', 'LocalStorageModule']);
    angular.module('app', ['authentication', 'ui.tree', 'cp.ngConfirm']);

    angular.module('app').run(function ($rootScope, $state, localStorageService) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
            $rootScope.globals = localStorageService.get('authentication');
            if (!$rootScope.globals && toState.name == 'checkout') {
                event.preventDefault();
                return $state.go('login');
            }
        });
    });
})();