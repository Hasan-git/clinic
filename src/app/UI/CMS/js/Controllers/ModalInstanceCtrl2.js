var inspiniaTemplate = 'views/common/notify.html';
angular
    .module('inspinia')
    .controller('ModalInstanceCtrl2', ModalInstanceCtrl2)
;


function ModalInstanceCtrl2($scope, $modalInstance, $filter, statetype, patientRepo) {

    var response = [];
    $scope.patients = [];
    $scope.statetype = statetype === true ? true : false;
    $scope.selection = statetype == "selection" ? "select" : false;
    $scope.patients = patientRepo;



    $scope.onSelect = function ($item, $model, $label) {
        $scope.task.mobile = $item.mobile;
        $scope.task.patientId = $item.id;
        $scope.existingPatient = true;
        $scope.task.patientName = $item.displayName;

    };
    $scope.checked = function () {
        if ($scope.checkbox == true) {

            $scope.task = "";
            $scope.existingPatient = true;

        } else if ($scope.checkbox == false) {

            $scope.task = "";
            $scope.existingPatient = false;
        }
    };

    $scope.ok = function () {
        var thiss = this;

        //var d =$filter('date')(thiss.task.Time,'HH:mm');
        var hour = $filter('date')(thiss.task.tasktime, 'HH');
        var minute = $filter('date')(thiss.task.tasktime, 'mm');
        response.push({ patientName: this.task.patientName, hour: hour, minute: minute, duration: this.task.duration, existingPatient: thiss.existingPatient, patientId: thiss.task.patientId, mobile: thiss.task.mobile, description: thiss.task.description, clinicId: thiss.clinicId.value });
        $modalInstance.close(response);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
};