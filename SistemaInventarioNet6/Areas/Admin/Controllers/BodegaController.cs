using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;
using System.Globalization;

namespace SistemaInventarioNet6.Areas.Admin.Controllers
{
    [Area("Admin")] //se debe agregar a que area pertenece para que la vista sea llamada correctamente
    public class BodegaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public BodegaController(IUnidadTrabajo unidadTrabajo) //constructor
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id) // el signo de ? indica que puede contener un valor o ser nulo
        {
            Bodega bodega = new Bodega();

            if(id == null)
            {
                //crear nueva bodega
                bodega.Estado = true;
                return View(bodega);
            }

            //Actualizar bodega
            bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());

            if (bodega == null)
            {
                return NotFound();
            }
            return View(bodega);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]// evita la falsificacion de solicitudes de sitios no autorizados

        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if(bodega.Id == 0)
                {
                    await _unidadTrabajo.Bodega.Agregar(bodega);
                    TempData[DS.Exitosa] = "Bodega Creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Bodega.Actualizar(bodega);
                    TempData[DS.Exitosa] = "Bodega Actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof (Index));
            }
            TempData[DS.Error] = "Error al Grabar Bodega";
            return View(bodega);
        }

        #region API
        [HttpGet]
        public async Task <IActionResult> ObtenerTodos()
        {
            
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new {data = todos}); // data es el nombre que se le va a refenciar el JavaScript
        }
        

        [HttpPost]
        public async Task<IActionResult>Delete(int id)
        {
            var bodegaDb = await _unidadTrabajo.Bodega.Obtener(id);
            if(bodegaDb == null)
            {
                return Json(new {success = false, message = "Error al borrar bodega"});
            }
            _unidadTrabajo.Bodega.Remover(bodegaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Bodega borrada exitosamente"});
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Bodega.ObtenerTodos();

            if (id == 0) 
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else 
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }
            if(valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }

        #endregion
    }
}
