window.scrollToTop = () => {
    const radzenBody = document.querySelector(".rz-body");
    radzenBody.scrollTo({
        top: 0,
        left: 0,
        behavior: "instant"
    });
};