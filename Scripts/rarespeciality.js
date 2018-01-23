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


    $('#rarelist_table').DataTable({
        lengthMenu: [[10, 25, 50], [10, 25, 50]],
        "language":
        {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "RareSpeciality.aspx/GetRares",
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
    $('#rarelist_table tfoot th').each(function () {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" style="width:100%" placeholder="Search ' + title + '" />');
        }
    });

    var table = $('#rarelist_table').DataTable();

    $(document).on('click', '.edit-rare', function () {
        var id = $(this).data('id');

        $.ajax({
            "url": "RareSpeciality.aspx/GetRareSpecialityById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                var rare = data.d;

                $("#MainContent_rare_speciality_id").val(rare.Id);
                $("#MainContent_name").val(rare.Name);
                $("#MainContent_email").val(rare.Email);
                $("#MainContent_mobile").val(rare.Mobile);
                $("#MainContent_city").val(rare.City);

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

        $("#rarelistli").removeClass("active");
        $("#rarelist").removeClass("active");
        $("#rareeditli").addClass("active");
        $("#rareedit").addClass("active");
    });

    $(document).on('click', '.add-rare-images', function () {
        var id = $(this).data('id');
        $("#rare_id").val(id);

        $.ajax({
            "url": "RareSpeciality.aspx/GetImagesById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                if (data.d.IsSuccess) {
                    var files = data.d.Data;
                    var myDropzone = new Dropzone("#my-dropzone", {
                        autoProcessQueue: false,
                        url: "Handlers/RareSpecialityImageUploader.ashx",
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
                                var rareid = $("#rare_id").val();
                                data.append("id", rareid);
                                var files = this.files;
                                var rejectedFiles = this.getRejectedFiles();
                                if (rejectedFiles.length > 0) {
                                    $.each(rejectedFiles, function (i, f) {
                                        files.splice(files.indexOf(f), 1);
                                    });
                                }
                                var fcnames = [];
                                $.each(files, function (i, file) {
                                    fcnames.push(rareid + "_" + file.name.replace(/\s+/g, ""));
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
                            if (files > 0)
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
});

