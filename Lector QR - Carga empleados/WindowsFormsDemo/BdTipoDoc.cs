using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace WindowsFormsDemo
{
    public class BdTipoDoc : IDAO<TipoDoc>
    {
        Acceso_BD oacceso = new Acceso_BD();
        public void Agregar(TipoDoc dato)
        {
            throw new NotImplementedException();
        }

        public List<TipoDoc> TraerTodos()
        {
            List<TipoDoc> aux = new List<TipoDoc>();
            string cmdtext = "select * from tipodoc";
            DataTable dt = oacceso.leerDatos(cmdtext);
            foreach (DataRow dr in dt.Rows)
            {
                int id = Convert.ToInt32(dr["idtipodoc"]);
                string detalle = Convert.ToString(dr["detalle"]);
                TipoDoc c = new TipoDoc(id, detalle);
                aux.Add(c);
            }
            return aux;
        }

        public void Borrar(TipoDoc dato)
        {
            throw new NotImplementedException();
        }

        public TipoDoc Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<TipoDoc> BuscarEspecial(string dato)
        {
            throw new NotImplementedException();
        }

        public void Modificar(TipoDoc dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
