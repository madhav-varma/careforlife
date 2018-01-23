$(document).ready(function () {

    Dropzone.autoDiscover = false;


    $('#bloodbanklist_table').DataTable({
        lengthMenu: [[10, 25, 50], [10, 25, 50]],
        "language":
        {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "BloodBank.aspx/GetBloodBanks",
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
            { "data": "OpeningYear", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Timing", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "CityName", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Link", "autoWidth": true, "orderable": false, "searchable": false }
        ],
        responsive: true,
        "pagingType": "full_numbers"
    });
    $('#bloodbanklist_table tfoot th').each(function () {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" style="width:100%" placeholder="Search ' + title + '" />');
        }
    });

    var table = $('#bloodbanklist_table').DataTable();

    $(document).on('click', '.edit-bloodbank', function () {
        var id = $(this).data('id');

        $.ajax({
            "url": "BloodBank.aspx/GetBloodBankById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                var bloodBank = data.d;

                $("#MainContent_blood_bank_id").val(bloodBank.Id);
                $("#MainContent_blood_bank_name").val(bloodBank.Name);
                $("#MainContent_address").val(bloodBank.Address);
                if (bloodBank.Timing) {
                    var t = bloodBank.Timing.split('to');

                    $("#MainContent_timingFrom").val(t[0]);
                    $("#MainContent_timingTo").val(t[1]);
                }
                $("#MainContent_opening_year").val(pathLab.OpeningYear);
                $("#MainContent_email").val(pathLab.Email);
                $("#MainContent_mobile").val(pathLab.Mobile);
                $("#MainContent_city").val(pathLab.City);

            }
        });

        $("#bloodbanklistli").removeClass("active");
        $("#bloodbanklist").removeClass("active");
        $("#bloodbankeditli").addClass("active");
        $("#bloodbankedit").addClass("active");
    });

    $(document).on('click', '.add-bloodbank-images', function () {
        var id = $(this).data('id');
        $("#bb_id").val(id);

        $.ajax({
            "url": "BloodBank.aspx/GetImagesById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                if (data.d.IsSuccess) {
                    var files = data.d.Data;
                    var myDropzone = new Dropzone("#my-dropzone", {
                        autoProcessQueue: false,
                        url: "Handlers/BloodBankImageUploader.ashx",
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
                                var bloodbankid = $("#bb_id").val();
                                data.append("id", bloodbankid);
                                var files = this.getAcceptedFiles();
                                var fnames = [];
                                $.each(files, function (i, file) {
                                    fnames.push(bloodbankid + "_" + file.name.replace(/\s+/g, ""));
                                });
                                if (fnames.length > 0) {
                                    data.append("fnames", fnames.join(" "));
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
                        myDropzone.processQueue();
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

