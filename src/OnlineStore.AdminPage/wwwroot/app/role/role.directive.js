/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').directive('roleExist', [
        '$http', '$q', function ($http, $q) {
            return {
                require: 'ngModel',
                link: function ($scope, element, attrs, ngModel) {
                    ngModel.$asyncValidators.roleExist = function (rolename) {
                        return $http.get('http://localhost:58969/api/Role/' + rolename, JSON.stringify(rolename), { headers: { 'Content-Type': 'application/json' } })
                        .then(function resolved() {
                            return true;
                        }, function rejected() {
                            return $q.rejected('rolename is exist');
                        });
                    }
                }
            }
        }
    ]);
})();