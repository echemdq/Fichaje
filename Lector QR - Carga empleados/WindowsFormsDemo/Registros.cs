using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class Registros
    {
        int idregistros;

        public int Idregistros
        {
            get { return idregistros; }
            set { idregistros = value; }
        }
        int idempleados;

        public int Idempleados
        {
            get { return idempleados; }
            set { idempleados = value; }
        }
        
        string registro;

        public string Registro
        {
            get { return registro; }
            set { registro = value; }
        }
        string foto;

        public string Foto
        {
            get { return foto; }
            set { foto = value; }
        }
        string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        string estado;

        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        string motivo;

        public string Motivo
        {
            get { return motivo; }
            set { motivo = value; }
        }

        public Registros(int idr, int ide, string r, string f, string n)
        {
            idregistros = idr;
            idempleados = ide;
            registro = r;
            foto = f;
            nombre = n;
        }

        public Registros(int idr, int ide, string r, string f, string n, string e, string m)
        {
            idregistros = idr;
            idempleados = ide;
            registro = r;
            foto = f;
            nombre = n;
            estado = e;
            motivo = m;
        }
    }
}
