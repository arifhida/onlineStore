/// <reference path="../../lib-npm/angular/angular.min.js" />
/// <reference path="../../lib-npm/angular/angular.js" />
"use strict";
(function () {
    angular.module('app').filter('jsonDate', ['$filter', function ($filter) {
        return function (input, format) {
            //return (input) ? $filter('date')(parseInt(input.substr(6)), format) : '';
            return (input) ? $filter('date')(input, format) : '';
        };
    }]).filter('Gender', function () {
        return function (input) {
            var Gender = (input && input == 1) ? 'Male' : 'Female';
            return Gender;
        };
    }).filter('stringToDate', function () {
        return function (input) {
            if (!input)
                return null;

            var date = moment(input);
            return date.isValid() ? date.toDate() : null;
        };
    }).directive('jsonToDate', function ($filter) {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {

                //format text going to user (model to view)
                ngModel.$formatters.push(function (value) {
                    console.log(value);
                    var date = $filter('stringToDate')(value);
                    return date.toString();
                });

                //format text from the user (view to model)
                ngModel.$parsers.push(function (value) {
                    var date = new Date(value);
                    if (!isNaN(date.getTime())) {
                        return moment(date).format();
                    }
                });
            }
        }
    }).filter('Confirmed', function () {
        return function (input) {
            var Confirmed = (input && input == true) ? 'Confirmed' : 'Pending';
            return Confirmed;
        }
    }).directive('fileUpload', [
        '$parse', '$http', function ($parse, $http) {
            return {
                restrict: 'A',
                link: function (scope, element, attrs) {
                    var model = $parse(attrs.fileUpload);
                    var url = attrs.fileUrl;
                    var modelSetter = model.assign;
                    element.bind('change', function () {
                        var form = new FormData();                        
                        form.append('file', element[0].files[0]);
                        $http.post(url, form, { headers: { 'Content-Type': undefined } }).then(function (response) {                            
                            modelSetter(scope, response.data);
                        });
                    });

                }
            }
        }
    ]);
})();