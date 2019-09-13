var allowLetters = /^[A-Za-z0-9_@@.-]+$/;
var allowLettersFullName = /^[A-Za-z0-9\s\S]+$/;
var invalidChar = /^[^'"<>]*$/;
var emailPattern = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,4}$/i);
var emailPattern2 = new RegExp(/^[+a-zA-Z0-9._-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,12}.[a-zA-Z]{2,4}$/i);

$(function () {
    $("#Email").focus();
    $("#Email").change(function () {
        validateForm();
    });
    //$("#Password").change(function () {
    //    validateForm();
    //});
    //$("#ConfirmPassword").change(function () {
    //    validateForm();
    //});
    $("#FullName").change(function () {
        validateForm();
    });
    $("#MobileNumber").change(function () {
        validateForm();
    });
    $("#Phone").change(function () {
        validateForm();
    });
    $("#Subcribe_phone").change(function () {
        validateForm();
    });
    $("#VerifyCode").change(function () {
        validateForm();
    });
    $("#Token").change(function () {
        validateForm();
    });
    $("#Captcha").change(function () {
        validateForm();
    });
    $("#EmailOnly").change(function () {
        validateForm();
    });
});

function validateForm() {
    var strErrors = "";
    var email = $("#Email").val();
    //var password = $("#Password").val();
    //var confirmPassword = $("#ConfirmPassword").val();
    var fullName = $("#FullName").val();
    var mobile = $("#MobileNumber").val();
    var phone = $("#Phone").val();
    var subPhone = $("#Subcribe_phone").val();
    //var verifyCode = $("#VerifyCode").val();
    var captcha = $("#Captcha").val();
    var emailOnly = $("#EmailOnly").val();
    var token = $("#Token").val();
    if (email != undefined) {
        if (email == "") {
            strErrors += "Địa chỉ Email/Số điện thoại không được để trống!\n";
        } else {
            if (email.indexOf(" ") > -1) {
                strErrors += "Địa chỉ Email/Số điện thoại không được có khoảng trắng!\n";
            }
            var number = tryParseInt(email);
            if (number != null) {
                if (email.length > 15) {
                    strErrors += "Số điện thoại quá dài!\n";
                } else if (email.length < 10) {
                    strErrors += "Số điện thoại không được ít hơn 10 số!\n";
                }
            } else if (email.indexOf("@") > 0 && email.length > 250) {
                strErrors += "Địa chỉ Email không được phép dài quá 250 ký tự!\n";
            }
            else if (!email.match(allowLetters)) {
                strErrors += "Không được nhập kí tự đặc biệt!\n";
            } else if (!email.match(emailPattern) && !email.match(emailPattern2)) {
                strErrors = "Địa chỉ Email không đúng định dạng!\n";
            }
        }
    }
    //if (password != undefined) {
    //    if (password == "") {
    //        strErrors += "Mật khẩu không được để trống!\n";
    //    } else if (password.length < 6) {
    //        strErrors += "Mật khẩu ít nhất từ 6 ký tự!\n";
    //    } else if (password.length > 30) {
    //        strErrors += "Mật khẩu được phép tối đa 30 ký tự!\n";
    //    }
    //}
    //if (confirmPassword != undefined) {
    //    if (confirmPassword == "") {
    //        strErrors += "Nhập lại mật khẩu không được để trống!\n";
    //    } else if (confirmPassword != $("#Password").val()) {
    //        strErrors += "Mật khẩu và Nhập lại mật khẩu không khớp!\n";
    //    }
    //}
    if (fullName != undefined) {
        fullName = fullName.replace(/\s/g, "");
        if (fullName == "") {
            strErrors += "Tên hiển thị không được để trống!\n";
        } else if (fullName.length > 25) {
            strErrors += "Tên hiển thị không được vượt quá 25 kí tự!\n";
        }
        else if (!fullName.match(allowLettersFullName)) {
            strErrors += "Không được nhập kí tự đặc biệt!\n";
        }
    }
    if (mobile != undefined) {
        if (mobile.indexOf(" ") > -1) {
            strErrors += "Số điện thoại không được có khoảng trắng!\n";
        }
        else if (mobile != "" && !mobile.match(allowLetters)) {
            strErrors += "Không được nhập kí tự đặc biệt!\n";
        } else if (mobile != "") {
            var number = tryParseInt(mobile);
            if (number == null || number == undefined) {
                strErrors += "Số điện thoại không đúng định dạng!\n";
            } else if (mobile.length < 10) {
                strErrors += "Số điện thoại không được ít hơn 10 số!\n";
            } else if (mobile.length > 15) {
                strErrors += "Số điện thoại quá dài!\n";
            }
        }
    }
    if (phone != undefined) {
        if (phone == "") {
            strErrors += "Số điện thoại không được để trống!\n";
        } else {
            var number = tryParseInt(phone);
            if (phone.indexOf(" ") > -1) {
                strErrors += "Số điện thoại không được có khoảng trắng!\n";
            } else if (!phone.match(allowLetters)) {
                strErrors += "Không được nhập kí tự đặc biệt!\n";
            } else if (number == null || number == undefined) {
                strErrors += "Số điện thoại không đúng định dạng!\n";
            } else if (phone.length < 10) {
                strErrors += "Số điện thoại không được ít hơn 10 số!\n";
            } else if (phone.length > 15) {
                strErrors += "Số điện thoại quá dài!\n";
            }
        }
    }
    if (subPhone != undefined) {
        if (subPhone == "") {
            strErrors += "Số điện thoại không được để trống!\n";
        } else if (!subPhone.match(allowLetters)) {
            strErrors += "Không được nhập kí tự đặc biệt!\n";
        } else {
            var number = tryParseInt(subPhone);
            if (number == null || number == undefined) {
                strErrors += "Số điện thoại không đúng định dạng!\n";
            } else if (subPhone.length < 10) {
                strErrors += "Số điện thoại không được ít hơn 10 số!\n";
            } else if (subPhone.length > 15) {
                strErrors += "Số điện thoại quá dài!\n";
            }
        }
    }
    //if (verifyCode != undefined) {
    //    if (verifyCode == "") {
    //        strErrors += "Mã bảo mật không được để trống!\n";
    //        $("#VerifyCode").val("");
    //        $("#VerifyCode").focus();
    //    } else if (!verifyCode.match(allowLetters)) {
    //        strErrors += "Không được nhập kí tự đặc biệt!\n";
    //        $("#VerifyCode").val("");
    //        $("#VerifyCode").focus();
    //    }
    //}
    if (token != undefined) {
        if (token == "") {
            strErrors += "Mã bảo mật không được để trống!\n";
            $("#Token").val("");
            $("#Token").focus();
        } else if (!token.match(allowLetters)) {
            strErrors += "Không được nhập kí tự đặc biệt!\n";
            $("#Token").val("");
            $("#Token").focus();
        }
        //else
        //    {
        //    var number = tryParseInt(token);
        //    if (number == null || number == undefined) {
        //        strErrors += "Mã bảo mật phải là dạng số!\n";
        //    }
        //    else if (token.length != 6) {
        //        strErrors += "Mã bảo mật phải có 6 số!\n";
        //    }
        //}
    }
    if (captcha != undefined) {
        if (captcha == "") {
            strErrors += "Captcha không được để trống!\n";
        } else {
            var number = tryParseInt(captcha);
            if (number == null || number == undefined) {
                strErrors += "Captcha không chính xác!\n";
            }
        }
    }
    if (emailOnly != undefined) {
        if (emailOnly == "") {
            strErrors += "Địa chỉ Email không được để trống!\n";
        } else {
            if (emailOnly.indexOf(" ") > -1) {
                strErrors += "Địa chỉ Email không được có khoảng trắng!\n";
            } else if (!emailOnly.match(allowLetters)) {
                strErrors += "Không được nhập kí tự đặc biệt!\n";
            } else if (!emailOnly.match(emailPattern) && !emailOnly.match(emailPattern2)) {
                strErrors = "Địa chỉ Email không đúng định dạng!\n";
            }
        }
    }

    if (strErrors != "") {
        showErrorMessage(strErrors);
        return false;
    } else {
        clearErrorMessage();
        return true;
    }
}

function showErrorMessage(str) {
    clearErrorMessage();
    $(".validation-summary-errors").hide();
    var errs = str.split("\n");
    var mess = "<ul>";
    for (var i = 0; i < errs.length; i++) {
        if (errs[i] != "") {
            mess += "<li>" + errs[i] + "</li>";
        }
    }
    mess += "</ul>";
    $("#divErrors").html(mess);
    $("#divErrors").show();
    $("button[type=submit]").attr("disabled", true);
}

function clearErrorMessage() {
    $("#divErrors").html("");
    $("#divErrors").hide();
    $("button[type=submit]").attr("disabled", false);
}

function tryParseInt(str, defaultValue) {
    var retValue = defaultValue;
    if (str != null) {
        if (str.length > 0) {
            if (!isNaN(str)) {
                retValue = parseInt(str);
            }
        }
    }
    return retValue;
}