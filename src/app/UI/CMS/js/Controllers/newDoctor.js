var inspiniaTemplate = 'views/common/notify.html';
angular
    .module('inspinia')
    .controller('newDoctor', newDoctor)
;


function newDoctor(doctorResource, toaster, notify) {
    var vm = this;
    vm.doctor = {};
    vm.inspiniaTemplate = 'views/common/notify.html';


    vm.doctor = new doctorResource;

    vm.doctor.birthday = new Date();
    vm.doctor.mobile = "";
    vm.doctor.phone = "";
    vm.doctor.type = "doctor";

    vm.doctor.expiryDate = new Date();
    vm.doctor.isExpired = false;

    //});

    vm.submit = function () {
        vm.loading = true;

        vm.doctor.$save(
            function (data) {
                vm.loading = false;
                vm.originalProduct = angular.copy(data);
                toaster.pop('success', "Notification", "Doctor created successfully", 4000);

            }, function (response) {
                vm.loading = false;
                if (response.data.modelState) {
                    vm.message = '';
                    for (var key in response.data.modelState) {
                        vm.message += '<p>' + response.data.modelState[key] + "</p>";
                    }
                    notify({ messageTemplate: vm.message, classes: 'alert-danger', templateUrl: vm.inspiniaTemplate, duration: '20000', position: 'left' });
                }

            });
    };//end submit func

    vm.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        vm.opened = true;
    };
    vm.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

}// end newdoctor controller