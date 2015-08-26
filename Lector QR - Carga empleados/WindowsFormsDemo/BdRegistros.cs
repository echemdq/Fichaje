﻿using System;
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

        public List<Registros> TraerTodosEspecial(string dato, string dato1, string dato2)
        {
            List<Registros> aux = new List<Registros>();
            DataTable dt = null;
            if (dato1 == "")
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados limit '" + dato + "'");
                }
                else
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados order by id desc limit " + dato + "");
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(Convert.ToInt32(dr["id"]), 0, Convert.ToString(dr["registro"]), Convert.ToString(dr["foto"]), Convert.ToString(dr["nombre"]));
                    aux.Add(r);
                }
            }
            else
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados where registro > '"+dato1+"' and registro < '"+dato2+"' limit '" + dato + "'");
                }
                else
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e on r.idempleados = e.idempleados where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' order by id desc limit " + dato + "");
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(Convert.ToInt32(dr["id"]), 0, Convert.ToString(dr["registro"]), Convert.ToString(dr["foto"]), Convert.ToString(dr["nombre"]));
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
                dt = oacceso.leerDatos("select r.idregistros as idregistros, r.idempleados as idempleados, convert(nvarchar,registro,120) as registro, r.foto as foto, e.nombre as nombre from Registros r inner join empleados e on r.idempleados = e.idempleados where " + dato + " order by registro asc");
            }
            else
            {
                dt = oacceso.leerDatos("select r.idregistros as idregistros, r.idempleados as idempleados, date_format(registro, '%Y-%m-%d %H:%i:%s') as registro, r.foto as foto, e.nombre as nombre from Registros r inner join empleados e on r.idempleados = e.idempleados where " + dato + " order by registro asc");
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
