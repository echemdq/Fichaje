/*
 * Copyright 2012 ZXing.Net authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


using System.Data;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using ZXing;
using ZXing.Client.Result;
using ZXing.Common;
using ZXing.Rendering;
using AForge.Video;
using AForge.Video.DirectShow;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.ComponentModel;

namespace WindowsFormsDemo
{
    public partial class WindowsFormsDemoForm : Form
    {
        List<TipoDeNovedades> listanov = new List<TipoDeNovedades>();
        private bool ExistenDispositivos = false;
        private FilterInfoCollection DispositivosDeVideo;
        private VideoCaptureDevice FuenteDeVideo = null;
        Usuarios usuarioActivo = null;
        private WebCam wCam;
        bool edito = false;
        private Timer webCamTimer;
        private readonly BarcodeReader barcodeReader;
        private readonly IList<ResultPoint> resultPoints;
        private readonly IList<Result> lastResults;
        private EncodingOptions EncodingOptions { get; set; }
        private Type Renderer { get; set; }
        private bool TryMultipleBarcodes { get; set; }
        private bool TryOnlyMultipleQRCodes { get; set; }
        ControladoraDiasLaborales controldia = new ControladoraDiasLaborales();
        ControladoraCentros controlcent = new ControladoraCentros();
        ControladoraTipoDoc controltipodoc = new ControladoraTipoDoc();
        ControladoraTipoEmp controltipoemp = new ControladoraTipoEmp();
        ControladoraEmpleados controlemp = new ControladoraEmpleados();
        ControladoraUsuarios controlusu = new ControladoraUsuarios();
        ControladoraFeriados controlfer = new ControladoraFeriados();
        ControladoraRegistros controlreg = new ControladoraRegistros();
        ControladoraNovedades controlnov = new ControladoraNovedades();
        Empleados empnov = null;
        
        List<Feriados> listaferiados = new List<Feriados>();
        bool edit = false;

        public WindowsFormsDemoForm()
        {
            InitializeComponent();
            BuscarDispositivos();
            barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                TryInverted = true,
                Options = new DecodingOptions { TryHarder = true }
            };
            barcodeReader.ResultPointFound += point =>
            {
                if (point == null)
                    resultPoints.Clear();
                else
                    resultPoints.Add(point);
            };
            barcodeReader.ResultFound += result =>
            {
                txtType.Clear();
                txtContent.Clear();
                txtTypeWebCam.Clear();
                txtContentWebCam.Clear();
                txtType.Text = result.BarcodeFormat.ToString();
                txtContent.Text += result.Text + Environment.NewLine;
                txtTypeWebCam.Text = result.BarcodeFormat.ToString();
                txtContentWebCam.Text += result.Text + Environment.NewLine;
                lastResults.Add(result);
                var parsedResult = ResultParser.parseResult(result);
                if (parsedResult != null)
                {
                    btnExtendedResult.Visible = !(parsedResult is TextParsedResult);
                    //txtContent.Text += "\r\n\r\nParsed result:\r\n" + parsedResult.DisplayResult + Environment.NewLine + Environment.NewLine;
                }
                else
                {
                    btnExtendedResult.Visible = false;
                }
            };
            resultPoints = new List<ResultPoint>();
            lastResults = new List<Result>();
            Renderer = typeof(BitmapRenderer);
        }




        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (var format in MultiFormatWriter.SupportedWriters)
            {
                if (format.ToString() == "QR_CODE")
                {
                    cmbEncoderType.Items.Add(format);
                }
            }
            cmbEncoderType.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            TerminarFuenteDeVideo();
            pbFotoUser.ImageLocation = null;
            Close();
        }
        //


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


        //
        private void btnSelectBarcodeImageFileForDecoding_Click(object sender, EventArgs e)
        {
            using (var openDlg = new OpenFileDialog())
            {
                openDlg.FileName = txtBarcodeImageFile.Text;
                openDlg.Multiselect = false;
                if (openDlg.ShowDialog(this) == DialogResult.OK)
                {
                    txtBarcodeImageFile.Text = openDlg.FileName;
                }
            }
        }

        private void btnStartDecoding_Click(object sender, EventArgs e)
        {
            try
            {
                var fileName = txtBarcodeImageFile.Text;
                if (!File.Exists(fileName))
                {
                    MessageBox.Show(this, String.Format("Archivo no encontrado: {0}", fileName), "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                using (var bitmap = (Bitmap)Bitmap.FromFile(fileName))
                {
                    if (TryOnlyMultipleQRCodes)
                        Decode(bitmap, TryMultipleBarcodes, new List<BarcodeFormat> { BarcodeFormat.QR_CODE });
                    else
                        Decode(bitmap, TryMultipleBarcodes, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Decode(Bitmap image, bool tryMultipleBarcodes, IList<BarcodeFormat> possibleFormats)
        {
            resultPoints.Clear();
            lastResults.Clear();
            txtContent.Text = String.Empty;

            var timerStart = DateTime.Now.Ticks;
            Result[] results = null;
            barcodeReader.Options.PossibleFormats = possibleFormats;
            if (tryMultipleBarcodes)
                results = barcodeReader.DecodeMultiple(image);
            else
            {
                var result = barcodeReader.Decode(image);
                if (result != null)
                {
                    results = new[] { result };
                }
            }
            var timerStop = DateTime.Now.Ticks;

            if (results == null)
            {
                txtContent.Text = "No barcode recognized";
            }
            labDuration.Text = new TimeSpan(timerStop - timerStart).Milliseconds.ToString("0 ms");

            if (results != null)
            {
                foreach (var result in results)
                {
                    if (result.ResultPoints.Length > 0)
                    {
                        var rect = new System.Drawing.Rectangle((int)result.ResultPoints[0].X, (int)result.ResultPoints[0].Y, 1, 1);
                        foreach (var point in result.ResultPoints)
                        {
                            if (point.X < rect.Left)
                                rect = new System.Drawing.Rectangle((int)point.X, rect.Y, rect.Width + rect.X - (int)point.X, rect.Height);
                            if (point.X > rect.Right)
                                rect = new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width + (int)point.X - rect.X, rect.Height);
                            if (point.Y < rect.Top)
                                rect = new System.Drawing.Rectangle(rect.X, (int)point.Y, rect.Width, rect.Height + rect.Y - (int)point.Y);
                            if (point.Y > rect.Bottom)
                                rect = new System.Drawing.Rectangle(rect.X, rect.Y, rect.Width, rect.Height + (int)point.Y - rect.Y);
                        }
                        using (var g = picBarcode.CreateGraphics())
                        {
                            g.DrawRectangle(Pens.Green, rect);
                        }
                    }
                }
            }
        }

        private void txtBarcodeImageFile_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var fileName = txtBarcodeImageFile.Text;
                if (File.Exists(fileName))
                    picBarcode.Load(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecodeWebCam_Click(object sender, EventArgs e)
        {
            txtContentWebCam.Clear();
            if (wCam == null)
            {
                wCam = new WebCam { Container = picWebCam };

                wCam.OpenConnection();

                webCamTimer = new Timer();
                webCamTimer.Tick += webCamTimer_Tick;
                webCamTimer.Interval = 200;
                webCamTimer.Start();
            }
            else
            {

                webCamTimer.Stop();
                webCamTimer = null;
                wCam.Dispose();
                wCam = null;
            }
        }

        void webCamTimer_Tick(object sender, EventArgs e)
        {
            var bitmap = wCam.GetCurrentImage();
            if (bitmap == null)
                return;
            Decode(bitmap, TryMultipleBarcodes, new List<BarcodeFormat> { BarcodeFormat.QR_CODE });
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_idemp.Text != "")
                {
                    var writer = new BarcodeWriter
                    {
                        Format = (BarcodeFormat)cmbEncoderType.SelectedItem,
                        Options = EncodingOptions ?? new EncodingOptions
                        {
                            Height = picEncodedBarCode.Height,
                            Width = picEncodedBarCode.Width
                        },
                        Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(Renderer)

                    };
                    picEncodedBarCode.Image = writer.Write(txt_documento.Text);
                    txtEncoderContent.Text = "Documento   " + txt_documento.Text + "\r\n" + "Nombre y Apellido   " + txt_nombre.Text;
                }
                else
                {
                    MessageBox.Show("Para generar el codigo debe seleccionar un empleado previamente en la solapa Empleados");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEncoderSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (picEncodedBarCode.Image != null)
                {
                    var fileName = String.Empty;
                    using (var dlg = new SaveFileDialog())
                    {
                        dlg.FileName = txt_nombre.Text + " " + txt_documento.Text;
                        dlg.DefaultExt = "png";
                        dlg.Filter = "PNG Files (*.png)|*.png|SVG Files (*.svg)|*.svg|BMP Files (*.bmp)|*.bmp|TIFF Files (*.tif)|*.tif|JPG Files (*.jpg)|*.jpg|All Files (*.*)|*.*";
                        if (dlg.ShowDialog(this) != DialogResult.OK)
                            return;
                        fileName = dlg.FileName;
                    }
                    var extension = Path.GetExtension(fileName).ToLower();
                    var bmp = (Bitmap)picEncodedBarCode.Image;
                    switch (extension)
                    {
                        case ".bmp":
                            bmp.Save(fileName, ImageFormat.Bmp);
                            break;
                        case ".jpeg":
                        case ".jpg":
                            bmp.Save(fileName, ImageFormat.Jpeg);
                            break;
                        case ".tiff":
                        case ".tif":
                            bmp.Save(fileName, ImageFormat.Tiff);
                            break;
                        case ".svg":
                            {
                                var writer = new BarcodeWriterSvg
                                {
                                    Format = (BarcodeFormat)cmbEncoderType.SelectedItem,
                                    Options = EncodingOptions ?? new EncodingOptions
                                    {
                                        Height = picEncodedBarCode.Height,
                                        Width = picEncodedBarCode.Width
                                    }
                                };
                                var svgImage = writer.Write(txtEncoderContent.Text);
                                File.WriteAllText(fileName, svgImage.Content, System.Text.Encoding.UTF8);
                            }
                            break;
                        default:
                            bmp.Save(fileName, ImageFormat.Png);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEncodeDecode_Click(object sender, EventArgs e)
        {
            if (picEncodedBarCode.Image != null)
            {
                tabCtrlMain.SelectedTab = tabPageDecoder;
                picBarcode.Image = picEncodedBarCode.Image;
                var pureBarcodeSetting = barcodeReader.Options.PureBarcode;
                try
                {
                    barcodeReader.Options.PureBarcode = true;
                    Decode((Bitmap)picEncodedBarCode.Image, false, null);
                }
                finally
                {
                    barcodeReader.Options.PureBarcode = pureBarcodeSetting;
                }
            }
        }

        private void btnEncodeOptions_Click(object sender, EventArgs e)
        {
            if (cmbEncoderType.SelectedItem == null)
            {
                MessageBox.Show(this, "Please select a barcode format first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                EncodingOptions options;
                switch ((BarcodeFormat)cmbEncoderType.SelectedItem)
                {
                    case BarcodeFormat.QR_CODE:
                        options = new ZXing.QrCode.QrCodeEncodingOptions
                        {
                            Height = picEncodedBarCode.Height,
                            Width = picEncodedBarCode.Width
                        };
                        break;
                    case BarcodeFormat.PDF_417:
                        options = new ZXing.PDF417.PDF417EncodingOptions
                        {
                            Height = picEncodedBarCode.Height,
                            Width = picEncodedBarCode.Width
                        };
                        break;
                    case BarcodeFormat.DATA_MATRIX:
                        options = new ZXing.Datamatrix.DatamatrixEncodingOptions
                        {
                            Height = picEncodedBarCode.Height,
                            Width = picEncodedBarCode.Width,
                            SymbolShape = ZXing.Datamatrix.Encoder.SymbolShapeHint.FORCE_SQUARE
                        };
                        break;
                    case BarcodeFormat.AZTEC:
                        options = new ZXing.Aztec.AztecEncodingOptions
                        {
                            Height = picEncodedBarCode.Height,
                            Width = picEncodedBarCode.Width,
                        };
                        break;
                    case BarcodeFormat.CODE_128:
                        options = new ZXing.OneD.Code128EncodingOptions
                        {
                            Height = picEncodedBarCode.Height,
                            Width = picEncodedBarCode.Width,
                        };
                        break;
                    default:
                        options = new EncodingOptions
                        {
                            Height = picEncodedBarCode.Height,
                            Width = picEncodedBarCode.Width
                        };
                        break;
                }
                var dlg = new EncodingOptionsForm
                {
                    Options = options,
                    Renderer = Renderer
                };
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    EncodingOptions = dlg.Options;
                    Renderer = dlg.Renderer;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(this, exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecodingOptions_Click(object sender, EventArgs e)
        {
            using (var dlg = new DecodingOptionsForm(barcodeReader, TryMultipleBarcodes, TryOnlyMultipleQRCodes))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    TryMultipleBarcodes = dlg.MultipleBarcodes;
                    TryOnlyMultipleQRCodes = dlg.MultipleBarcodesOnlyQR;
                }
            }
        }

        private void btnExtendedResult_Click(object sender, EventArgs e)
        {
            if (lastResults.Count < 1)
                return;
            var parsedResult = ResultParser.parseResult(lastResults[0]);
            using (var dlg = new ExtendedResultForm())
            {
                dlg.Result = parsedResult;
                dlg.ShowDialog(this);
            }
        }

        public void limpiar()
        {
            tabPageCargaEmpleados.BackColor = SystemColors.Info;
            txt_documento.Clear();
            txt_domicilio.Clear();
            txt_legajo.Clear();
            txt_nombre.Clear();
            pbFotoUser.ImageLocation = "";
            cmb_tipoemp.SelectedIndex = 0;
            cmb_tipodoc.SelectedIndex = 0;
            cmb_centrocostos.SelectedIndex = 0;
            cmb_horarios.SelectedIndex = 0;
        }

        public void deshabilitar()
        {
            btnIniciar.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            button12.Enabled = false;
            chk_empact.Enabled = false;
            txt_documento.Enabled = false;
            txt_domicilio.Enabled = false;
            txt_legajo.Enabled = false;
            txt_nombre.Enabled = false;
            cmb_tipoemp.Enabled = false;
            cmb_tipodoc.Enabled = false;
            cmb_centrocostos.Enabled = false;
            txt_nombre.BackColor = Color.LightGray;
            txt_documento.BackColor = Color.LightGray;
            txt_domicilio.BackColor = Color.LightGray;
            txt_legajo.BackColor = Color.LightGray;
        }

        public void habilitar()
        {
            btnIniciar.Enabled = true;
            button10.Enabled = true;
            button11.Enabled = true;
            button12.Enabled = true;
            chk_empact.Enabled = true;
            txt_documento.Enabled = true;
            txt_domicilio.Enabled = true;
            txt_legajo.Enabled = true;
            txt_nombre.Enabled = true;
            cmb_tipoemp.Enabled = true;
            cmb_tipodoc.Enabled = true;
            cmb_centrocostos.Enabled = true;
            txt_nombre.BackColor = Color.White;
            txt_documento.BackColor = Color.White;
            txt_domicilio.BackColor = Color.White;
            txt_legajo.BackColor = Color.White;
            txt_legajo.Focus();
        }

        private void WindowsFormsDemoForm_Load(object sender, EventArgs e)
        {
            
            Iniciar_Sesion frm = new Iniciar_Sesion();
            frm.ShowDialog();
            if (frm.ok == false)
            {
                this.Close();
            }
            else
            {
                maskedTextBox1.Text = monthCalendar1.SelectionRange.Start.ToString();
                int m = DateTime.Now.Month;
                m = m - 1;
                comboBox1.SelectedIndex = m;
                cmb_mesesnov.SelectedIndex = m;
                usuarioActivo = frm.usuario;
                txt_ano.Text = DateTime.Now.Year.ToString();
                if (usuarioActivo.Usuario != "supervisor")
                {
                    tabCtrlMain.TabPages.Remove(tabPageUsuarios);
                }
                else
                {
                    cmb_usuarios.DataSource = controlusu.TraerTodos();
                    cmb_usuarios.DisplayMember = "usuario";
                    cmb_usuarios.ValueMember = "idusuarios";
                    cmb_usuarios.SelectedIndex = 0;
                }
                lbl_idemp.Text = "";
                lbl_foto.Text = "";
                try
                {
                    listaferiados = controlfer.TraerTodos();
                    foreach (Feriados aux in listaferiados)
                    {
                        monthCalendar1.AddBoldedDate(aux.Fecha);
                    }
                    cmb_centrocostos.DataSource = controlcent.TraerTodos();
                    cmb_centrocostos.DisplayMember = "detalle";
                    cmb_centrocostos.ValueMember = "idcentrodecostros";

                    cmb_centrocostos1.DataSource = controlcent.TraerTodos();
                    cmb_centrocostos1.DisplayMember = "detalle";
                    cmb_centrocostos1.ValueMember = "idcentrodecostros";

                    cmb_centrocostos2.DataSource = controlcent.TraerTodos();
                    cmb_centrocostos2.DisplayMember = "detalle";
                    cmb_centrocostos2.ValueMember = "idcentrodecostros";

                    cmb_tipodoc.DataSource = controltipodoc.TraerTodos();
                    cmb_tipodoc.DisplayMember = "detalle";
                    cmb_tipodoc.ValueMember = "idtipodoc";

                    cmb_tipoemp.DataSource = controltipoemp.TraerTodos();
                    cmb_tipoemp.DisplayMember = "detalle";
                    cmb_tipoemp.ValueMember = "idtipodeempleados";

                    cmb_tipoemp1.DataSource = controltipoemp.TraerTodos();
                    cmb_tipoemp1.DisplayMember = "detalle";
                    cmb_tipoemp1.ValueMember = "idtipodeempleados";

                    cmb_tipoemp2.DataSource = controltipoemp.TraerTodos();
                    cmb_tipoemp2.DisplayMember = "detalle";
                    cmb_tipoemp2.ValueMember = "idtipodeempleados";

                    maskedTextBox11.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    maskedTextBox12.Text = DateTime.Today.ToString("dd/MM/yyyy");

                    Acceso_BD oacceso = new Acceso_BD();

                    cmb_tiponov.DataSource = oacceso.leerDatos("select * from tipodenovedad");
                    cmb_tiponov.DisplayMember = "detalle";
                    cmb_tiponov.ValueMember = "idtiponovedad";
                    cmb_tiponov.SelectedIndex = 0;

                    cmb_horarios.DataSource = oacceso.leerDatos("select concat(detalle,case when horario=1 then ' Corrido:' else ' Cortado:' end,ingreso1,'-',egreso1,'-',ingreso2,'-',egreso2) as detalle, idhorarios from horarios");
                    cmb_horarios.DisplayMember = "detalle";
                    cmb_horarios.ValueMember = "idhorarios";
                    cmb_horarios.SelectedIndex = 0;

                    DataTable dt = oacceso.leerDatos("select * from tiposdenovedades");
                    foreach (DataRow dr in dt.Rows)
                    {
                        TipoDeNovedades t = new TipoDeNovedades(Convert.ToInt32(dr["idtiposdenovedades"]), Convert.ToInt32(dr["idtipodenovedad"]), Convert.ToString(dr["detalle"]));
                        listanov.Add(t);
                    }
                    cmb_tiponov1.Items.Clear();
                    foreach (TipoDeNovedades aux in listanov)
                    {
                        if (aux.Idtipodenovedad == Convert.ToInt32(cmb_tiponov.SelectedValue))
                        {

                            cmb_tiponov1.DisplayMember = "detalle";
                            cmb_tiponov1.ValueMember = "idtiposdenovedades";
                            cmb_tiponov1.Items.Add(aux);
                        }
                    }
                    cmb_tiponov1.SelectedIndex = 0;
                    m = m + 1;
                    int ano = DateTime.Now.Year;
                    if (oacceso.Tipo == "sql")
                    {
                        dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde, hasta, n.detalle as novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtiposdenovedades where DATEPART(month, desde) = '" + m + "' and DATEPART(year, desde) = '" + ano + "' order by desde asc");
                    }
                    else
                    {
                        dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde, hasta, n.detalle as novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtiposdenovedades where month(desde) = '" + m + "' and year(desde) = '" + ano + "' order by desde asc");
                    }
                    dataGridView3.Columns[0].Visible = false;
                    m = m - 1;


                    int primera = ano - 2;
                    cmb_anos.Items.Clear();
                    for (int i = 0; i < 5; i++)
                    {
                        cmb_anos.Items.Add(primera);
                        primera = primera + 1;
                    }
                    cmb_anos.SelectedIndex = 2;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    dataGridView1.ColumnCount = 5;
                    dataGridView1.Columns[0].Name = "Nombre";
                    dataGridView1.Columns[1].Name = "Registro";
                    dataGridView1.Columns[2].Name = "Id Registro";
                    dataGridView1.Columns[3].Name = "Id Empleado";
                    dataGridView1.Columns[4].Name = "Foto";
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[4].Visible = false;
                    deshabilitar();
                }
            }
        }

        public void Pulsar(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.E)
            {
                MessageBox.Show("hola");
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Close();                   // Alert the user
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            habilitar();
            limpiar();
        }
        public void RecargarHorarios()
        {
            cmb_horarios.DataSource = null;
            Acceso_BD oacceso = new Acceso_BD();
            cmb_horarios.DataSource = oacceso.leerDatos("select concat(detalle,case when horario=1 then ' Corrido:' else ' Cortado:' end,ingreso1,'-',egreso1,'-',ingreso2,'-',egreso2) as detalle, idhorarios from horarios");
            cmb_horarios.DisplayMember = "detalle";
            cmb_horarios.ValueMember = "idhorarios";
            cmb_horarios.SelectedIndex = 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                TipoDoc tipodoc = (TipoDoc)cmb_tipodoc.SelectedItem;
                TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp.SelectedItem;
                CentroDeCostos cent = (CentroDeCostos)cmb_centrocostos.SelectedItem;
                Empleados emp = null;
                int activo = 1;
                if (!chk_empact.Checked)
                {
                    activo = 0;
                }
                emp = new Empleados(0, Convert.ToInt32(txt_legajo.Text), Convert.ToInt32(txt_documento.Text), txt_nombre.Text, txt_domicilio.Text, lbl_foto.Text, tipodoc, tipoemp, cent,activo);


                if (edit == false)
                {
                    controlemp.Agregar(emp);
                    MessageBox.Show("Empleado Cargado Correctamente");
                    edit = false;
                    lbl_idemp.Text = "";
                    txt_empleadohorario.Text = "Buscar Horarios Empleado para Eliminar";
                    dataGridView4.Rows.Clear();
                    dataGridView4.Refresh();
                    lbl_foto.Text = "";
                    deshabilitar();
                    limpiar();
                }
                else
                {
                    emp.Idempleados = Convert.ToInt32(lbl_idemp.Text);
                    controlemp.Modificar(emp);
                    MessageBox.Show("Empleado Modificado Correctamente");
                    edit = false;
                    lbl_idemp.Text = "";
                    txt_empleadohorario.Text = "Buscar Horarios Empleado para Eliminar";
                    dataGridView4.Rows.Clear();
                    dataGridView4.Refresh();
                    lbl_foto.Text = "";
                    deshabilitar();
                    limpiar();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                deshabilitar();
                limpiar();
                Buscar_Empleado frm = new Buscar_Empleado();
                frm.ShowDialog();
                Empleados u = frm.u;
                if (u != null)
                {
                    lbl_idemp.Text = Convert.ToString(u.Idempleados);
                    txt_empleadohorario.Text = u.Nombre;
                    txt_documento.Text = u.Documento.ToString();
                    txt_domicilio.Text = u.Domicilio;
                    txt_legajo.Text = u.Legajo.ToString();
                    txt_nombre.Text = u.Nombre;
                    cmb_centrocostos.Text = u.Centro.Detalle;
                    cmb_tipodoc.Text = u.Tipod.Detalle;
                    cmb_tipoemp.Text = u.Tipoe.Detalle;
                    if (u.Activo == 0)
                    {
                        chk_empact.Checked = false;
                        tabPageCargaEmpleados.BackColor = Color.LightCoral;
                    }
                    else if (u.Activo == 1)
                    {
                        chk_empact.Checked = true;
                        tabPageCargaEmpleados.BackColor = SystemColors.Info;
                    }

                    pbFotoUser.ImageLocation = u.Foto;
                    lbl_foto.Text = u.Foto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lbl_idemp.Text != "")
            {
                habilitar();
                edit = true;
            }
            else
            {
                MessageBox.Show("No hay ningun empleado para editar");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_idemp.Text != "")
                {
                    DialogResult dialogResult = MessageBox.Show("Esta seguro de eliminar al Empleado: " + txt_nombre.Text, "Eliminar Empleado", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        TipoDoc tipodoc = (TipoDoc)cmb_tipodoc.SelectedItem;
                        TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp.SelectedItem;
                        CentroDeCostos cent = (CentroDeCostos)cmb_centrocostos.SelectedItem;
                        Empleados emp = new Empleados(Convert.ToInt32(lbl_idemp.Text), Convert.ToInt32(txt_legajo.Text), Convert.ToInt32(txt_documento.Text), txt_nombre.Text, txt_domicilio.Text, lbl_foto.Text, tipodoc, tipoemp, cent, 1);
                        controlemp.Borrar(emp);
                        MessageBox.Show("Empleado Borrado Correctamente");
                        txt_empleadohorario.Text = "Buscar Horarios Empleado para Eliminar";
                        lbl_idemp.Text = "";
                        dataGridView4.Rows.Clear();
                        dataGridView4.Refresh();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                    }
                }
                else
                {
                    MessageBox.Show("No hay ningun empleado para eliminar");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                limpiar();
                deshabilitar();
                lbl_idemp.Text = "";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lbl_idemp.Text = "";
            txt_empleadohorario.Text = "Buscar Horarios Empleado para Eliminar";
            dataGridView4.Rows.Clear();
            dataGridView4.Refresh();
            limpiar();
            deshabilitar();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                txt_contrasena.UseSystemPasswordChar = true;
            }
            else
            {
                txt_contrasena.UseSystemPasswordChar = false;
            }
        }

        private void tabCtrlMain_Selected(object sender, TabControlEventArgs e)
        {
            //if (tabCtrlMain.SelectedIndex == 0 && usuarioActivo.Usuario == "supervisor")
            //{
            //    cmb_usuarios.DataSource = controlusu.TraerTodos();
            //    cmb_usuarios.DisplayMember = "usuario";
            //    cmb_usuarios.ValueMember = "idusuarios";
            //    cmb_usuarios.SelectedIndex = 0;
            //}
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmb_usuarios.Text != "" && txt_contrasena.Text != "")
                {
                    DialogResult dialogResult = MessageBox.Show("Esta seguro de cambiar la Contraseña del Usuario: " + cmb_usuarios.Text, "Cambio Contraseña", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Usuarios usuario = (Usuarios)cmb_usuarios.SelectedItem;
                        usuario.Contrasena = txt_contrasena.Text;
                        controlusu.Modificar(usuario);
                        MessageBox.Show("Contraseña modificada correctamente");
                        txt_contrasena.Text = "";

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        txt_contrasena.Text = "";
                        //do something else
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar un Usuario y una nueva Contraseña para poder modificarla");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Feriados f = new Feriados(0, Convert.ToDateTime(maskedTextBox1.Text), txt_feriado.Text);
                controlfer.Agregar(f);
                listaferiados = controlfer.TraerTodos();
                foreach (Feriados aux in listaferiados)
                {
                    monthCalendar1.AddBoldedDate(aux.Fecha);
                }
                monthCalendar1.UpdateBoldedDates();
                txt_feriado.Clear();
                maskedTextBox1.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            bool ok = false;
            foreach (Feriados aux in listaferiados)
            {
                if (aux.Fecha == monthCalendar1.SelectionRange.Start)
                {
                    txt_feriado.Text = aux.Detalle;
                    maskedTextBox1.Text = aux.Fecha.ToShortDateString();
                    lbl_idferiado.Text = aux.Idferiados.ToString();
                    ok = true;
                    break;
                }
            }
            if (!ok)
            {
                txt_feriado.Text = "";
                maskedTextBox1.Text = monthCalendar1.SelectionRange.Start.ToString();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_idferiado.Text != "")
                {
                    Feriados f = new Feriados(Convert.ToInt32(lbl_idferiado.Text), Convert.ToDateTime(maskedTextBox1.Text), "");
                    controlfer.Borrar(f);
                    monthCalendar1.RemoveAllBoldedDates();
                    listaferiados = null;
                    listaferiados = controlfer.TraerTodos();
                    foreach (Feriados aux in listaferiados)
                    {
                        monthCalendar1.AddBoldedDate(aux.Fecha);
                    }
                    monthCalendar1.UpdateBoldedDates();
                    txt_feriado.Clear();
                    maskedTextBox1.Clear();
                    lbl_idferiado.Text = "";
                    MessageBox.Show("Feriado Eliminado correctamente");
                }
                else
                {
                    MessageBox.Show("Debe elegir un feriado en el calendario para Eliminarlo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var fileName = String.Empty;
            try
            {
                if (FuenteDeVideo.IsRunning)
                {
                    TerminarFuenteDeVideo();
                }
                if (pbFotoUser.Image != null)
                {
                    using (var dlg = new SaveFileDialog())
                    {
                        dlg.FileName = txt_nombre.Text + " " + txt_documento.Text;

                        Acceso_BD oacceso = new Acceso_BD();
                        DataTable dt = oacceso.leerDatos("select detalle from configuraciones where codigo = 'fotos'");

                        foreach (DataRow dr in dt.Rows)
                        {
                            dlg.InitialDirectory = Convert.ToString(dr["detalle"]);
                        }
                        dlg.DefaultExt = "png";
                        dlg.Filter = "PNG Files (*.png)|*.png|SVG Files (*.svg)|*.svg|BMP Files (*.bmp)|*.bmp|TIFF Files (*.tif)|*.tif|JPG Files (*.jpg)|*.jpg|All Files (*.*)|*.*";
                        if (dlg.ShowDialog(this) != DialogResult.OK)
                            return;
                        fileName = dlg.FileName;
                    }
                    var extension = Path.GetExtension(fileName).ToLower();
                    var bmp = (Bitmap)pbFotoUser.Image;
                    switch (extension)
                    {
                        case ".bmp":
                            bmp.Save(fileName, ImageFormat.Bmp);
                            break;
                        case ".jpeg":
                        case ".jpg":
                            bmp.Save(fileName, ImageFormat.Jpeg);
                            break;
                        case ".tiff":
                        case ".tif":
                            bmp.Save(fileName, ImageFormat.Tiff);
                            break;
                        case ".svg":
                            {
                                var writer = new BarcodeWriterSvg
                                {
                                    Format = (BarcodeFormat)cmbEncoderType.SelectedItem,
                                    Options = EncodingOptions ?? new EncodingOptions
                                    {
                                        Height = picEncodedBarCode.Height,
                                        Width = picEncodedBarCode.Width
                                    }
                                };
                                var svgImage = writer.Write(txtEncoderContent.Text);
                                File.WriteAllText(fileName, svgImage.Content, System.Text.Encoding.UTF8);
                            }
                            break;
                        default:
                            bmp.Save(fileName, ImageFormat.Png);
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                pbFotoUser.Refresh();
                pbFotoUser.ImageLocation = fileName;
                lbl_foto.Text = fileName;
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {

            if (ExistenDispositivos)
            {
                FuenteDeVideo = new VideoCaptureDevice(DispositivosDeVideo[cboDispositivos.SelectedIndex].MonikerString);
                FuenteDeVideo.NewFrame += new NewFrameEventHandler(video_NuevoFrame);
                FuenteDeVideo.Start();
                cboDispositivos.Enabled = false;

                //gbMenu.Text = DispositivosDeVideo[cboDispositivos.SelectedIndex].Name.ToString();


            }
            else
                MessageBox.Show("Error: No se encuentra dispositivo.");
        }



        private void button11_Click(object sender, EventArgs e)
        {
            using (var openDlg = new OpenFileDialog())
            {
                openDlg.Multiselect = false;
                if (openDlg.ShowDialog(this) == DialogResult.OK)
                {
                    lbl_foto.Text = openDlg.FileName;
                }
            }
            try
            {
                if (FuenteDeVideo != null)
                {
                    FuenteDeVideo.SignalToStop();
                    FuenteDeVideo = null;
                }
                var fileName = lbl_foto.Text;
                if (!File.Exists(fileName))
                {
                    MessageBox.Show(this, String.Format("Archivo no encontrado: {0}", fileName), "Error", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
                pbFotoUser.Refresh();
                pbFotoUser.ImageLocation = fileName;
                if (lbl_foto.Text == "")
                {
                    lbl_foto.Text = fileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            TerminarFuenteDeVideo();
            pbFotoUser.ImageLocation = null;
        }



        private void button13_Click(object sender, EventArgs e)
        {
            txtContentWebCam.Clear();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Buscar_Empleado frm = new Buscar_Empleado();
            frm.ShowDialog();
            Empleados u = frm.u;
            if (u != null)
            {
                lbl_idem.Text = u.Idempleados.ToString();
                txt_doc1.Text = u.Documento.ToString();
                txt_empleado1.Text = u.Nombre.ToString();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_doc1.Text != "")
                {
                    Acceso_BD oacceso = new Acceso_BD();
                    int mes = comboBox1.SelectedIndex;
                    mes = mes + 1;
                    if (oacceso.Tipo == "sql")
                    {
                        dataGridView2.DataSource = oacceso.leerDatos("exec sp_registro " + txt_doc1.Text + ", " + mes + "," + txt_ano.Text);
                    }
                    else
                    {
                        dataGridView2.DataSource = oacceso.leerDatos("call sp_registro(" + txt_doc1.Text + ", " + mes + "," + txt_ano.Text + ")");
                    }
                    txt_empleado1.Text = dataGridView2[0, 0].Value.ToString();
                    dataGridView2.Columns[0].Visible = false;
                    dataGridView2.Columns[4].Visible = false;
                    dataGridView2.Columns[5].Visible = false;
                    TimeSpan ht = new TimeSpan(0, 0, 0);
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        string x = Convert.ToString(row.Cells["ht"].Value);
                        if (x != "")
                        {
                            string o = x;
                            string h = x.Remove(2, 6);
                            x = x.Remove(0, 3);
                            string m = x.Remove(2, 3);
                            x = x.Remove(0, 3);
                            string s = x;
                            TimeSpan mas = new TimeSpan(Convert.ToInt32(h), Convert.ToInt32(m), Convert.ToInt32(s));
                            ht = ht.Add(mas);

                        }
                    }
                    int hora = ht.Days * 24 + ht.Hours;

                    lbl_ht.Text = "Total Horas Trabajadas: " + hora.ToString() + "h:" + ht.Minutes.ToString() + "m:" + ht.Seconds.ToString() + "s";
                    //MessageBox.Show(ht);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int filaseleccionada = Convert.ToInt32(this.dataGridView1.CurrentRow.Index);
            string foto = dataGridView1[4, filaseleccionada].Value.ToString();
            pictureBox3.ImageLocation = foto;
        }

        private void tabPageRegistros_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int i = 0;
                if (txt_doc1.Text != "")
                {
                    dataGridView1.Rows.Clear();
                    List<Registros> list = new List<Registros>();
                    int filaseleccionada = Convert.ToInt32(this.dataGridView2.CurrentRow.Index);
                    DateTime de = Convert.ToDateTime(dataGridView2[4, filaseleccionada].Value);
                    DateTime h = de;
                    h = h.AddDays(1);
                    //list = controlreg.BuscarEspecial("e.documento = '" + txt_doc1.Text + "' and r.registro between '" + dataGridView2[4, filaseleccionada].Value.ToString() + "' and '" + dataGridView2[4, filaseleccionada + 1].Value.ToString() + "'");
                    Acceso_BD oacceso = new Acceso_BD();
                    if (oacceso.Tipo == "sql")
                    {
                        list = controlreg.BuscarEspecial("e.documento = '" + txt_doc1.Text + "' and r.fechareal between '" + de.ToString("dd/MM/yyyy HH:mm:ss") + "' and '" + h.ToString("dd/MM/yyyy HH:mm:ss") + "'");
                    }
                    else
                    {
                        list = controlreg.BuscarEspecial("e.documento = '" + txt_doc1.Text + "' and r.fechareal between '" + de.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + h.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                    }
                    foreach (Registros aux in list)
                    {
                        i++;
                    }
                    int x = 0;
                    if (i > 0)
                    {
                        dataGridView1.Rows.Add(i);
                        foreach (Registros aux in list)
                        {
                            dataGridView1.Rows[x].Cells[0].Value = aux.Nombre;
                            DateTime d = Convert.ToDateTime(aux.Registro);
                            dataGridView1.Rows[x].Cells[1].Value = d.ToString("dd/MM/yyyy HH:mm:ss");
                            dataGridView1.Rows[x].Cells[2].Value = aux.Idregistros;
                            dataGridView1.Rows[x].Cells[3].Value = aux.Idempleados;
                            dataGridView1.Rows[x].Cells[4].Value = aux.Foto;
                            txt_empleado1.Text = aux.Nombre;
                            x++;
                        }
                    }
                }
                if (i > 0)
                {
                    string foto = dataGridView1[4, 0].Value.ToString();
                    pictureBox3.ImageLocation = foto;
                }
                else
                {
                    pictureBox3.Image = null;
                    txt_empleado1.Text = "";
                }
                dataGridView1.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txt_doc1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            txt_empleado1.Clear();
            pictureBox3.Image = null;
        }

        private void btn_imprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_doc1.Text != "")
                {
                    Document document = new Document();
                    DateTime fecha = DateTime.Now;
                    string fe = "Registros " + comboBox1.Text + " " + txt_empleado1.Text + ".pdf";
                    Acceso_BD oacceso = new Acceso_BD();
                    DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                    string root = "";
                    foreach (DataRow dr in dt1.Rows)
                    {
                        root = Convert.ToString(dr["detalle"]);
                    }
                    if (File.Exists(root))
                    {
                    }
                    else
                    {
                        root = Environment.CurrentDirectory;
                    }



                    PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                    document.Open();
                    DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                    string empresa = "";
                    foreach (DataRow dr in dt12.Rows)
                    {
                        empresa = Convert.ToString(dr["detalle"]);
                    }
                    Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                    document.Add(new Paragraph(chunk));
                    document.Add(new Paragraph("                   "));
                    document.Add(new Paragraph("                   "));
                    chunk = new Chunk("Registro fichaje                                                ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    document.Add(new Paragraph(chunk));
                    chunk = new Chunk("Nombre: " + txt_empleado1.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    document.Add(new Paragraph(chunk));
                    chunk = new Chunk("DNI: " + txt_doc1.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    document.Add(new Paragraph(chunk));
                    chunk = new Chunk(lbl_ht.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    document.Add(new Paragraph(chunk));
                    document.Add(new Paragraph("                   "));
                    document.Add(new Paragraph("                   "));
                    int mes = comboBox1.SelectedIndex;
                    mes = mes + 1;
                    DataTable dt = null;
                    if (oacceso.Tipo == "sql")
                    {
                        dt = oacceso.leerDatos("exec sp_registro " + txt_doc1.Text + ", " + mes + "," + txt_ano.Text);
                    }
                    else
                    {
                        dt = oacceso.leerDatos("call sp_registro(" + txt_doc1.Text + ", " + mes + "," + txt_ano.Text + ")");
                    }
                    PdfPTable table = new PdfPTable(3);
                    iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                    iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                    PdfPCell cell = new PdfPCell(new Phrase("Registro Fichaje"));
                    cell.Colspan = 3;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                    table.AddCell(cell);
                    table.AddCell(new PdfPCell(new Phrase("Dia", fontH1)));
                    table.AddCell(new PdfPCell(new Phrase("Registros", fontH1)));
                    table.AddCell(new PdfPCell(new Phrase("Novedades", fontH1)));
                    table.WidthPercentage = 100;
                    float[] widths = new float[] { 1.5f, 2.8f, 2.2f };
                    table.SetWidths(widths);
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (oacceso.Tipo == "sql")
                        {
                            table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Día"]), fontH2)));
                        }
                        else
                        {
                            table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Dia"]), fontH2)));
                        }
                        table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Registro"]), fontH2)));
                        table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Novedades"]), fontH2)));
                    }
                    document.Add(table);
                    document.Close();
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    string pdfPath = root + fe;
                    //Path.Combine("C:\\Users\\Public\\Desktop", fe);
                    //Application.StartupPath
                    //MessageBox.Show(pdfPath);
                    proc.StartInfo.FileName = pdfPath;
                    proc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmb_tiponov_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_tiponov1.Items.Clear();
            int x = 0;
            foreach (TipoDeNovedades aux in listanov)
            {
                if (aux.Idtipodenovedad == Convert.ToInt32(cmb_tiponov.SelectedValue))
                {
                    cmb_tiponov1.DisplayMember = "detalle";
                    cmb_tiponov1.ValueMember = "idtiposdenovedades";
                    cmb_tiponov1.Items.Add(aux);
                    x++;
                }
            }
            if (x > 0)
            {
                cmb_tiponov1.SelectedIndex = 0;
                x = 0;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Buscar_Empleado frm = new Buscar_Empleado();
            frm.ShowDialog();
            empnov = frm.u;
            if (empnov != null)
            {
                lbl_idempnov.Text = empnov.Idempleados.ToString();
                txt_docnov.Text = empnov.Nombre.ToString();
            }
            cmb_anos_SelectedIndexChanged(sender, e);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                if (check_afectaemp.Checked)
                {
                    String t = txt_desdenov.Text;
                    t = t.Replace("/", "");
                    String tE = txt_hastanov.Text;
                    tE = tE.Replace("/", "");
                    if (t.Length < 8 && tE.Length < 8)
                    {
                        MessageBox.Show("Fechas invalidas");
                    }
                    else
                    {
                        if (txt_novedad.Text != "")
                        {
                            TipoDeNovedades tiponov = (TipoDeNovedades)cmb_tiponov1.SelectedItem;
                            Empleados em = new Empleados(0, 0, 0, "", "", "", null, null, null, 1);
                            Novedades nov = null;
                            if (tiponov == null)
                            {
                                nov = new Novedades(0, em, txt_desdenov.Text, txt_hastanov.Text, 0, txt_novedad.Text);
                            }
                            else
                            {
                                nov = new Novedades(0, em, txt_desdenov.Text, txt_hastanov.Text, tiponov.Idtipodenovedades, txt_novedad.Text);
                            }
                            controlnov.Agregar(nov);
                            MessageBox.Show("Novedad Cargada existosamente");
                            txt_docnov.Text = "Buscar un empleado para cargar novedad";
                            txt_novedad.Clear();
                            txt_desdenov.Clear();
                            txt_hastanov.Clear();
                            lbl_idempnov.Text = "";
                            cmb_mesesnov_SelectedIndexChanged(sender, e);
                            cmb_tiponov.SelectedValue = 0;
                        }
                        else
                        {
                            MessageBox.Show("Debe completar el campo novedad");
                        }
                    }
                }
                else
                {
                    String t = txt_desdenov.Text;
                    t = t.Replace("/", "");
                    String tE = txt_hastanov.Text;
                    tE = tE.Replace("/", "");
                    if (t.Length < 8 && tE.Length < 8)
                    {
                        MessageBox.Show("Fechas invalidas");
                    }
                    else
                    {
                        if (lbl_idempnov.Text != "" && txt_novedad.Text != "")
                        {
                            TipoDeNovedades tiponov = (TipoDeNovedades)cmb_tiponov1.SelectedItem;
                            Novedades nov = new Novedades(0, empnov, txt_desdenov.Text, txt_hastanov.Text, tiponov.Idtipodenovedades, txt_novedad.Text);
                            controlnov.Agregar(nov);
                            MessageBox.Show("Novedad Cargada existosamente");
                            txt_docnov.Text = "Buscar un empleado para cargar novedad";
                            txt_novedad.Clear();
                            txt_desdenov.Clear();
                            txt_hastanov.Clear();
                            label33.Text = "";
                            cmb_mesesnov_SelectedIndexChanged(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Debe completar los campos empleado y novedad");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void chk_empact_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_empact.Checked)
            {
                tabPageCargaEmpleados.BackColor = SystemColors.Info;
            }
            else
            {
                tabPageCargaEmpleados.BackColor = Color.LightCoral;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_rdesde.Text != "  /  /" && txt_rhasta.Text != "  /  /")
                {
                    Document document = new Document();
                    DateTime fecha = DateTime.Now;
                    string fe = "RegistrosGral " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                    Acceso_BD oacceso = new Acceso_BD();
                    DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                    string root = "";
                    foreach (DataRow dr in dt1.Rows)
                    {
                        root = Convert.ToString(dr["detalle"]);
                    }
                    if (File.Exists(root))
                    {
                    }
                    else
                    {
                        root = Environment.CurrentDirectory;
                    }
                    PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                    document.Open();
                    DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                    string empresa = "";
                    foreach (DataRow dr in dt12.Rows)
                    {
                        empresa = Convert.ToString(dr["detalle"]);
                    }
                    Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                    document.Add(new Paragraph(chunk));
                    document.Add(new Paragraph("                   "));
                    chunk = new Chunk("Registro Gral            Desde: " + txt_rdesde.Text + "       Hasta: " + txt_rhasta.Text + "                             ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    document.Add(new Paragraph(chunk));
                    //chunk = new Chunk("Nombre: " + txt_empleado1.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    //document.Add(new Paragraph(chunk));
                    //chunk = new Chunk("DNI: " + txt_doc1.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    //document.Add(new Paragraph(chunk));
                    document.Add(new Paragraph("                   "));
                    int mes = comboBox1.SelectedIndex;
                    mes = mes + 1;
                    DataTable dt = null;
                    if (oacceso.Tipo == "sql")
                    {
                        dt = oacceso.leerDatos("exec sp_registro1 '" + txt_rdesde.Text + "', '" + txt_rhasta.Text + "'");
                    }
                    else
                    {
                        DateTime d = Convert.ToDateTime(txt_rdesde.Text);
                        DateTime h = Convert.ToDateTime(txt_rhasta.Text);
                        if (chk_centrocosto.Checked)
                        {
                            CentroDeCostos cent = (CentroDeCostos)cmb_centrocostos1.SelectedItem;
                            if (chk_tipoemp.Checked)
                            {     
                                TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp1.SelectedItem;
                                dt = oacceso.leerDatos("call sp_registro1('" + d.ToString("yyyy-MM-dd") + "', '" + h.ToString("yyyy-MM-dd") + "','"+ cent.Idcentrodecostros+"','"+tipoemp.Idtipodeempleados+"')");
                            }
                            else
                            {
                                dt = oacceso.leerDatos("call sp_registro1('" + d.ToString("yyyy-MM-dd") + "', '" + h.ToString("yyyy-MM-dd") + "','" + cent.Idcentrodecostros + "','0')");
                            }
                        }
                        else
                        {
                            if (chk_tipoemp.Checked)
                            {
                                TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp1.SelectedItem;
                                dt = oacceso.leerDatos("call sp_registro1('" + d.ToString("yyyy-MM-dd") + "', '" + h.ToString("yyyy-MM-dd") + "','0','" + tipoemp.Idtipodeempleados + "')");
                            }
                            else
                            {
                                dt = oacceso.leerDatos("call sp_registro1('" + d.ToString("yyyy-MM-dd") + "', '" + h.ToString("yyyy-MM-dd") + "','0','0')");
                            }
                        }
                    }
                    PdfPTable table = new PdfPTable(3);
                    iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                    iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                    PdfPCell cell = new PdfPCell(new Phrase("Registro Fichaje"));
                    cell.Colspan = 3;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                    table.AddCell(cell);
                    table.AddCell(new PdfPCell(new Phrase("Dia", fontH1)));
                    table.AddCell(new PdfPCell(new Phrase("Registros", fontH1)));
                    table.AddCell(new PdfPCell(new Phrase("Novedades", fontH1)));
                    table.WidthPercentage = 100;
                    float[] widths = new float[] { 1.5f, 2.8f, 2.2f };
                    table.SetWidths(widths);
                    string pepe = "";
                    TimeSpan ht = new TimeSpan(0, 0, 0);
                    TimeSpan cero = new TimeSpan(0, 0, 0);
                    foreach (DataRow dr in dt.Rows)
                    {
                        string x = "";
                        if (pepe != Convert.ToString(dr["Empleado"]))
                        {
                            if (pepe != "")
                            {
                                if (ht != cero)
                                {
                                    int hora = ht.Days * 24 + ht.Hours;
                                    Chunk chunk1 = new Chunk("Total Horas Trabajadas del Empleado " + pepe + ": " + hora.ToString() + "h:" + ht.Minutes.ToString() + "m:" + ht.Seconds.ToString() + "s", FontFactory.GetFont("VERDANA", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                                    document.Add(new Paragraph(chunk1));
                                    document.Add(new Paragraph("                   "));
                                    ht = new TimeSpan(0, 0, 0);
                                }
                                document.Add(table);
                                document.NewPage();
                                table = new PdfPTable(3);
                                table.WidthPercentage = 100;
                                float[] widths1 = new float[] { 1.5f, 2.8f, 2.2f };
                                table.SetWidths(widths1);
                                table.AddCell(new PdfPCell(new Phrase("Dia", fontH1)));
                                table.AddCell(new PdfPCell(new Phrase("Registros", fontH1)));
                                table.AddCell(new PdfPCell(new Phrase("Novedades", fontH1)));
                            }
                            pepe = Convert.ToString(dr["Empleado"]);
                            PdfPCell cell1 = new PdfPCell(new Phrase(pepe));
                            cell1.Colspan = 3;
                            cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                            table.AddCell(cell1);
                            if (oacceso.Tipo == "sql")
                            {
                                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Día"]), fontH2)));
                            }
                            else
                            {
                                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Dia"]), fontH2)));
                            }
                            table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Registro"]), fontH2)));
                            table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Novedades"]), fontH2)));


                            x = Convert.ToString(dr["ht"]);
                            if (x != "")
                            {
                                string o = x;
                                string h = x.Remove(2, 6);
                                x = x.Remove(0, 3);
                                string m = x.Remove(2, 3);
                                x = x.Remove(0, 3);
                                string s = x;
                                TimeSpan mas = new TimeSpan(Convert.ToInt32(h), Convert.ToInt32(m), Convert.ToInt32(s));
                                ht = ht.Add(mas);
                            }


                        }
                        else
                        {
                            if (oacceso.Tipo == "sql")
                            {
                                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Día"]), fontH2)));
                            }
                            else
                            {
                                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Dia"]), fontH2)));
                            }
                            table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Registro"]), fontH2)));
                            table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Novedades"]), fontH2)));
                            x = Convert.ToString(dr["ht"]);
                            if (x != "")
                            {
                                string o = x;
                                string h = x.Remove(2, 6);
                                x = x.Remove(0, 3);
                                string m = x.Remove(2, 3);
                                x = x.Remove(0, 3);
                                string s = x;
                                TimeSpan mas = new TimeSpan(Convert.ToInt32(h), Convert.ToInt32(m), Convert.ToInt32(s));
                                ht = ht.Add(mas);
                            }
                        }


                    }
                    if (pepe != "")
                    {
                        if (ht != cero)
                        {
                            Chunk chunk1 = new Chunk("Total Horas Trabajadas del Empleado " + pepe + ": " + ht.ToString(), FontFactory.GetFont("VERDANA", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                            document.Add(new Paragraph(chunk1));
                            document.Add(new Paragraph("                   "));
                            ht = new TimeSpan(0, 0, 0);
                        }
                    }
                    document.Add(table);
                    document.Close();
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    string pdfPath = root + fe;
                    //Path.Combine("C:\\Users\\Public\\Desktop", fe);
                    //Application.StartupPath
                    //MessageBox.Show(pdfPath);
                    proc.StartInfo.FileName = pdfPath;
                    proc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void check_afectaemp_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void cmb_mesesnov_SelectedIndexChanged(object sender, EventArgs e)
        {
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();
            Acceso_BD oacceso = new Acceso_BD();
            int x = cmb_mesesnov.SelectedIndex;
            x = x + 1;
            if (lbl_idempnov.Text == "")
            {
                if (oacceso.Tipo == "sql")
                {
                    dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where DATEPART(month, desde) = '" + x + "' and DATEPART(year, desde) = '" + cmb_anos.Text + "' order by desde asc");
                }
                else
                {
                    dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where month(desde) = '" + x + "' and year(desde) = '" + cmb_anos.Text + "' order by desde asc");
                }
            }
            else
            {
                if (oacceso.Tipo == "sql")
                {
                    dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where n.idempleados = '" + lbl_idempnov.Text + "' and DATEPART(month, desde) = '" + x + "' and DATEPART(year, desde) = '" + cmb_anos.Text + "' order by desde asc");
                }
                else
                {
                    dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where n.idempleados = '" + lbl_idempnov.Text + "' and month(desde) = '" + x + "' and year(desde) = '" + cmb_anos.Text + "' order by desde asc");
                }
            }
            dataGridView3.Columns[0].Visible = false;
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int filaseleccionada = Convert.ToInt32(this.dataGridView3.CurrentRow.Index);
                int idnov = Convert.ToInt32(dataGridView3[0, filaseleccionada].Value);
                string novedad = dataGridView3[4, filaseleccionada].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("Esta seguro de eliminar la novedad: " + novedad, "Eliminar Novedad", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Novedades n = new Novedades(idnov, null, "", "", 1, "");
                    controlnov.Borrar(n);
                    MessageBox.Show("Novedad Eliminada Correctamente");
                    cmb_mesesnov_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmb_anos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                maskedTextBox2.Clear();
                maskedTextBox3.Clear();
                Acceso_BD oacceso = new Acceso_BD();
                int x = cmb_mesesnov.SelectedIndex;
                x = x + 1;
                if (lbl_idempnov.Text == "")
                {
                    if (oacceso.Tipo == "sql")
                    {
                        dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where DATEPART(month, desde) = '" + x + "' and DATEPART(year, desde) = '" + cmb_anos.Text + "' order by desde asc");
                    }
                    else
                    {
                        dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where month(desde) = '" + x + "' and year(desde) = '" + cmb_anos.Text + "' order by desde asc");
                    }
                }
                else
                {
                    if (oacceso.Tipo == "sql")
                    {
                        dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where n.idempleados = '" + lbl_idempnov.Text + "' and DATEPART(month, desde) = '" + x + "' and DATEPART(year, desde) = '" + cmb_anos.Text + "' order by desde asc");
                    }
                    else
                    {
                        dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where n.idempleados = '" + lbl_idempnov.Text + "' and month(desde) = '" + x + "' and year(desde) = '" + cmb_anos.Text + "' order by desde asc");
                    }
                }
                dataGridView3.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txt_desdenov_Validating(object sender, CancelEventArgs e)
        {
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView3 != null)
                {
                    Document document = new Document();
                    DateTime fecha = DateTime.Now;
                    string fe = "Novedades " + cmb_anos.Text + " " + cmb_mesesnov.Text + ".pdf";
                    Acceso_BD oacceso = new Acceso_BD();
                    DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'novedades'");
                    string root = "";
                    foreach (DataRow dr in dt1.Rows)
                    {
                        root = Convert.ToString(dr["detalle"]);
                    }
                    PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                    document.Open();
                    DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                    string empresa = "";
                    foreach (DataRow dr in dt12.Rows)
                    {
                        empresa = Convert.ToString(dr["detalle"]);
                    }
                    Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 25, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                    document.Add(new Paragraph(chunk));
                    document.Add(new Paragraph("                   "));
                    document.Add(new Paragraph("                   "));
                    chunk = new Chunk("Novedades                                                ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                    document.Add(new Paragraph(chunk));
                    if (maskedTextBox2.Text == "  /  /")
                    {
                        chunk = new Chunk("Mes: " + cmb_mesesnov.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        chunk = new Chunk("Año: " + cmb_anos.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                    }
                    else
                    {
                        chunk = new Chunk("Desde: " + maskedTextBox3.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        chunk = new Chunk("Hasta: " + maskedTextBox2.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                    }
                    document.Add(new Paragraph("                   "));
                    document.Add(new Paragraph("                   "));
                    int mes = comboBox1.SelectedIndex;
                    mes = mes + 1;

                    PdfPTable table = new PdfPTable(5);
                    iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                    iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                    PdfPCell cell = new PdfPCell(new Phrase("Novedades"));
                    cell.Colspan = 5;
                    cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                    table.AddCell(cell);
                    table.AddCell(new PdfPCell(new Phrase("Empleado", fontH1)));
                    table.AddCell(new PdfPCell(new Phrase("Desde", fontH1)));
                    table.AddCell(new PdfPCell(new Phrase("Hasta", fontH1)));
                    table.AddCell(new PdfPCell(new Phrase("Novedad", fontH1)));
                    table.AddCell(new PdfPCell(new Phrase("Tipo de Novedad", fontH1)));
                    table.WidthPercentage = 100;
                    float[] widths = new float[] { 2.2f, 1f, 1f, 2.4f, 1.5f };
                    table.SetWidths(widths);
                    foreach (DataGridViewRow dr in dataGridView3.Rows)
                    {
                        table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr.Cells[1].Value), fontH2)));
                        DateTime desde = Convert.ToDateTime(dr.Cells[2].Value);
                        table.AddCell(new PdfPCell(new Phrase(desde.ToString("dd/MM/yyyy"), fontH2)));
                        DateTime hasta = Convert.ToDateTime(dr.Cells[3].Value);
                        table.AddCell(new PdfPCell(new Phrase(hasta.ToString("dd/MM/yyyy"), fontH2)));
                        table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr.Cells[4].Value), fontH2)));
                        table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr.Cells[5].Value), fontH2)));
                    }
                    document.Add(table);
                    document.Close();
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    string pdfPath = root + fe;
                    //Path.Combine("C:\\Users\\Public\\Desktop", fe);
                    //Application.StartupPath
                    //MessageBox.Show(pdfPath);
                    proc.StartInfo.FileName = pdfPath;
                    proc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                Acceso_BD oacceso = new Acceso_BD();
                DateTime desde = Convert.ToDateTime(maskedTextBox3.Text);
                DateTime hasta = Convert.ToDateTime(maskedTextBox2.Text);
                if (desde <= hasta)
                {
                    if (lbl_idempnov.Text == "")
                    {
                        if (oacceso.Tipo == "sql")
                        {
                            dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where desde between '" + maskedTextBox3.Text + "' and '" + maskedTextBox2.Text + "' and hasta between '" + maskedTextBox3.Text + "' and '" + maskedTextBox2.Text + "' order by desde asc");
                        }
                        else
                        {
                            dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where desde between '" + desde.ToString("yyyy-MM-dd") + "' and '" + hasta.ToString("yyyy-MM-dd") + "' and hasta between '" + desde.ToString("yyyy-MM-dd") + "' and '" + hasta.ToString("yyyy-MM-dd") + "' order by desde asc");
                        }
                    }
                    else
                    {
                        if (oacceso.Tipo == "sql")
                        {
                            dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where n.idempleados = '" + lbl_idempnov.Text + "' and desde between '" + maskedTextBox3.Text + "' and '" + maskedTextBox2.Text + "' and hasta between '" + maskedTextBox3.Text + "' and '" + maskedTextBox2.Text + "' order by desde asc");
                        }
                        else
                        {
                            dataGridView3.DataSource = oacceso.leerDatos("select n.idnovedades as idnovedades, case when n.idempleados = 0 then 'Afecta todos' else e.nombre END AS Empleado, desde as Desde, hasta as Hasta, n.detalle as Novedad, concat(tn.detalle, ' ', t.detalle) as 'Tipo de Novedad' from novedades n left join empleados e on n.idempleados = e.idempleados inner join tiposdenovedades t on n.idtiposdenovedades = t.idtiposdenovedades inner join tipodenovedad tn on tn.idtiponovedad = t.idtipodenovedad where n.idempleados = '" + lbl_idempnov.Text + "' and desde between '" + desde.ToString("yyyy-MM-dd") + "' and '" + hasta.ToString("yyyy-MM-dd") + "' and hasta between '" + desde.ToString("yyyy-MM-dd") + "' and '" + hasta.ToString("yyyy-MM-dd") + "' order by desde asc");
                        }
                    }
                    dataGridView3.Columns[0].Visible = false;
                }
                else
                {
                    MessageBox.Show("La fecha ingresada en 'Hasta' debe ser mayor o igual a la ingresada en 'Desde'");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_impemp_Click(object sender, EventArgs e)
        {
            try
            {

                Document document = new Document();
                DateTime fecha = DateTime.Now;
                string fe = "EmpleadosGral " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                Acceso_BD oacceso = new Acceso_BD();
                DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                string root = "";
                foreach (DataRow dr in dt1.Rows)
                {
                    root = Convert.ToString(dr["detalle"]);
                }
                PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                document.Open();
                DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                string empresa = "";
                foreach (DataRow dr in dt12.Rows)
                {
                    empresa = Convert.ToString(dr["detalle"]);
                }
                Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                document.Add(new Paragraph(chunk));
                document.Add(new Paragraph("                   "));
                document.Add(new Paragraph("                   "));
                chunk = new Chunk("Registro Gral de Empleados al " + DateTime.Now.ToShortDateString() + " ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                document.Add(new Paragraph(chunk));
                //chunk = new Chunk("Nombre: " + txt_empleado1.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                //document.Add(new Paragraph(chunk));
                //chunk = new Chunk("DNI: " + txt_doc1.Text + "", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                //document.Add(new Paragraph(chunk));
                document.Add(new Paragraph("                   "));
                document.Add(new Paragraph("                   "));
                int mes = comboBox1.SelectedIndex;
                mes = mes + 1;
                DataTable dt = oacceso.leerDatos("SELECT legajo, documento, nombre, domicilio, c.detalle as centro from Empleados E inner join centrodecostos c on e.idcentrodecostos = c.idcentrodecostos order by c.detalle, nombre asc");
                PdfPTable table = new PdfPTable(5);
                iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                PdfPCell cell = new PdfPCell(new Phrase("Registro General de Empleados"));
                cell.Colspan = 5;
                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                table.AddCell(cell);
                table.AddCell(new PdfPCell(new Phrase("Legajo", fontH1)));
                table.AddCell(new PdfPCell(new Phrase("Documento", fontH1)));
                table.AddCell(new PdfPCell(new Phrase("Empleado", fontH1)));
                table.AddCell(new PdfPCell(new Phrase("Domicilio", fontH1)));
                table.AddCell(new PdfPCell(new Phrase("Centro", fontH1)));
                table.WidthPercentage = 100;
                float[] widths = new float[] { 1f, 1.2f, 3f, 2.8f, 2.2f };
                table.SetWidths(widths);
                foreach (DataRow dr in dt.Rows)
                {
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["legajo"]), fontH2)));
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["documento"]), fontH2)));
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["nombre"]), fontH2)));
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["domicilio"]), fontH2)));
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["centro"]), fontH2)));
                }
                document.Add(table);
                document.Close();
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                string pdfPath = root + fe;
                //Path.Combine("C:\\Users\\Public\\Desktop", fe);
                //Application.StartupPath
                //MessageBox.Show(pdfPath);
                proc.StartInfo.FileName = pdfPath;
                proc.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WindowsFormsDemoForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.E)
            {
                if (tabPageCargaEmpleados.BackColor == SystemColors.Info)
                {
                    tabPageCargaEmpleados.BackColor = SystemColors.ControlLight;
                }
                else
                {
                    tabPageCargaEmpleados.BackColor = SystemColors.Info;
                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_idemp.Text != "")
                {
                    maskedTextBox4.ValidatingType = typeof(System.DateTime);                    
                    maskedTextBox4.TypeValidationCompleted += new TypeValidationEventHandler(maskedTextBox4_TypeValidationCompleted);
                    maskedTextBox5.ValidatingType = typeof(System.DateTime);
                    maskedTextBox5.TypeValidationCompleted += new TypeValidationEventHandler(maskedTextBox5_TypeValidationCompleted);
                    if (maskedTextBox4.Text == "  /  /" && maskedTextBox5.Text == "  /  /" && maskedTextBox6.Text == "  :  :" || maskedTextBox4.Text == "  /  /" && maskedTextBox5.Text == "  /  /" || maskedTextBox4.Text == "  /  /" || maskedTextBox5.Text == "  /  /" || maskedTextBox5.Text == "  /  /" && maskedTextBox6.Text == "  :  :" || maskedTextBox6.Text == "  :  :")
                    {
                        MessageBox.Show("Completar todos los campos para poder registrar el fichaje");
                    }
                    else
                    {
                        Acceso_BD oacceso = new Acceso_BD();
                        DateTime fecha = Convert.ToDateTime(maskedTextBox4.Text);
                        DateTime fecha1 = Convert.ToDateTime(maskedTextBox5.Text);
                        oacceso.ActualizarBD("insert into registros(idempleados, registro, fechareal, manual) values('" + lbl_idemp.Text + "','" + fecha.ToString("yyyy-MM-dd") + " " + maskedTextBox6.Text + "','" + fecha1.ToString("yyyy-MM-dd") + " " + maskedTextBox6.Text + "','1')");
                        MessageBox.Show("Registro Ingresado correctamente");
                        maskedTextBox4.Clear();
                        maskedTextBox5.Clear();
                        maskedTextBox6.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Debe traer un empleado para poder cargar un registro");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void maskedTextBox4_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
            {
                toolTip1.ToolTipTitle = "Fecha Invalida";
                toolTip1.Show("El Formato Correcto es el siguiente: dd/mm/aaaa.", maskedTextBox4, 0, -20, 5000);
            }

        }

        private void maskedTextBox5_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
            {
                toolTip1.ToolTipTitle = "Fecha Invalida";
                toolTip1.Show("El Formato Correcto es el siguiente: dd/mm/aaaa.", maskedTextBox5, 0, -20, 5000);
            }
        }

        private void maskedTextBox6_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {
            Diaslaborales frm = new Diaslaborales();
            frm.Show();
        }

        private void rb_cortado_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txt_eg2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void txt_eg1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void txt_horas_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txt_ing2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void txt_ing1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void chk_noct_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void txt_descanso_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void maskedTextBox7_Validated(object sender, EventArgs e)
        {
            try
            {
                if (maskedTextBox7.Text != "  /  /")
                {
                    DateTime a = Convert.ToDateTime(maskedTextBox7.Text);
                    CultureInfo myCI = new CultureInfo("en-US");
                    CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
                    DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
                    Calendar myCal = myCI.Calendar;
                    if (myCal.GetWeekOfYear(a, myCWR, myFirstDOW) % 2 == 0)
                    {
                        TSemana.Text = "2";
                    }
                    else
                    {
                        TSemana.Text = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmb_centrocostos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPageCargaEmpleados_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            Diaslaborales frm = new Diaslaborales();
            frm.ShowDialog();
            RecargarHorarios();
        }

        private void tabPageNovedades_Click(object sender, EventArgs e)
        {

        }



        private void button24_Click(object sender, EventArgs e)
        {
            try
            {
                List<DiasLaborales> lili = new List<DiasLaborales>();
                dataGridView4.ColumnCount = 7;
                dataGridView4.Columns[0].Name = "IdHorariosLaborales";
                dataGridView4.Columns[1].Name = "Empleado";
                dataGridView4.Columns[2].Name = "Desde";
                dataGridView4.Columns[3].Name = "Hasta";
                dataGridView4.Columns[4].Name = "Semana";
                dataGridView4.Columns[5].Name = "Horario";
                dataGridView4.Columns[6].Name = "idhorario";
                dataGridView4.Columns[0].Visible = false;
                dataGridView4.Columns[6].Visible = false;
                dataGridView4.Rows.Clear();
                dataGridView4.Refresh();
                if (lbl_idemp.Text == "")
                {
                    MessageBox.Show("Debe seleccionar un empleado en la solapa 'EMPLEADOS' para poder buscar horarios");
                }
                else
                {
                    lili = controldia.BuscarEspecial(lbl_idemp.Text);
                    int i = 0;
                    foreach (DiasLaborales aux in lili)
                    {
                        i++;
                    }
                    int x = 0;
                    if (i > 0)
                    {
                        dataGridView4.Rows.Add(i);
                        foreach (DiasLaborales aux in lili)
                        {
                            dataGridView4.Rows[x].Cells[0].Value = aux.Id;
                            dataGridView4.Rows[x].Cells[1].Value = aux.Empleado.Nombre;
                            dataGridView4.Rows[x].Cells[2].Value = aux.Desde.ToShortDateString();
                            dataGridView4.Rows[x].Cells[5].Value = aux.Horario.Detalle;
                            dataGridView4.Rows[x].Cells[6].Value = aux.Horario.Idhorarios;
                            if (aux.Horario.Horario == "1")
                            {
                                dataGridView4.Rows[x].Cells[5].Value = dataGridView4.Rows[x].Cells[5].Value + ", Corrido, " + aux.Horario.Ingreso1 + " - " + aux.Horario.Egreso1 + " - " + aux.Horario.Ingreso2 + " - " + aux.Horario.Egreso2;
                            }
                            else
                            {
                                dataGridView4.Rows[x].Cells[5].Value = dataGridView4.Rows[x].Cells[5].Value + ", Cortado, " + aux.Horario.Ingreso1 + " - " + aux.Horario.Egreso1 + " - " + aux.Horario.Ingreso2 + " - " + aux.Horario.Egreso2;
                            }
                            if (aux.Horario.Lunes == "1")
                            {
                                dataGridView4.Rows[x].Cells[5].Value = dataGridView4.Rows[x].Cells[5].Value + ", Lunes";
                            }
                            if(aux.Horario.Martes == "1")
                            {
                                dataGridView4.Rows[x].Cells[5].Value = dataGridView4.Rows[x].Cells[5].Value + ", Martes";
                            }
                            if (aux.Horario.Miercoles == "1")
                            {
                                dataGridView4.Rows[x].Cells[5].Value = dataGridView4.Rows[x].Cells[5].Value + ", Miercoles";
                            }
                            if (aux.Horario.Jueves == "1")
                            {
                                dataGridView4.Rows[x].Cells[5].Value = dataGridView4.Rows[x].Cells[5].Value + ", Jueves";
                            }
                            if (aux.Horario.Viernes == "1")
                            {
                                dataGridView4.Rows[x].Cells[5].Value = dataGridView4.Rows[x].Cells[5].Value + ", Viernes";
                            }
                            if (aux.Horario.Sabado == "1")
                            {
                                dataGridView4.Rows[x].Cells[5].Value = dataGridView4.Rows[x].Cells[5].Value + ", Sabado";
                            }
                            if (aux.Horario.Domingo == "1")
                            {
                                dataGridView4.Rows[x].Cells[5].Value = dataGridView4.Rows[x].Cells[5].Value + ", Domingo";
                            }
                            if (Convert.ToString(aux.Hasta.ToShortDateString()) == "01/01/1900")
                            {
                                dataGridView4.Rows[x].Cells[3].Value = "";
                            }
                            else
                            {
                                dataGridView4.Rows[x].Cells[3].Value = aux.Hasta.ToShortDateString();
                            }
                            if (aux.Semana == "0")
                            {
                                dataGridView4.Rows[x].Cells[4].Value = "Todas";
                            }
                            else if (aux.Semana == "1")
                            {
                                dataGridView4.Rows[x].Cells[4].Value = "Impar";
                            }
                            else if (aux.Semana == "2")
                            {
                                dataGridView4.Rows[x].Cells[4].Value = "Par";
                            }

                            x++;
                        }
                    }
                }
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
                int idhorario = Convert.ToInt32(dataGridView4[0, filaseleccionada].Value);
                string Detalle = dataGridView4[1, filaseleccionada].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("Esta seguro de eliminar el horario del empleado: " + Detalle, "Eliminar Horario Empleado", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DiasLaborales n = new DiasLaborales(idhorario, null, null, "", DateTime.Now, DateTime.Now);
                    controldia.Borrar(n);
                    MessageBox.Show("Horario Eliminado Correctamente");
                    dataGridView4.Rows.Clear();
                    dataGridView4.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_idemp.Text != "")
                {
                    Empleados eq = controlemp.Buscar(lbl_idemp.Text);
                    string id = cmb_horarios.SelectedValue.ToString();
                    Horarios hori = new Horarios(Convert.ToInt32(id), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                    maskedTextBox9.ValidatingType = typeof(System.DateTime);
                    maskedTextBox9.TypeValidationCompleted += new TypeValidationEventHandler(maskedTextBox5_TypeValidationCompleted);
                    if (maskedTextBox9.Text == "  /  /")
                    {
                        DiasLaborales didi = new DiasLaborales(0, hori, eq, TSemana.Text, Convert.ToDateTime(maskedTextBox8.Text));
                        if (!edito)
                        {
                            controldia.Agregar(didi);
                            TSemana.Enabled = false;
                            maskedTextBox7.Enabled = false;
                            maskedTextBox8.Enabled = false;
                            maskedTextBox9.Enabled = false;
                            dataGridView4.Rows.Clear();
                            dataGridView4.Refresh();
                        }
                        else
                        {
                            didi.Id = Convert.ToInt32(label23.Text);
                            controldia.Modificar(didi);
                            label23.Text = "";
                            edito = false;
                            TSemana.Enabled = false;
                            maskedTextBox7.Enabled = false;
                            maskedTextBox8.Enabled = false;
                            maskedTextBox9.Enabled = false;
                            dataGridView4.Rows.Clear();
                            dataGridView4.Refresh();
                        }
                    }
                    else
                    {
                        DiasLaborales didi = new DiasLaborales(0, hori, eq, TSemana.Text, Convert.ToDateTime(maskedTextBox8.Text), Convert.ToDateTime(maskedTextBox9.Text));
                        if (!edito)
                        {
                            controldia.Agregar(didi);
                            TSemana.Enabled = false;
                            maskedTextBox7.Enabled = false;
                            maskedTextBox8.Enabled = false;
                            maskedTextBox9.Enabled = false;
                            dataGridView4.Rows.Clear();
                            dataGridView4.Refresh();
                        }
                        else
                        {
                            didi.Id = Convert.ToInt32(label23.Text);
                            controldia.Modificar(didi);
                            label23.Text = "";
                            edito = false;
                            TSemana.Enabled = false;
                            maskedTextBox7.Enabled = false;
                            maskedTextBox8.Enabled = false;
                            maskedTextBox9.Enabled = false;
                            dataGridView4.Rows.Clear();
                            dataGridView4.Refresh();
                        }
                    }
                    MessageBox.Show("Horario del empleado cargado exitosamente");
                    maskedTextBox9.Clear();
                    maskedTextBox8.Clear();
                    maskedTextBox7.Clear();
                    TSemana.Text = "";
                }
                else
                {
                    MessageBox.Show("Para cargar un horario debe primero seleccionar un empleado en la solapa 'Empleado'");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            dataGridView5.Columns.Clear();
            dataGridView5.Refresh();
            if (rb_ultimosreg.Checked)
            {
                if (!chk_desdehasta.Checked)
                {
                    int centro = 0;
                    int tipoe = 0;
                    if (chk_centrocostos1.Checked)
                    {
                        CentroDeCostos centrocos = (CentroDeCostos)cmb_centrocostos2.SelectedItem;
                        centro = centrocos.Idcentrodecostros;
                    }
                    else
                    {
                        centro = 0;
                    }
                    if (chk_tipoemp1.Checked)
                    {
                        TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp2.SelectedItem;
                        tipoe = tipoemp.Idtipodeempleados;
                    }
                    else
                    {
                        tipoe = 0;
                    }
                    dataGridView5.DataSource = controlreg.TraerTodosEspecial(maskedTextBox10.Text, "", "", centro,tipoe);
                    dataGridView5.Columns[0].Visible = false;
                    dataGridView5.Columns[1].Visible = false;
                    dataGridView5.Columns[3].Visible = false;
                    dataGridView5.Columns[5].Visible = false;
                    dataGridView5.Columns[6].Visible = false;
                }
                else
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        int centro = 0;
                        int tipoe = 0;
                        if (chk_centrocostos1.Checked)
                        {
                            CentroDeCostos centrocos = (CentroDeCostos)cmb_centrocostos2.SelectedItem;
                            centro = centrocos.Idcentrodecostros;
                        }
                        else
                        {
                            centro = 0;
                        }
                        if (chk_tipoemp1.Checked)
                        {
                            TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp2.SelectedItem;
                            tipoe = tipoemp.Idtipodeempleados;
                        }
                        else
                        {
                            tipoe = 0;
                        }
                        DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                        DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                        h = h.AddDays(1);
                        dataGridView5.DataSource = controlreg.TraerTodosEspecial(maskedTextBox10.Text, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"),centro, tipoe);
                        dataGridView5.Columns[0].Visible = false;
                        dataGridView5.Columns[1].Visible = false;
                        dataGridView5.Columns[3].Visible = false;
                        dataGridView5.Columns[5].Visible = false;
                        dataGridView5.Columns[6].Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Debe completar correctamente los campos desde y hasta");
                    }
                }
            }
            else if (rb_erroresfichado.Checked)
            {
                if (!chk_desdehasta.Checked)
                {
                    int centro = 0;
                    int tipoe = 0;
                    if (chk_centrocostos1.Checked)
                    {
                        CentroDeCostos centrocos = (CentroDeCostos)cmb_centrocostos2.SelectedItem;
                        centro = centrocos.Idcentrodecostros;
                    }
                    else
                    {
                        centro = 0;
                    }
                    if (chk_tipoemp1.Checked)
                    {
                        TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp2.SelectedItem;
                        tipoe = tipoemp.Idtipodeempleados;
                    }
                    else
                    {
                        tipoe = 0;
                    }
                    dataGridView5.DataSource = controlreg.TraerMalFichados(maskedTextBox10.Text, "", "",centro,tipoe);
                    dataGridView5.Columns[0].Visible = false;
                    dataGridView5.Columns[1].Visible = false;
                    dataGridView5.Columns[3].Visible = false;
                    dataGridView5.Columns[5].Visible = false;
                    dataGridView5.Columns[6].Visible = false;
                }
                else
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                        DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                        h = h.AddDays(1);
                        int centro = 0;
                        int tipoe = 0;
                        if (chk_centrocostos1.Checked)
                        {
                            CentroDeCostos centrocos = (CentroDeCostos)cmb_centrocostos2.SelectedItem;
                            centro = centrocos.Idcentrodecostros;
                        }
                        else
                        {
                            centro = 0;
                        }
                        if (chk_tipoemp1.Checked)
                        {
                            TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp2.SelectedItem;
                            tipoe = tipoemp.Idtipodeempleados;
                        }
                        else
                        {
                            tipoe = 0;
                        }
                        dataGridView5.DataSource = controlreg.TraerMalFichados(maskedTextBox10.Text, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"),centro,tipoe);
                        dataGridView5.Columns[0].Visible = false;
                        dataGridView5.Columns[1].Visible = false;
                        dataGridView5.Columns[3].Visible = false;
                        dataGridView5.Columns[5].Visible = false;
                        dataGridView5.Columns[6].Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Debe completar correctamente los campos desde y hasta");
                    }
                }
            }            
        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int filaseleccionada = Convert.ToInt32(this.dataGridView5.CurrentRow.Index);
            string foto = dataGridView5[3, filaseleccionada].Value.ToString();
            pictureBox4.ImageLocation = foto;
        }

        private void dataGridView5_KeyUp(object sender, KeyEventArgs e)
        {
            int filaseleccionada = Convert.ToInt32(this.dataGridView5.CurrentRow.Index);
            string foto = dataGridView5[3, filaseleccionada].Value.ToString();
            pictureBox4.ImageLocation = foto;
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ////////////////////
            try
            {
                TSemana.Enabled = false;
                maskedTextBox7.Enabled = false;
                maskedTextBox8.Enabled = false;
                maskedTextBox9.Enabled = false;
                int filaseleccionada = Convert.ToInt32(this.dataGridView4.CurrentRow.Index);
                int idhorario = Convert.ToInt32(dataGridView4[0, filaseleccionada].Value);
                label23.Text = idhorario.ToString();
                cmb_horarios.SelectedValue = Convert.ToInt32(dataGridView4[6, filaseleccionada].Value);
                maskedTextBox8.Text = Convert.ToString(dataGridView4[2, filaseleccionada].Value);
                maskedTextBox9.Text = Convert.ToString(dataGridView4[3, filaseleccionada].Value);
                if (Convert.ToString(dataGridView4[4, filaseleccionada].Value) == "Par")
                {
                    TSemana.Text = "2";
                }
                else if (Convert.ToString(dataGridView4[4, filaseleccionada].Value) == "Impar")
                {
                    TSemana.Text = "1";
                }
                else
                {
                    TSemana.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (label23.Text != "")
            {
                TSemana.Enabled = true;
                maskedTextBox7.Enabled = true;
                maskedTextBox8.Enabled = true;
                maskedTextBox9.Enabled = true;
                edito = true;
            }
            else
            {
                MessageBox.Show("Primero debe seleccionar una fila para editar ");
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            TSemana.Enabled = true;
            maskedTextBox7.Enabled = true;
            maskedTextBox8.Enabled = true;
            maskedTextBox9.Enabled = true;
            label23.Text = "";
            maskedTextBox9.Clear();
            maskedTextBox8.Clear();
            maskedTextBox7.Clear();
            TSemana.Text = "";
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtmotivo.Text != "" && rb_ultimosreg.Checked)
                {
                    int filaseleccionada = Convert.ToInt32(this.dataGridView5.CurrentRow.Index);
                    Acceso_BD oacceso = new Acceso_BD();
                    DialogResult dialogResult = MessageBox.Show("Esta seguro de anular el registro del empleado: " + dataGridView5[4, filaseleccionada].Value.ToString() + " con fecha y hora: " + dataGridView5[2, filaseleccionada].Value.ToString(), "Anular Registro", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        oacceso.ActualizarBD("update registros set estado = '0', motivo = '" + txtmotivo.Text + "' where idregistros = '" + dataGridView5[0, filaseleccionada].Value.ToString() + "'");
                        txtmotivo.Text = "";
                        dataGridView5.Columns.Clear();
                        dataGridView5.Refresh();
                    }
                }
                else
                {
                    MessageBox.Show("Debe completar el campo motivo para poder anular un registro");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView5.Columns.Clear();
            dataGridView5.Refresh();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView5.Columns.Clear();
            dataGridView5.Refresh();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            txt_docnov.Text = "Buscar un empleado para cargar novedad";
            txt_novedad.Clear();
            txt_desdenov.Clear();
            txt_hastanov.Clear();
            lbl_idempnov.Text = "";
            cmb_anos_SelectedIndexChanged(sender, e);
            cmb_tiponov.SelectedValue = 0;
        }

        

        private void button31_Click(object sender, EventArgs e)
        {
            label47.Visible = true;            
            Application.DoEvents();
            try
            {
                if (rb_ausencias.Checked)
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        
                        Document document = new Document();
                        DateTime fecha = DateTime.Now;
                        string fe = "AusenciasGral " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                        Acceso_BD oacceso = new Acceso_BD();
                        DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                        string root = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            root = Convert.ToString(dr["detalle"]);
                        }
                        if (File.Exists(root))
                        {
                        }
                        else
                        {
                            root = Environment.CurrentDirectory;
                        }
                        PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                        document.Open();
                        DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                        string empresa = "";
                        foreach (DataRow dr in dt12.Rows)
                        {
                            empresa = Convert.ToString(dr["detalle"]);
                        }
                        Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        chunk = new Chunk("Ausencias Gral            Desde: " + maskedTextBox12.Text + "       Hasta: " + maskedTextBox11.Text + "                             ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        int mes = comboBox1.SelectedIndex;
                        mes = mes + 1;                        
                        DataTable dt = null;
                        if (oacceso.Tipo == "sql")
                        {
                            dt = oacceso.leerDatos("exec sp_registro1 '" + maskedTextBox12.Text + "', '" + maskedTextBox11.Text + "'");
                        }
                        else
                        {
                            DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                            DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                            if (chk_centrocostos1.Checked)
                            {
                                CentroDeCostos cent = (CentroDeCostos)cmb_centrocostos2.SelectedItem;
                                if (chk_tipoemp.Checked)
                                {
                                    TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp2.SelectedItem;
                                    dt = oacceso.leerDatos("call sp_ausencias('" + d.ToString("yyyy-MM-dd") + "', '" + h.ToString("yyyy-MM-dd") + "','"+cent.Idcentrodecostros+"','"+tipoemp.Idtipodeempleados+"')");
                                }
                                else
                                {
                                    dt = oacceso.leerDatos("call sp_ausencias('" + d.ToString("yyyy-MM-dd") + "', '" + h.ToString("yyyy-MM-dd") + "','" + cent.Idcentrodecostros + "','0')");
                                }
                            }
                            else
                            {
                                if (chk_tipoemp1.Checked)
                                {
                                    TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp2.SelectedItem;
                                    dt = oacceso.leerDatos("call sp_ausencias('" + d.ToString("yyyy-MM-dd") + "', '" + h.ToString("yyyy-MM-dd") + "','0','" + tipoemp.Idtipodeempleados + "')");
                                }
                                else
                                {
                                    dt = oacceso.leerDatos("call sp_ausencias('" + d.ToString("yyyy-MM-dd") + "', '" + h.ToString("yyyy-MM-dd") + "','0','0')");
                                }
                            }
                        }
                        PdfPTable table = new PdfPTable(2);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                        iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                        PdfPCell cell = new PdfPCell(new Phrase("Registro Ausencias"));
                        cell.Colspan = 2;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                        table.AddCell(cell);
                        table.AddCell(new PdfPCell(new Phrase("Dia", fontH1)));
                        table.AddCell(new PdfPCell(new Phrase("Registros", fontH1)));
                        table.WidthPercentage = 100;
                        float[] widths = new float[] { 2f, 3f };
                        table.SetWidths(widths);
                        string pepe = "";
                        TimeSpan ht = new TimeSpan(0, 0, 0);
                        TimeSpan cero = new TimeSpan(0, 0, 0);
                        foreach (DataRow dr in dt.Rows)
                        {                           
                            if (pepe != Convert.ToString(dr["Empleado"]))
                            {
                                if (pepe != "")
                                {
                                    if (ht != cero)
                                    {
                                        int hora = ht.Days * 24 + ht.Hours;
                                        Chunk chunk1 = new Chunk("Total Horas Trabajadas del Empleado " + pepe + ": " + hora.ToString() + "h:" + ht.Minutes.ToString() + "m:" + ht.Seconds.ToString() + "s", FontFactory.GetFont("VERDANA", 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                                        document.Add(new Paragraph(chunk1));
                                        document.Add(new Paragraph("                   "));
                                        ht = new TimeSpan(0, 0, 0);
                                    }
                                    document.Add(table);
                                    table = new PdfPTable(2);
                                    table.WidthPercentage = 100;
                                    float[] widths1 = new float[] { 2f, 3f };
                                    table.SetWidths(widths1);
                                }
                                pepe = Convert.ToString(dr["Empleado"]);
                                PdfPCell cell1 = new PdfPCell(new Phrase(pepe));
                                cell1.Colspan = 2;
                                cell1.BackgroundColor = iTextSharp.text.BaseColor.YELLOW;
                                cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                                table.AddCell(cell1);
                                if (oacceso.Tipo == "sql")
                                {
                                    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Día"]), fontH2)));
                                }
                                else
                                {
                                    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Dia"]), fontH2)));
                                }
                                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Registro"]), fontH2)));
                            }
                            else
                            {
                                if (oacceso.Tipo == "sql")
                                {
                                    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Día"]), fontH2)));
                                }
                                else
                                {
                                    table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Dia"]), fontH2)));
                                }
                                table.AddCell(new PdfPCell(new Phrase(Convert.ToString(dr["Registro"]), fontH2)));
                            }
                        }
                        document.Add(table);
                        document.Close();                        
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        string pdfPath = root + fe;
                        proc.StartInfo.FileName = pdfPath;
                        proc.Start();
                        label47.Visible = false;
                    }
                }
                else if (rb_erroresfichado.Checked)
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        Document document = new Document();
                        DateTime fecha = DateTime.Now;
                        string fe = "ErroresFichado " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                        Acceso_BD oacceso = new Acceso_BD();
                        DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                        string root = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            root = Convert.ToString(dr["detalle"]);
                        }
                        if (File.Exists(root))
                        {
                        }
                        else
                        {
                            root = Environment.CurrentDirectory;
                        }
                        PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                        document.Open();
                        DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                        string empresa = "";
                        foreach (DataRow dr in dt12.Rows)
                        {
                            empresa = Convert.ToString(dr["detalle"]);
                        }
                        Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        chunk = new Chunk("Errores de Fichado            Desde: " + maskedTextBox12.Text + "       Hasta: " + maskedTextBox11.Text + "                             ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        int mes = comboBox1.SelectedIndex;
                        mes = mes + 1;
                        DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                        DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                        h = h.AddDays(1);
                        int centro = 0;
                        int tipoe = 0;
                        if (chk_centrocostos1.Checked)
                        {
                            CentroDeCostos centrocos = (CentroDeCostos)cmb_centrocostos2.SelectedItem;
                            centro = centrocos.Idcentrodecostros;
                        }
                        else
                        {
                            centro = 0;
                        }
                        if (chk_tipoemp1.Checked)
                        {
                            TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp2.SelectedItem;
                            tipoe = tipoemp.Idtipodeempleados;
                        }
                        else
                        {
                            tipoe = 0;
                        }
                        List<Registros> lo = controlreg.TraerMalFichados(maskedTextBox10.Text, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"), centro, tipoe);                       
                        PdfPTable table = new PdfPTable(1);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                        iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                        PdfPCell cell = new PdfPCell(new Phrase("Errores de Fichado"));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                        table.AddCell(cell);
                        table.WidthPercentage = 100;
                        //float[] widths = new float[] { 2f, 3f };
                        //table.SetWidths(widths);
                        string pepe = "";
                        TimeSpan ht = new TimeSpan(0, 0, 0);
                        TimeSpan cero = new TimeSpan(0, 0, 0);
                        Empleados exe = null;
                        foreach (Registros aux in lo)
                        {
                            document.Add(table);
                            exe = controlemp.Buscar(aux.Idempleados.ToString());
                            pepe = exe.Nombre;
                            table = new PdfPTable(2);
                            table.WidthPercentage = 100;
                            PdfPCell cell1 = new PdfPCell(new Phrase(pepe));
                            cell1.Colspan = 1;
                            cell1.BackgroundColor = iTextSharp.text.BaseColor.YELLOW;
                            cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell1);
                            PdfPCell cell2 = new PdfPCell(new Phrase(aux.Registro));
                            cell2.Colspan = 1;
                            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell2);
                        }
                        document.Add(table);
                        document.Close();
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        string pdfPath = root + fe;
                        proc.StartInfo.FileName = pdfPath;
                        proc.Start();
                        label47.Visible = false;
                    }
                }
                else if (rb_ultimosreg.Checked)
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        Document document = new Document();
                        DateTime fecha = DateTime.Now;
                        string fe = "UltimosReg " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                        Acceso_BD oacceso = new Acceso_BD();
                        DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                        string root = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            root = Convert.ToString(dr["detalle"]);
                        }
                        if (File.Exists(root))
                        {
                        }
                        else
                        {
                            root = Environment.CurrentDirectory;
                        }
                        PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                        document.Open();
                        DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                        string empresa = "";
                        foreach (DataRow dr in dt12.Rows)
                        {
                            empresa = Convert.ToString(dr["detalle"]);
                        }
                        Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        chunk = new Chunk("Ultimos Registros            Desde: " + maskedTextBox12.Text + "       Hasta: " + maskedTextBox11.Text + "                             ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        int mes = comboBox1.SelectedIndex;
                        mes = mes + 1;
                        DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                        DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                        h = h.AddDays(1);
                        int centro = 0;
                        int tipoe = 0;
                        if (chk_centrocostos1.Checked)
                        {
                            CentroDeCostos centrocos = (CentroDeCostos)cmb_centrocostos2.SelectedItem;
                            centro = centrocos.Idcentrodecostros;
                        }
                        else
                        {
                            centro = 0;
                        }
                        if (chk_tipoemp1.Checked)
                        {
                            TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp2.SelectedItem;
                            tipoe = tipoemp.Idtipodeempleados;
                        }
                        else
                        {
                            tipoe = 0;
                        }
                        List<Registros> lo = controlreg.TraerTodosEspecial(maskedTextBox10.Text, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"), centro, tipoe);
                        PdfPTable table = new PdfPTable(1);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                        iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                        PdfPCell cell = new PdfPCell(new Phrase("Ultimos Registros"));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                        table.AddCell(cell);
                        table.WidthPercentage = 100;
                        //float[] widths = new float[] { 2f, 3f };
                        //table.SetWidths(widths);
                        string pepe = "";
                        TimeSpan ht = new TimeSpan(0, 0, 0);
                        TimeSpan cero = new TimeSpan(0, 0, 0);
                        Empleados exe = null;
                        foreach (Registros aux in lo)
                        {
                            document.Add(table);
                            exe = controlemp.Buscar(aux.Idempleados.ToString());
                            pepe = exe.Nombre;
                            table = new PdfPTable(2);
                            table.WidthPercentage = 100;
                            PdfPCell cell1 = new PdfPCell(new Phrase(pepe));
                            cell1.Colspan = 1;
                            cell1.BackgroundColor = iTextSharp.text.BaseColor.YELLOW;
                            cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell1);
                            PdfPCell cell2 = new PdfPCell(new Phrase(aux.Registro));
                            cell2.Colspan = 1;
                            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell2);
                        }
                        document.Add(table);
                        document.Close();
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        string pdfPath = root + fe;
                        proc.StartInfo.FileName = pdfPath;
                        proc.Start();
                        label47.Visible = false;
                    }
                }
                else if (rb_fichajesmanu.Checked)
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        Document document = new Document();
                        DateTime fecha = DateTime.Now;
                        string fe = "Fichajes Manuales " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                        Acceso_BD oacceso = new Acceso_BD();
                        DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                        string root = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            root = Convert.ToString(dr["detalle"]);
                        }
                        if (File.Exists(root))
                        {
                        }
                        else
                        {
                            root = Environment.CurrentDirectory;
                        }
                        PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                        document.Open();
                        DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                        string empresa = "";
                        foreach (DataRow dr in dt12.Rows)
                        {
                            empresa = Convert.ToString(dr["detalle"]);
                        }
                        Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        chunk = new Chunk("Fichajes Manuales            Desde: " + maskedTextBox12.Text + "       Hasta: " + maskedTextBox11.Text + "                             ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        int mes = comboBox1.SelectedIndex;
                        mes = mes + 1;
                        DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                        DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                        h = h.AddDays(1);
                        int centro = 0;
                        int tipoe = 0;
                        if (chk_centrocostos1.Checked)
                        {
                            CentroDeCostos centrocos = (CentroDeCostos)cmb_centrocostos2.SelectedItem;
                            centro = centrocos.Idcentrodecostros;
                        }
                        else
                        {
                            centro = 0;
                        }
                        if (chk_tipoemp1.Checked)
                        {
                            TipoDeEmpleados tipoemp = (TipoDeEmpleados)cmb_tipoemp2.SelectedItem;
                            tipoe = tipoemp.Idtipodeempleados;
                        }
                        else
                        {
                            tipoe = 0;
                        }
                        List<Registros> lo = controlreg.TraerFichajesManuales(maskedTextBox10.Text, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"), centro, tipoe);
                        PdfPTable table = new PdfPTable(1);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                        iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                        PdfPCell cell = new PdfPCell(new Phrase("Fichajes Manuales"));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                        table.AddCell(cell);
                        table.WidthPercentage = 100;
                        //float[] widths = new float[] { 2f, 3f };
                        //table.SetWidths(widths);
                        string pepe = "";
                        TimeSpan ht = new TimeSpan(0, 0, 0);
                        TimeSpan cero = new TimeSpan(0, 0, 0);
                        foreach (Registros aux in lo)
                        {
                            document.Add(table);
                            pepe = aux.Nombre;
                            table = new PdfPTable(2);
                            table.WidthPercentage = 100;
                            PdfPCell cell1 = new PdfPCell(new Phrase(pepe));
                            cell1.Colspan = 1;
                            cell1.BackgroundColor = iTextSharp.text.BaseColor.YELLOW;
                            cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell1);
                            PdfPCell cell2 = new PdfPCell(new Phrase(aux.Registro));
                            cell2.Colspan = 1;
                            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell2);
                        }
                        document.Add(table);
                        document.Close();
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        string pdfPath = root + fe;
                        proc.StartInfo.FileName = pdfPath;
                        proc.Start();
                        label47.Visible = false;
                    }
                }
                else if (rb_fichajeanu.Checked)
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        Document document = new Document();
                        DateTime fecha = DateTime.Now;
                        string fe = "Fichajes Anulados " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                        Acceso_BD oacceso = new Acceso_BD();
                        DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                        string root = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            root = Convert.ToString(dr["detalle"]);
                        }
                        if (File.Exists(root))
                        {
                        }
                        else
                        {
                            root = Environment.CurrentDirectory;
                        }
                        PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                        document.Open();
                        DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                        string empresa = "";
                        foreach (DataRow dr in dt12.Rows)
                        {
                            empresa = Convert.ToString(dr["detalle"]);
                        }
                        Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        chunk = new Chunk("Fichajes Anulados            Desde: " + maskedTextBox12.Text + "       Hasta: " + maskedTextBox11.Text + "                             ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        int mes = comboBox1.SelectedIndex;
                        mes = mes + 1;
                        DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                        DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                        h = h.AddDays(1);
                        List<Registros> lo = controlreg.TraerFichajesAnulados(maskedTextBox10.Text, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"));
                        PdfPTable table = new PdfPTable(1);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                        iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                        PdfPCell cell = new PdfPCell(new Phrase("Fichajes Anulados"));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                        table.AddCell(cell);
                        table.WidthPercentage = 100;
                        //float[] widths = new float[] { 2f, 3f };
                        //table.SetWidths(widths);
                        string pepe = "";
                        TimeSpan ht = new TimeSpan(0, 0, 0);
                        TimeSpan cero = new TimeSpan(0, 0, 0);
                        foreach (Registros aux in lo)
                        {
                            document.Add(table);
                            pepe = aux.Nombre;
                            table = new PdfPTable(3);
                            table.WidthPercentage = 100;
                            PdfPCell cell1 = new PdfPCell(new Phrase(pepe));
                            cell1.Colspan = 1;
                            cell1.BackgroundColor = iTextSharp.text.BaseColor.YELLOW;
                            cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell1);
                            PdfPCell cell2 = new PdfPCell(new Phrase(aux.Registro));
                            cell2.Colspan = 1;
                            cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell2);
                            PdfPCell cell3 = new PdfPCell(new Phrase(aux.Foto));
                            cell3.Colspan = 1;
                            cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell3);
                        }
                        document.Add(table);
                        document.Close();
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        string pdfPath = root + fe;
                        proc.StartInfo.FileName = pdfPath;
                        proc.Start();
                        label47.Visible = false;
                    }
                }


                else if (rb_llegatardeTol.Checked)
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        Document document = new Document();
                        DateTime fecha = DateTime.Now;
                        string fe = "Llegadas tarde " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                        Acceso_BD oacceso = new Acceso_BD();
                        DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                        string root = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            root = Convert.ToString(dr["detalle"]);
                        }
                        if (File.Exists(root))
                        {
                        }
                        else
                        {
                            root = Environment.CurrentDirectory;
                        }
                        PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                        document.Open();
                        DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                        string empresa = "";
                        foreach (DataRow dr in dt12.Rows)
                        {
                            empresa = Convert.ToString(dr["detalle"]);
                        }
                        Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        chunk = new Chunk("Llegadas tarde            Desde: " + maskedTextBox12.Text + "       Hasta: " + maskedTextBox11.Text + "                             ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        int mes = comboBox1.SelectedIndex;
                        mes = mes + 1;
                        DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                        DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                        //h = h.AddDays(1);
                        List<Registros> lo = controlreg.TraerLlegadasTarde(maskedTextBox10.Text, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"),1,1);
                        PdfPTable table = new PdfPTable(1);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                        iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                        PdfPCell cell = new PdfPCell(new Phrase("Llegadas tarde Detallado Con Tolerancia"));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                        table.AddCell(cell);
                        table.WidthPercentage = 100;
                        //float[] widths = new float[] { 2f, 3f };
                        //table.SetWidths(widths);
                        string pepe = "";
                        TimeSpan ht = new TimeSpan(0, 0, 0);
                        TimeSpan cero = new TimeSpan(0, 0, 0);
                        document.Add(table);
                        table = new PdfPTable(3);
                        table.WidthPercentage = 100;
                        PdfPCell cell1 = new PdfPCell(new Phrase("Empleado"));
                        cell1.Colspan = 1;
                        cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell1);
                        PdfPCell cell2 = new PdfPCell(new Phrase("Dia"));
                        cell2.Colspan = 1;
                        cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell2);
                        PdfPCell cell3 = new PdfPCell(new Phrase("Minutos Tarde"));
                        cell3.Colspan = 1;
                        cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell3);
                        foreach (Registros aux in lo)
                        {
                            document.Add(table);
                            pepe = aux.Nombre;
                            table = new PdfPTable(3);
                            table.WidthPercentage = 100;
                            PdfPCell cell4 = new PdfPCell(new Phrase(pepe));
                            cell4.Colspan = 1;
                            cell4.BackgroundColor = iTextSharp.text.BaseColor.YELLOW;
                            cell4.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell4);
                            PdfPCell cell5 = new PdfPCell(new Phrase(aux.Registro));
                            cell5.Colspan = 1;
                            cell5.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell5);
                            PdfPCell cell6 = new PdfPCell(new Phrase(aux.Foto));
                            cell6.Colspan = 1;
                            cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell6);
                        }
                        document.Add(table);
                        document.Close();
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        string pdfPath = root + fe;
                        proc.StartInfo.FileName = pdfPath;
                        proc.Start();
                        label47.Visible = false;
                    }
                }

                else if (rb_llegatardeStol.Checked)
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        Document document = new Document();
                        DateTime fecha = DateTime.Now;
                        string fe = "Llegadas tarde " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                        Acceso_BD oacceso = new Acceso_BD();
                        DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                        string root = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            root = Convert.ToString(dr["detalle"]);
                        }
                        if (File.Exists(root))
                        {
                        }
                        else
                        {
                            root = Environment.CurrentDirectory;
                        }
                        PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                        document.Open();
                        DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                        string empresa = "";
                        foreach (DataRow dr in dt12.Rows)
                        {
                            empresa = Convert.ToString(dr["detalle"]);
                        }
                        Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        chunk = new Chunk("Llegadas tarde            Desde: " + maskedTextBox12.Text + "       Hasta: " + maskedTextBox11.Text + "                             ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        int mes = comboBox1.SelectedIndex;
                        mes = mes + 1;
                        DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                        DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                        //h = h.AddDays(1);
                        List<Registros> lo = controlreg.TraerLlegadasTarde(maskedTextBox10.Text, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"), 1, 0);
                        PdfPTable table = new PdfPTable(1);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                        iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                        PdfPCell cell = new PdfPCell(new Phrase("Llegadas tarde Detallado Sin Tolerancia"));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                        table.AddCell(cell);
                        table.WidthPercentage = 100;
                        //float[] widths = new float[] { 2f, 3f };
                        //table.SetWidths(widths);
                        string pepe = "";
                        TimeSpan ht = new TimeSpan(0, 0, 0);
                        TimeSpan cero = new TimeSpan(0, 0, 0);
                        document.Add(table);
                        table = new PdfPTable(3);
                        table.WidthPercentage = 100;
                        PdfPCell cell1 = new PdfPCell(new Phrase("Empleado"));
                        cell1.Colspan = 1;
                        cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell1);
                        PdfPCell cell2 = new PdfPCell(new Phrase("Dia"));
                        cell2.Colspan = 1;
                        cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell2);
                        PdfPCell cell3 = new PdfPCell(new Phrase("Minutos Tarde"));
                        cell3.Colspan = 1;
                        cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell3);
                        foreach (Registros aux in lo)
                        {
                            document.Add(table);
                            pepe = aux.Nombre;
                            table = new PdfPTable(3);
                            table.WidthPercentage = 100;
                            PdfPCell cell4 = new PdfPCell(new Phrase(pepe));
                            cell4.Colspan = 1;
                            cell4.BackgroundColor = iTextSharp.text.BaseColor.YELLOW;
                            cell4.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell4);
                            PdfPCell cell5 = new PdfPCell(new Phrase(aux.Registro));
                            cell5.Colspan = 1;
                            cell5.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell5);
                            PdfPCell cell6 = new PdfPCell(new Phrase(aux.Foto));
                            cell6.Colspan = 1;
                            cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell6);
                        }
                        document.Add(table);
                        document.Close();
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        string pdfPath = root + fe;
                        proc.StartInfo.FileName = pdfPath;
                        proc.Start();
                        label47.Visible = false;
                    }
                }

                else if (rb_llegatarde1Tol.Checked)
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        Document document = new Document();
                        DateTime fecha = DateTime.Now;
                        string fe = "Llegadas tarde " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                        Acceso_BD oacceso = new Acceso_BD();
                        DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                        string root = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            root = Convert.ToString(dr["detalle"]);
                        }
                        if (File.Exists(root))
                        {
                        }
                        else
                        {
                            root = Environment.CurrentDirectory;
                        }
                        PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                        document.Open();
                        DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                        string empresa = "";
                        foreach (DataRow dr in dt12.Rows)
                        {
                            empresa = Convert.ToString(dr["detalle"]);
                        }
                        Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        chunk = new Chunk("Llegadas tarde            Desde: " + maskedTextBox12.Text + "       Hasta: " + maskedTextBox11.Text + "                             ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        int mes = comboBox1.SelectedIndex;
                        mes = mes + 1;
                        DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                        DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                       //h = h.AddDays(1);
                        List<Registros> lo = controlreg.TraerLlegadasTarde(maskedTextBox10.Text, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"), 0, 1);
                        PdfPTable table = new PdfPTable(1);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                        iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                        PdfPCell cell = new PdfPCell(new Phrase("Llegadas tarde Acumulado Con Tolerancia"));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                        table.AddCell(cell);
                        table.WidthPercentage = 100;
                        //float[] widths = new float[] { 2f, 3f };
                        //table.SetWidths(widths);
                        string pepe = "";
                        TimeSpan ht = new TimeSpan(0, 0, 0);
                        TimeSpan cero = new TimeSpan(0, 0, 0);
                        document.Add(table);
                        table = new PdfPTable(3);
                        table.WidthPercentage = 100;
                        PdfPCell cell1 = new PdfPCell(new Phrase("Empleado"));
                        cell1.Colspan = 1;
                        cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell1);
                        PdfPCell cell2 = new PdfPCell(new Phrase("Cantidad Llegadas Tarde"));
                        cell2.Colspan = 1;
                        cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell2);
                        PdfPCell cell3 = new PdfPCell(new Phrase("Minutos Tarde Acumulado"));
                        cell3.Colspan = 1;
                        cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell3);
                        foreach (Registros aux in lo)
                        {
                            document.Add(table);
                            pepe = aux.Nombre;
                            table = new PdfPTable(3);
                            table.WidthPercentage = 100;
                            PdfPCell cell4 = new PdfPCell(new Phrase(pepe));
                            cell4.Colspan = 1;
                            cell4.BackgroundColor = iTextSharp.text.BaseColor.YELLOW;
                            cell4.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell4);
                            PdfPCell cell5 = new PdfPCell(new Phrase(aux.Registro));
                            cell5.Colspan = 1;
                            cell5.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell5);
                            PdfPCell cell6 = new PdfPCell(new Phrase(aux.Foto));
                            cell6.Colspan = 1;
                            cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell6);
                        }
                        document.Add(table);
                        document.Close();
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        string pdfPath = root + fe;
                        proc.StartInfo.FileName = pdfPath;
                        proc.Start();
                        label47.Visible = false;
                    }
                }

                else if (rb_llegatarde1STol.Checked)
                {
                    if (maskedTextBox12.Text != "  /  /" && maskedTextBox11.Text != "  /  /")
                    {
                        Document document = new Document();
                        DateTime fecha = DateTime.Now;
                        string fe = "Llegadas tarde " + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".pdf";
                        Acceso_BD oacceso = new Acceso_BD();
                        DataTable dt1 = oacceso.leerDatos("select detalle from configuraciones where codigo = 'registros'");
                        string root = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            root = Convert.ToString(dr["detalle"]);
                        }
                        if (File.Exists(root))
                        {
                        }
                        else
                        {
                            root = Environment.CurrentDirectory;
                        }
                        PdfWriter.GetInstance(document, new FileStream(root + fe, FileMode.OpenOrCreate));
                        document.Open();
                        DataTable dt12 = oacceso.leerDatos("select * from configuraciones where codigo = 'empresa'");
                        string empresa = "";
                        foreach (DataRow dr in dt12.Rows)
                        {
                            empresa = Convert.ToString(dr["detalle"]);
                        }
                        Chunk chunk = new Chunk(empresa, FontFactory.GetFont("VERDANA", 30, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.DARK_GRAY));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        chunk = new Chunk("Llegadas tarde            Desde: " + maskedTextBox12.Text + "       Hasta: " + maskedTextBox11.Text + "                             ", FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.BOLD));
                        document.Add(new Paragraph(chunk));
                        document.Add(new Paragraph("                   "));
                        int mes = comboBox1.SelectedIndex;
                        mes = mes + 1;
                        DateTime d = Convert.ToDateTime(maskedTextBox12.Text);
                        DateTime h = Convert.ToDateTime(maskedTextBox11.Text);
                        //h = h.AddDays(1);
                        List<Registros> lo = controlreg.TraerLlegadasTarde(maskedTextBox10.Text, d.ToString("yyyy-MM-dd"), h.ToString("yyyy-MM-dd"), 0, 0);
                        PdfPTable table = new PdfPTable(1);
                        iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 9, iTextSharp.text.Font.BOLD));
                        iTextSharp.text.Font fontH2 = new iTextSharp.text.Font(FontFactory.GetFont("ARIAL", 10, iTextSharp.text.Font.NORMAL));
                        PdfPCell cell = new PdfPCell(new Phrase("Llegadas tarde Acumulado Sin Tolerancia"));
                        cell.Colspan = 1;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right 
                        table.AddCell(cell);
                        table.WidthPercentage = 100;
                        //float[] widths = new float[] { 2f, 3f };
                        //table.SetWidths(widths);
                        string pepe = "";
                        TimeSpan ht = new TimeSpan(0, 0, 0);
                        TimeSpan cero = new TimeSpan(0, 0, 0);
                        document.Add(table);
                        table = new PdfPTable(3);
                        table.WidthPercentage = 100;
                        PdfPCell cell1 = new PdfPCell(new Phrase("Empleado"));
                        cell1.Colspan = 1;
                        cell1.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell1);
                        PdfPCell cell2 = new PdfPCell(new Phrase("Cantidad Llegadas Tarde"));
                        cell2.Colspan = 1;
                        cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell2);
                        PdfPCell cell3 = new PdfPCell(new Phrase("Minutos Tarde Acumulado"));
                        cell3.Colspan = 1;
                        cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                        table.AddCell(cell3);
                        foreach (Registros aux in lo)
                        {
                            document.Add(table);
                            pepe = aux.Nombre;
                            table = new PdfPTable(3);
                            table.WidthPercentage = 100;
                            PdfPCell cell4 = new PdfPCell(new Phrase(pepe));
                            cell4.Colspan = 1;
                            cell4.BackgroundColor = iTextSharp.text.BaseColor.YELLOW;
                            cell4.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell4);
                            PdfPCell cell5 = new PdfPCell(new Phrase(aux.Registro));
                            cell5.Colspan = 1;
                            cell5.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell5);
                            PdfPCell cell6 = new PdfPCell(new Phrase(aux.Foto));
                            cell6.Colspan = 1;
                            cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right                             
                            table.AddCell(cell6);
                        }
                        document.Add(table);
                        document.Close();
                        System.Diagnostics.Process proc = new System.Diagnostics.Process();
                        string pdfPath = root + fe;
                        proc.StartInfo.FileName = pdfPath;
                        proc.Start();
                        label47.Visible = false;
                    }
                }
                if (label47.Visible)
                {
                    label47.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabCtrlMain_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}