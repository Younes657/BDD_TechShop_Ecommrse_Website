
$(document).ready(function () {
    loadData();
});

function loadData() {
    $.ajax({
        url: '/Admin/User/GetUsers', 
        type: 'GET',
        success: function (response) {
            let action = document.getElementsByClassName("lock")
            let ids = document.getElementsByClassName("specific")
            let array = []
            Array.from(ids).forEach(function (elm) {
                Array.from(response).forEach(function (elm1) {
                    if (elm.innerText == elm1["id"]) {
                        let datenow = new Date().getTime();
                        let lookout = new Date(elm1["lockoutEnd"])
                        
                        if (lookout > datenow) {
                            array.push(1)
                        } else {
                            array.push(0);
                        }
                    }
                })
                for (let i = 0; i < array.length; i++) {
                    if (array[i] == 1) {
                        action[i].innerHTML = `<i class="fa-solid fa-lock fa-lg"></i>`
                    } else {
                        action[i].innerHTML = `<i class="fa-solid fa-lock-open fa-lg"></i>`
                    }
                }
            })
            
            console.log(response);
        },
        error: function (error) {
            console.log(error);
        }
    });
}
