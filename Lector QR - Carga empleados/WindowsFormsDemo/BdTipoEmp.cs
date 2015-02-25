using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace WindowsFormsDemo
{
    public class BdTipoEmp : IDAO<TipoDeEmpleados>
    {
        Acceso_BD oacceso = new Acceso_BD();
        public void Agregar(TipoDeEmpleados dato)
        {
            throw new NotImplementedException();
        }

        public List<TipoDeEmpleados> TraerTodos()
        {
            List<TipoDeEmpleados> aux = new List<TipoDeEmpleados>();
            string cmdtext = "select * from tipodeempleados";
            DataTable dt = oacceso.leerDatos(cmdtext);
            foreach (DataRow dr in dt.Rows)
            {
                int id = Convert.ToInt32(dr["idtipodeempleados"]);
                string detalle = Convert.ToString(dr["detalle"]);
                TipoDeEmpleados c = new TipoDeEmpleados(id, detalle);
                aux.Add(c);
            }
            return aux;
        }

        public void Borrar(TipoDeEmpleados dato)
        {
            throw new NotImplementedException();
        }

        public TipoDeEmpleados Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<TipoDeEmpleados> BuscarEspecial(string dato)
        {
            throw new NotImplementedException();
        }

        public void Modificar(TipoDeEmpleados dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
