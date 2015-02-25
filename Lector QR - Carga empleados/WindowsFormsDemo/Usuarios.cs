using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class Usuarios
    {
        int idusuarios;
        string usuario;
        string contrasena;

        public int Idusuarios
        {
            get { return idusuarios; }
            set { idusuarios = value; }
        }
        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        public string Contrasena
        {
            get { return contrasena; }
            set { contrasena = value; }
        }
        public Usuarios(int i, string u, string c)
        {
            idusuarios = i;
            usuario = u;
            contrasena = c;
        }

    }
}