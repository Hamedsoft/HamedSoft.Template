(function (window, document) {
    'use strict';
    /*
     * ------------------------------------------------------------
     * INITIALIZATION
     * ------------------------------------------------------------
     */
    document.addEventListener('DOMContentLoaded', function () {
        loadSettingsSections();
    });
    async function loadSettingsSections() {
        const containers = document.querySelectorAll('.settings-container');
        for (const container of containers) {
            await loadSettingsSection(container);
        }
    }
    async function loadSettingsSection(container) {
        const module = container.dataset.settingsModule || null;
        const feature = container.dataset.settingsFeature || null;
        const category = container.dataset.settingsCategory || null;
        const url = container.dataset.settingsSectionUrl;

        const params = new URLSearchParams();

        if (module !== null) {
            params.set('module', module);
        }

        if (feature !== null) {
            params.set('feature', feature);
        }

        if (category !== null) {
            params.set('category', category);
        }

        try {
            const queryString = params.toString();
            const requestUrl = queryString ? `${url}?${queryString}` : url;

            const response = await fetch(requestUrl, {
                method: 'GET', headers: {'X-Requested-With': 'XMLHttpRequest'}
            });

            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }

            const html = await response.text();

            container.innerHTML = html;

            initializeSettingControls(container);
        } catch (error) {
            console.error('Failed to load settings section.', error);

            container.innerHTML =
                '<div class="alert alert-danger">خطا در بارگذاری تنظیمات</div>';
        }
    }

    function initializeSettingControls(container) {
        initializePersianDatePickers(container);
        initializeTimePickers(container);
        synchronizeDateTimeValues(container);
        synchronizeTimeSpanValues(container);
    }
    /*
     * ------------------------------------------------------------
     * SAVE
     * ------------------------------------------------------------
     */
    document.addEventListener('click', async function (event) {
        const button = event.target.closest('.setting-save-btn');
        if (!button) {
            return;
        }
        event.preventDefault();
        await saveSetting(button);
    });
    async function saveSetting(button) {
        const key = button.dataset.settingKey;
        if (!key) {
            console.error('Setting key is missing.');
            return;
        }
        /*
         * ------------------------------------------------------------
         * Synchronize UI controls before reading the value.
         * This is especially important for DateTime and TimeSpan.
         * ------------------------------------------------------------
         */
        synchronizeSettingValue(button);
        /*
         * ------------------------------------------------------------
         * Container
         * ------------------------------------------------------------
         */
        const container = button.closest('.settings-container');
        if (!container) {
            console.error('Settings container was not found.');
            return;
        }
        /*
         * ------------------------------------------------------------
         * Read final value
         * ------------------------------------------------------------
         */
        const value = getSettingValue(button);
        /*
         * ------------------------------------------------------------
         * Validate value
         * ------------------------------------------------------------
         */
        if (value === null || value === undefined) {
            console.error(`Setting value not found for key: ${key}`);
            return;
        }
        /*
         * ------------------------------------------------------------
         * Update URL
         * ------------------------------------------------------------
         */
        const updateUrl = container.dataset.settingsUpdateUrl;
        if (!updateUrl) {
            console.error('Settings update URL is missing.');
            return;
        }
        /*
         * ------------------------------------------------------------
         * Anti-forgery token
         * ------------------------------------------------------------
         */
        const token = container.querySelector('input[name="__RequestVerificationToken"]')?.value;
        if (!token) {
            console.error('Anti-forgery token was not found.');
            return;
        }
        /*
         * ------------------------------------------------------------
         * Form Data
         * ------------------------------------------------------------
         */
        const formData = new FormData();
        formData.append('key', key);
        formData.append('value', value);
        formData.append('__RequestVerificationToken', token);
        /*
         * ------------------------------------------------------------
         * UI state
         * ------------------------------------------------------------
         */
        const originalText = button.innerHTML;
        button.disabled = true;
        button.innerHTML = 'در حال ذخیره...';
        /*
         * ------------------------------------------------------------
         * POST
         * ------------------------------------------------------------
         */
        try {
            const response = await fetch(updateUrl, {
                method: 'POST',
                body: formData,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            });
            /*
             * --------------------------------------------------------
             * HTTP error
             * --------------------------------------------------------
             */
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }
            /*
             * --------------------------------------------------------
             * Success
             * --------------------------------------------------------
             */
            button.innerHTML = 'ذخیره شد';

            Toast.success('تنظیمات با موفقیت ذخیره شد.');

            setTimeout(function () {
                button.innerHTML = originalText;
            }, 1500);
        } catch (error) {
            console.error('Failed to save setting.', error
            );

            Toast.error('ذخیره تنظیمات با خطا مواجه شد.');

            button.innerHTML = 'خطا';

            setTimeout(function () {
                button.innerHTML = originalText;
            }, 2000);
        } finally {
            button.disabled = false;
        }
    }
    /*
     * ------------------------------------------------------------
     * SYNCHRONIZE SETTING VALUE BEFORE SAVE
     * ------------------------------------------------------------
     */
    function synchronizeSettingValue(button) {
        const container = button.closest('.settings-container');
        if (!container) {
            return;
        }
        const key = button.dataset.settingKey;
        if (!key) {
            return;
        }
        const escapedKey = CSS.escape(key);
        /*
         * DateTime
         */
        const dateTimeInput = container.querySelector(`.setting-gregorian-value[data-setting-key="${escapedKey}"]`);
        if (dateTimeInput) {
            const wrapper = dateTimeInput.closest('.setting-datetime-wrapper');
            if (wrapper) {
                const dateInput = wrapper.querySelector('.persian-date-picker');
                const timeInput = wrapper.querySelector('.setting-time-input');
                if (dateInput && timeInput) {
                    synchronizeDateTimeValue(dateInput, timeInput, dateTimeInput);
                }
            }
            return;
        }
        /*
         * TimeSpan
         */
        const timeSpanInput = container.querySelector(`.setting-timespan-value[data-setting-key="${escapedKey}"]`);
        if (timeSpanInput) {
            const wrapper = timeSpanInput.closest('.setting-datetime-wrapper');
            if (!wrapper) {
                return;
            }
            const timeInput = wrapper.querySelector('.setting-time-input');
            if (timeInput) {
                updateTimeSpanValue(timeInput);
            }
        }
    }
    /*
     * ------------------------------------------------------------
     * GET VALUE
     * ------------------------------------------------------------
     */
    function getSettingValue(button) {
        const key = button.dataset.settingKey;
        if (!key) {
            return null;
        }
        const container = button.closest('.settings-container');
        if (!container) {
            return null;
        }
        const escapedKey = CSS.escape(key);
        /*
         * DateTime
         */
        const dateTimeInput = container.querySelector(`.setting-gregorian-value[data-setting-key="${escapedKey}"]`);
        if (dateTimeInput) {
            return dateTimeInput.value;
        }
        /*
         * TimeSpan
         */
        const timeSpanInput = container.querySelector(`.setting-timespan-value[data-setting-key="${escapedKey}"]`);
        if (timeSpanInput) {
            return timeSpanInput.value;
        }
        /*
         * Normal setting
         */
        const input = container.querySelector(`.setting-value-input[data-setting-key="${escapedKey}"]`);
        if (!input) {
            return null;
        }
        /*
         * Checkbox
         */
        if (input.type === 'checkbox') {
            return input.checked ? 'true' : 'false';
        }
        return input.value;
    }
    /*
     * ------------------------------------------------------------
     * PASSWORD
     * ------------------------------------------------------------
     */
    document.addEventListener('click', function (event) {
        const button = event.target.closest('.showpassword');
        if (!button) {
            return;
        }
        event.preventDefault();
        const inputGroup = button.closest('.input-group');
        if (!inputGroup) {
            return;
        }
        const input = inputGroup.querySelector('input.setting-value-input');
        if (!input) {
            return;
        }
        const icon = button.querySelector('i');
        const isPassword = input.type === 'password';
        input.type = isPassword ? 'text' : 'password';
        if (icon) {
            icon.classList.toggle('zmdi-eye-off', !isPassword);
            icon.classList.toggle('zmdi-eye', isPassword);
        }
    });
    /*
     * ------------------------------------------------------------
     * PERSIAN DATE PICKER
     * ------------------------------------------------------------
     */
    function initializePersianDatePickers(container) {
        const inputs = container.querySelectorAll('.persian-date-picker');
        inputs.forEach(function (input) {
            const $input = $(input);
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
                },
                onSelect: function () {
                    synchronizeDateTimeWrapper(input);
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
        const inputs = container.querySelectorAll('.setting-time-input');
        inputs.forEach(function (input) {
            const $input = $(input);
            if ($input.data('timepickerInitialized')) {
                return;
            }
            $input.timepicker();
            $input.data('timepickerInitialized', true);
            $input.on('change', function () {
                updateTimeSpanValue(input);
                synchronizeDateTimeWrapper(input);
            });
        });
    }
    /*
     * ------------------------------------------------------------
     * DATETIME SYNCHRONIZATION
     * ------------------------------------------------------------
     */
    function synchronizeDateTimeWrapper(input) {
        const wrapper = input.closest('.setting-datetime-wrapper');
        if (!wrapper) {
            return;
        }
        const dateInput = wrapper.querySelector('.persian-date-picker');
        const timeInput = wrapper.querySelector('.setting-time-input');
        const hiddenInput = wrapper.querySelector('.setting-gregorian-value');
        if (!dateInput || !timeInput || !hiddenInput) {
            return;
        }
        synchronizeDateTimeValue(dateInput, timeInput, hiddenInput);
    }

    function synchronizeDateTimeValues(container) {
        const wrappers = container.querySelectorAll('.setting-datetime-wrapper');
        wrappers.forEach(function (wrapper) {
            const dateInput = wrapper.querySelector('.persian-date-picker');
            const timeInput = wrapper.querySelector('.setting-time-input');
            const hiddenInput = wrapper.querySelector('.setting-gregorian-value');
            if (!dateInput || !timeInput || !hiddenInput) {
                return;
            }
            synchronizeDateTimeValue(dateInput, timeInput, hiddenInput);
        });
    }

    function synchronizeDateTimeValue(dateInput, timeInput, hiddenInput) {
        const datepicker = $(dateInput).data('datepicker');
        if (!datepicker || !datepicker.model) {
            return;
        }
        const selected = datepicker.model.state.selected;
        if (!selected || !selected.unixDate) {
            return;
        }
        const selectedDate = new Date(selected.unixDate);
        const time = $(timeInput).timepicker('getTime');
        if (time) {
            selectedDate.setHours(time.getHours(), time.getMinutes(), time.getSeconds(), 0);
        } else {
            const existingValue = hiddenInput.value;
            if (existingValue) {
                const existingDate = new Date(existingValue);
                if (!Number.isNaN(existingDate.getTime())) {
                    selectedDate.setHours(existingDate.getHours(), existingDate.getMinutes(), existingDate.getSeconds(), 0);
                }
            }
        }
        hiddenInput.value = formatDateTimeValue(selectedDate);
    }

    function formatDateTimeValue(date) {
        const year = date.getFullYear().toString().padStart(4, '0');
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const day = date.getDate().toString().padStart(2, '0');
        const hours = date.getHours().toString().padStart(2, '0');
        const minutes = date.getMinutes().toString().padStart(2, '0');
        const seconds = date.getSeconds().toString().padStart(2, '0');
        return (`${year}-${month}-${day}` + `T${hours}:${minutes}:${seconds}`);
    }
    /*
     * ------------------------------------------------------------
     * TIMESPAN
     * ------------------------------------------------------------
     */
    function synchronizeTimeValue(input) {
        const wrapper = input.closest('.setting-datetime-wrapper');
        if (!wrapper) {
            return;
        }
        const gregorianInput = wrapper.querySelector('.setting-gregorian-value');
        if (gregorianInput) {
            const dateInput = wrapper.querySelector('.persian-date-picker');
            const timeInput = wrapper.querySelector('.setting-time-input');
            if (dateInput && timeInput) {
                synchronizeDateTimeValue(dateInput, timeInput, gregorianInput);
            }
            return;
        }
        const timeSpanInput = wrapper.querySelector('.setting-timespan-value');
        if (timeSpanInput) {
            updateTimeSpanValue(input);
        }
    }

    function synchronizeTimeSpanValues(container) {
        const inputs = container.querySelectorAll('.setting-timespan-value');
        inputs.forEach(function (hiddenInput) {
            const wrapper = hiddenInput.closest('.setting-datetime-wrapper');
            if (!wrapper) {
                return;
            }
            const input = wrapper.querySelector('.setting-time-input');
            if (!input) {
                return;
            }
            updateTimeSpanValue(input);
        });
    }

    function updateTimeSpanValue(input) {
        const wrapper = input.closest('.setting-datetime-wrapper');
        if (!wrapper) {
            return;
        }
        const hiddenInput = wrapper.querySelector('.setting-timespan-value');
        if (!hiddenInput) {
            return;
        }
        const time = $(input).timepicker('getTime');
        if (!time) {
            hiddenInput.value = '';
            return;
        }
        hiddenInput.value = formatTimeSpan(time);
    }

    function formatTimeSpan(date) {
        const hours = String(date.getHours()).padStart(2, '0');
        const minutes = String(date.getMinutes()).padStart(2, '0');
        const seconds = String(date.getSeconds()).padStart(2, '0');
        return (`${hours}:${minutes}:${seconds}`);
    }
    /*
     * ------------------------------------------------------------
     * TODAY BUTTON
     * ------------------------------------------------------------
     */
    document.addEventListener('click', function (event) {
        const button = event.target.closest('.setting-today-button');
        if (!button) {
            return;
        }
        event.preventDefault();
        const input = button.closest('.input-group')?.querySelector('.persian-date-picker');
        if (!input) {
            return;
        }
        const datepicker = $(input).data('datepicker');
        if (!datepicker || !datepicker.model) {
            return;
        }
        const now = new Date().valueOf();
        datepicker.model.state.setSelectedDateTime('unix', now);
        datepicker.model.state.setViewDateTime('unix', now);
        datepicker.model.view.reRender();
        synchronizeDateTimeWrapper(input);
    });
    /*
     * ------------------------------------------------------------
     * SET CURRENT TIME
     * ------------------------------------------------------------
     */
    document.addEventListener('click', function (event) {
        const button = event.target.closest('.setting-set-time-button');
        if (!button) {
            return;
        }
        event.preventDefault();
        const wrapper = button.closest('.setting-datetime-wrapper');
        if (!wrapper) {
            return;
        }
        const input = wrapper.querySelector('.setting-time-input');
        if (!input) {
            return;
        }
        $(input).timepicker('setTime', new Date());
        synchronizeTimeValue(input);
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
        return value.replace(/0/g, '۰')
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
})(window, document);