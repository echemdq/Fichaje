using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WindowsFormsDemo
{
    public class BdCentros : IDAO<CentroDeCostos>
    {
        Acceso_BD oacceso = new Acceso_BD();
        public void Agregar(CentroDeCostos dato)
        {
            throw new NotImplementedException();
        }

        public List<CentroDeCostos> TraerTodos()
        {
            List<CentroDeCostos> aux = new List<CentroDeCostos>();
            string cmdtext = "select * from centrodecostos";
            DataTable dt = oacceso.leerDatos(cmdtext);
            foreach (DataRow dr in dt.Rows)
            {
                int id = Convert.ToInt32(dr["idcentrodecostos"]);
                string detalle = Convert.ToString(dr["detalle"]);
                CentroDeCostos c = new CentroDeCostos(id, detalle);
                aux.Add(c);
            }
            return aux;
        }

        public void Borrar(CentroDeCostos dato)
        {
            throw new NotImplementedException();
        }

        public CentroDeCostos Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<CentroDeCostos> BuscarEspecial(string dato)
        {
            throw new NotImplementedException();
        }

        public void Modificar(CentroDeCostos dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
