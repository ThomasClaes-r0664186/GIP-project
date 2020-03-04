let checkbox = document.getElementById('checkbox');
let delivery_div = document.getElementById('delivery');
checkbox.onclick = function () {
    console.log(this);
    if (this.checked) {
        delivery_div.style['display'] = 'block';
    } else {
        delivery_div.style['display'] = 'none';
    }
};