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



    }
}
