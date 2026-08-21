document.addEventListener("DOMContentLoaded", () => {
    initializeSettingDateTimePickers();
});

function initializeSettingDateTimePickers() {
    const pickers = document.querySelectorAll(
        ".setting-persian-datetime"
    );

    pickers.forEach(initializeSettingDateTimePicker);
}

function initializeSettingDateTimePicker(picker) {
    const key = picker.dataset.settingKey;
    const gregorianValue = picker.dataset.gregorianValue;

    if (!key || !gregorianValue) {
        return;
    }

    const date = new Date(gregorianValue);

    if (Number.isNaN(date.getTime())) {
        console.warn(
            `Invalid DateTime for setting '${key}': ${gregorianValue}`
        );

        return;
    }

    const hiddenInput = document.querySelector(
        `.setting-gregorian-value[data-setting-key="${CSS.escape(key)}"]`
    );

    const timeInput = document.querySelector(
        `.setting-time-input[data-setting-key="${CSS.escape(key)}"]`
    );

    if (!hiddenInput || !timeInput) {
        console.warn(
            `DateTime inputs not found for setting '${key}'.`
        );

        return;
    }

    /*
     * Gregorian -> Jalali
     */

    const jalali = toJalali(
        date.getFullYear(),
        date.getMonth() + 1,
        date.getDate()
    );

    picker.setValue(
        jalali.year,
        jalali.month,
        jalali.day
    );

    /*
     * Initialize time.
     */

    timeInput.value = formatTime(date);

    /*
     * Date changed.
     */

    picker.addEventListener("change", event => {
        updateGregorianValue(
            picker,
            timeInput,
            hiddenInput,
            event
        );
    });

    /*
     * Time changed.
     */

    timeInput.addEventListener("change", () => {
        updateGregorianValue(
            picker,
            timeInput,
            hiddenInput
        );
    });
}

function updateGregorianValue(
    picker,
    timeInput,
    hiddenInput,
    event = null
) {
    /*
     * If the component gives us an ISO value,
     * use its Gregorian date as the base.
     */

    let baseDate = null;

    if (event?.detail?.isoString) {
        baseDate = new Date(event.detail.isoString);
    }

    /*
     * Otherwise use the current hidden Gregorian value.
     */

    if (!baseDate || Number.isNaN(baseDate.getTime())) {
        baseDate = new Date(hiddenInput.value);
    }

    if (Number.isNaN(baseDate.getTime())) {
        return;
    }

    const timeParts = timeInput.value.split(":");

    const hours = Number(timeParts[0] ?? 0);
    const minutes = Number(timeParts[1] ?? 0);
    const seconds = Number(timeParts[2] ?? 0);

    baseDate.setHours(
        hours,
        minutes,
        seconds,
        0
    );

    hiddenInput.value = formatIsoLocal(baseDate);
}

function formatTime(date) {
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    const seconds = String(date.getSeconds()).padStart(2, "0");

    return `${hours}:${minutes}:${seconds}`;
}

function formatIsoLocal(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    const seconds = String(date.getSeconds()).padStart(2, "0");

    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
}