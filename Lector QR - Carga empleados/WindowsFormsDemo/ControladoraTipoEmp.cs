using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WindowsFormsDemo
{
    public class ControladoraTipoEmp : IDAO<TipoDeEmpleados>
    {
        BdTipoEmp bdtipoem = new BdTipoEmp();

        public void Agregar(TipoDeEmpleados dato)
        {
            throw new NotImplementedException();
        }

        public List<TipoDeEmpleados> TraerTodos()
        {
            return bdtipoem.TraerTodos();
        }

        public void Borrar(TipoDeEmpleados dato)
        {
            throw new NotImplementedException();
        }

        public TipoDeEmpleados Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<TipoDeEmpleados> BuscarEspecial(string dato)
        {
            throw new NotImplementedException();
        }

        public void Modificar(TipoDeEmpleados dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
