
//este js sera llamada desde Index

let datatable;


$(document).ready(function () { //saber cuando la lista esta cargado para ejecutar la funcion
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": { //Cambiar el idioma de la tabla predeterminada
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"

            }
        },

        "ajax": {
            "url": "/Admin/Marca/ObtenerTodos" //nombre del area/controlador/metodo
        },
        "columns": [
            { "data": "nombre", "width": "20%" },
            { "data": "descripcion", "width": "40%" },
            {
                "data": "estado",
                "render": function (data) //determinar si el estado es true o false para mostrar activo e inactivo
                {
                    if (data == true) {
                        return "Activo"
                    }
                    else {
                        return "Inactivo"
                    }
                }, "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {  //codigo html ``
                    return ` <div class = "text-center">     
                             <a href="/Admin/Marca/Upsert/${data}" class="btn btn-success" text-white style="cursor:pointer">  
                             <i class="bi bi-pencil-square"></i>
                              </a>

                             <a onclick = Delete("/Admin/Marca/Delete/${data}") class="btn btn-danger text-white style="cursor:pointer">
                               <i class="bi bi-trash3-fill"></i>
                             </a>

                           </div> `;
                }, "width" : "20%"
            }
        ]

    });
}

function Delete(url) {
    //SweetAlert
    swal({
        title: "Esta seguro de eliminar la marca?",
        text: "Este registro no podra ser recuperado",
        icon: "warning",
        buttons: true,
        dangerMode: true

    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message); //libreria toastr
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}