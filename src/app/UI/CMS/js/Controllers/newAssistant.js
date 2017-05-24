var inspiniaTemplate = 'views/common/notify.html';
angular
    .module('inspinia')
    .controller('newAssistant', newAssistant)
;

function newAssistant(assistantResource, toaster, notify) {
    var vm = this;
    vm.assistant = {};
    vm.inspiniaTemplate = 'views/common/notify.html';


    vm.assistant = new assistantResource;

    vm.assistant.birthday = new Date();
    vm.assistant.mobile = "";
    vm.assistant.phone = "";
    vm.assistant.type = "assistant";

    vm.assistant.expiryDate = new Date();
    vm.assistant.isExpired = false;

    //});

    vm.submit = function () {
        vm.loading = true;
        vm.assistant.$save(
            function (data) {
                vm.loading = false;
                vm.originalProduct = angular.copy(data);
                toaster.pop('success', "Notification", "Assistant created successfully", 4000);

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

}// end newpatient controller