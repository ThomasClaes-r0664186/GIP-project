$(document).ready(function(){
    console.log('document ready');
    $('#custtable').DataTable({
        "ajax":{
            "url": "/data/fieldofstudies",
            "type": "POST",
            "datatype": "Json"
        },
        "columns":getColums(),
        "serverSide": true,
        "proccessing": true,
        "order": [[0, "asc"]]
    });
    $('#custtable tbody').on('click', 'button', function(e) {
        e.preventDefault();
        var id = $( this ).parent().parent().children()[0].innerText;
        if ($(e.target).is("button.editModal.btn.btn-sm.btn-dark")) {
            editModal(id);
        }else if($(e.target).is("button.deleteModal.btn.btn-sm.btn-danger")){
            deleteModal(id);
        }
    });
    function getColums(){
        let columns = [];
        columns.push({ "data" : "RichtingCode" });
        columns.push({ "data": "RichtingTitel" });
        columns.push({ "data": "Type" });
        columns.push({ "data": "RichtingStudiepunten"});
        let role = $("#rol").text();
        if(role==='Student') {
            columns.push({
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "defaultContent": '<div class="changeMe"><div class="spinner-border" role="status"> <span class="sr-only">Loading...</span> </div></div>'
            });
        }else{
            columns.push(
            {
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "defaultContent": "<button type='button' class='editModal btn btn-sm btn-dark'>Edit</button>"
            });
           columns.push( 
            {
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "defaultContent": "<button type='button' class='deleteModal btn btn-sm btn-danger' >Delete</button>"
            });
        }
        return columns;
    }
    function editModal(id){
        //toont modal en gebruikt DOM voor value data juist te tonen
        var json;
        var mydata;
        console.log("staring editModal...");
        getJson().then(function(data){
            json = data.data;
        }).then(function(){
            //nadat de json is gekregen
            console.log("looking for correct data");
            for(var i = 0; i < json.length; i++) {
                var d = json[i];
                if(String(d.RichtingCode) === String(id)){
                    mydata = d;
                    console.log('found data: '+mydata);
                }
            }
            console.log(mydata);
        }).then(function(){
            console.log('change DOM of modal');
            //verander alle data van de modal
            $('#edit-code').val(mydata.RichtingCode);
            $('#edit-RichtingTitel').val(mydata.RichtingTitel);
            $('#edit-Type').val(mydata.Type);
            $('#edit-RichtingStudiepunten').val(mydata.RichtingStudiepunten);
            $('#edit-richtingId').val(mydata.Id);
            console.log('DOM done, showing modal');
            $('#editModal').modal('show');
        });
    }
    function deleteModal(id){
        //toont modal en gebruikt DOM voor value data juist te tonen
        var json;
        var mydata;
        console.log("staring deleteModal...");
        getJson().then(function(data){
            json = data.data;
            if(json){
                console.log('received json file! ');
            }else{
                throw "Did not receive json file";
                return;
            }
        }).then(function(){
            //nadat de json is gekregen
            console.log("looking for correct data");
            for(var i = 0; i < json.length; i++) {
                var d = json[i];
                if(String(d.RichtingCode) === String(id)){
                    mydata = d;
                    console.log('found data: '+mydata);
                }
            }
            console.log(mydata);
        }).then(function(){
            console.log('change DOM of modal');
            $('#delete-richtingId').val(mydata.Id);
            let verwijderText = "Ben je zeker dat je de richting "+mydata.Type+": "+mydata.RichtingTitel+" ("+mydata.RichtingCode+") wilt verwijderen?";
            $("#verwijderText").text(verwijderText);
            $('#deleteModal').modal('show');
        });

    }
    function getJson(){
        return new Promise(function(resolve, reject)  {
            $.getJSON( "/data/fieldofstudiesjson", function( data ) {
                resolve(data);
            });
        })
    }
});