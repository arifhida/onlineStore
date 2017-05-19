/// <reference path="../../lib/angular/angular.min.js" />
/// <reference path="../../lib/angular/angular.js" />

"use strict";
(function () {
    angular.module('app').controller('mainController', [
        '$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {
            $scope.username = $rootScope.globals.UserName;            
        }
    ]);
})();