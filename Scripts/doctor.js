﻿$(document).ready(function () {

    function addBlankLocation() {
        var index = $("div.locations-div").length;

        if (index > 0)
            index = parseInt($($("div.locations-div")[index -1]).data("index")) + 1

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
        "order": [[0, "desc"]],
        "columns": [
            { "data": "Name", "autoWidth": true },
            { "data": "Tagline", "autoWidth": true },
            { "data": "Degree", "autoWidth": true },
            { "data": "Experience", "autoWidth": true },
            { "data": "Mobile", "autoWidth": true },
            { "data": "SpecialityName", "autoWidth": true },
            { "data": "CityName", "autoWidth": true },
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

    $(document).on('click', '.edit-doc', function () {
        var id = $(this).data('id');

        $.ajax({
            "url": "Doctor.aspx/GetDoctorById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                var doc = data.d;

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
                }
                var loctmpl = $("#locationsTemplate");
                $("#timings_rep").empty().append($(loctmpl).tmpl(t));

                var services = doc.Services ? doc.Services.split('\n') : [];
                if (services.length > 0) {
                    $.each(services, function (i, service) {

                    });
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

        $.ajax({
            "url": "Doctor.aspx/GetImagesById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {

                var files = data.d;

                var myDropzone = new Dropzone("#my-dropzone", {
                    autoProcessQueue: false,
                    url: "Doctor.aspx/UploadImagesById?id=" + id,
                    addRemoveLinks: true,
                    paramName: "file", // The name that will be used to transfer the file
                    maxFilesize: 2, // MB
                    accept: function (file, done) {
                        if (file.name == "justinbieber.jpg") {
                            done("Naha, you don't.");
                        }
                        else { done(); }
                    },
                    init: function () {

                    }
                });

                if (files.length > 0) {
                    myDropzone.removeAllFiles();
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
                    myDropzone.options.url = "Doctor.aspx/UploadImagesById?id=" + id;
                    myDropzone.processQueue();
                });

                $("#exampleModal").modal("show");
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
