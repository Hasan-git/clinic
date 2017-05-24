var inspiniaTemplate = 'views/common/notify.html';
angular
    .module('inspinia')
    .controller('editPatient', editPatient)
;

function editPatient(patientResource, toaster, notify, patientData, $modal, patientResource, $scope, $state) {
    $scope.male = false;
    $scope.female = false;


    $scope.patient = {};
    $scope.inspiniaTemplate = 'views/common/notify.html';
    console.log(patientData);

    if (patientData) {
        $scope.patient = patientData;
        $scope.patient.birthday = $scope.patient.birthday == "" ? "" : $scope.patient.birthday;
        $scope.patient.mobile = $scope.patient.mobile == "" ? "" : $scope.patient.mobile;
        $scope.patient.phone = $scope.patient.phone == "" ? "" : $scope.patient.phone;
        $scope.male = $scope.patient.gender == "male" ? true : false;
        $scope.female = $scope.patient.gender == "female" ? true : false;
        $scope.originalPatient = angular.copy(patientData);
    }
    $scope.submit = function () {
        $scope.loading = true;
        //$scope.patient.$update({ id: $scope.patient.patientId }).$promise.then(function (data) { alert("done"); $scope.loading = false;}, function (error) { });
        //alert(angular.toJson($scope.patient));
        patientResource.patient.update($scope.patient).$promise.then(function (data) {
            $scope.loading = false;
            toaster.pop('success', "Notification", "Patient updated successfully", 4000);
            $state.go('patient.patients_list');
        }, function (error) {
            $scope.loading = false;
            if (response.data.modelState) {
                $scope.message = '';
                for (var key in response.data.modelState) {
                    $scope.message += '<p>' + response.data.modelState[key] + "</p>";
                }
                notify({ messageTemplate: $scope.message, classes: 'alert-danger', templateUrl: inspiniaTemplate, duration: '4000', position: 'left' });
            }
        });

        //$scope.patient.$update({ id: $scope.patient.patientId },
        //            function (data) {
        //                $scope.loading = false;
        //                toaster.pop('success', "Notification", "Patient updated successfully", 4000);
        //            }, function (response) {

        //                $scope.loading = false;
        //                if (response.data.modelState) {
        //                    $scope.message = '';
        //                    for (var key in response.data.modelState) {
        //                        $scope.message += '<p>' + response.data.modelState[key] + "</p>";
        //                    }
        //                    notify({ messageTemplate: $scope.message, classes: 'alert-danger', templateUrl: $scope.inspiniaTemplate, duration: '4000', position: 'left' });
        //                }
        //            });
    }
    $scope.cancel = function (editForm) {
        editForm.$setPristine();
        $scope.patient = angular.copy($scope.originalPatient);
    };


}