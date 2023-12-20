
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
    let puntuacionBuscar = document.getElementById("puntuacionBuscar");

    for (var a = 0; a < rating.length; a++) {

        $(rating[a]).starrr({


            rating: rating[a].getAttribute("data-rating"),

            change: function (e, valor) {
                reltado.value = valor;
                if (valor !=undefined) { 
                    reltado.innerHTML = '(' + valor + ')';
                    puntuacionBuscar.innerHTML = '(' + valor + ')';

                }

            }
        })
    }
    /* Cambiar numeros flotantes.*/
    function financial(x) {
        return Number.parseFloat(x).toFixed(1);
    }
    comentarios = document.querySelectorAll('#comentarios-conteo');

    var rar = $('#miDiv').data('promedio');
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
        decimal = decimal.replace(/,/, '.');
        let horas = Math.floor(decimal), // Obtenemos la parte entera
            restoHoras = Math.floor(decimal % 1 * 100), // Obtenemos la parde decimal
            decimalMinutos = restoHoras * 60 / 100, // Obtenemos los minutos expresado en decimal

            minutos = Math.floor(decimalMinutos), // Obtenemos la parte entera
            restoMins = Math.floor(decimalMinutos % 1 * 100), // Obtenemos la parde decimal
            segundos = Math.floor(restoMins * 60 / 100); // Obtenemos los segundos expresado en entero

        return `${('00' + horas).slice(-2)}:${('00' + minutos).slice(-2)}:${('00' + segundos).slice(-2)}`;
    }
    /* FIN Cambiar numeros decimal a horas.*/



    // tiempo mis siervicios

    let servicio = document.querySelectorAll('#tiempo-servicio');
    for (let i = 0; i < servicio.length; i++) {
        let servicioTiempo = servicio[i].getAttribute('data-tiempo').toString();
        servicioTiempo = servicioTiempo.replace(/,/, '.');
        servicioTiempo = servicioTiempo == 0.50 ? '30 minutos' : servicioTiempo;
        servicioTiempo = servicioTiempo == 1.50 ? '1 hora y 30 minutos' : servicioTiempo;
        servicioTiempo = servicioTiempo == 2.00 ? '2 horas' : servicioTiempo;
        servicioTiempo = servicioTiempo == 1.00 ? '1 hora' : servicioTiempo;

        servicio[i].innerHTML = '<b>Tiempo: </b>' + servicioTiempo;

      

    }
      // fin tiempo mis siervicios


   // obtener los datos 
   
    if ($("#l-v").length) {
        var Hsemanaini = $('#l-v').data('semanaini');
        var hsemanafin = $('#l-v').data('semanafin');
        var hsi = $('#sabado').data('findesemanaini');
        var hsf = $('#sabado').data('findesemanafin');
        var hdi = $('#domingo').data('domingoini');
        var hdf = $('#domingo').data('domingofin');


        hini = decimalAHora(Hsemanaini.toString());
        h = hini.substr(0, 2);
        m = hini.substr(3, 3);

        hfin = decimalAHora(hsemanafin.toString());
        hcierre = hfin.substr(0, 2);
        mcierre = hfin.substr(3, 3);

        hsabini = decimalAHora(hsi.toString());
        hsabi = hsabini.substr(0, 2);
        msabi = hsabini.substr(3, 3);

        hsabfin = decimalAHora(hsf.toString());
        hsabf = hsabfin.substr(0, 2);
        msabf = hsabfin.substr(3, 3);

        hdomini = decimalAHora(hdi.toString());
        hdomi = hdomini.substr(0, 2);
        mdomi = hdomini.substr(3, 3);

        hdomfin = decimalAHora(hdf.toString());
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
        /*Imprimir horas con formato y se valida si esta cerrado*/
        formatinicio == '07:00:am' && formatcierre == '07:00:am' ? $('#l-v').text("Lunes a Viernes:" + " " + 'Cerrado' + " " + "-" + " " + 'Cerrado') : $('#l-v').text("Lunes a Viernes:" + " " + formatinicio + " " + "-" + " " + formatcierre),
        formatiniciosab == '07:00:am' && formatcierresab == '07:00:am' ? $('#sabado').text("Sabado:" + " " + 'Cerrado' + " " + "-" + " " + 'Cerrado') : $('#sabado').text("Sabado:" + " " + formatiniciosab + " " + "-" + " " + formatcierresab),
        formatiniciodom == '07:00:am' && formatcierredom == '07:00:am' ? $('#domingo').text("Domingo:" + " " + 'Cerrado' + " " + "-" + " " + 'Cerrado') : $('#domingo').text("Domingo:" + " " + formatiniciodom + " " + "-" + " " + formatcierredom)

    }
    function ModificarHora(decimal) {
        // La función ModificarHora() permanece igual
        decimal = decimal.replace(/,/, '.');
        let horas = Math.floor(decimal),
            minutosDecimal = decimal % 1 * 100,
            minutos = Math.floor(minutosDecimal),
            segundos = Math.floor((minutosDecimal % 1) * 60);

        let am_pm = 'am';
        if (horas >= 12) {
            am_pm = 'pm';
            if (horas > 12) {
                horas -= 12;
            }
        }

        return `${('00' + horas).slice(-2)}:${('00' + minutos).slice(-2)}:${am_pm}`;
    }

    // Verificamos si existen elementos con la clase ".hora"
    if ($(".hora").length) {
        let HoraCita = $('.hora');
        HoraCita.each(function (index, eHora) {
            let hora = $(eHora).data('hora');
            console.log(hora)
            ModHora = ModificarHora(hora.toString());
            // Imprimimos el resultado en el elemento HTML
            $(eHora).html('<b>Hora de la cita: </b>' +"" + ModHora);

        });
    }
  
   

    ///*Activador de datos mas informacion detalles.*/
    //    if ($("#more-information").length) {
    //        const det = document.getElementById(`more-information`);
    //    const pdetalles = document.getElementById(`pdetalles`).getAttribute("data-rating");
    //    function resolveAfter2Seconds() {
    //        return new Promise(resolve => {

    //            resolve('resolved');
    //            if (pdetalles !== "") {
    //                det.classList.add('show');

    //            }

    //        });
    //    }
    //}

    /////* Fin Activador de datos mas innformacion detalles.*/

    if (pathNa == '/SERVICIOS/Details/') {
        if ($("#tiempo").length) {
            tiempo = $('#tiempo').data('tiempo');
            tiempo = tiempo.toString();
            tiempo = tiempo.replace(/,/, '.');
            tiempo = tiempo == 0.50 ? '30 minutos' : tiempo;
            tiempo = tiempo == 1.50 ? '1 hora y 30 minutos' : tiempo;
            tiempo = tiempo == 2.00 ? '2 horas' : tiempo;
            tiempo = tiempo == 1.00 ? '1 hora' : tiempo;

            document.getElementById("tiempo").innerHTML = "<b>Tiempo:</b>" + " " + tiempo;
         /*   $('#tiempo').text = "<b>Tiempo:</b>" + " " + tiempo + " " + "hora";*/

         
            tiempo = tiempo.replace(/,/, '.');
          /*  t = Math.trunc(tiempo)*/
          
        }


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

           /* alert(valor);*/

        }

    });


});


$(document).ready(function () {
    $(document).on('change', 'input[type="file"]', function () {
        let fileName = this.files[0].name;
        let fileSize = this.files[0].size;
        let idname = $(this).attr('id');
        let id = this.id;
        
        if (fileSize > 12000000) {
            let parrafo = $('#p-modal-img');
            parrafo.text('El archivo no debe superar los 12MB');
            $('#modalImg').modal('show'); // abrirr el modal de boostrap
            this.value = '';
            return;
        }

        let validExtensions = ['jpg', 'jpeg', 'png'];
        let fileExtension = fileName.split('.').pop().toLowerCase();

        if (!validExtensions.includes(fileExtension)) {
            let parrafo = $('#p-modal-img');
            parrafo.text('El archivo no tiene la extensión adecuada');
            $('#modalImg').modal('show'); // abrirr el modal de boostrap

            this.value = '';
            return;
        }

        let res = fileName.substring(0, 15) + "...";
        $('span.' + idname).next().find('span').html(res);

        if (pathname == '/SERVICIOS/Create') {
            $('span.' + id).html(res);
        }
    });
});





/*Inicio de validacion */
const formulario = document.getElementById('formulario');

function countChars(obj) {
    document.getElementById("charNum").innerHTML = obj.value.length + ' characters';
}

const inputs = document.querySelectorAll('#formulario input');

const expresiones = {
    usuario: /^[a-zA-Z0-9\_\-\@\s]{4,45}$/, // Letras, numeros, guion y guion_bajo
    nombre: /^[a-zA-ZÀ-ÿ\s]{4,25}$/, // Letras y espacios, pueden llevar acentos de 4 a 25 digitos.
    nombreservicio: /[A-Z? a-z 0-9?]{4,25}$/, //Alfanumerico
    password: /^.{4,12}$/, // 4 a 12 digitos.
    correo: /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i,
    telefono: /^\d{9,10}$/, // 9 a 10 numeros.
    fecha: /^(?:0?[1-9]|1[1-2])([\-/.])(3[01]|[12][0-9]|0?[1-9])\1\d{4}$/, //pra fechas mes/dia/año
    numero: /^[0-9]{1,4}$/, //numeros de 1 a 4 digios
    alphanumerico: /[A-Z? a-z 0-9 À-ÿ .,]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*){10,250}$/,//Alfanumerico
    callenumero: /^[a-zA-ZÀ-ÿ\s#,\.0-9?]{1,45}$/, // Letras, espacios, #, ., comas y números, pueden llevar acentos de 4 a 45 caracteres.
    precio: /^\d{1,4}(\.\d{1,2})?$/, //número decimal o flotante
    fbuser: /[a-zA-Z0-9\_\-\@]/, //Fb user
    iguser: /[a-zA-Z0-9\_\-\@]/,// Instgram User.
    web: /[a-zA-Z0-9\_\-\@]/,
    ciudadnombre: /[A-Z? a-z 0-9?]{10,45}$/,//Alfanumerico
    descralpha: /^(?!.*  )(?=.*[a-zA-Z0-9])[a-zA-Z0-9., ]{10,250}$/, //para los campos descripsion

};


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
    img: false,
    trasaction_reference:false,

}
const validarFormulario = (e) => {
   /* console.log(e);*/
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
            validarCampo(expresiones.nombreservicio, e.target, 'nombre_servicio');
            break;
        case "tiempo":
            validarCampo(expresiones.precio, e.target, 'tiempo');
            break;
        case "descripcion":
            validarCampo(expresiones.descralpha, e.target, 'descripcion');
            break;
        case "calle":
            validarCampo(expresiones.callenumero, e.target, 'calle');
            break;
        case "nombre_peluqueria":
            validarCampo(expresiones.usuario, e.target, 'nombre_peluqueria');
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
        case "img1":
            validarImg();
            break;
        case "trasaction_reference":
            validarCampo(expresiones.descralpha, e.target, 'trasaction_reference');
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



inputs.forEach((input) => {
    input.addEventListener('keyup', validarFormulario);
    input.addEventListener('blur', validarFormulario);

});
const validarImg = () => {
        const inputImagen = $('#img1')[0];
        if (inputImagen.files.length === 0) {
            event.preventDefault(); // Evitar que el formulario se envíe
            campos['img'] = false;
            $('label[for="img1"]').css('outline', '2px solid red');
            $('label[for="img1"]').tooltip({
                title: 'Por favor, sube al menos una imagen para poder continuar',
                placement: 'top',
                 delay: { "show": 200, "hide": 2000 } // 2 segundos de retraso para ocultar
            }).tooltip('show');
            //let parrafo = $('#p-modal');
            //parrafo.text('Por favor subir al menos una imagen para poder continuar');
            $('#modal').modal('show'); // abrirr el modal de boostrap 
        } else {
            campos['img'] = true;
            $('label[for="img1"]').css('outline', 'none');
            $('label[for="img1"]').tooltip({
                title: 'Por favor, sube al menos una imagen para poder continuar',
                placement: 'top'
            }).tooltip('hide');
       
        }


}



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


                if (campos.precio_servicio && campos.nombre_servicio && campos.descripcion && campos.img ) {
                    formulario.submit();

                } else {
                    validarImg();
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;

            // para registrar negocios o peluqueria 
            case '/Account/Register':

                if (campos.Email && campos.nombre && campos.apellido && campos.nombre_peluqueria && campos.Password && campos.ConfirmPassword && campos.calle) {


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

            case '/AspNetUsers/Edit': // para editar la info del que esta logueado
              

                if (campos.nombre && campos.apellido && campos.nombre_peluqueria && campos.calle && campos.telefono && campos.fecha_nacimiento_) {


                    formulario.submit();

                } else {
                    let parrafo = $('#p-modal');
                    parrafo.text('Por favor verificar que los datos del formulario esten correcto.');
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;
            case '/AspNetUsers/EditUserCliente': // para editar la info del que esta logueado usario cliente


                if (campos.nombre && campos.apellido && campos.telefono && campos.fecha_nacimiento_) {


                    formulario.submit();

                } else {
                    let parrafo = $('#p-modal');
                    parrafo.text('Por favor verificar que los datos del formulario esten correcto antes de editar.');
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;

            case '/AspNetUsers/Edit/': // para editar la info del que esta logueado
              
                if (campos.nombre && campos.apellido && campos.nombre_peluqueria  && campos.calle && campos.telefono) {


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
            case '/Account/ForgotPassword': // para olvide la contraseña
                if (campos.Email) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;
           
            case '/REDES_SOCIALES/Edit':
                if (campos.whatsapp) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;
            case '/REDES_SOCIALES/Create':
                if (campos.whatsapp) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;

            case '/SUSCRIPCIONs/Create':  // para crear suscripciones 

        

                if (campos.trasaction_reference && campos.comentario) {
                    formulario.submit();

                } else {
                  
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }

                break;


        }
        // Validacion para casos de paginas dinamicas. 
        switch (pathNa) {

            case '/REDES_SOCIALES/Edit/':
                if (campos.whatsapp) {


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
                if (campos.nombre_servicio && precio_servicio  && campos.descripcion) {


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
             
                if (campos.nombre && campos.apellido && campos.nombre_peluqueria  && campos.calle && campos.telefono) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;

            case '/AspNetUsers/Edit/':
           
                if (campos.nombre && campos.apellido && campos.nombre_peluqueria && campos.calle && campos.telefono) {


                    formulario.submit();

                } else {
                    $('#modal').modal('show'); // abrirr el modal de boostrap 

                }
                break;

        }


    });
}

if (pathname === '/REDES_SOCIALES/Edit') {
    let inputs = document.querySelectorAll('input[type=text]');
    for (let i = 0; i < inputs.length ; i++) {

        document.getElementById(`grupo__${inputs[i].name}`).classList.remove('formulario__grupo-incorrecto');
        document.getElementById(`grupo__${inputs[i].name}`).classList.add('formulario__grupo-correcto');
        document.querySelector(`#grupo__${inputs[i].name} i`).classList.add('fa-check-circle');
        document.querySelector(`#grupo__${inputs[i].name} i`).classList.remove('fa-times-circle');
        document.querySelector(`#grupo__${inputs[i].name} .formulario__input-error`).classList.remove('formulario__input-error-activo')
        campos[inputs[i].name] = true;

    }


}

if (pathNa === '/AspNetUsers/Edit/') {
    let inputs = document.querySelectorAll('input[type=text]');
    for (let i = 0; i < inputs.length -2; i++) {

        document.getElementById(`grupo__${inputs[i].name}`).classList.remove('formulario__grupo-incorrecto');
        document.getElementById(`grupo__${inputs[i].name}`).classList.add('formulario__grupo-correcto');
        document.querySelector(`#grupo__${inputs[i].name} i`).classList.add('fa-check-circle');
        document.querySelector(`#grupo__${inputs[i].name} i`).classList.remove('fa-times-circle');
        document.querySelector(`#grupo__${inputs[i].name} .formulario__input-error`).classList.remove('formulario__input-error-activo')
        campos[inputs[i].name] = true;

    }
    document.getElementById(`grupo__fecha_nacimiento_`).classList.remove('formulario__grupo-incorrecto');
    document.getElementById(`grupo__fecha_nacimiento_`).classList.add('formulario__grupo-correcto');
    document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.add('fa-check-circle');
    document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.remove('fa-times-circle');
    document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-error`).classList.remove('formulario__input-error-activo')
    document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-errorr`).classList.remove('formulario__input-error-activo');
    campos[fecha_nacimiento_] = true;
}
if (pathname === '/AspNetUsers/EditUserCliente') {
    let inputs = document.querySelectorAll('input[type=text]');
    for (let i = 0; i < inputs.length; i++) {

        document.getElementById(`grupo__${inputs[i].name}`).classList.remove('formulario__grupo-incorrecto');
        document.getElementById(`grupo__${inputs[i].name}`).classList.add('formulario__grupo-correcto');
        document.querySelector(`#grupo__${inputs[i].name} i`).classList.add('fa-check-circle');
        document.querySelector(`#grupo__${inputs[i].name} i`).classList.remove('fa-times-circle');
        document.querySelector(`#grupo__${inputs[i].name} .formulario__input-error`).classList.remove('formulario__input-error-activo')
        campos[inputs[i].name] = true;

    }
    document.getElementById(`grupo__fecha_nacimiento_`).classList.remove('formulario__grupo-incorrecto');
    document.getElementById(`grupo__fecha_nacimiento_`).classList.add('formulario__grupo-correcto');
    document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.add('fa-check-circle');
    document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.remove('fa-times-circle');
    document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-error`).classList.remove('formulario__input-error-activo')
    document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-errorr`).classList.remove('formulario__input-error-activo');
    campos['fecha_nacimiento_'] = true;
}









    // comentarios asincronos.
    if ($('#pdetalles').length) {
    $(function () {
        let $h3s = $('li.opcion-detalles').click(function () {
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
      
    }

    asyncCall();


        let triggerTabList = [].slice.call(document.querySelectorAll('#myTab a'))
    triggerTabList.forEach(function (triggerEl) {
        let tabTrigger = new bootstrap.Tab(triggerEl)

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
///*FIN de Coookiess*/
//* Carrusel*/

window.addEventListener('load', function () {
    // Mobile-first defaults

    if (carousel = document.querySelector('.carousel__lista')) {

        new Glider(document.querySelector('.carousel__lista'), {
            slidesToShow: 1,
            slidesToScroll: 1,
            scrollLock: false,
            dots: '.carousel__indicadores',
            draggable: true,
            arrows: {
                prev: '.carousel__anterior',
                next: '.carousel__siguiente'
            },
            responsive: [
                {
                    // screens greater than >= 775px
                    breakpoint: 450,
                    draggable: true,
                    settings: {
                        // Set to `auto` and provide item width to adjust to viewport
                        slidesToShow: 1,
                        slidesToScroll: 1
                       
                    }
                }, {
                    // screens greater than >= 1024px
                    breakpoint: 800,
                    draggable: true,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                       
                    }
                }
            ]
        });



        let touchStartX = 0;
        let touchStartY = 0;
        let touchEndX = 0;
        let touchEndY = 0;
        let elementosTouch = document.querySelectorAll(".galeria");

        for (let i = 0; i < elementosTouch.length; i++) {
            elementosTouch[i].addEventListener('touchstart', function (event) {
                touchStartX = event.changedTouches[0].screenX;
                touchStartY = event.changedTouches[0].screenY;
            });

            elementosTouch[i].addEventListener('touchend', function (event) {
                touchEndX = event.changedTouches[0].screenX;
                touchEndY = event.changedTouches[0].screenY;
                slideG();
            });
        }

        function slideG() {
            if (touchEndX < touchStartX) {
                $(".carousel__siguiente").trigger("click");
              
            } else if (touchEndX > touchStartX) {
                $(".carousel__anterior").trigger("click");
               
            }

            if (touchEndY < touchStartY) {
              
            } else if (touchEndY > touchStartY) {
             
            }

            if (touchEndY == touchStartY && touchEndX == touchStartX) {
               
            }
        }






    }
});




/*MAPA para servicio*/

if (pathNa === '/SERVICIOS/Details/') {
    let latitud = $('#latitud').data('latitud');
    let longitud = $('#longitud').data('longitud');
    let nombre = $('#nombre-negocio').data('nombre');
    let map = L.map('map').setView([latitud, longitud], 15);
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
var msf_getFsTag = document.getElementsByTagName("fieldset");
/*let eml = document.getElementById("Email");*/
if (msf_getFsTag.length) {
   
    //// dom variables

    //var msf_getFsTag = document.getElementsByTagName("fieldset");


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

    let tileLayer = new L.tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: 'Map data & copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'your.mapbox.access.token'
    });


    let map = new L.Map('map', {

        'center': [-0.18205139994814276, -78.46831482728689],
        'zoom': 12,
        'layers': [tileLayer]
    });

    let marker = L.marker([-0.18205139994814276, -78.46831482728689], {
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
            scrollLock: true,
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
        document.getElementById('Longitud').value = marker.getLatLng().lng;
      
    });

    /* FIN MAPA*/


}

if (pathNa == '/SERVICIOS/Edit/') {
    let inputs = document.querySelectorAll('input[type=text]');
    for (var i = 0; i < inputs.length -1; i++) {
     
        document.getElementById(`grupo__${inputs[i].name}`).classList.remove('formulario__grupo-incorrecto');
        document.getElementById(`grupo__${inputs[i].name}`).classList.add('formulario__grupo-correcto');
        document.querySelector(`#grupo__${inputs[i].name} i`).classList.add('fa-check-circle');
        document.querySelector(`#grupo__${inputs[i].name} i`).classList.remove('fa-times-circle');
        document.querySelector(`#grupo__${inputs[i].name} .formulario__input-error`).classList.remove('formulario__input-error-activo')
        campos[inputs[i].name] = true;

    }


    //data que viene selecccionada ya de la bd 
  
    
    $("#select").val($("#duraciones").val())
    //obtener valor cambiado del select
    $("#select").change(function () {
        console.log($('#select').val()
        )
        let selected = $('#select').val()
        $("#duraciones").val(selected)
    });





}
if (pathname === '/AspNetUsers/Edit') {

    let inputs = document.querySelectorAll('input[type=text]');
    for (var i = 0; i < inputs.length - 2; i++) {
       
        document.getElementById(`grupo__${inputs[i].name}`).classList.remove('formulario__grupo-incorrecto');
        document.getElementById(`grupo__${inputs[i].name}`).classList.add('formulario__grupo-correcto');
        document.querySelector(`#grupo__${inputs[i].name} i`).classList.add('fa-check-circle');
        document.querySelector(`#grupo__${inputs[i].name} i`).classList.remove('fa-times-circle');
        document.querySelector(`#grupo__${inputs[i].name} .formulario__input-error`).classList.remove('formulario__input-error-activo')
        campos[inputs[i].name] = true;
       
    }
    document.getElementById(`grupo__fecha_nacimiento_`).classList.remove('formulario__grupo-incorrecto');
    document.getElementById(`grupo__fecha_nacimiento_`).classList.add('formulario__grupo-correcto');
    document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.add('fa-check-circle');
    document.querySelector(`#grupo__fecha_nacimiento_ i`).classList.remove('fa-times-circle');
    document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-error`).classList.remove('formulario__input-error-activo')
    document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-errorr`).classList.remove('formulario__input-error-activo');
    campos['fecha_nacimiento_'] = true;
    
  /*  validarFormulario*/


    //Mapa

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
        document.getElementById('Longitud').value = marker.getLatLng().lng;
  
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
       /* console.log(document.getElementById('Latitud').value = marker.getLatLng().lat)*/
        document.getElementById('Longitud').value = marker.getLatLng().lng;
      /*  console.log(document.getElementById('Longitud').text = marker.getLatLng().lng)*/
    });

    /* FIN MAPA*/
}



$(document).ready(function () {

    // Creamos el array con las parroquias de los cantones

    let cantonesYProvincias = {

        "CUENCA": ["BELLAVISTA", "CAÑARIBAMBA", "EL BATÁN", "EL SAGRARIO", "EL VECINO", "GIL RAMÍREZ DÁVALOS", "HUAYNACÁPAC", "MACHÁNGARA", "MONAY", "SAN BLAS", "SAN SEBASTIÁN", "SUCRE", "TOTORACOCHA", "YANUNCAY", "HERMANO MIGUEL", "CUENCA", "BAÑOS", "CUMBE", "CHAUCHA", "CHECA (JIDCAY)", "CHIQUINTAD", "LLACAO", "MOLLETURO", "NULTI", "OCTAVIO CORDERO PALACIOS (SANTA ROSA)", "PACCHA", "QUINGEO", "RICAURTE", "SAN JOAQUÍN", "SANTA ANA", "SAYAUSÍ", "SIDCAY", "SININCAY", "TARQUI", "TURI", "VALLE", "VICTORIA DEL PORTETE (IRQUIS)"],
        "GIRÓN": ["GIRÓN", "ASUNCIÓN", "SAN GERARDO"],
        "GUALACEO": ["GUALACEO", "CHORDELEG", "DANIEL CÓRDOVA TORAL (EL ORIENTE)", "JADÁN", "MARIANO MORENO", "PRINCIPAL", "REMIGIO CRESPO TORAL (GÚLAG)", "SAN JUAN", "ZHIDMAD", "LUIS CORDERO VEGA", "SIMÓN BOLÍVAR (CAB. EN GAÑANZOL)"],
        "NABÓN": ["NABÓN", "COCHAPATA", "EL PROGRESO (CAB.EN ZHOTA)", "LAS NIEVES (CHAYA)", "OÑA"],
        "PAUTE": ["PAUTE", "AMALUZA", "BULÁN (JOSÉ VÍCTOR IZQUIERDO)", "CHICÁN (GUILLERMO ORTEGA)", "EL CABO", "GUACHAPALA", "GUARAINAG", "PALMAS", "PAN", "SAN CRISTÓBAL (CARLOS ORDÓÑEZ LAZO)", "SEVILLA DE ORO", "TOMEBAMBA", "DUG DUG"],
        "PUCARA": ["PUCARÁ", "CAMILO PONCE ENRÍQUEZ (CAB. EN RÍO 7 DE MOLLEPONGO", "SAN RAFAEL DE SHARUG"],
        "SAN FERNANDO": ["SAN FERNANDO", "CHUMBLÍN"],
        "SANTA ISABEL": ["SANTA ISABEL (CHAGUARURCO)", "ABDÓN CALDERÓN (LA UNIÓN)", "EL CARMEN DE PIJILÍ", "ZHAGLLI (SHAGLLI)", "SAN SALVADOR DE CAÑARIBAMBA"],
        "SIGSIG": ["SIGSIG", "CUCHIL (CUTCHIL)", "GIMA", "GUEL", "LUDO", "SAN BARTOLOMÉ", "SAN JOSÉ DE RARANGA"],
        "OÑA": ["SAN FELIPE DE OÑA CABECERA CANTONAL", "SUSUDEL"],
        "CHORDELEG": ["CHORDELEG", "PRINCIPAL", "LA UNIÓN", "LUIS GALARZA ORELLANA (CAB.EN DELEGSOL)", "SAN MARTÍN DE PUZHIO"],
        "EL PAN": ["EL PAN", "AMALUZA", "PALMAS", "SAN VICENTE"],
        "SEVILLA DE ORO": ["SEVILLA DE ORO", "AMALUZA", "PALMAS"],
        "GUACHAPALA": ["GUACHAPALA"],
        "CAMILO PONCE ENRÍQUEZ": ["CAMILO PONCE ENRÍQUEZ", "EL CARMEN DE PIJILÍ"],
        "GUARANDA": ["ÁNGEL POLIBIO CHÁVES", "GABRIEL IGNACIO VEINTIMILLA", "GUANUJO", "GUARANDA", "FACUNDO VELA", "JULIO E. MORENO (CATANAHUÁN GRANDE)", "LAS NAVES", "SALINAS", "SAN LORENZO", "SAN SIMÓN (YACOTO)", "SANTA FÉ (SANTA FÉ)", "SIMIÁTUG", "SAN LUIS DE PAMBIL"],
        "CHILLANES": ["CHILLANES", "SAN JOSÉ DEL TAMBO (TAMBOPAMBA)"],
        "CHIMBO": ["SAN JOSÉ DE CHIMBO", "ASUNCIÓN (ASANCOTO)", "CALUMA", "MAGDALENA (CHAPACOTO)", "SAN SEBASTIÁN", "TELIMBELA"],
        "ECHEANDÍA": ["ECHEANDÍA"],
        "SAN MIGUEL": ["SAN MIGUEL", "BALSAPAMBA", "BILOVÁN", "RÉGULO DE MORA", "SAN PABLO (SAN PABLO DE ATENAS)", "SANTIAGO", "SAN VICENTE"],
        "CALUMA": ["CALUMA"],
        "LAS NAVES": ["LAS MERCEDES", "LAS NAVES", "LAS NAVES"],
        "AZOGUES": ["AURELIO BAYAS MARTÍNEZ", "AZOGUES", "BORRERO", "SAN FRANCISCO", "AZOGUES", "COJITAMBO", "DÉLEG", "GUAPÁN", "JAVIER LOYOLA (CHUQUIPATA)", "LUIS CORDERO", "PINDILIG", "RIVERA", "SAN MIGUEL", "SOLANO", "TADAY"],
        "BIBLIÁN": ["BIBLIÁN", "NAZÓN (CAB. EN PAMPA DE DOMÍNGUEZ)", "SAN FRANCISCO DE SAGEO", "TURUPAMBA", "JERUSALÉN"],
        "CAÑAR": ["CAÑAR", "CHONTAMARCA", "CHOROCOPTE", "GENERAL MORALES (SOCARTE)", "GUALLETURO", "HONORATO VÁSQUEZ (TAMBO VIEJO)", "INGAPIRCA", "JUNCAL", "SAN ANTONIO", "SUSCAL", "TAMBO", "ZHUD", "VENTURA", "DUCUR"],
        "LA TRONCAL": ["LA TRONCAL", "MANUEL J. CALLE", "PANCHO NEGRO"],
        "EL TAMBO": ["EL TAMBO"],
        "DÉLEG": ["DÉLEG", "SOLANO"],
        "SUSCAL": ["SUSCAL"],
        "TULCÁN": ["GONZÁLEZ SUÁREZ", "TULCÁN", "TULCÁN", "EL CARMELO (EL PUN)", "HUACA", "JULIO ANDRADE (OREJUELA)", "MALDONADO", "PIOTER", "TOBAR DONOSO (LA BOCANA DE CAMUMBÍ)", "TUFIÑO", "URBINA (TAYA)", "EL CHICAL", "MARISCAL SUCRE", "SANTA MARTHA DE CUBA"],
        "BOLÍVAR": ["BOLÍVAR", "GARCÍA MORENO", "LOS ANDES", "MONTE OLIVO", "SAN VICENTE DE PUSIR", "SAN RAFAEL", "CALCETA", "MEMBRILLO", "QUIROGA"],
        "ESPEJO": ["EL ÁNGEL", "27 DE SEPTIEMBRE", "EL ANGEL", "EL GOALTAL", "LA LIBERTAD (ALIZO)", "SAN ISIDRO"],
        "MIRA": ["MIRA (CHONTAHUASI)", "CONCEPCIÓN", "JIJÓN Y CAAMAÑO (CAB. EN RÍO BLANCO)", "JUAN MONTALVO (SAN IGNACIO DE QUIL)"],
        "MONTÚFAR": ["GONZÁLEZ SUÁREZ", "SAN JOSÉ", "SAN GABRIEL", "CRISTÓBAL COLÓN", "CHITÁN DE NAVARRETE", "FERNÁNDEZ SALVADOR", "LA PAZ", "PIARTAL"],
        "SAN PEDRO DE HUACA": ["HUACA", "MARISCAL SUCRE"],
        "LATACUNGA": ["ELOY ALFARO (SAN FELIPE)", "IGNACIO FLORES (PARQUE FLORES)", "JUAN MONTALVO (SAN SEBASTIÁN)", "LA MATRIZ", "SAN BUENAVENTURA", "LATACUNGA", "ALAQUES (ALÁQUEZ)", "BELISARIO QUEVEDO (GUANAILÍN)", "GUAITACAMA (GUAYTACAMA)", "JOSEGUANGO BAJO", "LAS PAMPAS", "MULALÓ", "11 DE NOVIEMBRE (ILINCHISI)", "POALÓ", "SAN JUAN DE PASTOCALLE", "SIGCHOS", "TANICUCHÍ", "TOACASO", "PALO QUEMADO"],
        "LA MANÁ": ["EL CARMEN", "LA MANÁ", "EL TRIUNFO", "LA MANÁ", "GUASAGANDA (CAB.EN GUASAGANDA)", "PUCAYACU"],
        "PANGUA": ["EL CORAZÓN", "MORASPUNGO", "PINLLOPATA", "RAMÓN CAMPAÑA"],
        "PUJILI": ["PUJILÍ", "ANGAMARCA", "CHUCCHILÁN (CHUGCHILÁN)", "GUANGAJE", "ISINLIBÍ (ISINLIVÍ)", "LA VICTORIA", "PILALÓ", "TINGO", "ZUMBAHUA"],
        "SALCEDO": ["SAN MIGUEL", "ANTONIO JOSÉ HOLGUÍN (SANTA LUCÍA)", "CUSUBAMBA", "MULALILLO", "MULLIQUINDIL (SANTA ANA)", "PANSALEO"],
        "SAQUISILÍ": ["SAQUISILÍ", "CANCHAGUA", "CHANTILÍN", "COCHAPAMBA"],
        "SIGCHOS": ["SIGCHOS", "CHUGCHILLÁN", "ISINLIVÍ", "LAS PAMPAS", "PALO QUEMADO"],
        "RIOBAMBA": ["LIZARZABURU", "MALDONADO", "VELASCO", "VELOZ", "YARUQUÍES", "RIOBAMBA", "CACHA (CAB. EN MACHÁNGARA)", "CALPI", "CUBIJÍES", "FLORES", "LICÁN", "LICTO", "PUNGALÁ", "PUNÍN", "QUIMIAG", "SAN JUAN", "SAN LUIS"],
        "ALAUSI": ["ALAUSÍ", "ACHUPALLAS", "CUMANDÁ", "GUASUNTOS", "HUIGRA", "MULTITUD", "PISTISHÍ (NARIZ DEL DIABLO)", "PUMALLACTA", "SEVILLA", "SIBAMBE", "TIXÁN"],
        "COLTA": ["CAJABAMBA", "SICALPA", "VILLA LA UNIÓN (CAJABAMBA)", "CAÑI", "COLUMBE", "JUAN DE VELASCO (PANGOR)", "SANTIAGO DE QUITO (CAB. EN SAN ANTONIO DE QUITO)"],
        "CHAMBO": ["CHAMBO"],
        "CHUNCHI": ["CHUNCHI", "CAPZOL", "COMPUD", "GONZOL", "LLAGOS"],
        "GUAMOTE": ["GUAMOTE", "CEBADAS", "PALMIRA"],
        "GUANO": ["EL ROSARIO", "LA MATRIZ", "GUANO", "GUANANDO", "ILAPO", "LA PROVIDENCIA", "SAN ANDRÉS", "SAN GERARDO DE PACAICAGUÁN", "SAN ISIDRO DE PATULÚ", "SAN JOSÉ DEL CHAZO", "SANTA FÉ DE GALÁN", "VALPARAÍSO"],
        "PALLATANGA": ["PALLATANGA"],
        "PENIPE": ["PENIPE", "EL ALTAR", "MATUS", "PUELA", "SAN ANTONIO DE BAYUSHIG", "LA CANDELARIA", "BILBAO (CAB.EN QUILLUYACU)"],
        "CUMANDÁ": ["CUMANDÁ"],
        "MACHALA": ["LA PROVIDENCIA", "MACHALA", "PUERTO BOLÍVAR", "NUEVE DE MAYO", "EL CAMBIO", "EL RETIRO"],
        "ARENILLAS": ["ARENILLAS", "CHACRAS", "LA LIBERTAD", "LAS LAJAS (CAB. EN LA VICTORIA)", "PALMALES", "CARCABÓN"],
        "ATAHUALPA": ["PACCHA", "AYAPAMBA", "CORDONCILLO", "MILAGRO", "SAN JOSÉ", "SAN JUAN DE CERRO AZUL"],
        "BALSAS": ["BALSAS", "BELLAMARÍA"],
        "CHILLA": ["CHILLA"],
        "EL GUABO": ["EL GUABO", "BARBONES (SUCRE)", "LA IBERIA", "TENDALES (CAB.EN PUERTO TENDALES)", "RÍO BONITO"],
        "HUAQUILLAS": ["ECUADOR", "EL PARAÍSO", "HUALTACO", "MILTON REYES", "UNIÓN LOJANA", "HUAQUILLAS"],
        "MARCABELÍ": ["MARCABELÍ", "EL INGENIO"],
        "PASAJE": ["BOLÍVAR", "LOMA DE FRANCO", "OCHOA LEÓN (MATRIZ)", "TRES CERRITOS", "PASAJE", "BUENAVISTA", "CASACAY", "LA PEAÑA", "PROGRESO", "UZHCURRUMI", "CAÑAQUEMADA"],
        "PIÑAS": ["LA MATRIZ", "LA SUSAYA", "PIÑAS GRANDE", "PIÑAS", "CAPIRO (CAB. EN LA CAPILLA DE CAPIRO)", "LA BOCANA", "MOROMORO (CAB. EN EL VADO)", "PIEDRAS", "SAN ROQUE (AMBROSIO MALDONADO)", "SARACAY"],
        "PORTOVELO": ["PORTOVELO", "CURTINCAPA", "MORALES", "SALATÍ"],
        "SANTA ROSA": ["SANTA ROSA", "PUERTO JELÍ", "BALNEARIO JAMBELÍ (SATÉLITE)", "JUMÓN (SATÉLITE)", "NUEVO SANTA ROSA", "SANTA ROSA", "BELLAVISTA", "JAMBELÍ", "LA AVANZADA", "SAN ANTONIO", "TORATA", "VICTORIA"],
        "ZARUMA": ["ZARUMA", "ABAÑÍN", "ARCAPAMBA", "GUANAZÁN", "GUIZHAGUIÑA", "HUERTAS", "MALVAS", "MULUNCAY GRANDE", "SINSAO", "SALVIAS"],
        "LAS LAJAS": ["LA VICTORIA", "PLATANILLOS", "VALLE HERMOSO", "LA VICTORIA", "LA LIBERTAD", "EL PARAÍSO", "SAN ISIDRO"],
        "ESMERALDAS": ["BARTOLOMÉ RUIZ (CÉSAR FRANCO CARRIÓN)", "5 DE AGOSTO", "ESMERALDAS", "LUIS TELLO (LAS PALMAS)", "SIMÓN PLATA TORRES", "ESMERALDAS", "ATACAMES", "CAMARONES (CAB. EN SAN VICENTE)", "CRNEL. CARLOS CONCHA TORRES (CAB.EN HUELE)", "CHINCA", "CHONTADURO", "CHUMUNDÉ", "LAGARTO", "LA UNIÓN", "MAJUA", "MONTALVO (CAB. EN HORQUETA)", "RÍO VERDE", "ROCAFUERTE", "SAN MATEO", "SÚA (CAB. EN LA BOCANA)", "TABIAZO", "TACHINA", "TONCHIGÜE", "VUELTA LARGA"],
        "ELOY ALFARO": ["VALDEZ (LIMONES)", "ANCHAYACU", "ATAHUALPA (CAB. EN CAMARONES)", "BORBÓN", "LA TOLA", "LUIS VARGAS TORRES (CAB. EN PLAYA DE ORO)", "MALDONADO", "PAMPANAL DE BOLÍVAR", "SAN FRANCISCO DE ONZOLE", "SANTO DOMINGO DE ONZOLE", "SELVA ALEGRE", "TELEMBÍ", "COLÓN ELOY DEL MARÍA", "SAN JOSÉ DE CAYAPAS", "TIMBIRÉ"],
        "MUISNE": ["MUISNE", "BOLÍVAR", "DAULE", "GALERA", "QUINGUE (OLMEDO PERDOMO FRANCO)", "SALIMA", "SAN FRANCISCO", "SAN GREGORIO", "SAN JOSÉ DE CHAMANGA (CAB.EN CHAMANGA)"],
        "QUININDÉ": ["ROSA ZÁRATE (QUININDÉ)", "CUBE", "CHURA (CHANCAMA) (CAB. EN EL YERBERO)", "MALIMPIA", "VICHE", "LA UNIÓN"],
        "SAN LORENZO": ["SAN LORENZO", "ALTO TAMBO (CAB. EN GUADUAL)", "ANCÓN (PICHANGAL) (CAB. EN PALMA REAL)", "CALDERÓN", "CARONDELET", "5 DE JUNIO (CAB. EN UIMBI)", "CONCEPCIÓN", "MATAJE (CAB. EN SANTANDER)", "SAN JAVIER DE CACHAVÍ (CAB. EN SAN JAVIER)", "SANTA RITA", "TAMBILLO", "TULULBÍ (CAB. EN RICAURTE)", "URBINA"],
        "ATACAMES": ["ATACAMES", "LA UNIÓN", "SÚA (CAB. EN LA BOCANA)", "TONCHIGÜE", "TONSUPA"],
        "RIOVERDE": ["RIOVERDE", "CHONTADURO", "CHUMUNDÉ", "LAGARTO", "MONTALVO (CAB. EN HORQUETA)", "ROCAFUERTE"],
        "LA CONCORDIA": ["LA CONCORDIA", "MONTERREY", "LA VILLEGAS", "PLAN PILOTO"],
        "GUAYAQUIL": ["AYACUCHO", "BOLÍVAR (SAGRARIO)", "CARBO (CONCEPCIÓN)", "FEBRES CORDERO", "GARCÍA MORENO", "LETAMENDI", "NUEVE DE OCTUBRE", "OLMEDO (SAN ALEJO)", "ROCA", "ROCAFUERTE", "SUCRE", "TARQUI", "URDANETA", "XIMENA", "PASCUALES", "GUAYAQUIL", "CHONGÓN", "JUAN GÓMEZ RENDÓN (PROGRESO)", "MORRO", "PASCUALES", "PLAYAS (GRAL. VILLAMIL)", "POSORJA", "PUNÁ", "TENGUEL"],
        "DO BAQUERIZO MORENO JU": ["ALFREDO BAQUERIZO MORENO (JUJÁN)"],
        "BALAO": ["BALAO"],
        "BALZAR": ["BALZAR"],
        "COLIMES": ["COLIMES", "SAN JACINTO"],
        "DAULE": ["DAULE", "LA AURORA (SATÉLITE)", "BANIFE", "EMILIANO CAICEDO MARCOS", "MAGRO", "PADRE JUAN BAUTISTA AGUIRRE", "SANTA CLARA", "VICENTE PIEDRAHITA", "DAULE", "ISIDRO AYORA (SOLEDAD)", "JUAN BAUTISTA AGUIRRE (LOS TINTOS)", "LAUREL", "LIMONAL", "LOMAS DE SARGENTILLO", "LOS LOJAS (ENRIQUE BAQUERIZO MORENO)", "PIEDRAHITA (NOBOL)"],
        "DURÁN": ["ELOY ALFARO (DURÁN)", "EL RECREO"],
        "EL EMPALME": ["VELASCO IBARRA (EL EMPALME)", "GUAYAS (PUEBLO NUEVO)", "EL ROSARIO"],
        "EL TRIUNFO": ["EL TRIUNFO"],
        "MILAGRO": ["MILAGRO", "CHOBO", "GENERAL ELIZALDE (BUCAY)", "MARISCAL SUCRE (HUAQUES)", "ROBERTO ASTUDILLO (CAB. EN CRUCE DE VENECIA)"],
        "NARANJAL": ["NARANJAL", "JESÚS MARÍA", "SAN CARLOS", "SANTA ROSA DE FLANDES", "TAURA"],
        "NARANJITO": ["NARANJITO"],
        "PALESTINA": ["PALESTINA"],
        "PEDRO CARBO": ["PEDRO CARBO"],
        "SAMBORONDÓN": ["SAMBORONDÓN", "LA PUNTILLA (SATÉLITE)", "SAMBORONDÓN", "TARIFA"],
        "SANTA LUCÍA": ["SANTA LUCÍA"],
        "SALITRE URBINA JADO": ["BOCANA", "CANDILEJOS", "CENTRAL", "PARAÍSO", "SAN MATEO", "EL SALITRE (LAS RAMAS)", "GRAL. VERNAZA (DOS ESTEROS)", "LA VICTORIA (ÑAUZA)", "JUNQUILLAL"],
        "SAN JACINTO DE YAGUACHI": ["SAN JACINTO DE YAGUACHI", "CRNEL. LORENZO DE GARAICOA (PEDREGAL)", "CRNEL. MARCELINO MARIDUEÑA (SAN CARLOS)", "GRAL. PEDRO J. MONTERO (BOLICHE)", "SIMÓN BOLÍVAR", "YAGUACHI VIEJO (CONE)", "VIRGEN DE FÁTIMA"],
        "PLAYAS": ["GENERAL VILLAMIL (PLAYAS)"],
        "SIMÓN BOLÍVAR": ["SIMÓN BOLÍVAR", "CRNEL.LORENZO DE GARAICOA (PEDREGAL)"],
        "ORONEL MARCELINO MARIDUE": ["CORONEL MARCELINO MARIDUEÑA (SAN CARLOS)"],
        "LOMAS DE SARGENTILLO": ["LOMAS DE SARGENTILLO", "ISIDRO AYORA (SOLEDAD)"],
        "NOBOL": ["NARCISA DE JESÚS"],
        "GENERAL ANTONIO ELIZALDE": ["GENERAL ANTONIO ELIZALDE (BUCAY)"],
        "ISIDRO AYORA": ["ISIDRO AYORA"],
        "IBARRA": ["CARANQUI", "GUAYAQUIL DE ALPACHACA", "SAGRARIO", "SAN FRANCISCO", "LA DOLOROSA DEL PRIORATO", "SAN MIGUEL DE IBARRA", "AMBUQUÍ", "ANGOCHAGUA", "CAROLINA", "LA ESPERANZA", "LITA", "SALINAS", "SAN ANTONIO"],
        "ANTONIO ANTE": ["ANDRADE MARÍN (LOURDES)", "ATUNTAQUI", "IMBAYA (SAN LUIS DE COBUENDO)", "SAN FRANCISCO DE NATABUELA", "SAN JOSÉ DE CHALTURA", "SAN ROQUE"],
        "COTACACHI": ["SAGRARIO", "SAN FRANCISCO", "COTACACHI", "APUELA", "GARCÍA MORENO (LLURIMAGUA)", "IMANTAG", "PEÑAHERRERA", "PLAZA GUTIÉRREZ (CALVARIO)", "QUIROGA", "6 DE JULIO DE CUELLAJE (CAB. EN CUELLAJE)", "VACAS GALINDO (EL CHURO) (CAB.EN SAN MIGUEL ALTO)"],
        "OTAVALO": ["JORDÁN", "SAN LUIS", "OTAVALO", "DR. MIGUEL EGAS CABEZAS (PEGUCHE)", "EUGENIO ESPEJO (CALPAQUÍ)", "GONZÁLEZ SUÁREZ", "PATAQUÍ", "SAN JOSÉ DE QUICHINCHE", "SAN JUAN DE ILUMÁN", "SAN PABLO", "SAN RAFAEL", "SELVA ALEGRE (CAB.EN SAN MIGUEL DE PAMPLONA)"],
        "PIMAMPIRO": ["PIMAMPIRO", "CHUGÁ", "MARIANO ACOSTA", "SAN FRANCISCO DE SIGSIPAMBA"],
        "SAN MIGUEL DE URCUQUÍ": ["URCUQUÍ CABECERA CANTONAL", "CAHUASQUÍ", "LA MERCED DE BUENOS AIRES", "PABLO ARENAS", "SAN BLAS", "TUMBABIRO"],
        "LOJA": ["EL SAGRARIO", "SAN SEBASTIÁN", "SUCRE", "VALLE", "LOJA", "CHANTACO", "CHUQUIRIBAMBA", "EL CISNE", "GUALEL", "JIMBILLA", "MALACATOS (VALLADOLID)", "SAN LUCAS", "SAN PEDRO DE VILCABAMBA", "SANTIAGO", "TAQUIL (MIGUEL RIOFRÍO)", "VILCABAMBA (VICTORIA)", "YANGANA (ARSENIO CASTILLO)", "QUINARA"],
        "CALVAS": ["CARIAMANGA", "CHILE", "SAN VICENTE", "CARIAMANGA", "COLAISACA", "EL LUCERO", "UTUANA", "SANGUILLÍN"],
        "CATAMAYO": ["CATAMAYO", "SAN JOSÉ", "CATAMAYO (LA TOMA)", "EL TAMBO", "GUAYQUICHUMA", "SAN PEDRO DE LA BENDITA", "ZAMBI"],
        "CELICA": ["CELICA", "CRUZPAMBA (CAB. EN CARLOS BUSTAMANTE)", "CHAQUINAL", "12 DE DICIEMBRE (CAB. EN ACHIOTES)", "PINDAL (FEDERICO PÁEZ)", "POZUL (SAN JUAN DE POZUL)", "SABANILLA", "TNTE. MAXIMILIANO RODRÍGUEZ LOAIZA"],
        "CHAGUARPAMBA": ["CHAGUARPAMBA", "BUENAVISTA", "EL ROSARIO", "SANTA RUFINA", "AMARILLOS"],
        "ESPÍNDOLA": ["AMALUZA", "BELLAVISTA", "JIMBURA", "SANTA TERESITA", "27 DE ABRIL (CAB. EN LA NARANJA)", "EL INGENIO", "EL AIRO"],
        "GONZANAMÁ": ["GONZANAMÁ", "CHANGAIMINA (LA LIBERTAD)", "FUNDOCHAMBA", "NAMBACOLA", "PURUNUMA (EGUIGUREN)", "QUILANGA (LA PAZ)", "SACAPALCA", "SAN ANTONIO DE LAS ARADAS (CAB. EN LAS ARADAS)"],
        "MACARÁ": ["GENERAL ELOY ALFARO (SAN SEBASTIÁN)", "MACARÁ (MANUEL ENRIQUE RENGEL SUQUILANDA)", "MACARÁ", "LARAMA", "LA VICTORIA", "SABIANGO (LA CAPILLA)"],
        "PALTAS": ["CATACOCHA", "LOURDES", "CATACOCHA", "CANGONAMÁ", "GUACHANAMÁ", "LA TINGUE", "LAURO GUERRERO", "OLMEDO (SANTA BÁRBARA)", "ORIANGA", "SAN ANTONIO", "CASANGA", "YAMANA"],
        "PUYANGO": ["ALAMOR", "CIANO", "EL ARENAL", "EL LIMO (MARIANA DE JESÚS)", "MERCADILLO", "VICENTINO"],
        "SARAGURO": ["SARAGURO", "EL PARAÍSO DE CELÉN", "EL TABLÓN", "LLUZHAPA", "MANÚ", "SAN ANTONIO DE QUMBE (CUMBE)", "SAN PABLO DE TENTA", "SAN SEBASTIÁN DE YÚLUC", "SELVA ALEGRE", "URDANETA (PAQUISHAPA)", "SUMAYPAMBA"],
        "SOZORANGA": ["SOZORANGA", "NUEVA FÁTIMA", "TACAMOROS"],
        "ZAPOTILLO": ["ZAPOTILLO", "MANGAHURCO (CAZADEROS)", "GARZAREAL", "LIMONES", "PALETILLAS", "BOLASPAMBA"],
        "PINDAL": ["PINDAL", "CHAQUINAL", "12 DE DICIEMBRE (CAB.EN ACHIOTES)", "MILAGROS"],
        "QUILANGA": ["QUILANGA", "FUNDOCHAMBA", "SAN ANTONIO DE LAS ARADAS (CAB. EN LAS ARADAS)"],
        "OLMEDO": ["OLMEDO", "LA TINGUE"],
        "BABAHOYO": ["CLEMENTE BAQUERIZO", "DR. CAMILO PONCE", "BARREIRO", "EL SALTO", "BABAHOYO", "BARREIRO (SANTA RITA)", "CARACOL", "FEBRES CORDERO (LAS JUNTAS)", "PIMOCHA", "LA UNIÓN"],
        "BABA": ["BABA", "GUARE", "ISLA DE BEJUCAL"],
        "MONTALVO": ["MONTALVO"],
        "PUEBLOVIEJO": ["PUEBLOVIEJO", "PUERTO PECHICHE", "SAN JUAN"],
        "QUEVEDO": ["QUEVEDO", "SAN CAMILO", "SAN JOSÉ", "GUAYACÁN", "NICOLÁS INFANTE DÍAZ", "SAN CRISTÓBAL", "SIETE DE OCTUBRE", "24 DE MAYO", "VENUS DEL RÍO QUEVEDO", "VIVA ALFARO", "BUENA FÉ", "MOCACHE", "SAN CARLOS", "VALENCIA", "LA ESPERANZA"],
        "URDANETA": ["CATARAMA", "RICAURTE"],
        "VENTANAS": ["10 DE NOVIEMBRE", "VENTANAS", "QUINSALOMA", "ZAPOTAL", "CHACARITA", "LOS ÁNGELES"],
        "VÍNCES": ["VINCES", "ANTONIO SOTOMAYOR (CAB. EN PLAYAS DE VINCES)", "PALENQUE"],
        "PALENQUE": ["PALENQUE"],
        "BUENA FÉ": ["SAN JACINTO DE BUENA FÉ", "7 DE AGOSTO", "11 DE OCTUBRE", "SAN JACINTO DE BUENA FÉ", "PATRICIA PILAR"],
        "VALENCIA": ["VALENCIA"],
        "MOCACHE": ["MOCACHE"],
        "QUINSALOMA": ["QUINSALOMA"],
        "PORTOVIEJO": ["PORTOVIEJO", "12 DE MARZO", "COLÓN", "PICOAZÁ", "SAN PABLO", "ANDRÉS DE VERA", "FRANCISCO PACHECO", "18 DE OCTUBRE", "SIMÓN BOLÍVAR", "ABDÓN CALDERÓN (SAN FRANCISCO)", "ALHAJUELA (BAJO GRANDE)", "CRUCITA", "PUEBLO NUEVO", "RIOCHICO (RÍO CHICO)", "SAN PLÁCIDO", "CHIRIJOS"],
        "CHONE": ["CHONE", "SANTA RITA", "BOYACÁ", "CANUTO", "CONVENTO", "CHIBUNGA", "ELOY ALFARO", "RICAURTE", "SAN ANTONIO"],
        "EL CARMEN": ["EL CARMEN", "4 DE DICIEMBRE", "EL CARMEN", "WILFRIDO LOOR MOREIRA (MAICITO)", "SAN PEDRO DE SUMA"],
        "FLAVIO ALFARO": ["FLAVIO ALFARO", "SAN FRANCISCO DE NOVILLO (CAB. EN ZAPALLO)"],
        "JIPIJAPA": ["DR. MIGUEL MORÁN LUCIO", "MANUEL INOCENCIO PARRALES Y GUALE", "SAN LORENZO DE JIPIJAPA", "JIPIJAPA", "AMÉRICA", "EL ANEGADO (CAB. EN ELOY ALFARO)", "JULCUY", "LA UNIÓN", "MACHALILLA", "MEMBRILLAL", "PEDRO PABLO GÓMEZ", "PUERTO DE CAYO", "PUERTO LÓPEZ"],
        "JUNÍN": ["JUNÍN"],
        "MANTA": ["LOS ESTEROS", "MANTA", "SAN MATEO", "TARQUI", "ELOY ALFARO", "MANTA", "SAN LORENZO", "SANTA MARIANITA (BOCA DE PACOCHE)"],
        "MONTECRISTI": ["ANIBAL SAN ANDRÉS", "MONTECRISTI", "EL COLORADO", "GENERAL ELOY ALFARO", "LEONIDAS PROAÑO", "MONTECRISTI", "JARAMIJÓ", "LA PILA"],
        "PAJÁN": ["PAJÁN", "CAMPOZANO (LA PALMA DE PAJÁN)", "CASCOL", "GUALE", "LASCANO"],
        "PICHINCHA": ["PICHINCHA", "BARRAGANETE", "SAN SEBASTIÁN"],
        "ROCAFUERTE": ["ROCAFUERTE"],
        "SANTA ANA": ["SANTA ANA", "LODANA", "SANTA ANA DE VUELTA LARGA", "AYACUCHO", "HONORATO VÁSQUEZ (CAB. EN VÁSQUEZ)", "LA UNIÓN", "OLMEDO", "SAN PABLO (CAB. EN PUEBLO NUEVO)"],
        "SUCRE": ["BAHÍA DE CARÁQUEZ", "LEONIDAS PLAZA GUTIÉRREZ", "BAHÍA DE CARÁQUEZ", "CANOA", "COJIMÍES", "CHARAPOTÓ", "10 DE AGOSTO", "JAMA", "PEDERNALES", "SAN ISIDRO", "SAN VICENTE"],
        "TOSAGUA": ["TOSAGUA", "BACHILLERO", "ANGEL PEDRO GILER (LA ESTANCILLA)"],
        "24 DE MAYO": ["SUCRE", "BELLAVISTA", "NOBOA", "ARQ. SIXTO DURÁN BALLÉN"],
        "PEDERNALES": ["PEDERNALES", "COJIMÍES", "10 DE AGOSTO", "ATAHUALPA"],
        "PUERTO LÓPEZ": ["PUERTO LÓPEZ", "MACHALILLA", "SALANGO"],
        "JAMA": ["JAMA"],
        "JARAMIJÓ": ["JARAMIJÓ"],
        "SAN VICENTE": ["SAN VICENTE", "CANOA"],
        "MORONA": ["MACAS", "ALSHI (CAB. EN 9 DE OCTUBRE)", "CHIGUAZA", "GENERAL PROAÑO", "HUASAGA (CAB.EN WAMPUIK)", "MACUMA", "SAN ISIDRO", "SEVILLA DON BOSCO", "SINAÍ", "TAISHA", "ZUÑA (ZÚÑAC)", "TUUTINENTZA", "CUCHAENTZA", "SAN JOSÉ DE MORONA", "RÍO BLANCO"],
        "GUALAQUIZA": ["GUALAQUIZA", "MERCEDES MOLINA", "GUALAQUIZA", "AMAZONAS (ROSARIO DE CUYES)", "BERMEJOS", "BOMBOIZA", "CHIGÜINDA", "EL ROSARIO", "NUEVA TARQUI", "SAN MIGUEL DE CUYES", "EL IDEAL"],
        "LIMÓN INDANZA": ["GENERAL LEONIDAS PLAZA GUTIÉRREZ (LIMÓN)", "INDANZA", "PAN DE AZÚCAR", "SAN ANTONIO (CAB. EN SAN ANTONIO CENTRO)", "SAN CARLOS DE LIMÓN (SAN CARLOS DEL", "SAN JUAN BOSCO", "SAN MIGUEL DE CONCHAY", "SANTA SUSANA DE CHIVIAZA (CAB. EN CHIVIAZA)", "YUNGANZA (CAB. EN EL ROSARIO)"],
        "PALORA": ["PALORA (METZERA)", "ARAPICOS", "CUMANDÁ (CAB. EN COLONIA AGRÍCOLA SEVILLA DEL ORO)", "HUAMBOYA", "SANGAY (CAB. EN NAYAMANACA)"],
        "SANTIAGO": ["SANTIAGO DE MÉNDEZ", "COPAL", "CHUPIANZA", "PATUCA", "SAN LUIS DE EL ACHO (CAB. EN EL ACHO)", "SANTIAGO", "TAYUZA", "SAN FRANCISCO DE CHINIMBIMI"],
        "SUCÚA": ["SUCÚA", "ASUNCIÓN", "HUAMBI", "LOGROÑO", "YAUPI", "SANTA MARIANITA DE JESÚS"],
        "HUAMBOYA": ["HUAMBOYA", "CHIGUAZA", "PABLO SEXTO"], "SAN JUAN BOSCO": ["SAN JUAN BOSCO", "PAN DE AZÚCAR", "SAN CARLOS DE LIMÓN", "SAN JACINTO DE WAKAMBEIS", "SANTIAGO DE PANANZA"],
        "TAISHA": ["TAISHA", "HUASAGA (CAB. EN WAMPUIK)", "MACUMA", "TUUTINENTZA", "PUMPUENTSA"],
        "LOGROÑO": ["LOGROÑO", "YAUPI", "SHIMPIS"],
        "PABLO SEXTO": ["PABLO SEXTO"],
        "TIWINTZA": ["SANTIAGO", "SAN JOSÉ DE MORONA"],
        "TENA": ["TENA", "AHUANO", "CARLOS JULIO AROSEMENA TOLA (ZATZA-YACU)", "CHONTAPUNTA", "PANO", "PUERTO MISAHUALLI", "PUERTO NAPO", "TÁLAG", "SAN JUAN DE MUYUNA"],
        "ARCHIDONA": ["ARCHIDONA", "AVILA", "COTUNDO", "LORETO", "SAN PABLO DE USHPAYACU", "PUERTO MURIALDO"],
        "EL CHACO": ["EL CHACO", "GONZALO DíAZ DE PINEDA (EL BOMBÓN)", "LINARES", "OYACACHI", "SANTA ROSA", "SARDINAS"],
        "QUIJOS": ["BAEZA", "COSANGA", "CUYUJA", "PAPALLACTA", "SAN FRANCISCO DE BORJA (VIRGILIO DÁVILA)", "SAN JOSÉ DEL PAYAMINO", "SUMACO"],
        "CARLOS JULIO AROSEMENA TOL": ["CARLOS JULIO AROSEMENA TOLA"],
        "PASTAZA": ["PUYO", "ARAJUNO", "CANELOS", "CURARAY", "DIEZ DE AGOSTO", "FÁTIMA", "MONTALVO (ANDOAS)", "POMONA", "RÍO CORRIENTES", "RÍO TIGRE", "SANTA CLARA", "SARAYACU", "SIMÓN BOLÍVAR (CAB. EN MUSHULLACTA)", "TARQUI", "TENIENTE HUGO ORTIZ", "VERACRUZ (INDILLAMA) (CAB. EN INDILLAMA)", "EL TRIUNFO"],
        "MERA": ["MERA", "MADRE TIERRA", "SHELL"], "SANTA CLARA": ["SANTA CLARA", "SAN JOSÉ"],
        "ARAJUNO": ["ARAJUNO", "CURARAY"],
        "QUITO": ["BELISARIO QUEVEDO", "CARCELÉN", "CENTRO HISTÓRICO", "COCHAPAMBA", "COMITÉ DEL PUEBLO", "COTOCOLLAO", "CHILIBULO", "CHILLOGALLO", "CHIMBACALLE", "EL CONDADO", "GUAMANÍ", "IÑAQUITO", "ITCHIMBÍA", "JIPIJAPA", "KENNEDY", "LA ARGELIA", "LA CONCEPCIÓN", "LA ECUATORIANA", "LA FERROVIARIA", "LA LIBERTAD", "LA MAGDALENA", "LA MENA", "MARISCAL SUCRE", "PONCEANO", "PUENGASÍ", "QUITUMBE", "RUMIPAMBA", "SAN BARTOLO", "SAN ISIDRO DEL INCA", "SAN JUAN", "SOLANDA", "TURUBAMBA", "QUITO DISTRITO METROPOLITANO", "ALANGASÍ", "AMAGUAÑA", "ATAHUALPA", "CALACALÍ", "CALDERÓN", "CONOCOTO", "CUMBAYÁ", "CHAVEZPAMBA", "CHECA", "EL QUINCHE", "GUALEA", "GUANGOPOLO", "GUAYLLABAMBA", "LA MERCED", "LLANO CHICO", "LLOA", "MINDO", "NANEGAL", "NANEGALITO", "NAYÓN", "NONO", "PACTO", "PEDRO VICENTE MALDONADO", "PERUCHO", "PIFO", "PÍNTAG", "POMASQUI", "PUÉLLARO", "PUEMBO", "SAN ANTONIO", "SAN JOSÉ DE MINAS", "SAN MIGUEL DE LOS BANCOS", "TABABELA", "TUMBACO", "YARUQUÍ", "ZAMBIZA", "PUERTO QUITO"],
        "CAYAMBE": ["AYORA", "CAYAMBE", "JUAN MONTALVO", "ASCÁZUBI", "CANGAHUA", "OLMEDO (PESILLO)", "OTÓN", "SANTA ROSA DE CUZUBAMBA"],
        "MEJIA": ["MACHACHI", "ALÓAG", "ALOASÍ", "CUTUGLAHUA", "EL CHAUPI", "MANUEL CORNEJO ASTORGA (TANDAPI)", "TAMBILLO", "UYUMBICHO"], "PEDRO MONCAYO": ["TABACUNDO", "LA ESPERANZA", "MALCHINGUÍ", "TOCACHI", "TUPIGACHI"],
        "RUMIÑAHUI": ["SANGOLQUÍ", "SAN PEDRO DE TABOADA", "SAN RAFAEL", "SANGOLQUI", "COTOGCHOA", "RUMIPAMBA"],
        "SAN MIGUEL DE LOS BANCOS": ["SAN MIGUEL DE LOS BANCOS", "MINDO", "PEDRO VICENTE MALDONADO", "PUERTO QUITO"],
        "PEDRO VICENTE MALDONADO": ["PEDRO VICENTE MALDONADO"],
        "PUERTO QUITO": ["PUERTO QUITO"],
        "AMBATO": ["ATOCHA – FICOA", "CELIANO MONGE", "HUACHI CHICO", "HUACHI LORETO", "LA MERCED", "LA PENÍNSULA", "MATRIZ", "PISHILATA", "SAN FRANCISCO", "AMBATO", "AMBATILLO", "ATAHUALPA (CHISALATA)", "AUGUSTO N. MARTÍNEZ (MUNDUGLEO)", "CONSTANTINO FERNÁNDEZ (CAB. EN CULLITAHUA)", "HUACHI GRANDE", "IZAMBA", "JUAN BENIGNO VELA", "MONTALVO", "PASA", "PICAIGUA", "PILAGÜÍN (PILAHÜÍN)", "QUISAPINCHA (QUIZAPINCHA)", "SAN BARTOLOMÉ DE PINLLOG", "SAN FERNANDO (PASA SAN FERNANDO)", "SANTA ROSA", "TOTORAS", "CUNCHIBAMBA", "UNAMUNCHO"],
        "BAÑOS DE AGUA SANTA": ["BAÑOS DE AGUA SANTA", "LLIGUA", "RÍO NEGRO", "RÍO VERDE", "ULBA"],
        "CEVALLOS": ["CEVALLOS"],
        "MOCHA": ["MOCHA", "PINGUILÍ"],
        "PATATE": ["PATATE", "EL TRIUNFO", "LOS ANDES (CAB. EN POATUG)", "SUCRE (CAB. EN SUCRE-PATATE URCU)"],
        "QUERO": ["QUERO", "RUMIPAMBA", "YANAYACU - MOCHAPATA (CAB. EN YANAYACU)"], "SAN PEDRO DE PELILEO": ["PELILEO", "PELILEO GRANDE", "PELILEO", "BENÍTEZ (PACHANLICA)", "BOLÍVAR", "COTALÓ", "CHIQUICHA (CAB. EN CHIQUICHA GRANDE)", "EL ROSARIO (RUMICHACA)", "GARCÍA MORENO (CHUMAQUI)", "GUAMBALÓ (HUAMBALÓ)", "SALASACA"],
        "SANTIAGO DE PÍLLARO": ["CIUDAD NUEVA", "PÍLLARO", "PÍLLARO", "BAQUERIZO MORENO", "EMILIO MARÍA TERÁN (RUMIPAMBA)", "MARCOS ESPINEL (CHACATA)", "PRESIDENTE URBINA (CHAGRAPAMBA -PATZUCUL)", "SAN ANDRÉS", "SAN JOSÉ DE POALÓ", "SAN MIGUELITO"],
        "TISALEO": ["TISALEO", "QUINCHICOTO"],
        "ZAMORA": ["EL LIMÓN", "ZAMORA", "CUMBARATZA", "GUADALUPE", "IMBANA (LA VICTORIA DE IMBANA)", "PAQUISHA", "SABANILLA", "TIMBARA", "ZUMBI", "SAN CARLOS DE LAS MINAS"],
        "CHINCHIPE": ["ZUMBA", "CHITO", "EL CHORRO", "EL PORVENIR DEL CARMEN", "LA CHONTA", "PALANDA", "PUCAPAMBA", "SAN FRANCISCO DEL VERGEL", "VALLADOLID", "SAN ANDRÉS"],
        "NANGARITZA": ["GUAYZIMI", "ZURMI", "NUEVO PARAÍSO"],
        "YACUAMBI": ["28 DE MAYO (SAN JOSÉ DE YACUAMBI)", "LA PAZ", "TUTUPALI"], "YANTZAZA (YANZATZA)": ["YANTZAZA (YANZATZA)", "CHICAÑA", "EL PANGUI", "LOS ENCUENTROS"], "EL PANGUI": ["EL PANGUI", "EL GUISME", "PACHICUTZA", "TUNDAYME"], "CENTINELA DEL CÓNDOR": ["ZUMBI", "PAQUISHA", "TRIUNFO-DORADO", "PANGUINTZA"],
        "PALANDA": ["PALANDA", "EL PORVENIR DEL CARMEN", "SAN FRANCISCO DEL VERGEL", "VALLADOLID", "LA CANELA"],
        "PAQUISHA": ["PAQUISHA", "BELLAVISTA", "NUEVO QUITO"], "SAN CRISTÓBAL": ["PUERTO BAQUERIZO MORENO", "EL PROGRESO", "A SANTA MARÍA (FLOREANA) (CAB. EN PTO. VELASCO IBARR"],
        "ISABELA": ["PUERTO VILLAMIL", "TOMÁS DE BERLANGA (SANTO TOMÁS)"], "SANTA CRUZ": ["PUERTO AYORA", "BELLAVISTA", "SANTA ROSA (INCLUYE LA ISLA BALTRA)"], "LAGO AGRIO": ["NUEVA LOJA", "CUYABENO", "DURENO", "GENERAL FARFÁN", "TARAPOA", "EL ENO", "PACAYACU", "JAMBELÍ", "SANTA CECILIA", "AGUAS NEGRAS"],
        "GONZALO PIZARRO": ["EL DORADO DE CASCALES", "EL REVENTADOR", "GONZALO PIZARRO", "LUMBAQUÍ", "PUERTO LIBRE", "SANTA ROSA DE SUCUMBÍOS"],
        "PUTUMAYO": ["PUERTO EL CARMEN DEL PUTUMAYO", "PALMA ROJA", "PUERTO BOLÍVAR (PUERTO MONTÚFAR)", "PUERTO RODRÍGUEZ", "SANTA ELENA"],
        "SHUSHUFINDI": ["SHUSHUFINDI", "LIMONCOCHA", "PAÑACOCHA", "SAN ROQUE (CAB. EN SAN VICENTE)", "SAN PEDRO DE LOS COFANES", "SIETE DE JULIO"],
        "SUCUMBÍOS": ["LA BONITA", "EL PLAYÓN DE SAN FRANCISCO", "LA SOFÍA", "ROSA FLORIDA", "SANTA BÁRBARA"],
        "CASCALES": ["EL DORADO DE CASCALES", "SANTA ROSA DE SUCUMBÍOS", "SEVILLA"],
        "CUYABENO": ["TARAPOA", "CUYABENO", "AGUAS NEGRAS"],
        "ORELLANA": ["PUERTO FRANCISCO DE ORELLANA (EL COCA)", "DAYUMA", "TARACOA (NUEVA ESPERANZA: YUCA)", "ALEJANDRO LABAKA", "EL DORADO", "EL EDÉN", "GARCÍA MORENO", "INÉS ARANGO (CAB. EN WESTERN)", "LA BELLEZA", "NUEVO PARAÍSO (CAB. EN UNIÓN", "SAN JOSÉ DE GUAYUSA", "SAN LUIS DE ARMENIA"],
        "AGUARICO": ["TIPITINI", "NUEVO ROCAFUERTE", "CAPITÁN AUGUSTO RIVADENEYRA", "CONONACO", "SANTA MARÍA DE HUIRIRIMA", "TIPUTINI", "YASUNÍ"],
        "LA JOYA DE LOS SACHAS": ["LA JOYA DE LOS SACHAS", "ENOKANQUI", "POMPEYA", "SAN CARLOS", "SAN SEBASTIÁN DEL COCA", "LAGO SAN PEDRO", "RUMIPAMBA", "TRES DE NOVIEMBRE", "UNIÓN MILAGREÑA"],
        "LORETO": ["LORETO", "AVILA (CAB. EN HUIRUNO)", "PUERTO MURIALDO", "SAN JOSÉ DE PAYAMINO", "SAN JOSÉ DE DAHUANO", "SAN VICENTE DE HUATICOCHA"],
        "SANTO DOMINGO": ["ABRAHAM CALAZACÓN", "BOMBOLÍ", "CHIGUILPE", "RÍO TOACHI", "RÍO VERDE", "SANTO DOMINGO DE LOS COLORADOS", "ZARACAY", "SANTO DOMINGO DE LOS COLORADOS", "ALLURIQUÍN", "PUERTO LIMÓN", "LUZ DE AMÉRICA", "SAN JACINTO DEL BÚA", "VALLE HERMOSO", "EL ESFUERZO", "SANTA MARÍA DEL TOACHI"],
        "SANTA ELENA": ["BALLENITA", "SANTA ELENA", "SANTA ELENA", "ATAHUALPA", "COLONCHE", "CHANDUY", "MANGLARALTO", "SIMÓN BOLÍVAR (JULIO MORENO)", "SAN JOSÉ DE ANCÓN"],
        "LA LIBERTAD": ["LA LIBERTAD"],
        "SALINAS": ["CARLOS ESPINOZA LARREA", "GRAL. ALBERTO ENRÍQUEZ GALLO", "VICENTE ROCAFUERTE", "SANTA ROSA", "SALINAS", "ANCONCITO", "JOSÉ LUIS TAMAYO (MUEY)"],
        "LAS GOLONDRINAS": ["LAS GOLONDRINAS"],
        "MANGA DEL CURA": ["MANGA DEL CURA"],
        "EL PIEDRERO": ["EL PIEDRERO"],
        "SAN VICENTE": ['SAN VICENTE', 'CANOA'],
        "SAN JUAN BOSCO": ['SAN JUAN BOSCO', 'PAN DE AZÚCAR', 'SAN CARLOS DE LIMÓN', 'SAN JACINTO DE WAKAMBEIS', 'SANTIAGO DE PANANZA'],
        "SANTA CLARA": ['SANTA CLARA', 'SAN JOSÉ'],
        "PEDRO MONCAYO": ['TABACUNDO', 'LA ESPERANZA', 'MALCHINGUÍ', 'TOCACHI', 'TUPIGACHI'],
        "SAN PEDRO DE PELILEO": ['PELILEO', 'PELILEO GRANDE', 'PELILEO', 'BENÍTEZ (PACHANLICA)', 'BOLÍVAR', 'COTALÓ', 'CHIQUICHA (CAB. EN CHIQUICHA GRANDE)', 'EL ROSARIO (RUMICHACA)', 'GARCÍA MORENO (CHUMAQUI)', 'GUAMBALÓ (HUAMBALÓ)', 'SALASACA'],
        "YANTZAZA ": ['YANTZAZA (YANZATZA)', 'CHICAÑA', 'EL PANGUI', 'LOS ENCUENTROS'],
        "EL PANGUI": ['EL PANGUI', 'EL GUISME', 'PACHICUTZA', 'TUNDAYME'],
        "CENTINELA DEL CÓNDOR": ['ZUMBI', 'PAQUISHA', 'TRIUNFO-DORADO', 'PANGUINTZA'],
        "SAN CRISTÓBAL": ['PUERTO BAQUERIZO MORENO', 'EL PROGRESO', 'A SANTA MARÍA (FLOREANA) (CAB. EN PTO. VELASCO IBARR'],
        "SANTA CRUZ": ['PUERTO AYORA', 'BELLAVISTA', 'SANTA ROSA (INCLUYE LA ISLA BALTRA)'],
        "LAGO AGRIO": ['NUEVA LOJA', 'CUYABENO', 'DURENO', 'GENERAL FARFÁN', 'TARAPOA', 'EL ENO', 'PACAYACU', 'JAMBELÍ', 'SANTA CECILIA', 'AGUAS NEGRAS']


    };
   
    // Función para cargar las provincias según el cantón seleccionado
    function cargarProvincias() {
        let cantonSelect = document.getElementById("canton");
        let provinciaSelect = document.getElementById("sector");
        let canton = cantonSelect.value;

        // Limpia el selector de provincias
        provinciaSelect.innerHTML = "<option value=''>Seleccione una sector</option>";

        // Verifica si el cantón seleccionado existe en el objeto cantonesYProvincias
        if (canton in cantonesYProvincias) {
            let provincias = cantonesYProvincias[canton];
            // Crea y agrega las opciones de provincias según el cantón seleccionado
            provincias.forEach(function (provincia) {
                let option = document.createElement("option");
                option.value = provincia.toLowerCase();
                option.textContent = provincia;
                provinciaSelect.appendChild(option);
            });
        }
    }

 


    //obtener valor cambiado del select
    $("#canton").change(function () {
        cargarProvincias();

    });

       // Agregar evento click al selectHtml para que cargue las provincias al hacer clic
    $("#sector").dblclick(function () {
        cargarProvincias();
        console.log('Doble clic');
    });
 
  

    ///tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })


});


   
    //tarjeta previamente seleccionada
  
if ($("#estado1").length) {
    var estado1 = $('#estado1').data('estado');
    if (estado1 == 'activo') {
        $("#radio1").prop("checked", true);
      
        $('#comtarjeta1').prop('value', 'Guardar cambios');
        $('#comtarjeta2').prop('value', '');
        $('#comtarjeta3').prop('value', '');
    }

} else {
    var parrafo = $('#textpago').text('Actualmente no cuenta con un metodo de pago agregado, por favor agregue metodo de pago nuevo.');}
    if ($("#estado2").length) {
        var estado2 = $('#estado2').data('estado');
        if (estado2 == 'activo') {
            $("#radio2").prop("checked", true);
        
            $('#comtarjeta2').prop('value', 'Guardar cambios');
            $('#comtarjeta1').prop('value', '');
            $('#comtarjeta3').prop('value', '');
        }

    }
    if ($("#estado3").length) {
        var estado3 = $('#estado3').data('estado');
        if (estado3 == 'activo') {
            $("#radio3").prop("checked", true);
        
        }

    }
   
 
    
//tarjeta
    const validar_radio = (e) => {
    switch (e.target.id) {
        case "radio1":
        
            $('#elegir2').html("");
            $('#elegir3').html("");
            $('#comtarjeta1').prop('value', 'Guardar cambios');
            $('#comtarjeta2').prop('value', '');
            $('#comtarjeta3').prop('value', '');
            break;
        case "radio2":
            $('#elegir1').html("");
            $('#elegir3').html("");
     
            $('#comtarjeta2').prop('value', 'Guardar cambios');
            $('#comtarjeta1').prop('value', '');
            $('#comtarjeta3').prop('value', '');
            break;
        case "radio3":
            $('#elegir1').html("");
            $('#elegir2').html("");
       
            $('#comtarjeta1').prop('value', '');
            $('#comtarjeta2').prop('value', '');
            $('#comtarjeta3').prop('value', 'Guardar cambios');
            break;
        default:
            console.log('Error,tarjeta no seleccionada. Consulte con el administrador');
            break;

    }
}

const radios = document.querySelectorAll("#form input");

   
radios.forEach((radio) => {
    radio.addEventListener('click', validar_radio)
   
});

/* Map para asp net user index */
if (pathname === '/AspNetUsers/index') {

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
/*Fin estado cita*/
// Verifiicar aceptados no estan cumpliendo las reglas 
let x = document.querySelectorAll("#estadocita");

for (let i = 0; i < x.length; i++) {

    let estado = x[i].getAttribute('data-estadocita').toUpperCase();
   
    if (estado == 'ACEPTADO' ) {
        x[i].classList.add('text-success');
        x[i].classList.remove('text-warning');
        x[i].classList.remove('text-danger');
    } if (estado == 'PENDIENTE') {
        x[i].classList.add('text-warning');
        x[i].classList.remove('text-success');
        x[i].classList.remove('text-danger');
    } if (estado == 'CANCELADO') {
        x[i].classList.add('text-danger');
        x[i].classList.remove('text-success');
        x[i].classList.remove('text-warning');
    } if (estado == 'COMPLETADO') {
        x[i].classList.add('text-success');
        x[i].classList.remove('text-warning');
        x[i].classList.remove('text-danger');
       
    }

    
  
}
/*Fin estado cita*/

// botones dependiento estado






/* Tendecias js*/

if (pathname =='/Home/Tendencias' ) {

    $(document).ready(function () {
        // Toggle de informacion max o menos segun el click 

        $(".toggle").click(function () {
            $(this).text(function (i, text) {
                console.log('funciona' + this);
                return text === "+ Información" ? "- Información" : "+ Información";

            })
        });


        // textos saber mas tendencias.
        let tendenciasText = document.querySelectorAll('.card-text-trend');

        for (i = 0; i < tendenciasText.length; i++) {
            let readtext = tendenciasText[i].textContent.substr(0, 76) + '...'

            tendenciasText[i].innerHTML = readtext;

        }
      
        // Mostrar Lightbox al hacer clic en una imagen
        $(".lightbox-link").on("click", function (e) {
            e.preventDefault();
            let imageUrl = $(this).attr("href");
            let detailsUrl = $(this).siblings(".lightbox-details").attr("href");
            $("#lightbox img").attr("src", imageUrl);
            let completUrl = '/SERVICIOS/Details/' + detailsUrl
            $("#lightbox a").attr("href", completUrl);
            $("#lightbox").fadeIn();
        });

        // Obtener el elemento de cierre del Lightbox
        var lightboxClose = $('.lightbox-close');


        $(".lightbox-close").on("click", function (event) {
            event.preventDefault();
            $(".lightbox-tendencia").fadeOut();
        });

        
 

    });
 

}

