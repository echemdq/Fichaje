using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class Horarios
    {
        int idhorarios;
        string detalle;
        string horario;
        string descanso;
        string horasdetrabajo;
        string nocturno;
        string ingreso1;
        string egreso1;
        string ingreso2;
        string egreso2;
        string lunes;
        string martes;
        string miercoles;
        string jueves;
        string viernes;
        string sabado;
        string domingo;

        public Horarios(int id, string det, string hor, string des, string horas, string noc, string ing1, string eg1, string ing2, string eg2, string lu, string mar, string mier, string jue, string vie, string sab, string dom)
        {
            idhorarios = id;
            detalle = det;
            horario = hor;
            descanso = des;
            horasdetrabajo = horas;
            nocturno = noc;
            ingreso1 = ing1;
            egreso1 = eg1;
            ingreso2 = ing2;
            egreso2 = eg2;
            lunes = lu;
            martes = mar;
            miercoles = mier;
            jueves = jue;
            viernes = vie;
            sabado = sab;
            domingo = dom;
        }


        public string Domingo
        {
            get { return domingo; }
            set { domingo = value; }
        }

        public string Sabado
        {
            get { return sabado; }
            set { sabado = value; }
        }

        public string Viernes
        {
            get { return viernes; }
            set { viernes = value; }
        }

        public string Jueves
        {
            get { return jueves; }
            set { jueves = value; }
        }

        public string Miercoles
        {
            get { return miercoles; }
            set { miercoles = value; }
        }

        public string Martes
        {
            get { return martes; }
            set { martes = value; }
        }

        public string Lunes
        {
            get { return lunes; }
            set { lunes = value; }
        }
        

        public string Egreso2
        {
            get { return egreso2; }
            set { egreso2 = value; }
        }

        public string Ingreso2
        {
            get { return ingreso2; }
            set { ingreso2 = value; }
        }

        public string Egreso1
        {
            get { return egreso1; }
            set { egreso1 = value; }
        }

        public string Ingreso1
        {
            get { return ingreso1; }
            set { ingreso1 = value; }
        }

        public string Nocturno
        {
            get { return nocturno; }
            set { nocturno = value; }
        }

        public string Horasdetrabajo
        {
            get { return horasdetrabajo; }
            set { horasdetrabajo = value; }
        }

        public string Descanso
        {
            get { return descanso; }
            set { descanso = value; }
        }


        public string Horario
        {
            get { return horario; }
            set { horario = value; }
        }

        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }

        public int Idhorarios
        {
            get { return idhorarios; }
            set { idhorarios = value; }
        }
    }
}
