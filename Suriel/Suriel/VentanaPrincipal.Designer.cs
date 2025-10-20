namespace Suriel
{
    partial class VentanaPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            areaDeChat = new RichTextBox();
            cajaDeTextoMensaje = new TextBox();
            botonEnviar = new Button();
            checkboxModoBDPrivada = new CheckBox();
            labelModo = new Label();
            checkboxVozActivada = new CheckBox();
            barraVolumen = new TrackBar();
            labelVolumen = new Label();
            controladorDeAudio = new FlowLayoutPanel();
            contenedorDeModos = new FlowLayoutPanel();
            checkboxModoIABase = new CheckBox();
            checkboxModoSuriel = new CheckBox();
            flowLayoutPanel6 = new FlowLayoutPanel();
            contenedorHistorial = new FlowLayoutPanel();
            flowLayoutPanel4 = new FlowLayoutPanel();
            flowLayoutPanel5 = new FlowLayoutPanel();
            label1 = new Label();
            checkboxHistorial1 = new CheckBox();
            checkboxHistorial2 = new CheckBox();
            checkboxHistorial3 = new CheckBox();
            checkboxHistorial4 = new CheckBox();
            checkboxHistorial5 = new CheckBox();
            flowLayoutPanel7 = new FlowLayoutPanel();
            botonBorrarHistorial = new Button();
            contenedorTitulo = new FlowLayoutPanel();
            contenedorHerramientas = new FlowLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            botonCargarTxt = new Button();
            flowLayoutPanel3 = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)barraVolumen).BeginInit();
            controladorDeAudio.SuspendLayout();
            contenedorDeModos.SuspendLayout();
            contenedorHistorial.SuspendLayout();
            flowLayoutPanel5.SuspendLayout();
            contenedorHerramientas.SuspendLayout();
            SuspendLayout();
            // 
            // areaDeChat
            // 
            areaDeChat.BackColor = Color.White;
            areaDeChat.Location = new Point(155, 85);
            areaDeChat.Margin = new Padding(1, 3, 1, 3);
            areaDeChat.Name = "areaDeChat";
            areaDeChat.Size = new Size(405, 329);
            areaDeChat.TabIndex = 0;
            areaDeChat.Text = "";
            areaDeChat.TextChanged += areaDeChat_TextChanged;
            // 
            // cajaDeTextoMensaje
            // 
            cajaDeTextoMensaje.BackColor = Color.White;
            cajaDeTextoMensaje.Location = new Point(155, 420);
            cajaDeTextoMensaje.Margin = new Padding(1, 3, 3, 3);
            cajaDeTextoMensaje.Multiline = true;
            cajaDeTextoMensaje.Name = "cajaDeTextoMensaje";
            cajaDeTextoMensaje.Size = new Size(405, 75);
            cajaDeTextoMensaje.TabIndex = 1;
            cajaDeTextoMensaje.KeyDown += cajaDeTextoMensaje_KeyDown;
            // 
            // botonEnviar
            // 
            botonEnviar.BackColor = Color.White;
            botonEnviar.Font = new Font("Segoe UI", 14F);
            botonEnviar.Location = new Point(3, 336);
            botonEnviar.Name = "botonEnviar";
            botonEnviar.Size = new Size(126, 37);
            botonEnviar.TabIndex = 2;
            botonEnviar.Text = "Enviar";
            botonEnviar.UseVisualStyleBackColor = false;
            botonEnviar.Click += botonEnviar_Click;
            // 
            // checkboxModoBDPrivada
            // 
            checkboxModoBDPrivada.BackColor = Color.DarkGray;
            checkboxModoBDPrivada.Location = new Point(18, 53);
            checkboxModoBDPrivada.Name = "checkboxModoBDPrivada";
            checkboxModoBDPrivada.Size = new Size(105, 19);
            checkboxModoBDPrivada.TabIndex = 3;
            checkboxModoBDPrivada.Text = "BD Privada";
            checkboxModoBDPrivada.UseVisualStyleBackColor = false;
            checkboxModoBDPrivada.CheckedChanged += checkboxModoBDPrivada_CheckedChanged;
            // 
            // labelModo
            // 
            labelModo.Anchor = AnchorStyles.None;
            labelModo.BackColor = Color.DarkGray;
            labelModo.Font = new Font("Segoe UI", 14F);
            labelModo.Location = new Point(1, 0);
            labelModo.Name = "labelModo";
            labelModo.Size = new Size(122, 25);
            labelModo.TabIndex = 4;
            labelModo.Text = "Modos";
            labelModo.TextAlign = ContentAlignment.MiddleCenter;
            labelModo.Click += labelModo_Click;
            // 
            // checkboxVozActivada
            // 
            checkboxVozActivada.BackColor = Color.DarkGray;
            checkboxVozActivada.Font = new Font("Segoe UI", 15F);
            checkboxVozActivada.Location = new Point(111, 3);
            checkboxVozActivada.Margin = new Padding(0, 3, 2, 3);
            checkboxVozActivada.Name = "checkboxVozActivada";
            checkboxVozActivada.Size = new Size(13, 29);
            checkboxVozActivada.TabIndex = 5;
            checkboxVozActivada.UseVisualStyleBackColor = false;
            checkboxVozActivada.CheckedChanged += checkboxVozActivada_CheckedChanged_1;
            checkboxVozActivada.CheckStateChanged += checkboxVozActivada_CheckedChanged;
            // 
            // barraVolumen
            // 
            barraVolumen.BackColor = Color.DarkGray;
            barraVolumen.Location = new Point(3, 39);
            barraVolumen.Maximum = 100;
            barraVolumen.Name = "barraVolumen";
            barraVolumen.Size = new Size(116, 45);
            barraVolumen.TabIndex = 6;
            barraVolumen.Scroll += barraVolumen_Scroll;
            // 
            // labelVolumen
            // 
            labelVolumen.BackColor = Color.DarkGray;
            labelVolumen.Location = new Point(3, 4);
            labelVolumen.Margin = new Padding(3, 4, 0, 0);
            labelVolumen.Name = "labelVolumen";
            labelVolumen.Size = new Size(108, 32);
            labelVolumen.TabIndex = 7;
            labelVolumen.Text = "Reproducir audio de respuestas";
            labelVolumen.Click += labelVolumen_Click;
            // 
            // controladorDeAudio
            // 
            controladorDeAudio.BackColor = Color.DarkGray;
            controladorDeAudio.Controls.Add(labelVolumen);
            controladorDeAudio.Controls.Add(checkboxVozActivada);
            controladorDeAudio.Controls.Add(barraVolumen);
            controladorDeAudio.Location = new Point(3, 221);
            controladorDeAudio.Name = "controladorDeAudio";
            controladorDeAudio.Size = new Size(126, 70);
            controladorDeAudio.TabIndex = 8;
            controladorDeAudio.Paint += flowLayoutPanel1_Paint;
            // 
            // contenedorDeModos
            // 
            contenedorDeModos.BackColor = Color.DarkGray;
            contenedorDeModos.Controls.Add(labelModo);
            contenedorDeModos.Controls.Add(checkboxModoIABase);
            contenedorDeModos.Controls.Add(checkboxModoBDPrivada);
            contenedorDeModos.Controls.Add(checkboxModoSuriel);
            contenedorDeModos.FlowDirection = FlowDirection.RightToLeft;
            contenedorDeModos.Location = new Point(3, 78);
            contenedorDeModos.Name = "contenedorDeModos";
            contenedorDeModos.Size = new Size(126, 100);
            contenedorDeModos.TabIndex = 9;
            // 
            // checkboxModoIABase
            // 
            checkboxModoIABase.BackColor = Color.DarkGray;
            checkboxModoIABase.Location = new Point(18, 28);
            checkboxModoIABase.Name = "checkboxModoIABase";
            checkboxModoIABase.Size = new Size(105, 19);
            checkboxModoIABase.TabIndex = 5;
            checkboxModoIABase.Text = "IA Base";
            checkboxModoIABase.UseVisualStyleBackColor = false;
            // 
            // checkboxModoSuriel
            // 
            checkboxModoSuriel.BackColor = Color.DarkGray;
            checkboxModoSuriel.Location = new Point(18, 78);
            checkboxModoSuriel.Name = "checkboxModoSuriel";
            checkboxModoSuriel.Size = new Size(105, 19);
            checkboxModoSuriel.TabIndex = 6;
            checkboxModoSuriel.Text = "NULL";
            checkboxModoSuriel.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanel6
            // 
            flowLayoutPanel6.BackColor = Color.DimGray;
            flowLayoutPanel6.Location = new Point(3, 5);
            flowLayoutPanel6.Margin = new Padding(1, 3, 1, 3);
            flowLayoutPanel6.Name = "flowLayoutPanel6";
            flowLayoutPanel6.Size = new Size(147, 60);
            flowLayoutPanel6.TabIndex = 2;
            // 
            // contenedorHistorial
            // 
            contenedorHistorial.BackColor = Color.Gray;
            contenedorHistorial.Controls.Add(flowLayoutPanel4);
            contenedorHistorial.Controls.Add(flowLayoutPanel5);
            contenedorHistorial.Controls.Add(flowLayoutPanel7);
            contenedorHistorial.Controls.Add(botonBorrarHistorial);
            contenedorHistorial.Location = new Point(3, 85);
            contenedorHistorial.Margin = new Padding(1, 3, 1, 3);
            contenedorHistorial.Name = "contenedorHistorial";
            contenedorHistorial.Size = new Size(147, 410);
            contenedorHistorial.TabIndex = 3;
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.BackColor = Color.Gray;
            flowLayoutPanel4.Location = new Point(3, 3);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(144, 69);
            flowLayoutPanel4.TabIndex = 11;
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.BackColor = Color.DarkGray;
            flowLayoutPanel5.Controls.Add(label1);
            flowLayoutPanel5.Controls.Add(checkboxHistorial1);
            flowLayoutPanel5.Controls.Add(checkboxHistorial2);
            flowLayoutPanel5.Controls.Add(checkboxHistorial3);
            flowLayoutPanel5.Controls.Add(checkboxHistorial4);
            flowLayoutPanel5.Controls.Add(checkboxHistorial5);
            flowLayoutPanel5.FlowDirection = FlowDirection.RightToLeft;
            flowLayoutPanel5.Location = new Point(3, 78);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new Size(141, 190);
            flowLayoutPanel5.TabIndex = 10;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.BackColor = Color.DarkGray;
            label1.Font = new Font("Segoe UI", 14F);
            label1.Location = new Point(1, 0);
            label1.Name = "label1";
            label1.Size = new Size(137, 25);
            label1.TabIndex = 4;
            label1.Text = "Modos";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // checkboxHistorial1
            // 
            checkboxHistorial1.BackColor = Color.DarkGray;
            checkboxHistorial1.Font = new Font("Segoe UI", 14F);
            checkboxHistorial1.Location = new Point(17, 28);
            checkboxHistorial1.Name = "checkboxHistorial1";
            checkboxHistorial1.Size = new Size(121, 24);
            checkboxHistorial1.TabIndex = 5;
            checkboxHistorial1.Text = "Historial 1";
            checkboxHistorial1.UseVisualStyleBackColor = false;
            // 
            // checkboxHistorial2
            // 
            checkboxHistorial2.BackColor = Color.DarkGray;
            checkboxHistorial2.Font = new Font("Segoe UI", 14F);
            checkboxHistorial2.Location = new Point(17, 58);
            checkboxHistorial2.Name = "checkboxHistorial2";
            checkboxHistorial2.Size = new Size(121, 24);
            checkboxHistorial2.TabIndex = 3;
            checkboxHistorial2.Text = "Historial 2";
            checkboxHistorial2.UseVisualStyleBackColor = false;
            // 
            // checkboxHistorial3
            // 
            checkboxHistorial3.BackColor = Color.DarkGray;
            checkboxHistorial3.Font = new Font("Segoe UI", 14F);
            checkboxHistorial3.Location = new Point(17, 88);
            checkboxHistorial3.Name = "checkboxHistorial3";
            checkboxHistorial3.Size = new Size(121, 24);
            checkboxHistorial3.TabIndex = 6;
            checkboxHistorial3.Text = "Historial 3";
            checkboxHistorial3.UseVisualStyleBackColor = false;
            // 
            // checkboxHistorial4
            // 
            checkboxHistorial4.BackColor = Color.DarkGray;
            checkboxHistorial4.Font = new Font("Segoe UI", 14F);
            checkboxHistorial4.Location = new Point(17, 118);
            checkboxHistorial4.Name = "checkboxHistorial4";
            checkboxHistorial4.Size = new Size(121, 24);
            checkboxHistorial4.TabIndex = 7;
            checkboxHistorial4.Text = "Historial 4";
            checkboxHistorial4.UseVisualStyleBackColor = false;
            // 
            // checkboxHistorial5
            // 
            checkboxHistorial5.BackColor = Color.DarkGray;
            checkboxHistorial5.Font = new Font("Segoe UI", 14F);
            checkboxHistorial5.Location = new Point(17, 148);
            checkboxHistorial5.Name = "checkboxHistorial5";
            checkboxHistorial5.Size = new Size(121, 24);
            checkboxHistorial5.TabIndex = 8;
            checkboxHistorial5.Text = "Historial 5";
            checkboxHistorial5.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanel7
            // 
            flowLayoutPanel7.BackColor = Color.Gray;
            flowLayoutPanel7.Location = new Point(3, 274);
            flowLayoutPanel7.Name = "flowLayoutPanel7";
            flowLayoutPanel7.Size = new Size(141, 33);
            flowLayoutPanel7.TabIndex = 13;
            // 
            // botonBorrarHistorial
            // 
            botonBorrarHistorial.Font = new Font("Segoe UI", 13F);
            botonBorrarHistorial.Location = new Point(3, 313);
            botonBorrarHistorial.Name = "botonBorrarHistorial";
            botonBorrarHistorial.Size = new Size(141, 37);
            botonBorrarHistorial.TabIndex = 5;
            botonBorrarHistorial.Text = "Borrar historial";
            botonBorrarHistorial.UseVisualStyleBackColor = true;
            botonBorrarHistorial.Click += botonBorrarHistorial_Click;
            // 
            // contenedorTitulo
            // 
            contenedorTitulo.BackColor = Color.DimGray;
            contenedorTitulo.Location = new Point(153, 5);
            contenedorTitulo.Name = "contenedorTitulo";
            contenedorTitulo.Size = new Size(544, 60);
            contenedorTitulo.TabIndex = 1;
            // 
            // contenedorHerramientas
            // 
            contenedorHerramientas.BackColor = Color.Gray;
            contenedorHerramientas.Controls.Add(flowLayoutPanel1);
            contenedorHerramientas.Controls.Add(contenedorDeModos);
            contenedorHerramientas.Controls.Add(botonCargarTxt);
            contenedorHerramientas.Controls.Add(controladorDeAudio);
            contenedorHerramientas.Controls.Add(flowLayoutPanel3);
            contenedorHerramientas.Controls.Add(botonEnviar);
            contenedorHerramientas.Location = new Point(565, 85);
            contenedorHerramientas.Margin = new Padding(1, 3, 1, 3);
            contenedorHerramientas.Name = "contenedorHerramientas";
            contenedorHerramientas.Size = new Size(132, 410);
            contenedorHerramientas.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.Gray;
            flowLayoutPanel1.Location = new Point(3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(126, 69);
            flowLayoutPanel1.TabIndex = 10;
            // 
            // botonCargarTxt
            // 
            botonCargarTxt.Font = new Font("Segoe UI", 11F);
            botonCargarTxt.Location = new Point(3, 184);
            botonCargarTxt.Name = "botonCargarTxt";
            botonCargarTxt.Size = new Size(123, 31);
            botonCargarTxt.TabIndex = 14;
            botonCargarTxt.Text = "Cargar BDP";
            botonCargarTxt.UseVisualStyleBackColor = true;
            botonCargarTxt.Click += botonCargarTxt_Click_1;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.BackColor = Color.Gray;
            flowLayoutPanel3.Location = new Point(3, 297);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(126, 33);
            flowLayoutPanel3.TabIndex = 12;
            // 
            // VentanaPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(701, 501);
            Controls.Add(contenedorHistorial);
            Controls.Add(flowLayoutPanel6);
            Controls.Add(cajaDeTextoMensaje);
            Controls.Add(areaDeChat);
            Controls.Add(contenedorTitulo);
            Controls.Add(contenedorHerramientas);
            Name = "VentanaPrincipal";
            Text = "Suriel";
            FormClosing += VentanaPrincipal_FormClosing;
            Load += VentanaPrincipal_Load;
            ((System.ComponentModel.ISupportInitialize)barraVolumen).EndInit();
            controladorDeAudio.ResumeLayout(false);
            controladorDeAudio.PerformLayout();
            contenedorDeModos.ResumeLayout(false);
            contenedorHistorial.ResumeLayout(false);
            flowLayoutPanel5.ResumeLayout(false);
            contenedorHerramientas.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox areaDeChat;
        private TextBox cajaDeTextoMensaje;
        private Button botonEnviar;
        private CheckBox checkboxModoBDPrivada;
        private Label labelModo;
        private CheckBox checkboxVozActivada;
        private TrackBar barraVolumen;
        private Label labelVolumen;
        private FlowLayoutPanel controladorDeAudio;
        private FlowLayoutPanel contenedorDeModos;
        private FlowLayoutPanel contenedorTitulo;
        private FlowLayoutPanel flowLayoutPanel6;
        private FlowLayoutPanel contenedorHistorial;
        private CheckBox checkboxModoIABase;
        private CheckBox checkboxModoSuriel;
        private FlowLayoutPanel contenedorHerramientas;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel3;
        private FlowLayoutPanel flowLayoutPanel4;
        private FlowLayoutPanel flowLayoutPanel5;
        private Label label1;
        private CheckBox checkboxHistorial1;
        private CheckBox checkboxHistorial2;
        private CheckBox checkboxHistorial3;
        private CheckBox checkboxHistorial4;
        private CheckBox checkboxHistorial5;
        private FlowLayoutPanel flowLayoutPanel7;
        private Button botonBorrarHistorial;
        private Button botonCargarTxt;
    }
}
