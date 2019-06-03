using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Interfaces
{
    public interface IRepositorio <T> where T: Entidades.Base
    {
        bool Crear(T entidad);
        bool Editar(string id, T entidadModificada);
        bool Eliminar(T entidad);
        List<T> Leer { get; }
    }
}
