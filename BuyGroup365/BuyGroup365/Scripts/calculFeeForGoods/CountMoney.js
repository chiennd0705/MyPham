﻿var cal = {
    //thuoc tinh km chuyen hang cua cal
    'kmRoadToTranspot': 0,
    'totalMoneyHang': 0,
    'gramHang': 0,
    'feeTranspotGood': 0,
    'selectionGramGood': 2,
};
function checkLocation(reciverAddress) {
    if (reciverAddress == "Hà Nội") {
        return "Nội thành HN";
    }
    return "Liên tỉnh";
}
function calFee(kmRoad, gramGoods, location) {

    if (location == "Nội thành HN") {
        if (gramGoods <= 5000) {
            return 15000;
        }
        else if (gramGoods > 5000) {
            return ((gramGoods - 5000) / 500) * 5 + 15;
        }
    }
    else if (location == "Nội thành HCM") {
        if (gramGoods < 2000) {
            return 20000;
        }
        else if (gramGoods > 5000) {
            return ((gramGoods - 5000) / 500) * 5 + 20;
        }
    }
        //Tinh phi chuyen hang lien tinh
    else {
        if (gramGoods <= 250) {
            if (kmRoad < 100) {
                return 21780;
            }
            else if (kmRoad >= 100 && kmRoad < 300) {
                return 24024;
            }
            else {
                return 30360;
            }
        }
        else if (gramGoods > 250 && gramGoods <= 500) {
            if (kmRoad < 100) {
                return 31548;
            }
            else if (kmRoad >= 100 && kmRoad < 300) {
                return 33396;
            }
            else {
                return 39468;
            }
        }
        else if (gramGoods > 500 && gramGoods <= 1000) {
            if (kmRoad < 100) {
                return 43824;
            }
            else if (kmRoad >= 100 && kmRoad < 300) {
                return 44880;
            }
            else {
                return 57684;
            }
        }
        else if (gramGoods > 1000 && gramGoods <= 1500) {
            if (kmRoad < 100) {
                return 52.800;
            }
            else if (kmRoad >= 100 && kmRoad < 300) {
                return 55176;
            }
            else {
                return 74448;
            }
        }
        else if (gramGoods > 1500 && gramGoods <= 2000) {
            if (kmRoad < 100) {
                return 63888;
            }
            else if (kmRoad >= 100 && kmRoad < 300) {
                return 68244;
            }
            else {
                return 90420;
            }
        }
        else if (gramGoods > 2000 && gramGoods <= 5000) {
            return 0;
        }
        else {
            if (kmRoad < 100) {
                return 63888 + ((gramGoods - 5000) / 500) * 4620;
            }
            else if (kmRoad >= 100 && kmRoad < 300) {
                return 68244 + ((gramGoods - 5000) / 500) * 5676;
            }
            else {
                return 90420 + ((gramGoods - 5000) / 500) * 11220;
            }
        }
    }
}

function showLocation(loc1, loc2) {

    geocoder.getLocations(loc1, function (response) {
        if (!response || response.Status.code != 200) {
            alert("Sorry, we were unable to geocode the first address");
        }
        else {
            location1 = { lat: response.Placemark[0].Point.coordinates[1], lon: response.Placemark[0].Point.coordinates[0], address: response.Placemark[0].address };
            geocoder.getLocations(loc2, function (response) {
                if (!response || response.Status.code != 200) {
                    alert("Sorry, we were unable to geocode the second address");
                }
                else {
                    location2 = { lat: response.Placemark[0].Point.coordinates[1], lon: response.Placemark[0].Point.coordinates[0], address: response.Placemark[0].address };
                    calculateDistance();

                }
            });
        }
    });
}

function calculateDistance() {
    try {
        var glatlng1 = new GLatLng(location1.lat, location1.lon);
        var glatlng2 = new GLatLng(location2.lat, location2.lon);
        var miledistance = glatlng1.distanceFrom(glatlng2, 3959).toFixed(1);
        var kmdistance = (miledistance * 1.609344).toFixed(1);

        var a = '<strong>Địa chỉ 1: </strong>' + location1.address + '<br /><strong>Địa chỉ 2: </strong>' + location2.address + '<br /><strong>Khoảng cánh: </strong>' + miledistance + ' miles (or ' + kmdistance + ' km)';
        //   $('#results').append(a);
        $('#distanceRoad').val(kmdistance);
    }
    catch (error) {
        alert(error);
    }
}
function formatNumber(num) {
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
}
function Onpay() {
    //if (!ValidilationFrom())
    //    return false;
    $('#overbackgroud').show();
    var recieveOrder = [];
    var buyerOrder = [];
    var TaxOrder = [];
    var km = "0";
    var shopid = "0";
    var payFrom = "1";
    
    //if (payFrom == null) {
    //    alert("Vui lòng chọn hình thức thanh toán!");
    //    $('#overbackgroud').hide();
    //} else {
        var note = $('#NoteByCustomerShipping').val();
 
            recieveOrder.push($('#FullName').val());
            recieveOrder.push($('#Mobile').val());
            recieveOrder.push($('#Email').val());
            recieveOrder.push($('#DeliveryAddressName').val());
            var detailadress1 = $('#DeliveryAddressName').val() + ", " + $("#receiverPhuong option:selected").text() + ", " + $("#receiverTown option:selected").text() + ", " + $("#Location option:selected").text();
            recieveOrder.push(detailadress1);
           // alert($("#receiverPhuong option:selected").val());
            recieveOrder.push($("#receiverPhuong option:selected").val());
            buyerOrder.push($('#FullName').val());
            buyerOrder.push($('#Mobile').val());
            buyerOrder.push($('#Email').val());
            var adress = $('#Address').val();
            buyerOrder.push(adress);

            TaxOrder.push($('#InvoiceName').val());
            TaxOrder.push($('#BillingAddress').val());
            TaxOrder.push($('#InvoiceTaxCode').val());


        km = $("#distanceRoad").val();
        shopid = 0;//$("#shopid").val();


        $.ajax({
            url: '/PayGoods/PaymentCheckOut',
            data: {
                'recieveOrder': JSON.stringify(recieveOrder),
                'buyerOrder': JSON.stringify(buyerOrder),
                'TaxOrder': JSON.stringify(TaxOrder),
                'km': km,
                'note': note,
                'shopid': shopid,
                'payfrom': payFrom
            },
            Type: 'POST',
            dataType: 'json', contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //$('#overbackgroud').hide();
              //  alert("Chúng tôi đã gửi đơn hàng vào email " + recieveOrder[2] + "");
               // window.location.href = "/Notification/OrderContent?od=" + data;
                if (data == 1) {
                    window.location.href = "/Notification/Success";
                } else if (data == 2) {
                    window.location.href = "/Notification/Error";
                } else {
                    window.location.href = data;
                }
            },
            error: function () {
                alert('Hệ thống đang cập nhật vui lòng thực hiện lại sau');
                $('#overbackgroud').hide();
            }

        });
  //  }
   
}

function OnpayCard() {
    if (!ValidilationFrom())
        return false;
    $('#overbackgroud').show();
    var recieveOrder = [];
    var buyerOrder = [];
    var TaxOrder = [];
    var km = "0";
    var shopid = "0";
    var payFrom = "1";

    //if (payFrom == null) {
    //    alert("Vui lòng chọn hình thức thanh toán!");
    //    $('#overbackgroud').hide();
    //} else {
    var note = $('#NoteByCustomerShipping').val();

    recieveOrder.push($('#FullName').val());
    recieveOrder.push($('#Mobile').val());
    recieveOrder.push($('#Email').val());
    recieveOrder.push($('#DeliveryAddressName').val());
    var detailadress1 = $('#DeliveryAddressName').val() + ", " + $("#receiverPhuong option:selected").text() + ", " + $("#receiverTown option:selected").text() + ", " + $("#Location option:selected").text();
    recieveOrder.push(detailadress1);
    // alert($("#receiverPhuong option:selected").val());
    recieveOrder.push($("#receiverPhuong option:selected").val());
    buyerOrder.push($('#FullName').val());
    buyerOrder.push($('#Mobile').val());
    buyerOrder.push($('#Email').val());
    var adress = $('#Address').val();
    buyerOrder.push(adress);

    TaxOrder.push($('#InvoiceName').val());
    TaxOrder.push($('#BillingAddress').val());
    TaxOrder.push($('#InvoiceTaxCode').val());


    km = $("#distanceRoad").val();
    shopid = 0;//$("#shopid").val();


    $.ajax({
        url: '/PayGoods/PaymentCheckOut',
        data: {
            'recieveOrder': JSON.stringify(recieveOrder),
            'buyerOrder': JSON.stringify(buyerOrder),
            'TaxOrder': JSON.stringify(TaxOrder),
            'km': km,
            'note': note,
            'shopid': shopid,
            'payfrom': payFrom
        },
        Type: 'POST',
        dataType: 'json', contentType: 'application/json; charset=utf-8',
        success: function (data) {
            //$('#overbackgroud').hide();
            //  alert("Chúng tôi đã gửi đơn hàng vào email " + recieveOrder[2] + "");
            // window.location.href = "/Notification/OrderContent?od=" + data;
            if (data == 1) {
                window.location.href = "/PayGoods/PaymentCard";
            } else if (data == 2) {
                window.location.href = "/Notification/Error";
            } else {
                window.location.href = data;
            }
        },
        error: function () {
            alert('Hệ thống đang cập nhật vui lòng thực hiện lại sau');
            $('#overbackgroud').hide();
        }

    });
    //  }

}
function ValidilationFrom() {
    if ($('#FullName').val() == "") {
        alert("Nhập họ tên khách hàng");
        $('#FullName').focus();
        return false;
    }
    else
    if ($('#Mobile').val() == "") {
        alert("Nhập số điện thoại nhận hàng");
        $('#DeliveryMobile').focus();
        return false;
    }
    else
        if (!CheckPhone()) {
            return CheckPhone();
        }
        else if ($('#Email').val() == "") {
            alert("Nhập mail nhận hàng");
            $('#DeliveryEmail').focus();
            return false;
        }
        else if (!CheckMail()) {
            return CheckMail()
        }
        else if ($('#Location option:selected').val() == "-1") {
            alert("Nhập tỉnh thành");
            $('#Location').focus();
            return false;
        } else if ($('#receiverTown option:selected').val() == "-1") {
            alert("Nhập quận huyện");
            $('#receiverTown').focus();
            return false;
        }

        else if ($('#DeliveryAddressName').val() == "") {
            alert("Nhập địa chỉ chi tiết");
            $('#DeliveryAddressName').focus();
            return false;
        } else {
            return true;

        }
}
function CheckPhone() {
 
    var vl = $('#Mobile').val();
   
    if (validatePhone(vl)==false) {
        alert("Số điện thoại không hợp lệ");
        $('#Mobile').focus();
        return false;
    }
    else
        return true;
}
function CheckMail() {
   
    var vl = $('#Email').val();
    if (ValidEmailAddress(vl) == false) {
        alert("Mail không hợp lệ");
        $('#Email').focus();
        return false;
    }
    else
        return true;
}
function validatePhone(value) {
    var filter = /^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$/;
    if (filter.test(value)) {
        return true;
    }
    else {
        return false;
    }
}
function ValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
}