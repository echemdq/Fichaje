using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class ControladoraNovedades : IDAO<Novedades>
    {
        BdNovedades bd = new BdNovedades();
        public void Agregar(Novedades dato)
        {
            bd.Agregar(dato);
        }

        public List<Novedades> TraerTodos()
        {
            throw new NotImplementedException();
        }

        public void Borrar(Novedades dato)
        {
            bd.Borrar(dato);
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
