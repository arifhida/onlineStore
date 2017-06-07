/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').directive('fileUpload', [
        '$parse', '$http', function ($parse, $http) {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    var model = $parse(attrs.fileUpload);
                    var url = attrs.fileUrl;
                    var modelSetter = model.assign;
                    element.bind('change', function () {
                        var form = new FormData();
                        //scope.$apply(function () {
                        //    modelSetter(scope, element[0].files[0]);
                        //});
                        form.append('file', element[0].files[0]);
                        $http.post(url, form, { headers: { 'Content-Type': undefined } }).then(function (response) {
                            //scope.profile = response.data;
                            modelSetter(scope, response.data);
                        });
                    });

                }
            }
        }
    ]).directive('showTabs', function () {
        return {
            link: function (scope, element, attrs) {
                element.click(function (e) {
                    e.preventDefault();
                    $(element).tab('show');
                });
            }
        };
    }).directive('fileMulti', ['$parse', '$http', function ($parse, $http) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var model = $parse(attrs.fileMulti);
                var url = attrs.fileUrl;
                var modelSetter = model.assign;
                element.bind('change', function () {
                    var data = model(scope);
                    var form = new FormData();
                    for (var i = 0; i < element[0].files.length; i++) {
                        form.append('files', element[0].files[i]);
                    }
                    $http.post(url, form, { headers: { 'Content-Type': undefined } }).then(function (response) {
                        if (data) {
                            for (var i = 0; i < response.data.length; i++) {
                                data.push(response.data[i]);
                            }
                            modelSetter(scope, data);
                        } else {
                            modelSetter(scope, response.data);
                        }

                    });
                });
            }
        }
    }]).factory('PagerService', function () {
        var service = {};
        service.GetPager = function (page, totalPage, pageSize) {
            var startPage, endPage;
            if (totalPage <= 10) {
                startPage = 1;
                endPage = totalPage;
            } else {
                if (page <= 6) {
                    startPage = 1;
                    endPage = 10;
                } else if (page + 4 >= totalPage) {
                    startPage = totalPage - 9;
                    endPage = totalPage;
                } else {
                    startPage = page - 5;
                    endPage = page + 4;
                }
            }
            var pages = [];
            for (var i = startPage; i < endPage; i++) {
                pages.push(i);
            }

            return {
                pages : pages
            }
        }
        return service;
    });
})();