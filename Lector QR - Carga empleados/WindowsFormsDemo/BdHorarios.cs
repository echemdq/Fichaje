using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public void Borrar(Horarios dato)
        {
            throw new NotImplementedException();
        }

        public Horarios Buscar(string dato)
        {
            throw new NotImplementedException();
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
