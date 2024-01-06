//let selectList = document.getElementById("selectList");
//let SelectedOption = selectList.options[selectList.selectedIndex];
//let tracking = document.getElementById("track");
//console.log(SelectedOption)
//console.log(SelectedOption.disabled);
//console.log(tracking.value == "" || tracking.value == 0)
function ValidateShip() {
    let selectList = document.getElementById("selectList");
    let tracking = document.getElementById("track");
    console.log(selectList)
    console.log(tracking);
    let SelectedOption = selectList.options[selectList.selectedIndex];
    console.log(SelectedOption)
    console.log(SelectedOption.disabled);
    if (SelectedOption.disabled) {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "You should Choose One of the shipper",
            // footer: '<a href="#">Why do I have this issue?</a>'
        });
        return false;
    }

    if (tracking.value == 0 || tracking.value == "") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: "You should enter the Tracking Number !!",
            // footer: '<a href="#">Why do I have this issue?</a>'
        });
        return false;
    }
    return true;

}
