using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Material:Base
    {
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
    }
}
