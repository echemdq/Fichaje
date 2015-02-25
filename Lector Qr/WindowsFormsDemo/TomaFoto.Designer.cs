namespace WindowsFormsDemo
{
    partial class TomaFoto
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
            this.components = new System.ComponentModel.Container();
            this.pbFotoUser = new System.Windows.Forms.PictureBox();
            this.cboDispositivos = new System.Windows.Forms.ComboBox();
            this.timer5 = new System.Windows.Forms.Timer(this.components);
            this.timer6 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbFotoUser)).BeginInit();
            this.SuspendLayout();
            // 
            // pbFotoUser
            // 
            this.pbFotoUser.BackColor = System.Drawing.SystemColors.Info;
            this.pbFotoUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbFotoUser.Location = new System.Drawing.Point(90, 26);
            this.pbFotoUser.Name = "pbFotoUser";
            this.pbFotoUser.Size = new System.Drawing.Size(379, 331);
            this.pbFotoUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbFotoUser.TabIndex = 23;
            this.pbFotoUser.TabStop = false;
            // 
            // cboDispositivos
            // 
            this.cboDispositivos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDispositivos.FormattingEnabled = true;
            this.cboDispositivos.Location = new System.Drawing.Point(128, 280);
            this.cboDispositivos.Name = "cboDispositivos";
            this.cboDispositivos.Size = new System.Drawing.Size(182, 21);
            this.cboDispositivos.TabIndex = 26;
            this.cboDispositivos.Visible = false;
            // 
            // timer5
            // 
            this.timer5.Interval = 4500;
            this.timer5.Tick += new System.EventHandler(this.timer5_Tick);
            // 
            // timer6
            // 
            this.timer6.Interval = 1000;
            this.timer6.Tick += new System.EventHandler(this.timer6_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(1, 369);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(362, 32);
            this.label2.TabIndex = 27;
            this.label2.Text = "Se tomara una foto en:";
            // 
            // TomaFoto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(556, 405);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboDispositivos);
            this.Controls.Add(this.pbFotoUser);
            this.MaximizeBox = false;
            this.Name = "TomaFoto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Toma Foto";
            this.Load += new System.EventHandler(this.TomaFoto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbFotoUser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbFotoUser;
        private System.Windows.Forms.ComboBox cboDispositivos;
        private System.Windows.Forms.Timer timer5;
        private System.Windows.Forms.Timer timer6;
        private System.Windows.Forms.Label label2;
    }
}