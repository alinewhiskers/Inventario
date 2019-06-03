using System;
using System.Collections.Generic;
using System.Text;

namespace Inventario.COMMON.Entidades
{
    public class Vale:Base
    {
        public DateTime FechaHoraSolicitud { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime? FechaEntregaReal { get; set; }
        public List<Material> MaterialesPrestados { get; set; }
        public Empleado Solicitante { get; set; }
        public Empleado EncargadoDeAlmacen { get; set; }
    }
}
