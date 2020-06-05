$(document).ready(function(){
    console.log('document ready');
    $('#custtable').DataTable({
        "ajax":{
            "url": "/data/lokalen",
            "type": "POST",
            "datatype": "Json"
        },
        "columns":[
            { "data" : "Lokaal" ,"name":"Lokaal"},
            { "data": "Type","name":"Type" },
            { "data": "Capaciteit","name":"Capaciteit" },
            { "data": "Middelen","name":"Middelen" },
            {
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "defaultContent": "<button type='button' class='editModal btn btn-sm btn-dark'>Edit</button>"
            },
            {
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "defaultContent": "<button type='button' class='deleteModal btn btn-sm btn-danger' >Delete</button>"
            }
        ],"serverSide": true,
        "order": [0, "asc"],
        "proccessing": true
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
                if(String(d.Lokaal) === String(id)){
                    mydata = d;
                    console.log('found data: '+mydata);
                }
            }
            console.log(mydata);
        }).then(function(){
            console.log('change DOM of modal');
            //verander alle data van de modal
            $('#edit-lokaalId').val(mydata.Id);
            $('#edit-gebouw').val(mydata.Gebouw);
            $('#edit-nummer').val(mydata.Nummer);
            $('#edit-verdiep').val(mydata.Verdiep);
            $('#type option[value='+mydata.Type+']').attr('selected','selected');
            $('#edit-capaciteit').val(mydata.Capaciteit);
            $('#middelen option[value="'+mydata.Middelen+']"').attr('selected','selected');
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
                if(String(d.Lokaal) === String(id)){
                    mydata = d;
                    console.log('found data: '+mydata);
                }
            }
            console.log(mydata);
        }).then(function(){
            console.log('change DOM of modal');
            $('#delete-lokaalId').val(mydata.Id);
            let verwijderText = "Ben je zeker dat je lokaal "+mydata.Gebouw+mydata.Verdiep+mydata.Nummer+" wilt verwijderen?";
            $("#verwijderText").text(verwijderText);
            $('#deleteModal').modal('show');
        });

    }
    function getJson(){
        return new Promise(function(resolve, reject)  {
            $.getJSON( "/data/lokalenjson", function( data ) {
                resolve(data);
            });
        })
    }
});