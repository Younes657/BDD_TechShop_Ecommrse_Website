
let pp = document.querySelectorAll(".notification");

pp.forEach(function (elem) {
	let id = setTimeout(function () {
		elem.classList.add("notification-2V");
		window.onclick = function () {
			elem.classList.remove("notification-2V")
			clearTimeout(id);
		}
	}, 200)

})
