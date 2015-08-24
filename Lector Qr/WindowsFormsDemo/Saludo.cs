using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;

namespace WindowsFormsDemo
{
    public partial class Saludo : Form
    {
        AccesoBd oacceso;
        string USER = "";
        string server = "";
        DateTime horafichaje;
        string database = "";
        public bool ficho = true;
        public string empleado = "";
        
        public Saludo(string USUARIO, DateTime h)
        {
            InitializeComponent();
            horafichaje = h;
            USER = USUARIO;
        }

        public void buscarEnXML()
        {
            XDocument miXML = XDocument.Load(@"C:\Prueba\MiDoc.xml");


            var server1 = from nombre in miXML.Elements("Servers").Elements("Server")
                          where nombre.Attribute("Id_Server").Value == "1"
                          select nombre.Element("Direccion").Value;

            foreach (string minom in server1)
            {
                server = minom;
            }

            var database1 = from nombre in miXML.Elements("Servers").Elements("Server")
                            where nombre.Attribute("Id_Server").Value == "1"
                            select nombre.Element("BaseDatos").Value;

            foreach (string minom in database1)
            {
                database = minom;
            }

        }

        private void Saludo_Load(object sender, EventArgs e)
        {
            // buscarEnXML();
            try
            {
                oacceso = new AccesoBd();
                int x = Convert.ToInt32(USER);
                DataTable dt = null;
                if (oacceso.Tipo == "sql")
                {
                    dt = oacceso.leerDatos("select (select count(*) from registros where idempleados = empleados.idempleados and registro = '" + horafichaje.ToString("dd-MM-yyyy") + "') as hoy, idempleados, nombre, foto, activo from empleados where documento = '" + x.ToString() + "'");
                }
                else
                {
                    dt = oacceso.leerDatos("select (select count(*) from registros where idempleados = 'empleados.idempleados' and date_format(registro, '%Y-%m-%d') = '" + horafichaje.ToString("yyyy-MM-dd") + "') as hoy, idempleados, nombre, foto, activo from empleados where documento = '" + x.ToString() + "'");
                }
                

                string nombre = "";
                string idemp = "";
                string foto = "";
                int hoy = 0;
                string activo = "1";
                foreach (DataRow dr in dt.Rows)
                {
                    nombre = Convert.ToString(dr["nombre"]);
                    foto = Convert.ToString(dr["foto"]);
                    idemp = Convert.ToString(dr["idempleados"]);
                    hoy = Convert.ToInt32(dr["hoy"]);
                    activo = Convert.ToString(dr["activo"]);
                }

                if (nombre != "" && activo == "1")
                {
                    try
                    {
                        DataTable dta = null;
                        if (oacceso.Tipo == "sql")
                        {
                            dta = oacceso.leerDatos("SELECT TOP 1 *, DATEDIFF([minute], registro, GETDATE()) AS diferencia FROM REGISTROS ORDER BY idregistros DESC");
                        }
                        else
                        {
                            dta = oacceso.leerDatos("SELECT *, timestampdiff(MINUTE, registro, now()) AS diferencia FROM REGISTROS ORDER BY idregistros DESC limit 1");
                        }
                        int dif = 0;
                        string id = "";
                        foreach (DataRow dr in dta.Rows)
                        {
                            dif = Convert.ToInt32(dr["diferencia"]);
                            id = Convert.ToString(dr["idempleados"]);
                        }
                        if (dif < 10 && id == idemp)
                        {
                            int dife = 10 - dif;
                            label2.Text = nombre;
                            label3.Text = "Imposible grabar registro, espere " + dife + " minutos";
                            label2.ForeColor = Color.Red;
                            label3.ForeColor = Color.Red;
                            ficho = false;
                            timer1.Tick += new EventHandler(timer_Tick);
                            timer1.Interval = 1500;
                            timer1.Enabled = true;                       // Enable the timer
                            timer1.Start();
                        }
                        else
                        {
                            string hora = "";
                            if (oacceso.Tipo == "sql")
                            {
                                hora = horafichaje.ToString("dd/MM/yyyy HH:mm:ss");
                                oacceso.ActualizarBD("insert into registros(idempleados, registro, fechareal) values ('" + idemp + "','" + hora + "','" + hora + "')");
                            }
                            else
                            {
                                hora = horafichaje.ToString("yyyy-MM-dd HH:mm:ss");
                                oacceso.ActualizarBD("insert into registros(idempleados, registro, fechareal) values ('" + idemp + "','" + hora + "','" + hora + "')");
                            }
                            empleado = nombre;
                            pictureBox1.ImageLocation = foto;
                            DateTime n;
                            n = Convert.ToDateTime("12:00:00");
                            DateTime ni = DateTime.Now;
                            if (ni < n)
                            {
                                label1.Text = "BUEN DIA ";
                            }
                            else
                            {
                                label1.Text = "BUENAS TARDES ";
                            }
                            label2.Text = nombre;
                            label3.Text = hora;
                            label2.ForeColor = Color.Black;
                            label3.ForeColor = Color.Black;
                            timer1.Tick += new EventHandler(timer_Tick);
                            timer1.Interval = 1500;
                            timer1.Enabled = true;                       // Enable the timer
                            timer1.Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        ficho = false;
                        this.Close();
                    }
                }
                else
                {
                    label2.Text = "Empleado inexistente";
                    label3.Text = "Codigo de ingreso invalido";
                    label2.ForeColor = Color.Red;
                    label3.ForeColor = Color.Red;
                    ficho = false;
                    timer1.Tick += new EventHandler(timer_Tick);
                    timer1.Interval = 1500;
                    timer1.Enabled = true;                       // Enable the timer
                    timer1.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ficho = false;
                this.Close();
            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            this.Close();
            // Alert the user
        }
    }
}
