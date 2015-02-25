using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class Novedades
    {
        int idnovedades;

        public int Idnovedades
        {
            get { return idnovedades; }
            set { idnovedades = value; }
        }
        Empleados empleado;

        public Empleados Empleado
        {
            get { return empleado; }
            set { empleado = value; }
        }
        string desde;

        public string Desde
        {
            get { return desde; }
            set { desde = value; }
        }
        string hasta;

        public string Hasta
        {
            get { return hasta; }
            set { hasta = value; }
        }
        int idtipodenovedades;

        public int Idtipodenovedades
        {
            get { return idtipodenovedades; }
            set { idtipodenovedades = value; }
        }
        string detalle;

        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }

        public Novedades(int i, Empleados e, string d, string h, int idt, string det)
        {
            idnovedades = i;
            empleado = e;
            desde = d;
            hasta = h;
            idtipodenovedades = idt;
            detalle = det;
        }
    }
}
