const city = document.getElementById('city');
function createOrder(event) {
    event.preventDefault();
    var cityName = city.options[city.selectedIndex].text;
    const district = document.getElementById('district');
    var districtName = district.options[district.selectedIndex].text;
    const ward = document.getElementById('ward');
    var wardName = ward.options[ward.selectedIndex].text;
    var address = {
        cityName,
        districtName,

    }
    console.log(address);
    //$.ajax({
    //    url: '/checkout/order/',
    //})

}