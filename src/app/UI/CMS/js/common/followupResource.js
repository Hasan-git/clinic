(function () {
    "use strict";

    function followUpResource($resource, appSettings) {

        return $resource(appSettings.serverPath + "/api/FollowUp/:id", null,
            {
                'update': { method: 'PUT', url: appSettings.serverPath + '/api/FollowUp/' }
            });

    };

    angular
        .module("common.services")
        .factory("followUpResource", ["$resource", "appSettings", followUpResource]);
}());

