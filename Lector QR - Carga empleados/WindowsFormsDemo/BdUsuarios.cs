using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace WindowsFormsDemo
{
    public class BdUsuarios : IDAO<Usuarios>
    {
        Acceso_BD oacceso = new Acceso_BD();
        #region Miembros de IDAO<Usuarios>

        public void Agregar(Usuarios dato)
        {
            string cmdtext = "insert into usuarios(usuario, contrasena, estado) values ('" + dato.Usuario + "', '" + dato.Contrasena + "')";
            oacceso.ActualizarBD(cmdtext);
        }

        public List<Usuarios> TraerTodos()
        {
            string cmdtext = "select * from usuarios order by usuario desc";
            DataTable dt = oacceso.leerDatos(cmdtext);
            Usuarios usuario = null;
            List<Usuarios> lista = new List<Usuarios>();
            foreach (DataRow dr in dt.Rows)
            {
                usuario = new Usuarios(Convert.ToInt32(dr["idusuarios"]), Convert.ToString(dr["usuario"]), Convert.ToString(dr["contrasena"]));
                lista.Add(usuario);
            }
            return lista;
        }

        public void Borrar(Usuarios dato)
        {
            throw new NotImplementedException();
        }

        public Usuarios Buscar(string dato)
        {
            Usuarios usuario = null;
            string cmdtext = "select * from usuarios where usuario = '" + dato + "'";
            DataTable dt = oacceso.leerDatos(cmdtext);
            foreach (DataRow dr in dt.Rows)
            {
                usuario = new Usuarios(Convert.ToInt32(dr["idusuarios"]), Convert.ToString(dr["usuario"]), Convert.ToString(dr["contrasena"]));
            }
            return usuario;
        }

        public List<Usuarios> BuscarEspecial(string dato)
        {
            string cmdtext = "select * from usuarios where usuario " + dato + "order by usuario";
            DataTable dt = oacceso.leerDatos(cmdtext);
            Usuarios usuario = null;
            List<Usuarios> lista = new List<Usuarios>();
            foreach (DataRow dr in dt.Rows)
            {
                usuario = new Usuarios(Convert.ToInt32(dr["idusuarios"]), Convert.ToString(dr["usuario"]), Convert.ToString(dr["contrasena"]));
                lista.Add(usuario);
            }
            return lista;
        }

        public void Modificar(Usuarios dato)
        {
            string cmdtext = "update usuarios set contrasena = '" + dato.Contrasena + "' where idusuarios = '" + dato.Idusuarios + "'";
            oacceso.ActualizarBD(cmdtext);
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
