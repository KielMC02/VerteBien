
$(document).ready(function () {

    var slice = [].slice;

    (function ($, window) {
        var Starrr;
        window.Starrr = Starrr = (function () {
            Starrr.prototype.defaults = {
                rating: void 0,
                max: 5,
                readOnly: false,
                emptyClass: 'fa fa-star-o',
                fullClass: 'fa fa-star',
                change: function (e, value) { }
            };

            function Starrr($el, options) {
                this.options = $.extend({}, this.defaults, options);
                this.$el = $el;
                this.createStars();
                this.syncRating();
                if (this.options.readOnly) {
                    return;
                }
                this.$el.on('mouseover.starrr', 'a', (function (_this) {
                    return function (e) {
                        return _this.syncRating(_this.getStars().index(e.currentTarget) + 1);
                    };
                })(this));
                this.$el.on('mouseout.starrr', (function (_this) {
                    return function () {
                        return _this.syncRating();
                    };
                })(this));
                this.$el.on('click.starrr', 'a', (function (_this) {
                    return function (e) {
                        e.preventDefault();
                        return _this.setRating(_this.getStars().index(e.currentTarget) + 1);
                    };
                })(this));
                this.$el.on('starrr:change', this.options.change);
            }

            Starrr.prototype.getStars = function () {
                return this.$el.find('a');
            };

            Starrr.prototype.createStars = function () {
                var j, ref, results;
                results = [];
                for (j = 1, ref = this.options.max; 1 <= ref ? j <= ref : j >= ref; 1 <= ref ? j++ : j--) {
                    results.push(this.$el.append("<a href='#' />"));
                }
                return results;
            };

            Starrr.prototype.setRating = function (rating) {
                if (this.options.rating === rating) {
                    rating = void 0;
                }
                this.options.rating = rating;
                this.syncRating();
                return this.$el.trigger('starrr:change', rating);
            };

            Starrr.prototype.getRating = function () {
                return this.options.rating;
            };

            Starrr.prototype.syncRating = function (rating) {
                var $stars, i, j, ref, results;
                rating || (rating = this.options.rating);
                $stars = this.getStars();
                results = [];
                for (i = j = 1, ref = this.options.max; 1 <= ref ? j <= ref : j >= ref; i = 1 <= ref ? ++j : --j) {
                    results.push($stars.eq(i - 1).removeClass(rating >= i ? this.options.emptyClass : this.options.fullClass).addClass(rating >= i ? this.options.fullClass : this.options.emptyClass));
                }
                return results;
            };

            return Starrr;

        })();
        return $.fn.extend({
            starrr: function () {
                var args, option;
                option = arguments[0], args = 2 <= arguments.length ? slice.call(arguments, 1) : [];
                return this.each(function () {
                    var data;
                    data = $(this).data('starrr');
                    if (!data) {
                        $(this).data('starrr', (data = new Starrr($(this), option)));
                    }
                    if (typeof option === 'string') {
                        return data[option].apply(data, args);
                    }
                });
            }
        });
    })(window.jQuery, window);


    /*pequeño Js para enseñar las estrellas segun valoracion que trae @ViewBag.promedio*/
    var rating = document.getElementsByClassName("starrr");
    var f = document.getElementById("resstrella");

    for (var a = 0; a < rating.length; a++) {

        $(rating[a]).starrr({

            readOnly: true,

            rating: rating[a].getAttribute("data-rating")
        });
    }


    /*pequeño Js para enseñar las estrellas este es para la parte de  para mostrar edicion*/
    var rating = document.getElementsByClassName("str");
    var reltado = document.getElementById("resstrella");

    for (var a = 0; a < rating.length; a++) {

        $(rating[a]).starrr({


            rating: rating[a].getAttribute("data-rating"),

            change: function (e, valor) {

                //   alert(valor);
                reltado.value = valor;


            }
        })
    }
    /* Cambiar numeros flotantes.*/
    function financial(x) {
        return Number.parseFloat(x).toFixed(1);
    }
    comentarios = document.querySelectorAll('#comentarios-conteo');

    var rar = $('#miDiv').data('promedio')
    ror = financial(rar).trim();
    /* Cantidad de votos formatiados e impresos dinamicamente.*/
    $('#cantidad-votos').text(ror + " " + "votos");
    /*Cantidad de comentarios.*/
    let ii = 0
    while (ii < comentarios.length) {
        ii++;
    }
    $('#reviews-clientes').text(ii + "" + " reviews de los clientes.");

    /*   Cambiar numeros decimal a horas*/

    function decimalAHora(decimal) {
        let horas = Math.floor(decimal), // Obtenemos la parte entera
            restoHoras = Math.floor(decimal % 1 * 100), // Obtenemos la parde decimal
            decimalMinutos = restoHoras * 60 / 100, // Obtenemos los minutos expresado en decimal

            minutos = Math.floor(decimalMinutos), // Obtenemos la parte entera
            restoMins = Math.floor(decimalMinutos % 1 * 100), // Obtenemos la parde decimal
            segundos = Math.floor(restoMins * 60 / 100); // Obtenemos los segundos expresado en entero

        return `${('00' + horas).slice(-2)}:${('00' + minutos).slice(-2)}:${('00' + segundos).slice(-2)}`;
    }
    /* FIN Cambiar numeros decimal a horas.*/




    var Hsemanaini = $('#l-v').data('semanaini');
    var hsemanafin = $('#l-v').data('semanafin');
    var hsi = $('#sabado').data('findesemanaini');
    var hsf = $('#sabado').data('findesemanafin');
    var hdi = $('#domingo').data('domingoini');
    var hdf = $('#domingo').data('domingofin');


    hini = decimalAHora(Hsemanaini);
    h = hini.substr(0, 2);
    m = hini.substr(3, 3);

    hfin = decimalAHora(hsemanafin);
    hcierre = hfin.substr(0, 2);
    mcierre = hfin.substr(3, 3);

    hsabini = decimalAHora(hsi);
    hsabi = hsabini.substr(0, 2);
    msabi = hsabini.substr(3, 3);

    hsabfin = decimalAHora(hsf);
    hsabf = hsabfin.substr(0, 2);
    msabf = hsabfin.substr(3, 3);

    hdomini = decimalAHora(hdi);
    hdomi = hdomini.substr(0, 2);
    mdomi = hdomini.substr(3, 3);

    hdomfin = decimalAHora(hdf);
    hdomf = hdomfin.substr(0, 2);
    mdomf = hdomfin.substr(3, 3);


    /*Evaluar si la hora es AM o PM */
    var ampm = h >= 12 ? 'pm' : 'am';
    var ampmcierre = hcierre >= 12 ? 'pm' : 'am';
    var ampmsabini = hsabi >= 12 ? 'pm' : 'am';
    var ampmcsabcie = hsabf >= 12 ? 'pm' : 'am';
    var ampmcdomini = hdomi >= 12 ? 'pm' : 'am';
    var ampmcdomfin = hdomf >= 12 ? 'pm' : 'am';


    /*FIN Formatear fechas AM y PM */
    const horas_semanales = {
        '13': '1',
        '14': '2',
        '15': '3',
        '16': '4',
        '17': '5',
        '18': '6',
        '19': '7',
        '20': '8',
        '21': '9',
        '22': '10',
        '23': '11',
        '24': '12'
    }
    const hsemini = horas_semanales[h] || h;
    const hsemfin = horas_semanales[hcierre] || hcierre;
    const hsabadoini = horas_semanales[hsabi] || hsabi;
    const hsabadofin = horas_semanales[hsabf] || hsabf;
    const hdomingoini = horas_semanales[hdomi] || hdomi;
    const hsdomingofin = horas_semanales[hdomf] || hdomf;


    /*para imprimir el formato*/
    formatinicio = hsemini + ":" + m + "" + ampm;
    formatcierre = hsemfin + ":" + mcierre + "" + ampmcierre;
    formatiniciosab = hsabadoini + ":" + msabi + "" + ampmsabini;
    formatcierresab = hsabadofin + ":" + msabf + "" + ampmcsabcie;
    formatiniciodom = hdomingoini + ":" + mdomi + "" + ampmcdomini;
    formatcierredom = hsdomingofin + ":" + mdomf + "" + ampmcdomfin;
    /*Imprimir horas con formato*/
    $('#l-v').text("Lunes a Viernes:" + " " + formatinicio + " " + "-" + " " + formatcierre);
    $('#sabado').text("Sabado:" + " " + formatiniciosab + " " + "-" + " " + formatcierresab);
    $('#domingo').text("Domingo:" + " " + formatiniciodom + " " + "-" + " " + formatcierredom);


    /*Activador de datos mas informacion detalles.*/

    const det = document.getElementById(`more-information`);

    if (det) {
        const pdetalles = document.getElementById(`pdetalles`).getAttribute("data-rating");
        function resolveAfter2Seconds() {
            return new Promise(resolve => {

                resolve('resolved');
                if (pdetalles !== "") {
                    det.classList.add('show');

                }

            });
        }
    }
    /* Fin Activador de datos mas innformacion detalles.*/

    tiempo = $('#tiempo').data('tiempo')
    if (tiempo) {
        t = Math.trunc(tiempo)
        document.getElementById("tiempo").innerHTML = "<b>Tiempo:</b>" + " " + t + " " + "hora";
    }

    /*pequeño Js para enseñar las estrellas este es para la parte de  para mostrar edicion*/
    var rating = document.getElementsByClassName("str");
    var reltado = document.getElementById("resstrella");

    for (var a = 0; a < rating.length; a++) {

        $(rating[a]).starrr({


            rating: rating[a].getAttribute("data-rating"),

            change: function (e, valor) {

               
                reltado.value = valor;

            }
        });
    }


    $('#st').starrr({

        rating: 3,
        change: function (e, valor) {

            alert(valor);

        }

    });


});
/* query para poner el nombre del archivo que se sube*/
jQuery('input[type=file]').change(function (event) {
    var filename = jQuery(this).val().split('\\').pop();
    var idname = jQuery(this).attr('id');
    let id = this.id;
    var res = filename.substring(0, 15);
    var fname = res + "...";
    jQuery('span.' + idname).next().find('span').html(fname);
    /*   $('span.' + id).html(fname);*/


});


///* FIN query para poner el nombre del archivo que se sube*/

$(document).on('change', 'input[type="file"]', function () {
    // this.files[0].size recupera el tamaño del archivo
    // alert(this.files[0].size);

    var fileName = this.files[0].name;
    var fileSize = this.files[0].size;


    if (fileSize > 5000000) {


        alert('El archivo no debe superar los 5MB');

        this.value = '';
        this.files[0].name = '';
    } else {


        // recuperamos la extensión del archivo
        var ext = fileName.split('.').pop();

        // Convertimos en minúscula porque 
        // la extensión del archivo puede estar en mayúscula
        ext = ext.toLowerCase();

        // console.log(ext);
        switch (ext) {
            case 'jpg':
            case 'jpeg':
            case 'png':
            case 'pdf': break;
            default:
                alert('El archivo no tiene la extensión adecuada');
                this.value = ''; // reset del valor
                this.files[0].name = '';

        }

    }
});
///* fin query para poner el nombre del archivo que se sube*/


/*Inicio de validacion */
const formulario = document.getElementById('formulario');

function countChars(obj) {
    document.getElementById("charNum").innerHTML = obj.value.length + ' characters';
}

const inputs = document.querySelectorAll('#formulario input');

const expresiones = {
    usuario: /^[a-zA-Z0-9\_\-\@]{4,16}$/, // Letras, numeros, guion y guion_bajo
    nombre: /^[a-zA-ZÀ-ÿ\s]{4,25}$/, // Letras y espacios, pueden llevar acentos de 4 a 25 digitos.
    password: /^.{4,12}$/, // 4 a 12 digitos.
    correo: /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i,
    telefono: /^\d{9,10}$/, // 9 a 10 numeros.
    fecha: /^(?:0?[1-9]|1[1-2])([\-/.])(3[01]|[12][0-9]|0?[1-9])\1\d{4}$/, //pra fechas mes/dia/año
    numero: /^[0-9]{1,4}$/, //numeros de 1 a 4 digios
    alphanumerico: /[A-Z? a-z 0-9?]{10,150}$/,//Alfanumerico
    callenumero: /^[a-zA-ZÀ-ÿ\s  0-9?]{2,15}$/, // Letras y espacios, pueden llevar acentos de 4 a 25 digitos.
    precio: /^\d{1,4}(\.\d{1,2})?$/, //número decimal o flotante
    fbuser: /(?: (?: http | https): \/\/)?(?:www.)?facebook.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-]*\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?@/, //Fb user 
    iguser: /(?:www\.)?(?:instagram\.com|instagr\.am)@/,// Instgram User. 
    web: /(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})@/,//PAra paginas webz
    ciudadnombre: /[A-Z? a-z 0-9?]{10,25}$/,//Alfanumerico
}

const campos = {
    usuario: false,
    nombre: false,
    Password: false,
    Email: false,
    telefono: false,
    /*  fecha_nacimiento_: false,*/
    nombre_peluqueria: false,
    apellido: false,
    ConfirmPassword: false,
    fecha_nacimiento_: false,
    telefono: false,
    ciudad: false,
    calle: false,
    sector: false,
    precio_servicio: false,
    nombre_servicio: false,
    tiempo: false,
    descripcion: false,
    precio: false,
    whatsapp: false,
    instagram: false,
    web_app: false,
    facebook: false,
    id_usuario: false,
    comentario: false,
    nombre_categoria: false,
  

}
const validarFormulario = (e) => {
    switch (e.target.name) {
        case "usuario":
            validarCampo(expresiones.usuario, e.target, 'usuario');
            break;
        case "nombre":
            validarCampo(expresiones.nombre, e.target, 'nombre');
            break;
        case "apellido":
            validarCampo(expresiones.nombre, e.target, 'apellido');
            break;
        case "Password":
            validarCampo(expresiones.password, e.target, 'Password');
            validarPassword2();
            break;
        case "ConfirmPassword":
            validarPassword2();
            break;
        case "Email":
            validarCampo(expresiones.correo, e.target, 'Email');
            break;
        case "telefono":
            validarCampo(expresiones.telefono, e.target, 'telefono');
            break;
        case "fecha_nacimiento_":
            validarFech(expresiones.fecha, e.target, 'fecha_nacimiento_');
            calcularEdad();
            break;
        case "ciudad":
            validarCampo(expresiones.ciudadnombre, e.target, 'ciudad');
            break;
        case "sector":
            validarCampo(expresiones.nombre, e.target, 'sector');
            break;
        case "precio_servicio":
            validarCampo(expresiones.precio, e.target, 'precio_servicio');
            break;
        case "nombre_servicio":
            validarCampo(expresiones.nombre, e.target, 'nombre_servicio');
            break;
        case "tiempo":
            validarCampo(expresiones.precio, e.target, 'tiempo');
            break;
        case "descripcion":
            validarCampo(expresiones.alphanumerico, e.target, 'descripcion');
            Validate();
            break;
        case "calle":
            validarCampo(expresiones.callenumero, e.target, 'calle');
            break;
        case "nombre_peluqueria":
            validarCampo(expresiones.nombre, e.target, 'nombre_peluqueria');
            break;
        case "id_usuario":
            validarCampo(expresiones.correo, e.target, 'id_usuario');
            break;
        case "whatsapp":
            validarCampo(expresiones.telefono, e.target, 'whatsapp');
            break;
        case "facebook":
            validarCampo(expresiones.fbuser, e.target, 'facebook');
            break;
        case "instagram":
            validarCampo(expresiones.iguser, e.target, 'instagram');
            break;
        case "web_app":
            validarCampo(expresiones.web, e.target, 'web_app');
            break;
        case "comentario":
            validarCampo(expresiones.alphanumerico, e.target, 'comentario');
            break;
        case "nombre_categoria":
            validarCampo(expresiones.nombre, e.target, 'nombre_categoria');
            break;
       
       
    }
}

const validarCampo = (expresion, input, campo) => {

    if (expresion.test(input.value)) {
        document.getElementById(`grupo__${campo}`).classList.remove('formulario__grupo-incorrecto');
        document.getElementById(`grupo__${campo}`).classList.add('formulario__grupo-correcto');
        document.querySelector(`#grupo__${campo} i`).classList.add('fa-check-circle');
        document.querySelector(`#grupo__${campo} i`).classList.remove('fa-times-circle');
        document.querySelector(`#grupo__${campo} .formulario__input-error`).classList.remove('formulario__input-error-activo')
        campos[campo] = true;

    } else {
        document.getElementById(`grupo__${campo}`).classList.add('formulario__grupo-incorrecto');
        document.getElementById(`grupo__${campo}`).classList.remove('formulario__grupo-correcto');
        document.querySelector(`#grupo__${campo} i`).classList.add('fa-times-circle');
        document.querySelector(`#grupo__${campo} i`).classList.remove('fa-check-circle');
        document.querySelector(`#grupo__${campo} .formulario__input-error`).classList.add('formulario__input-error-activo')
        campos[campo] = false;
    }
}


const validarFech = (expresion, input, campo) => {

    if (expresion.test(input.value)) {

        campos[campo] = true;

    } else {
        campos[campo] = false;
    }
}




//Validar fecha de cumpleaños

var loc = window.location;
var pathNa = loc.pathname.substring(0, loc.pathname.lastIndexOf('/') + 1);

var pathname = window.location.pathname;
//if (pathname == '/Account/Register') {
//    //datepicker de formulario neogcio
//    $(function () {
//        var dtToday = new Date();

//        var month = dtToday.getMonth() + 1;
//        var day = dtToday.getDate();
//        var year = dtToday.getFullYear();
//        if (month < 10)
//            month = '0' + month.toString();
//        if (day < 10)
//            day = '0' + day.toString();

//        var maxDate = year + '-' + month + '-' + day;
//        $('#fecha_nacimiento_').attr('max', maxDate);
//    });
//}

if (pathname == '/Account/RegisterUser') {



    function calcularEdad() {
        var d = document.getElementById("fecha_nacimiento_").value;
        var inDate = new Date(d);
        var anio = inDate.getFullYear();
        var fec_actual = new Date();
        var fec_anio = fec_actual.getFullYear();
        var edad = fec_anio - anio;
        if (edad >= 15) {
            document.getElementById(`grupo__fecha_nacimiento_`).classList.remove('formulario__grupo-incorrecto');
            document.getElementById(`grupo__fecha_nacimiento_`).classList.add('formulario__grupo-correcto');
            document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.add('fa-check-circle');
            document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.remove('fa-times-circle');
            document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-error`).classList.remove('formulario__input-error-activo')
            document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-errorr`).classList.remove('formulario__input-error-activo');

        } else {
            document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-errorr`).classList.add('formulario__input-error-activo');
            document.getElementById(`grupo__fecha_nacimiento_`).classList.add('formulario__grupo-incorrecto');
            document.getElementById(`grupo__fecha_nacimiento_`).classList.remove('formulario__grupo-correcto');
            document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.add('fa-times-circle');
            document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.remove('fa-check-circle');
            document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-error`).classList.add('formulario__input-error-activo')

        }
    }
} else {
    function calcularEdad() {
        var d = document.getElementById("fecha_nacimiento_").value;
        var inDate = new Date(d);
        var anio = inDate.getFullYear();
        var fec_actual = new Date();
        var fec_anio = fec_actual.getFullYear();
        var edad = fec_anio - anio;
        if (edad >= 18) {

            document.getElementById(`grupo__fecha_nacimiento_`).classList.remove('formulario__grupo-incorrecto');
            document.getElementById(`grupo__fecha_nacimiento_`).classList.add('formulario__grupo-correcto');
            document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.add('fa-check-circle');
            document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.remove('fa-times-circle');
            document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-error`).classList.remove('formulario__input-error-activo')
            document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-errorr`).classList.remove('formulario__input-error-activo');

        } else {
            document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-errorr`).classList.add('formulario__input-error-activo');
            document.getElementById(`grupo__fecha_nacimiento_`).classList.add('formulario__grupo-incorrecto');
            document.getElementById(`grupo__fecha_nacimiento_`).classList.remove('formulario__grupo-correcto');
            document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.add('fa-times-circle');
            document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.remove('fa-check-circle');
            document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-error`).classList.add('formulario__input-error-activo')
        }
    }

}



const validarPassword2 = () => {
    const inputPassword1 = document.getElementById('Password');
    const inputPassword2 = document.getElementById('ConfirmPassword');

    if (inputPassword1.value !== inputPassword2.value) {
        document.getElementById(`grupo__ConfirmPassword`).classList.add('formulario__grupo-incorrecto');
        document.getElementById(`grupo__ConfirmPassword`).classList.remove('formulario__grupo-correcto');
        document.querySelector(`#grupo__ConfirmPassword i`).classList.add('fa-times-circle');
        document.querySelector(`#grupo__ConfirmPassword i`).classList.remove('fa-check-circle');
        document.querySelector(`#grupo__ConfirmPassword .formulario__input-error`).classList.add('formulario__input-error-activo');
        campos['Password'] = false;
        campos['ConfirmPassword'] = false;
    } else {
        document.getElementById(`grupo__ConfirmPassword`).classList.remove('formulario__grupo-incorrecto');
        document.getElementById(`grupo__ConfirmPassword`).classList.add('formulario__grupo-correcto');
        document.querySelector(`#grupo__ConfirmPassword i`).classList.remove('fa-times-circle');
        document.querySelector(`#grupo__ConfirmPassword i`).classList.add('fa-check-circle');
        document.querySelector(`#grupo__ConfirmPassword .formulario__input-error`).classList.remove('formulario__input-error-activo');
        campos['Password'] = true;
        campos['ConfirmPassword'] = true;
    }
}
var usernamecheck = /[A-Z? a-z 0-9?]{10,250}$/;

const Validate = (e) => {
    var val = document.getElementById('descripcion').value;
    var lines = val.split('\n');

    for (var i = 0; i < lines.length; i++) {
        if (!val.trim() == "") {

            if (!lines[i].match(usernamecheck)) {

                document.getElementById(`grupo__descripcion`).classList.add('formulario__grupo-incorrecto');
                document.getElementById(`grupo__descripcion`).classList.remove('formulario__grupo-correcto');
                document.querySelector(`#grupo__descripcion i`).classList.add('fa-times-circle');
                document.querySelector(`#grupo__descripcion i`).classList.remove('fa-check-circle');
                document.querySelector(`#grupo__descripcion .formulario__input-error`).classList.add('formulario__input-error-activo');
                campos['descripcion'] = false;

            } else {
                document.getElementById(`grupo__descripcion`).classList.remove('formulario__grupo-incorrecto');
                document.getElementById(`grupo__descripcion`).classList.add('formulario__grupo-correcto');
                document.querySelector(`#grupo__descripcion i`).classList.remove('fa-times-circle');
                document.querySelector(`#grupo__descripcion i`).classList.add('fa-check-circle');
                document.querySelector(`#grupo__descripcion .formulario__input-error`).classList.remove('formulario__input-error-activo');
                campos['descripcion'] = true;

            }
        } else {
            document.getElementById(`grupo__descripcion`).classList.add('formulario__grupo-incorrecto');
            document.getElementById(`grupo__descripcion`).classList.remove('formulario__grupo-correcto');
            document.querySelector(`#grupo__descripcion i`).classList.add('fa-times-circle');
            document.querySelector(`#grupo__descripcion i`).classList.remove('fa-check-circle');
            document.querySelector(`#grupo__descripcion .formulario__input-error`).classList.add('formulario__input-error-activo');
            campos['descripcion'] = false;
        }


    }
}



inputs.forEach((input) => {
    input.addEventListener('keyup', validarFormulario);
    input.addEventListener('blur', validarFormulario);

});




//$('input[type="file"]').change(function () {
//    $("button").prop("disabled", this.files.length == 0);
//    document.getElementById('obj1').style.display = 'none';
//});



//var pathname = window.location.pathname;
////if (pathname == '/Account/Register') {
////     else {Account/RegisterUser
////   } }
//const imgiput = document.getElementById('file1');
//if (imgiput.value == "") {
//    alert("Imagen vacia");
//} else { }

// Busca la pagina y el directorio exacto. 



var frm = document.getElementById("formulario");
if (frm) {
    formulario.addEventListener('submit', (e) => {
        e.preventDefault();

        switch (pathname) {
            case '/SERVICIOS/Create':  // para registrar negocios o peluqueria 

                if (campos.tiempo && campos.precio_servicio && campos.nombre_servicio && campos.descripcion) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;

            // para registrar negocios o peluqueria 
            case '/Account/Register':

                if (campos.Email && campos.nombre && campos.apellido && campos.nombre_peluqueria  && campos.Password && campos.ConfirmPassword && campos.calle) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;
            case '/Account/RegisterUser': // para registrar clientes 
                if (campos.Email && campos.nombre && campos.apellido && campos.Password && campos.ConfirmPassword) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;
            case '/CATEGORIAS_SERVICIOS/Create': // para registrar clientes 
                if (campos.descripcion && campos.nombre_categoria) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;

            case '/AspNetUsers/Edit': // ppara editar la info del que esta logueado
                if (campos.nombre && campos.apellido && campos.nombre_peluqueria && campos.ciudad && campos.calle && campos.telefono) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;
            case '/AspNetUsers/Edit/': // ppara editar la info del que esta logueado
                if (campos.nombre && campos.apellido && campos.nombre_peluqueria && campos.ciudad && campos.calle && campos.telefono) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;
            case '/Aspnetusers/Edit/': // ppara editar la info del que esta logueado
                if (campos.nombre && campos.apellido && campos.nombre_peluqueria && campos.ciudad && campos.calle && campos.telefono) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;





        }
        // Validacion para casos de paginas dinamicas. 
        switch (pathNa) {

            case '/REDES_SOCIALES/Edit/':
                if (campos.whatsapp && campos.instagram && campos.facebook && campos.web_app) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;

            case '/PUNTUACION_SERVICIOS/Edit/': //
                if (campos.descripcion) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;
            case '/PUNTUACION_PELUQUERIA/Edit/':
                if (campos.descripcion) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;
            case '/SERVICIOS/Edit/':
                if (campos.nombre_servicio && precio_servicio && campos.tiempo && campos.descripcion) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;
            case '/CATEGORIAS_SERVICIOS/Edit/':
                if (campos.nombre_categoria && campos.descripcion) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;
            case '/Aspnetusers/Edit/':
                if (campos.nombre && campos.apellido && campos.nombre_peluqueria && campos.ciudad && campos.calle && campos.telefono) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;

            case '/AspNetUsers/Edit/':
                if (campos.nombre && campos.apellido && campos.nombre_peluqueria && campos.ciudad && campos.calle && campos.telefono) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;

        }


    });
}




// comentarios asincronos.
if (document.getElementById(`pdetalles`)) {
    $(function () {
        var $h3s = $('li.opcion-detalles').click(function () {
            $h3s.removeClass('active');
            $(this).addClass('active');
        });
    });
    const det = document.getElementById(`more-information`);

    const pdetalles = document.getElementById(`pdetalles`).getAttribute("data-rating");



    function resolveAfter2Seconds() {
        return new Promise(resolve => {

            resolve('resolved');
            if (pdetalles !== "") {
                det.classList.add('show');


            }

        });
    }

    async function asyncCall() {

        const result = await resolveAfter2Seconds();
        // expected output: "resolved"
    }

    asyncCall();


    var triggerTabList = [].slice.call(document.querySelectorAll('#myTab a'))
    triggerTabList.forEach(function (triggerEl) {
        var tabTrigger = new bootstrap.Tab(triggerEl)

        triggerEl.addEventListener('click', function (event) {
            event.preventDefault()
            tabTrigger.show()
        })
    })
}


/*pequeño Js para enseñar las estrellas este es para la parte de  para mostrar edicion*/
/* var rating = document.getElementsByClassName("str");

 function camstr(strval) {
     srt = strval;
     rating: rating[srt].getAttribute("data-rating")
 }
 /*
 
 /*pequeño Js para enseñar las estrellas este es para editar las estrellas */

/*Para estrellas*/
/*
let star = document.querySelectorAll('input');
let showValue = document.querySelector('#rating-value');

for (let i = 0; i < star.length; i++) {
    star[i].addEventListener('click', function () {
        i = this.value;
       
        showValue.innerHTML = i + " de 5";
        
    });
}
*/

/*Coookiess*/

const botonAceptarCookies = document.getElementById('btn-aceptar-cookies');
const avisoCookies = document.getElementById('aviso-cookies');
const fondoAvisoCookies = document.getElementById('fondo-aviso-cookies');



if (!localStorage.getItem('cookies-aceptadas')) {
    avisoCookies.classList.add('activo');
    fondoAvisoCookies.classList.add('activo');
}
botonAceptarCookies.addEventListener('click', () => {
    avisoCookies.classList.remove('activo');
    fondoAvisoCookies.classList.remove('activo');

    localStorage.setItem('cookies-aceptadas', true);


});
/*FIN de Coookiess*/
//* Carrusel*/

window.addEventListener('load', function () {
    // Mobile-first defaults

    if (carousel = document.querySelector('.carousel__lista')) {

        new Glider(document.querySelector('.carousel__lista'), {
            slidesToShow: 1,
            slidesToScroll: 1,
            dots: '.carousel__indicadores',
            draggable: 'true',
            arrows: {
                prev: '.carousel__anterior',
                next: '.carousel__siguiente'
            },
            responsive: [
                {
                    // screens greater than >= 775px
                    breakpoint: 450,
                    settings: {
                        // Set to `auto` and provide item width to adjust to viewport
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }, {
                    // screens greater than >= 1024px
                    breakpoint: 800,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });
    }
});



/*MAPA para servicio*/

if (pathNa === '/SERVICIOS/Details/') {
    var latitud = $('#latitud').data('latitud');
    var longitud = $('#longitud').data('longitud');
    var nombre = $('#nombre-negocio').data('nombre');
    var map = L.map('map').setView([latitud, longitud], 15);
    L.marker([latitud, longitud]).addTo(map)
        .bindPopup(nombre)
        .openPopup();
    L.tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data & copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'your.mapbox.access.token'
    }).addTo(map);


}

/* FIN MAPA*/

let eml = document.getElementById("Email");
if (eml) {
    console.log("Reloaded");

    // dom variables

    var msf_getFsTag = document.getElementsByTagName("fieldset");


    // declaring the active fieldset & the total fieldset count
    var msf_form_nr = 0;
    var fieldset = msf_getFsTag[msf_form_nr];
    fieldset.className = "msf_show";

    // creates and stores a number of bullets
    var msf_bullet_nr = "<div class='msf_bullet'></div>";
    var msf_length = msf_getFsTag.length;
    for (var i = 1; i < msf_length; ++i) {
        msf_bullet_nr += "<div class='msf_bullet'></div>";
    };
    // injects bullets
    var msf_bullet_o = document.getElementsByClassName("msf_bullet_o");
    for (var i = 0; i < msf_bullet_o.length; ++i) {
        var msf_b_item = msf_bullet_o[i];
        msf_b_item.innerHTML = msf_bullet_nr;
    };

    // removes the first back button & the last next button
    document.getElementsByName("back")[0].className = "msf_hide";
    document.getElementsByName("next")[msf_bullet_o.length - 1].className = "msf_hide";

    // Makes the first dot active
    var msf_bullets = document.getElementsByClassName("msf_bullet");
    msf_bullets[msf_form_nr].className += " msf_bullet_active";

    // Validation loop & goes to the next step
    function msf_btn_next() {

        var msf_val = true;

        var msf_fs = document.querySelectorAll("fieldset")[msf_form_nr];
        var msf_fs_i_count = msf_fs.querySelectorAll("input").length;

        for (i = 0; i < msf_fs_i_count; ++i) {
            var msf_input_s = msf_fs.querySelectorAll("input")[i];
            if (msf_input_s.getAttribute("type") === "button") {
                // nothing happens
            } else {
                if (msf_input_s.value === "") {
                    msf_val = false;
                } else {
                    if (msf_val === false) { } else {
                        msf_val = true;
                    }
                }
            };
        };
        if (msf_val === true) {
            // goes to the next step
            var selection = msf_getFsTag[msf_form_nr];
            selection.className = "msf_hide";
            msf_form_nr = msf_form_nr + 1;
            var selection = msf_getFsTag[msf_form_nr];
            selection.className = "msf_show";
            // refreshes the bullet
            var msf_bullets_a = msf_form_nr * msf_length + msf_form_nr;
            msf_bullets[msf_bullets_a].className += " msf_bullet_active";

            var divmap = document.getElementById("div-map");
            if (divmap) {
                var cls = divmap.getAttribute("class");

                if (cls === "msf_show") {

                    mapactive();


                }
            }
        }
    };

    // goes one step back
    function msf_btn_back() {
        msf_getFsTag[msf_form_nr].className = "msf_hide";
        msf_form_nr = msf_form_nr - 1;
        msf_getFsTag[msf_form_nr].className = "msf_showhide";
    };
}

///* MApa registro*/
function mapactive() {

    var tileLayer = new L.tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data & copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'your.mapbox.access.token'
    });


    var map = new L.Map('map', {

        'center': [-0.18205139994814276, -78.46831482728689],
        'zoom': 12,
        'layers': [tileLayer]
    });

    var marker = L.marker([-0.18205139994814276, -78.46831482728689], {
        draggable: true
    }).addTo(map);


    marker.on('dragend', function (e) {
        document.getElementById('latitud').value = marker.getLatLng().lat;
        document.getElementById('longitud').value = marker.getLatLng().lng;
    });



};


/* FIn MAPA registro*/


//* Carrusel*/

window.addEventListener('load', function () {
    // Mobile-first defaults

    if (carousel = document.querySelector('.carousel__lista')) {

        new Glider(document.querySelector('.carousel__lista'), {
            slidesToShow: 1,
            slidesToScroll: 1,
            dots: '.carousel__indicadores',
            draggable: 'true',
            arrows: {
                prev: '.carousel__anterior',
                next: '.carousel__siguiente'
            },
            responsive: [
                {
                    // screens greater than >= 775px
                    breakpoint: 450,
                    settings: {
                        // Set to `auto` and provide item width to adjust to viewport
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }, {
                    // screens greater than >= 1024px
                    breakpoint: 800,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });
    }
});

/*pequeño Js para enseñar las estrellas segun valoracion que trae @ViewBag.promedio*/
var rating = document.getElementsByClassName("starrr");
var f = document.getElementById("resstrella");

//for (var a = 0; a < rating.length; a++) {

//    $(rating[a]).starrr({

//        readOnly: true,

//        rating: rating[a].getAttribute("data-rating")
//    });
//}

/* Cambiar numeros flotantes.*/
function financial(x) {
    return Number.parseFloat(x).toFixed(1);
}
comentarios = document.querySelectorAll('#comentarios-conteo');

var rar = $('#miDiv').data('promedio')
ror = financial(rar).trim();
/* Cantidad de votos formatiados e impresos dinamicamente.*/
$('#cantidad-votos').text(ror + " " + "votos");
/*Cantidad de comentarios.*/
let ii = 0
while (ii < comentarios.length) {
    ii++;
}
$('#reviews-clientes').text(ii + "" + " reviews de los clientes.");

/*   Cambiar numeros decimal a horas*/

function decimalAHora(decimal) {
    let horas = Math.floor(decimal), // Obtenemos la parte entera
        restoHoras = Math.floor(decimal % 1 * 100), // Obtenemos la parde decimal
        decimalMinutos = restoHoras * 60 / 100, // Obtenemos los minutos expresado en decimal

        minutos = Math.floor(decimalMinutos), // Obtenemos la parte entera
        restoMins = Math.floor(decimalMinutos % 1 * 100), // Obtenemos la parde decimal
        segundos = Math.floor(restoMins * 60 / 100); // Obtenemos los segundos expresado en entero

    return `${('00' + horas).slice(-2)}:${('00' + minutos).slice(-2)}:${('00' + segundos).slice(-2)}`;
}
/* FIN Cambiar numeros decimal a horas.*/




var Hsemanaini = $('#l-v').data('semanaini');
var hsemanafin = $('#l-v').data('semanafin');
var hsi = $('#sabado').data('findesemanaini');
var hsf = $('#sabado').data('findesemanafin');
var hdi = $('#domingo').data('domingoini');
var hdf = $('#domingo').data('domingofin');


hini = decimalAHora(Hsemanaini);
h = hini.substr(0, 2);
m = hini.substr(3, 3);

hfin = decimalAHora(hsemanafin);
hcierre = hfin.substr(0, 2);
mcierre = hfin.substr(3, 3);

hsabini = decimalAHora(hsi);
hsabi = hsabini.substr(0, 2);
msabi = hsabini.substr(3, 3);

hsabfin = decimalAHora(hsf);
hsabf = hsabfin.substr(0, 2);
msabf = hsabfin.substr(3, 3);

hdomini = decimalAHora(hdi);
hdomi = hdomini.substr(0, 2);
mdomi = hdomini.substr(3, 3);

hdomfin = decimalAHora(hdf);
hdomf = hdomfin.substr(0, 2);
mdomf = hdomfin.substr(3, 3);


/*Evaluar si la hora es AM o PM */
var ampm = h >= 12 ? 'pm' : 'am';
var ampmcierre = hcierre >= 12 ? 'pm' : 'am';
var ampmsabini = hsabi >= 12 ? 'pm' : 'am';
var ampmcsabcie = hsabf >= 12 ? 'pm' : 'am';
var ampmcdomini = hdomi >= 12 ? 'pm' : 'am';
var ampmcdomfin = hdomf >= 12 ? 'pm' : 'am';


/*FIN Formatear fechas AM y PM */
const horas_semanales = {
    '13': '1',
    '14': '2',
    '15': '3',
    '16': '4',
    '17': '5',
    '18': '6',
    '19': '7',
    '20': '8',
    '21': '9',
    '22': '10',
    '23': '11',
    '24': '12'
}
const hsemini = horas_semanales[h] || h;
const hsemfin = horas_semanales[hcierre] || hcierre;
const hsabadoini = horas_semanales[hsabi] || hsabi;
const hsabadofin = horas_semanales[hsabf] || hsabf;
const hdomingoini = horas_semanales[hdomi] || hdomi;
const hsdomingofin = horas_semanales[hdomf] || hdomf;


/*para imprimir el formato*/
formatinicio = hsemini + ":" + m + "" + ampm;
formatcierre = hsemfin + ":" + mcierre + "" + ampmcierre;
formatiniciosab = hsabadoini + ":" + msabi + "" + ampmsabini;
formatcierresab = hsabadofin + ":" + msabf + "" + ampmcsabcie;
formatiniciodom = hdomingoini + ":" + mdomi + "" + ampmcdomini;
formatcierredom = hsdomingofin + ":" + mdomf + "" + ampmcdomfin;
/*Imprimir horas con formato*/
$('#l-v').text("Lunes a Viernes:" + " " + formatinicio + " " + "-" + " " + formatcierre);
$('#sabado').text("Sabado:" + " " + formatiniciosab + " " + "-" + " " + formatcierresab);
$('#domingo').text("Domingo:" + " " + formatiniciodom + " " + "-" + " " + formatcierredom);


/*Activador de datos mas informacion detalles.*/

const det = document.getElementById(`more-information`);

if (det) {
    const pdetalles = document.getElementById(`pdetalles`).getAttribute("data-rating");
    function resolveAfter2Seconds() {
        return new Promise(resolve => {

            resolve('resolved');
            if (pdetalles !== "") {
                det.classList.add('show');

            }

        });
    }
}
/* Fin Activador de datos mas innformacion detalles.*/

tiempo = $('#tiempo').data('tiempo')
if (tiempo) {
    t = Math.trunc(tiempo)
    document.getElementById("tiempo").innerHTML = "<b>Tiempo:</b>" + " " + t + " " + "hora";
}




/*MAPA para servicio*/

if (pathNa === '/AspNetUsers/Details/') {
    var latitud = $('#latitud').data('latitud');
    var longitud = $('#longitud').data('longitud');
    var nombre = $('#nombre-negocio').data('nombre');
    var map = L.map('map').setView([latitud, longitud], 15);
    L.marker([latitud, longitud]).addTo(map)
        .bindPopup(nombre)
        .openPopup();
    L.tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data & copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'your.mapbox.access.token'
    }).addTo(map);


    /* FIN MAPA*/


}

/*MAPA edita informacion*/

if (pathNa === '/AspNetUsers/Edit/') {

    var latitud = $('#latitud').data('latitud');
    var longitud = $('#longitud').data('longitud');
    var nombre = $('#nombre-negocio').data('nombre');


    var tileLayer = new L.tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data & copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'your.mapbox.access.token'
    });


    var map = new L.Map('map', {
        'setView': [latitud, longitud],
        'center': [latitud, longitud],
        'zoom': 12,
        'layers': [tileLayer]
    });

    var marker = L.marker([latitud, longitud], {
        draggable: true
    }).addTo(map);


    marker.on('dragend', function (e) {
        document.getElementById('Latitud').text = marker.getLatLng().lat;
        console.log(document.getElementById('Latitud').value = marker.getLatLng().lat)
        document.getElementById('Longitud').value = marker.getLatLng().lng;
        console.log(document.getElementById('Longitud').text = marker.getLatLng().lng)
    });

    /* FIN MAPA*/


}
if (pathname === '/AspNetUsers/Edit') {

    var latitud = $('#latitud').data('latitud');
    var longitud = $('#longitud').data('longitud');
    var nombre = $('#nombre-negocio').data('nombre');


    var tileLayer = new L.tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data & copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'your.mapbox.access.token'
    });


    var map = new L.Map('map', {
        'setView': [latitud, longitud],
        'center': [latitud, longitud],
        'zoom': 12,
        'layers': [tileLayer]
    });

    var marker = L.marker([latitud, longitud], {
        draggable: true
    }).addTo(map);


    marker.on('dragend', function (e) {
        document.getElementById('Latitud').text = marker.getLatLng().lat;
        console.log(document.getElementById('Latitud').value = marker.getLatLng().lat)
        document.getElementById('Longitud').value = marker.getLatLng().lng;
        console.log(document.getElementById('Longitud').text = marker.getLatLng().lng)
    });

    /* FIN MAPA*/


}


if (pathNa === '/Aspnetusers/Edit/') {

    var latitud = $('#latitud').data('latitud');
    var longitud = $('#longitud').data('longitud');
    var nombre = $('#nombre-negocio').data('nombre');


    var tileLayer = new L.tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data & copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'your.mapbox.access.token'
    });


    var map = new L.Map('map', {
        'setView': [latitud, longitud],
        'center': [latitud, longitud],
        'zoom': 12,
        'layers': [tileLayer]
    });

    var marker = L.marker([latitud, longitud], {
        draggable: true
    }).addTo(map);


    marker.on('dragend', function (e) {
        document.getElementById('Latitud').text = marker.getLatLng().lat;
        console.log(document.getElementById('Latitud').value = marker.getLatLng().lat)
        document.getElementById('Longitud').value = marker.getLatLng().lng;
        console.log(document.getElementById('Longitud').text = marker.getLatLng().lng)
    });

    /* FIN MAPA*/
}



$(document).ready(function () {

    // Creamos el array con los perfiles que existe para trabajar con nosotros
    var Quito = [
        { display: "5 esquinas", value: "5 esquinas" },
        { display: "Alangasí", value: "Alangasí" },
        { display: "Atucucho", value: "Atucucho" },
        { display: "Bellavista", value: "Bellavista" },
        { display: "Carcelén", value: "Carcelén" },
        { display: "Caupichu", value: "Caupichu" },
        { display: "Centro Histórico", value: "Centro Histórico" },
        { display: "Chilibulo", value: "Chilibulo" },
        { display: "Chillogallo", value: "Chillogallo" },
        { display: "Chimbacalle", value: "Chimbacalle" },
        { display: "Ciudadela del Ejército", value: "Ciudadela del Ejército" },
        { display: "Ciudadela Ibarra", value: "Ciudadela Ibarra" },
        { display: "Comité del Pueblo", value: "Comité del Pueblo" },
        { display: "Conocoto", value: "Conocoto" },
        { display: "Cornejo", value: "Cornejo" },
        { display: "Cotocollao", value: "Cotocollao" },
        { display: "Cumbayá", value: "Cumbayá" },
        { display: "El Batán", value: "El Batán" },
        { display: "El Beaterio", value: "El Beaterio" },
        { display: "El Calzado", value: "El Calzado" },
        { display: "El Camal", value: "El Camal" },
        { display: "El Condado", value: "El Condado" },
        { display: "El Dorado", value: "El Dorado" },
        { display: "El Ejido", value: "El Ejido" },
        { display: "El Inca", value: "El Inca" },
        { display: "El Panecillo", value: "El Panecillo" },
        { display: "El Pintado", value: "El Pintado" },
        { display: "El Tejar", value: "El Tejar" },
        { display: "El Troje", value: "El Troje" },
        { display: "Guajalo", value: "Guajalo" },
        { display: "Guamaní", value: "Guamaní" },
        { display: "Guápulo", value: "Guápulo" },
        { display: "Iñaquito", value: "Iñaquito" },
        { display: "Kennedy", value: "Kennedy" },
        { display: "La Argelia", value: "La Argelia" },
        { display: "La Bota", value: "La Bota" },
        { display: "La Ecuatoriana", value: "La Ecuatoriana" },
        { display: "La Ferroviaria", value: "La Ferroviaria" },
        { display: "La Floresta", value: "La Floresta" },
        { display: "La Florida", value: "La Florida" },
        { display: "La Forestal", value: "La Forestal" },
        { display: "La González Suárez", value: "La González Suárez" },
        { display: "La Guaragua", value: "La Guaragua" },
        { display: "La Libertad", value: "La Libertad" },
        { display: "La Loma Grande", value: "La Loma Grande" },
        { display: "La Magdalena", value: "La Magdalena" },
        { display: "La Marín", value: "La Marín" },
        { display: "La Mariscal", value: "La Mariscal" },
        { display: "La Mena", value: "La Mena" },
        { display: "La Ronda", value: "La Ronda" },
        { display: "La Tola", value: "La Tola" },
        { display: "La Vicentina", value: "La Vicentina" },
        { display: "La Victoria", value: "La Victoria" },
        { display: "Las Casas", value: "Las Casas" },
        { display: "Lucha de los Pobres", value: "Lucha de los Pobres" },
        { display: "Luluncoto", value: "Luluncoto" },
        { display: "Manuelita Saenz", value: "Manuelita Saenz" },
        { display: "Mena de Hierro", value: "Mena de Hierro" },
        { display: "Miraflores", value: "Miraflores" },
        { display: "Monjas", value: "Monjas" },
        { display: "Nueva Aurora", value: "Nueva Aurora" },
        { display: "Oriente Quiteño", value: "Oriente Quiteño" },
        { display: "Pifo", value: "Pifo" },
        { display: "Ponceano", value: "Ponceano" },
        { display: "Puembo", value: "Puembo" },
        { display: "Puengasí", value: "Puengasí" },
        { display: "Quito Norte", value: "Quito Norte" },
        { display: "Quito Sur", value: "Quito Sur" },
        { display: "Quito Tennis", value: "Quito Tennis" },
        { display: "Quitumbe", value: "Quitumbe" },
        { display: "Reino de Quito", value: "Reino de Quito" },
        { display: "Rumiñahui", value: "Rumiñahui" },
        { display: "San", value: "San" },
        { display: "San Bartolo", value: "San Bartolo" },
        { display: "San Carlos", value: "San Carlos" },
        { display: "San Diego", value: "San Diego" },
        { display: "San Juan", value: "San Juan" },
        { display: "San Marcos", value: "San Marcos" },
        { display: "San Matin", value: "San Matin" },
        { display: "San Rafael", value: "San Rafael" },
        { display: "Santa Rita", value: "Santa Rita" },
        { display: "Solanda", value: "Solanda" },
        { display: "Tababela", value: "Tababela" },
        { display: "Toctiuco", value: "Toctiuco" },
        { display: "Tumbaco", value: "Tumbaco" },
        { display: "Turubamba", value: "Turubamba" },
        { display: "Villaflora", value: "Villaflora" }
    ];

 
    var Aguarico = [
        { display: "Tipitini", value: "Tipitini" },
        { display: "Nuevo Rocafuerte", value: "Nuevo Rocafuerte" },
        { display: "Capitán Augusto Rivadeneyra", value: "Capitán Augusto Rivadeneyra" },
        { display: "Cononaco", value: "Cononaco" },
        { display: "Santa María De Huiririma", value: "Santa María De Huiririma" },
        { display: "Tiputini", value: "Tiputini" },
        { display: "Yasuní", value: "Yasuní" }
    ];

    var Alausi = [
        { display: "Alausí", value: "Alausí" },
        { display: "Achupallas", value: "Achupallas" },
        { display: "Cumandá", value: "Cumandá" },
        { display: "Guasuntos", value: "Guasuntos" },
        { display: "Huigra", value: "Huigra" },
        { display: "Multitud", value: "Multitud" },
        { display: "Pistishí (Nariz Del Diablo)", value: "Pistishí (Nariz Del Diablo)" },
        { display: "Pumallacta", value: "Pumallacta" },
        { display: "Sevilla", value: "Sevilla" },
        { display: "Sibambe", value: "Sibambe" },
        { display: "Tixán", value: "Tixán" }

    ];
    var Ambato = [
        { display: "Atocha – Ficoa", value: "Atocha – Ficoa" },
        { display: "Celiano Monge", value: "Celiano Monge" },
        { display: "Huachi Chico", value: "Huachi Chico" },
        { display: "Huachi Loreto", value: "Huachi Loreto" },
        { display: "La Merced", value: "La Merced" },
        { display: "La Península", value: "La Península" },
        { display: "Matriz", value: "Matriz" },
        { display: "Pishilata", value: "Pishilata" },
        { display: "San Francisco", value: "San Francisco" },
        { display: "Ambato", value: "Ambato" },
        { display: "Ambatillo", value: "Ambatillo" },
        { display: "Atahualpa (Chisalata)", value: "Atahualpa (Chisalata)" },
        { display: "Augusto N. Martínez (Mundugleo)", value: "Augusto N. Martínez (Mundugleo)" },
        { display: "Constantino Fernández (Cab. En Cullitahua)", value: "Constantino Fernández (Cab. En Cullitahua)" },
        { display: "Huachi Grande", value: "Huachi Grande" },
        { display: "Izamba", value: "Izamba" },
        { display: "Juan Benigno Vela", value: "Juan Benigno Vela" },
        { display: "Montalvo", value: "Montalvo" },
        { display: "Pasa", value: "Pasa" },
        { display: "Picaigua", value: "Picaigua" },
        { display: "Pilagüín (Pilahüín)", value: "Pilagüín (Pilahüín)" },
        { display: "Quisapincha (Quizapincha)", value: "Quisapincha (Quizapincha)" },
        { display: "San Bartolomé De Pinllog", value: "San Bartolomé De Pinllog" },
        { display: "San Fernando (Pasa San Fernando)", value: "San Fernando (Pasa San Fernando)" },
        { display: "Santa Rosa", value: "Santa Rosa" },
        { display: "Totoras", value: "Totoras" },
        { display: "Cunchibamba", value: "Cunchibamba" },
        { display: "Unamuncho", value: "Unamuncho" }


    ];

    var Antonio_Ante = [{ display: "Andrade Marín (Lourdes)", value: "Andrade Marín (Lourdes)" },
    { display: "Atuntaqui", value: "Atuntaqui" },
    { display: "Imbaya (San Luis De Cobuendo)", value: "Imbaya (San Luis De Cobuendo)" },
    { display: "San Francisco De Natabuela", value: "San Francisco De Natabuela" },
    { display: "San José De Chaltura", value: "San José De Chaltura" },
    { display: "San Roque", value: "San Roque" }


    ];
    var Arajuno = [

        { display: "Arajuno", value: "Arajuno" },
        { display: "Curaray", value: "Curaray" }

    ];
    var Archidona = [
        { display: "Archidona", value: "Archidona" },
        { display: "Avila", value: "Avila" },
        { display: "Cotundo", value: "Cotundo" },
        { display: "Loreto", value: "Loreto" },
        { display: "San Pablo De Ushpayacu", value: "San Pablo De Ushpayacu" },
        { display: "Puerto Murialdo", value: "Puerto Murialdo" }


    ];

    var Arenillas = [
        { display: "Arenillas", value: "Arenillas" },
        { display: "Chacras", value: "Chacras" },
        { display: "La Libertad", value: "La Libertad" },
        { display: "Las Lajas (Cab. En La Victoria)", value: "Las Lajas (Cab. En La Victoria)" },
        { display: "Palmales", value: "Palmales" },
        { display: "Carcabón", value: "Carcabón" }

    ];
    var Arlos_Julio_Arosemena_Tol = [
        { display: "Carlos Julio Arosemena Tola", value: "Carlos_Julio_Arosemena_Tola" }


    ];
    var Atacames = [{ display: "Atacames", value: "Atacames" }, { display: "La Unión", value: "La Unión" }, { display: "Súa (Cab. En La Bocana)", value: "Súa (Cab. En La Bocana)" }, { display: "Tonchigüe", value: "Tonchigüe" }, { display: "Tonsupa", value: "Tonsupa" }];

    var Atahualpa = [{ display: "Paccha", value: "Paccha" }, { display: "Ayapamba", value: "Ayapamba" }, { display: "Cordoncillo", value: "Cordoncillo" }, { display: "Milagro", value: "Milagro" }, { display: "San José", value: "San José" }, { display: "San Juan De Cerro Azul", value: "San Juan De Cerro Azul" }];


    var Baba = [{ display: "Baba", value: "Baba" }, { display: "Guare", value: "Guare" }, { display: "Isla De Bejucal", value: "Isla De Bejucal" }, { display: "Baba", value: "Baba" }, { display: "Guare", value: "Guare" }, { display: "Isla De Bejucal", value: "Isla De Bejucal" }, { display: "Baba", value: "Baba" }, { display: "Guare", value: "Guare" }, { display: "Isla De Bejucal", value: "Isla De Bejucal" }, { display: "Baba", value: "Baba" }, { display: "Guare", value: "Guare" }];
    var Babahoyo = [{ display: "Clemente Baquerizo", value: "Clemente Baquerizo" }, { display: "Dr. Camilo Ponce", value: "Dr. Camilo Ponce" }, { display: "Barreiro", value: "Barreiro" }, { display: "El Salto", value: "El Salto" }, { display: "Babahoyo", value: "Babahoyo" }, { display: "Barreiro (Santa Rita)", value: "Barreiro (Santa Rita)" }, { display: "Caracol", value: "Caracol" }, { display: "Febres Cordero (Las Juntas)", value: "Febres Cordero (Las Juntas)" }, { display: "Pimocha", value: "Pimocha" }, { display: "La Unión", value: "La Unión" }];
    var Balao = [{ display: "Balao", value: "Balao" }];
    var Balsas = [{ display: "Balsas", value: "Balsas" }, { display: "Bellamaría", value: "Bellamaría" }];
    var Balzar = [{ display: "Balzar", value: "Balzar" }];
    var Baños_De_Agua_Santa = [{ display: "Baños De Agua Santa", value: "Baños De Agua Santa" }, { display: "Lligua", value: "Lligua" }, { display: "Río Negro", value: "Río Negro" }, { display: "Río Verde", value: "Río Verde" }, { display: "Ulba", value: "Ulba" }];
    var Biblian = [{ display: "Biblián", value: "Biblián" }, { display: "Nazón (Cab. En Pampa De Domínguez)", value: "Nazón (Cab. En Pampa De Domínguez)" }, { display: "San Francisco De Sageo", value: "San Francisco De Sageo" }, { display: "Turupamba", value: "Turupamba" }, { display: "Jerusalén", value: "Jerusalén" }];
    var Bolivar = [{ display: "Bolívar", value: "Bolívar" }, { display: "García Moreno", value: "García Moreno" }, { display: "Los Andes", value: "Los Andes" }, { display: "Monte Olivo", value: "Monte Olivo" }, { display: "San Vicente De Pusir", value: "San Vicente De Pusir" }, { display: "San Rafael", value: "San Rafael" }, { display: "Calceta", value: "Calceta" }, { display: "Membrillo", value: "Membrillo" }, { display: "Quiroga", value: "Quiroga" }];
    var Buena_Fe = [{ display: "San Jacinto De Buena Fé", value: "San Jacinto De Buena Fé" }, { display: "7 De Agosto", value: "7 De Agosto" }, { display: "11 De Octubre", value: "11 De Octubre" }, { display: "San Jacinto De Buena Fé", value: "San Jacinto De Buena Fé" }, { display: "Patricia Pilar", value: "Patricia Pilar" }];
    var Caluma = [{ display: "Caluma", value: "Caluma" }];
    var Calvas = [{ display: "Cariamanga", value: "Cariamanga" }, { display: "Chile", value: "Chile" }, { display: "San Vicente", value: "San Vicente" }, { display: "Cariamanga", value: "Cariamanga" }, { display: "Colaisaca", value: "Colaisaca" }, { display: "El Lucero", value: "El Lucero" }, { display: "Utuana", value: "Utuana" }, { display: "Sanguillín", value: "Sanguillín" }];
    var Camilo_Ponce_Enriquez = [{ display: "Camilo Ponce Enríquez", value: "Camilo Ponce Enríquez" }, { display: "El Carmen De Pijilí", value: "El Carmen De Pijilí" }];
    var Cañar = [{ display: "Cañar", value: "Cañar" }, { display: "Chontamarca", value: "Chontamarca" }, { display: "Chorocopte", value: "Chorocopte" }, { display: "General Morales (Socarte)", value: "General Morales (Socarte)" }, { display: "Gualleturo", value: "Gualleturo" }, { display: "Honorato Vásquez (Tambo Viejo)", value: "Honorato Vásquez (Tambo Viejo)" }, { display: "Ingapirca", value: "Ingapirca" }, { display: "Juncal", value: "Juncal" }, { display: "San Antonio", value: "San Antonio" }, { display: "Suscal", value: "Suscal" }, { display: "Tambo", value: "Tambo" }, { display: "Zhud", value: "Zhud" }, { display: "Ventura", value: "Ventura" }, { display: "Ducur", value: "Ducur" }];
    var Cascales = [{ display: "El Dorado De Cascales", value: "El Dorado De Cascales" }, { display: "Santa Rosa De Sucumbíos", value: "Santa Rosa De Sucumbíos" }, { display: "Sevilla", value: "Sevilla" }];
    var Catamayo = [{ display: "Catamayo", value: "Catamayo" }, { display: "San José", value: "San José" }, { display: "Catamayo (La Toma)", value: "Catamayo (La Toma)" }, { display: "El Tambo", value: "El Tambo" }, { display: "Guayquichuma", value: "Guayquichuma" }, { display: "San Pedro De La Bendita", value: "San Pedro De La Bendita" }, { display: "Zambi", value: "Zambi" }];
    var Cayambe = [{ display: "Ayora", value: "Ayora" }, { display: "Cayambe", value: "Cayambe" }, { display: "Juan Montalvo", value: "Juan Montalvo" }, { display: "Cayambe", value: "Cayambe" }, { display: "Ascázubi", value: "Ascázubi" }, { display: "Cangahua", value: "Cangahua" }, { display: "Olmedo (Pesillo)", value: "Olmedo (Pesillo)" }, { display: "Otón", value: "Otón" }, { display: "Santa Rosa De Cuzubamba", value: "Santa Rosa De Cuzubamba" }];
    var Celica = [{ display: "Celica", value: "Celica" }, { display: "Cruzpamba (Cab. En Carlos Bustamante)", value: "Cruzpamba (Cab. En Carlos Bustamante)" }, { display: "Chaquinal", value: "Chaquinal" }, { display: "12 De Diciembre (Cab. En Achiotes)", value: "12 De Diciembre (Cab. En Achiotes)" }, { display: "Pindal (Federico Páez)", value: "Pindal (Federico Páez)" }, { display: "Pozul (San Juan De Pozul)", value: "Pozul (San Juan De Pozul)" }, { display: "Sabanilla", value: "Sabanilla" }, { display: "Tnte. Maximiliano Rodríguez Loaiza", value: "Tnte. Maximiliano Rodríguez Loaiza" }];
    var Centinela_Del_Cóndor = [{ display: "Zumbi", value: "Zumbi" }, { display: "Paquisha", value: "Paquisha" }, { display: "Triunfo-Dorado", value: "Triunfo-Dorado" }, { display: "Panguintza", value: "Panguintza" }];
    var Cevallos = [{ display: "Cevallos", value: "Cevallos" }];
    var Chaguarpamba = [{ display: "Chaguarpamba", value: "Chaguarpamba" }, { display: "Buenavista", value: "Buenavista" }, { display: "El Rosario", value: "El Rosario" }, { display: "Santa Rufina", value: "Santa Rufina" }, { display: "Amarillos", value: "Amarillos" },];
    var Chambo = [{ display: "Chambo", value: "Chambo" }];
    var Chilla = [{ display: "Chilla", value: "Chilla" },];
    var Chillanes = [{ display: "Chillanes", value: "Chillanes" }, { display: "San José Del Tambo (Tambopamba)", value: "San José Del Tambo (Tambopamba)" }];
    var Chimbo = [{ display: "San José De Chimbo", value: "San José De Chimbo" }, { display: "Asunción (Asancoto)", value: "Asunción (Asancoto)" }, { display: "Caluma", value: "Caluma" }, { display: "Magdalena (Chapacoto)", value: "Magdalena (Chapacoto)" }, { display: "San Sebastián", value: "San Sebastián" }, { display: "Telimbela", value: "Telimbela" }];
    var Chinchipe = [{ display: "Zumba", value: "Zumba" }, { display: "Chito", value: "Chito" }, { display: "El Chorro", value: "El Chorro" }, { display: "El Porvenir Del Carmen", value: "El Porvenir Del Carmen" }, { display: "La Chonta", value: "La Chonta" }, { display: "Palanda", value: "Palanda" }, { display: "Pucapamba", value: "Pucapamba" }, { display: "San Francisco Del Vergel", value: "San Francisco Del Vergel" }, { display: "Valladolid", value: "Valladolid" }, { display: "San Andrés", value: "San Andrés" }];
    var Chone = [{ display: "Chone", value: "Chone" }, { display: "Santa Rita", value: "Santa Rita" }, { display: "Boyacá", value: "Boyacá" }, { display: "Canuto", value: "Canuto" }, { display: "Convento", value: "Convento" }, { display: "Chibunga", value: "Chibunga" }, { display: "Eloy Alfaro", value: "Eloy Alfaro" }, { display: "Ricaurte", value: "Ricaurte" }, { display: "San Antonio", value: "San Antonio" }];
    var Chordeleg = [{ display: "Chordeleg", value: "Chordeleg" }, { display: "Principal", value: "Principal" }, { display: "La Unión", value: "La Unión" }, { display: "Luis Galarza Orellana (Cab.En Delegsol)", value: "Luis Galarza Orellana (Cab.En Delegsol)" }, { display: "San Martín De Puzhio", value: "San Martín De Puzhio" }];
    var Chunchi = [{ display: "Chunchi", value: "Chunchi" }, { display: "Capzol", value: "Capzol" }, { display: "Compud", value: "Compud" }, { display: "Gonzol", value: "Gonzol" }, { display: "Llagos", value: "Llagos" }];
    var Colimes = [{ display: "Colimes", value: "Colimes" }, { display: "San Jacinto", value: "San Jacinto" }];
    var Colta = [{ display: "Cajabamba", value: "Cajabamba" }, { display: "Sicalpa", value: "Sicalpa" }, { display: "Villa La Unión (Cajabamba)", value: "Villa La Unión (Cajabamba)" }, { display: "Cañi", value: "Cañi" }, { display: "Columbe", value: "Columbe" }, { display: "Juan De Velasco (Pangor)", value: "Juan De Velasco (Pangor)" }, { display: "Santiago De Quito (Cab. En San Antonio De Quito)", value: "Santiago De Quito (Cab. En San Antonio De Quito)" }];
    var Cotacachi = [{ display: "Sagrario", value: "Sagrario" }, { display: "San Francisco", value: "San Francisco" }, { display: "Cotacachi", value: "Cotacachi" }, { display: "Apuela", value: "Apuela" }, { display: "García Moreno (Llurimagua)", value: "García Moreno (Llurimagua)" }, { display: "Imantag", value: "Imantag" }, { display: "Peñaherrera", value: "Peñaherrera" }, { display: "Plaza Gutiérrez (Calvario)", value: "Plaza Gutiérrez (Calvario)" }, { display: "Quiroga", value: "Quiroga" }, { display: "6 De Julio De Cuellaje (Cab. En Cuellaje)", value: "6 De Julio De Cuellaje (Cab. En Cuellaje)" }, { display: "Vacas Galindo (El Churo) (Cab.En San Miguel Alto", value: "Vacas Galindo (El Churo) (Cab.En San Miguel Alto" }];
    var Cuenca = [{ display: "Bellavista", value: "Bellavista" }, { display: "Cañaribamba", value: "Cañaribamba" }, { display: "El Batán", value: "El Batán" }, { display: "El Sagrario", value: "El Sagrario" }, { display: "El Vecino", value: "El Vecino" }, { display: "Gil Ramírez Dávalos", value: "Gil Ramírez Dávalos" }, { display: "Huaynacápac", value: "Huaynacápac" }, { display: "Machángara", value: "Machángara" }, { display: "Monay", value: "Monay" }, { display: "San Blas", value: "San Blas" }, { display: "San Sebastián", value: "San Sebastián" }, { display: "Sucre", value: "Sucre" }, { display: "Totoracocha", value: "Totoracocha" }, { display: "Yanuncay", value: "Yanuncay" }, { display: "Hermano Miguel", value: "Hermano Miguel" }, { display: "Cuenca", value: "Cuenca" }, { display: "Baños", value: "Baños" }, { display: "Cumbe", value: "Cumbe" }, { display: "Chaucha", value: "Chaucha" }, { display: "Checa (Jidcay)", value: "Checa (Jidcay)" }, { display: "Chiquintad", value: "Chiquintad" }, { display: "Llacao", value: "Llacao" }, { display: "Molleturo", value: "Molleturo" }, { display: "Nulti", value: "Nulti" }, { display: "Octavio Cordero Palacios (Santa Rosa)", value: "Octavio Cordero Palacios (Santa Rosa)" }, { display: "Paccha", value: "Paccha" }, { display: "Quingeo", value: "Quingeo" }, { display: "Ricaurte", value: "Ricaurte" }, { display: "San Joaquín", value: "San Joaquín" }, { display: "Santa Ana", value: "Santa Ana" }, { display: "Sayausí", value: "Sayausí" }, { display: "Sidcay", value: "Sidcay" }, { display: "Sinincay", value: "Sinincay" }, { display: "Tarqui", value: "Tarqui" }, { display: "Turi", value: "Turi" }, { display: "Valle", value: "Valle" }, { display: "Victoria Del Portete (Irquis)", value: "Victoria Del Portete (Irquis)" }];
    var Cumanda = [{ display: "Cumandá", value: "Cumandá" },];
    var Cuyabeno = [{ display: "Tarapoa", value: "Tarapoa" }, { display: "Cuyabeno", value: "Cuyabeno" }, { display: "Aguas Negras", value: "Aguas Negras" }];
    var Daule = [{ display: "Daule", value: "Daule" }, { display: "La Aurora (Satélite)", value: "La Aurora (Satélite)" }, { display: "Banife", value: "Banife" }, { display: "Emiliano Caicedo Marcos", value: "Emiliano Caicedo Marcos" }, { display: "Magro", value: "Magro" }, { display: "Padre Juan Bautista Aguirre", value: "Padre Juan Bautista Aguirre" }, { display: "Santa Clara", value: "Santa Clara" }, { display: "Vicente Piedrahita", value: "Vicente Piedrahita" }, { display: "Daule", value: "Daule" }, { display: "Isidro Ayora (Soledad)", value: "Isidro Ayora (Soledad)" }, { display: "Juan Bautista Aguirre (Los Tintos)", value: "Juan Bautista Aguirre (Los Tintos)" }, { display: "Laurel", value: "Laurel" }, { display: "Limonal", value: "Limonal" }, { display: "Lomas De Sargentillo", value: "Lomas De Sargentillo" }, { display: "Los Lojas (Enrique Baquerizo Moreno)", value: "Los Lojas (Enrique Baquerizo Moreno)" }, { display: "Piedrahita (Nobol)", value: "Piedrahita (Nobol)" }];
    var Déleg = [{ display: "Déleg", value: "Déleg" }, { display: "Solano", value: "Solano" }];
    var Do_Baquerizo_Moreno = [{ display: "Alfredo Baquerizo Moreno (Juján)", value: "Alfredo Baquerizo Moreno (Juján)" }];
    var Duran = [{ display: "Eloy Alfaro", value: "Eloy Alfaro " }, { display: "El Recreo", value: "El Recreo" }, { display: "Eloy Alfaro", value: "Eloy Alfaro" }];
    var Echeandía = [{ display: "Echeandía", value: "Echeandía" }];
    var El_Carmen = [{ display: "El Carmen", value: "El Carmen" }, { display: "4 De Diciembre", value: "4 De Diciembre" }, { display: "El Carmen", value: "El Carmen" }, { display: "Wilfrido Loor Moreira (Maicito)", value: "Wilfrido Loor Moreira (Maicito)" }, { display: "San Pedro De Suma", value: "San Pedro De Suma" },];
    var El_Chaco = [{ display: "El Chaco", value: "El Chaco" }, { display: "Gonzalo Díaz De Pineda (El Bombón)", value: "Gonzalo Díaz De Pineda (El Bombón)" }, { display: "Linares", value: "Linares" }, { display: "Oyacachi", value: "Oyacachi" }, { display: "Santa Rosa", value: "Santa Rosa" }, { display: "Sardinas", value: "Sardinas" },];
    var El_Empalme = [{ display: "Velasco Ibarra (El Empalme)", value: "Velasco Ibarra (El Empalme)" }, { display: "Guayas (Pueblo Nuevo)", value: "Guayas (Pueblo Nuevo)" }, { display: "El Rosario", value: "El Rosario" },];
    var El_Guabo = [{ display: "El Guabo", value: "El Guabo" }, { display: "Barbones (Sucre)", value: "Barbones (Sucre)" }, { display: "La Iberia", value: "La Iberia" }, { display: "Tendales (Cab.En Puerto Tendales)", value: "Tendales (Cab.En Puerto Tendales)" }, { display: "Río Bonito", value: "Río Bonito" },];
    var El_Pan = [{ display: "El Pan", value: "El Pan" }, { display: "Amaluza", value: "Amaluza" }, { display: "Palmas", value: "Palmas" }, { display: "San Vicente", value: "San Vicente" }];
    var El_Pangui = [{ display: "El Pangui", value: "El Pangui" }, { display: "El Guisme", value: "El Guisme" }, { display: "Pachicutza", value: "Pachicutza" }, { display: "Tundayme", value: "Tundayme" },];
    var El_Piedrero = [{ display: "El Piedrero", value: "El Piedrero" }];
    var El_Tambo = [{ display: "El Tambo", value: "El Tambo" }];
    var El_Triunfo = [{ display: "El Triunfo", value: "El Triunfo" }];
    var Eloy_Alfaro = [{ display: "Valdez (Limones)", value: "Valdez (Limones)" }, { display: "Anchayacu", value: "Anchayacu" }, { display: "Atahualpa (Cab. En Camarones)", value: "Atahualpa (Cab. En Camarones)" }, { display: "Borbón", value: "Borbón" }, { display: "La Tola", value: "La Tola" }, { display: "Luis Vargas Torres (Cab. En Playa De Oro)", value: "Luis Vargas Torres (Cab. En Playa De Oro)" }, { display: "Maldonado", value: "Maldonado" }, { display: "Pampanal De Bolívar", value: "Pampanal De Bolívar" }, { display: "San Francisco De Onzole", value: "San Francisco De Onzole" }, { display: "Santo Domingo De Onzole", value: "Santo Domingo De Onzole" }, { display: "Selva Alegre", value: "Selva Alegre" }, { display: "Telembí", value: "Telembí" }, { display: "Colón Eloy Del María", value: "Colón Eloy Del María" }, { display: "San José De Cayapas", value: "San José De Cayapas" }, { display: "Timbiré", value: "Timbiré" },];
    var Esmeraldas = [{ display: "Bartolomé Ruiz (César Franco Carrión)", value: "Bartolomé Ruiz (César Franco Carrión)" }, { display: "5 De Agosto", value: "5 De Agosto" }, { display: "Esmeraldas", value: "Esmeraldas" }, { display: "Luis Tello (Las Palmas)", value: "Luis Tello (Las Palmas)" }, { display: "Simón Plata Torres", value: "Simón Plata Torres" }, { display: "Esmeraldas", value: "Esmeraldas" }, { display: "Atacames", value: "Atacames" }, { display: "Camarones (Cab. En San Vicente)", value: "Camarones (Cab. En San Vicente)" }, { display: "Crnel. Carlos Concha Torres (Cab.En Huele)", value: "Crnel. Carlos Concha Torres (Cab.En Huele)" }, { display: "Chinca", value: "Chinca" }, { display: "Chontaduro", value: "Chontaduro" }, { display: "Chumundé", value: "Chumundé" }, { display: "Lagarto", value: "Lagarto" }, { display: "La Unión", value: "La Unión" }, { display: "Majua", value: "Majua" }, { display: "Montalvo (Cab. En Horqueta)", value: "Montalvo (Cab. En Horqueta)" }, { display: "Río Verde", value: "Río Verde" }, { display: "Rocafuerte", value: "Rocafuerte" }, { display: "San Mateo", value: "San Mateo" }, { display: "Súa (Cab. En La Bocana)", value: "Súa (Cab. En La Bocana)" }, { display: "Tabiazo", value: "Tabiazo" }, { display: "Tachina", value: "Tachina" }, { display: "Tonchigüe", value: "Tonchigüe" }, { display: "Vuelta Larga", value: "Vuelta Larga" },];
    var Espejo = [{ display: "El Ángel", value: "El Ángel" }, { display: "27 De Septiembre", value: "27 De Septiembre" }, { display: "El Angel", value: "El Angel" }, { display: "El Goaltal", value: "El Goaltal" }, { display: "La Libertad (Alizo)", value: "La Libertad (Alizo)" }, { display: "San Isidro", value: "San Isidro" },];
    var Espíndola = [{ display: "Amaluza", value: "Amaluza" }, { display: "Bellavista", value: "Bellavista" }, { display: "Jimbura", value: "Jimbura" }, { display: "Santa Teresita", value: "Santa Teresita" }, { display: "27 De Abril (Cab. En La Naranja)", value: "27 De Abril (Cab. En La Naranja)" }, { display: "El Ingenio", value: "El Ingenio" }, { display: "El Airo", value: "El Airo" },];
    var Flavio_Alfaro = [{ display: "Flavio Alfaro", value: "Flavio Alfaro" }, { display: "San Francisco De Novillo (Cab. En", value: "San Francisco De Novillo (Cab. En" }, { display: "Zapallo", value: "Zapallo" },];
    var General_Antonio_Elizalde = [{ display: "General Antonio Elizalde (Bucay)", value: "General Antonio Elizalde (Bucay)" }];
    var Girón = [{ display: "Girón", value: "Girón" }, { display: "Asunción", value: "Asunción" }, { display: "San Gerardo", value: "San Gerardo" },];
    var Gonzalo_Pizarro = [{ display: "El Dorado De Cascales", value: "El Dorado De Cascales" }, { display: "El Reventador", value: "El Reventador" }, { display: "Gonzalo Pizarro", value: "Gonzalo Pizarro" }, { display: "Lumbaquí", value: "Lumbaquí" }, { display: "Puerto Libre", value: "Puerto Libre" }, { display: "Santa Rosa De Sucumbíos", value: "Santa Rosa De Sucumbíos" },];
    var Gonzanamá = [{ display: "Gonzanamá", value: "Gonzanamá" }, { display: "Changaimina (La Libertad)", value: "Changaimina (La Libertad)" }, { display: "Fundochamba", value: "Fundochamba" }, { display: "Nambacola", value: "Nambacola" }, { display: "Purunuma (Eguiguren)", value: "Purunuma (Eguiguren)" }, { display: "Quilanga (La Paz)", value: "Quilanga (La Paz)" }, { display: "Sacapalca", value: "Sacapalca" }, { display: "San Antonio De Las Aradas (Cab. En Las Aradas)", value: "San Antonio De Las Aradas (Cab. En Las Aradas)" },];
    var Guachapala = [{ display: "Guachapala", value: "Guachapala" },];
    var Gualaceo = [{ display: "Gualaceo", value: "Gualaceo" }, { display: "Chordeleg", value: "Chordeleg" }, { display: "Daniel Córdova Toral (El Oriente)", value: "Daniel Córdova Toral (El Oriente)" }, { display: "Jadán", value: "Jadán" }, { display: "Mariano Moreno", value: "Mariano Moreno" }, { display: "Principal", value: "Principal" }, { display: "Remigio Crespo Toral (Gúlag)", value: "Remigio Crespo Toral (Gúlag)" }, { display: "San Juan", value: "San Juan" }, { display: "Zhidmad", value: "Zhidmad" }, { display: "Luis Cordero Vega", value: "Luis Cordero Vega" }, { display: "Simón Bolívar (Cab. En Gañanzol)", value: "Simón Bolívar (Cab. En Gañanzol)" },];
    var Gualaquiza = [{ display: "Mercedes Molina", value: "Mercedes Molina" }, { display: "Gualaquiza", value: "Gualaquiza" }, { display: "Amazonas (Rosario De Cuyes)", value: "Amazonas (Rosario De Cuyes)" }, { display: "Bermejos", value: "Bermejos" }, { display: "Bomboiza", value: "Bomboiza" }, { display: "Chigüinda", value: "Chigüinda" }, { display: "El Rosario", value: "El Rosario" }, { display: "Nueva Tarqui", value: "Nueva Tarqui" }, { display: "San Miguel De Cuyes", value: "San Miguel De Cuyes" }, { display: "El Ideal", value: "El Ideal" }, { display: "General Leonidas Plaza Gutiérrez (Limón)", value: "General Leonidas Plaza Gutiérrez (Limón)" },];
    var Guamote = [{ display: "Cebadas", value: "Cebadas" }, { display: "Palmira", value: "Palmira" }, { display: "El Rosario", value: "El Rosario" },];
    var Guano = [{ display: "La Matriz", value: "La Matriz" }, { display: "Guano", value: "Guano" }, { display: "Guanando", value: "Guanando" }, { display: "Ilapo", value: "Ilapo" }, { display: "La Providencia", value: "La Providencia" }, { display: "San Andrés", value: "San Andrés" }, { display: "San Gerardo De Pacaicaguán", value: "San Gerardo De Pacaicaguán" }, { display: "San Isidro De Patulú", value: "San Isidro De Patulú" }, { display: "San José Del Chazo", value: "San José Del Chazo" }, { display: "Santa Fé De Galán", value: "Santa Fé De Galán" }, { display: "Valparaíso", value: "Valparaíso" }, { display: "Pallatanga", value: "Pallatanga" },];
    var Guaranda = [{ display: "Gabriel Ignacio Veintimilla", value: "Gabriel Ignacio Veintimilla" }, { display: "Guanujo", value: "Guanujo" }, { display: "Guaranda", value: "Guaranda" }, { display: "Facundo Vela", value: "Facundo Vela" }, { display: "Guanujo", value: "Guanujo" }, { display: "Julio E. Moreno (Catanahuán Grande)", value: "Julio E. Moreno (Catanahuán Grande)" }, { display: "Las Naves", value: "Las Naves" }, { display: "Salinas", value: "Salinas" }, { display: "San Lorenzo", value: "San Lorenzo" }, { display: "San Simón (Yacoto)", value: "San Simón (Yacoto)" }, { display: "Santa Fé (Santa Fé)", value: "Santa Fé (Santa Fé)" }, { display: "Simiátug", value: "Simiátug" }, { display: "San Luis De Pambil", value: "San Luis De Pambil" }, { display: "Chillanes", value: "Chillanes" },];
    var Guayaquil = [{ display: "Bolívar (Sagrario)", value: "Bolívar (Sagrario)" }, { display: "Carbo (Concepción)", value: "Carbo (Concepción)" }, { display: "Febres Cordero", value: "Febres Cordero" }, { display: "García Moreno", value: "García Moreno" }, { display: "Letamendi", value: "Letamendi" }, { display: "Nueve De Octubre", value: "Nueve De Octubre" }, { display: "Olmedo (San Alejo)", value: "Olmedo (San Alejo)" }, { display: "Roca", value: "Roca" }, { display: "Rocafuerte", value: "Rocafuerte" }, { display: "Sucre", value: "Sucre" }, { display: "Tarqui", value: "Tarqui" }, { display: "Urdaneta", value: "Urdaneta" }, { display: "Ximena", value: "Ximena" }, { display: "Pascuales", value: "Pascuales" }, { display: "Guayaquil", value: "Guayaquil" }, { display: "Chongón", value: "Chongón" }, { display: "Juan Gómez Rendón (Progreso)", value: "Juan Gómez Rendón (Progreso)" }, { display: "Morro", value: "Morro" }, { display: "Pascuales", value: "Pascuales" }, { display: "Playas (Gral. Villamil)", value: "Playas (Gral. Villamil)" }, { display: "Posorja", value: "Posorja" }, { display: "Puná", value: "Puná" }, { display: "Tenguel", value: "Tenguel" }, { display: "Alfredo Baquerizo Moreno (Juján)", value: "Alfredo Baquerizo Moreno (Juján)" },];
    var Huamboya = [{ display: "Chiguaza", value: "Chiguaza" }, { display: "Pablo Sexto", value: "Pablo Sexto" }, { display: "San Juan Bosco", value: "San Juan Bosco" }];
    var Huaquillas = [{ display: "El Paraíso", value: "El Paraíso" }, { display: "Hualtaco", value: "Hualtaco" }, { display: "Milton Reyes", value: "Milton Reyes" }, { display: "Unión Lojana", value: "Unión Lojana" }, { display: "Huaquillas", value: "Huaquillas" }, { display: "Marcabelí", value: "Marcabelí" }];
    var Ibarra = [{ display: "Guayaquil De Alpachaca", value: "Guayaquil De Alpachaca" }, { display: "Sagrario", value: "Sagrario" }, { display: "San Francisco", value: "San Francisco" }, { display: "La Dolorosa Del Priorato", value: "La Dolorosa Del Priorato" }, { display: "San Miguel De Ibarra", value: "San Miguel De Ibarra" }, { display: "Ambuquí", value: "Ambuquí" }, { display: "Angochagua", value: "Angochagua" }, { display: "Carolina", value: "Carolina" }, { display: "La Esperanza", value: "La Esperanza" }, { display: "Lita", value: "Lita" }, { display: "Salinas", value: "Salinas" }, { display: "San Antonio", value: "San Antonio" }, { display: "Andrade Marín (Lourdes)", value: "Andrade Marín (Lourdes)" },];
    var Isabela = [{ display: "Tomás De Berlanga (Santo Tomás)", value: "Tomás De Berlanga (Santo Tomás)" }, { display: "Puerto Ayora", value: "Puerto Ayora" }];
    var Isidro_Ayora = [{ display: "Isidro Ayora", value: "Isidro Ayora" }];
    var Jama = [{ display: "Jama", value: "Jama" }];
    var Jaramijó = [{ display: "Jaramijó", value: "Jaramijó" }];
    var Jipijapa = [{ display: "Dr. Miguel Morán Lucio", value: "Dr. Miguel Morán Lucio" }, { display: "Manuel Inocencio Parrales Y Guale", value: "Manuel Inocencio Parrales Y Guale" }, { display: "San Lorenzo De Jipijapa", value: "San Lorenzo De Jipijapa" }, { display: "Jipijapa", value: "Jipijapa" }, { display: "América", value: "América" }, { display: "El Anegado (Cab. En Eloy Alfaro)", value: "El Anegado (Cab. En Eloy Alfaro)" }, { display: "Julcuy", value: "Julcuy" }, { display: "La Unión", value: "La Unión" }, { display: "Machalilla", value: "Machalilla" }, { display: "Membrillal", value: "Membrillal" }, { display: "Pedro Pablo Gómez", value: "Pedro Pablo Gómez" }, { display: "Puerto De Cayo", value: "Puerto De Cayo" }, { display: "Puerto López", value: "Puerto López" }];
    var Junín = [{ display: "Junín", value: "Junín" }];
    var La_Concordia = [{ display: "La Concordia", value: "La Concordia" }, { display: "Monterrey", value: "Monterrey" }, { display: "La Villegas", value: "La Villegas" }, { display: "Plan Piloto", value: "Plan Piloto" }];
    var La_Joya_De_Los_Sachas = [{ display: "La Joya De Los Sachas", value: "La Joya De Los Sachas" }, { display: "Enokanqui", value: "Enokanqui" }, { display: "Pompeya", value: "Pompeya" }, { display: "San Carlos", value: "San Carlos" }, { display: "San Sebastián Del Coca", value: "San Sebastián Del Coca" }, { display: "Lago San Pedro", value: "Lago San Pedro" }, { display: "Rumipamba", value: "Rumipamba" }, { display: "Tres De Noviembre", value: "Tres De Noviembre" }, { display: "Unión Milagreña", value: "Unión Milagreña" }];
    var La_Libertad = [{ display: "La Libertad", value: "La Libertad" },];
    var La_Maná = [{ display: "El Carmen", value: "El Carmen" }, { display: "La Maná", value: "La Maná" }, { display: "El Triunfo", value: "El Triunfo" }, { display: "La Maná", value: "La Maná" }, { display: "Guasaganda (Cab.En Guasaganda", value: "Guasaganda (Cab.En Guasaganda" }, { display: "Pucayacu", value: "Pucayacu" }];
    var La_Troncal = [{ display: "La Troncal", value: "La Troncal" }, { display: "Manuel J. Calle", value: "Manuel J. Calle" }, { display: "Pancho Negro", value: "Pancho Negro" },];
    var Lago_Agrio = [{ display: "Nueva Loja", value: "Nueva Loja" }, { display: "Cuyabeno", value: "Cuyabeno" }, { display: "Dureno", value: "Dureno" }, { display: "General Farfán", value: "General Farfán" }, { display: "Tarapoa", value: "Tarapoa" }, { display: "El Eno", value: "El Eno" }, { display: "Pacayacu", value: "Pacayacu" }, { display: "Jambelí", value: "Jambelí" }, { display: "Santa Cecilia", value: "Santa Cecilia" }, { display: "Aguas Negras", value: "Aguas Negras" }];
    var Las_Golondrinas = [{ display: "Las Golondrinas", value: "Las Golondrinas" },];
    var Las_Lajas = [{ display: "La Victoria", value: "La Victoria" }, { display: "Platanillos", value: "Platanillos" }, { display: "Valle Hermoso", value: "Valle Hermoso" }, { display: "La Victoria", value: "La Victoria" }, { display: "La Libertad", value: "La Libertad" }, { display: "El Paraíso", value: "El Paraíso" }, { display: "San Isidro", value: "San Isidro" }];
    var Las_Naves = [{ display: "Las Mercedes", value: "Las Mercedes" }, { display: "Las Naves", value: "Las Naves" }, { display: "Las Naves", value: "Las Naves" }];
    var Latacunga = [{ display: "Eloy Alfaro (San Felipe)", value: "Eloy Alfaro (San Felipe)" }, { display: "Ignacio Flores (Parque Flores)", value: "Ignacio Flores (Parque Flores)" }, { display: "Juan Montalvo (San Sebastián)", value: "Juan Montalvo (San Sebastián)" }, { display: "La Matriz", value: "La Matriz" }, { display: "San Buenaventura", value: "San Buenaventura" }, { display: "Latacunga", value: "Latacunga" }, { display: "Alaques (Aláquez)", value: "Alaques (Aláquez)" }, { display: "Belisario Quevedo (Guanailín)", value: "Belisario Quevedo (Guanailín)" }, { display: "Guaitacama (Guaytacama)", value: "Guaitacama (Guaytacama)" }, { display: "Joseguango Bajo", value: "Joseguango Bajo" }, { display: "Las Pampas", value: "Las Pampas" }, { display: "Mulaló", value: "Mulaló" }, { display: "11 De Noviembre (Ilinchisi)", value: "11 De Noviembre (Ilinchisi)" }, { display: "Poaló", value: "Poaló" }, { display: "San Juan De Pastocalle", value: "San Juan De Pastocalle" }, { display: "Sigchos", value: "Sigchos" }, { display: "Tanicuchí", value: "Tanicuchí" }, { display: "Toacaso", value: "Toacaso" }, { display: "Palo Quemado", value: "Palo Quemado" }];
    var Limón_Indanza = [{ display: "General Leonidas Plaza Gutiérrez (Limón)", value: "General Leonidas Plaza Gutiérrez (Limón)" }, { display: "Indanza", value: "Indanza" }, { display: "Pan De Azúcar", value: "Pan De Azúcar" }, { display: "San Antonio (Cab. En San Antonio Centro", value: "San Antonio (Cab. En San Antonio Centro" }, { display: "San Carlos De Limón (San Carlos Del", value: "San Carlos De Limón (San Carlos Del" }, { display: "San Juan Bosco", value: "San Juan Bosco" }, { display: "San Miguel De Conchay", value: "San Miguel De Conchay" }, { display: "Santa Susana De Chiviaza (Cab. En Chiviaza)", value: "Santa Susana De Chiviaza (Cab. En Chiviaza)" }, { display: "Yunganza (Cab. En El Rosario)", value: "Yunganza (Cab. En El Rosario)" }];
    var Logroño = [{ display: "Logroño", value: "Logroño" }, { display: "Yaupi", value: "Yaupi" }, { display: "Shimpis", value: "Shimpis" }];
    var Loja = [{ display: "El Sagrario", value: "El Sagrario" }, { display: "San Sebastián", value: "San Sebastián" }, { display: "Sucre", value: "Sucre" }, { display: "Valle", value: "Valle" }, { display: "Loja", value: "Loja" }, { display: "Chantaco", value: "Chantaco" }, { display: "Chuquiribamba", value: "Chuquiribamba" }, { display: "El Cisne", value: "El Cisne" }, { display: "Gualel", value: "Gualel" }, { display: "Jimbilla", value: "Jimbilla" }, { display: "Malacatos (Valladolid)", value: "Malacatos (Valladolid)" }, { display: "San Lucas", value: "San Lucas" }, { display: "San Pedro De Vilcabamba", value: "San Pedro De Vilcabamba" }, { display: "Santiago", value: "Santiago" }, { display: "Taquil (Miguel Riofrío)", value: "Taquil (Miguel Riofrío)" }, { display: "Vilcabamba (Victoria)", value: "Vilcabamba (Victoria)" }, { display: "Yangana (Arsenio Castillo)", value: "Yangana (Arsenio Castillo)" }, { display: "Quinara", value: "Quinara" },];
    var Lomas_De_Sargentillo = [{ display: "Lomas De Sargentillo", value: "Lomas De Sargentillo" }, { display: "Isidro Ayora (Soledad)", value: "Isidro Ayora (Soledad)" },];
    var Loreto = [{ display: "Loreto", value: "Loreto" }, { display: "Avila (Cab. En Huiruno)", value: "Avila (Cab. En Huiruno)" }, { display: "Puerto Murialdo", value: "Puerto Murialdo" }, { display: "San José De Payamino", value: "San José De Payamino" }, { display: "San José De Dahuano", value: "San José De Dahuano" }, { display: "San Vicente De Huaticocha", value: "San Vicente De Huaticocha" },];
    var Macará = [{ display: "General Eloy Alfaro (San Sebastián)", value: "General Eloy Alfaro (San Sebastián)" }, { display: "Macará (Manuel Enrique Rengel Suquilanda)", value: "Macará (Manuel Enrique Rengel Suquilanda)" }, { display: "Macará", value: "Macará" }, { display: "Larama", value: "Larama" }, { display: "La Victoria", value: "La Victoria" }, { display: "Sabiango (La Capilla)", value: "Sabiango (La Capilla)" },];
    var Machala = [{ display: "La Providencia", value: "La Providencia" }, { display: "Machala", value: "Machala" }, { display: "Puerto Bolívar", value: "Puerto Bolívar" }, { display: "Nueve De Mayo", value: "Nueve De Mayo" }, { display: "El Cambio", value: "El Cambio" }, { display: "Machala", value: "Machala" }, { display: "El Cambio", value: "El Cambio" }, { display: "El Retiro", value: "El Retiro" },];
    var Manga_Del_Cura = [{ display: "Manga Del Cura", value: "Manga Del Cura" }];
    var Manta = [{ display: "Los Esteros", value: "Los Esteros" }, { display: "Manta", value: "Manta" }, { display: "San Mateo", value: "San Mateo" }, { display: "Tarqui", value: "Tarqui" }, { display: "Eloy Alfaro", value: "Eloy Alfaro" }, { display: "Manta", value: "Manta" }, { display: "San Lorenzo", value: "San Lorenzo" }, { display: "Santa Marianita (Boca De Pacoche)", value: "Santa Marianita (Boca De Pacoche)" },];
    var Mira = [{ display: "Mira (Chontahuasi)", value: "Mira (Chontahuasi)" }, { display: "Concepción", value: "Concepción" }, { display: "Jijón Y Caamaño (Cab. En Río Blanco)", value: "Jijón Y Caamaño (Cab. En Río Blanco)" }, { display: "Juan Montalvo (San Ignacio De Quil)", value: "Juan Montalvo (San Ignacio De Quil)" }];

    var Marcabelí = [{ display: "Marcabelí", value: "Marcabelí" }, { display: "El Ingenio", value: "El Ingenio" }];
    var Mejvar_Mejia=[{display:"Mera",value:"Mera"},{display:"Madre Tierra",value:"Madre Tierra"},{display:"Shell",value:"Shell"}];
    var Mejia = [{ display: "Mera", value: "Mera" }, { display: "Madre Tierra", value: "Madre Tierra" }, { display: "Shell", value: "Shell" }];
    var Milagro = [{ display: "Milagro", value: "Milagro" }, { display: "Chobo", value: "Chobo" }, { display: "General Elizalde (Bucay)", value: "General Elizalde (Bucay)" }, { display: "Mariscal Sucre (Huaques)", value: "Mariscal Sucre (Huaques)" }, { display: "Roberto Astudillo (Cab. En Cruce De Venecia)", value: "Roberto Astudillo (Cab. En Cruce De Venecia)" },];
    var Milagro = [{ display: "Mira (Chontahuasi)", value: "Mira (Chontahuasi)" }, { display: "Concepción", value: "Concepción" }, { display: "Jijón Y Caamaño (Cab. En Río Blanco)", value: "Jijón Y Caamaño (Cab. En Río Blanco)" }, { display: "Juan Montalvo (San Ignacio De Quil)", value: "Juan Montalvo (San Ignacio De Quil)" },];
    var Mocache = [{ display: "Mocache", value: "Mocache" }];
    var Mocha = [{ display: "Mocha", value: "Mocha" }, { display: "Pinguilí", value: "Pinguilí" }];
    var Montalvo = [{ display: "Montalvo", value: "Montalvo" }];
    var De_Mayo = [{ display: "Sucre", value: "Sucre" }, { display: "Bellavista", value: "Bellavista" }, { display: "Noboa", value: "Noboa" }, { display: "Arq. Sixto Durán Ballén", value: "Arq. Sixto Durán Ballén" },];
    var Montecristi = [{ display: "Anibal San Andrés", value: "Anibal San Andrés" }, { display: "Montecristi", value: "Montecristi" }, { display: "El Colorado", value: "El Colorado" }, { display: "General Eloy Alfaro", value: "General Eloy Alfaro" }, { display: "Leonidas Proaño", value: "Leonidas Proaño" }, { display: "Montecristi", value: "Montecristi" }, { display: "Jaramijó", value: "Jaramijó" }, { display: "La Pila", value: "La Pila" },];
    var Montúfar = [{ display: "González Suárez", value: "González Suárez" }, { display: "San José", value: "San José" }, { display: "San Gabriel", value: "San Gabriel" }, { display: "Cristóbal Colón", value: "Cristóbal Colón" }, { display: "Chitán De Navarrete", value: "Chitán De Navarrete" }, { display: "Fernández Salvador", value: "Fernández Salvador" }, { display: "La Paz", value: "La Paz" }, { display: "Piartal", value: "Piartal" }];
    var Morona = [{ display: "Macas", value: "Macas" }, { display: "Alshi (Cab. En 9 De Octubre)", value: "Alshi (Cab. En 9 De Octubre)" }, { display: "Chiguaza", value: "Chiguaza" }, { display: "General Proaño", value: "General Proaño" }, { display: "Huasaga (Cab.En Wampuik)", value: "Huasaga (Cab.En Wampuik)" }, { display: "Macuma", value: "Macuma" }, { display: "San Isidro", value: "San Isidro" }, { display: "Sevilla Don Bosco", value: "Sevilla Don Bosco" }, { display: "Sinaí", value: "Sinaí" }, { display: "Taisha", value: "Taisha" }, { display: "Zuña (Zúñac)", value: "Zuña (Zúñac)" }, { display: "Tuutinentza", value: "Tuutinentza" }, { display: "Cuchaentza", value: "Cuchaentza" }, { display: "San José De Morona", value: "San José De Morona" }, { display: "Río Blanco", value: "Río Blanco" }];
    var Muisne = [{ display: "Muisne", value: "Muisne" }, { display: "Bolívar", value: "Bolívar" }, { display: "Daule", value: "Daule" }, { display: "Galera", value: "Galera" }, { display: "Quingue (Olmedo Perdomo Franco)", value: "Quingue (Olmedo Perdomo Franco)" }, { display: "Salima", value: "Salima" }, { display: "San Francisco", value: "San Francisco" }, { display: "San Gregorio", value: "San Gregorio" }, { display: "San José De Chamanga (Cab.En Chamanga)", value: "San José De Chamanga (Cab.En Chamanga)" }];
    var Nabón = [{ display: "Nabón", value: "Nabón" }, { display: "Cochapata", value: "Cochapata" }, { display: "El Progreso (Cab.En Zhota)", value: "El Progreso (Cab.En Zhota)" }, { display: "Las Nieves (Chaya)", value: "Las Nieves (Chaya)" }, { display: "Oña", value: "Oña" },];
    var Nangaritza = [{ display: "Guayzimi", value: "Guayzimi" }, { display: "Zurmi", value: "Zurmi" }, { display: "Nuevo Paraíso", value: "Nuevo Paraíso" }];
    var Naranjal = [{ display: "Naranjal", value: "Naranjal" }, { display: "Jesús María", value: "Jesús María" }, { display: "San Carlos", value: "San Carlos" }, { display: "Santa Rosa De Flandes", value: "Santa Rosa De Flandes" }, { display: "Taura", value: "Taura" }];
    var Naranjito= [{ display: "Naranjito", value: "Naranjito" }];
    var Nobol = [{ display: "Narcisa De Jesús", value: "Narcisa De Jesús" }];
    var Olmedo = [{ display: "Olmedo", value: "Olmedo" }, { display: "La Tingue", value: "La Tingue" }];
    var Oña = [{ display: "San Felipe De Oña Cabecera Cantonal", value: "San Felipe De Oña Cabecera Cantonal" }, { display: "Susudel", value: "Susudel" },];
    var Orellana = [{ display: "Puerto Francisco De Orellana (El Coca)", value: "Puerto Francisco De Orellana (El Coca)" }, { display: "Dayuma", value: "Dayuma" }, { display: "Taracoa (Nueva Esperanza: Yuca)", value: "Taracoa (Nueva Esperanza: Yuca)" }, { display: "Alejandro Labaka", value: "Alejandro Labaka" }, { display: "El Dorado", value: "El Dorado" }, { display: "El Edén", value: "El Edén" }, { display: "García Moreno", value: "García Moreno" }, { display: "Inés Arango (Cab. En Western)", value: "Inés Arango (Cab. En Western)" }, { display: "La Belleza", value: "La Belleza" }, { display: "Nuevo Paraíso (Cab. En Unión", value: "Nuevo Paraíso (Cab. En Unión" }, { display: "San José De Guayusa", value: "San José De Guayusa" }, { display: "San Luis De Armenia", value: "San Luis De Armenia" }];
    var Oronel_Marcelino_Maridue = [{ display: "Coronel Marcelino Maridueña (San Carlos)", value: "Coronel Marcelino Maridueña (San Carlos)" },];
    var Otavalo = [{ display: "Jordán", value: "Jordán" }, { display: "San Luis", value: "San Luis" }, { display: "Otavalo", value: "Otavalo" }, { display: "Dr. Miguel Egas Cabezas (Peguche)", value: "Dr. Miguel Egas Cabezas (Peguche)" }, { display: "Eugenio Espejo (Calpaquí)", value: "Eugenio Espejo (Calpaquí)" }, { display: "González Suárez", value: "González Suárez" }, { display: "Pataquí", value: "Pataquí" }, { display: "San José De Quichinche", value: "San José De Quichinche" }, { display: "San Juan De Ilumán", value: "San Juan De Ilumán" }, { display: "San Pablo", value: "San Pablo" }, { display: "San Rafael", value: "San Rafael" }, { display: "Selva Alegre (Cab.En San Miguel De Pamplona)", value: "Selva Alegre (Cab.En San Miguel De Pamplona)" }];
    var Pablo_Sexto = [{ display: "Pablo Sexto", value: "Pablo Sexto" },];
    var Paján = [{ display: "Paján", value: "Paján" }, { display: "Campozano (La Palma De Paján)", value: "Campozano (La Palma De Paján)" }, { display: "Cascol", value: "Cascol" }, { display: "Guale", value: "Guale" }, { display: "Lascano", value: "Lascano" },];
    var Palanda = [{ display: "Palanda", value: "Palanda" }, { display: "El Porvenir Del Carmen", value: "El Porvenir Del Carmen" }, { display: "San Francisco Del Vergel", value: "San Francisco Del Vergel" }, { display: "Valladolid", value: "Valladolid" }, { display: "La Canela", value: "La Canela" },];
    var Palenque = [{ display: "Palenque", value: "Palenque" }];
    var Palestina = [{ display: "Palestina", value: "Palestina" }];
    var Pallatanga = [{ display: "Pallatanga", value: "Pallatanga" }];
    var Palora = [{ display: "Palora (Metzera)", value: "Palora (Metzera)" }, { display: "Arapicos", value: "Arapicos" }, { display: "Cumandá (Cab. En Colonia Agrícola Sevilla Del Oro)", value: "Cumandá (Cab. En Colonia Agrícola Sevilla Del Oro)" }, { display: "Huamboya", value: "Huamboya" }, { display: "Sangay (Cab. En Nayamanaca)", value: "Sangay (Cab. En Nayamanaca)" },];
    var Paltas = [{ display: "Catacocha", value: "Catacocha" }, { display: "Lourdes", value: "Lourdes" }, { display: "Catacocha", value: "Catacocha" }, { display: "Cangonamá", value: "Cangonamá" }, { display: "Guachanamá", value: "Guachanamá" }, { display: "La Tingue", value: "La Tingue" }, { display: "Lauro Guerrero", value: "Lauro Guerrero" }, { display: "Olmedo (Santa Bárbara)", value: "Olmedo (Santa Bárbara)" }, { display: "Orianga", value: "Orianga" }, { display: "San Antonio", value: "San Antonio" }, { display: "Casanga", value: "Casanga" }, { display: "Yamana", value: "Yamana" },];
    var Pangua = [{ display: "El Corazón", value: "El Corazón" }, { display: "Moraspungo", value: "Moraspungo" }, { display: "Pinllopata", value: "Pinllopata" }, { display: "Ramón Campaña", value: "Ramón Campaña" },];
    var Paquisha = [{ display: "Paquisha", value: "Paquisha" }, { display: "Bellavista", value: "Bellavista" }, { display: "Nuevo Quito", value: "Nuevo Quito" },];
    var Pasaje = [{ display: "Bolívar", value: "Bolívar" }, { display: "Loma De Franco", value: "Loma De Franco" }, { display: "Ochoa León (Matriz)", value: "Ochoa León (Matriz)" }, { display: "Tres Cerritos", value: "Tres Cerritos" }, { display: "Pasaje", value: "Pasaje" }, { display: "Buenavista", value: "Buenavista" }, { display: "Casacay", value: "Casacay" }, { display: "La Peaña", value: "La Peaña" }, { display: "Progreso", value: "Progreso" }, { display: "Uzhcurrumi", value: "Uzhcurrumi" }, { display: "Cañaquemada", value: "Cañaquemada" },];
    var Pastaza = [{ display: "Puyo", value: "Puyo" }, { display: "Arajuno", value: "Arajuno" }, { display: "Canelos", value: "Canelos" }, { display: "Curaray", value: "Curaray" }, { display: "Diez De Agosto", value: "Diez De Agosto" }, { display: "Fátima", value: "Fátima" }, { display: "Montalvo (Andoas)", value: "Montalvo (Andoas)" }, { display: "Pomona", value: "Pomona" }, { display: "Río Corrientes", value: "Río Corrientes" }, { display: "Río Tigre", value: "Río Tigre" }, { display: "Santa Clara", value: "Santa Clara" }, { display: "Sarayacu", value: "Sarayacu" }, { display: "Simón Bolívar (Cab. En Mushullacta)", value: "Simón Bolívar (Cab. En Mushullacta)" }, { display: "Tarqui", value: "Tarqui" }, { display: "Teniente Hugo Ortiz", value: "Teniente Hugo Ortiz" }, { display: "Veracruz (Indillama) (Cab. En Indillama)", value: "Veracruz (Indillama) (Cab. En Indillama)" }, { display: "El Triunfo", value: "El Triunfo" },];
    var Patate = [{ display: "Patate", value: "Patate" }, { display: "El Triunfo", value: "El Triunfo" }, { display: "Los Andes (Cab. En Poatug)", value: "Los Andes (Cab. En Poatug)" }, { display: "Sucre (Cab. En Sucre-Patate Urcu)", value: "Sucre (Cab. En Sucre-Patate Urcu)" },];
    var Paute = [{ display: "Paute", value: "Paute" }, { display: "Amaluza", value: "Amaluza" }, { display: "Bulán (José Víctor Izquierdo)", value: "Bulán (José Víctor Izquierdo)" }, { display: "Chicán (Guillermo Ortega)", value: "Chicán (Guillermo Ortega)" }, { display: "El Cabo", value: "El Cabo" }, { display: "Guachapala", value: "Guachapala" }, { display: "Guarainag", value: "Guarainag" }, { display: "Palmas", value: "Palmas" }, { display: "Pan", value: "Pan" }, { display: "San Cristóbal (Carlos Ordóñez Lazo)", value: "San Cristóbal (Carlos Ordóñez Lazo)" }, { display: "Sevilla De Oro", value: "Sevilla De Oro" }, { display: "Tomebamba", value: "Tomebamba" }, { display: "Dug Dug", value: "Dug Dug" },];
    var Pedernales = [{ display: "Pedernales", value: "Pedernales" }, { display: "Cojimíes", value: "Cojimíes" }, { display: "10 De Agosto", value: "10 De Agosto" }, { display: "Atahualpa", value: "Atahualpa" },];
    var Pedro_Carbo = [{ display: "Pedro Carbo", value: "Pedro Carbo" }, { display: "Valle De La Virgen", value: "Valle De La Virgen" }, { display: "Sabanilla", value: "Sabanilla" },];
    var Pedro_Moncayo = [{ display: "Tabacundo", value: "Tabacundo" }, { display: "La Esperanza", value: "La Esperanza" }, { display: "Malchinguí", value: "Malchinguí" }, { display: "Tocachi", value: "Tocachi" }, { display: "Tupigachi", value: "Tupigachi" },];
    var Pedro_Vicente_Maldonado = [{ display: "Pedro Vicente Maldonado", value: "Pedro Vicente Maldonado" }];
    var Penipe = [{ display: "Penipe", value: "Penipe" }, { display: "El Altar", value: "El Altar" }, { display: "Matus", value: "Matus" }, { display: "Puela", value: "Puela" }, { display: "San Antonio De Bayushig", value: "San Antonio De Bayushig" }, { display: "La Candelaria", value: "La Candelaria" }, { display: "Bilbao (Cab.En Quilluyacu)", value: "Bilbao (Cab.En Quilluyacu)" },];
    var Pichincha = [{ display: "Pichincha", value: "Pichincha" }, { display: "Barraganete", value: "Barraganete" }, { display: "San Sebastián", value: "San Sebastián" },];
    var Pimampiro = [{ display: "Pimampiro", value: "Pimampiro" }, { display: "Chugá", value: "Chugá" }, { display: "Mariano Acosta", value: "Mariano Acosta" }, { display: "San Francisco De Sigsipamba", value: "San Francisco De Sigsipamba" },];
    var Pindal = [{ display: "Pindal", value: "Pindal" }, { display: "Chaquinal", value: "Chaquinal" }, { display: "12 De Diciembre (Cab.En Achiotes)", value: "12 De Diciembre (Cab.En Achiotes)" }, { display: "Milagros", value: "Milagros" },];
    var Piñas = [{ display: "La Matriz", value: "La Matriz" }, { display: "La Susaya", value: "La Susaya" }, { display: "Piñas Grande", value: "Piñas Grande" }, { display: "Piñas", value: "Piñas" }, { display: "Capiro (Cab. En La Capilla De Capiro)", value: "Capiro (Cab. En La Capilla De Capiro)" }, { display: "La Bocana", value: "La Bocana" }, { display: "Moromoro (Cab. En El Vado)", value: "Moromoro (Cab. En El Vado)" }, { display: "Piedras", value: "Piedras" }, { display: "San Roque (Ambrosio Maldonado)", value: "San Roque (Ambrosio Maldonado)" }, { display: "Saracay", value: "Saracay" },];
    var Playas = [{ display: "General Villamil (Playas)", value: "General Villamil (Playas)" }];
    var Portovelo = [{ display: "Portovelo", value: "Portovelo" }, { display: "Curtincapa", value: "Curtincapa" }, { display: "Morales", value: "Morales" }, { display: "Salatí", value: "Salatí" },];
    var Portoviejo = [{ display: "Portoviejo", value: "Portoviejo" }, { display: "12 De Marzo", value: "12 De Marzo" }, { display: "Colón", value: "Colón" }, { display: "Picoazá", value: "Picoazá" }, { display: "San Pablo", value: "San Pablo" }, { display: "Andrés De Vera", value: "Andrés De Vera" }, { display: "Francisco Pacheco", value: "Francisco Pacheco" }, { display: "18 De Octubre", value: "18 De Octubre" }, { display: "Simón Bolívar", value: "Simón Bolívar" }, { display: "Portoviejo", value: "Portoviejo" }, { display: "Abdón Calderón (San Francisco)", value: "Abdón Calderón (San Francisco)" }, { display: "Alhajuela (Bajo Grande)", value: "Alhajuela (Bajo Grande)" }, { display: "Crucita", value: "Crucita" }, { display: "Pueblo Nuevo", value: "Pueblo Nuevo" }, { display: "Riochico (Río Chico)", value: "Riochico (Río Chico)" }, { display: "San Plácido", value: "San Plácido" }, { display: "Chirijos", value: "Chirijos" },];
    var Pucara = [{ display: "Pucará", value: "Pucará" }, { display: "Camilo Ponce Enríquez (Cab. En Río 7 De Mollepongo", value: "Camilo Ponce Enríquez (Cab. En Río 7 De Mollepongo" }, { display: "San Rafael De Sharug", value: "San Rafael De Sharug" },];
    var Puebloviejo = [{ display: "Puebloviejo", value: "Puebloviejo" }, { display: "Puerto Pechiche", value: "Puerto Pechiche" }, { display: "San Juan", value: "San Juan" },];
    var Puerto_López = [{ display: "Puerto López", value: "Puerto López" }, { display: "Machalilla", value: "Machalilla" }, { display: "Salango", value: "Salango" },];
    var Puerto_Quito = [{ display: "Puerto Quito", value: "Puerto Quito" }];
    var Pujili = [{ display: "Pujilí", value: "Pujilí" }, { display: "Angamarca", value: "Angamarca" }, { display: "Chucchilán (Chugchilán)", value: "Chucchilán (Chugchilán)" }, { display: "Guangaje", value: "Guangaje" }, { display: "Isinlibí (Isinliví)", value: "Isinlibí (Isinliví)" }, { display: "La Victoria", value: "La Victoria" }, { display: "Pilaló", value: "Pilaló" }, { display: "Tingo", value: "Tingo" }, { display: "Zumbahua", value: "Zumbahua" },];
    var Putumayo=[{display:"Puerto El Carmen Del Putumayo",value:"Puerto El Carmen Del Putumayo"},{display:"Palma Roja",value:"Palma Roja"},{display:"Puerto Bolívar (Puerto Montúfar)",value:"Puerto Bolívar (Puerto Montúfar)"},{display:"Puerto Rodríguez",value:"Puerto Rodríguez"},{display:"Santa Elena",value:"Santa Elena"},    ];
    var Puyango = [{ display: "Alamor", value: "Alamor" }, { display: "Ciano", value: "Ciano" }, { display: "El Arenal", value: "El Arenal" }, { display: "El Limo (Mariana De Jesús)", value: "El Limo (Mariana De Jesús)" }, { display: "Mercadillo", value: "Mercadillo" }, { display: "Vicentino", value: "Vicentino" },];
    var Quero = [{ display: "Quero", value: "Quero" }, { display: "Rumipamba", value: "Rumipamba" }, { display: "Yanayacu - Mochapata (Cab. En Yanayacu)", value: "Yanayacu - Mochapata (Cab. En Yanayacu)" },];
    var Quevedo = [{ display: "Quevedo", value: "Quevedo" }, { display: "San Camilo", value: "San Camilo" }, { display: "San José", value: "San José" }, { display: "Guayacán", value: "Guayacán" }, { display: "Nicolás Infante Díaz", value: "Nicolás Infante Díaz" }, { display: "San Cristóbal", value: "San Cristóbal" }, { display: "Siete De Octubre", value: "Siete De Octubre" }, { display: "24 De Mayo", value: "24 De Mayo" }, { display: "Venus Del Río Quevedo", value: "Venus Del Río Quevedo" }, { display: "Viva Alfaro", value: "Viva Alfaro" }, { display: "Quevedo", value: "Quevedo" }, { display: "Buena Fé", value: "Buena Fé" }, { display: "Mocache", value: "Mocache" }, { display: "San Carlos", value: "San Carlos" }, { display: "Valencia", value: "Valencia" }, { display: "La Esperanza", value: "La Esperanza" },];
    var Quijos = [{ display: "Baeza", value: "Baeza" }, { display: "Cosanga", value: "Cosanga" }, { display: "Cuyuja", value: "Cuyuja" }, { display: "Papallacta", value: "Papallacta" }, { display: "San Francisco De Borja (Virgilio Dávila)", value: "San Francisco De Borja (Virgilio Dávila)" }, { display: "San José Del Payamino", value: "San José Del Payamino" }, { display: "Sumaco", value: "Sumaco" },];
    var Quilanga = [{ display: "Quilanga", value: "Quilanga" }, { display: "Fundochamba", value: "Fundochamba" }, { display: "San Antonio De Las Aradas (Cab. En Las Aradas)", value: "San Antonio De Las Aradas (Cab. En Las Aradas)" },];
    var Quinindé = [{ display: "Rosa Zárate (Quinindé)", value: "Rosa Zárate (Quinindé)" }, { display: "Cube", value: "Cube" }, { display: "Chura (Chancama) (Cab. En El Yerbero)", value: "Chura (Chancama) (Cab. En El Yerbero)" }, { display: "Malimpia", value: "Malimpia" }, { display: "Viche", value: "Viche" }, { display: "La Unión", value: "La Unión" },];
    var Quinsaloma = [{ display: "Quinsaloma", value: "Quinsaloma" }];
    var Riobamba = [{ display: "Lizarzaburu", value: "Lizarzaburu" }, { display: "Maldonado", value: "Maldonado" }, { display: "Velasco", value: "Velasco" }, { display: "Veloz", value: "Veloz" }, { display: "Yaruquíes", value: "Yaruquíes" }, { display: "Riobamba", value: "Riobamba" }, { display: "Cacha (Cab. En Machángara)", value: "Cacha (Cab. En Machángara)" }, { display: "Calpi", value: "Calpi" }, { display: "Cubijíes", value: "Cubijíes" }, { display: "Flores", value: "Flores" }, { display: "Licán", value: "Licán" }, { display: "Licto", value: "Licto" }, { display: "Pungalá", value: "Pungalá" }, { display: "Punín", value: "Punín" }, { display: "Quimiag", value: "Quimiag" }, { display: "San Juan", value: "San Juan" }, { display: "San Luis", value: "San Luis" },];
    var Rioverde = [{ display: "Rioverde", value: "Rioverde" }, { display: "Chontaduro", value: "Chontaduro" }, { display: "Chumundé", value: "Chumundé" }, { display: "Lagarto", value: "Lagarto" }, { display: "Montalvo (Cab. En Horqueta)", value: "Montalvo (Cab. En Horqueta)" }, { display: "Rocafuerte", value: "Rocafuerte" },];
    var Rocafuerte = [{ display: "Rocafuerte", value: "Rocafuerte" }];
    var Rumiñahui = [{ display: "Sangolquí", value: "Sangolquí" }, { display: "San Pedro De Taboada", value: "San Pedro De Taboada" }, { display: "San Rafael", value: "San Rafael" }, { display: "Sangolqui", value: "Sangolqui" }, { display: "Cotogchoa", value: "Cotogchoa" }, { display: "Rumipamba", value: "Rumipamba" },];
    var Salcedo = [{ display: "San Miguel", value: "San Miguel" }, { display: "Antonio José Holguín (Santa Lucía)", value: "Antonio José Holguín (Santa Lucía)" }, { display: "Cusubamba", value: "Cusubamba" }, { display: "Mulalillo", value: "Mulalillo" }, { display: "Mulliquindil (Santa Ana)", value: "Mulliquindil (Santa Ana)" }, { display: "Pansaleo", value: "Pansaleo" },];
    var Salinas = [{ display: "Carlos Espinoza Larrea", value: "Carlos Espinoza Larrea" }, { display: "Gral. Alberto Enríquez Gallo", value: "Gral. Alberto Enríquez Gallo" }, { display: "Vicente Rocafuerte", value: "Vicente Rocafuerte" }, { display: "Santa Rosa", value: "Santa Rosa" }, { display: "Salinas", value: "Salinas" }, { display: "Anconcito", value: "Anconcito" }, { display: "José Luis Tamayo (Muey)", value: "José Luis Tamayo (Muey)" },];
    var Salitre = [{ display: "Bocana", value: "Bocana" }, { display: "Candilejos", value: "Candilejos" }, { display: "Central", value: "Central" }, { display: "Paraíso", value: "Paraíso" }, { display: "San Mateo", value: "San Mateo" }, { display: "El Salitre (Las Ramas)", value: "El Salitre (Las Ramas)" }, { display: "Gral. Vernaza (Dos Esteros)", value: "Gral. Vernaza (Dos Esteros)" }, { display: "La Victoria (Ñauza)", value: "La Victoria (Ñauza)" }, { display: "Junquillal", value: "Junquillal" },];
    var Samborondón = [{ display: "Samborondón", value: "Samborondón" }, { display: "La Puntilla (Satélite)", value: "La Puntilla (Satélite)" }, { display: "Samborondón", value: "Samborondón" }, { display: "Tarifa", value: "Tarifa" },];
    var San_Cristóbal = [{ display: "Puerto Baquerizo Moreno", value: "Puerto Baquerizo Moreno" }, { display: "El Progreso", value: "El Progreso" }, { display: "A Santa María (Floreana) (Cab. En Pto. Velasco Ibarr", value: "A Santa María (Floreana) (Cab. En Pto. Velasco Ibarr" },];
    var San_Fernando = [{ display: "San Fernando", value: "San Fernando" }, { display: "Chumblín", value: "Chumblín" },];
    var San_Jacinto_De_Yaguachi = [{ display: "San Jacinto De Yaguachi", value: "San Jacinto De Yaguachi" }, { display: "Crnel. Lorenzo De Garaicoa (Pedregal)", value: "Crnel. Lorenzo De Garaicoa (Pedregal)" }, { display: "Crnel. Marcelino Maridueña (San Carlos)", value: "Crnel. Marcelino Maridueña (San Carlos)" }, { display: "Gral. Pedro J. Montero (Boliche)", value: "Gral. Pedro J. Montero (Boliche)" }, { display: "Simón Bolívar", value: "Simón Bolívar" }, { display: "Yaguachi Viejo (Cone)", value: "Yaguachi Viejo (Cone)" }, { display: "Virgen De Fátima", value: "Virgen De Fátima" },];
    var San_Juan_Bosco = [{ display: "San Juan Bosco", value: "San Juan Bosco" }, { display: "Pan De Azúcar", value: "Pan De Azúcar" }, { display: "San Carlos De Limón", value: "San Carlos De Limón" }, { display: "San Jacinto De Wakambeis", value: "San Jacinto De Wakambeis" }, { display: "Santiago De Pananza", value: "Santiago De Pananza" },];
    var San_Lorenzo = [{ display: "San Lorenzo", value: "San Lorenzo" }, { display: "Alto Tambo (Cab. En Guadual)", value: "Alto Tambo (Cab. En Guadual)" }, { display: "Ancón (Pichangal) (Cab. En Palma Real)", value: "Ancón (Pichangal) (Cab. En Palma Real)" }, { display: "Calderón", value: "Calderón" }, { display: "Carondelet", value: "Carondelet" }, { display: "5 De Junio (Cab. En Uimbi)", value: "5 De Junio (Cab. En Uimbi)" }, { display: "Concepción", value: "Concepción" }, { display: "Mataje (Cab. En Santander)", value: "Mataje (Cab. En Santander)" }, { display: "San Javier De Cachaví (Cab. En San Javier)", value: "San Javier De Cachaví (Cab. En San Javier)" }, { display: "Santa Rita", value: "Santa Rita" }, { display: "Tambillo", value: "Tambillo" }, { display: "Tululbí (Cab. En Ricaurte)", value: "Tululbí (Cab. En Ricaurte)" }, { display: "Urbina", value: "Urbina" },];
    var San_Miguel = [{ display: "San Miguel", value: "San Miguel" }, { display: "Balsapamba", value: "Balsapamba" }, { display: "Bilován", value: "Bilován" }, { display: "Régulo De Mora", value: "Régulo De Mora" }, { display: "San Pablo (San Pablo De Atenas)", value: "San Pablo (San Pablo De Atenas)" }, { display: "Santiago", value: "Santiago" }, { display: "San Vicente", value: "San Vicente" },];
    var San_Miguel_De_Los_Bancos = [{ display: "San Miguel De Los Bancos", value: "San Miguel De Los Bancos" },{ display: "Mindo", value: "Mindo" }, { display: "Pedro Vicente Maldonado", value: "Pedro Vicente Maldonado" }, { display: "Puerto Quito", value: "Puerto Quito" },];
    var San_Miguel_De_Urcuquí = [{ display: "Urcuquí Cabecera Cantonal", value: "Urcuquí Cabecera Cantonal" }, { display: "Cahuasquí", value: "Cahuasquí" }, { display: "La Merced De Buenos Aires", value: "La Merced De Buenos Aires" }, { display: "Pablo Arenas", value: "Pablo Arenas" }, { display: "San Blas", value: "San Blas" }, { display: "Tumbabiro", value: "Tumbabiro" },];
    var San_Pedro_De_Huaca = [{ display: "Huaca", value: "Huaca" }, { display: "Mariscal Sucre", value: "Mariscal Sucre" },];
    var San_Pedro_De_Pelileo = [{ display: "Pelileo", value: "Pelileo" }, { display: "Pelileo Grande", value: "Pelileo Grande" }, { display: "Pelileo", value: "Pelileo" }, { display: "Benítez (Pachanlica)", value: "Benítez (Pachanlica)" }, { display: "Bolívar", value: "Bolívar" }, { display: "Cotaló", value: "Cotaló" }, { display: "Chiquicha (Cab. En Chiquicha Grande)", value: "Chiquicha (Cab. En Chiquicha Grande)" }, { display: "El Rosario (Rumichaca)", value: "El Rosario (Rumichaca)" }, { display: "García Moreno (Chumaqui)", value: "García Moreno (Chumaqui)" }, { display: "Guambaló (Huambaló)", value: "Guambaló (Huambaló)" }, { display: "Salasaca", value: "Salasaca" },];
    var San_Vicente = [{ display: "San Vicente", value: "San Vicente" }, { display: "Canoa", value: "Canoa" }];
    var Santa_Ana = [{ display: "Santa Ana", value: "Santa Ana" }, { display: "Lodana", value: "Lodana" }, { display: "Santa Ana De Vuelta Larga", value: "Santa Ana De Vuelta Larga" }, { display: "Ayacucho", value: "Ayacucho" }, { display: "Honorato Vásquez (Cab. En Vásquez)", value: "Honorato Vásquez (Cab. En Vásquez)" }, { display: "La Unión", value: "La Unión" }, { display: "Olmedo", value: "Olmedo" }, { display: "San Pablo (Cab. En Pueblo Nuevo)", value: "San Pablo (Cab. En Pueblo Nuevo)" },];
    var Santa_Clara = [{ display: "Santa Clara", value: "Santa Clara" }, { display: "San José", value: "San José" },];
    var Santa_Cruz = [{ display: "Puerto Ayora", value: "Puerto Ayora" }, { display: "Bellavista", value: "Bellavista" }, { display: "Santa Rosa (Incluye La Isla Baltra)", value: "Santa Rosa (Incluye La Isla Baltra)" },];
    var Santa_Elena = [{ display: "Ballenita", value: "Ballenita" }, { display: "Santa Elena", value: "Santa Elena" }, { display: "Santa Elena", value: "Santa Elena" }, { display: "Atahualpa", value: "Atahualpa" }, { display: "Colonche", value: "Colonche" }, { display: "Chanduy", value: "Chanduy" }, { display: "Manglaralto", value: "Manglaralto" }, { display: "Simón Bolívar (Julio Moreno)", value: "Simón Bolívar (Julio Moreno)" }, { display: "San José De Ancón", value: "San José De Ancón" },];
    var Santa_Isabel = [{ display: "Santa Isabel (Chaguarurco)", value: "Santa Isabel (Chaguarurco)" }, { display: "Abdón Calderón (La Unión)", value: "Abdón Calderón (La Unión)" }, { display: "El Carmen De Pijilí", value: "El Carmen De Pijilí" }, { display: "Zhaglli (Shaglli)", value: "Zhaglli (Shaglli)" }, { display: "San Salvador De Cañaribamba", value: "San Salvador De Cañaribamba" },];
    var Santa_Lucía = [{ display: "Santa Lucía", value: "Santa Lucía" }, ];
    var Santa_Rosa = [{ display: "Santa Rosa", value: "Santa Rosa" }, { display: "Puerto Jelí", value: "Puerto Jelí" }, { display: "Balneario Jambelí (Satélite)", value: "Balneario Jambelí (Satélite)" }, { display: "Jumón (Satélite)", value: "Jumón (Satélite)" }, { display: "Nuevo Santa Rosa", value: "Nuevo Santa Rosa" }, { display: "Santa Rosa", value: "Santa Rosa" }, { display: "Bellavista", value: "Bellavista" }, { display: "Jambelí", value: "Jambelí" }, { display: "La Avanzada", value: "La Avanzada" }, { display: "San Antonio", value: "San Antonio" }, { display: "Torata", value: "Torata" }, { display: "Victoria", value: "Victoria" }, { display: "Bellamaría", value: "Bellamaría" },];
    var Santiago = [{ display: "Santiago De Méndez", value: "Santiago De Méndez" }, { display: "Copal", value: "Copal" }, { display: "Chupianza", value: "Chupianza" }, { display: "Patuca", value: "Patuca" }, { display: "San Luis De El Acho (Cab. En El Acho)", value: "San Luis De El Acho (Cab. En El Acho)" }, { display: "Santiago", value: "Santiago" }, { display: "Tayuza", value: "Tayuza" }, { display: "San Francisco De Chinimbimi", value: "San Francisco De Chinimbimi" },];
    var Santiago_De_Píllaro = [{ display: "Ciudad Nueva", value: "Ciudad Nueva" }, { display: "Píllaro", value: "Píllaro" }, { display: "Píllaro", value: "Píllaro" }, { display: "Baquerizo Moreno", value: "Baquerizo Moreno" }, { display: "Emilio María Terán (Rumipamba)", value: "Emilio María Terán (Rumipamba)" }, { display: "Marcos Espinel (Chacata)", value: "Marcos Espinel (Chacata)" }, { display: "Presidente Urbina (Chagrapamba -Patzucul)", value: "Presidente Urbina (Chagrapamba -Patzucul)" }, { display: "San Andrés", value: "San Andrés" }, { display: "San José De Poaló", value: "San José De Poaló" }, { display: "San Miguelito", value: "San Miguelito" },];
    var Santo_Domingo = [{ display: "Abraham Calazacón", value: "Abraham Calazacón" }, { display: "Bombolí", value: "Bombolí" }, { display: "Chiguilpe", value: "Chiguilpe" }, { display: "Río Toachi", value: "Río Toachi" }, { display: "Río Verde", value: "Río Verde" }, { display: "Santo Domingo De Los Colorados", value: "Santo Domingo De Los Colorados" }, { display: "Zaracay", value: "Zaracay" }, { display: "Santo Domingo De Los Colorados", value: "Santo Domingo De Los Colorados" }, { display: "Alluriquín", value: "Alluriquín" }, { display: "Puerto Limón", value: "Puerto Limón" }, { display: "Luz De América", value: "Luz De América" }, { display: "San Jacinto Del Búa", value: "San Jacinto Del Búa" }, { display: "Valle Hermoso", value: "Valle Hermoso" }, { display: "El Esfuerzo", value: "El Esfuerzo" }, { display: "Santa María Del Toachi", value: "Santa María Del Toachi" },];
    var Saquisilí = [{ display: "Saquisilí", value: "Saquisilí" }, { display: "Canchagua", value: "Canchagua" }, { display: "Chantilín", value: "Chantilín" }, { display: "Cochapamba", value: "Cochapamba" },];
    var Saraguro = [{ display: "Saraguro", value: "Saraguro" }, { display: "El Paraíso De Celén", value: "El Paraíso De Celén" }, { display: "El Tablón", value: "El Tablón" }, { display: "Lluzhapa", value: "Lluzhapa" }, { display: "Manú", value: "Manú" }, { display: "San Antonio De Qumbe (Cumbe)", value: "San Antonio De Qumbe (Cumbe)" }, { display: "San Pablo De Tenta", value: "San Pablo De Tenta" }, { display: "San Sebastián De Yúluc", value: "San Sebastián De Yúluc" }, { display: "Selva Alegre", value: "Selva Alegre" }, { display: "Urdaneta (Paquishapa)", value: "Urdaneta (Paquishapa)" }, { display: "Sumaypamba", value: "Sumaypamba" },];
    var Sevilla_De_Oro = [{ display: "Sevilla De Oro", value: "Sevilla De Oro" }, { display: "Amaluza", value: "Amaluza" }, { display: "Palmas", value: "Palmas" },];
    var Shushufindi = [{ display: "Shushufindi", value: "Shushufindi" }, { display: "Limoncocha", value: "Limoncocha" }, { display: "Pañacocha", value: "Pañacocha" }, { display: "San Roque (Cab. En San Vicente)", value: "San Roque (Cab. En San Vicente)" }, { display: "San Pedro De Los Cofanes", value: "San Pedro De Los Cofanes" }, { display: "Siete De Julio", value: "Siete De Julio" },];
    var Sigchos = [{ display: "Sigchos", value: "Sigchos" }, { display: "Chugchillán", value: "Chugchillán" }, { display: "Isinliví", value: "Isinliví" }, { display: "Las Pampas", value: "Las Pampas" }, { display: "Palo Quemado", value: "Palo Quemado" },];
    var Sigsig = [{ display: "Sigsig", value: "Sigsig" }, { display: "Cuchil (Cutchil)", value: "Cuchil (Cutchil)" }, { display: "Gima", value: "Gima" }, { display: "Guel", value: "Guel" }, { display: "Ludo", value: "Ludo" }, { display: "San Bartolomé", value: "San Bartolomé" }, { display: "San José De Raranga", value: "San José De Raranga" },];
    var Simón_Bolívar = [{ display: "Simón Bolívar", value: "Simón Bolívar" }, { display: "Crnel.Lorenzo De Garaicoa (Pedregal)", value: "Crnel.Lorenzo De Garaicoa (Pedregal)" },];
    var Sozoranga = [{ display: "Sozoranga", value: "Sozoranga" }, { display: "Nueva Fátima", value: "Nueva Fátima" }, { display: "Tacamoros", value: "Tacamoros" },];
    var Sucre = [{ display: "Bahía De Caráquez", value: "Bahía De Caráquez" }, { display: "Leonidas Plaza Gutiérrez", value: "Leonidas Plaza Gutiérrez" }, { display: "Bahía De Caráquez", value: "Bahía De Caráquez" }, { display: "Canoa", value: "Canoa" }, { display: "Cojimíes", value: "Cojimíes" }, { display: "Charapotó", value: "Charapotó" }, { display: "10 De Agosto", value: "10 De Agosto" }, { display: "Jama", value: "Jama" }, { display: "Pedernales", value: "Pedernales" }, { display: "San Isidro", value: "San Isidro" }, { display: "San Vicente", value: "San Vicente" },];
    var Sucúa = [{ display: "Sucúa", value: "Sucúa" }, { display: "Asunción", value: "Asunción" }, { display: "Huambi", value: "Huambi" }, { display: "Logroño", value: "Logroño" }, { display: "Yaupi", value: "Yaupi" }, { display: "Santa Marianita De Jesús", value: "Santa Marianita De Jesús" },];
    var Sucumbíos = [{ display: "La Bonita", value: "La Bonita" }, { display: "El Playón De San Francisco", value: "El Playón De San Francisco" }, { display: "La Sofía", value: "La Sofía" }, { display: "Rosa Florida", value: "Rosa Florida" }, { display: "Santa Bárbara", value: "Santa Bárbara" },];
    var Suscal = [{ display: "Suscal", value: "Suscal" } ];
    var Taisha = [{ display: "Taisha", value: "Taisha" }, { display: "Huasaga (Cab. En Wampuik)", value: "Huasaga (Cab. En Wampuik)" }, { display: "Macuma", value: "Macuma" }, { display: "Tuutinentza", value: "Tuutinentza" }, { display: "Pumpuentsa", value: "Pumpuentsa" },];
    var Tena = [{ display: "Tena", value: "Tena" }, { display: "Ahuano", value: "Ahuano" }, { display: "Carlos Julio Arosemena Tola (Zatza-Yacu)", value: "Carlos Julio Arosemena Tola (Zatza-Yacu)" }, { display: "Chontapunta", value: "Chontapunta" }, { display: "Pano", value: "Pano" }, { display: "Puerto Misahualli", value: "Puerto Misahualli" }, { display: "Puerto Napo", value: "Puerto Napo" }, { display: "Tálag", value: "Tálag" }, { display: "San Juan De Muyuna", value: "San Juan De Muyuna" },];
    var Tisaleo = [{ display: "Tisaleo", value: "Tisaleo" }, { display: "Quinchicoto", value: "Quinchicoto" },];
    var Tiwintza = [{ display: "Santiago", value: "Santiago" }, { display: "San José De Morona", value: "San José De Morona" },];
    var Tosagua = [{ display: "Tosagua", value: "Tosagua" }, { display: "Bachillero", value: "Bachillero" }, { display: "Angel Pedro Giler (La Estancilla)", value: "Angel Pedro Giler (La Estancilla)" },];
    var Tulcán = [{ display: "González Suárez", value: "González Suárez" }, { display: "Tulcán", value: "Tulcán" }, { display: "Tulcán", value: "Tulcán" }, { display: "El Carmelo (El Pun)", value: "El Carmelo (El Pun)" }, { display: "Huaca", value: "Huaca" }, { display: "Julio Andrade (Orejuela)", value: "Julio Andrade (Orejuela)" }, { display: "Maldonado", value: "Maldonado" }, { display: "Pioter", value: "Pioter" }, { display: "Tobar Donoso (La Bocana De Camumbí)", value: "Tobar Donoso (La Bocana De Camumbí)" }, { display: "Tufiño", value: "Tufiño" }, { display: "Urbina (Taya)", value: "Urbina (Taya)" }, { display: "El Chical", value: "El Chical" }, { display: "Mariscal Sucre", value: "Mariscal Sucre" }, { display: "Santa Martha De Cuba", value: "Santa Martha De Cuba" },];
    var Urdaneta = [{ display: "Catarama", value: "Catarama" }, { display: "Ricaurte", value: "Ricaurte" }];
    var Valencia = [{ display: "Valencia", value: "Valencia" }];
    var Ventanas = [{ display: "10 De Noviembre", value: "10 De Noviembre" }, { display: "Ventanas", value: "Ventanas" }, { display: "Quinsaloma", value: "Quinsaloma" }, { display: "Zapotal", value: "Zapotal" }, { display: "Chacarita", value: "Chacarita" }, { display: "Los Ángeles", value: "Los Ángeles" },];
    var Vínces = [{ display: "Vinces", value: "Vinces" }, { display: "Antonio Sotomayor (Cab. En Playas De Vinces)", value: "Antonio Sotomayor (Cab. En Playas De Vinces)" }, { display: "Palenque", value: "Palenque" },];
    var Yacuambi = [{ display: "28 De Mayo (San José De Yacuambi)", value: "28 De Mayo (San José De Yacuambi)" }, { display: "La Paz", value: "La Paz" }, { display: "Tutupali", value: "Tutupali" },];
    var Yantzaza = [{ display: "Yantzaza (Yanzatza)", value: "Yantzaza (Yanzatza)" }, { display: "Chicaña", value: "Chicaña" }, { display: "El Pangui", value: "El Pangui" }, { display: "Los Encuentros", value: "Los Encuentros" },];
    var Zamora = [{ display: "El Limón", value: "El Limón" }, { display: "Zamora", value: "Zamora" }, { display: "Zamora", value: "Zamora" }, { display: "Cumbaratza", value: "Cumbaratza" }, { display: "Guadalupe", value: "Guadalupe" }, { display: "Imbana (La Victoria De Imbana)", value: "Imbana (La Victoria De Imbana)" }, { display: "Paquisha", value: "Paquisha" }, { display: "Sabanilla", value: "Sabanilla" }, { display: "Timbara", value: "Timbara" }, { display: "Zumbi", value: "Zumbi" }, { display: "San Carlos De Las Minas", value: "San Carlos De Las Minas" },]; var Zapotillo = [{ display: "Zapotillo", value: "Zapotillo" }, { display: "Mangahurco (Cazaderos)", value: "Mangahurco (Cazaderos)" }, { display: "Garzareal", value: "Garzareal" }, { display: "Limones", value: "Limones" }, { display: "Paletillas", value: "Paletillas" }, { display: "Bolaspamba", value: "Bolaspamba" },];
    var Zapotillo = [{ display: "Zapotillo", value: "Zapotillo" }, { display: "Mangahurco (Cazaderos)", value: "Mangahurco (Cazaderos)" }, { display: "Garzareal", value: "Garzareal" }, { display: "Limones", value: "Limones" }, { display: "Paletillas", value: "Paletillas" }, { display: "Bolaspamba", value: "Bolaspamba" },];
    var Zaruma = [{ display: "Zaruma", value: "Zaruma" }, { display: "Abañín", value: "Abañín" }, { display: "Arcapamba", value: "Arcapamba" }, { display: "Guanazán", value: "Guanazán" }, { display: "Guizhaguiña", value: "Guizhaguiña" }, { display: "Huertas", value: "Huertas" }, { display: "Malvas", value: "Malvas" }, { display: "Muluncay Grande", value: "Muluncay Grande" }, { display: "Sinsao", value: "Sinsao" }, { display: "Salvias", value: "Salvias" },];
    var Azogues = [{ display: "Aurelio Bayas Martínez", value: "Aurelio Bayas Martínez" }, { display: "Azogues", value: "Azogues" }, { display: "Borrero", value: "Borrero" }, { display: "San Francisco", value: "San Francisco" }, { display: "Azogues", value: "Azogues" }, { display: "Cojitambo", value: "Cojitambo" }, { display: "Déleg", value: "Déleg" }, { display: "Guapán", value: "Guapán" }, { display: "Javier Loyola (Chuquipata)", value: "Javier Loyola (Chuquipata)" }, { display: "Luis Cordero", value: "Luis Cordero" }, { display: "Pindilig", value: "Pindilig" }, { display: "Rivera", value: "Rivera" }, { display: "San Miguel", value: "San Miguel" }, { display: "Solano", value: "Solano" }, { display: "Taday", value: "Taday" },];



     









    var servicio_default = [
        { display: "Seleccione una...", value: "Seleccione una..." }];


    // Aqui creamos verificamos cual opciones apareceran dependiendo de la seleccion@superservicios

    $("#canton").change(function () {
        var parent = $(this).val();
        switch (parent) {
            case 'Quito':
                list(Quito);
            case 'Antonio_Ante': list(Antonio_Ante); break;
            case 'Arajuno': list(Arajuno); break;
            case 'Archidona': list(Archidona); break;
            case 'Ambato': list(Ambato); break;
            case 'Aguarico': list(Aguarico); break;
            case 'Arlos_Julio_Arosemena_Tol': list(Arlos_Julio_Arosemena_Tol); break;
            case 'Cuenca': list(Cuenca); break;
            case 'Girón': list(Girón); break;
            case 'Gualaceo': list(Gualaceo); break;
            case 'Nabón': list(Nabón); break;
            case 'Paute': list(Paute); break;
            case 'Pucara': list(Pucara); break;
            case 'San_Fernando': list(San_Fernando); break;
            case 'Santa_Isabel': list(Santa_Isabel); break;
            case 'Sigsig': list(Sigsig); break;
            case 'Oña': list(Oña); break;
            case 'Chordeleg': list(Chordeleg); break;
            case 'El_Pan': list(El_Pan); break;
            case 'Sevilla_De_Oro': list(Sevilla_De_Oro); break;
            case 'Guachapala': list(Guachapala); break;
            case 'Camilo Ponce Enríquez': list(Camilo_Ponce_Enriquez); break;
            case 'Guaranda': list(Guaranda); break;
            case 'Chillanes': list(Chillanes); break;
            case 'Chimbo': list(Chimbo); break;
            case 'Echeandía': list(Echeandía); break;
            case 'San_Miguel': list(San_Miguel); break;
            case 'Caluma': list(Caluma); break;
            case 'Las_Naves': list(Las_Naves); break;
            case 'Azogues': list(Azogues); break;
            case 'Biblián': list(Biblián); break;
            case 'Cañar': list(Cañar); break;
            case 'La_Troncal': list(La_Troncal); break;
            case 'El_Tambo': list(El_Tambo); break;
            case 'Déleg': list(Déleg); break;
            case 'Suscal': list(Suscal); break;
            case 'Tulcán': list(Tulcán); break;
            case 'Bolívar': list(Bolívar); break;
            case 'Espejo': list(Espejo); break;
            case 'Mira': list(Mira); break;
            case 'Montúfar': list(Montúfar); break;
            case 'San_Pedro_De_Huaca': list(San_Pedro_De_Huaca); break;
            case 'Latacunga': list(Latacunga); break;
            case 'La_Maná': list(La_Maná); break;
            case 'Pangua': list(Pangua); break;
            case 'Pujili': list(Pujili); break;
            case 'Salcedo': list(Salcedo); break;
            case 'Saquisilí': list(Saquisilí); break;
            case 'Sigchos': list(Sigchos); break;
            case 'Riobamba': list(Riobamba); break;
            case 'Alausi': list(Alausi); break;
            case 'Colta': list(Colta); break;
            case 'Chambo': list(Chambo); break;
            case 'Chunchi': list(Chunchi); break;
            case 'Guamote': list(Guamote); break;
            case 'Guano': list(Guano); break;
            case 'Pallatanga': list(Pallatanga); break;
            case 'Penipe': list(Penipe); break;
            case 'Cumandá': list(Cumandá); break;
            case 'Machala': list(Machala); break;
            case 'Arenillas': list(Arenillas); break;
            case 'Atahualpa': list(Atahualpa); break;
            case 'Balsas': list(Balsas); break;
            case 'Chilla': list(Chilla); break;
            case 'El_Guabo': list(El_Guabo); break;
            case 'Huaquillas': list(Huaquillas); break;
            case 'Marcabelí': list(Marcabelí); break;
            case 'Pasaje': list(Pasaje); break;
            case 'Piñas': list(Piñas); break;
            case 'Portovelo': list(Portovelo); break;
            case 'Santa_Rosa': list(Santa_Rosa); break;
            case 'Zaruma': list(Zaruma); break;
            case 'Las_Lajas': list(Las_Lajas); break;
            case 'Esmeraldas': list(Esmeraldas); break;
            case 'Eloy_Alfaro': list(Eloy_Alfaro); break;
            case 'Muisne': list(Muisne); break;
            case 'Quinindé': list(Quinindé); break;
            case 'San_Lorenzo': list(San_Lorenzo); break;
            case 'Atacames': list(Atacames); break;
            case 'Rioverde': list(Rioverde); break;
            case 'La_Concordia': list(La_Concordia); break;
            case 'Guayaquil': list(Guayaquil); break;
            case 'Do_Baquerizo_Moreno': list(Do_Baquerizo_Moreno); break;
            case 'Balao': list(Balao); break;
            case 'Balzar': list(Balzar); break;
            case 'Colimes': list(Colimes); break;
            case 'Daule': list(Daule); break;
            case 'Duran': list(Duran); break;
            case 'El_Empalme': list(El_Empalme); break;
            case 'El_Triunfo': list(El_Triunfo); break;
            case 'Milagro': list(Milagro); break;
            case 'Naranjal': list(Naranjal); break;
            case 'Naranjito': list(Naranjito); break;
            case 'Palestina': list(Palestina); break;
            case 'Baba': list(Baba); break;
            case 'De_Mayo': list(De_Mayo); break;
            case 'Babahoyo': list(Babahoyo); break;
            case 'Baños_De_Agua_Santa': list(Baños_De_Agua_Santa); break;
            case 'Biblian': list(Biblian); break;
            case 'Bolivar': list(Bolivar); break;
            case 'Buena_Fe': list(Buena_Fe); break;
            case 'Calvas': list(Calvas); break;
            case 'Cascales': list(Cascales); break;
            case 'Catamayo': list(Catamayo); break;
            case 'Cayambe': list(Cayambe); break;
            case 'Celica': list(Celica); break;
            case 'Centinela_Del_Cóndor': list(Centinela_Del_Cóndor); break;
            case 'Cevallos': list(Cevallos); break;
            case 'Chaguarpamba': list(Chaguarpamba); break;
            case 'Chinchipe': list(Chinchipe); break;
            case 'Chone': list(Chone); break;
            case 'Cotacachi': list(Cotacachi); break;
            case 'Cumanda': list(Cumanda); break;
            case 'Cuyabeno': list(Cuyabeno); break;
            case 'Delég': list(Deleg); break;
            case 'El_Carmen': list(El_Carmen); break;
            case 'El_Chaco': list(El_Chaco); break;
            case 'El_Pangui': list(El_Pangui); break;
            case 'El_Piedrero': list(El_Piedrero); break;
            case 'Espíndola': list(Espíndola); break;
            case 'Flavio_Alfaro': list(Flavio_Alfaro); break;
            case 'General_Antonio_Elizalde': list(General_Antonio_Elizalde); break;
            case 'Gonzalo_Pizarro': list(Gonzalo_Pizarro); break;
            case 'Gonzanamá': list(Gonzanamá); break;
            case 'Gualaquiza': list(Gualaquiza); break;
            case 'Huamboya': list(Huamboya); break;
            case 'Ibarra': list(Ibarra); break;
            case 'Isabela': list(Isabela); break;
            case 'Isidro_Ayora': list(Isidro_Ayora); break;
            case 'Jama': list(Jama); break;
            case 'Jaramijó': list(Jaramijó); break;
            case 'Jipijapa': list(Jipijapa); break;
            case 'Junín': list(Junín); break;
            case 'La_Joya_De_Los_Sachas': list(La_Joya_De_Los_Sachas); break;
            case 'La_Libertad': list(La_Libertad); break;
            case 'La_Maná': list(La_Maná); break;
            case 'Lago_Agrio': list(Lago_Agrio); break;
            case 'Las_Golondrinas': list(Las_Golondrinas); break;
            case 'Limón_Indanza': list(Limón_Indanza); break;
            case 'Logroño': list(Logroño); break;
            case 'Loja': list(Loja); break;
            case 'Lomas_De_Sargentillo': list(Lomas_De_Sargentillo); break;
            case 'Loreto': list(Loreto); break;
            case 'Macará': list(Macará); break;
            case 'Manga_Del_Cura': list(Manga_Del_Cura); break;
            case 'Manta': list(Manta); break;
            case 'Mejvar_Mejia': list(Mejvar_Mejia); break;
            case 'Mejia': list(Mejia); break;
            case 'Mocache': list(Mocache); break;
            case 'Mocha': list(Mocha); break;
            case 'Montalvo': list(Montalvo); break;
            case 'Montecristi': list(Montecristi); break;
            case 'Morona': list(Morona); break;
            case 'Nangaritza': list(Nangaritza); break;
            case 'Nobol': list(Nobol); break;
            case 'Olmedo': list(Olmedo); break;
            case 'Orellana': list(Orellana); break;
            case 'Oronel_Marcelino_Maridue': list(Oronel_Marcelino_Maridue); break;
            case 'Otavalo': list(Otavalo); break;
            case 'Pablo_Sexto': list(Pablo_Sexto); break;
            case 'Paján': list(Paján); break;
            case 'Palanda': list(Palanda); break;
            case 'Palenque': list(Palenque); break;
            case 'Palora': list(Palora); break;
            case 'Paltas': list(Paltas); break;
            case 'Paquisha': list(Paquisha); break;
            case 'Pastaza': list(Pastaza); break;
            case 'Patate': list(Patate); break;
            case 'Pedernales': list(Pedernales); break;
            case 'Pedro_Carbo': list(Pedro_Carbo); break;
            case 'Pedro_Moncayo': list(Pedro_Moncayo); break;
            case 'Pedro_Vicente_Maldonado': list(Pedro_Vicente_Maldonado); break;
            case 'Pichincha': list(Pichincha); break;
            case 'Pimampiro': list(Pimampiro); break;
            case 'Pindal': list(Pindal); break;
            case 'Playas': list(Playas); break;
            case 'Portoviejo': list(Portoviejo); break;
            case 'Puebloviejo': list(Puebloviejo); break;
            case 'Puerto_López': list(Puerto_López); break;
            case 'Puerto_Quito': list(Puerto_Quito); break;
            case 'Putumayo': list(Putumayo); break;
            case 'Puyango': list(Puyango); break;
            case 'Quero': list(Quero); break;
            case 'Quevedo': list(Quevedo); break;
            case 'Quijos': list(Quijos); break;
            case 'Quilanga': list(Quilanga); break;
            case 'Quinsaloma': list(Quinsaloma); break;
            case 'Rocafuerte': list(Rocafuerte); break;
            case 'Rumiñahui': list(Rumiñahui); break;
            case 'Salinas': list(Salinas); break;
            case 'Salitre': list(Salitre); break;
            case 'Samborondón': list(Samborondón); break;
            case 'San_Cristóbal': list(San_Cristóbal); break;
            case 'San_Jacinto_De_Yaguachi': list(San_Jacinto_De_Yaguachi); break;
            case 'San_Juan_Bosco': list(San_Juan_Bosco); break;
            case 'San_Miguel_De_Los_Bancos': list(San_Miguel_De_Los_Bancos); break;
            case 'San_Miguel_De_Urcuquí': list(San_Miguel_De_Urcuquí); break;
            case 'San_Pedro_De_Pelileo': list(San_Pedro_De_Pelileo); break;
            case 'San_Vicente': list(San_Vicente); break;
            case 'Santa_Ana': list(Santa_Ana); break;
            case 'Santa_Clara': list(Santa_Clara); break;
            case 'Santa_Cruz': list(Santa_Cruz); break;
            case 'Santa_Elena': list(Santa_Elena); break;
            case 'Santa_Lucía': list(Santa_Lucía); break;
            case 'Santiago': list(Santiago); break;
            case 'Santiago_De_Píllaro': list(Santiago_De_Píllaro); break;
            case 'Santo_Domingo': list(Santo_Domingo); break;
            case 'Saraguro': list(Saraguro); break;
            case 'Shushufindi': list(Shushufindi); break;
            case 'Simón_Bolívar': list(Simón_Bolívar); break;
            case 'Sozoranga': list(Sozoranga); break;
            case 'Sucre': list(Sucre); break;
            case 'Sucúa': list(Sucúa); break;
            case 'Sucumbíos': list(Sucumbíos); break;
            case 'Taisha': list(Taisha); break;
            case 'Tena': list(Tena); break;
            case 'Tisaleo': list(Tisaleo); break;
            case 'Tiwintza': list(Tiwintza); break;
            case 'Tosagua': list(Tosagua); break;
            case 'Urdaneta': list(Urdaneta); break;
            case 'Valencia': list(Valencia); break;
            case 'Ventanas': list(Ventanas); break;
            case 'Vínces': list(Vínces); break;
            case 'Yacuambi': list(Yacuambi); break;
            case 'Yantzaza': list(Yantzaza); break;
            case 'Zamora': list(Zamora); break;
            case 'Zapotillo': list(Zapotillo); break;

            case '0':
                list(servicio_default);
                break;
            default: //default child option is blank
                $("#inlineFormSelectPref").html('');
                break;
        }
    });
    //function to populate child select box
    function list(array_list) {
        $("#inlineFormSelectPref").html(""); //reset child options
        $(array_list).each(function (i) { //populate child options
            $("#inlineFormSelectPref").append("<option value=\"" + array_list[i].value + "\">" + array_list[i].display + "</option>");
        });

    }




});
