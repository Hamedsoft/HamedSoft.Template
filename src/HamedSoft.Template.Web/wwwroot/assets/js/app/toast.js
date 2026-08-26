(function (window, document, $) {
    'use strict';
    /*
     * ------------------------------------------------------------
     * CONFIGURATION
     * ------------------------------------------------------------
     */
    const DEFAULT_DELAY = 3000;
    const CONTAINER_ID = 'app-toast-container';
    /*
     * ------------------------------------------------------------
     * CONTAINER
     * ------------------------------------------------------------
     */
    function getContainer() {
        let container = document.getElementById(CONTAINER_ID);
        if (container) {
            return container;
        }
        container = document.createElement('div');
        container.id = CONTAINER_ID;
        container.className = 'position-fixed bottom-0 first-0 p-3';
        container.style.zIndex = '9999';
        document.body.appendChild(container);
        return container;
    }
    /*
     * ------------------------------------------------------------
     * SHOW
     * ------------------------------------------------------------
     */
    function show(message, type, delay) {
        if (!message) {
            return;
        }
        const container = getContainer();
        const toast = document.createElement('div');
        toast.className = `toast fade text-bg-${type} border-0`;
        toast.setAttribute('role', 'alert');
        toast.setAttribute('aria-live', 'assertive');
        toast.setAttribute('aria-atomic', 'true');
        if (type == 'info') 
            toast.innerHTML = `
                <div class="toast align-items-center text-white mb-1 bg-primary border-0 show" role="alert" aria-live="assertive" aria-atomic="true" data-bs-autohide="false">
	                <div class="d-flex">
		                <div class="toast-body">
                            ${escapeHtml(message)}
		                </div>
		                <button aria-label="Close" class="btn-close fs-20 ms-auto mt-2 pe-2" data-bs-dismiss="toast"><span aria-hidden="true">×</span></button>
	                </div>
                </div>
                `;
        else if (type == 'success')
            toast.innerHTML = `
                <div class="toast align-items-center text-white mb-1 bg-success border-0 show" role="alert" aria-live="assertive" aria-atomic="true" data-bs-autohide="false">
	                <div class="d-flex">
		                <div class="toast-body">
                            ${escapeHtml(message)}
		                </div>
		                <button aria-label="Close" class="btn-close fs-20 ms-auto mt-2 pe-2" data-bs-dismiss="toast"><span aria-hidden="true">×</span></button>
	                </div>
                </div>
                `;
        else if (type == 'warning')
            toast.innerHTML = `
                <div class="toast align-items-center text-white mb-1 bg-warning border-0 show" role="alert" aria-live="assertive" aria-atomic="true" data-bs-autohide="false">
	                <div class="d-flex">
		                <div class="toast-body">
                            ${escapeHtml(message)}
		                </div>
		                <button aria-label="Close" class="btn-close fs-20 ms-auto mt-2 pe-2" data-bs-dismiss="toast"><span aria-hidden="true">×</span></button>
	                </div>
                </div>
                `;
        else if (type == 'danger')
            toast.innerHTML = `
                <div class="toast align-items-center text-white mb-1 bg-danger border-0 show" role="alert" aria-live="assertive" aria-atomic="true" data-bs-autohide="false">
	                <div class="d-flex">
		                <div class="toast-body">
                            ${escapeHtml(message)}
		                </div>
		                <button aria-label="Close" class="btn-close fs-20 ms-auto mt-2 pe-2" data-bs-dismiss="toast"><span aria-hidden="true">×</span></button>
	                </div>
                </div>
                `;
        else 
            toast.innerHTML = `
                <div class="toast align-items-center text-white mb-1 bg-secondary border-0 show" role="alert" aria-live="assertive" aria-atomic="true" data-bs-autohide="false">
	                <div class="d-flex">
		                <div class="toast-body">
                            ${escapeHtml(message)}
		                </div>
		                <button aria-label="Close" class="btn-close fs-20 ms-auto mt-2 pe-2" data-bs-dismiss="toast"><span aria-hidden="true">×</span></button>
	                </div>
                </div>
                `;
        container.appendChild(toast);
        if (typeof bootstrap === 'undefined' || !bootstrap.Toast) {
            console.warn('Bootstrap Toast is not available.');
            return;
        }
        const bootstrapToast = bootstrap.Toast.getOrCreateInstance(toast, {
            delay: delay ?? DEFAULT_DELAY
        });
        toast.addEventListener('hidden.bs.toast', function () {
            toast.remove();
        }, {
            once: true
        });
        bootstrapToast.show();
    }
    /*
     * ------------------------------------------------------------
     * TYPES
     * ------------------------------------------------------------
     */
    function success(message, delay) {
        show(message, 'success', delay);
    }

    function error(message, delay) {
        show(message, 'danger', delay);
    }

    function warning(message, delay) {
        show(message, 'warning', delay);
    }

    function info(message, delay) {
        show(message, 'info', delay);
    }
    /*
     * ------------------------------------------------------------
     * SECURITY
     * ------------------------------------------------------------
     */
    function escapeHtml(value) {
        const element = document.createElement('div');
        element.textContent = String(value);
        return element.innerHTML;
    }
    /*
     * ------------------------------------------------------------
     * PUBLIC API
     * ------------------------------------------------------------
     */
    window.Toast = {
        success: success,
        error: error,
        warning: warning,
        info: info
    };
})(window, document, jQuery);