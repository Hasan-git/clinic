(function () {
    "use strict";

    function patientResource($resource, appSettings) {


        return {
            patient: $resource(appSettings.serverPath + "/api/Patients/:id", null,
            {
                'get': { method: 'GET'},
                'update': { method: 'PUT' }
            }),
            doctor: $resource(appSettings.serverPath + "/api/Patients/:id/Doctor", null,
            {
                'query': { method: 'GET' },
                'doctorPatient': {
                    //Get patient related to specific doctor by doctor Id
                    method: 'GET',
                    isArray: true
                }
            })
        }
        
    };

    angular
        .module("common.services")
        .factory("patientResource",["$resource","appSettings",patientResource]);
}());

//return {
//    default: $resource(appSettings.serverPath + "/api/Patients/:id", null,
//    {
//        'get': { method: 'GET',isArray: true },
//        'update': { method: 'PUT' }
//    }),
//    doctor: $resource(appSettings.serverPath + "/api/Patients/:id/Doctor", null,
//    {
//        //'doctor': {
//        //    method: 'GET',
//        //    headers: { 'Content-Type': 'application/json' }
//        //}
//        'query': { method: 'GET' },
//        'doctorz': {
//            method: 'GET',
//            isArray: true
//        }
//    })
//}









//return $resource(appSettings.serverPath + "/api/Patients/:id",null,
//            {
//                'update': { method: 'PUT' }

//            });