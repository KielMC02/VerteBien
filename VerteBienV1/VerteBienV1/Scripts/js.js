
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

                //   alert(valor);
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
jQuery('input[type=file]').change(function () {
    var filename = jQuery(this).val().split('\\').pop();
    var idname = jQuery(this).attr('id');
    jQuery('span.' + idname).next().find('span').html(filename);
});

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
/* fin query para poner el nombre del archivo que se sube*/


/*Inicio de validacion */
const formulario = document.getElementById('formulario');

function countChars(obj) {
    document.getElementById("charNum").innerHTML = obj.value.length + ' characters';
}

const inputs = document.querySelectorAll('#formulario input');

const expresiones = {
    usuario: /^[a-zA-Z0-9\_\-\@]{4,16}$/, // Letras, numeros, guion y guion_bajo
    nombre: /^[a-zA-ZÀ-ÿ\s]{4,25}$/, // Letras y espacios, pueden llevar acentos.
    password: /^.{4,12}$/, // 4 a 12 digitos.
    correo: /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i,
    telefono: /^\d{9,10}$/, // 7 a 14 numeros.
    fecha: /^(?:0?[1-9]|1[1-2])([\-/.])(3[01]|[12][0-9]|0?[1-9])\1\d{4}$/, //pra fechas mes/dia/año
    numero: /^[0-9]{1,4}$/, //numeros de 1 a 4 digios
    alphanumerico: /[A-Z? a-z 0-9?]{10,150}$/,//Alfanumerico
    precio: /^\d{1,4}(\.\d{1,2})?$/, //número decimal o flotante
    fbuser: /(?: (?: http | https): \/\/)?(?:www.)?facebook.com\/(?:(?:\w)*#!\/)?(?:pages\/)?(?:[?\w\-]*\/)?(?:profile.php\?id=(?=\d.*))?([\w\-]*)?/, //Fb user 
    iguser: /(?:www\.)?(?:instagram\.com|instagr\.am)/,// Instgram User. 
    web: /(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})/,//PAra paginas web


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
            validarCampo(expresiones.nombre, e.target, 'ciudad');
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
            validarCampo(expresiones.numero, e.target, 'calle');
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




$('input[type="file"]').change(function () {
    $("button").prop("disabled", this.files.length == 0);
    document.getElementById('obj1').style.display = 'none';
});



//var pathname = window.location.pathname;
////if (pathname == '/Account/Register') {
////     else {Account/RegisterUser
////   } }
//const imgiput = document.getElementById('file1');
//if (imgiput.value == "") {
//    alert("Imagen vacia");
//} else { }

// Busca la pagina y el directorio exacto. 
var loc = window.location;
var pathNa = loc.pathname.substring(0, loc.pathname.lastIndexOf('/') + 1);



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

                if (campos.Email && campos.nombre && campos.apellido && campos.nombre_peluqueria && campos.ciudad && campos.Password && campos.ConfirmPassword && campos.calle && campos.ciudad) {


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
                if (campos.nombre && campos.apellido && campos.telefono && campos.calle && campos.ciudad && campos.nombre_peluqueria) {


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
                console.log("Show la cosa ");

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
    });
}