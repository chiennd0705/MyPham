//
//	jQuery Validate example script
//
//	Prepared by David Cochran
//
//	Free for your use -- No warranties, no guarantees!
//
$(document).ready(function () {
    // Validate
    // http://bassistance.de/jquery-plugins/jquery-plugin-validation/
    // http://docs.jquery.com/Plugins/Validation/
    // http://docs.jquery.com/Plugins/Validation/validate#toptions
    $('#myform').validate({
        rules: {
            username: {
                minlength: 2,
                required: true
            },
            password: {
                required: true,
                minlength: 6,
            },
            cpassword: {
                equalTo: "#password",
                required: true,
                minlength: 6,
            },
            email: {
                required: true,
                email: true
            },
            subject: {
                minlength: 2,
                required: true
            },
            message: {
                minlength: 2,
                required: true
            }
        },
        highlight: function (element) {
            $(element).closest('.control-group').removeClass('success').addClass('error');
        },
        success: function (element) {
            element
                .text('').addClass('valid')
                .closest('.control-group').removeClass('error').addClass('success');
        }
    });
}); // end document.ready