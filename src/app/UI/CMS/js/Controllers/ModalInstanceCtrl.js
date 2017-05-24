var inspiniaTemplate = 'views/common/notify.html';
angular
    .module('inspinia')
    .controller('ModalInstanceCtrl', ModalInstanceCtrl)
;


function ModalInstanceCtrl($scope, $modalInstance) {

    $scope.ok = function () {
        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
};