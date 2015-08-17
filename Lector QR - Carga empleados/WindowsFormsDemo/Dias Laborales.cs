using System;
using System.Globalization;
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
    public partial class Diaslaborales : Form
    {
        ControladoraHorarios controlh = new ControladoraHorarios();
        public Diaslaborales()
        {
            InitializeComponent();
        }

        private void rb_corrido_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_corrido.Checked)
            {
                txt_descanso.Visible = true;
                label18.Visible = true;
                txt_ing1.Visible = true;
                txt_ing2.Visible = true;
                txt_eg1.Visible = true;
                txt_eg2.Visible = true;
                label21.Text = "Salida Descanso";
                label23.Text = "Regreso Descanso";
                label20.Visible = true;
                label21.Visible = true;
                label22.Visible = true;
                label23.Visible = true;
                txt_eg1_Validated(sender, e);
                txt_eg2_Validated(sender, e);
            }
            else
            {
                txt_descanso.Visible = true;
                txt_ing1.Visible = true;
                txt_ing2.Visible = true;
                txt_eg1.Visible = true;
                txt_eg2.Visible = true;
                label21.Text = "Ingreso 2";
                label23.Text = "Egreso 2";
                label20.Visible = true;
                label21.Visible = true;
                label22.Visible = true;
                label23.Visible = true;
                label18.Visible = true;
                txt_eg1_Validated(sender, e);
                txt_eg2_Validated(sender, e);
            }

        }

        private void Diaslaborales_Load(object sender, EventArgs e)
        {
            try
            {
                dataGridView4.ColumnCount = 17;
                dataGridView4.Columns[0].Name = "IdHorarios";
                dataGridView4.Columns[1].Name = "Detalle";
                dataGridView4.Columns[2].Name = "Horario";
                dataGridView4.Columns[3].Name = "Horas de Trabajo";
                dataGridView4.Columns[4].Name = "Descanso";
                dataGridView4.Columns[5].Name = "Nocturno";
                dataGridView4.Columns[6].Name = "Ingreso 1";
                dataGridView4.Columns[7].Name = "Egreso 1";
                dataGridView4.Columns[8].Name = "Ingreso 2";
                dataGridView4.Columns[9].Name = "Egreso 2";
                dataGridView4.Columns[10].Name = "Lunes";
                dataGridView4.Columns[11].Name = "Martes";
                dataGridView4.Columns[12].Name = "Miercoles";
                dataGridView4.Columns[13].Name = "Jueves";
                dataGridView4.Columns[14].Name = "Viernes";
                dataGridView4.Columns[15].Name = "Sabado";
                dataGridView4.Columns[16].Name = "Domingo";
                dataGridView4.Columns[0].Visible = false;
                Recargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txt_eg1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_eg1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txt_eg2_TextChanged(object sender, EventArgs e)
        {


        }

        private void txt_eg1_Validated(object sender, EventArgs e)
        {
            System.Text.RegularExpressions.Regex rtime = new System.Text.RegularExpressions.Regex(@"[0-2][0-9]\:[0-6][0-9]");


            if (!rtime.IsMatch(txt_eg1.Text) || !rtime.IsMatch(txt_ing1.Text))
            {

                //MessageBox.Show("Please provide the time in hh:mm format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                    DateTime a = Convert.ToDateTime(txt_ing1.Text);
                    DateTime b = Convert.ToDateTime(txt_eg1.Text);
                    if (b < a)
                    {
                        b = b.AddDays(1);
                        chk_noct.Checked = true;
                    }
                    else
                    {
                        chk_noct.Checked = false;
                    }

                    //DateTime horas = b.Subtract(a).TotalHours;
                    //txt_horas.Text = horas.ToString();
                    TimeSpan ts = b - a;
                    txt_horas.Text = ts.ToString();
            }
           

        }

        private void txt_eg2_Validated(object sender, EventArgs e)
        {
            System.Text.RegularExpressions.Regex rtime = new System.Text.RegularExpressions.Regex(@"[0-2][0-9]\:[0-6][0-9]");


            if (!rtime.IsMatch(txt_eg2.Text) || !rtime.IsMatch(txt_ing2.Text))
            {

                //MessageBox.Show("Please provide the time in hh:mm format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                if (rb_corrido.Checked)
                {
                    DateTime a = Convert.ToDateTime(txt_ing2.Text);
                    DateTime b = Convert.ToDateTime(txt_eg2.Text);
                    if (b < a)
                    {
                        b = b.AddDays(1);
                        chk_noct.Checked = true;
                    }
                    else
                    {
                        chk_noct.Checked = false;
                    }

                    //DateTime horas = b.Subtract(a).TotalHours;
                    //txt_horas.Text = horas.ToString();
                    TimeSpan ts = b - a;
                    txt_descanso.Text = ts.ToString();
                }
                else
                {
                    DateTime a = Convert.ToDateTime(txt_ing1.Text);
                    DateTime b = Convert.ToDateTime(txt_eg1.Text);
                    DateTime c = Convert.ToDateTime(txt_ing2.Text);
                    DateTime d = Convert.ToDateTime(txt_eg2.Text);
                    if (d < c)
                    {
                        d = d.AddDays(1);
                        chk_noct.Checked = true;
                    }
                    else
                    {
                        chk_noct.Checked = false;
                    }

                    TimeSpan ts = (b - a) + (d - c);

                    txt_horas.Text = ts.ToString();
                    txt_descanso.Text = "00:00";
                }

            }
        }

        private void txt_eg2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        
        public void limpiar()
        {
            Tdetalle.Clear();
            rb_corrido.Checked = true;
            txt_ing1.Clear();
            txt_eg1.Clear();
            txt_ing2.Clear();
            txt_eg2.Clear();
            txt_horas.Clear();
            txt_descanso.Clear();
            chk_noct.Checked = false;
            checkedListBox1.SetItemChecked(0, false);
            checkedListBox1.SetItemChecked(1, false);
            checkedListBox1.SetItemChecked(2, false);
            checkedListBox1.SetItemChecked(3, false);
            checkedListBox1.SetItemChecked(4, false);
            checkedListBox1.SetItemChecked(5, false);
            checkedListBox1.SetItemChecked(6, false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string detalle = Tdetalle.Text;
                string horario = "";
                if (rb_corrido.Checked)
                {
                    horario = "1";
                }
                else
                {
                    horario = "0";
                }
                string ingreso1 = txt_ing1.Text;
                string egreso1 = txt_eg1.Text;
                string ingreso2 = txt_ing2.Text;
                string egreso2 = txt_eg2.Text;
                string horasdetrabajo = txt_horas.Text;
                string descanso = txt_descanso.Text;
                string lunes = "0";
                string martes = "0";
                string miercoles = "0";
                string jueves = "0";
                string viernes = "0";
                string sabado = "0";
                string domingo = "0";
                int i;
                for (i = 0; i <= (checkedListBox1.Items.Count - 1); i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        if (checkedListBox1.Items[i].ToString() == "Lunes")
                        {
                            lunes = "1";
                        }
                        else if (checkedListBox1.Items[i].ToString() == "Martes")
                        {
                            martes = "1";
                        }
                        else if (checkedListBox1.Items[i].ToString() == "Miercoles")
                        {
                            miercoles = "1";
                        }
                        else if (checkedListBox1.Items[i].ToString() == "Jueves")
                        {
                            jueves = "1";
                        }
                        else if (checkedListBox1.Items[i].ToString() == "Viernes")
                        {
                            viernes = "1";
                        }
                        else if (checkedListBox1.Items[i].ToString() == "Sabado")
                        {
                            sabado = "1";
                        }
                        else if (checkedListBox1.Items[i].ToString() == "Domingo")
                        {
                            domingo = "1";
                        }
                    }
                }
                string noct = "0";
                if (chk_noct.Checked)
                {
                    noct = "1";
                }
                Horarios h = new Horarios(0, detalle, horario, descanso, horasdetrabajo, noct, ingreso1, egreso1, ingreso2, egreso2, lunes, martes, miercoles, jueves, viernes, sabado, domingo);
                controlh.Agregar(h);
                MessageBox.Show("Horario Cargado Correctamente");
                limpiar();
                Recargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int filaseleccionada = Convert.ToInt32(this.dataGridView4.CurrentRow.Index);
                int idnov = Convert.ToInt32(dataGridView4[0, filaseleccionada].Value);
                string Detalle = dataGridView4[1, filaseleccionada].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("Esta seguro de eliminar la fila: " + Detalle, "Eliminar Horario Configurado", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Horarios n = new Horarios(idnov, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                    controlh.Borrar(n);
                    MessageBox.Show("Horario Eliminado Correctamente");
                }
                Recargar();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Recargar()
        {
            dataGridView4.Rows.Clear();
            dataGridView4.Refresh();
            List<Horarios> lista = controlh.TraerTodos();
            int i = 0;
            foreach (Horarios aux in lista)
            {
                i++;
            }
            int x = 0;
            if (i > 0)
            {
                dataGridView4.Rows.Add(i);
                foreach (Horarios aux in lista)
                {
                    dataGridView4.Rows[x].Cells[0].Value = aux.Idhorarios;
                    dataGridView4.Rows[x].Cells[1].Value = aux.Detalle;
                    if (aux.Horario == "1")
                    {
                        dataGridView4.Rows[x].Cells[2].Value = "Corrido";
                    }
                    else
                    {
                        dataGridView4.Rows[x].Cells[2].Value = "Cortado";
                    }
                    dataGridView4.Rows[x].Cells[3].Value = aux.Horasdetrabajo;
                    dataGridView4.Rows[x].Cells[4].Value = aux.Descanso;
                    if (aux.Nocturno == "1")
                    {
                        dataGridView4.Rows[x].Cells[5].Value = "Si";
                    }
                    else
                    {
                        dataGridView4.Rows[x].Cells[5].Value = "No";
                    }
                    dataGridView4.Rows[x].Cells[6].Value = aux.Ingreso1;
                    dataGridView4.Rows[x].Cells[7].Value = aux.Egreso1;
                    dataGridView4.Rows[x].Cells[8].Value = aux.Ingreso2;
                    dataGridView4.Rows[x].Cells[9].Value = aux.Egreso2;
                    if (aux.Lunes == "1")
                    {
                        dataGridView4.Rows[x].Cells[10].Value = "Si";
                    }
                    else
                    {
                        dataGridView4.Rows[x].Cells[10].Value = "No";
                    }
                    if (aux.Martes == "1")
                    {
                        dataGridView4.Rows[x].Cells[11].Value = "Si";
                    }
                    else
                    {
                        dataGridView4.Rows[x].Cells[11].Value = "No";
                    }
                    if (aux.Miercoles == "1")
                    {
                        dataGridView4.Rows[x].Cells[12].Value = "Si";
                    }
                    else
                    {
                        dataGridView4.Rows[x].Cells[12].Value = "No";
                    }
                    if (aux.Jueves == "1")
                    {
                        dataGridView4.Rows[x].Cells[13].Value = "Si";
                    }
                    else
                    {
                        dataGridView4.Rows[x].Cells[13].Value = "No";
                    }
                    if (aux.Viernes == "1")
                    {
                        dataGridView4.Rows[x].Cells[14].Value = "Si";
                    }
                    else
                    {
                        dataGridView4.Rows[x].Cells[14].Value = "No";
                    }
                    if (aux.Sabado == "1")
                    {
                        dataGridView4.Rows[x].Cells[15].Value = "Si";
                    }
                    else
                    {
                        dataGridView4.Rows[x].Cells[15].Value = "No";
                    }
                    if (aux.Domingo == "1")
                    {
                        dataGridView4.Rows[x].Cells[16].Value = "Si";
                    }
                    else
                    {
                        dataGridView4.Rows[x].Cells[16].Value = "No";
                    }
                    x++;
                }
            }
        }
        private void Diaslaborales_Activated(object sender, EventArgs e)
        {
            
        }
    }
}
