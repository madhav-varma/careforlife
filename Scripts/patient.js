$(document).ready(function () {
    $('#patientlist_table').DataTable({
        lengthMenu: [[10, 25, 50], [10, 25, 50]],
        "language":
        {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "PatientEducation.aspx/GetVideos",
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
            { "data": "Url", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "SpecialityName", "autoWidth": true, "orderable": true, "searchable": true },
           
            { "data": "Link", "autoWidth": true, "orderable": false, "searchable": false }
        ],
        responsive: true,
        "pagingType": "full_numbers"
    });
    $('#patientlist_table tfoot th').each(function () {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" style="width:100%" placeholder="Search ' + title + '" />');
        }
    });

    var table = $('#patientlist_table').DataTable();

    $(document).on('click', '.delete-pe', function () {

        if (confirm('Are you sure, you want to delete this item ?')) {
            var id = $(this).data('id');

            $.ajax({
                "url": "PatientEducation.aspx/DeleteVideoById?id=" + id,
                "contentType": "application/json",
                "type": "GET",
                "dataType": "JSON",
                "success": function (data) {
                    table.ajax.reload();
                }
            });
        }
    });
    $(document).on('click', '.edit-pe', function () {
        var id = $(this).data('id');

        $.ajax({
            "url": "PatientEducation.aspx/GetVideoById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                var video = data.d;
                $("#MainContent_video_id").val(video.Id);
                $("#MainContent_video_name").val(video.Name);
                $("#MainContent_video_url").val(video.Url);                
                $("#MainContent_speciality").val(video.Speciality);                

                $("label.error").hide();
            }
        });

        $("#patientlistli").removeClass("active");
        $("#patientlist").removeClass("active");
        $("#patienteditli").addClass("active");
        $("#patientedit").addClass("active");
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
    $(document).on("click", "#savePE", function () {
        var valid = $("#patientform").valid();
        if (valid) {
            $("#MainContent_sendPE").trigger("click");
        }
    });



});