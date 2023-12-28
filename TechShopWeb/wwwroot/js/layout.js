let listHead = document.querySelector(".list-head");
let navBar = document.querySelector("header .container .left .navBar");
let navBarelm = document.querySelectorAll("header .container .left .navBar > li");
//for the active class
//navBar.onclick = function (e) {
//    console.log(e.target)
//    console.log(e.target.parentElement)
//    navBarelm.forEach(element => {
//        element.classList.remove("Active")
//    });
//    navBarelm.forEach(element => {
//        if (e.target === element || e.target.parentElement === element) {
//            element.classList.add("Active")
//        }
//    })
//}

if (sessionStorage.getItem("Active")) {
    navBarelm.forEach(element => {
        element.classList.remove("Active");
    });
    document.querySelector(`header .container .left .navBar [data-src = ${sessionStorage.getItem("Active")}`)
        .classList.add("Active");
}

navBarelm.forEach((val) => {
    val.onclick = function (e) {

        navBarelm.forEach(element => {
            element.classList.remove("Active");
        });
        val.classList.add("Active");
        sessionStorage.setItem("Active", val.dataset.src);
    }
})
//end
console.log(listHead);
console.log(navBar);
listHead.onclick = function () {
    console.log("clicked");
    console.log(navBar.classList)
    navBar.classList.toggle("toggle");
}
// for the scroll button
let btnScroll = document.querySelector(".toThetop")
console.log(btnScroll)
window.onscroll = function () {
    // console.log(document.body.scrollTop) why it is 0 always fuck it my mind is sick because of it
    // console.log(window.scrollY)
    if (window.scrollY > 100) {
        btnScroll.style.display = "flex";
    } else {
        btnScroll.style.display = "none"
    }
}