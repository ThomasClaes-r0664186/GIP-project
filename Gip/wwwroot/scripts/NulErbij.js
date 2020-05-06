function nulErbij() {
    var val = document.getElementById('nummer').value;
    var numbers = Number(val.replace(/[a-zA-Z]/g, ''));
    var letter = val.replace(/[0-9]/g, '');
    if (val !== "") {
        var letter = val.replace(/[0-9]/g, '');
        var numbers = Number(val.replace(/[a-zA-Z]/g, ''));
        if (numbers.toString().length === 1 && val !== 0) {
            document.getElementById('nummer').value = "0" + numbers + letter;
        }
    }
}