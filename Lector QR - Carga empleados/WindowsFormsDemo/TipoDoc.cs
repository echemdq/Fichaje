using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class TipoDoc
    {
        int idtipodoc;

        public int Idtipodoc
        {
            get { return idtipodoc; }
            set { idtipodoc = value; }
        }
        string detalle;

        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }

        public TipoDoc(int i, string d)
        {
            idtipodoc = i;
            detalle = d;
        }
    }
}
