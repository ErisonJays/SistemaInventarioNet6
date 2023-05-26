using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;
using System.Globalization;

namespace SistemaInventarioNet6.Areas.Admin.Controllers
{
    [Area("Admin")] //se debe agregar a que area pertenece para que la vista sea llamada correctamente
    public class MarcaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public MarcaController (IUnidadTrabajo unidadTrabajo) //constructor
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id) // el signo de ? indica que puede contener un valor o ser nulo
        {
            Marca marca = new Marca();

            if(id == null)
            {
                //crear nueva marca
                marca.Estado = true;
                return View(marca);
            }

            //Actualizar marca
            marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());

            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]// evita la falsificacion de solicitudes de sitios no autorizados

        public async Task<IActionResult> Upsert(Marca marca)
        {
            if (ModelState.IsValid)
            {
                if(marca.Id == 0)
                {
                    await _unidadTrabajo.Marca.Agregar(marca);
                    TempData[DS.Exitosa] = "Marca Creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Marca.Actualizar(marca);
                    TempData[DS.Exitosa] = "Marca Actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof (Index));
            }
            TempData[DS.Error] = "Error al Grabar Marca";
            return View(marca);
        }

        #region API
        [HttpGet]
        public async Task <IActionResult> ObtenerTodos()
        {
            
            var todos = await _unidadTrabajo.Marca.ObtenerTodos();
            return Json(new {data = todos}); // data es el nombre que se le va a refenciar el JavaScript
        }
        

        [HttpPost]
        public async Task<IActionResult>Delete(int id)
        {
            var marcaDb = await _unidadTrabajo.Marca.Obtener(id);
            if(marcaDb == null)
            {
                return Json(new {success = false, message = "Error al borrar marca"});
            }
            _unidadTrabajo.Marca.Remover(marcaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca borrada exitosamente"});
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Marca.ObtenerTodos();

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
