var inspiniaTemplate = 'views/common/notify.html';
angular
    .module('inspinia')
    .controller('consultationList', consultationList)
;

function consultationList($scope, resolvedData, $stateParams, toaster, patientResource, followUpResource, consultationResource, $modal, $state, $rootScope) {
    $scope.consultations = resolvedData;
    $scope.idOfpatient = $stateParams.patientid;

    console.log($scope.consultations)

    patientResource.patient.get({ id: $scope.idOfpatient }).$promise.then(function (response) {
        console.log(response)
        $scope.patient = response;
    })

    //alert(angular.toJson(patientdetails));
    //console.log(resolvedData);

    $scope.deleteConsultation = function (idx) {
        var modalInstance = $modal.open({
            animation: true,
            templateUrl: 'deleteConsultation.html',
            scope: $scope,
            size: 'lg',
        });

        $scope.yes = function () {
            consultationResource.consultations.deleteConsultation({ id: idx }).$promise.then(function (data) {
                consultationResource.factory.GetConsultations({ doctorId: $rootScope.rootDoctorId, patientId: $scope.idOfpatient }).$promise.then(function (response) {
                    $scope.consultations = JSON.parse(angular.toJson(response));
                    modalInstance.close();
                    toaster.pop('success', "Notification", "Consultation deleted successfully", 4000);

                })
                //$state.go('consultation.consultation_list', { patientid: $scope.idOfpatient });
            })
        };
        $scope.no = function () {
            modalInstance.dismiss('cancel');
        };
    }


    $scope.deleteFollowUp = function (idx) {
        var modalInstance = $modal.open({
            animation: true,
            templateUrl: 'deleteFollowUp.html',
            scope: $scope,
            size: 'lg',
        });

        $scope.yesDelete = function () {
            followUpResource.deleteFollowUp({ id: idx }).$promise.then(function (data) {
                consultationResource.factory.GetConsultations({ doctorId: $rootScope.rootDoctorId, patientId: $scope.idOfpatient }).$promise.then(function (response) {
                    $scope.consultations = JSON.parse(angular.toJson(response));
                    modalInstance.close();
                    toaster.pop('success', "Notification", "Consultation deleted successfully", 4000);

                })
                //$state.go('consultation.consultation_list', { patientid: $scope.idOfpatient });
            })
        };
        $scope.noDelete = function () {
            modalInstance.dismiss('cancel');
        };
    }


}