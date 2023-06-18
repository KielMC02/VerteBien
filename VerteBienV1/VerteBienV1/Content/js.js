

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
        });
    }


    $('#st').starrr({

        rating: 3,
        change: function (e, valor) {

            alert(valor);

        }

    });


});

/* Pequeño query para poner el nombre del archivo que se sube*/
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

function validarImagen(input) {
    var file = input.files[0];
    var maxSize = 8 * 1024 * 1024; // 8MB en bytes
    var minWidth = 540;
    var minHeight = 540;
    var maxWidth = 1080;
    var maxHeight = 1080;

    // Validar el tamaño del archivo
    if (file.size > maxSize) {
        $('#modalFileSize').modal('show');
        input.value = "";
        return;
    }

    // Crear una nueva imagen para obtener sus dimensiones
    var img = new Image();
    img.onload = function () {
        // Validar la resolución mínima y máxima de la imagen
        if (img.width < minWidth || img.height < minHeight) {
            $('#modalFileMin').modal('show');
            input.value = "";
        } else if (img.width > maxWidth || img.height > maxHeight) {
            $('#modalFileMax').modal('show');
            input.value = "";
        } else {
            // Mostrar el nombre del archivo seleccionado si pasa la validación
            var filename = input.value.split('\\').pop();
            var idname = input.id;
            var res = filename.substring(0, 15);
            var fname = res + "...";
            jQuery('span.' + idname).next().find('span').html(fname);

            if (pathname == '/SERVICIOS/Create') {
                $('span.' + idname).html(fname);
            }
        }
    };
    img.src = URL.createObjectURL(file);
}

// FIN Función para validar el tamaño y la resolución de las imágenes


//// Función para validar el tamaño y la resolución de las imágenes
//function validarImagen(input) {
//    var file = input.files[0];
//    var maxSize = 8 * 1024 * 1024; // 8MB en bytes
//    var minWidth = 540;
//    var minHeight = 540;

//    // Validar el tamaño del archivo
//    if (file.size > maxSize) {
//        $('#modalFileSize').modal('show'); // abrirr el modal de boostrap
//        /*    alert("El tamaño máximo permitido para la imagen es de 8MB.");*/
//        input.value = "";
//        return;
//    }

//    // Crear una nueva imagen para obtener sus dimensiones
//    var img = new Image();
//    img.onload = function () {
//        // Validar la resolución mínima de la imagen
//        if (img.width < minWidth || img.height < minHeight) {
//            $('#modalFile').modal('show'); // abrirr el modal de boostrap
//            /*    alert("La resolución mínima permitida para la imagen es de 540x540 píxeles.");*/
//            input.value = "";
//        } else {
//            // Mostrar el nombre del archivo seleccionado si pasa la validación
//            var filename = input.value.split('\\').pop();
//            var idname = input.id;
//            var res = filename.substring(0, 15);
//            var fname = res + "...";
//            jQuery('span.' + idname).next().find('span').html(fname);
//            /*console.log(fname)*/
//            if (pathname == '/SERVICIOS/Create') {
//                $('span.' + idname).html(fname);
//                /* console.log(fname)*/
//            }
//        }
//    };
//    img.src = URL.createObjectURL(file);
//}

////// FIN Función para validar el tamaño y la resolución de las imágenes

//// Asociar la función de validación al evento onchange de los elementos input[type=file]
//jQuery('input[type=file]').change(function (event) {
//    validarImagen(this);
//});






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

            document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-errorr`).classList.remove('formulario__input-error-activo');

        } else {
            document.querySelector(`#grupo__fecha_nacimiento_ .formulario__input-errorr`).classList.add('formulario__input-error-activo');
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







///* JS para los modal */
//var myModal = document.getElementById('exampleModal')
//var myInput = document.getElementById('btn55')

//myModal.addEventListener('shown.bs.modal', function () {
//myInput.focus()
//})



/*pequeño Js para enseñar las estrellas este es para la parte de  para mostrar edicion*/
/* var rating = document.getElementsByClassName("str");

 function camstr(strval) {
     srt = strval;
     rating: rating[srt].getAttribute("data-rating")
 }
 /*

 /*pequeño Js para enseñar las estrellas este es para ediar las estrellas */

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



















