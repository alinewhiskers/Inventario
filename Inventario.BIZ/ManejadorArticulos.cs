using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario.BIZ
{
    public class ManejadorArticulos : IManejadorArticulos
    {
        IRepositorio<Articulo> repositorio;
        public ManejadorArticulos(IRepositorio<Articulo> repositorio)
        {
            this.repositorio = repositorio;
        }
        public List<Articulo> Listar => repositorio.Leer;

        public bool Agregar(Articulo entidad)
        {
            return repositorio.Crear(entidad);
        }

        public Articulo BuscarPorId(string id)
        {
            return Listar.Where(e => e.Id == id).SingleOrDefault();
        }

        public bool Eliminar(string id)
        {
            return repositorio.Eliminar(id);
        }

        public List<Articulo> ArticulosPorArea(string area)
        {
            throw new NotImplementedException();
        }

        public bool Modificar(Articulo entidad)
        {
            return repositorio.Editar(entidad);
        }
    }
}
