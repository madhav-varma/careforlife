$(document).ready(function () {
    Dropzone.autoDiscover = false;


    $('#camplist_table').DataTable({
        lengthMenu: [[10, 25, 50], [10, 25, 50]],
        "language":
        {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "Camp.aspx/GetCamps",
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
            { "data": "Description", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Address", "autoWidth": true, "orderable": true, "searchable": true },            
            { "data": "Timing", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "CityName", "autoWidth": true, "orderable": true, "searchable": true },

            { "data": "Link", "autoWidth": true, "orderable": false, "searchable": false }
        ],
        responsive: true,
        "pagingType": "full_numbers"
    });
    $('#camplist_table tfoot th').each(function () {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" style="width:100%" placeholder="Search ' + title + '" />');
        }
    });

    var table = $('#camplist_table').DataTable();

    $(document).on('click', '.delete-cp', function () {

        if (confirm('Are you sure, you want to delete this item ?')) {
            var id = $(this).data('id');

            $.ajax({
                "url": "Camp.aspx/DeleteCampById?id=" + id,
                "contentType": "application/json",
                "type": "GET",
                "dataType": "JSON",
                "success": function (data) {
                    table.ajax.reload();
                }
            });
        }
    });
    $(document).on('click', '.edit-cp', function () {
        var id = $(this).data('id');

        $.ajax({
            "url": "Camp.aspx/GetCampById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                var camp = data.d;

                $("#MainContent_camp_id").val(camp.Id);
                $("#MainContent_organizer").val(camp.Organizer);
                $("#MainContent_camp_title").val(camp.Name);
                $("#MainContent_address").val(camp.Address);
                $("#MainContent_description").val(camp.Description);
                $("#MainContent_description1").val(camp.Description1);
                $("#MainContent_description2").val(camp.Description2);
                $("#MainContent_city").val(camp.City);
                var timings = camp.Timing;
                var t = [];
                if (timings.length > 0) {

                    var tt = [];
                    if (timings)
                        tt = timings.split('-');
                    if (tt.length < 2)
                        tt = timings.split('&');
                    if (tt.length < 2)
                        tt = timings.split('to');

                    $("#MainContent_timingFrom").val(tt[0]);
                    $("#MainContent_timingTo").val(tt[1]);
                }

                $("label.error").hide();


            }
        });

        $("#camplistli").removeClass("active");
        $("#camplist").removeClass("active");
        $("#campeditli").addClass("active");
        $("#campedit").addClass("active");
    });

    $(document).on('click', '.add-cp-images', function () {
        var id = $(this).data('id');
        $("#cp_id").val(id);

        $.ajax({
            "url": "Camp.aspx/GetImagesById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                if (data.d.IsSuccess) {
                    var files = data.d.Data;
                    var myDropzone = new Dropzone("#my-dropzone", {
                        autoProcessQueue: false,
                        url: "Handlers/CampImageUploader.ashx",
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
                                var cpid = $("#cp_id").val();
                                data.append("id", cpid);
                                var files = this.files;
                                var rejectedFiles = this.getRejectedFiles();
                                if (rejectedFiles.length > 0) {
                                    $.each(rejectedFiles, function (i, f) {
                                        files.splice(files.indexOf(f), 1);
                                    });
                                }
                                var fcnames = [];
                                $.each(files, function (i, file) {
                                    fcnames.push(cpid + "_" + file.name.replace(/\s+/g, ""));
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
                        // myDropzone.options.url = "Camp.aspx/UploadImagesById?id=" + id;
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
    $(document).on("click", "#saveCP", function () {
        var valid = $("#campform").valid();
        if (valid) {
            $("#MainContent_sendCP").trigger("click");
        }
    });
});