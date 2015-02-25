using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class TipoDeNovedades
    {
        int idtipodenovedades;

        public int Idtipodenovedades
        {
            get { return idtipodenovedades; }
            set { idtipodenovedades = value; }
        }
        int idtipodenovedad;

        public int Idtipodenovedad
        {
            get { return idtipodenovedad; }
            set { idtipodenovedad = value; }
        }
        string detalle;

        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }
        public TipoDeNovedades(int i, int id, string d)
        {
            idtipodenovedades = i;
            idtipodenovedad = id;
            detalle = d;
        }
    }
}
