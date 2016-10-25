using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;

namespace WindowsFormsDemo
{
    public partial class TomaFoto : Form
    {
        private VideoCaptureDevice FuenteDeVideo = null;
        private FilterInfoCollection DispositivosDeVideo;
        private bool ExistenDispositivos = false;
        string user = "";
        double timeLeft = 5;
        int tim1 = 0;
        double resta = 0.5;
        DateTime dt;
        AccesoBd oacceso = new AccesoBd();
        int idreg = 0;
        public TomaFoto(string USUARIO, DateTime h, int tim, int idr)
        {
            InitializeComponent();
            user = USUARIO;
            dt = h;
            BuscarDispositivos();
            tim1 = tim;
            idreg = idr;
            timeLeft = tim / 1000;
        }

        private void TomaFoto_Load(object sender, EventArgs e)
        {
            if (ExistenDispositivos)
            {
                FuenteDeVideo = new VideoCaptureDevice(DispositivosDeVideo[cboDispositivos.SelectedIndex].MonikerString);
                FuenteDeVideo.NewFrame += new NewFrameEventHandler(video_NuevoFrame);
                FuenteDeVideo.Start();
                cboDispositivos.Enabled = false;
                timer5.Interval = tim1;
                timer5.Tick += new EventHandler(timer5_Tick);
                timer5.Enabled = true;
                timer6.Tick += new EventHandler(timer6_Tick);
                timer6.Enabled = true;  
            }
            else
                MessageBox.Show("Error: No se encuentra dispositivo.");
        }
        public void CargarDispositivos(FilterInfoCollection Dispositivos)
        {
            for (int i = 0; i < Dispositivos.Count; i++)
                cboDispositivos.Items.Add(Dispositivos[i].Name.ToString()); //cboDispositivos es nuestro combobox
            cboDispositivos.Text = cboDispositivos.Items[0].ToString();
        }

        public void BuscarDispositivos()
        {
            DispositivosDeVideo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (DispositivosDeVideo.Count == 0)
                ExistenDispositivos = false;
            else
            {
                ExistenDispositivos = true;
                CargarDispositivos(DispositivosDeVideo);
            }
        }

        public void TerminarFuenteDeVideo()
        {
            if (!(FuenteDeVideo == null))
                if (FuenteDeVideo.IsRunning)
                {
                    FuenteDeVideo.SignalToStop();
                    FuenteDeVideo = null;
                }
        }

        private void video_NuevoFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            pbFotoUser.Image = Imagen; //pbFotoUser es nuestro pictureBox
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            try
            {
                string ruta = "";
                DataTable dta = oacceso.leerDatos("select detalle from configuraciones where codigo = 'fichaje'");
                foreach (DataRow dr in dta.Rows)
                {
                    ruta = Convert.ToString(dr["detalle"]);
                }
                if (pbFotoUser.Image != null)
                {
                    try
                    {
                        
                        dta = oacceso.leerDatos("select max(idregistros) as id from registros");
                        string id = "";
                        foreach (DataRow dr in dta.Rows)
                        {
                            id = Convert.ToString(dr["id"]);
                        }
                        pbFotoUser.Image.Save(ruta + Convert.ToInt32(user) + " " + dt.ToString("dd-MM-yyyy HH-mm-ss") + ".jpg", ImageFormat.Jpeg);
                        oacceso.ActualizarBD("update registros set foto = '" + ruta + Convert.ToInt32(user) + " " + dt.ToString("dd-MM-yyyy HH-mm-ss") + ".jpg' where idregistros = '" + idreg + "'");
                        TerminarFuenteDeVideo();
                    }
                    catch (Exception ex)
                    {
                        dta = oacceso.leerDatos("select max(idregistros) as id from registros");
                        string id1 = "";
                        foreach (DataRow dr in dta.Rows)
                        {
                            id1 = Convert.ToString(dr["id"]);
                        }
                        oacceso.ActualizarBD("update registros set foto = '"+ex.Message+"' where idregistros = '" + idreg + "'");
                        TerminarFuenteDeVideo();
                    }                    
                }
                else
                {                    
                    dta = oacceso.leerDatos("select max(idregistros) as id from registros");
                    string id = "";
                    foreach (DataRow dr in dta.Rows)
                    {
                        id = Convert.ToString(dr["id"]);
                    }
                    oacceso.ActualizarBD("update registros set foto = 'ERROR AL SACAR FOTO' where idregistros = '" + idreg + "'");
                    TerminarFuenteDeVideo();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void timer6_Tick(object sender, EventArgs e) 
        {
            if (timeLeft > 0)
            {
                timeLeft = timeLeft - resta;
                label2.Text = "Se tomara una foto en: "+ timeLeft + " segundos";                  
            }
        }

    }
}
