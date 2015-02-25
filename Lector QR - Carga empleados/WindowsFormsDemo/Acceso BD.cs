using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml;
using MySql.Data.MySqlClient;

namespace WindowsFormsDemo
{
    class Acceso_BD
    {
        static string server = "";

        public static string Server
        {
            get { return Acceso_BD.server; }
            set { Acceso_BD.server = value; }
        }
        static string database = "";

        public static string Database
        {
            get { return Acceso_BD.database; }
            set { Acceso_BD.database = value; }
        }

        static string tipo = "";

        public string Tipo
        {
            get { return Acceso_BD.tipo; }
            set { Acceso_BD.tipo = value; }
        }

        static string uid = "sa";

        public static string Uid
        {
            get { return Acceso_BD.uid; }
            set { Acceso_BD.uid = value; }
        }
        static string password = "Pa21rA*";
        //static string password = "210466";
        public static string Password
        {
            get { return Acceso_BD.password; }
            set { Acceso_BD.password = value; }
        }
        public Acceso_BD()
        {
            buscarEnXML();
            if (tipo == "sql")
            {
                uid = "sa";
                cn = new SqlConnection("Data Source=" + server + ";" + "Initial Catalog=" + database + ";" + "User id=" + Uid + ";" + "Password=" + Password + ";");
            }
            else
            {
                uid = "root";
                password = "romeo1";
                cn1 = new MySqlConnection("Server=" + server + ";" + "DATABASE=" + database + ";" + "User ID=" + Uid + ";" + "PASSWORD=" + Password + ";");
            }
        }

        public void buscarEnXML()
        {

            XDocument miXML = XDocument.Load(@"C:\Program Files\FV Sistemas\Control de Accesos\MiDoc.xml");


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

            var tipo1 = from nombre in miXML.Elements("Servers").Elements("Server")
                        where nombre.Attribute("Id_Server").Value == "1"
                        select nombre.Element("Tipo").Value;

            foreach (string minom in tipo1)
            {
                tipo = minom;
            }



        }

        SqlConnection cn = null;
        MySqlConnection cn1 = null;

        public void Conectar()
        {
            cn.Open();
        }
        public void desconectar()
        {
            cn.Close();
        }

        public void Conectar1()
        {
            cn1.Open();
        }
        public void desconectar1()
        {
            cn1.Close();
        }

        public void ActualizarBD(string query)
        {
            try
            {
                if (tipo == "sql")
                {
                    Conectar();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = query;
                    cmd.Connection = cn;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    Conectar1();
                    MySqlCommand cmd = new MySqlCommand();
                    query = query.Replace("\\", "\\\\");
                    cmd.CommandText = query;
                    cmd.Connection = cn1;
                    cmd.ExecuteNonQuery();
                }

            }
            finally
            {
                if (tipo == "sql")
                {
                    desconectar();
                }
                else
                {
                    desconectar1();
                }
            }
        }

        public DataTable leerDatos(string cmdtext)
        {
            try
            {
                DataTable dt = new DataTable();
                if (tipo == "sql")
                {
                    SqlCommand cmd = new SqlCommand(cmdtext, cn);
                    Conectar();
                    SqlDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                }
                else
                {
                    MySqlCommand cmd = new MySqlCommand(cmdtext, cn1);
                    Conectar1();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                }
                return dt;

            }
            finally
            {
                if (tipo == "sql")
                {
                    desconectar();
                }
                else
                {
                    desconectar1();
                }
            }
        }
    }
}