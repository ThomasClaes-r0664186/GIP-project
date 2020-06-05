$(document).ready(function(){
    console.log('document ready');
    $('#custtable').DataTable({
        "ajax":{
            "url": "/data/aanvragen",
            "type": "POST",
            "datatype": "Json"
        },
        "columns":[
            { "data": "VakCode"},
            { "data": "Titel" },
            { "data": "RNum" },
            { "data": "Naam" },
            { "data": "VoorNaam" },
            {
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "defaultContent": "<button type='button' class='denyModal btn btn-sm btn-danger'>Weiger</button>"
            },
            {
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "defaultContent": "<button type='button' class='approveModal btn btn-sm btn-success' >Accepteer</button>"
            }
        ],"serverSide": true,
        "order": [0, "asc"],
        "proccessing": true
    });
    $('#custtable tbody').on('click', 'button', function(e) {
        e.preventDefault();
        var id = $( this ).parent().parent().children()[0].innerText;
        if ($(e.target).is("button.denyModal")) {
            denyModal(id);
        }else if($(e.target).is("button.approveModal")){
            approveModal(id);
        }
    });
    function denyModal(id){
        //toont modal en gebruikt DOM voor value data juist te tonen
        var json;
        var mydata;
        console.log("staring denyModal...");
        getJson().then(function(data){
            json = data.data;
        }).then(function(){
            //nadat de json is gekregen
            console.log("looking for correct data");
            for(var i = 0; i < json.length; i++) {
                var d = json[i];
                if(String(d.VakCode) === String(id)){
                    mydata = d;
                    console.log('found data: '+mydata);
                }
            }
            console.log(mydata);
        }).then(function(){
            console.log('change DOM of modal');
            //verander alle data van de modal
            $('#deny-cuId').val(mydata.cuId);
            $('#denyText').text("Ben je zeker dat je de "+mydata.Naam+" "+mydata.Voornaam+" zijn aanvraag wilt afwijzen voor het vak "+mydata.Titel+"? U kan hier ook nog een korte omschrijving geven voor het afwijzen.");
            console.log('DOM done, showing modal');
            $('#denyModal').modal('show');
        });
    }
    function approveModal(id){
        //toont modal en gebruikt DOM voor value data juist te tonen
        var json;
        var mydata;
        console.log("staring approveModal...");
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
                if(String(d.VakCode) === String(id)){
                    mydata = d;
                    console.log('found data: '+mydata);
                }
            }
            console.log(mydata);
        }).then(function(){
            console.log('change DOM of modal');
            $('#delete-lokaalId').val(mydata.Id);
            $("#approveText").text("Ben je zeker dat je de "+mydata.Naam+" "+mydata.VoorNaam+" zijn aanvraag wilt goedkeuren voor het vak "+mydata.Titel+"?");
            $('#approveModal').modal('show');
        });

    }
    function getJson(){
        return new Promise(function(resolve, reject)  {
            $.getJSON( "/data/aanvragenjson", function( data ) {
                resolve(data);
            });
        })
    }
});