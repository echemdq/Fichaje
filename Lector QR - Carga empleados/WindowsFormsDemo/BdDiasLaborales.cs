using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WindowsFormsDemo
{
    public class BdDiasLaborales : IDAO<DiasLaborales>
    {
        Acceso_BD oacceso = new Acceso_BD();
        public void Agregar(DiasLaborales dato)
        {
            string hasta = dato.Hasta.ToShortDateString();
            if (hasta == "01/01/0001")
            {
                string cmdtext = "INSERT INTO horariosempleados(idhorarios, idempleados, desde, semana) VALUES ('" + dato.Horario.Idhorarios + "', '" + dato.Empleado.Idempleados + "', '" + dato.Desde.ToString("yyyy/MM/dd") + "', '" + dato.Semana + "')";
                oacceso.ActualizarBD(cmdtext);
            }
            else
            {
                string cmdtext = "INSERT INTO horariosempleados(idhorarios, idempleados, desde, hasta, semana) VALUES ('" + dato.Horario.Idhorarios + "', '" + dato.Empleado.Idempleados + "', '" + dato.Desde.ToString("yyyy/MM/dd") + "', '" + dato.Hasta.ToString("yyyy/MM/dd") + "', '" + dato.Semana + "')";
                oacceso.ActualizarBD(cmdtext);
            }
        }

        public List<DiasLaborales> TraerTodos()
        {
            throw new NotImplementedException(); 
        }

        public void Borrar(DiasLaborales dato)
        {
            string cmdtext = "delete from horariosempleados where idhorariosempleados = '" + dato.Id + "'";
            oacceso.ActualizarBD(cmdtext);
        }

        public DiasLaborales Buscar(string dato)
        {

            throw new NotImplementedException();
        }

        public List<DiasLaborales> BuscarEspecial(string dato)
        {
            DiasLaborales di = null;
            string cmdtext = "select he.idhorariosempleados as id, he.idempleados as nombre, he.semana as semana, he.desde as desde, he.hasta as hasta from horariosempleados he where idempleados = '" + dato + "'";
            DataTable dt = oacceso.leerDatos(cmdtext);
            List<DiasLaborales> lista = new List<DiasLaborales>();
            DateTime hasta = Convert.ToDateTime("1900/01/01");
            foreach (DataRow dr in dt.Rows)
            {
                BdEmpleados bd = new BdEmpleados();
                Empleados e = null;
                
                e = bd.Buscar(Convert.ToString(dr["nombre"]));
                if (Convert.ToString(dr["hasta"]) != "")
                {
                    hasta = Convert.ToDateTime(dr["hasta"]);
                }
                di = new DiasLaborales(Convert.ToInt32(dr["id"]), null, e, Convert.ToString(dr["semana"]), Convert.ToDateTime(dr["desde"]), hasta);
                lista.Add(di);
            }
            return lista;
        }

        public void Modificar(DiasLaborales dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
