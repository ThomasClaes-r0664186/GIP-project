var curr = new Date;
var first = curr.getDate() - curr.getDay();
var first = first + 1
var last = first + 4;

var monday = new Date(curr.setDate(first)).toLocaleDateString();
var friday = new Date(curr.setDate(last)).toLocaleDateString();

document.getElementById("dateOfWeek").innerHTML = "Planning van week " + monday + " tot " + friday;

