$(document).ready(function () {

    function addBlankLocation() {
        var index = $("div.locations-div").length;

        if (index > 0)
            index = parseInt($($("div.locations-div")[index - 1]).data("index")) + 1

        var loctmpl = $("#locationsTemplate");
        var location = {
            index: index,
            address: "",
            hospital: "",
            timingTo: "",
            timingFrom: ""
        }
        $("#timings_rep").append($(loctmpl).tmpl(location));
    }

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
    addBlankLocation();
    addBlankServices();
    changeSwitchery($("#is_special"), false);
    $("#MainContent_chk_special").prop("checked", false);

    $(document).on("click", ".del-loc", function () {
        $(this).parent().parent().parent().remove();
    });

    $(document).on("click", ".del-services", function () {
        $(this).parent().parent().parent().parent().remove();
    });

    $(document).on("click", "#addloc", function () {
        addBlankLocation();
    });

    $(document).on("click", "#addservices", function () {
        addBlankServices();
    });



    Dropzone.autoDiscover = false;


    $('#doclist_table').DataTable({
        lengthMenu: [[10, 25, 50], [10, 25, 50]],
        "language":
        {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "Doctor.aspx/GetDoctors",
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
            { "data": "Tagline", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Degree", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Experience", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Mobile", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "SpecialityName", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "CityName", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Link", "autoWidth": true, "orderable": false, "searchable": false }
        ],
        responsive: true,
        "pagingType": "full_numbers"
    });
    $('#doclist_table tfoot th').each(function () {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" style="width:100%" placeholder="Search ' + title + '" />');
        }
    });

    var table = $('#doclist_table').DataTable();

    $(document).on('click', '.delete-doc', function () {

        if (confirm('Are you sure, you want to delete this item ?')) {
            var id = $(this).data('id');

            $.ajax({
                "url": "Doctor.aspx/DeleteDoctorById?id=" + id,
                "contentType": "application/json",
                "type": "GET",
                "dataType": "JSON",
                "success": function (data) {
                    table.ajax.reload();
                }
            });
        }
    });

    var changeCheckbox = document.querySelector('.js-switch');

    changeCheckbox.onchange = function () {
        var chk = changeCheckbox.checked;

        $("#MainContent_chk_special").prop("checked", chk);

    };

    $(document).on('click', '.edit-doc', function () {
        var id = $(this).data('id');

        $.ajax({
            "url": "Doctor.aspx/GetDoctorById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                var doc = data.d;

                $("#MainContent_doctor_id").val(doc.Id);
                $("#MainContent_name").val(doc.Name);
                $("#MainContent_tagline").val(doc.Tagline);
                $("#MainContent_degree").val(doc.Degree);
                $("#MainContent_experience").val(doc.Experience ? doc.Experience.split(' ')[0] : "");
                $("#MainContent_email").val(doc.Email);
                $("#MainContent_mobile").val(doc.Mobile);
                $("#MainContent_speciality").val(doc.Speciality);
                $("#MainContent_city").val(doc.City);
                changeSwitchery($("#is_special"), doc.IsSpecial);
                $("#MainContent_chk_special").prop("checked", doc.IsSpecial);

                var timings = JSON.parse(doc.Timing);
                var t = [];
                if (timings.length > 0) {
                    $.each(timings, function (i, timing) {
                        var tt = [];
                        if (timing.timing)
                            tt = timing.timing.split('-');
                        t.push({
                            index: i,
                            hospital: timing.hospital,
                            address: timing.Address,
                            timingTo: tt[1],
                            timingFrom: tt[0],
                        });
                    });
                    var loctmpl = $("#locationsTemplate");
                    $("#timings_rep").empty().append($(loctmpl).tmpl(t));
                }

                var s = [];
                var ser = $("#servicesTemplate");
                var services = doc.Services ? doc.Services.split('\n') : [];
                if (services.length > 0) {
                    $.each(services, function (i, service) {
                        s.push({
                            index: i,
                            service: service
                        });
                    });
                    $("#services_rep").empty().append($(ser).tmpl(s));
                }
            }
        });

        $("#doclistli").removeClass("active");
        $("#doclist").removeClass("active");
        $("#doceditli").addClass("active");
        $("#docedit").addClass("active");
    });

    $(document).on('click', '.add-doc-images', function () {
        var id = $(this).data('id');
        $("#doc_id").val(id);

        $.ajax({
            "url": "Doctor.aspx/GetImagesById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                if (data.d.IsSuccess) {
                    var files = data.d.Data;
                    var myDropzone = new Dropzone("#my-dropzone", {
                        autoProcessQueue: false,
                        url: "Handlers/DoctorImageUploader.ashx",
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
                                var docid = $("#doc_id").val();
                                data.append("id", docid);
                                var files = this.files;
                                var rejectedFiles = this.getRejectedFiles();
                                if (rejectedFiles.length > 0) {
                                    $.each(rejectedFiles, function (i, f) {
                                        files.splice(files.indexOf(f), 1);
                                    });
                                }
                                var fcnames = [];
                                $.each(files, function (i, file) {
                                    fcnames.push(docid + "_" + file.name.replace(/\s+/g, ""));
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
    $(document).on("click", "#saveDoc", function () {
        var valid = $("#docform").valid();
        if (valid) {
            $("#MainContent_sendDoc").trigger("click");
        }
    });
    function changeSwitchery(element, checked) {
        if ((element.is(':checked') && checked == false) || (!element.is(':checked') && checked == true)) {
            element.parent().find('.switchery').trigger('click');
        }
    }
});

