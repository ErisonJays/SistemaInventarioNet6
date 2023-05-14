using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class //<T> where T : class --> vuelve la interfaz generica para que reciba todo tipo de objetos que le enviemos.
    {
        Task<T> Obtener(int id);  // Task vuelve el metodo asincrono

        Task<IEnumerable<T>> ObtenerTodos(
             Expression<Func<T, bool>> filtro = null,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
             string incluirPropiedades = null,
             bool isTracking = true
             );

        Task<T> ObtenerPrimero(
             Expression<Func<T, bool>> filtro = null,
             string incluirPropiedades = null,
             bool isTracking = true
             );

        Task Agregar(T entidad);

        void Remover(T entidad);

        void RemoverRango(IEnumerable<T> entidad);
    }
}
