﻿$(document).ready(function () {
    $(document).on("click", "#cancel", function () {
        location.reload();
    });

    $("#images").fileinput({
        showUpload: false,
        showPreview: false,
        showRemove: false,
        browseOnZoneClick: true,
        allowedFileTypes: ["image"],
        maxFileSize: 2048,
        maxFileCount: 5,
        msgSizeTooLarge: "File larger than 2MB not allowed.",
        msgFilesTooMany: "Maximum 5 files are allowed.",
        msgValidationError: "<span class='text-danger'><i class='glyphicon glyphicon-exclamation-sign'></i> File Upload Error</span>",
        hiddenThumbnailContent: true
    });

    $('#images').on('fileerror', function (event, data, msg) {
        alert(msg);
    });


    function addBlankCCServices() {
        var index = $("div.services-div").length;

        if (index > 0)
            index = parseInt($($("div.services-div")[index - 1]).data("index")) + 1

        var ser = $("#servicesTemplate");
        var service = {
            index: index,
            service: ""
        }
        $("#cc_services_rep").append($(ser).tmpl(service));
    };

    addBlankCCServices();

    $(document).on("click", ".del-services", function () {
        $(this).parent().parent().parent().parent().remove();
    });

    $(document).on("click", "#addservices", function () {
        addBlankCCServices();
    });

    Dropzone.autoDiscover = false;

    $('#cclist_table').DataTable({
        lengthMenu: [[10, 25, 50], [10, 25, 50]],
        "language":
        {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "CriticalCare.aspx/GetCriticalCares",
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
            { "data": "Email", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Mobile", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "CityName", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Link", "autoWidth": true, "orderable": false, "searchable": false }
        ],
        responsive: true,
        "pagingType": "full_numbers"
    });
    $('#cclist_table tfoot th').each(function () {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" style="width:100%" placeholder="Search ' + title + '" />');
        }
    });

    var table = $('#cclist_table').DataTable();

    $(document).on('click', '.delete-cc', function () {

        if (confirm('Are you sure, you want to delete this item ?')) {
            var id = $(this).data('id');

            $.ajax({
                "url": "CriticalCare.aspx/DeleteCriticalCareById?id=" + id,
                "contentType": "application/json",
                "type": "GET",
                "dataType": "JSON",
                "success": function (data) {
                    table.ajax.reload();
                }
            });
        }
    });

    $(document).on('click', '.edit-cc', function () {
        var id = $(this).data('id');

        $.ajax({
            "url": "CriticalCare.aspx/GetCriticalCareById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                var cc = data.d;

                $("#MainContent_cc_id").val(cc.Id);
                $("#MainContent_name").val(cc.Name);
                $("#MainContent_address").val(cc.Address);
                $("#MainContent_email").val(cc.Email);
                $("#MainContent_mobile").val(cc.Mobile);
                $("#MainContent_city").val(cc.City);
                $("#MainContent_speciality").val(cc.Specialities);  

                $("#image-container").empty().append("<a class='add-cc-images form-group btn btn-primary' data-id='" + cc.Id + "'>Update Images</a>");

                $("label.error").hide();

                var s = [];
                var ser = $("#servicesTemplate");
                var services = cc.Services ? cc.Services.split('\n') : [];
                if (services.length > 0) {
                    $.each(services, function (i, service) {
                        s.push({
                            index: i,
                            service: service
                        });
                    });
                    $("#cc_services_rep").empty().append($(ser).tmpl(s));
                }
            }
        });

        $("#cclistli").removeClass("active");
        $("#cclist").removeClass("active");
        $("#cceditli").addClass("active");
        $("#ccedit").addClass("active");
    });

    $(document).on('click', '.add-cc-images', function () {
        var id = $(this).data('id');
        $("#cc_dz_id").val(id);

        $.ajax({
            "url": "CriticalCare.aspx/GetImagesById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                if (data.d.IsSuccess) {
                    var files = data.d.Data;
                    var myDropzone = new Dropzone("#my-dropzone", {
                        autoProcessQueue: false,
                        url: "Handlers/CriticalCareImageUploader.ashx",
                        addRemoveLinks: true,
                        uploadMultiple: true,
                        maxFiles: 5,
                        parallelUploads: 5,
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
                                var ccid = $("#cc_dz_id").val();
                                data.append("id", ccid);

                                var files = this.files;
                                var rejectedFiles = this.getRejectedFiles();
                                if (rejectedFiles.length > 0) {
                                    $.each(rejectedFiles, function (i, f) {
                                        files.splice(files.indexOf(f), 1);
                                    });
                                }
                                var fcnames = [];
                                $.each(files, function (i, file) {
                                    fcnames.push(ccid + "_" + file.name.replace(/\s+/g, ""));
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
    $(document).on("click", "#saveCC", function () {
        var valid = $("#ccform").valid();
        if (valid) {
            $("#MainContent_sendCC").trigger("click");
        }
    });
});

