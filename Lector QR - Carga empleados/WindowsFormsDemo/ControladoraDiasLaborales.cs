﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsDemo
{
    public class ControladoraDiasLaborales : IDAO<DiasLaborales>
    {
        BdDiasLaborales bd = new BdDiasLaborales();
        public void Agregar(DiasLaborales dato)
        {
            throw new NotImplementedException();
        }

        public List<DiasLaborales> TraerTodos()
        {
            throw new NotImplementedException();
        }

        public void Borrar(DiasLaborales dato)
        {
            throw new NotImplementedException();
        }

        public DiasLaborales Buscar(string dato)
        {
            throw new NotImplementedException();
        }

        public List<DiasLaborales> BuscarEspecial(string dato)
        {
            return bd.BuscarEspecial(dato);
        }

        public void Modificar(DiasLaborales dato)
        {
            throw new NotImplementedException();
        }

        public int traerSigID()
        {
            throw new NotImplementedException();
        }
    }
}
