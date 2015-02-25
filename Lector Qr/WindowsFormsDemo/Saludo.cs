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
                    dt = oacceso.leerDatos("select (select count(*) from registros where idempleados = empleados.idempleados and registro = '" + horafichaje.ToString("dd-MM-yyyy") + "') as hoy, horario, case when horario = 1 then egreso1 else egreso2 end as salida, idempleados, ingreso1, nombre, foto, nocturno from empleados where documento = '" + x.ToString() + "'");
                }
                else
                {
                    dt = oacceso.leerDatos("select (select count(*) from registros where idempleados = 'empleados.idempleados' and date_format(registro, '%Y-%m-%d') = '" + horafichaje.ToString("yyyy-MM-dd") + "') as hoy, horario, case when horario = 1 then egreso1 else egreso2 end as salida, idempleados, ingreso1, nombre, foto, nocturno from empleados where documento = '" + x.ToString() + "'");
                }
                

                string nombre = "";
                string idemp = "";
                int noct = 0;
                string foto = "";
                DateTime salida = DateTime.Now;
                int horario = 0;
                DateTime ingreso = DateTime.Now;
                int hoy = 0;
                DateTime fechareal = DateTime.Now;
                foreach (DataRow dr in dt.Rows)
                {
                    nombre = Convert.ToString(dr["nombre"]);
                    foto = Convert.ToString(dr["foto"]);
                    idemp = Convert.ToString(dr["idempleados"]);
                    noct = Convert.ToInt32(dr["nocturno"]);
                    salida = Convert.ToDateTime(dr["salida"]);
                    horario = Convert.ToInt32(dr["horario"]);
                    hoy = Convert.ToInt32(dr["hoy"]);
                    ingreso = Convert.ToDateTime(dr["ingreso1"]);
                }
                salida = salida.AddHours(3);
                ingreso = ingreso.AddHours(-2);
                DateTime tres = new DateTime(2000,10,10,03,00,00);
                if (noct == 1 && hoy == 0 && horafichaje.TimeOfDay <= salida.TimeOfDay)
                {
                    fechareal = horafichaje.AddDays(-1);
                }
                else if (noct == 0 && hoy == 0 && horafichaje.TimeOfDay <= salida.TimeOfDay && horafichaje.TimeOfDay < ingreso.TimeOfDay && horafichaje.TimeOfDay <= tres.TimeOfDay)
                {
                    fechareal = horafichaje.AddDays(-1);
                }
                else
                {
                    fechareal = horafichaje;
                }
                if (nombre != "")
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
                            timer1.Interval = 3000;
                            timer1.Enabled = true;                       // Enable the timer
                            timer1.Start();
                        }
                        else
                        {
                            string hora = "";
                            if (oacceso.Tipo == "sql")
                            {
                                hora = horafichaje.ToString("dd/MM/yyyy HH:mm:ss");
                                oacceso.ActualizarBD("insert into registros(idempleados, registro, fechareal) values ('" + idemp + "','" + hora + "','" + fechareal.ToString("dd-MM-yyyy HH:mm:ss") + "')");
                            }
                            else
                            {
                                hora = horafichaje.ToString("yyyy-MM-dd HH:mm:ss");
                                oacceso.ActualizarBD("insert into registros(idempleados, registro, fechareal) values ('" + idemp + "','" + hora + "','" + fechareal.ToString("yyyy-MM-dd HH:mm:ss") + "')");
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
                            timer1.Interval = 5000;
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
                    timer1.Interval = 3000;
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
