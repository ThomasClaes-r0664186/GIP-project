function digitalClock() {

    let timeAndDateHTML = document.getElementById('dateAndClock');


    let d = new Date();
    let year = d.getFullYear();
    let month = d.getMonth();
    let date = d.getDate();
    let hours = d.getHours();
    hours = addZero(hours);
    let minutes = d.getMinutes();
    minutes = addZero(minutes);
    let seconds = d.getSeconds();
    seconds = addZero(seconds);
    let today = d.getDay();
    let dayName = ['Zondag', 'Maandag', 'Dinsdag', 'Woensdag', 'Donderdag', 'Vrijdag', 'Zaterdag'];
    let monthName = ['Januari', 'Februari', 'Maart', 'April', 'Mei', 'Juni', 'Juli', 'Augustus', 'September', 'Oktober', 'November', 'December'];

    timeAndDateHTML.innerHTML = dayName[today] + ',  ' + date + ' ' + monthName[month] + ' ' + year + ' ' + +hours + ':' + minutes + ':' + seconds;

}

function addZero(i) {
    if (i < 10) {
        i = '0' + i;
    }
    return i;
}

setInterval(digitalClock, 1000);
