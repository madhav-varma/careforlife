$(document).ready(function () {  
    $(document).on("click", "#cancel", function () {
        location.reload();
    });

    function addBlankLocation() {
        var index = $("div.doctors-div").length;

        if (index > 0)
            index = parseInt($($("div.doctors-div")[index - 1]).data("index")) + 1;

        var doctmpl = $("#doctorsTemplate");
        var doctor = {
            index: index,
            degree: "",
            docname: "",
            docmobile: "",
            timingTo: "",
            timingFrom: "",
            docservice: ""
        }
        $("#timings_rep").append($(doctmpl).tmpl(doctor));
    }
    $(document).on("click", ".del-loc", function () {
        $(this).parent().parent().parent().remove();
    });
    $(document).on("click", "#addloc", function () {
        addBlankLocation();
    });

    addBlankLocation();

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

    $(document).on("change", "#MainContent_hrs24", function () {
        if ($(this).prop("checked")) {
            $("#MainContent_timingFrom").prop("disabled", true);
            $("#MainContent_timingTo").prop("disabled", true);
        }
        else {
            $("#MainContent_timingFrom").prop("disabled", false);
            $("#MainContent_timingTo").prop("disabled", false);
        }

    });

    Dropzone.autoDiscover = false;

    function addBlankServices() {
        var index = $("div.services-div").length;

        if (index > 0)
            index = parseInt($($("div.services-div")[index - 1]).data("index")) + 1

        var ser = $("#servicesTemplate");
        var service = {
            index: index,
            service: ""
        }
        $("#services_rep").append($(ser).tmpl(service));
    };
    $(document).on("click", "#addservices", function () {
        addBlankServices();
    });

    $(document).on("click", ".del-services", function () {
        $(this).parent().parent().parent().parent().remove();
    });

    
    addBlankServices();

    $('#mflist_table').DataTable({
        lengthMenu: [[10, 25, 50], [10, 25, 50]],
        "language":
        {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "MedicalFacility.aspx/GetMedicalFacilities",
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
            { "data": "Address", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Email", "autoWidth": true, "orderable": true, "searchable": true },            
            { "data": "Mobile", "autoWidth": true, "orderable": true, "searchable": true },            
            { "data": "CityName", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Link", "autoWidth": true, "orderable": false, "searchable": false }
        ],
        responsive: true,
        "pagingType": "full_numbers"
    });
    $('#mflist_table tfoot th').each(function () {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" style="width:100%" placeholder="Search ' + title + '" />');
        }
    });

    var table = $('#mflist_table').DataTable();

    $(document).on('click', '.delete-mf', function () {

        if (confirm('Are you sure, you want to delete this item ?')) {
            var id = $(this).data('id');

            $.ajax({
                "url": "MedicalFacility.aspx/DeleteMedicalFacilityById?id=" + id,
                "contentType": "application/json",
                "type": "GET",
                "dataType": "JSON",
                "success": function (data) {
                    table.ajax.reload();
                }
            });
        }
    });

    $(document).on('click', '.edit-mf', function () {
        var id = $(this).data('id');

        $.ajax({
            "url": "MedicalFacility.aspx/GetMedicalFacilityById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                var mf = data.d;

                $("#MainContent_facility_id").val(mf.Id);
                $("#MainContent_name").val(mf.Name);
                $("#MainContent_address").val(mf.Address);          
                $("#MainContent_email").val(mf.Email);
                $("#MainContent_mobile").val(mf.Mobile);               
                $("#MainContent_city").val(mf.City);                    
                $("#MainContent_description").val(mf.Description);    

                $("#image-container").empty().append("<a class='add-mf-images form-group btn btn-primary' data-id='" + mf.Id + "'>Update Images</a>");


                var doctors = JSON.parse(mf.Doctor);
                var d = [];
                if (doctors.length > 0) {
                    $.each(doctors, function (i, doc) {
                        var tt = [];
                        if (doc.timing)
                            tt = doc.timing.split('-');
                        if (tt.length < 2)
                            tt = doc.timing.split('&');
                        if (tt.length < 2)
                            tt = doc.timing.split('to');

                        d.push({
                            index: i,
                            docname: doc.docname,
                            degree: doc.degree,
                            docmobile: doc.docmobile,
                            docservice: doc.docservice,
                            timingTo: tt[1].trim(),
                            timingFrom: tt[0].trim(),
                        });
                    });
                    var doctmpl = $("#doctorsTemplate");
                    $("#timings_rep").empty().append($(doctmpl).tmpl(d));
                }

                if (mf.Timing) {
                    if (mf.Timing === "24 hrs") {
                        $("#MainContent_hrs24").prop("checked", true);
                    }
                    else {
                        $("#MainContent_hrs24").prop("checked", false);
                        var tt = [];
                        if (mf.Timing)
                            tt = mf.Timing.split('-');
                        if (tt.length < 2)
                            tt = mf.Timing.split('&');
                        if (tt.length < 2)
                            tt = mf.Timing.split('to');

                        $("#MainContent_timingFrom").val(tt[0].trim());
                        $("#MainContent_timingTo").val(tt[1].trim());
                    }
                }
                $("#MainContent_hrs24").trigger("change");

                var s = [];
                var ser = $("#servicesTemplate");
                var services = mf.Services ? mf.Services.split('\n') : [];
                if (services.length > 0) {
                    $.each(services, function (i, service) {
                        s.push({
                            index: i,
                            service: service
                        });
                    });
                    $("#services_rep").empty().append($(ser).tmpl(s));
                }

                $("label.error").hide();
            }
        });

        $("#mflistli").removeClass("active");
        $("#mflist").removeClass("active");
        $("#mfeditli").addClass("active");
        $("#mfedit").addClass("active");
    });

    $(document).on('click', '.add-mf-images', function () {
        var id = $(this).data('id');
        $("#mf_id").val(id);

        $.ajax({
            "url": "MedicalFacility.aspx/GetImagesById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                if (data.d.IsSuccess) {
                    var files = data.d.Data;
                    var myDropzone = new Dropzone("#my-dropzone", {
                        autoProcessQueue: false,
                        url: "Handlers/MedicalFacilityImageUploader.ashx",
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
                            debugger
                            var msgEl = $(file.previewElement).find('.dz-error-message');
                            msgEl.text("File not uploaded");
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
                                var mfid = $("#mf_id").val();
                                data.append("id", mfid);
                                var files = this.files;
                                var rejectedFiles = this.getRejectedFiles();
                                if (rejectedFiles.length > 0) {
                                    $.each(rejectedFiles, function (i, f) {
                                        files.splice(files.indexOf(f), 1);
                                    });
                                }
                                var fcnames = [];
                                $.each(files, function (i, file) {
                                    fcnames.push(mfid + "_" + file.name.replace(/\s+/g, ""));
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
    $(document).on("click", "#saveMF", function () {
        var valid = $("#mfform").valid();
        if (valid) {
            $("#MainContent_sendMF").trigger("click");
        }
    });
});

