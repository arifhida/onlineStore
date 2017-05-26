/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />
'use strict';
(function () {
    angular.module('app').directive('skuexist', [
        '$parse', '$http', '$q', function ($parse, $http, $q) {
            return {
                require: 'ngModel',
                link: function ($scope, element, attrs, ngModel) {
                    var model = $parse(attrs.storeId);
                    var StoreId = model($scope);                    
                    ngModel.$asyncValidators.skuexist = function (sku) {
                        console.log(StoreId);
                        return $http.get('api/Product/CheckSKU/' + sku + '/' + StoreId).then(
                            function resolved() {
                                console.log(StoreId);
                                return true;
                            }, function rejected() {
                                return $q.rejected('SKU is exist');
                            });
                    }
                }
            }
        }
    ]);
})();