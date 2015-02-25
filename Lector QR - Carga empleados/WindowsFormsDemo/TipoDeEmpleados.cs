using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class TipoDeEmpleados
    {
        int idtipodeempleados;

        public int Idtipodeempleados
        {
            get { return idtipodeempleados; }
            set { idtipodeempleados = value; }
        }
        string detalle;

        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }

        public TipoDeEmpleados(int i, string d)
        {
            idtipodeempleados = i;
            detalle = d;
        }
    }
}
