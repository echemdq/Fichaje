using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace WindowsFormsDemo
{
    public class BdFeriados : IDAO<Feriados>
    {
        Acceso_BD oacceso = new Acceso_BD();
        public void Agregar(Feriados dato)
        {
            string cmdtext = "";
            if (oacceso.Tipo == "sql")
            {
                cmdtext = "insert into feriados(fecha, detalle) values('" + dato.Fecha.ToShortDateString() + "','" + dato.Detalle + "')";
            }
            else
            {
                cmdtext = "insert into feriados(fecha, detalle) values('" + dato.Fecha.ToString("yyyy-MM-dd") + "','" + dato.Detalle + "')";
            }
            oacceso.ActualizarBD(cmdtext);
        }

        public List<Feriados> TraerTodos()
        {
            string cmdtext = "select * from Feriados";
            DataTable dt = oacceso.leerDatos(cmdtext);
            Feriados usuario = null;
            List<Feriados> lista = new List<Feriados>();
            foreach (DataRow dr in dt.Rows)
            {
                usuario = new Feriados(Convert.ToInt32(dr["idferiados"]), Convert.ToDateTime(dr["fecha"]), Convert.ToString(dr["detalle"]));
                lista.Add(usuario);
            }
            return lista;
        }

        public void Borrar(Feriados dato)
        {
            string cmdtext = "delete from feriados where idferiados = '"+dato.Idferiados+"'";
            oacceso.ActualizarBD(cmdtext);
        }

        public Feriados Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<Feriados> BuscarEspecial(string dato)
        {
            throw new NotImplementedException();
        }

        public void Modificar(Feriados dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
