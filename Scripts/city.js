$(document).ready(function () {
    $('#citylist_table').DataTable({
        lengthMenu: [[10, 25, 50], [10, 25, 50]],
        "language":
        {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "City.aspx/GetCities",
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
            { "data": "StateName", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "DoctorCount", "autoWidth": true, "orderable": true, "searchable": true },

            { "data": "Link", "autoWidth": true, "orderable": false, "searchable": false }
        ],
        responsive: true,
        "pagingType": "full_numbers"
    });
    $('#citylist_table tfoot th').each(function () {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" style="width:100%" placeholder="Search ' + title + '" />');
        }
    });

    var table = $('#citylist_table').DataTable();

    $(document).on('click', '.delete-city', function () {

        if (confirm('Are you sure, you want to delete this item ?')) {
            var id = $(this).data('id');

            $.ajax({
                "url": "City.aspx/DeleteCityById?id=" + id,
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
    $(document).on('click', '.edit-city', function () {
        var id = $(this).data('id');

        $.ajax({
            "url": "City.aspx/GetCityById?id=" + id,
            "contentType": "application/json",
            "type": "GET",
            "dataType": "JSON",
            "success": function (data) {
                var city = data.d;
                $("#MainContent_city_id").val(city.Id);
                $("#MainContent_city_name").val(city.Name);
                $("#MainContent_state").val(city.StateId);
                $("#MainContent_doc_count").val(city.DoctorCount);

                $("label.error").hide();
            }
        });

        $("#citylistli").removeClass("active");
        $("#citylist").removeClass("active");
        $("#cityeditli").addClass("active");
        $("#cityedit").addClass("active");
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
    $(document).on("click", "#saveCity", function () {
        var valid = $("#cityform").valid();
        if (valid) {
            $("#MainContent_sendCity").trigger("click");
        }
    });

    $(document).on("click", "#cancel", function () {
        location.reload();
    });

});