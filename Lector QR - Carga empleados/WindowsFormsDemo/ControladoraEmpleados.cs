using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WindowsFormsDemo
{
    public class ControladoraEmpleados : IDAO<Empleados>
    {
        BdEmpleados bdemp = new BdEmpleados();
        public void Agregar(Empleados dato)
        {
            bdemp.Agregar(dato);
        }

        public List<Empleados> TraerTodos()
        {
            throw new NotImplementedException();
        }

        public void Borrar(Empleados dato)
        {
            bdemp.Borrar(dato);
        }

        public Empleados Buscar(string dato)
        {
            return bdemp.Buscar(dato);
        }

        public List<Empleados> BuscarEspecial(string dato)
        {
            return bdemp.BuscarEspecial(dato);
        }

        public void Modificar(Empleados dato)
        {
            bdemp.Modificar(dato);
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
