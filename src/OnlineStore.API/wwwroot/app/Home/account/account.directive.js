/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').directive('storeExist', [
         '$http', '$q', function ($http, $q) {
             return {
                 require: 'ngModel',
                 link: function ($scope, element, attrs, ngModel) {
                     ngModel.$asyncValidators.storeexist = function (storename) {
                         return $http.get('api/Store/' + storename, JSON.stringify(storename), { headers: { 'Content-Type': 'application/json' } })
                       .then(function resolved() {
                           return true;
                       }, function rejected() {
                           return $q.rejected('name is exist');
                       });
                     }
                 }
             }
         }
    ]);
})();