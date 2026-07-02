let handler = function (e) {
    e.preventDefault();
    e.returnValue = '';
    return '';
};

globalThis.unsavedChangesGuard = (function () {
    let enabled = false;

    return {
        enable: function () {
            if (enabled) return;
            window.addEventListener('beforeunload', handler);
            enabled = true;
        },
        disable: function () {
            if (!enabled) return;
            window.removeEventListener('beforeunload', handler);
            enabled = false;
        }
    };
})();