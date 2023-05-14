using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioNet6.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class // hedera de IRepositorio
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext db)  //constructor
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        //Agregamos async a cada metodo para convertido en un metodo asincrono
        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad); // equivalente a : insert into Table
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id); // equivalente a :  select * from  (solo por Id)
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro); // equivalente a : select * from where ...
            }

            if (incluirPropiedades != null)
            {
                foreach (var item in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) // separar por ',' y eleminar los espacios en blanco
                {
                    query = query.Include(item); // ej "Categoria, Marca", traera tambien los objetos relacionados

                }

            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking(); // para no darle seguimiento a los objetos recuperado // mejora el rendimiento
            }

            return await query.ToListAsync();
        }
        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null,
            string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro); // equivalente a : select * from where ...
            }

            if (incluirPropiedades != null)
            {
                foreach (var item in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) // separar por ',' y eleminar los espacios en blanco
                {
                    query = query.Include(item); // ej "Categoria, Marca", traera tambien los objetos relacionados

                }

            }

            if (!isTracking)
            {
                query = query.AsNoTracking(); // para no darle seguimiento a los objetos recuperado // mejora el rendimiento
            }

            return await query.FirstOrDefaultAsync();

        }


        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }

        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);
        }
    }
}
