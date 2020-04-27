let today = new Date();
let hourNow = today.getHours();
let greeting;

if (hourNow < 5) {
    greeting = "Goedenacht";
}
else if (hourNow < 12) {
    greeting = "Goedemorgend";
}
else if (hourNow < 20) {
    greeting = "Goedenamiddag";
}
else if (hourNow < 24) {
    greeting = "Goedenavond";
}
else {
    greeting = "Welkom";
}

document.getElementById("greeting").innerText = greeting;