using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class BdNovedades : IDAO<Novedades>
    {
        Acceso_BD oacceso = new Acceso_BD();
        public void Agregar(Novedades dato)
        {
            string cmdtext = "";
            DateTime d = Convert.ToDateTime(dato.Desde);
            DateTime h = Convert.ToDateTime(dato.Hasta);
            if (oacceso.Tipo == "sql")
            {
                cmdtext = "insert into novedades(idempleados, desde, hasta, idtiposdenovedades, detalle) values('" + dato.Empleado.Idempleados + "','" + dato.Desde + "','" + dato.Hasta + "','" + dato.Idtipodenovedades + "','" + dato.Detalle + "')";
            }
            else
            {
                cmdtext = "insert into novedades(idempleados, desde, hasta, idtiposdenovedades, detalle) values('" + dato.Empleado.Idempleados + "','" + d.ToString("yyyy-MM-dd") + "','" + h.ToString("yyyy-MM-dd") + "','" + dato.Idtipodenovedades + "','" + dato.Detalle + "')";
            }
            oacceso.ActualizarBD(cmdtext);
        }

        public List<Novedades> TraerTodos()
        {
            throw new NotImplementedException();
        }

        public void Borrar(Novedades dato)
        {
            oacceso.ActualizarBD("delete from novedades where idnovedades = '" + dato.Idnovedades + "'");
        }

        public Novedades Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<Novedades> BuscarEspecial(string dato)
        {
            throw new NotImplementedException();
        }

        public void Modificar(Novedades dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
