/// <reference path="../../../lib-npm/angular/angular.min.js" />
/// <reference path="../../../lib-npm/angular/angular.js" />

"use strict";
(function () {
    angular.module('app').controller('cartCtrl', [
        '$rootScope', '$scope', '$state', function ($rootScope, $scope, $state) {
            $scope.delete = function (item) {
                $rootScope.cart.splice($rootScope.cart.indexOf(item), 1);
                if (localStorage.getItem('cart')) {
                    localStorage.removeItem('cart');
                }
                localStorage.setItem('cart', JSON.stringify($rootScope.cart));
                $scope.getSubTotal();
            };

            $scope.addQty = function (item) {
                $rootScope.cart[$rootScope.cart.indexOf(item)].Quantity += 1;
                if (localStorage.getItem('cart')) {
                    localStorage.removeItem('cart');
                }
                localStorage.setItem('cart', JSON.stringify($rootScope.cart));
                $scope.getSubTotal();
            };
            $scope.SubTotal = 0;
            $scope.Tax = 0;
            $scope.getSubTotal = function () {
                $scope.SubTotal = 0;
                for (var i = 0; i < $rootScope.cart.length; i++) {
                    $scope.SubTotal += ($rootScope.cart[i].Price * $rootScope.cart[i].Quantity);
                }
                $scope.Tax = $scope.SubTotal * 0.1;
                $scope.Total = $scope.SubTotal + $scope.Tax;
            };
            $scope.Total = 0;
            $scope.getSubTotal();
            $scope.substractQty = function (item) {
                $rootScope.cart[$rootScope.cart.indexOf(item)].Quantity = ($rootScope.cart[$rootScope.cart.indexOf(item)].Quantity > 1) ? ($rootScope.cart[$rootScope.cart.indexOf(item)].Quantity - 1) : 1;
                if (localStorage.getItem('cart')) {
                    localStorage.removeItem('cart');
                }
                localStorage.setItem('cart', JSON.stringify($rootScope.cart));
                $scope.getSubTotal();
            }
        }
    ]);
})();