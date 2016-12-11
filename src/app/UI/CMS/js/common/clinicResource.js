(function () {
    "use strict";

    function clinicResource($resource, appSettings) {

        return {
            clinics: $resource(appSettings.serverPath + "/api/Clinics/:id", null,
            {
                'update': { method: 'PUT' },
                'clinics': {
                    //Get doctor's Clinics by doctor Id
                    method: 'GET',
                    isArray: false,
                    url: appSettings.serverPath + '/api/Clinics/Doctor/:id',
                    params: {
                        id: '@id'
                    }
                }
            }),
            doctor: $resource(appSettings.serverPath + "/api/Clinics/Doctor/:id", null,
            {
                'get': {
                    method: 'GET',
                    isArray: true
                },
                'query': { method: 'GET' }
                
            })
        }

    };

    angular
        .module("common.services")
        .factory("clinicResource", ["$resource", "appSettings", clinicResource]);
}());