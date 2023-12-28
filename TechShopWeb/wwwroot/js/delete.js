
let form = document.getElementById("aform");
console.log(form);
function submitMethode(e) {
    console.log("aaaa");
    var id = e.target.parentElement.dataset.id;
    e.preventDefault();
    document.querySelector('#aform input[name="id"]').value = id;
    Validate();
}
let btn = document.getElementById("btnOut");
console.log(btn);
//btn.addEventListener("click", submitMethode(e))

function Validate(id) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            form.submit();
        }
    });
}