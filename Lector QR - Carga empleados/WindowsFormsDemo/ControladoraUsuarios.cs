using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class ControladoraUsuarios : IDAO<Usuarios>
    {
        BdUsuarios bd = new BdUsuarios();
        #region Miembros de IDAO<Usuarios>

        public void Agregar(Usuarios dato)
        {
            bd.Agregar(dato);
        }

        public List<Usuarios> TraerTodos()
        {
            return bd.TraerTodos();
        }

        public void Borrar(Usuarios dato)
        {
            throw new NotImplementedException();
        }

        public Usuarios Buscar(string dato)
        {
            return bd.Buscar(dato);
        }

        public List<Usuarios> BuscarEspecial(string dato)
        {
            return bd.BuscarEspecial(dato);
        }

        public void Modificar(Usuarios dato)
        {
            bd.Modificar(dato);
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
