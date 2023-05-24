using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;
using System.Globalization;

namespace SistemaInventarioNet6.Areas.Admin.Controllers
{
    [Area("Admin")] //se debe agregar a que area pertenece para que la vista sea llamada correctamente
    public class CategoriaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CategoriaController(IUnidadTrabajo unidadTrabajo) //constructor
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id) // el signo de ? indica que puede contener un valor o ser nulo
        {
            Categoria categoria = new Categoria();

            if(id == null)
            {
                //crear nueva categoria
                categoria.Estado = true;
                return View(categoria);
            }

            //Actualizar categoria
            categoria = await _unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());

            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]// evita la falsificacion de solicitudes de sitios no autorizados

        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                if(categoria.Id == 0)
                {
                    await _unidadTrabajo.Categoria.Agregar(categoria);
                    TempData[DS.Exitosa] = "Categoria Creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Categoria.Actualizar(categoria);
                    TempData[DS.Exitosa] = "Categoria Actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof (Index));
            }
            TempData[DS.Error] = "Error al Grabar Categoria";
            return View(categoria);
        }

        #region API
        [HttpGet]
        public async Task <IActionResult> ObtenerTodos()
        {
            
            var todos = await _unidadTrabajo.Categoria.ObtenerTodos();
            return Json(new {data = todos}); // data es el nombre que se le va a refenciar el JavaScript
        }
        

        [HttpPost]
        public async Task<IActionResult>Delete(int id)
        {
            var categoriaDb = await _unidadTrabajo.Categoria.Obtener(id);
            if(categoriaDb == null)
            {
                return Json(new {success = false, message = "Error al borrar categoria"});
            }
            _unidadTrabajo.Categoria.Remover(categoriaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "categoria borrada exitosamente"});
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Categoria.ObtenerTodos();

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
