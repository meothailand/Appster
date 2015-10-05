
var modifyUser = {
	//binding event
	init: function () {
		$('#saveUser').unbind('click').click(function () {
			handler.showAjaxModal("Updating user! Please wait!");
			$(this).delay(300, function () {
				handler.hideAjaxModal();
			});
			return false;
		});
	}
};


//When the server is ready..

    $ = jQuery.noConflict();

    $(document).ready(function () {
        $("#profileImage").on("change", function () {
            var files = !!this.files ? this.files : [];
            if (!files.length || !window.FileReader) return; // no file selected, or no FileReader support


            if (/^image/.test(files[0].type)) { // only image file
                var reader = new FileReader(); // instance of the FileReader
                reader.readAsDataURL(files[0]); // read the local file

                reader.onloadend = function () { // set image data as background of div
                    jobImage = this.result;

                    //$('#img_div_pre').show();
                    $("#profile_img").attr("src", jobImage);

                }
            }
        });

        //code for the date picker
        $('#dob').datepicker({
            dateFormat: "dd/mm/yy",
            yearRange: '1950:' + new Date().getFullYear(),
            changeMonth: true,
            changeYear: true,
        });

        modifyUser.init();
    });
