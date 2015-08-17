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

        string foto;

        public string Foto
        {
            get { return foto; }
            set { foto = value; }
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

        int activo;

        public int Activo
        {
            get { return activo; }
            set { activo = value; }
        }
        
        public Empleados(int i, int l, int d, string n, string dom, string f, TipoDoc td, TipoDeEmpleados te, CentroDeCostos c, int act)
        {
            idempleados = i;
            legajo = l;
            documento = d;
            nombre = n;
            domicilio = dom;
            foto = f;
            tipod = td;
            tipoe = te;
            centro = c;
            activo = act;
        }
    }
}
