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
            }),
            uploadTest: $resource(appSettings.serverPath + "/api/uploadTest", { id: "@id" },
            {
                'upload': {
                    method: 'POST',
                    transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                },
                
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