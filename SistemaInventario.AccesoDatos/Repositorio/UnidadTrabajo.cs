﻿using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioNet6.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;

        public IBodegaRepositorio Bodega { get; private set; }

        public ICategoriaRepositorio Categoria { get; private set; }

        public IMarcaRepositorio Marca { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Bodega = new BodegaRepositorio(_db);
            Categoria = new CategoriaRepositorio(_db);
            Marca = new MarcaRepositorio(_db);

        }

        
        public void Dispose()
        {
            _db.Dispose(); //Libera lo que este en memoria
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}
