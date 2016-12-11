(function () {
    "use strict";

    function assistantResource($resource, appSettings) {

        return $resource(appSettings.serverPath + "/api/Assistants/:id", null,
            {
                'update': { method: 'PUT' }
            });

    };

    angular
        .module("common.services")
        .factory("assistantResource", ["$resource", "appSettings", assistantResource]);
}());

