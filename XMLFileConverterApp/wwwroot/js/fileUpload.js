$(document).ready(function () {
    let input = document.getElementById("fileUpload");
    let imageName = document.getElementById("file-name")

    input.addEventListener("change", () => {
        let inputImage = document.querySelector("input[type=file]").files[0];
        imageName.innerText = inputImage.name;
    })

    $("#upload-file-btn").on("click", function () {
        var uploadBtn = $(".activate");
        if (!uploadBtn.hasClass('loading')) {
            uploadBtn.addClass('loading');
        }

        uploadFiles('fileUpload');
    });

    function uploadFiles(inputId) {
        var input = document.getElementById(inputId);
        var files = input.files;
        var formData = new FormData();

        if (files.length === 0) {
            $(".activate").removeClass('loading done');
            alert('Please select file.');
            return;
        }

        for (var i = 0; i != files.length; i++) {
            formData.append("fileUpload", files[i]);
        }

        $.ajax(
            {
                url: $("#fileUpload").data("upload-file-url"),
                data: formData,
                processData: false,
                contentType: false,
                type: "POST",
                success: function (partialView) {
                    $("#file-upload-result").html(partialView);
                    if ($("#file-upload-failed").is(":visible")) {
                        $(".activate").removeClass('loading done');
                    }
                    else {
                        $(".activate").addClass('done');
                        setTimeout(function () {
                            $(".activate").removeClass('loading done');
                        }, 3000);
                    }
                },
                error: function () {
                    $("#file-upload-result").text('An error occurred while uploading your file.');
                    $(".activate").removeClass('loading done');
                },
            }
        );
    }
});