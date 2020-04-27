$(document).ready(function () {
    $("#tableToSort").tablesorter({
        sortList: [[0, 0]],
        headers: {
            4: { sorter: false },
            5: { sorter: false }
        }
    });
});