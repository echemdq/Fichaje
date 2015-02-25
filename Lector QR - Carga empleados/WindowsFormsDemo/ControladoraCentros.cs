using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class ControladoraCentros : IDAO<CentroDeCostos>
    {
        BdCentros bdcent = new BdCentros();
        public void Agregar(CentroDeCostos dato)
        {
            throw new NotImplementedException();
        }

        public List<CentroDeCostos> TraerTodos()
        {
            return bdcent.TraerTodos();
        }

        public void Borrar(CentroDeCostos dato)
        {
            throw new NotImplementedException();
        }

        public CentroDeCostos Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<CentroDeCostos> BuscarEspecial(string dato)
        {
            throw new NotImplementedException();
        }

        public void Modificar(CentroDeCostos dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
