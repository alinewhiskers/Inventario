using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Articulo: Base
    {
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string Pedido { get; set; }
        public double Precio{ get; set; }
        public string Unidad { get; set; }
        public string Tamanio { get; set; }


    }
}
