using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WindowsFormsDemo
{
    public class BdHorarios : IDAO<Horarios>
    {
        Acceso_BD oacceso = new Acceso_BD();

        public void Agregar(Horarios dato)
        {
            string cmdtext = "INSERT INTO horarios(detalle, horario, descanso, horasdetrabajo, nocturno, ingreso1, egreso1, ingreso2, egreso2, lunes, martes, miercoles, jueves, viernes, sabado, domingo) VALUES ('"+ dato.Detalle + "', '"+dato.Horario+"', '"+dato.Descanso+"', '"+dato.Horasdetrabajo+"', '"+dato.Nocturno+"', '"+dato.Ingreso1+"', '"+dato.Egreso1+"', '"+dato.Ingreso2+"', '"+dato.Egreso2+"', '"+dato.Lunes+"', '"+dato.Martes+"', '"+dato.Miercoles+"', '"+dato.Jueves+"', '"+dato.Viernes+"', '"+dato.Sabado+"', '"+dato.Domingo+"')";
            oacceso.ActualizarBD(cmdtext);
        }
                

        public List<Horarios> TraerTodos()
        {
            List<Horarios> aux = new List<Horarios>();
            string cmdtext = "select detalle, idhorarios, horario, descanso, horasdetrabajo, nocturno, ingreso1, egreso1, ingreso2, egreso2, lunes, martes, miercoles, jueves, viernes, sabado, domingo from horarios";
            DataTable dt = oacceso.leerDatos(cmdtext);
            foreach (DataRow dr in dt.Rows)
            {
                int id = Convert.ToInt32(dr["idhorarios"]);
                string detalle = Convert.ToString(dr["detalle"]);
                string horario = Convert.ToString(dr["horario"]);
                string descanso = Convert.ToString(dr["descanso"]);
                string horasdetrabajo = Convert.ToString(dr["horasdetrabajo"]);
                string nocturno = Convert.ToString(dr["nocturno"]);
                string ingreso1 = Convert.ToString(dr["ingreso1"]);
                string egreso1 = Convert.ToString(dr["egreso1"]);
                string ingreso2 = Convert.ToString(dr["ingreso2"]);
                string egreso2 = Convert.ToString(dr["egreso2"]);
                string lunes = Convert.ToString(dr["lunes"]);
                string martes = Convert.ToString(dr["martes"]);
                string miercoles = Convert.ToString(dr["miercoles"]);
                string jueves = Convert.ToString(dr["jueves"]);
                string viernes = Convert.ToString(dr["viernes"]);
                string sabado = Convert.ToString(dr["sabado"]);
                string domingo = Convert.ToString(dr["domingo"]);
                Horarios h = new Horarios(id, detalle, horario, descanso, horasdetrabajo, nocturno, ingreso1, egreso1, ingreso2, egreso2, lunes, martes, miercoles, jueves, viernes, sabado, domingo);
                aux.Add(h);
            }
            return aux;
        }

        public void Borrar(Horarios dato)
        {
            string cmdtext = "delete from horarios where idhorarios = '" + dato.Idhorarios + "'";
            oacceso.ActualizarBD(cmdtext);
        }

        public Horarios Buscar(string dato)
        {
            string cmdtext = "select * from horarios where idhorarios = '" + dato + "'";
            DataTable dt = oacceso.leerDatos(cmdtext);
            Horarios h = null;
            foreach (DataRow dr in dt.Rows)
            {
                int id = Convert.ToInt32(dr["idhorarios"]);
                string detalle = Convert.ToString(dr["detalle"]);
                string horario = Convert.ToString(dr["horario"]);
                string descanso = Convert.ToString(dr["descanso"]);
                string horasdetrabajo = Convert.ToString(dr["horasdetrabajo"]);
                string nocturno = Convert.ToString(dr["nocturno"]);
                string ingreso1 = Convert.ToString(dr["ingreso1"]);
                string egreso1 = Convert.ToString(dr["egreso1"]);
                string ingreso2 = Convert.ToString(dr["ingreso2"]);
                string egreso2 = Convert.ToString(dr["egreso2"]);
                string lunes = Convert.ToString(dr["lunes"]);
                string martes = Convert.ToString(dr["martes"]);
                string miercoles = Convert.ToString(dr["miercoles"]);
                string jueves = Convert.ToString(dr["jueves"]);
                string viernes = Convert.ToString(dr["viernes"]);
                string sabado = Convert.ToString(dr["sabado"]);
                string domingo = Convert.ToString(dr["domingo"]);
                h = new Horarios(id, detalle, horario, descanso, horasdetrabajo, nocturno, ingreso1, egreso1, ingreso2, egreso2, lunes, martes, miercoles, jueves, viernes, sabado, domingo);
            }
            return h;
        }

        public List<Horarios> BuscarEspecial(string dato)
        {
            throw new NotImplementedException();
        }

        public void Modificar(Horarios dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
