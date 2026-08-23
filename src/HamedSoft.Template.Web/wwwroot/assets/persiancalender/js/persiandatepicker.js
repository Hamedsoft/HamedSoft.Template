(function (window, $) {
    'use strict';
    function convertToPersianNumbers(value) {
        if (!value) {
            return value;
        }

        return value
            .replace(/0/g, '۰')
            .replace(/1/g, '۱')
            .replace(/2/g, '۲')
            .replace(/3/g, '۳')
            .replace(/4/g, '۴')
            .replace(/5/g, '۵')
            .replace(/6/g, '۶')
            .replace(/7/g, '۷')
            .replace(/8/g, '۸')
            .replace(/9/g, '۹');
    }

    function initializePersianDatePickers(container) {

        const $container = container ? $(container) : $(document);
        const $inputs = $container.find('.persian-date-picker');

        $inputs.each(function () {
            const $input = $(this);
            if ($input.data('pDatepickerInitialized')) {
                return;
            }

            $input.pDatepicker({
                format: 'YYYY/MM/DD',
                autoClose: true,
                initialValue: false,
                calendarType: 'persian',
                navigator: { enabled: true, scroll: { enabled: true }, text: { btnNextText: '<', btnPrevText: '>' } },
                toolbox: { calendarSwitch: { enabled: false } },
                timePicker: { enabled: false },
                dayPicker: { enabled: true, titleFormat: 'YYYY MMMM' },
                monthPicker: { enabled: true },
                yearPicker: { enabled: true }
            });

            $input.data('pDatepickerInitialized', true);
            $input.on('input.persianDatePicker', function () { $(this).val(convertToPersianNumbers($(this).val())); });

        });
    }

    window.initializePersianDatePickers = initializePersianDatePickers;

    $(document).ready(function () {

        initializePersianDatePickers(document);
    });

})(window, jQuery);