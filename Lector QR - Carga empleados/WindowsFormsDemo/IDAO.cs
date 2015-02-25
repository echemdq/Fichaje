using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public interface IDAO<T>
    {
        void Agregar(T dato);
        List<T> TraerTodos();
        void Borrar(T dato);
        T Buscar(string dato);
        List<T> BuscarEspecial(string dato);
        void Modificar(T dato);
        int traerSigID();
    }
}
