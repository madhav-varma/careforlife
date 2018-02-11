$(document).ready(function () {

    $(document).on("click", "#cancel", function () {
        location.reload();
    });

    $("#image").fileinput({
        showUpload: false,
        showPreview: false,
        showRemove: false,
        browseOnZoneClick: true,
        allowedFileTypes: ["image"],
        maxFileSize: 2048,
        maxFileCount: 1,
        msgSizeTooLarge: "File larger than 2MB not allowed.",
        msgFilesTooMany: "Only 1 file is allowed.",
        msgValidationError: "<span class='text-danger'><i class='glyphicon glyphicon-exclamation-sign'></i> File Upload Error</span>",
        hiddenThumbnailContent: true
    });

    $('#image').on('fileerror', function (event, data, msg) {
        alert(msg);
    });

    changeSwitchery($("#is_rare"), false);
    $("#MainContent_chk_rare").prop("checked", false);

    Dropzone.autoDiscover = false;

    $('#spllist_table').DataTable({
        lengthMenu: [[10, 25, 50], [10, 25, 50]],
        "language":
        {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "Speciality.aspx/GetSpecialities",
            "contentType": "application/json",
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                return JSON.stringify({ "model": d });
            },
            "dataSrc": function (json) {
                json.draw = json.d.draw;
                json.recordsTotal = json.d.recordsTotal;
                json.recordsFiltered = json.d.recordsFiltered;
                json.data = json.d.data;
                var return_data = json;
                return return_data.data;
            },

        },
        //"order": [[0, "desc"]],
        "columns": [
            { "data": "Name", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "IsRareString", "autoWidth": true, "orderable": true, "searchable": true },
            
            { "data": "Link", "autoWidth": true, "orderable": false, "searchable": false }
        ],
        responsive: true,
        "pagingType": "full_numbers"
    });
    $('#spllist_table tfoot th').each(function () {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" style="width:100%" placeholder="Search ' + title + '" />');
        }
    });

    var table = $('#spllist_table').DataTable();

    $(document).on('click', '.delete-spl', function () {

        if (confirm('Are you sure, you want to delete this item ?')) {
            var id = $(this).data('id');

            $.ajax({
                "url": "Speciality.aspx/DeleteSpecialityById?id=" + id,
                "contentType": "application/json",
                "type": "GET",
                "dataType": "JSON",
                "success": function (data) {
                    if (data.d.IsSuccess) {
                        table.ajax.reload();
                    }
                    else {
                        alert(data.d.Message);
                    }
                }
            });
        }
    });

    var changeCheckbox = document.querySelector('.js-switch');

    changeCheckbox.onchange = function () {
        var chk = changeCheckbox.checked;
        $("#MainContent_chk_rare").prop("checked", chk);
    };

    $(document).on('click', '.edit-spl', function () {
        var id = $(this).data('id');

        $.ajax({
            "url": "Speciality.aspx/GetSpecialityById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                var spl = data.d;

                $("#MainContent_speciality_id").val(spl.Id);
                $("#MainContent_name").val(spl.Name);                
                changeSwitchery($("#is_rare"), spl.IsRare);
                $("#MainContent_chk_rare").prop("checked", spl.IsRare);

                $("#image-container").empty().append("<a class='add-spl-images form-group btn btn-primary' data-id='" + spl.Id + "'>Update Image</a>");

                $("label.error").hide();               
            }
        });

        $("#spllistli").removeClass("active");
        $("#spllist").removeClass("active");
        $("#spleditli").addClass("active");
        $("#spledit").addClass("active");
    });

    $(document).on('click', '.add-spl-images', function () {
        var id = $(this).data('id');
        $("#spl_id").val(id);

        $.ajax({
            "url": "Speciality.aspx/GetImageById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                if (data.d.IsSuccess) {
                    var files = data.d.Data;
                    var myDropzone = new Dropzone("#my-dropzone", {
                        autoProcessQueue: false,
                        url: "Handlers/SpecialityImageUploader.ashx",
                        addRemoveLinks: true,
                        uploadMultiple: true,
                        maxFiles: 1,
                        parallelUploads: 1,
                        paramName: "file", // The name that will be used to transfer the file
                        maxFilesize: 2, // MB
                        success: function (file, data) {
                            var response = JSON.parse(data);
                            if (!response.IsSuccess) {
                                alert(response.Message);
                                this.defaultOptions.error(file, response.Message);
                            }
                        },
                        error: function (file, error) {
                            var msg = file.accepted ? "File not uploaded" : error;
                            var msgEl = $(file.previewElement).find('.dz-error-message');
                            msgEl.text(msg);
                            msgEl.show();
                            msgEl.css("opacity", 1);
                        },
                        accept: function (file, done) {
                            if (file.name === "justinbieber.jpg") {
                                done("Naha, you don't.");
                            }
                            else { done(); }
                        },
                        init: function () {
                            this.on("sending", function (file, xhr, data) {
                                var splid = $("#spl_id").val();
                                data.append("id", splid);
                                var files = this.files;
                                var rejectedFiles = this.getRejectedFiles();
                                if (rejectedFiles.length > 0) {
                                    $.each(rejectedFiles, function (i, f) {
                                        files.splice(files.indexOf(f), 1);
                                    });
                                }
                                var fcnames = [];
                                $.each(files, function (i, file) {
                                    fcnames.push(splid + "_" + file.name.replace(/\s+/g, ""));
                                });

                                if (fcnames.length > 0) {
                                    data.append("fnames", fcnames.join(" "));
                                }
                            });
                        }
                    });

                    myDropzone.removeAllFiles();
                    if (files.length > 0) {
                        for (var i = 0; i < files.length; i++) {
                            if (files[i]) {
                                var mock = {
                                    name: files[i].Name,
                                    size: parseInt(files[i].Size),
                                    type: 'image/jpeg',
                                    status: Dropzone.ADDED,
                                    url: "photo/" + files[i].Name
                                };

                                mock.accepted = true;
                                myDropzone.files.push(mock);
                                myDropzone.emit('addedfile', mock);
                                myDropzone.createThumbnailFromUrl(mock, mock.url);
                                myDropzone.emit('complete', mock);
                            }
                        }
                    }

                    $('#uploadImages').click(function () {
                        // myDropzone.options.url = "Doctor.aspx/UploadImagesById?id=" + id;
                        if (myDropzone.getQueuedFiles().length > 0) {
                            myDropzone.processQueue();
                        }
                        else {
                            // Upload anyway without files
                            var files = myDropzone.files;
                            var rejectedFiles = myDropzone.getRejectedFiles();
                            if (rejectedFiles.length > 0) {
                                $.each(rejectedFiles, function (i, f) {
                                    files.splice(files.indexOf(f), 1);
                                });
                            }
                            if (files.length > 0)
                                myDropzone.uploadFiles(myDropzone.files);
                        }
                    });

                    $("#exampleModal").modal("show");
                }

                else {
                    alert(data.d.Message);
                }
            }
        });


    });

    table.columns().every(function () {
        var that = this;

        $('input', this.footer()).on('keyup change', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });
    $(document).on("click", "#saveSPL", function () {
        var valid = $("#splform").valid();
        if (valid) {
            $("#MainContent_sendSPL").trigger("click");
        }
    });
    function changeSwitchery(element, checked) {
        if ((element.is(':checked') && checked == false) || (!element.is(':checked') && checked == true)) {
            element.parent().find('.switchery').trigger('click');
        }
    }
});

