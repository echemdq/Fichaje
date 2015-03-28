namespace WindowsFormsDemo
{
    partial class Diaslaborales
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chk_noct = new System.Windows.Forms.CheckBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.txt_eg2 = new System.Windows.Forms.MaskedTextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txt_eg1 = new System.Windows.Forms.MaskedTextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txt_horas = new System.Windows.Forms.MaskedTextBox();
            this.txt_ing2 = new System.Windows.Forms.MaskedTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txt_ing1 = new System.Windows.Forms.MaskedTextBox();
            this.rb_corrido = new System.Windows.Forms.RadioButton();
            this.rb_cortado = new System.Windows.Forms.RadioButton();
            this.label22 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txt_descanso = new System.Windows.Forms.MaskedTextBox();
            this.textTIdBox1 = new System.Windows.Forms.TextBox();
            this.Tdetalle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chk_noct
            // 
            this.chk_noct.AutoSize = true;
            this.chk_noct.Enabled = false;
            this.chk_noct.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.chk_noct.Location = new System.Drawing.Point(26, 166);
            this.chk_noct.Name = "chk_noct";
            this.chk_noct.Size = new System.Drawing.Size(138, 20);
            this.chk_noct.TabIndex = 64;
            this.chk_noct.Text = "Horario Nocturno";
            this.chk_noct.UseVisualStyleBackColor = true;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.SystemColors.Info;
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "Lunes",
            "Martes",
            "Miercoles",
            "Jueves",
            "Viernes",
            "Sabado",
            "Domingo"});
            this.checkedListBox1.Location = new System.Drawing.Point(198, 166);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(222, 126);
            this.checkedListBox1.TabIndex = 58;
            // 
            // txt_eg2
            // 
            this.txt_eg2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_eg2.Location = new System.Drawing.Point(515, 90);
            this.txt_eg2.Mask = "00:00";
            this.txt_eg2.Name = "txt_eg2";
            this.txt_eg2.Size = new System.Drawing.Size(55, 22);
            this.txt_eg2.TabIndex = 57;
            this.txt_eg2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_eg2.ValidatingType = typeof(System.DateTime);
            this.txt_eg2.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txt_eg2_MaskInputRejected);
            this.txt_eg2.TextChanged += new System.EventHandler(this.txt_eg2_TextChanged);
            this.txt_eg2.Validated += new System.EventHandler(this.txt_eg2_Validated);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(385, 94);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(124, 14);
            this.label23.TabIndex = 63;
            this.label23.Text = "Regreso Descanso";
            // 
            // txt_eg1
            // 
            this.txt_eg1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_eg1.Location = new System.Drawing.Point(515, 62);
            this.txt_eg1.Mask = "00:00";
            this.txt_eg1.Name = "txt_eg1";
            this.txt_eg1.Size = new System.Drawing.Size(55, 22);
            this.txt_eg1.TabIndex = 54;
            this.txt_eg1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_eg1.ValidatingType = typeof(System.DateTime);
            this.txt_eg1.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.txt_eg1_MaskInputRejected);
            this.txt_eg1.TextChanged += new System.EventHandler(this.txt_eg1_TextChanged);
            this.txt_eg1.Validated += new System.EventHandler(this.txt_eg1_Validated);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(195, 94);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(110, 14);
            this.label21.TabIndex = 62;
            this.label21.Text = "Salida Descanso";
            // 
            // txt_horas
            // 
            this.txt_horas.Enabled = false;
            this.txt_horas.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_horas.Location = new System.Drawing.Point(323, 122);
            this.txt_horas.Mask = "00:00";
            this.txt_horas.Name = "txt_horas";
            this.txt_horas.Size = new System.Drawing.Size(56, 22);
            this.txt_horas.TabIndex = 51;
            this.txt_horas.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_horas.ValidatingType = typeof(System.DateTime);
            // 
            // txt_ing2
            // 
            this.txt_ing2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ing2.Location = new System.Drawing.Point(323, 91);
            this.txt_ing2.Mask = "00:00";
            this.txt_ing2.Name = "txt_ing2";
            this.txt_ing2.Size = new System.Drawing.Size(56, 22);
            this.txt_ing2.TabIndex = 56;
            this.txt_ing2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_ing2.ValidatingType = typeof(System.DateTime);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(195, 125);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(110, 14);
            this.label19.TabIndex = 60;
            this.label19.Text = "Horas Laborales";
            // 
            // txt_ing1
            // 
            this.txt_ing1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ing1.Location = new System.Drawing.Point(323, 62);
            this.txt_ing1.Mask = "00:00";
            this.txt_ing1.Name = "txt_ing1";
            this.txt_ing1.Size = new System.Drawing.Size(56, 22);
            this.txt_ing1.TabIndex = 53;
            this.txt_ing1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_ing1.ValidatingType = typeof(System.DateTime);
            // 
            // rb_corrido
            // 
            this.rb_corrido.AutoSize = true;
            this.rb_corrido.Checked = true;
            this.rb_corrido.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.rb_corrido.Location = new System.Drawing.Point(26, 70);
            this.rb_corrido.Name = "rb_corrido";
            this.rb_corrido.Size = new System.Drawing.Size(123, 20);
            this.rb_corrido.TabIndex = 49;
            this.rb_corrido.TabStop = true;
            this.rb_corrido.Text = "Horario Corrido";
            this.rb_corrido.UseVisualStyleBackColor = true;
            this.rb_corrido.CheckedChanged += new System.EventHandler(this.rb_corrido_CheckedChanged);
            // 
            // rb_cortado
            // 
            this.rb_cortado.AutoSize = true;
            this.rb_cortado.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.rb_cortado.Location = new System.Drawing.Point(26, 96);
            this.rb_cortado.Name = "rb_cortado";
            this.rb_cortado.Size = new System.Drawing.Size(129, 20);
            this.rb_cortado.TabIndex = 50;
            this.rb_cortado.Text = "Horario Cortado";
            this.rb_cortado.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(385, 65);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(63, 14);
            this.label22.TabIndex = 55;
            this.label22.Text = "Egreso 1";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(195, 70);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(68, 14);
            this.label20.TabIndex = 61;
            this.label20.Text = "Ingreso 1";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(384, 125);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(68, 14);
            this.label18.TabIndex = 59;
            this.label18.Text = "Descanso";
            // 
            // txt_descanso
            // 
            this.txt_descanso.Enabled = false;
            this.txt_descanso.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_descanso.Location = new System.Drawing.Point(515, 122);
            this.txt_descanso.Mask = "00:00";
            this.txt_descanso.Name = "txt_descanso";
            this.txt_descanso.Size = new System.Drawing.Size(55, 22);
            this.txt_descanso.TabIndex = 52;
            this.txt_descanso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_descanso.ValidatingType = typeof(System.DateTime);
            // 
            // textTIdBox1
            // 
            this.textTIdBox1.Location = new System.Drawing.Point(271, 12);
            this.textTIdBox1.Name = "textTIdBox1";
            this.textTIdBox1.Size = new System.Drawing.Size(69, 20);
            this.textTIdBox1.TabIndex = 65;
            // 
            // Tdetalle
            // 
            this.Tdetalle.Location = new System.Drawing.Point(272, 36);
            this.Tdetalle.Name = "Tdetalle";
            this.Tdetalle.Size = new System.Drawing.Size(278, 20);
            this.Tdetalle.TabIndex = 66;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(23, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 16);
            this.label1.TabIndex = 68;
            this.label1.Text = "Id";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 16);
            this.label2.TabIndex = 69;
            this.label2.Text = "Detalle";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Info;
            this.button2.BackgroundImage = global::WindowsFormsDemo.Properties.Resources.Save;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button2.Location = new System.Drawing.Point(149, 323);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 50);
            this.button2.TabIndex = 71;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Diaslaborales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(597, 409);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Tdetalle);
            this.Controls.Add(this.textTIdBox1);
            this.Controls.Add(this.chk_noct);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.txt_eg2);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.txt_eg1);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.txt_horas);
            this.Controls.Add(this.txt_ing2);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.txt_ing1);
            this.Controls.Add(this.rb_corrido);
            this.Controls.Add(this.rb_cortado);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.txt_descanso);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Diaslaborales";
            this.Text = "Configuración Días Laborales";
            this.Load += new System.EventHandler(this.Diaslaborales_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chk_noct;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.MaskedTextBox txt_eg2;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.MaskedTextBox txt_eg1;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.MaskedTextBox txt_horas;
        private System.Windows.Forms.MaskedTextBox txt_ing2;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.MaskedTextBox txt_ing1;
        private System.Windows.Forms.RadioButton rb_corrido;
        private System.Windows.Forms.RadioButton rb_cortado;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.MaskedTextBox txt_descanso;
        private System.Windows.Forms.TextBox textTIdBox1;
        private System.Windows.Forms.TextBox Tdetalle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
    }
}