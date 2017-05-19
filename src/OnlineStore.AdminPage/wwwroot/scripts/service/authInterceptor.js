/// <reference path="../../lib/angular/angular.min.js" />
/// <reference path="../../lib/angular/angular.js" />

"use strict";
(function () {
    angular.module('app').factory('beforeSendRequest', [
        'localStorageService', '$q', '$injector', function (localStorageService, $q, $injector) {
            return {
                request: function (config) {
                    config.headers = config.headers || {};
                    var authData = localStorageService.get('authentication');
                    if (authData) {
                        config.headers.Authorization = 'Bearer ' + authData.token;
                    }
                    return config;
                },
                requestError: function (rejection) {                    
                    if (rejection.status == 401) {
                        var stateService = $injector.get('$state');
                        stateService.go('login');
                        return $q.reject(rejection);
                    }
                    return $q.reject(rejection);
                }
                ,responseError: function (rejection) {
                    if (rejection.status == 401) {                        
                        var stateService = $injector.get('$state');
                        stateService.go('login');                       
                    }
                    return $q.reject(rejection);
                }

            }
        }

    ]);
    angular.module('app').config(function ($httpProvider) {
        $httpProvider.interceptors.push('beforeSendRequest');
    });
})();