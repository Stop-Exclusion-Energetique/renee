;(function () {
    let pushState = history.pushState;
    let replaceState = history.replaceState;
    history.pushState = function () {
        pushState.apply(history, arguments);
        window.dispatchEvent(new Event('pushstate'));
        window.dispatchEvent(new Event('locationchange'));
    };
    history.replaceState = function () {
        replaceState.apply(history, arguments);
        window.dispatchEvent(new Event('replacestate'));
        window.dispatchEvent(new Event('locationchange'));
    };
    window.addEventListener('popstate', function () {
        window.dispatchEvent(new Event('locationchange'))
    });
})();


// Usage example:

window.addEventListener('locationchange', function () {
    let url = window.location.href;
    let r = document.querySelector(':root');

    if (
        url.endsWith("/quickAdd")
        || url.includes("/newoccupantdisplay/")
        || url.includes("/synthesis/identification/")
        || url.includes("/organizeAndFinance/")
        || url.includes("/synthesis/organizeAndFinance/")
        || url.includes("/realiseAndFollow/")
        || url.includes("/synthesis/realizeAndFollow/")
        || url.includes("/quickAddCoproperty")
        || url.includes("/coproperty/identification")
        || url.includes("/coproperty/organizeAndFinance")
        || url.includes("/coproperty/realizeAndFollow")
        || url.includes("/coproperty/synthesis/identification")
        || url.includes("/coproperty/synthesis/organizeAndFinance")
        || url.includes("/coproperty/synthesis/realizeAndFollow")
    ) {
        r.style.setProperty('--primary', '#64addd');
        r.style.setProperty('--rz-primary', '#64addd');
        r.style.setProperty('--pure-material-primary-rgb', '#64addd');
    } else {
        r.style.setProperty('--primary', '#eb6d78');
        r.style.setProperty('--rz-primary', '#eb6d78');
        r.style.setProperty('--pure-material-primary-rgb', '#eb6d78');
    }
});

window.addEventListener('load', function () {
    let url = window.location.href;
    let r = document.querySelector(':root');

    if (
        url.endsWith("/quickAdd")
        || url.includes("/newoccupantdisplay/")
        || url.includes("/synthesis/identification/")
        || url.includes("/organizeAndFinance/")
        || url.includes("/synthesis/organizeAndFinance/")
        || url.includes("/realiseAndFollow/")
        || url.includes("/synthesis/realizeAndFollow/")
        || url.includes("/quickAddCoproperty")
        || url.includes("/coproperty/identification")
        || url.includes("/coproperty/organizeAndFinance")
        || url.includes("/coproperty/realizeAndFollow")
        || url.includes("/coproperty/synthesis/identification")
        || url.includes("/coproperty/synthesis/organizeAndFinance")
        || url.includes("/coproperty/synthesis/realizeAndFollow")
    ) {
        r.style.setProperty('--primary', '#64addd');
        r.style.setProperty('--rz-primary', '#64addd');
        r.style.setProperty('--pure-material-primary-rgb', '#64addd');
    } else {
        r.style.setProperty('--primary', '#eb6d78');
        r.style.setProperty('--rz-primary', '#eb6d78');
        r.style.setProperty('--pure-material-primary-rgb', '#eb6d78');
    }
});