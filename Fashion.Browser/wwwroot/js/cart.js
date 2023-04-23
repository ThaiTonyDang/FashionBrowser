$(document).on("click", "#closemodal", function () {
    $('#confirm-modal').hide();
})

function addToCart(id, e) {
    e.preventDefault();
    $.ajax({
        url: '/cart/addtocart/' + id,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            let data = response;  
            saveCartDataToLocalStorage(data);
            let quantity = data.length;
            getCartNumber(quantity);
            checkMark(id);
            Swal.fire({
                icon: 'success',
                title: 'SUCCESS..!!',
                text: 'ADD THIS PRODUCT SUCCESS',
                footer: '<a href="/shoppingcart/">VISIT CART</a>'
            })
        },
        error: function (response, status, error) {
            if (response.status === 400) {
                Swal.fire({
                    icon: 'error',
                    title: error,
                    text: 'Something went wrong!',
                    footer: '<a href="/shoppingcart/">VISIT CART</a>'
                })
            }
        }
    });
}

function checkMark(id) {
    document.getElementById("bi-fill-visibility-" + id).style.visibility = "visible";
    document.getElementById("bi-fill-visibility-" + id).style.opacity = "1";
    document.getElementById("bi-fill-visibility-" + id).style.color = "forestgreen";
}

function getCartNumber(quantity) {
    let cartElement = document.getElementById("cart__item-number");
    if (cartElement) cartElement.innerText = parseInt(quantity);
}

function saveCartDataToLocalStorage(data) {
    localStorage.setItem("localProductData", JSON.stringify(data));
}

function getDataCartFromStorage() {
    let dataLocal = localStorage.getItem("localProductData");
    if (dataLocal) return JSON.parse(dataLocal);
    return dataLocal;
}