window.appendStar = function (elementName, isRequired) {
    if (elementName != null && isRequired) {
        const requiredStarHTML = '<span style="color:red">*</span>';
        const element = document.getElementsByName(elementName)[0].parentElement.parentElement.getElementsByClassName('rz-placeholder')[0];
        if (typeof element !== "undefined" && !element.innerHTML.trim().endsWith(requiredStarHTML)) {
            element.innerHTML = element.innerHTML + requiredStarHTML;
        }
    }
};