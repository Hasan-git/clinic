(function () {
    "use strict";

    function doctorResource($resource, appSettings) {

        return $resource(appSettings.serverPath + "/api/Doctors/:id", null,
            {
                'update': { method: 'PUT' }
            });

    };

    angular
        .module("common.services")
        .factory("doctorResource",["$resource","appSettings",doctorResource]);
}());

