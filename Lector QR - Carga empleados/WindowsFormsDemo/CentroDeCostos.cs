using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class CentroDeCostos
    {
        int idcentrodecostros;

        public int Idcentrodecostros
        {
            get { return idcentrodecostros; }
            set { idcentrodecostros = value; }
        }
        string detalle;

        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }
        public CentroDeCostos(int i, string d)
        {
            idcentrodecostros = i;
            detalle = d;
        }
    }
}
