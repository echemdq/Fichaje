using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace WindowsFormsDemo
{
    public class BdRegistros : IDAO<Registros>
    {
        Acceso_BD oacceso = new Acceso_BD();
        public void Agregar(Registros dato)
        {
            throw new NotImplementedException();
        }

        public List<Registros> TraerTodos()
        {
            throw new NotImplementedException();
        }

        public List<Registros> TraerFichajesAnulados(string dato, string dato1, string dato2)
        {
            List<Registros> aux = new List<Registros>();
            DataTable dt = null;
            if (dato1 == "")
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,r.registro as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados where r.estado = '1' GROUP BY r.idempleados,DATE(r.registro) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.registro) limit " + dato);
                }
                else
                {
                    dt = oacceso.leerDatos("select e.nombre as nombre, r.registro as registro, r.motivo as motivo from registros r left join empleados e on e.idempleados=r.idempleados where r.estado=0 order by e.nombre, r.registro limit " + dato);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(0, 0, Convert.ToString(dr["registro"]), Convert.ToString(dr["motivo"]), Convert.ToString(dr["nombre"]));
                    aux.Add(r);
                }
            }
            else
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados where registro > '" + dato1 + "' and registro < '" + dato2 + "' and estado = '1' limit '" + dato + "'");
                }
                else
                {
                    dt = oacceso.leerDatos("select e.nombre as nombre, r.registro as registro, r.motivo as motivo from registros r left join empleados e on e.idempleados=r.idempleados where r.registro between '" + dato1 + "' and '" + dato2 + "' and r.estado=0 order by e.nombre, r.registro limit " + dato);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(0, 0, Convert.ToString(dr["registro"]), Convert.ToString(dr["motivo"]), Convert.ToString(dr["nombre"]));                    
                    aux.Add(r);
                }
            }
            return aux;
        }

        public List<Registros> TraerFichajesManuales(string dato, string dato1, string dato2)
        {
            List<Registros> aux = new List<Registros>();
            DataTable dt = null;
            if (dato1 == "")
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,r.registro as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados where r.estado = '1' GROUP BY r.idempleados,DATE(r.registro) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.registro) limit " + dato);
                }
                else
                {
                    dt = oacceso.leerDatos("select r.idempleados as id, e.nombre as nombre, r.registro as registro from registros r left join empleados e on e.idempleados=r.idempleados where r.manual=1 and r.estado=1 order by e.nombre, r.registro limit " + dato);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["id"]), Convert.ToString(dr["registro"]), "", Convert.ToString(dr["nombre"]));
                    aux.Add(r);
                }
            }
            else
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados where registro > '" + dato1 + "' and registro < '" + dato2 + "' and estado = '1' limit '" + dato + "'");
                }
                else
                {
                    dt = oacceso.leerDatos("select r.idempleados as id, e.nombre as nombre, r.registro as registro from registros r left join empleados e on e.idempleados=r.idempleados where r.manual=1 and registro between '" + dato1 + "' and '" + dato2 + "' and r.estado=1 order by e.nombre, r.registro limit " + dato);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["id"]), Convert.ToString(dr["registro"]), "", Convert.ToString(dr["nombre"]));
                    aux.Add(r);
                }
            }
            return aux;
        }

        public List<Registros> TraerMalFichados(string dato, string dato1, string dato2)
        {
            List<Registros> aux = new List<Registros>();
            DataTable dt = null;
            if (dato1 == "")
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,r.registro as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados where r.estado = '1' GROUP BY r.idempleados,DATE(r.registro) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.registro) limit " + dato);
                }
                else
                {
                    dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,DATE_FORMAT(DATE(r.registro), '%d/%m/%Y') as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados where r.estado = '1' GROUP BY r.idempleados,DATE(r.registro) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.registro) limit " + dato);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(Convert.ToInt32(dr["id"]), 0, Convert.ToString(dr["registro"]), "", Convert.ToString(dr["nombre"]));
                    aux.Add(r);
                }
            }
            else
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados where registro > '" + dato1 + "' and registro < '" + dato2 + "' and estado = '1' limit '" + dato + "'");
                }
                else
                {
                    dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,DATE_FORMAT(DATE(r.registro), '%d/%m/%Y') as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' and r.estado = '1' GROUP BY r.idempleados,DATE(r.registro) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.registro) limit " + dato);                    
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["id"]), Convert.ToString(dr["registro"]), "", Convert.ToString(dr["nombre"]));
                    aux.Add(r);
                }
            }
            return aux;
        }

        public List<Registros> TraerTodosEspecial(string dato, string dato1, string dato2)
        {
            List<Registros> aux = new List<Registros>();
            DataTable dt = null;
            if (dato1 == "")
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados and estado = '1' limit '" + dato + "'");
                }
                else
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.idempleados as id1, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados and estado = '1' order by id desc limit " + dato + "");
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["id1"]), Convert.ToString(dr["registro"]), Convert.ToString(dr["foto"]), Convert.ToString(dr["nombre"]));
                    aux.Add(r);
                }
            }
            else
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados where registro > '" + dato1 + "' and registro < '" + dato2 + "' and estado = '1' limit '" + dato + "'");
                }
                else
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.idempleados as id1, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e on r.idempleados = e.idempleados where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' and estado = '1' order by id desc limit " + dato + "");
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["id1"]), Convert.ToString(dr["registro"]), Convert.ToString(dr["foto"]), Convert.ToString(dr["nombre"]));
                    aux.Add(r);
                }
            }
            return aux;
        }

        public void Borrar(Registros dato)
        {
            throw new NotImplementedException();
        }

        public Registros Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<Registros> BuscarEspecial(string dato)
        {
            List<Registros> aux = new List<Registros>();
            DataTable dt = null;
            if (oacceso.Tipo == "sql")
            {
                dt = oacceso.leerDatos("select r.idregistros as idregistros, r.idempleados as idempleados, convert(nvarchar,registro,120) as registro, r.foto as foto, e.nombre as nombre from Registros r inner join empleados e on r.idempleados = e.idempleados where " + dato + " and estado = '1' order by registro asc");
            }
            else
            {
                dt = oacceso.leerDatos("select r.idregistros as idregistros, r.idempleados as idempleados, date_format(registro, '%Y-%m-%d %H:%i:%s') as registro, r.foto as foto, e.nombre as nombre from Registros r inner join empleados e on r.idempleados = e.idempleados where " + dato + " and estado = '1' order by registro asc");
            }
            foreach (DataRow dr in dt.Rows)
            {
                Registros r = new Registros(Convert.ToInt32(dr["idregistros"]), Convert.ToInt32(dr["idempleados"]), Convert.ToString(dr["registro"]), Convert.ToString(dr["foto"]), Convert.ToString(dr["nombre"]));
                aux.Add(r);
            }
            return aux;
        }

        public void Modificar(Registros dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
