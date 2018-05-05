$(document).ready(function () {
    bindPersons();
});


//"mRender": function (data, type, row) {
//    var r1 = changeurl(blokval, tabval, row.request - id, row.request - type, row.request - status - id, row.request - status - code);
//    return '<b><div class="solTitle"><a class="cursor gglink" href="' + r1 + 'id="+row.request-no+'</a ></div ></b > '
//}
function bindPersons() {
    $('#tabPerson').DataTable({
        destory: true,
        cache: false,
        "aaSorting": [
            [1, 'desc']
        ],

        "bFilter": true,
        "autoWidth": false,
        "processing": true, // for show progress bar
        "serverSide": true, // for processor server side
        "filter": true, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": urlindex,
            "data": {
                //'blkval': blokval,
            },
            "type": "POST",
            "datatype": "json"
        },
        "initComplete": function (settings, Json) {
        },
        "columns": [
            {
                "data": "LastName",
                "name": "LastName",
                "width": '12%'
            },
            {
                "data": "FirstName",
                "name": "FirstName",
                "width": '12%'
            },
            {
                "data": "HireDate",
                "name": "HireDate",
                "dateFormat":"MM/dd/yyyy"
            },
            {
                "data": "EnrollmentDate",
                "name": "EnrollmentDate"
            }

        ]
    });
}