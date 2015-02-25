using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace WindowsFormsDemo
{
    public class BdEmpleados : IDAO<Empleados>
    {
        Acceso_BD oacceso = new Acceso_BD();
        public void Agregar(Empleados dato)
        {
            string cmdtext = "";
            cmdtext = "insert into empleados(legajo, idtipodoc,documento, nombre, domicilio, idcentrodecostos, idtipodeempleados, foto, descanso, horasdetrabajo, horario, ingreso1, egreso1, ingreso2, egreso2, lunes, martes, miercoles, jueves, viernes, sabado, domingo, activo, nocturno) values ('" + dato.Legajo + "','" + dato.Tipod.Idtipodoc + "','" + dato.Documento + "','" + dato.Nombre + "','" + dato.Domicilio + "','" + dato.Centro.Idcentrodecostros + "','" + dato.Tipoe.Idtipodeempleados + "','" + dato.Foto + "','" + dato.Descanso + "','" + dato.Horastrabajo + "','" + dato.Horario + "','" + dato.Ing1.TimeOfDay + "','" + dato.Eg1.TimeOfDay + "','" + dato.Ing2.TimeOfDay + "','" + dato.Eg2.TimeOfDay + "','" + dato.Lunes + "','" + dato.Martes + "','" + dato.Miercoles + "','" + dato.Jueves + "','" + dato.Viernes + "','" + dato.Sabado + "','" + dato.Domingo + "','"+dato.Activo+"','"+dato.Nocturno+"')";
            oacceso.ActualizarBD(cmdtext);
        }

        public List<Empleados> TraerTodos()
        {
            throw new NotImplementedException();
        }

        public void Borrar(Empleados dato)
        {
            string cmdtext = "delete from empleados where idempleados = '" + dato.Idempleados + "'";
            oacceso.ActualizarBD(cmdtext);
        }

        public Empleados Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<Empleados> BuscarEspecial(string dato)
        {
            string cmdtext = "select nocturno, activo, lunes, martes, miercoles, jueves, viernes, sabado, domingo, ingreso1, egreso1, ingreso2, egreso2, horario, descanso, horasdetrabajo, foto, idempleados, legajo, documento, nombre, domicilio, td.idtipodoc, td.detalle as detalledoc, te.idtipodeempleados, te.detalle as detalleemp, c.idcentrodecostos, c.detalle as detallecen from empleados e inner join tipodoc td on e.idtipodoc = td.idtipodoc inner join tipodeempleados te on e.idtipodeempleados = te.idtipodeempleados inner join centrodecostos c on e.idcentrodecostos = c.idcentrodecostos where nombre "+ dato +" or legajo " + dato + " order by nombre";
            DataTable dt = oacceso.leerDatos(cmdtext);
            Empleados usuario = null;
            CentroDeCostos centro = null;
            TipoDeEmpleados tipoe = null;
            TipoDoc tipod = null;
            List<Empleados> lista = new List<Empleados>();
            foreach (DataRow dr in dt.Rows)
            {
                centro = new CentroDeCostos(Convert.ToInt32(dr["idcentrodecostos"]),Convert.ToString(dr["detallecen"]));
                tipoe = new TipoDeEmpleados(Convert.ToInt32(dr["idtipodeempleados"]),Convert.ToString(dr["detalleemp"]));
                tipod = new TipoDoc(Convert.ToInt32(dr["idtipodoc"]),Convert.ToString(dr["detalledoc"]));
                string ing1 = Convert.ToString(dr["ingreso1"]);
                string eg1 = Convert.ToString(dr["egreso1"]);
                string ing2 = Convert.ToString(dr["ingreso2"]);
                string eg2 = Convert.ToString(dr["egreso2"]);
                int descanso = Convert.ToInt32(dr["descanso"]);
                int activo = Convert.ToInt32(dr["activo"]);
                int nocturno = Convert.ToInt32(dr["nocturno"]);
                usuario = new Empleados(Convert.ToInt32(dr["idempleados"]), Convert.ToInt32(dr["legajo"]), Convert.ToInt32(dr["documento"]), Convert.ToString(dr["nombre"]), Convert.ToString(dr["domicilio"]), Convert.ToInt32(dr["horario"]), Convert.ToString(dr["foto"]), tipod, tipoe, centro, descanso, Convert.ToInt32(dr["horasdetrabajo"]), Convert.ToDateTime(ing1), Convert.ToDateTime(eg1), Convert.ToDateTime(ing2), Convert.ToDateTime(eg2), Convert.ToInt32(dr["lunes"]), Convert.ToInt32(dr["martes"]), Convert.ToInt32(dr["miercoles"]), Convert.ToInt32(dr["jueves"]), Convert.ToInt32(dr["viernes"]), Convert.ToInt32(dr["sabado"]), Convert.ToInt32(dr["domingo"]), activo, nocturno);                
                lista.Add(usuario);
            }
            return lista;
        }

        public void Modificar(Empleados dato)
        {
            string cmdtext = "update empleados set legajo = '" + dato.Legajo + "', documento = '" + dato.Documento + "', domicilio = '" + dato.Domicilio + "', nombre = '" + dato.Nombre + "', idtipodoc = '" + dato.Tipod.Idtipodoc + "', idtipodeempleados = '" + dato.Tipoe.Idtipodeempleados + "', idcentrodecostos ='" + dato.Centro.Idcentrodecostros + "', foto = '" + dato.Foto + "', horario = '" + dato.Horario + "', descanso = '" + dato.Descanso + "', horasdetrabajo = '" + dato.Horastrabajo + "', ingreso1 = '" + dato.Ing1.TimeOfDay + "', egreso1 = '" + dato.Eg1.TimeOfDay + "', ingreso2 = '" + dato.Ing2.TimeOfDay + "', egreso2 = '" + dato.Eg2.TimeOfDay + "', lunes = '" + dato.Lunes + "', martes = '" + dato.Martes + "', miercoles = '" + dato.Miercoles + "', jueves = '" + dato.Jueves + "', viernes = '" + dato.Viernes + "', sabado = '" + dato.Sabado + "', domingo = '" + dato.Domingo + "', activo = '"+dato.Activo+"', nocturno = '"+dato.Nocturno+"' where idempleados = '" + dato.Idempleados + "'";
            oacceso.ActualizarBD(cmdtext);
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}