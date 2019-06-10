using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Inventario.DAL
{
    public class RepositorioDeArticulos : IRepositorio<Articulo>
    {
        private string DBName = "Inventario.db";
        private string TableName = "Articulos";

        public List<Articulo> Leer
        {
            get
            {
                List<Articulo> datos = new List<Articulo>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Articulo>(TableName).FindAll().ToList();
                }
                return datos;
            }
        }

        public bool Crear(Articulo entidad)
        {
            entidad.Id = Guid.NewGuid().ToString();
            entidad.PrecioTotal = entidad.Precio * 1.16;
            try
            {
                using (var db=new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Articulo>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool Editar(Articulo entidadModificada)
        {
            entidadModificada.PrecioTotal = entidadModificada.Precio * 1.16;
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Articulo>(TableName);
                    coleccion.Update(entidadModificada);
                }
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Eliminar(string id)
        {
            try
            {
                int r;
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Articulo>(TableName);
                    r=coleccion.Delete(e=>e.Id ==id);
                }
                return r>0;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
