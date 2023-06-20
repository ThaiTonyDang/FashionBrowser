function logoutConfirm(event) {
    event.preventDefault();
    Swal.fire({
        title: 'Are you sure?',
        text: "Do you want to log out ?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, Log out!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/logout/',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'SUCCESS !',
                        confirmButtonText: 'OK',
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = '/users/login/';
                        }
                    })
                },
                error: function (response, status, error) {
                    Swal.fire({
                        icon: 'error',
                        title: error,
                        text: 'Something went wrong! Cannot Logout',
                    })
                }
            });
        }
    })
}