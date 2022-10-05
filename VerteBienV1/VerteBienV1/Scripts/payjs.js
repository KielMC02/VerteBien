

const ccicon = document.getElementById('ccicon');
const ccsingle = document.getElementById('ccsingle');
let cctype = null;

// CREDIT CARD IMAGEN JS
document.querySelector('.preload').classList.remove('preload');
document.querySelector('.creditcard').addEventListener('click', function () {
    if (this.classList.contains('flipped')) {
        document.getElementById('svgsecurity').innerHTML = 'xxx';
        this.classList.remove('flipped');
    } else {
        document.getElementById('svgsecurity').innerHTML = 'xxx';
        this.classList.add('flipped');
    }
})
// FIN CREDIT CARD IMAGE JS

// rellenar.
$(document).on('input', 'input', function () {

    let nam = $('.name').val();
    if (nam.length == 0 || nam.trim() === '') {
        document.getElementById('svgname').innerHTML = 'John Doe';
        document.getElementById('svgnameback').innerHTML = 'John Doe';
    } else {
        //20 caracteres contando espacio
        document.getElementById('svgname').innerHTML = nam;
        document.getElementById('svgnameback').innerHTML = nam;
    }




});
//fin rellenar.

$(document).on('keydown', 'input[type=tel]', function () {

    let num = $('.card-number').val();


    if (num.length == 0) {
        document.getElementById('svgnumber').innerHTML = '0123 4567 8910 1112';
    } else {
        document.getElementById('svgnumber').innerHTML = num;
      
    };


    let exp = $('.expiry').val();
    if (exp.length == 0) {
        document.getElementById('svgexpire').innerHTML = '01/23';
    } else {
        document.getElementById('svgexpire').innerHTML = exp;
    }


});




