document.addEventListener("DOMContentLoaded", () => {

    initializeSettingSaveButtons();

});


document.addEventListener("click", function (event) {

    const button = event.target.closest(".showpassword");

    if (!button) {
        return;
    }

    event.preventDefault();

    const inputGroup = button.closest(".input-group");

    if (!inputGroup) {
        return;
    }

    const input = inputGroup.querySelector("input.setting-value");

    if (!input) {
        return;
    }

    const icon = button.querySelector("i");

    const isPassword = input.type === "password";

    input.type = isPassword ? "text" : "password";

    if (icon) {
        icon.classList.toggle("zmdi-eye-off", !isPassword);
        icon.classList.toggle("zmdi-eye", isPassword);
    }

});


function initializeSettingSaveButtons() {

    document
        .querySelectorAll(".setting-save-btn")
        .forEach(button => {

            button.addEventListener("click", async () => {
                await saveSetting(button);
            });

        });
}


async function saveSetting(button) {

    const key = button.dataset.settingKey;

    if (!key) {
        return;
    }

    const value = getSettingValue(key);

    if (value === null) {
        return;
    }

    const token = document.querySelector(
        'input[name="__RequestVerificationToken"]'
    )?.value;

    const formData = new FormData();

    formData.append("key", key);
    formData.append("value", value);

    if (token) {
        formData.append("__RequestVerificationToken", token);
    }

    const originalText = button.innerHTML;

    button.disabled = true;
    button.innerHTML = "در حال ذخیره...";

    try {

        const response = await fetch(
            "/Settings/Update",
            {
                method: "POST",
                body: formData
            });

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        button.innerHTML = "ذخیره شد";

        setTimeout(() => {
            button.innerHTML = originalText;
        }, 1500);

    }
    catch (error) {

        console.error(error);

        button.innerHTML = "خطا";

        setTimeout(() => {
            button.innerHTML = originalText;
        }, 2000);

    }
    finally {

        button.disabled = false;

    }
}


function getSettingValue(key) {

    const dateTimeInput = document.querySelector(
        `.setting-gregorian-value[data-setting-key="${CSS.escape(key)}"]`
    );

    if (dateTimeInput) {
        return dateTimeInput.value;
    }

    const input = document.querySelector(
        `.setting-value-input[data-setting-key="${CSS.escape(key)}"]`
    );

    if (!input) {
        return null;
    }

    return input.value;
}