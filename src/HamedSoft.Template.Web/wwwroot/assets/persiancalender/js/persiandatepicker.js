(function (window, $) {
    'use strict';
    /*
     * ------------------------------------------------------------
     * INITIALIZATION
     * ------------------------------------------------------------
     */
    $(document).ready(function () {
        initializePersianDatePickers(document);
        initializeTimePickers(document);
    });
    /*
     * ------------------------------------------------------------
     * HELPERS
     * ------------------------------------------------------------
     */
    function convertToPersianNumbers(value) {
        if (!value) {
            return value;
        }
        return value.replace(/0/g, '۰').replace(/1/g, '۱').replace(/2/g, '۲').replace(/3/g, '۳').replace(/4/g, '۴').replace(/5/g, '۵').replace(/6/g, '۶').replace(/7/g, '۷').replace(/8/g, '۸').replace(/9/g, '۹');
    }
    /*
     * ------------------------------------------------------------
     * PERSIAN DATE PICKER
     * ------------------------------------------------------------
     */
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
                navigator: {
                    enabled: true,
                    scroll: {
                        enabled: true
                    },
                    text: {
                        btnNextText: '<',
                        btnPrevText: '>'
                    }
                },
                toolbox: {
                    calendarSwitch: {
                        enabled: false
                    }
                },
                timePicker: {
                    enabled: false
                },
                dayPicker: {
                    enabled: true,
                    titleFormat: 'YYYY MMMM'
                },
                monthPicker: {
                    enabled: true
                },
                yearPicker: {
                    enabled: true
                }
            });
            $input.data('pDatepickerInitialized', true);
            $input.on('input.persianDatePicker', function () {
                $(this).val(convertToPersianNumbers($(this).val()));
            });
        });
    }
    /*
     * ------------------------------------------------------------
     * TIME PICKER
     * ------------------------------------------------------------
     */
    function initializeTimePickers(container) {
        const $container = container ? $(container) : $(document);
        $container.find('.setting-time-input').each(function () {
            const $input = $(this);
            if ($input.data('timepickerInitialized')) {
                return;
            }
            $input.timepicker();
            $input.data('timepickerInitialized', true);
        });
    }
    /*
     * ------------------------------------------------------------
     * TODAY BUTTON
     * ------------------------------------------------------------
     */
    $(document).on('click', '.setting-today-button', function (event) {
        event.preventDefault();
        const $button = $(this);
        const $input = $button.closest('.input-group').find('.persian-date-picker').first();
        if ($input.length === 0) {
            return;
        }
        const datepicker = $input.data('datepicker');
        if (!datepicker || !datepicker.model) {
            return;
        }
        const model = datepicker.model;
        const now = new Date().valueOf();
        model.state.setSelectedDateTime('unix', now);
        model.state.setViewDateTime('unix', now);
        model.view.reRender();
    });
    /*
     * ------------------------------------------------------------
     * SET CURRENT TIME
     * ------------------------------------------------------------
     */
    $(document).on('click', '.setting-set-time-button', function () {
        const $button = $(this);
        const $timeInput = $button.closest('.setting-datetime-wrapper').find('.setting-time-input').first();
        if ($timeInput.length === 0) {
            return;
        }
        $timeInput.timepicker('setTime', new Date());
    });
    /*
     * ------------------------------------------------------------
     * PUBLIC API
     * ------------------------------------------------------------
     */
    window.initializePersianDatePickers = initializePersianDatePickers;
    window.initializeTimePickers = initializeTimePickers;
})(window, jQuery);