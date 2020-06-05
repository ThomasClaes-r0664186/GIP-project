$(document).ready(function(){
    console.log('document ready');
    $('#custtable').DataTable({
        "ajax":{
            "url": "/data/users",
            "type": "POST",
            "datatype": "Json"
        },
        "columns":[
            { "data" : "UserName","name":"UserName" },
            { "data": "VoorNaam" ,"name":"VoorNaam" },
            { "data": "Naam" ,"name":"Naam"},
            {
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "render": function ( data, type, row, meta ) {
                   return "<a role='button' href='/admin/user/edit?Id="+row.Id+"' class='btn btn-sm btn-dark'>Edit</a>"
                }
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
        if ($(e.target).is("button.deleteModal")) {
            deleteModal(id);
        }
    });
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
                if(String(d.UserName) === String(id)){
                    mydata = d;
                    console.log('found data: '+mydata);
                }
            }
            console.log(mydata);
        }).then(function(){
            console.log('change DOM of modal');
            $('#delete-id').val(mydata.Id);
            let verwijderText = "Ben je zeker dat je de user met username "+mydata.UserName+" wilt verwijderen?";
            $("#verwijderText").text(verwijderText);
            $('#deleteModal').modal('show');
        });

    }
    function getJson(){
        return new Promise(function(resolve, reject)  {
            $.getJSON( "/data/usersjson", function( data ) {
                console.log(data);
                resolve(data);
            });
        })
    }
});