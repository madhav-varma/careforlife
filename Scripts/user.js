$(document).ready(function () {
    $('#userlist_table').DataTable({
        lengthMenu: [[10, 25, 50], [10, 25, 50]],
        "language":
        {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "User.aspx/GetUsers",
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
            { "data": "FullName", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Email", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Mobile", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "Profession", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "City", "autoWidth": true, "orderable": true, "searchable": true },
            { "data": "State", "autoWidth": true, "orderable": true, "searchable": true },

            { "data": "Link", "autoWidth": true, "orderable": false, "searchable": false }
        ],
        responsive: true,
        "pagingType": "full_numbers"
    });
    $('#userlist_table tfoot th').each(function () {
        var title = $(this).text();
        if (title !== "") {
            $(this).html('<input type="text" style="width:100%" placeholder="Search ' + title + '" />');
        }
    });

    var table = $('#userlist_table').DataTable();
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
    $(document).on("click", ".enable-lns", function () {
        if (confirm('Are you sure, you want to enable learning sharing for this user ?')) {
            var id = $(this).data('id');

            $.ajax({
                "url": "User.aspx/EnableDisableLearning?is_doctor='true'&id='" + id + "'",
                "contentType": "application/json",
                "type": "GET",
                "dataType": "JSON",
                "success": function (data) {
                    table.ajax.reload();
                }
            });
        }
    });
    $(document).on("click", ".disable-lns", function () {
        if (confirm('Are you sure, you want to disable learning sharing for this user ?')) {
            var id = $(this).data('id');

            $.ajax({
                "url": "User.aspx/EnableDisableLearning?is_doctor='false'&id='" + id + "'",
                "contentType": "application/json",
                "type": "GET",
                "dataType": "JSON",
                "success": function (data) {
                    table.ajax.reload();
                }
            });
        }
    });
});