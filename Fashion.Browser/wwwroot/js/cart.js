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
                    text: 'OUT OF STOCK',
                    footer: '<a href="/shoppingcart/">VISIT CART</a>'
                })
            }
        }
    });
}

function deleteCart(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You want to remove this product from your cart ?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/delete/' + id,
                type: 'DELETE',
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    saveCartDataToLocalStorage(response);
                    getCartNumber(response.length);
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'SUCCESS !',
                        confirmButtonText: 'OK',
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = '/shoppingcart/';
                        }
                    })
                },
                error: function (response, status, error) {
                    if (response.status === 404) {
                        Swal.fire({
                            icon: 'error',
                            title: error,
                            text: 'CART NOT FOUND',
                            footer: '<a href="/shoppingcart/">VISIT CART</a>'
                        })
                    }

                    Swal.fire({
                        icon: 'error',
                        title: error,
                        footer: '<a href="/shoppingcart/">VISIT CART</a>'
                    })
                }
            });
        }
    })
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