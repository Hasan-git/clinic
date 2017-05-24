var inspiniaTemplate = 'views/common/notify.html';
angular
    .module('inspinia')
    .controller('consultationView', consultationView)
;

function consultationView(appSettings, $scope, resolvedData, consultationResource, toaster, patientResource) {
    $scope.consultation = resolvedData;
    $scope.idOfpatient = resolvedData.patientId;
    $scope.appSettings = appSettings

    patientResource.patient.get({ id: $scope.idOfpatient }).$promise.then(function (data) {
        $scope.patientHistory = data.medicalStatus;
        $scope.patientResolved = JSON.parse(angular.toJson(data))
        $scope.$watch('consultation.entryDate', function (newVal, oldVal) {
            var ageCalculated = moment(newVal).diff($scope.patientResolved.birthday, 'years', false);
            $scope.age = ageCalculated >= 0 ? ageCalculated : "Unknown";
        });
    })

    $scope.deleteImage = function (imgId, index) {
        consultationResource.consultations.deleteImage({ id: imgId }).$promise.then(function () {
            $scope.consultation.images.splice(index, 1)
            toaster.pop('success', "Notification", "Image deleted successfully", 4000);
        })
    }
}