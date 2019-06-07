using Inventario.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Interfaces
{
    public interface IManejadorArticulos:IManejadorGenerico<Articulo>
    {
        List<Articulo> ArticulosPorArea(string area);
    }
}
