using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class ControladoraRegistros : IDAO<Registros>
    {
        BdRegistros bd = new BdRegistros();
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
            return bd.TraerTodosEspecial(dato, dato1, dato2);
        }

        public List<Registros> TraerMalFichados(string dato, string dato1, string dato2)
        {
            return bd.TraerMalFichados(dato, dato1, dato2);
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
            return bd.BuscarEspecial(dato);
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
