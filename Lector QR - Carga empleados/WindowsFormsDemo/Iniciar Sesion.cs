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
    public partial class Iniciar_Sesion : Form
    {
        ControladoraUsuarios control = new ControladoraUsuarios();
        List<Usuarios> lista = new List<Usuarios>();
        public bool ok = false;
        public Usuarios usuario = null;
        public Iniciar_Sesion()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Usuarios aux in lista)
            {
                if (aux.Usuario == combobox1.Text && aux.Contrasena == txt_contrasena.Text)
                {
                    ok = true;
                    usuario = aux;
                    this.Close();
                    break;
                }
            }
            if (!ok)
            {
                MessageBox.Show("Contraseña Incorrecta");
                txt_contrasena.Clear();
                txt_contrasena.Focus();
            }
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ok = false;
            this.Close();
        }

        private void Iniciar_Sesion_Load_1(object sender, EventArgs e)
        {
            lista = control.TraerTodos();
            combobox1.DataSource = lista;
            combobox1.DisplayMember = "usuario";
            combobox1.ValueMember = "idusuarios";
            combobox1.SelectedIndex = 0;
        }

        private void txt_contrasena_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1_Click(sender, e);
            }
        }
    }
}
