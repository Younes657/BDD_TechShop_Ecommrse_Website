
let PlaceOrderbtn = document.getElementById("Order-btn");
let formOrder = document.getElementById("Order-form")
PlaceOrderbtn.onclick = function (e) {
    e.preventDefault();
    Validate();
}

function Validate() {
    Swal.fire({
        title: "Payment Functionality",
        text: `The payment function is not yet set, by clicking the Yes button, 
        your order will be considered paid. For Testing Purpose`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, I understand"
    }).then((result) => {
        if (result.isConfirmed) {
            formOrder.submit();
        }
    });
}