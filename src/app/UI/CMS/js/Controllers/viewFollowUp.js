var inspiniaTemplate = 'views/common/notify.html';
angular
    .module('inspinia')
    .controller('viewFollowUp', viewFollowUp)
;

function viewFollowUp(appSettings, $scope, resolvedData, $stateParams, consultationResource, toaster, patientResource) {
    $scope.followup = resolvedData;
    $scope.appSettings = appSettings
    console.log(resolvedData)
    $scope.idOfpatient = $stateParams.patientid;
    patientResource.patient.get({ id: $scope.idOfpatient }).$promise.then(function (data) {
        $scope.patientHistory = data.medicalStatus;
        $scope.patientResolved = JSON.parse(angular.toJson(data))
        $scope.$watch('followUp.entryDate', function (newVal, oldVal) {
            var ageCalculated = moment(newVal).diff($scope.patientResolved.birthday, 'years', false);
            $scope.age = ageCalculated >= 0 ? ageCalculated : "Unknown";
        });
    })
    consultationResource.consultations.Consultation({ consultationId: $scope.followup.consultationId }).$promise.then(function (data) {
        $scope.parentConsultation = JSON.parse(angular.toJson(data))
        console.log("Parent Consultation >", $scope.parentConsultation)
    });
    $scope.deleteImage = function (imgId, index) {
        consultationResource.consultations.deleteImage({ id: imgId }).$promise.then(function () {
            $scope.followup.images.splice(index, 1)
            toaster.pop('success', "Notification", "Image deleted successfully", 4000);
        })
    }
}