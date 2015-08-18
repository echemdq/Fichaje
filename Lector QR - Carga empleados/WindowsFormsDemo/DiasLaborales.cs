using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class DiasLaborales
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        Horarios horario;

        public Horarios Horario
        {
            get { return horario; }
            set { horario = value; }
        }

        Empleados empleado;

        public Empleados Empleado
        {
            get { return empleado; }
            set { empleado = value; }
        }

        string semana;

        public string Semana
        {
            get { return semana; }
            set { semana = value; }
        }

        DateTime desde;

        public DateTime Desde
        {
            get { return desde; }
            set { desde = value; }
        }

        DateTime hasta;

        public DateTime Hasta
        {
            get { return hasta; }
            set { hasta = value; }
        }

        public DiasLaborales(int i, Horarios h, Empleados e, string s, DateTime d, DateTime ha)
        {
            id = i;
            horario = h;
            empleado = e;
            semana = s;
            desde = d;
            hasta = ha;
        }
    }
}
