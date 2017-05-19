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
    });
})();