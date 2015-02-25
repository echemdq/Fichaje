using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class Feriados
    {
        int idferiados;

        public int Idferiados
        {
            get { return idferiados; }
            set { idferiados = value; }
        }

        DateTime fecha;

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        string detalle;

        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }

        public Feriados(int i, DateTime f, string d)
        {
            idferiados = i;
            fecha = f;
            detalle = d;
        }
    }
}
