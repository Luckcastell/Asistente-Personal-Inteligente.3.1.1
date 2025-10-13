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
            ((System.ComponentModel.ISupportInitialize)barraVolumen).BeginInit();
            SuspendLayout();
            // 
            // areaDeChat
            // 
            areaDeChat.Location = new Point(94, 57);
            areaDeChat.Name = "areaDeChat";
            areaDeChat.Size = new Size(100, 96);
            areaDeChat.TabIndex = 0;
            areaDeChat.Text = "";
            // 
            // cajaDeTextoMensaje
            // 
            cajaDeTextoMensaje.Location = new Point(259, 105);
            cajaDeTextoMensaje.Name = "cajaDeTextoMensaje";
            cajaDeTextoMensaje.Size = new Size(100, 23);
            cajaDeTextoMensaje.TabIndex = 1;
            cajaDeTextoMensaje.KeyDown += cajaDeTextoMensaje_KeyDown;
            // 
            // botonEnviar
            // 
            botonEnviar.Location = new Point(343, 172);
            botonEnviar.Name = "botonEnviar";
            botonEnviar.Size = new Size(75, 23);
            botonEnviar.TabIndex = 2;
            botonEnviar.Text = "button1";
            botonEnviar.UseVisualStyleBackColor = true;
            botonEnviar.Click += botonEnviar_Click;
            // 
            // checkboxModoBDPrivada
            // 
            checkboxModoBDPrivada.AutoSize = true;
            checkboxModoBDPrivada.Location = new Point(526, 126);
            checkboxModoBDPrivada.Name = "checkboxModoBDPrivada";
            checkboxModoBDPrivada.Size = new Size(82, 19);
            checkboxModoBDPrivada.TabIndex = 3;
            checkboxModoBDPrivada.Text = "checkBox1";
            checkboxModoBDPrivada.UseVisualStyleBackColor = true;
            // 
            // labelModo
            // 
            labelModo.AutoSize = true;
            labelModo.Location = new Point(248, 174);
            labelModo.Name = "labelModo";
            labelModo.Size = new Size(38, 15);
            labelModo.TabIndex = 4;
            labelModo.Text = "label1";
            // 
            // checkboxVozActivada
            // 
            checkboxVozActivada.AutoSize = true;
            checkboxVozActivada.Location = new Point(137, 226);
            checkboxVozActivada.Name = "checkboxVozActivada";
            checkboxVozActivada.Size = new Size(82, 19);
            checkboxVozActivada.TabIndex = 5;
            checkboxVozActivada.Text = "checkBox1";
            checkboxVozActivada.UseVisualStyleBackColor = true;
            checkboxVozActivada.CheckStateChanged += checkboxVozActivada_CheckedChanged;
            // 
            // barraVolumen
            // 
            barraVolumen.Location = new Point(230, 34);
            barraVolumen.Maximum = 100;
            barraVolumen.Name = "barraVolumen";
            barraVolumen.Size = new Size(104, 45);
            barraVolumen.TabIndex = 6;
            barraVolumen.Scroll += barraVolumen_Scroll;
            // 
            // labelVolumen
            // 
            labelVolumen.AutoSize = true;
            labelVolumen.Location = new Point(76, 164);
            labelVolumen.Name = "labelVolumen";
            labelVolumen.Size = new Size(38, 15);
            labelVolumen.TabIndex = 7;
            labelVolumen.Text = "label1";
            // 
            // VentanaPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(labelVolumen);
            Controls.Add(barraVolumen);
            Controls.Add(checkboxVozActivada);
            Controls.Add(labelModo);
            Controls.Add(checkboxModoBDPrivada);
            Controls.Add(botonEnviar);
            Controls.Add(cajaDeTextoMensaje);
            Controls.Add(areaDeChat);
            Name = "VentanaPrincipal";
            Text = "Form1";
            FormClosing += VentanaPrincipal_FormClosing;
            ((System.ComponentModel.ISupportInitialize)barraVolumen).EndInit();
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
    }
}
