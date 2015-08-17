using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsDemo
{
    public partial class Buscar_Empleado : Form
    {
        ControladoraEmpleados controlemp = new ControladoraEmpleados();
        public Empleados u = null;
        public Buscar_Empleado()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                dataGridView1.Rows.Clear();
                string d1 = "";
                string d2 = "";
                string d3 = textBox1.Text.Trim();
                int cant = 1;

                bool a = true;
                while (a == true)
                {
                    int f = d3.LastIndexOf(" ");
                    if (f == -1 && d3.Length != 0)
                    {
                        d1 = d3.Trim();
                        d3 = "";
                        if (cant == 1)
                        {
                            d2 += " like '%" + d1 + "%' ";
                        }
                        else
                        {
                            d2 += " and nombre like '%" + d1 + "%' ";
                        }
                        a = false;
                    }
                    else
                    {
                        int c = d3.LastIndexOf(" ");

                        if (c != -1)
                        {
                            int d = d3.LastIndexOf(" ");
                            d1 = d3.Substring(d, d3.Length - d);
                            d1 = d1.Trim();
                            d = d3.LastIndexOf(" ");
                            d3 = d3.Substring(0, d);
                            if (cant == 1)
                            {
                                d2 += " like '%" + d1 + "%' ";
                            }
                            else
                            {
                                d2 += " and nombre like '%" + d1 + "%' ";

                            }
                        }
                        else
                        {
                            a = false;
                        }
                    }
                    cant++;
                }
                List<Empleados> lista = controlemp.BuscarEspecial(d2);
                int i = 0;
                foreach (Empleados aux in lista)
                {
                    i++;
                }
                int x = 0;
                if (i > 0)
                {
                    dataGridView1.Rows.Add(i);
                    foreach (Empleados aux in lista)
                    {
                        dataGridView1.Rows[x].Cells[0].Value = aux.Legajo;
                        dataGridView1.Rows[x].Cells[1].Value = aux.Nombre;
                        dataGridView1.Rows[x].Cells[2].Value = aux.Tipod.Detalle;
                        dataGridView1.Rows[x].Cells[3].Value = aux.Documento;
                        dataGridView1.Rows[x].Cells[4].Value = aux.Domicilio;
                        dataGridView1.Rows[x].Cells[5].Value = aux.Centro.Detalle;
                        dataGridView1.Rows[x].Cells[6].Value = aux.Tipoe.Detalle;
                        dataGridView1.Rows[x].Cells[7].Value = aux.Idempleados;
                        dataGridView1.Rows[x].Cells[8].Value = aux.Tipoe.Idtipodeempleados;
                        dataGridView1.Rows[x].Cells[9].Value = aux.Tipod.Idtipodoc;
                        dataGridView1.Rows[x].Cells[10].Value = aux.Centro.Idcentrodecostros;
                        dataGridView1.Rows[x].Cells[11].Value = aux.Foto;
                        dataGridView1.Rows[x].Cells[12].Value = aux.Activo;
                        x++;
                    }
                }
            }
        }

        private void Buscar_Empleado_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 13;
            dataGridView1.Columns[0].Name = "Legajo";
            dataGridView1.Columns[1].Name = "Nombre";
            dataGridView1.Columns[2].Name = "Tipo Doc";
            dataGridView1.Columns[3].Name = "Documento";
            dataGridView1.Columns[4].Name = "Domicilio";
            dataGridView1.Columns[5].Name = "Centro";
            dataGridView1.Columns[6].Name = "Tipo Empleado";
            dataGridView1.Columns[7].Name = "idempleado";
            dataGridView1.Columns[8].Name = "idtipoem";
            dataGridView1.Columns[9].Name = "idtipodoc";
            dataGridView1.Columns[10].Name = "idcentro";
            dataGridView1.Columns[11].Name = "foto";
            dataGridView1.Columns[12].Name = "";
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].Visible = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int filaseleccionada = Convert.ToInt32(this.dataGridView1.CurrentRow.Index);
            int idcentro = Convert.ToInt32(dataGridView1[10, filaseleccionada].Value);
            string centro = dataGridView1[5, filaseleccionada].Value.ToString();
            CentroDeCostos c = new CentroDeCostos(idcentro, centro);
            int idtipoem = Convert.ToInt32(dataGridView1[8, filaseleccionada].Value);
            string tipoemp = dataGridView1[6, filaseleccionada].Value.ToString();
            TipoDeEmpleados tipoe = new TipoDeEmpleados(idtipoem, tipoemp);
            int idtipod = Convert.ToInt32(dataGridView1[9, filaseleccionada].Value);
            string tipod = dataGridView1[2, filaseleccionada].Value.ToString();
            TipoDoc tipodoc = new TipoDoc(idtipod, tipod);
            int id = Convert.ToInt32(dataGridView1[7, filaseleccionada].Value);
            int legajo  = Convert.ToInt32(dataGridView1[0, filaseleccionada].Value);
            string nombre = dataGridView1[1, filaseleccionada].Value.ToString();
            int documento  = Convert.ToInt32(dataGridView1[3, filaseleccionada].Value);
            string domicilio = dataGridView1[4, filaseleccionada].Value.ToString();
            string foto = dataGridView1[11, filaseleccionada].Value.ToString();
            int activo = Convert.ToInt32(dataGridView1[12, filaseleccionada].Value);
            u = new Empleados(id, legajo, documento, nombre, domicilio, foto, tipodoc, tipoe, c, activo);
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1_Click(sender, e);
            }
        }

        
    }
}
