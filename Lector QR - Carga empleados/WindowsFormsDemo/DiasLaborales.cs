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
        string detalle;

        public string Detalle
        {
            get { return detalle; }
            set { detalle = value; }
        }
        string semana;

        public string Semana
        {
            get { return semana; }
            set { semana = value; }
        }
        string horario;

        public string Horario
        {
            get { return horario; }
            set { horario = value; }
        }
        string ingreso1;

        public string Ingreso1
        {
            get { return ingreso1; }
            set { ingreso1 = value; }
        }
        string ingreso2;

        public string Ingreso2
        {
            get { return ingreso2; }
            set { ingreso2 = value; }
        }
        string egreso1;

        public string Egreso1
        {
            get { return egreso1; }
            set { egreso1 = value; }
        }
        string egreso2;

        public string Egreso2
        {
            get { return egreso2; }
            set { egreso2 = value; }
        }
        string nocturno;

        public string Nocturno
        {
            get { return nocturno; }
            set { nocturno = value; }
        }
        int horaslaborales;

        public int Horaslaborales
        {
            get { return horaslaborales; }
            set { horaslaborales = value; }
        }
        int descanso;

        public int Descanso
        {
            get { return descanso; }
            set { descanso = value; }
        }
        string lunes;

        public string Lunes
        {
            get { return lunes; }
            set { lunes = value; }
        }
        string martes;

        public string Martes
        {
            get { return martes; }
            set { martes = value; }
        }
        string miercoles;

        public string Miercoles
        {
            get { return miercoles; }
            set { miercoles = value; }
        }
        string jueves;

        public string Jueves
        {
            get { return jueves; }
            set { jueves = value; }
        }
        string viernes;

        public string Viernes
        {
            get { return viernes; }
            set { viernes = value; }
        }
        string sabado;

        public string Sabado
        {
            get { return sabado; }
            set { sabado = value; }
        }
        string domingo;

        public string Domingo
        {
            get { return domingo; }
            set { domingo = value; }
        }
        public DiasLaborales(int i, string d, string s, string h, string i1, string i2, string e1, string e2, int hl, int desc, string n, string lu, string ma, string mi, string ju, string vi, string sa, string dom)
        {
            id = i;
            detalle = d;
            semana = s;
            horario = h;
            ingreso1 = i1;
            ingreso2 = i2;
            egreso1 = e1;
            egreso2 = e2;
            horaslaborales = hl;
            descanso = desc;
            nocturno = n;
            lunes = lu;
            martes = ma;
            miercoles = mi;
            jueves = ju;
            viernes = vi;
            sabado = sa;
            domingo = dom;
        }
    }
}
