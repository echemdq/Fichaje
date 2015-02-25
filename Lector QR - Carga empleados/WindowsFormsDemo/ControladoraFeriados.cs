using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class ControladoraFeriados : IDAO<Feriados>
    {
        BdFeriados bd = new BdFeriados();
        public void Agregar(Feriados dato)
        {
            bd.Agregar(dato);
        }

        public List<Feriados> TraerTodos()
        {
            return bd.TraerTodos();
        }

        public void Borrar(Feriados dato)
        {
            bd.Borrar(dato);
        }

        public Feriados Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<Feriados> BuscarEspecial(string dato)
        {
            throw new NotImplementedException();
        }

        public void Modificar(Feriados dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
