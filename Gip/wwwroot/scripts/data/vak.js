$(document).ready(function(){
    console.log('document ready');
    $('#custtable').DataTable({
        "ajax":{
            "url": "/data/vakken",
            "type": "POST",
            "datatype": "Json"
        },
        "columns":getCollums()
        ,"serverSide": true,
        "order": [0, "asc"],
        "proccessing": true,
        "drawCallback": function(settings, json) {
            $('.formPostDraw').remove();
            //Word gecalled nadat de tabel gemaakt is.
            //buttons worden nu correct gezet.
            let file;
            getJson().then(function(data){
                file = data.data;
            }).then(function(){
                $(".changeMe").get().forEach(function(entry, index, array) {
                    for(let i = 0; i < file.length; i++) {
                        let d = file[i];
                        let vakcode = String($( entry ).parent().parent().children()[0].innerText);
                        if(String(d.Vakcode) === vakcode){
                            let cases= [];
                            cases[0] = '<div class="formPostDraw"><form method="post"  action="/vak/subscribe"><input type="hidden" name="vakCode" value="'+d.courseId+'"/><input type="submit" value="Schrijf in" class="btn btn-success"></form></div>';
                            cases[1] = '<div class="formPostDraw"><form method="post"  action="/vak/unSubscribe"> <input type="hidden" name="vakCode" value="'+d.courseId+'" /> <input type="submit" value="Schrijf uit" class="btn btn-danger"> </form></div>';
                            cases[2] = '<div class="formPostDraw"><form method="post" action="/vak/unSubscribe"> <input type="hidden" name="vakCode" value="'+d.courseId+'"/> <input type="submit" value="Stop aanvraag" class="btn btn-danger"> </form> <a>Uw aanvraag is in verwerking </a></div>';
                            cases[3] = '<div class="formPostDraw"><form method="post"  action="/vak/unSubscribe"> <input type="hidden" name="vakCode" value="'+d.courseId+'"/> <input type="submit" value="Schrijf in" class="btn btn-secondary" disabled> </form> '+((d.afwijzingBeschrijving)?d.afwijzingBeschrijving:'')+'</div>';
                            cases[4] = '<a>Er is iets misgelopen waardoor u dit te zien krijgt.</a>';
                            try {
                                if($('.spinner-border').length !== 0) {
                                    $(entry).append(cases[d.Ingeschreven]);
                                    if(index===array.length-1){
                                        $(".spinner-border").remove();
                                    }
                                }
                            }catch(err){
                                $(".spinner-border").remove();
                                $(entry).append(cases[4]);
                            }
                        }
                    }
                });
            }).then(function(){//veranderDatum
                $(".veranderDatum").get().forEach(function(entry, index, array) {
                    for (let i = 0; i < file.length; i++) {
                        let d = file[i];
                        let vakcode = String($( entry ).parent().parent().children()[0].innerText);
                        if(String(d.Vakcode) === vakcode) {
                            try {
                                if($('.spinner-border').length !== 0) {
                                    $(entry).append(cases[d.Ingeschreven]);
                                    if(index===array.length-1){
                                        $(".spinner-border").remove();
                                    }
                                }
                            }catch(err){
                                $(entry).append(cases[4]);
                            }
                        }
                    }
                });
            });
        }
    });

    function getCollums () {
        //omdat het kollomen aantal bij lector en student verschilt, gebruiken we deze functie
        //eerst rol zoeken. bij #rol (lijn 8-15)
        let collumns = [
            { "data" : "Vakcode","name": "Vakcode",
                "render": function ( data, type, row, meta ) {
                if($('#rol').text()==='Lector'||$('#rol').text()==='Admin'){
                    return '<a id="vakcodeClick" class="veranderDatum" href="planner/ViewCourseUsers?vakcode='+row.Id+'">'+data+'</a>'
                }else{
                    return data;
                }
                
                }
            },
            { "data" : "Titel", "name": "Titel",
                "render": function ( data, type, row, meta ) {
                    if($('#rol').text()==='Lector'||$('#rol').text()==='Admin'){
                        return '<a id="vakcodeClick" class="veranderDatum" href="planner/ViewCourseUsers?vakcode='+row.Id+'">'+data+'</a>'
                    }else{
                        return data;
                    }
                }
            },
            { "data": "Studiepunten", "name": "Studiepunten" }
        ];
        let rol = $("#rol").text();
        if(rol==="Lector"|| rol==="Admin"){
            collumns.push({
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "defaultContent": "<button type='button' class='editModal btn btn-sm btn-dark'>Edit</button>"
            });
            collumns.push({
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "defaultContent": "<button type='button' class='deleteModal btn btn-sm btn-danger' >Delete</button>"
            });
        
        }else{
            collumns.push({
                "bSortable": false,
                "bSearchable": false,
                "data" : null,
                "defaultContent": '<div class="changeMe"><div class="spinner-border" role="status"> <span class="sr-only">Loading...</span> </div></div>'
            })
            //fix dit met ingescrheven attribute
        }
        return collumns;
    }
    function getJson(){
        return new Promise(function(resolve, reject)  {
            $.getJSON( "/data/vakkenjson", function( data ) {
                resolve(data);
            });
        })
    }
    //clickables voor buttons
    $('#custtable tbody').on('click', 'button', function(e) {
        e.preventDefault();
        var id = $( this ).parent().parent().children()[0].innerText;
        console.log(id);
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
                if(String(d.Vakcode) === String(id)){
                    mydata = d;
                    console.log('found data: '+mydata);
                }
            }
            console.log(mydata);
        }).then(function(){
            console.log('change DOM of modal');
            //verander alle data van de modal
            $('#vakcodeOld').val(mydata.courseId);
            console.log(mydata);
            $('#vakcodeNew').val(mydata.Vakcode);
            $('#edit-titel').val(mydata.Titel);
            $('#edit-studiepunten').val(mydata.Studiepunten);
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
                if(String(d.Vakcode) === String(id)){
                    mydata = d;
                    console.log('found data: '+mydata);
                }
            }
            console.log(mydata);
        }).then(function(){
            console.log('change DOM of modal');
            $('#delete-vakcode').val(mydata.courseId);
            let verwijderText = "Ben je zeker dat je het vak "+mydata.Titel+" wilt verwijderen?";
            $("#verwijderText").text(verwijderText);
            $('#deleteModal').modal('show');
        });
    }
});