/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />


"use strict";
(function () {
    angular.module('app').controller('categoryController', function ($scope, $http, initialData) {
        $scope.data = angular.copy(initialData);
        $scope.loading = false;
        var nodeData = null;
        $scope.toggle = function (scope) {
            scope.toggle();
        };
        var defcat = {
            Id: 0,
            ParentId: 0,
            CategoryName: '',
            CategoryDescription: '',
            Children: []
        };
        $scope.addRoot = function () {
            nodeData = null;
            $scope.cat.ParentId = 0;
            var element = angular.element('#myModal');
            element.modal('show');
        }
        $scope.reset = function () {
            $scope.cat = angular.copy(defcat);
            $scope.frmCat.$setPristine();
        }
        $scope.cat = angular.copy(defcat);
        $scope.addItem = function (scope, parentId) {
            console.log($scope);
            nodeData = scope.$modelValue;
            $scope.cat.ParentId = parentId;
            var element = angular.element('#myModal');
            element.modal('show');
        }
        $scope.Save = function () {
            $http.post('http://localhost:58969/api/Category/AddCategory', $scope.cat, { headers: { 'Content-Type': 'application/json' } })
            .then(function (response) {
                if (nodeData) {
                    nodeData.Children.push(response.data);
                } else {
                    $scope.data.push(response.data);
                }
                var element = angular.element('#myModal');
                element.modal('hide');
                $scope.reset();
            });
        }
        $scope.del = function (scope, Id) {
            $http.delete('http://localhost:58969/api/Category/Delete?Id=' + Id, { headers: { 'Content-Type': 'application/json' } })
            .then(function (response) {
                scope.remove();
            });

        }

    });
})();