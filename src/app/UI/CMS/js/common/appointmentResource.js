(function () {
    "use strict";

    function appointmentResource($resource, appSettings) {

        return {
            appointments: $resource(appSettings.serverPath + "/api/Appointments/:id", null,
            {
                'get': { method: 'GET', isArray: false},
                'update': { method: 'PUT' }
            }),
            doctor: $resource(appSettings.serverPath + "/api/Appointments/:id/Doctor", null,
            {
                'query': { method: 'GET' },
                'doctorAppointment': {
                    method: 'GET',
                    isArray: true
                }
            })
        }

    };

    angular
        .module("common.services")
        .factory("appointmentResource",["$resource","appSettings",appointmentResource]);
}());

//return $resource(appSettings.serverPath + "/api/Appointments/:id", null,
//            {
//                'update': { method: 'PUT' }
//            });