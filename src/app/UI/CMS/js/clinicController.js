var inspiniaTemplate = 'views/common/notify.html';


//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        newDoctor         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +
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

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        newAssistant         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +


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

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        newPatient         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function newPatient(patientResource, toaster, notify, currentUser,$rootScope,$state) {
    var vm = this;
    vm.patient = {};
    vm.patient = new patientResource.patient;
    vm.inspiniaTemplate = 'views/common/notify.html';

    vm.patient.birthday = new Date();

    vm.submit = function () {
        vm.loading = true;
        vm.patient.doctorId = $rootScope.rootDoctorId;
        vm.patient.clinicId = "0aa75235-15d1-11e6-9663-005056c00111";
        vm.patient.entryDate = new Date();

        vm.patient.$save(
            function (data) {
                vm.loading = false;
                //TODO : handle Id from data and redirect to patient's consultations list
                var patientId = JSON.parse(angular.toJson(data)).patientId
                $state.go('consultation.consultation_list', { patientid: patientId });
                toaster.pop('success', "Notification", "Patient created successfully", 4000);
            }, function (response) {
                vm.loading = false;
                if (response.data.modelState) {
                    vm.message = '';
                    for (var key in response.data.modelState) {
                        vm.message += '<p>' + response.data.modelState[key] + "</p>";
                    }
                    notify({ messageTemplate: vm.message, classes: 'alert-danger', templateUrl: vm.inspiniaTemplate, duration: '4000', position: 'left' });
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

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        patientList         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function patientList(patientResource, DTOptionsBuilder, DTColumnBuilder, resolvedData) {
    var patlist = this;
    patlist.patients = {};
    patlist.patients = resolvedData;

    patlist.dtOptions = DTOptionsBuilder.newOptions()
        .withDOM('<"html5buttons"B>lTfgtp<"bottom"i<"clear">>')
        .withButtons([
            { extend: 'copy', title: 'Patient List', filename: "Patients", exportOptions: { columns: [0, 1, 2, 3, 4] } },
            { extend: 'csv', title: 'Patient List', filename: "Patients", exportOptions: { columns: [0, 1, 2, 3, 4] } },
            { extend: 'excel', title: 'Patient List', filename: "Patients", exportOptions: { columns: [0, 1, 2, 3, 4] } },
            { extend: 'pdf', title: 'Patient List', filename: "Patients", exportOptions: { columns: [0, 1, 2, 3, 4] } },
            {
                extend: 'print',

                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');
                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                    $(win.document.body).find("tr").each(function () {
                        $(this).find('td:last').css('display', 'none').css('visibility', 'hidden').remove();
                        $(this).find('th:last').css('display', 'none').css('visibility', 'hidden').remove();
                    });
                }
            }
        ]);

    //patlist.submit = function (dataz) {

    //    var data = new FormData(dataz);
    //    jQuery.each(jQuery('#file')[0].files, function (i, file) {
    //        data.append('image', file);
    //    });
    //    angular.forEach(dataz, function (value, key) {
    //        data.append(key, data[key]);
    //    });
    //    jQuery.ajax({
    //        url: 'http://localhost:63392/api/uploadTest',
    //        data: data,
    //        cache: false,
    //        contentType: false,
    //        processData: false,
    //        type: 'POST',
    //        success: function (data) {
    //            alert(data);
    //        }
    //    });


    //};


}// End patientList

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        newConsultation         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function newConsultation($scope, $stateParams, consultationResource, $rootScope, notify, toaster, $state,FileUploader) {

    $scope.idOfpatient = $stateParams.patientid;

    //----------------------------
    //      Uploader Settings
    //----------------------------
    var uploader = $scope.uploader = new FileUploader({
        // url: 'upload.php'
        url: 'http://localhost:63392/api/consultations/uploadTest',
        formData: null,
    });

    // FILTERS
    uploader.filters.push({
        name: 'imageFilter',
        fn: function(item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });
    uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
        toaster.pop('error', "Notification", "Invalid file type !", 40000);
    };
    //----------------------------
    //      Uploader Settings
    //----------------------------

    $scope.consultation = new consultationResource.consultations;
    $scope.consultation.patientId = $stateParams.patientid;
    $scope.submit = function () {
        $scope.loading = true;
        //Get clinicId from Dropdown list 
        $scope.consultation.clinicId = this.clinicId.value;
        $scope.consultation.doctorId = $rootScope.rootDoctorId;
        $scope.consultation.$save(
            function (data) {
                var response = JSON.parse(angular.toJson(data))
                
                if (uploader.queue.length) {
                    uploader.onBeforeUploadItem = function (item) {
                        item.formData = [{ consultationId: response.consultationId }];
                    };
                    $scope.uploader.uploadAll(); // Uncomment to upload

                    uploader.onCompleteAll = function (item) {
                        console.log(item)
                        $scope.loading = false;
                        $state.go('consultation.consultation_list', { patientid: $scope.idOfpatient });
                        toaster.pop('success', "Notification", "Consultation created successfully", 4000);

                    }
                } else if (!uploader.queue.length) {
                    $scope.loading = false;
                    $state.go('consultation.consultation_list', { patientid: $scope.idOfpatient });
                    toaster.pop('success', "Notification", "Consultation created successfully", 4000);
                }
                
            }, function (response) {
                $scope.loading = false;
                if (response.data.modelState) {
                    $scope.message = '';
                    for (var key in response.data.modelState) {
                        $scope.message += '<p>' + response.data.modelState[key] + "</p>";
                    }
                    notify({ messageTemplate: $scope.message, classes: 'alert-danger', templateUrl: inspiniaTemplate, duration: '4000', position: 'left' });
                }
            });//end Save
    };//end submit func



    $scope.SliderCost = {
        grid: true,
        min: 0,
        max: 200,
        step: 1,
        postfix: " $",
        from: 0,
        onFinish: function (data) { $scope.consultation.cost = data.fromNumber }
        // onChange: function (data) { console.log(data); }
    };
    $scope.SliderPaid = {
        grid: true,
        min: 0,
        max: 200,
        step: 1,
        postfix: " $",
        from: 0,
        onFinish: function (data) { $scope.consultation.paid = data.fromNumber }
        // onChange: function (data) { console.log(data); }
    };

}

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        consultationList         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function consultationList($scope, resolvedData,$stateParams) {
    $scope.consultations = resolvedData;
    $scope.idOfpatient = $stateParams.patientid;
    //alert(angular.toJson(patientdetails));
    //console.log(resolvedData);
}

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        consultationView         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +


function consultationView($scope, resolvedData, consultationResource, toaster) {
    $scope.consultation = resolvedData;
    $scope.idOfpatient = resolvedData.patientId;
    $scope.deleteImage = function (imgId, index) {
        consultationResource.consultations.deleteImage({ id: imgId }).$promise.then(function () {
            $scope.consultation.images.splice(index, 1)
            toaster.pop('success', "Notification", "Image deleted successfully", 4000);
        })
    }
}

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        editConsultation         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function editConsultation($scope, $stateParams, resolvedData, consultationResource, toaster, notify, $state, FileUploader) {

    $scope.consultation = {};
    $scope.consultation = resolvedData;
    this.clinicId = $scope.consultation.clinicId;
    console.log($scope.consultation)
    //----------------------------
    //      Uploader Settings
    //----------------------------
    var uploader = $scope.uploader = new FileUploader({
        // url: 'upload.php'
        url: 'http://localhost:63392/api/consultations/uploadTest',
        formData: null,
    });

    // FILTERS
    uploader.filters.push({
        name: 'imageFilter',
        fn: function (item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });
    uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
        toaster.pop('error', "Notification", "Invalid file type !", 40000);
    };
    //----------------------------
    //      Uploader Settings
    //----------------------------

    if (resolvedData) {
        $scope.originalconsultation = angular.copy(resolvedData);
    }
    $scope.submit = function() {
        $scope.loading = true;
        $scope.consultation.clinicId  = this.clinicId.value;
        consultationResource.consultations.update($scope.consultation).$promise.then(function (data) {

            if (uploader.queue.length) {
                uploader.onBeforeUploadItem = function (item) {
                    
                    item.formData = [{ consultationId: $scope.consultation.id }];
                };
                $scope.uploader.uploadAll(); // Uncomment to upload

                uploader.onCompleteAll = function (item) {
                    $scope.loading = false;
                    $state.go('consultation.consultation_list', { patientid: $scope.consultation.patientId });
                    toaster.pop('success', "Notification", "Consultation updated successfully", 4000);

                }
            } else if (!uploader.queue.length) {
                $scope.loading = false;
                $state.go('consultation.consultation_list', { patientid: $scope.consultation.patientId });
                toaster.pop('success', "Notification", "Consultation updated successfully", 4000);
            }


            $scope.loading = false;
            $state.go('consultation.consultation_list', { patientid: $scope.consultation.patientId });
            toaster.pop('success', "Notification", "Consultation updated successfully", 4000);

        }, function (error) {
            $scope.loading = false;
                if (response.data.modelState) {
                    $scope.message = '';
                    for (var key in response.data.modelState) {
                        $scope.message += '<p>' + response.data.modelState[key] + "</p>";
                    }
                    notify({ messageTemplate: $scope.message, classes: 'alert-danger', templateUrl: $scope.inspiniaTemplate, duration: '4000', position: 'left' });
                }
        });
        
    };
    $scope.cancel = function (editForm) {
        editForm.$setPristine();
        $scope.consultation = angular.copy($scope.originalconsultation);
    };

    $scope.deleteImage = function (imgId,index) {
        consultationResource.consultations.deleteImage({ id: imgId }).$promise.then(function () {
            $scope.consultation.images.splice(index, 1)
            toaster.pop('success', "Notification", "Image deleted successfully", 4000);
        })
    }

    $scope.SliderCost = {
             grid: true,
            min: 0,
            max: 200,
            step: 1,
            postfix: " $",
            from: $scope.consultation.cost,
            onFinish: function (data) {  $scope.consultation.cost = data.fromNumber }
           // onChange: function (data) { console.log(data); }
    };

    $scope.SliderPaid = {
        grid: true,
        min: 0,
        max: 200,
        step: 1,
        postfix: " $",
        from: $scope.consultation.paid,
        onFinish: function (data) {$scope.consultation.paid = data.fromNumber }
        // onChange: function (data) { console.log(data); }
    };
};

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        newFollowUp         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function newFollowUp($scope, $stateParams, followUpResource, $rootScope, toaster, $state, FileUploader) {

   

    // idOfpatient used to inject in header menu
    $scope.idOfpatient = $stateParams.patientid;

    $scope.inspiniaTemplate = 'views/common/notify.html';
    
    $scope.followUp = new followUpResource;

    //----------------------------
    //      Uploader Settings
    //----------------------------
    var uploader = $scope.uploader = new FileUploader({
        // url: 'upload.php'
        url: 'http://localhost:63392/api/followup/uploadTest',
        formData: null,
    });

    // FILTERS
    uploader.filters.push({
        name: 'imageFilter',
        fn: function (item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });
    uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
        toaster.pop('error', "Notification", "Invalid file type !", 40000);
    };
    //----------------------------
    //      Uploader Settings
    //----------------------------

    //$scope.followUp = {
    //    "systolic": 99,
    //    "diastolic": 99,
    //    "heartRate": 99,
    //    "temprature": 33
    //};

    $scope.submit = function () {
        $scope.loading = true;
        console.log($scope.followUp);
        //follow up related to clinic and consultation
        $scope.followUp.consultationId = $stateParams.consultationId;
        
        //Get clinicId from Dropdown list 
        $scope.followUp.clinicId = this.clinicId.value;
        $scope.followUp.$save(
            function (data) {
                var response = JSON.parse(angular.toJson(data))

                if (uploader.queue.length) {
                    uploader.onBeforeUploadItem = function (item) {
                        item.formData = [{ followupId: response.followUpId }];
                    };
                    $scope.uploader.uploadAll(); // Uncomment to upload

                    uploader.onCompleteAll = function (item) {
                        console.log(item)
                        $scope.loading = false;
                        $state.go('consultation.consultation_list', { patientid: $scope.idOfpatient });
                        toaster.pop('success', "Notification", "Follow up created successfully", 4000);

                    }
                } else if (!uploader.queue.length) {
                    $scope.loading = false;
                    $state.go('consultation.consultation_list', { patientid: $scope.idOfpatient });
                    toaster.pop('success', "Notification", "Follow up created successfully", 4000);
                }

            }, function (response) {
                $scope.loading = false;
                if (response.data.modelState) {
                    $scope.message = '';
                    for (var key in response.data.modelState) {
                        $scope.message += '<p>' + response.data.modelState[key] + "</p>";
                    }
                    notify({ messageTemplate: $scope.message, classes: 'alert-danger', templateUrl: $scope.inspiniaTemplate, duration: '4000', position: 'left' });
                }
            });//end Save
    };//end submit func

    
   
    $scope.SliderCost = {
        grid: true,
        min: 0,
        max: 200,
        step: 1,
        postfix: " $",
        from: 0,
        onFinish: function (data) { $scope.followUp.cost = data.fromNumber }
        // onChange: function (data) { console.log(data); }
    };
    $scope.SliderPaid = {
        grid: true,
        min: 0,
        max: 200,
        step: 1,
        postfix: " $",
        from: 0,
        onFinish: function (data) { $scope.followUp.paid = data.fromNumber }
        // onChange: function (data) { console.log(data); }
    };
}

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        editFollowUp         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function editFollowUp($scope, $stateParams, resolvedData, followUpResource, toaster, notify, $state, FileUploader, consultationResource) {

    $scope.followUp = {};
    $scope.followUp = resolvedData;
    $scope.idOfpatient = $stateParams.patientid;
    this.clinicId = $scope.followUp.clinicId ;

    if (resolvedData) {
        $scope.originalfollowUp = angular.copy(resolvedData);
    }
    //----------------------------
    //      Uploader Settings
    //----------------------------
    var uploader = $scope.uploader = new FileUploader({
        // url: 'upload.php'
        url: 'http://localhost:63392/api/followup/uploadTest',
        formData: null,
    });

    // FILTERS
    uploader.filters.push({
        name: 'imageFilter',
        fn: function (item /*{File|FileLikeObject}*/, options) {
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    });
    uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
        console.info('onWhenAddingFileFailed', item, filter, options);
        toaster.pop('error', "Notification", "Invalid file type !", 40000);
    };
    //----------------------------
    //      Uploader Settings
    //----------------------------


    $scope.submit = function () {
        console.log($scope.followUp);
        $scope.loading = true;
        $scope.followUp.clinicId = this.clinicId.value;
        followUpResource.update($scope.followUp).$promise.then(function (data) {

            if (uploader.queue.length) {
                uploader.onBeforeUploadItem = function (item) {

                    item.formData = [{ followupId: $scope.followUp.id }];
                };
                $scope.uploader.uploadAll(); // Uncomment to upload

                uploader.onCompleteAll = function (item) {
                    $scope.loading = false;
                    $state.go('consultation.consultation_list', { patientid: $scope.followUp.patientId });
                    toaster.pop('success', "Notification", "Follow up updated successfully", 4000);

                }
            } else if (!uploader.queue.length) {
                $scope.loading = false;
                $state.go('consultation.consultation_list', { patientid: $scope.followUp.patientId });
                toaster.pop('success', "Notification", "Follow up updated successfully", 4000);
            }


            $scope.loading = false;
            $state.go('consultation.consultation_list', { patientid: $scope.idOfpatient });
            toaster.pop('success', "Notification", "FollowUp updated successfully", 4000);

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

    };
    $scope.cancel = function (editForm) {
        editForm.$setPristine();
        $scope.followUp = angular.copy($scope.originalfollowUp);
    };

    $scope.deleteImage = function (imgId, index) {
        consultationResource.consultations.deleteImage({ id: imgId }).$promise.then(function () {
            $scope.followUp.images.splice(index, 1)
            toaster.pop('success', "Notification", "Image deleted successfully", 4000);
        })
    }

    $scope.SliderCost = {
        grid: true,
        min: 0,
        max: 200,
        step: 1,
        postfix: "$",
        from: $scope.followUp.cost = $scope.followUp.cost == null ? 0 : $scope.followUp.cost,
        onFinish: function (data) { $scope.followUp.cost = data.fromNumber }
        // onChange: function (data) { console.log(data); }
    };
    $scope.SliderPaid = {
        grid: true,
        min: 0,
        max: 200,
        step: 1,
        postfix: " $",
        from: $scope.followUp.cost = $scope.followUp.cost == null ? 0 : $scope.followUp.cost,
        onFinish: function (data) { $scope.followUp.paid = data.fromNumber }
        // onChange: function (data) { console.log(data); }
    };
};

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        viewFollowUp         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +


function viewFollowUp($scope, resolvedData, $stateParams, consultationResource, toaster) {
    $scope.followup = resolvedData;
    $scope.idOfpatient = $stateParams.patientid;
    $scope.deleteImage = function (imgId, index) {
        consultationResource.consultations.deleteImage({ id: imgId }).$promise.then(function () {
            $scope.followup.images.splice(index, 1)
            toaster.pop('success', "Notification", "Image deleted successfully", 4000);
        })
    }
}


//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        viewPatient         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function viewPatient($stateParams, patientdetails) {
    //alert(angular.toJson(patientdetails));
    var patdetails = this;

    patdetails.patientdetails = patientdetails;
}

//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        editPatient         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function editPatient(patientResource, toaster, notify, patientData) {
    var vm = this;
    vm.male = false;
    vm.female = false;


    vm.patient = {};
    vm.inspiniaTemplate = 'views/common/notify.html';

    if (patientData) {
        vm.patient = patientData;
        vm.patient.birthday = vm.patient.birthday == "" ? "" : vm.patient.birthday;
        vm.patient.mobile = vm.patient.mobile == "" ? "" : vm.patient.mobile;
        vm.patient.phone = vm.patient.phone == "" ? "" : vm.patient.phone;
        vm.male = vm.patient.gender == "male" ? true : false;
        vm.female = vm.patient.gender == "female" ? true : false;
        vm.originalPatient = angular.copy(patientData);
    }

    

    vm.submit = function () {
        vm.loading = true;
        //vm.patient.$update({ id: vm.patient.patientId }).$promise.then(function (data) { alert("done"); vm.loading = false;}, function (error) { });
        alert(angular.toJson(vm.patient));
        patientResource.patient.update(vm.patient).$promise.then(function (data) {
            vm.loading = false;
            toaster.pop('success', "Notification", "patient updated successfully", 4000);

        }, function (error) {
            vm.loading = false;
            if (response.data.modelState) {
                $scope.message = '';
                for (var key in response.data.modelState) {
                    $scope.message += '<p>' + response.data.modelState[key] + "</p>";
                }
                notify({ messageTemplate: $scope.message, classes: 'alert-danger', templateUrl: inspiniaTemplate, duration: '4000', position: 'left' });
            }
        });

        //vm.patient.$update({ id: vm.patient.patientId },
        //            function (data) {
        //                vm.loading = false;
        //                toaster.pop('success', "Notification", "Patient updated successfully", 4000);
        //            }, function (response) {

        //                vm.loading = false;
        //                if (response.data.modelState) {
        //                    vm.message = '';
        //                    for (var key in response.data.modelState) {
        //                        vm.message += '<p>' + response.data.modelState[key] + "</p>";
        //                    }
        //                    notify({ messageTemplate: vm.message, classes: 'alert-danger', templateUrl: vm.inspiniaTemplate, duration: '4000', position: 'left' });
        //                }
        //            });
    }
    vm.cancel = function (editForm) {
        editForm.$setPristine();
        vm.patient = angular.copy(vm.originalPatient);

    };
}



//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        modalDemoCtrl         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function modalDemoCtrl($scope, $modal) {


    $scope.addNote = function () {

        var modalInstance = $modal.open({
            templateUrl: 'views/modal/add_note.html',
            controller: ModalInstanceCtrl
        });
    };
    $scope.editNote = function () {

        var modalInstance = $modal.open({
            templateUrl: 'views/modal/edit_note.html',
            controller: ModalInstanceCtrl
        });
    };
    $scope.open = function () {

        var modalInstance = $modal.open({
            templateUrl: 'views/modal_example.html',
            controller: ModalInstanceCtrl
        });
    };

    $scope.open1 = function () {
        var modalInstance = $modal.open({
            templateUrl: 'views/modal_example1.html',
            controller: ModalInstanceCtrl
        });
    };

    $scope.open2 = function () {
        var modalInstance = $modal.open({
            templateUrl: 'views/modal_example2.html',
            controller: ModalInstanceCtrl,
            windowClass: "animated fadeIn"
        });
    };

    $scope.open3 = function (size) {
        var modalInstance = $modal.open({
            templateUrl: 'views/modal_example3.html',
            size: size,
            controller: ModalInstanceCtrl
        });
    };

    $scope.open4 = function () {
        var modalInstance = $modal.open({
            templateUrl: 'views/modal_example2.html',
            controller: ModalInstanceCtrl,
            windowClass: "animated flipInY"
        });
    };
    $scope.mission = function () {

        var modalInstance = $modal.open({
            templateUrl: 'views/modal/mission.html',
            controller: ModalInstanceCtrl
        });
    };

    $scope.openModal = function (state) {

        var modalInstance = $modal.open({
            templateUrl: 'views/modal/add_task.html',
            controller: ModalInstanceCtrl2,
            resolve: {
                statetype: function () {
                    return state;
                },
                patientRepo: function (patientsData) {
                    var patients = [];
                    return patientsData.setProfiles().then(function (data) {
                        return patients = patientsData.getProfiles();
                    });
                }
            }
        });
        modalInstance.result.then(function (Result) {

        }, function () {
            //$log.info('Modal dismissed at: ' + new Date());
        });
        return modalInstance;
    };
};

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

function ModalInstanceCtrl($scope, $modalInstance) {

    $scope.ok = function () {
        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
};


//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        CalendarCtrl         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function CalendarCtrl($scope, $filter, appointmentResource, uiCalendarConfig, $compile, $templateCache, patientsData, toaster,$rootScope) {

    var eventsCall = [];
    $scope.events = [];
    $scope.patients = [];

    $scope.calendarLoading = function (loading) {
        if (loading == true) {
            $('.spinner').fadeIn(1500);
        } else {
            $('.spinner').fadeOut(1500);
        }
    };

    patientsData.setProfiles().then(function (data) {
        $scope.patients = patientsData.getProfiles();
    });

    $scope.calendarLoading(true);
    appointmentResource.appointments.query().$promise.then(function (data) {

            angular.forEach(data, function (value, key) {
                eventsCall.push({
                    id: value.id,
                    title: value.title,
                    start: value.start,
                    end: value.end,
                    allDay: value.allDay,
                    added: value.added,
                    patientName: value.patientName,
                    patientId: value.patientId,
                    mobile: value.mobile,
                    existingPatient: value.existingPatient,
                    description: value.description
                });
                $scope.calendarLoading(false);
            });
            $scope.calendarLoading(false);
    }, function () {
        $scope.calendarLoading(false);
        toaster.pop('warning', "Notification", "An error occured", 4000);
    });
    $scope.events = eventsCall;

    /* message on eventClick */
    $scope.alertOnEventClick = function (event, allDay, jsEvent, view) {
    };

    /* message on Drop */
    $scope.alertOnDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
    };

    $scope.toDate = function (thisDate) {
        var response = [];
        var day = $filter('date')(thisDate, 'dd');
        var month = $filter('date')(thisDate, 'MM');
        var year = $filter('date')(thisDate, 'yyyy');
        response.push({ day: day, month: month, year: year });
        return response;
    };

    $scope.externalDrop = function (date, jsEvent, ui, resourceId) {

        // state used to dismis time field of appointment in popup modal , if month dismis time field
        var state = resourceId.type === "month" ? true : false;

        $scope.openModal(state).result
        .then(function (result) {
            $scope.calendarLoading(true);
            var elementClass = jsEvent.target.textContent;
            var assignedClass;
            if (elementClass == "New Consultation") {
                assignedClass = "new";
            } else if (elementClass == "Follow Up") {
                assignedClass = "followup";
            }
            var thisdate = new Date(date);
            var d1 = thisdate.getDate();
            var m1 = thisdate.getMonth();
            var y1 = thisdate.getFullYear();
            //console.log(result);
            // alert(angular.toJson(result));
            var starthour = resourceId.type == "month" ? result[0].hour : thisdate.getHours();
            var startminute = resourceId.type == "month" ? result[0].minute : thisdate.getMinutes();
            //start: new Date(y1, m1, d1, starthour, startminute).add(4,'days'),
            //end: new Date(y1, m1, d1, starthour, startminute + result[0].duration)
            $scope.newAppointment = {};
            //////////////
            $scope.newAppointment = new appointmentResource.appointments;
            //////////////

            // creating appointment -> Post 
            // $scope.sendPost = function () {
            $scope.newAppointment.doctorId = $rootScope.rootDoctorId;
            $scope.newAppointment.title = result[0].patientName;
            $scope.newAppointment.patientName = result[0].patientName;
            $scope.newAppointment.description = result[0].description;
            $scope.newAppointment.allDay = false,
            $scope.newAppointment.start = moment({ year: y1, month: m1, day: d1, hour: starthour, minute: startminute });
            $scope.newAppointment.end = moment({ year: y1, month: m1, day: d1, hour: starthour, minute: startminute }).add(result[0].duration, 'minute');
            $scope.newAppointment.existingPatient = result[0].existingPatient;
            $scope.newAppointment.patientId = result[0].patientId;
            $scope.newAppointment.mobile = result[0].mobile;
            $scope.newAppointment.clinicId = result[0].clinicId;
           // console.log(result[0].clinicId);
            //Create appointment
            $scope.newAppointment.$save(function (data) {
                $scope.calendarLoading(false);
                $scope.events.push($scope.newAppointment);
                toaster.pop('success', "Notification", "Appointment added successfully", 4000);
            },
        function () {
            //error || check for overlap 
            $scope.calendarLoading(false);
            toaster.pop('warning', "Notification", "An error occured", 4000);
        });
            //}
        });// End Modal 
    };// ExternalDrop

   

    //redirect calendar to the day agenda on day press ( month agenda )
    $scope.dayClick = function (dateDayClick, jsEvent, view) {
        
        var toThatDay = dateDayClick.format();
        uiCalendarConfig.calendars.myCalendar1.fullCalendar('changeView', 'agendaDay');
        uiCalendarConfig.calendars.myCalendar1.fullCalendar("gotoDate", toThatDay);
    };

    //render each event when page and events are ready
    $scope.eventRender = function (event, element, view) {
        var compileContent = function () {
            //console.log(event);
            $scope.s = [];
            $scope.s = event;
            $scope.remove = function (index,appoitnemtnId) {
                //alert("Index  " + index + "  patient ID  " + appoitnemtnId);
                //$scope.events.splice(index, 1);
                //Call appointment repo and delete appointment by appointmentId
                uiCalendarConfig.calendars.myCalendar1.fullCalendar('removeEvents', function (e) {
                    return e._id === index;
                }
                );
            };
            var content = $compile($templateCache.get('POPEVENT'))($scope);
            return content;
        };
        var popoverTemplate = ['<div class="popover" style="border:0;background-color: transparent;max-width:350px; width:290px;font-size:11px;height:220px">',
            '<div class="arrow"></div>',
            '<div class="popover-content" style="padding-left:0px;padding-bottom:6px;">',
            '</div>',
            '</div>'].join('');

        //element.popover({ placement: 'top', html: true, content: $compile($templateCache.get('POPEVENT'))($scope), container: 'body'});

        element.popover({
            // container: 'body',
            content: compileContent,
            template: popoverTemplate,
            placement: "top",
            html: true
        });

        element.attr({
            'tooltip': event.title,
            'tooltip-append-to-body': true
        });
        $('.fc-button').click(function () { $('.popover').remove(); });
        $('body').on('click', function (e) {
            if (!element.is(e.target) && element.has(e.target).length === 0 && $('.popover').has(e.target).length === 0)
                element.popover('hide');
        });

        $compile(element)($scope);
    };
    $scope.eventDragStart = function (event, jsEvent, ui, view) {
        $('#tooltip').remove();
        $('.tooltip').remove();
        $('.popover').remove();
    };
    $scope.eventMouseout = function (event, jsEvent, view) {
        $('#tooltip').remove();
        $('.tooltip').remove();


    };
    $scope.eventDragStop = function (event, jsEvent, ui, view) {
        $('#tooltip').remove();
        $('.tooltip').remove();
        $('.popover').remove();
    };
    $scope.viewRender = function(view, element) {
        $('.popover').remove();
    };

    $scope.alertOnResize = function (event, delta, revertFunc, jsEvent, ui, view) {
        $scope.calendarLoading(true);
        $scope.resizedEvent = {};
        appointmentResource.appointments.get({ id: event.id }).$promise.then(function (data) {
            $scope.resizedEvent = data;
            $scope.resizedEvent.start = moment(event.start._d).utcOffset('Asia/Beirut');
            $scope.resizedEvent.end = moment(event.end._d).utcOffset('Asia/Beirut');
            $scope.resizedEvent.$update({ id: event.id },
                    function (data) {
                        $scope.calendarLoading(false);
                        toaster.pop('success', "Notification", "Appointment updated successfully", 4000);
                    }, function (error) {
                        $scope.calendarLoading(false);
                        revertFunc();
                        toaster.pop('warning', "Notification", "An error occured", 4000);
                    });
        }, function (error) {
            $scope.calendarLoading(false);
            revertFunc();
            toaster.pop('warning', "Notification", "An error occured", 4000);
        });



    };
    $scope.eventDrop = function (event, delta, revertFunc, jsEvent, ui, view) {
        $scope.calendarLoading(true);
        $scope.droppedEvent = {};
        appointmentResource.appointments.get({ id: event.id }).$promise.then(function (data) {
            //console.log(data);
            $scope.droppedEvent = data;
            $scope.droppedEvent.start = event.start;
            $scope.droppedEvent.end = event.end;
            $scope.droppedEvent.id = event.id;
            //console.log($scope.droppedEvent.end);
            $scope.droppedEvent.$update({ id: event.id },
                    function (data) {
                        $scope.calendarLoading(false);
                        toaster.pop('success', "Notification", "Appointment updated successfully", 4000);
                    }, function (error) {
                        $scope.calendarLoading(false);
                        revertFunc();
                        toaster.pop('warning', "Notification", "An error occured !", 4000);
                    });
        }, function (error) {
            $scope.calendarLoading(false);
            revertFunc();
            toaster.pop('warning', "Notification", "An error occured !!", 4000);
        });// end Get Method
    }//end event drop

    $scope.select = function (start, end, allDay, ev) {

        // if condition -> Prevent select method on month agenda & prevent select to run on dayclick in month agenda
        if (ev.name !== 'month') {
        var state = "selection";
        $scope.openModal(state).result
        .then(function (result) {
            $scope.calendarLoading(true);


            //console.log(result);
            // alert(angular.toJson(result));

            $scope.newAppointment = {};
            appointmentResource.appointments.get({ id: 0 }).$promise.then(function (data) {
                $scope.newAppointment = data;
                $scope.sendPost();
            });
            // creating appointment -> Post 
            $scope.sendPost = function () {
                $scope.newAppointment.title = result[0].patientName;
                $scope.newAppointment.patientName = result[0].patientName;
                $scope.newAppointment.description = result[0].description;
                $scope.newAppointment.allDay = false,
                $scope.newAppointment.start = start;
                $scope.newAppointment.end = end;
                $scope.newAppointment.existingPatient = result[0].existingPatient;
                $scope.newAppointment.patientId = result[0].patientId;
                $scope.newAppointment.mobile = result[0].mobile;
                $scope.newAppointment.$save(function (data) {
                    $scope.calendarLoading(false);
                    $scope.events.push($scope.newAppointment);
                    toaster.pop('success', "Notification", "Appointment added successfully", 4000);
                },
            function () {
                //error || check for overlap 
            });
            }
        });// End Modal
        }
    }//end select function
   
    
    /* config object */
    $scope.uiConfig = {
        calendar: {
            height: 650,
            editable: true,
            header: {
                left: 'prev,today,next',
                center: 'title',
                right: 'month,agendaWeek,agendaDay'
            },
            allDaySlot: false,
            droppable: true,
            viewRender: $scope.viewRender,
            // timezone: 'Europe/Copenhagen',
            eventDragStart: $scope.eventDragStart,
            eventMouseout: $scope.eventMouseout,
            eventDragStop: $scope.eventDragStop,
            eventRender: $scope.eventRender,
            dayClick: $scope.dayClick,
            eventClick: $scope.alertOnEventClick,
            eventDrop: $scope.eventDrop,
            eventResize: $scope.alertOnResize,
            drop: $scope.externalDrop,
            selectable: true,
            selectHelper: true,
            select: $scope.select,
            selectOverlap: function (event) {
                //get current view name  
                var view = uiCalendarConfig.calendars.myCalendar1.fullCalendar('getView');
               // while agenda month , select overlap disable droping the external drop in a slot that have an event in
                if (view.name !== "month") {
                    // if not month return false to prevent overlaping , and give background warning
                    return event.rendering === 'background';
                } else {
                    //if month return true to enable overlaping -> giving access to external drop to get working 
                    return true;
                }
            },
            loading: function (bool, view) {
                if (bool) {
                    $('.spinner').show();
                } else {
                    $('.spinner').hide();
                }
            },
            events: {
                data: eventsCall
            },
            slotDuration: '00:15:00',
            defaultTimedEventDuration: "00:30:00",
            eventLimit: true,
            views: {
                agenda: {
                    //eventLimit: 20,
                    eventLimitText: "more",
                    eventLimitClick: "popover"
                }
            },
            eventOverlap: function (stillEvent, movingEvent) {
                
                    return stillEvent.allDay && movingEvent.allDay;
              
                
            }
        }
    };

    /* Event sources array */
    $scope.eventSources = [$scope.events];
} //  +++++ END Calendar Controller ++++


//  + ----------------------------------------------------------------------- +
//  |                                                                         |
//  |                        MainCtrl         
//  |                                                                         |
//  + ----------------------------------------------------------------------- +

function MainCtrl(patientResource, $scope) {


 
   

    this.maxdate1 = moment();
    this.maxdate = moment();
    this.mindate = moment();


    this.datepickeroptions = {};
    this.datepickeroptions = {
        mindate: moment('1900-01-01'),
        maxdate: moment(),
        maxview: 'year',
        minview: 'date',
        format1: 'YYYY-MM-DD'
    }

    this.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        this.opened = true;
    };
    this.opened = false;


    this.datePicker = {
        minDate: "1900-01-01", maxDate: new Date()
    }
    this.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    this.validation =
    {
        numeric: /^[a-zA-Z]+$/,
        number: /^[0-9]*$/,
        alpha: /^[a-zA-Z0-9]*$/,
        alpha_d: /^[a-zA-Z0-9-_.]+$/

    };


   
    this.slideInterval = 5000;


    /**
     * states - Data used in Advanced Form view for Chosen plugin
     */
    this.states = [
        'Alabama',
        'Alaska'
        
    ];

    /**
     * New consultation tabset active boutton (Laboratory Data)
     */
    this.isActive = false;
    this.labTabActive = function () {
        this.isActive = true;
        //Scroll to top

    };

    /**
     * persons - Data used in Tables view for Data Tables plugin
     */
    this.searchname = "";
    this.persons = [
        {
            patientId: '1',
            firstName: 'Hasan',
            middelName: 'Ibrahim',
            lastName: 'Rifaii',
            mobile: '961 3 999999'
        }
    ];
    this.assistants = [
            {
                id: '1',
                Name: 'Samah',
                type: 'Basic',
                clinic_link: 'Saida',
                price: '200$',
                purchase_date: '22-03-2015',
                expiry_date: '22-03-2016',
                status: 'Enabled'
            }
    ];


    this.clinics = [
    {
        id: '1',
        Name: 'Hamra',
        type: 'Basic',
        clinic_link: 'Basic',
        price: '200$',
        purchase_date: '22-03-2015',
        expiry_date: '22-03-2016',
        status: 'Enabled'
    }
    ];
    /**
     * check's - Few variables for checkbox input used in iCheck plugin. Only for demo purpose
     */
    this.checkOne = true;
    this.checkTwo = true;
    this.checkThree = true;
    this.checkFour = true;

    /**
     * knobs - Few variables for knob plugin used in Advanced Plugins view
     */
    this.knobOne = 75;
    this.knobTwo = 95;
    this.knobThree = 50;
    this.knobSystolic = 100;
    this.knobDiastolic = 70;
    this.KnobHeartRate = 70;
    this.KnobTemperature = 37;

    /**
     * Variables used for Ui Elements view
     */
    this.bigTotalItems = 175;
    this.bigCurrentPage = 1;
    this.maxSize = 5;
    this.singleModel = 1;
    this.radioModel = 'Middle';
    this.checkModel = {
        left: false,
        middle: true,
        right: false
    };


    /**
     * alerts - used for dynamic alerts in Notifications and Tooltips view
     */
    this.alerts = [
        { type: 'danger', msg: 'Oh snap! Change a few things up and try submitting again.' },
        { type: 'success', msg: 'Well done! You successfully read this important alert message.' },
        { type: 'info', msg: 'OK, You are done a great job man.' },
    ];

    /**
     * addAlert, closeAlert  - used to manage alerts in Notifications and Tooltips view
     */
    this.addAlert = function () {
        this.alerts.push({ msg: 'Another alert!' });
    };

    this.closeAlert = function (index) {
        this.alerts.splice(index, 1);
    };

    /**
     * randomStacked - used for progress bar (stacked type) in Badges adn Labels view
     */
    this.randomStacked = function () {
        this.stacked = [];
        var types = ['success', 'info', 'warning', 'danger'];

        for (var i = 0, n = Math.floor((Math.random() * 4) + 1) ; i < n; i++) {
            var index = Math.floor((Math.random() * 4));
            this.stacked.push({
                value: Math.floor((Math.random() * 30) + 1),
                type: types[index]
            });
        }
    };
    /**
     * initial run for random stacked value
     */
    this.randomStacked();

    /**
     * summernoteText - used for Summernote plugin
     */
    this.summernoteText = ['<h3>Hello Jonathan! </h3>',
    '<p>dummy text of the printing and typesetting industry. <strong>Lorem Ipsum has been the dustrys</strong> standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more',
        'recently with</p>'].join('');

    /**
     * General variables for Peity Charts
     * used in many view so this is in Main controller
     */
    this.BarChart = {
        data: [5, 3, 9, 6, 5, 9, 7, 3, 5, 2, 4, 7, 3, 2, 7, 9, 6, 4, 5, 7, 3, 2, 1, 0, 9, 5, 6, 8, 3, 2, 1],
        options: {
            fill: ["#1ab394", "#d7d7d7"],
            width: 100
        }
    };

    this.BarChart2 = {
        data: [5, 3, 9, 6, 5, 9, 7, 3, 5, 2],
        options: {
            fill: ["#1ab394", "#d7d7d7"],
        }
    };

    this.BarChart3 = {
        data: [5, 3, 2, -1, -3, -2, 2, 3, 5, 2],
        options: {
            fill: ["#1ab394", "#d7d7d7"],
        }
    };

    this.LineChart = {
        data: [5, 9, 7, 3, 5, 2, 5, 3, 9, 6, 5, 9, 4, 7, 3, 2, 9, 8, 7, 4, 5, 1, 2, 9, 5, 4, 7],
        options: {
            fill: '#1ab394',
            stroke: '#169c81',
            width: 64
        }
    };

    this.LineChart2 = {
        data: [3, 2, 9, 8, 47, 4, 5, 1, 2, 9, 5, 4, 7],
        options: {
            fill: '#1ab394',
            stroke: '#169c81',
            width: 64
        }
    };

    this.LineChart3 = {
        data: [5, 3, 2, -1, -3, -2, 2, 3, 5, 2],
        options: {
            fill: '#1ab394',
            stroke: '#169c81',
            width: 64
        }
    };

    this.LineChart4 = {
        data: [5, 3, 9, 6, 5, 9, 7, 3, 5, 2],
        options: {
            fill: '#1ab394',
            stroke: '#169c81',
            width: 64
        }
    };

    this.PieChart = {
        data: [1, 5],
        options: {
            fill: ["#1ab394", "#d7d7d7"]
        }
    };

    this.PieChart2 = {
        data: [226, 360],
        options: {
            fill: ["#1ab394", "#d7d7d7"]
        }
    };
    this.PieChart3 = {
        data: [0.52, 1.561],
        options: {
            fill: ["#1ab394", "#d7d7d7"]
        }
    };
    this.PieChart4 = {
        data: [1, 4],
        options: {
            fill: ["#1ab394", "#d7d7d7"]
        }
    };
    this.PieChart5 = {
        data: [226, 134],
        options: {
            fill: ["#1ab394", "#d7d7d7"]
        }
    };
    this.PieChart6 = {
        data: [0.52, 1.041],
        options: {
            fill: ["#1ab394", "#d7d7d7"]
        }
    };

    //ionSlider
    
    //this.ionSliderOptions8 = {
    //    min: 0,
    //    max: 200,
    //    step: 1,
    //    postfix: " $"
    //};

};

angular
    .module('inspinia')
    .controller('MainCtrl', MainCtrl)
    .controller('modalDemoCtrl', modalDemoCtrl)
    .controller('CalendarCtrl', CalendarCtrl)
    .controller('patientList', patientList)
    .controller('viewPatient', viewPatient)
    .controller('editPatient', editPatient)
    .controller('newConsultation', newConsultation)
    .controller('consultationList', consultationList)
    .controller('consultationView', consultationView)
    .controller('editConsultation', editConsultation)
    .controller('newFollowUp', newFollowUp)
    .controller('editFollowUp', editFollowUp)
    .controller('newPatient', ['patientResource', newPatient])
    .controller('newDoctor', ['doctorResource', newDoctor]); 