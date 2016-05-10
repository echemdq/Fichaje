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

        public List<Registros> TraerTodosEspecial(string dato, string dato1, string dato2, int c, int e)
        {
            return bd.TraerTodosEspecial(dato, dato1, dato2,c,e);
        }

        public List<Registros> TraerMalFichados(string dato, string dato1, string dato2, int c, int e)
        {
            return bd.TraerMalFichados(dato, dato1, dato2, c, e);
        }

        public List<Registros> TraerFichajesManuales(string dato, string dato1, string dato2, int c, int e)
        {
            return bd.TraerFichajesManuales(dato, dato1, dato2, c,e);
        }

        public List<Registros> TraerLlegadasTarde(string dato, string dato1, string dato2, int tipo, int tolerancia,int c, int e)
        {
            return bd.TraerLlegadasTarde(dato, dato1, dato2, tipo, tolerancia,c,e);
        }

        public List<Registros> TraerDescanso(string dato, string dato1, string dato2, int tipo, int tolerancia, int c, int e)
        {
            return bd.TraerDescanso(dato, dato1, dato2, tipo, tolerancia, c, e);
        }

        public List<Registros> TraerFichajesAnulados(string dato, string dato1, string dato2, int c, int e)
        {
            return bd.TraerFichajesAnulados(dato, dato1, dato2,c,e);
        }

        public List<Registros> TraerFichajesDesactivados(string dato, string dato1, string dato2, int c, int e)
        {
            return bd.TraerFichajesDesactivados(dato, dato1, dato2, c, e);
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
