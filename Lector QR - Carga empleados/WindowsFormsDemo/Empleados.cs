using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class Empleados
    {
        int idempleados;

        public int Idempleados
        {
            get { return idempleados; }
            set { idempleados = value; }
        }
        int legajo;

        public int Legajo
        {
            get { return legajo; }
            set { legajo = value; }
        }
        int documento;

        public int Documento
        {
            get { return documento; }
            set { documento = value; }
        }
        string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        string domicilio;

        public string Domicilio
        {
            get { return domicilio; }
            set { domicilio = value; }
        }
        int horario;

        public int Horario
        {
            get { return horario; }
            set { horario = value; }
        }
        string foto;

        public string Foto
        {
            get { return foto; }
            set { foto = value; }
        }
        int descanso;

        public int Descanso
        {
            get { return descanso; }
            set { descanso = value; }
        }
        int horastrabajo;

        public int Horastrabajo
        {
            get { return horastrabajo; }
            set { horastrabajo = value; }
        }
        TipoDoc tipod;

        public TipoDoc Tipod
        {
            get { return tipod; }
            set { tipod = value; }
        }

        TipoDeEmpleados tipoe;

        public TipoDeEmpleados Tipoe
        {
            get { return tipoe; }
            set { tipoe = value; }
        }

        CentroDeCostos centro;

        public CentroDeCostos Centro
        {
            get { return centro; }
            set { centro = value; }
        }
        DateTime ing1;

        public DateTime Ing1
        {
            get { return ing1; }
            set { ing1 = value; }
        }
        DateTime ing2;

        public DateTime Ing2
        {
            get { return ing2; }
            set { ing2 = value; }
        }
        DateTime eg1;

        public DateTime Eg1
        {
            get { return eg1; }
            set { eg1 = value; }
        }
        DateTime eg2;

        public DateTime Eg2
        {
            get { return eg2; }
            set { eg2 = value; }
        }
        int lunes;

        public int Lunes
        {
            get { return lunes; }
            set { lunes = value; }
        }
        int martes;

        public int Martes
        {
            get { return martes; }
            set { martes = value; }
        }
        int miercoles;

        public int Miercoles
        {
            get { return miercoles; }
            set { miercoles = value; }
        }
        int jueves;

        public int Jueves
        {
            get { return jueves; }
            set { jueves = value; }
        }
        int viernes;

        public int Viernes
        {
            get { return viernes; }
            set { viernes = value; }
        }
        int sabado;

        public int Sabado
        {
            get { return sabado; }
            set { sabado = value; }
        }
        int domingo;

        public int Domingo
        {
            get { return domingo; }
            set { domingo = value; }
        }
        int activo;

        public int Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        private int nocturno;

        public int Nocturno
        {
            get { return nocturno; }
            set { nocturno = value; }
        }
        
        public Empleados(int i, int l, int d, string n, string dom, int h, string f, TipoDoc td, TipoDeEmpleados te, CentroDeCostos c, int desc, int horas, DateTime i1, DateTime e1, DateTime i2, DateTime e2, int lu, int ma, int mie, int jue, int vie, int sa, int domi, int act, int noct)
        {
            idempleados = i;
            legajo = l;
            documento = d;
            nombre = n;
            domicilio = dom;
            horario = h;
            foto = f;
            tipod = td;
            tipoe = te;
            centro = c;
            descanso = desc;
            horastrabajo = horas;
            ing1 = i1;
            eg1 = e1;
            ing2 = i2;
            eg2 = e2;
            lunes = lu;
            martes = ma;
            miercoles = mie;
            jueves = jue;
            viernes = vie;
            sabado = sa;
            domingo = domi;
            activo = act;
            nocturno = noct;
        }
        public Empleados(int i, int l, int d, string n, string dom, int h, string f, TipoDoc td, TipoDeEmpleados te, CentroDeCostos c, int horas, DateTime i1, DateTime e1, DateTime i2, DateTime e2, int lu, int ma, int mie, int jue, int vie, int sa, int domi, int act, int noct)
        {
            idempleados = i;
            legajo = l;
            documento = d;
            nombre = n;
            domicilio = dom;
            horario = h;
            foto = f;
            tipod = td;
            tipoe = te;
            centro = c;
            horastrabajo = horas;
            ing1 = i1;
            eg1 = e1;
            ing2 = i2;
            eg2 = e2;
            lunes = lu;
            martes = ma;
            miercoles = mie;
            jueves = jue;
            viernes = vie;
            sabado = sa;
            domingo = domi;
            activo = act;
            nocturno = noct;
        }
    }
}
