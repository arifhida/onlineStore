/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').directive('userexist', [
        '$http', '$q', function ($http, $q) {
            return {
                require: 'ngModel',
                link: function ($scope, element, attrs, ngModel) {
                    ngModel.$asyncValidators.userexist = function (username) {
                        return $http.post('api/Customer/CheckUserName', JSON.stringify(username), { headers: { 'Content-Type': 'application/json' } })
                        .then(function resolved() {
                            return true;
                        }, function rejected() {
                            return $q.rejected('username is exist');
                        });
                    }

                }
            }
        }
    ]).directive('emailexist', [
        '$http', '$q', function ($http, $q) {
            return {
                require: 'ngModel',
                link: function ($scope, element, attrs, ngModel) {
                    ngModel.$asyncValidators.emailexist = function (email) {
                        return $http.post('api/Customer/CheckEmail', JSON.stringify(email), { headers: { 'Content-Type': 'application/json' } })
                        .then(function resolved() {
                            return true;
                        }, function rejected() {
                            return $q.rejected('email is exist');
                        });
                    }
                }
            }
        }
    ]);
})();