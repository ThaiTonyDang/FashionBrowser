const OPERATORS = {
    SUBTRACT: "SUB",
    ADDITION: "ADD"
};

function addToCart(id, e) {
    e.preventDefault();
    let quantityInput = 1;
    $.ajax({
        url: '/cart/addtocart/' + id + '/' + quantityInput,
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

            if (response.status === 401) {
                window.location.href = '/users/login';
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

$(".items-count").on("click", function () {
    let id = $(this).data('value');
    let productPrice = $('#cartitem-price-' + id).val();
    let operator = $(this).val();
    let quantity = getQuantityAdjustment(operator, id);
    $.ajax({
        url: '/adjustquantity/' + id + '/' + operator,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            let price = quantity * productPrice;

            let format = getPriceFormat(price);
            $('#item-price-' + id).html(format);

            saveCartDataToLocalStorage(response);       

            var totalPrice = response.reduce(function (accumulator, current) {
                return accumulator + current.product.price * current.quantity;
            }, 0);

            $('#total-price').html(getPriceFormat(totalPrice));
            $('.cart-total-price').html(getPriceFormat(totalPrice + 10000));
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
            if (response.status === 401) {
                var name = response.productItemViewModel.name;
                Swal.fire({
                    icon: 'error',
                    title: error,
                    text: 'ID OF PRODUCTs' + name + 'WRONG',
                    footer: '<a href="/shoppingcart/">VISIT CART</a>'
                })
            }
            Swal.fire({
                icon: 'error',
                title: error,
                text: 'SERVER ERROR',
                footer: '<a href="/shoppingcart/">VISIT CART</a>'
            })
        }
    });
});

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

function getPriceFormat(price) {
    return new Intl.NumberFormat(
        'it-IT',
        {
            style: 'currency',
            currency: 'VND' 
        }
    ).format(price);
}

function getQuantityAdjustment(operator, productId) {
    var result = $('#sst-' + productId);
    var sst = result.val();
    switch (operator) {
        case OPERATORS.ADDITION:
            if (!isNaN(sst)) {
                sst++;
                result.val(sst);
            } 
            return sst;
        default:
            if (!isNaN(sst) && sst!=1) {
                sst--;
                result.val(sst);
                return sst;
            }
            if (sst <= 1) {
                result.val(1);
                deleteCart(productId);
                return 1;       
            }
    }
}

function getTotalPrice(data) {
    var totalPrice = data.reduce(function (accumulator, current) {
        return accumulator + current.product.price;
    }, 0);
}


function addProductToCart(id, e) {
    e.preventDefault();
    let quantityInput = $('.input-text').val();
    $.ajax({
        url: '/cart/addtocart/' + id + '/' + quantityInput,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            let data = response;
            saveCartDataToLocalStorage(data);
            let quantity = data.length;
            getCartNumber(quantity);
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

            if (response.status === 401) {
                window.location.href = '/users/login';
            }
        }
    });
}