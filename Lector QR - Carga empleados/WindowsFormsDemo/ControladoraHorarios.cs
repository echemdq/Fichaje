using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class ControladoraHorarios : IDAO<Horarios>
    {
        BdHorarios bd = new BdHorarios();
        public void Agregar(Horarios dato)
        {
            bd.Agregar(dato);
        }

        public List<Horarios> TraerTodos()
        {
           return bd.TraerTodos();
        }

        public void Borrar(Horarios dato)
        {
            bd.Borrar(dato);
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
