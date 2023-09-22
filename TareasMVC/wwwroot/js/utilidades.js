async function manejarErrorApi(respuesta) {
    let mensajeError = '';

    if (respuesta.status === 400) {
        mensajeError = await respuesta.text();
    } else if (respuesta.status === 404) {
        mensajeError = "El recurso no ah sido encontrado";
    } else {
        mensajeError = "Ah ocurrido un error en su solicitud";
    }

    mostrarMensajeError(mensajeError);
}



function mostrarMensajeError(mensaje) {
    Swal.fire({
        icon: 'error',
        title: 'Error...',
        text: mensaje
    });
}