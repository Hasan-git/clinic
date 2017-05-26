var inspiniaTemplate = 'views/common/notify.html';
angular
    .module('inspinia')
    .controller('MainCtrl', MainCtrl)
;

function MainCtrl(patientResource, $scope,$rootScope) {

    $rootScope.main = {
        clinicId: "0AA75235-15D1-11E6-9663-005056C00111",
        doctorId: "0AA75235-15D1-11E6-9663-005056C00112"
    }

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
        alpha_d: /^[a-zA-Z0-9-_.]+$/,
        date: /^((((19|[2-9]\d)\d{2})\-(0[13578]|1[02])\-(0[1-9]|[12]\d|3[01]))|(((19|[2-9]\d)\d{2})\-(0[13456789]|1[012])\-(0[1-9]|[12]\d|30))|(((19|[2-9]\d)\d{2})\-02\-(0[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))\-02\-29))$/

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