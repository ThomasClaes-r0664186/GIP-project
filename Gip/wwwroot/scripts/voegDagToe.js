function voegDagToe(_dateString) {
    let dateString = _dateString.toString();
    let dagen = ["Zondag", "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag"];
    let arrayOfDate = dateString.split("/");
    let swappedDate = arrayOfDate[1] + "/" + arrayOfDate[0] + "-/" + arrayOfDate[2];
    let date = new Date(Date.parse(swappedDate));
    return dagen[date.getDay()] + " " + dateString;
}
$(document).ready(function () {
    $('td.veranderDatum').each(function () {
        $(this).text(voegDagToe($(this).text()));
    });
})
$(document).ready(function () {
    feather.replace();
})