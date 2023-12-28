
let pp = document.querySelector(".notification");
let id = setTimeout(function () {
	pp.classList.add("notification-2V");
	window.onclick = function () {
		pp.classList.remove("notification-2V")
		clearTimeout(id);
	}
}, 200)
