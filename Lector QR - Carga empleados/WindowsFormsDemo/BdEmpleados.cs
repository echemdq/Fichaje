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
            cmdtext = "insert into empleados(legajo, idtipodoc,documento, nombre, domicilio, idcentrodecostos, idtipodeempleados, foto, activo) values ('" + dato.Legajo + "','" + dato.Tipod.Idtipodoc + "','" + dato.Documento + "','" + dato.Nombre + "','" + dato.Domicilio + "','" + dato.Centro.Idcentrodecostros + "','" + dato.Tipoe.Idtipodeempleados + "','" + dato.Foto + "','"+dato.Activo+"')";
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
            string cmdtext = "select activo, foto, idempleados, legajo, documento, nombre, domicilio, td.idtipodoc, td.detalle as detalledoc, te.idtipodeempleados, te.detalle as detalleemp, c.idcentrodecostos, c.detalle as detallecen from empleados e inner join tipodoc td on e.idtipodoc = td.idtipodoc inner join tipodeempleados te on e.idtipodeempleados = te.idtipodeempleados inner join centrodecostos c on e.idcentrodecostos = c.idcentrodecostos where idempleados = '" + dato + "' order by nombre";
            DataTable dt = oacceso.leerDatos(cmdtext);
            Empleados usuario = null;
            CentroDeCostos centro = null;
            TipoDeEmpleados tipoe = null;
            TipoDoc tipod = null;
            foreach (DataRow dr in dt.Rows)
            {
                centro = new CentroDeCostos(Convert.ToInt32(dr["idcentrodecostos"]), Convert.ToString(dr["detallecen"]));
                tipoe = new TipoDeEmpleados(Convert.ToInt32(dr["idtipodeempleados"]), Convert.ToString(dr["detalleemp"]));
                tipod = new TipoDoc(Convert.ToInt32(dr["idtipodoc"]), Convert.ToString(dr["detalledoc"]));
                int activo = Convert.ToInt32(dr["activo"]);
                usuario = new Empleados(Convert.ToInt32(dr["idempleados"]), Convert.ToInt32(dr["legajo"]), Convert.ToInt32(dr["documento"]), Convert.ToString(dr["nombre"]), Convert.ToString(dr["domicilio"]), Convert.ToString(dr["foto"]), tipod, tipoe, centro, activo);
                
            }
            return usuario;
        }

        public List<Empleados> BuscarEspecial(string dato)
        {
            string cmdtext = "select activo, foto, idempleados, legajo, documento, nombre, domicilio, td.idtipodoc, td.detalle as detalledoc, te.idtipodeempleados, te.detalle as detalleemp, c.idcentrodecostos, c.detalle as detallecen from empleados e inner join tipodoc td on e.idtipodoc = td.idtipodoc inner join tipodeempleados te on e.idtipodeempleados = te.idtipodeempleados inner join centrodecostos c on e.idcentrodecostos = c.idcentrodecostos where nombre "+ dato +" or legajo " + dato + " order by nombre";
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
                int activo = Convert.ToInt32(dr["activo"]);
                usuario = new Empleados(Convert.ToInt32(dr["idempleados"]), Convert.ToInt32(dr["legajo"]), Convert.ToInt32(dr["documento"]), Convert.ToString(dr["nombre"]), Convert.ToString(dr["domicilio"]), Convert.ToString(dr["foto"]), tipod, tipoe, centro, activo);                
                lista.Add(usuario);
            }
            return lista;
        }

        public void Modificar(Empleados dato)
        {
            string cmdtext = "update empleados set legajo = '" + dato.Legajo + "', documento = '" + dato.Documento + "', domicilio = '" + dato.Domicilio + "', nombre = '" + dato.Nombre + "', idtipodoc = '" + dato.Tipod.Idtipodoc + "', idtipodeempleados = '" + dato.Tipoe.Idtipodeempleados + "', idcentrodecostos ='" + dato.Centro.Idcentrodecostros + "', foto = '" + dato.Foto + "', activo = '"+dato.Activo+"' where idempleados = '" + dato.Idempleados + "'";
            oacceso.ActualizarBD(cmdtext);
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}