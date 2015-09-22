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

        public List<Registros> TraerFichajesAnulados(string dato, string dato1, string dato2, int c, int e)
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
                    

                    if (c != 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select e.nombre as nombre, r.registro as registro, r.motivo as motivo from registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where e.idcentrodecostos = '" + c + "' and e.idtipodeempleados = '" + e + "' and r.estado=0 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c != 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select e.nombre as nombre, r.registro as registro, r.motivo as motivo from registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos  where e.idcentrodecostos = '" + c + "' and r.estado=0 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c == 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select e.nombre as nombre, r.registro as registro, r.motivo as motivo from registros r left join empleados e on e.idempleados=r.idempleados left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where e.idtipodeempleados = '" + e + "' and r.estado=0 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c == 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select e.nombre as nombre, r.registro as registro, r.motivo as motivo from registros r left join empleados e on e.idempleados=r.idempleados where r.estado=0 order by e.nombre, r.registro limit " + dato);
                    }
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
                    if (c != 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select e.nombre as nombre, r.registro as registro, r.motivo as motivo from registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where r.registro between '" + dato1 + "' and '" + dato2 + "' and e.idcentrodecostos = '" + c + "' and e.idtipodeempleados = '" + e + "' and r.estado=0 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c != 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select e.nombre as nombre, r.registro as registro, r.motivo as motivo from registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos  where r.registro between '" + dato1 + "' and '" + dato2 + "' and e.idcentrodecostos = '" + c + "' and r.estado=0 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c == 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select e.nombre as nombre, r.registro as registro, r.motivo as motivo from registros r left join empleados e on e.idempleados=r.idempleados left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where r.registro between '" + dato1 + "' and '" + dato2 + "' and e.idtipodeempleados = '" + e + "' and r.estado=0 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c == 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select e.nombre as nombre, r.registro as registro, r.motivo as motivo from registros r left join empleados e on e.idempleados=r.idempleados where r.registro between '" + dato1 + "' and '" + dato2 + "' and r.estado=0 order by e.nombre, r.registro limit " + dato);
                    }
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(0, 0, Convert.ToString(dr["registro"]), Convert.ToString(dr["motivo"]), Convert.ToString(dr["nombre"]));                    
                    aux.Add(r);
                }
            }
            return aux;
        }

        public List<Registros> TraerFichajesManuales(string dato, string dato1, string dato2, int c, int e)
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
                    if (c != 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select r.idempleados as id, e.nombre as nombre, r.registro as registro from registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where e.idcentrodecostos = '" + c + "' and e.idtipodeempleados = '" + e + "' and r.manual=1 and r.estado=1 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c != 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select r.idempleados as id, e.nombre as nombre, r.registro as registro from registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos where e.idcentrodecostos = '" + c + "' and r.manual=1 and r.estado=1 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c == 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select r.idempleados as id, e.nombre as nombre, r.registro as registro from registros r left join empleados e on e.idempleados=r.idempleados left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where e.idtipodeempleados = '" + e + "' and r.manual=1 and r.estado=1 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c == 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select r.idempleados as id, e.nombre as nombre, r.registro as registro from registros r left join empleados e on e.idempleados=r.idempleados where r.manual=1 and r.estado=1 order by e.nombre, r.registro limit " + dato);
                    }
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
                    if (c != 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select r.idempleados as id, e.nombre as nombre, r.registro as registro from registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where registro between '" + dato1 + "' and '" + dato2 + "' and e.idcentrodecostos = '" + c + "' and e.idtipodeempleados = '" + e + "' and r.manual=1 and r.estado=1 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c != 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select r.idempleados as id, e.nombre as nombre, r.registro as registro from registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos where registro between '" + dato1 + "' and '" + dato2 + "' and e.idcentrodecostos = '" + c + "' and r.manual=1 and r.estado=1 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c == 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select r.idempleados as id, e.nombre as nombre, r.registro as registro from registros r left join empleados e on e.idempleados=r.idempleados left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where registro between '" + dato1 + "' and '" + dato2 + "' and e.idtipodeempleados = '" + e + "' and r.manual=1 and r.estado=1 order by e.nombre, r.registro limit " + dato);
                    }
                    else if (c == 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select r.idempleados as id, e.nombre as nombre, r.registro as registro from registros r left join empleados e on e.idempleados=r.idempleados where registro between '" + dato1 + "' and '" + dato2 + "' and r.manual=1 and r.estado=1 order by e.nombre, r.registro limit " + dato);
                    }
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["id"]), Convert.ToString(dr["registro"]), "", Convert.ToString(dr["nombre"]));
                    aux.Add(r);
                }
            }
            return aux;
        }

        public List<Registros> TraerLlegadasTarde(string dato, string dato1, string dato2, int tipo, int tolerancia,int c,int e)
        {
            List<Registros> aux = new List<Registros>();
            DataTable dt = null;
            if (dato1 == "")
            {

            }
            else
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados where registro > '" + dato1 + "' and registro < '" + dato2 + "' and estado = '1' limit '" + dato + "'");
                }
                else
                {
                    dt = oacceso.leerDatos("call sp_llegadastarde('"+dato1+"', '"+dato2+"', '"+tipo+"', '"+tolerancia+"', '"+c+"', '"+e+"')");
                }
                if (tipo == 1 && tolerancia == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Registros r = new Registros(0, Convert.ToInt32(dr["minutos"]), Convert.ToString(dr["dia"]), Convert.ToString(dr["minutos"]), Convert.ToString(dr["empleado"]));
                        aux.Add(r);
                    }
                }
                else if (tipo == 1 && tolerancia == 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Registros r = new Registros(0, Convert.ToInt32(dr["minutos"]), Convert.ToString(dr["dia"]), Convert.ToString(dr["minutos"]), Convert.ToString(dr["empleado"]));
                        aux.Add(r);
                    }
                }
                else if (tipo == 0 && tolerancia == 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Registros r = new Registros(0, Convert.ToInt32(dr["minutos"]), Convert.ToString(dr["cantidad"]), Convert.ToString(dr["minutos"]), Convert.ToString(dr["empleado"]));
                        aux.Add(r);
                    }
                }
                else if (tipo == 0 && tolerancia == 1)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Registros r = new Registros(0, Convert.ToInt32(dr["minutos"]), Convert.ToString(dr["cantidad"]), Convert.ToString(dr["minutos"]), Convert.ToString(dr["empleado"]));
                        aux.Add(r);
                    }
                }
            }
            return aux;
        }



        public List<Registros> TraerMalFichados(string dato, string dato1, string dato2, int c, int e)
        {
            List<Registros> aux = new List<Registros>();
            DataTable dt = null;
            if (dato1 == "")
            {
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,r.registro as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados where r.estado = '1' GROUP BY r.idempleados,DATE(r.fechareal) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.fechareal) limit " + dato);
                }
                else
                {
                    if (c != 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,DATE_FORMAT(DATE(r.registro), '%d/%m/%Y') as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where r.estado = '1' and e.idcentrodecostos = '"+c+"' and e.idtipodeempleados = '"+e+"' GROUP BY r.idempleados,DATE(r.fechareal) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.fechareal) limit " + dato);
                    }
                    else if (c != 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,DATE_FORMAT(DATE(r.registro), '%d/%m/%Y') as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos where r.estado = '1' and e.idcentrodecostos = '" + c + "' GROUP BY r.idempleados,DATE(r.fechareal) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.fechareal) limit " + dato);
                    }
                    else if (c == 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,DATE_FORMAT(DATE(r.registro), '%d/%m/%Y') as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where r.estado = '1' and e.idtipodeempleados = '" + e + "' GROUP BY r.idempleados,DATE(r.fechareal) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.fechareal) limit " + dato);
                    }
                    else if (c == 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,DATE_FORMAT(DATE(r.registro), '%d/%m/%Y') as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados where r.estado = '1' GROUP BY r.idempleados,DATE(r.fechareal) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.fechareal) limit " + dato);
                    }
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
                    dt = oacceso.leerDatos("select r.idregistros as id, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r inner join empleados e where r.idempleados = e.idempleados where fechareal > '" + dato1 + "' and fechareal < '" + dato2 + "' and estado = '1' limit '" + dato + "'");
                }
                else
                {
                    if (c != 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,DATE_FORMAT(DATE(r.registro), '%d/%m/%Y') as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' and r.estado = '1' and e.idcentrodecostos = '" + c + "' and e.idtipodeempleados = '" + e + "' GROUP BY r.idempleados,DATE(r.fechareal) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.fechareal) limit " + dato);
                    }
                    else if (c != 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,DATE_FORMAT(DATE(r.registro), '%d/%m/%Y') as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' and r.estado = '1' and e.idcentrodecostos = '" + c + "' GROUP BY r.idempleados,DATE(r.fechareal) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.fechareal) limit " + dato);
                    }
                    else if (c == 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,DATE_FORMAT(DATE(r.registro), '%d/%m/%Y') as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' and r.estado = '1' and e.idtipodeempleados = '" + e + "' GROUP BY r.idempleados,DATE(r.fechareal) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.fechareal) limit " + dato);
                    }
                    else if (c == 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("SELECT r.idempleados as id, e.nombre as nombre,DATE_FORMAT(DATE(r.registro), '%d/%m/%Y') as registro,COUNT(*) as cont FROM registros r left join empleados e on e.idempleados=r.idempleados where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' and r.estado = '1' GROUP BY r.idempleados,DATE(r.fechareal) having  mod(count(*),2) <> 0 order by e.nombre, DATE(r.fechareal) limit " + dato);
                    }
                }
                foreach (DataRow dr in dt.Rows)
                {
                    Registros r = new Registros(Convert.ToInt32(dr["id"]), Convert.ToInt32(dr["id"]), Convert.ToString(dr["registro"]), "", Convert.ToString(dr["nombre"]));
                    aux.Add(r);
                }
            }
            return aux;
        }

        public List<Registros> TraerTodosEspecial(string dato, string dato1, string dato2, int c, int e)
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
                    if (c != 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select r.idregistros as id, r.idempleados as id1, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r left join empleados e on r.idempleados = e.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where estado = '1' and e.idcentrodecostos = '" + c + "' and e.idtipodeempleados = '" + e + "' order by id desc limit " + dato + "");
                    }
                    else if (c != 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select r.idregistros as id, r.idempleados as id1, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r left join empleados e on r.idempleados = e.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos where estado = '1' and e.idcentrodecostos = '" + c + "' order by id desc limit " + dato + "");
                    }
                    else if (c == 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select r.idregistros as id, r.idempleados as id1, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r left join empleados e on r.idempleados = e.idempleados left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where estado = '1' and e.idtipodeempleados = '" + e + "' order by id desc limit " + dato + "");
                    }
                    else if (c == 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select r.idregistros as id, r.idempleados as id1, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r left join empleados e on r.idempleados = e.idempleados where estado = '1' order by id desc limit " + dato + "");
                    }
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
                    if (c != 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select r.idregistros as id, r.idempleados as id1, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r left join empleados e on r.idempleados = e.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' and estado = '1' and e.idcentrodecostos = '" + c + "' and e.idtipodeempleados = '" + e + "' order by id desc limit " + dato + "");
                    }
                    else if (c != 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select r.idregistros as id, r.idempleados as id1, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r left join empleados e on r.idempleados = e.idempleados left join centrodecostos c on c.idcentrodecostos = e.idcentrodecostos where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' and estado = '1' and e.idcentrodecostos = '" + c + "' order by id desc limit " + dato + "");
                    }
                    else if (c == 0 && e != 0)
                    {
                        dt = oacceso.leerDatos("select r.idregistros as id, r.idempleados as id1, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r left join empleados e on r.idempleados = e.idempleados left join tipodeempleados t on t.idtipodeempleados = e.idtipodeempleados where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' and estado = '1' and e.idtipodeempleados = '" + e + "' order by id desc limit " + dato + "");
                    }
                    else if (c == 0 && e == 0)
                    {
                        dt = oacceso.leerDatos("select r.idregistros as id, r.idempleados as id1, r.foto as foto, r.registro as registro, e.nombre as nombre from registros r left join empleados e on r.idempleados = e.idempleados where registro >= '" + dato1 + "' and registro <= '" + dato2 + "' and estado = '1' order by id desc limit " + dato + "");
                    }
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
