using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Suriel
{
    public partial class VentanaPrincipal : Form
    {
        private readonly GestorGroq gestorGroq;
        private readonly GestorVoz gestorVoz;
        private List<CheckBox> checkboxesModo;
        private List<CheckBox> checkboxesHistorial;

        /// <summary>
        /// Documentacion: Constructor de la Ventana Principal. Se inicializan los gestores y la interfaz.
        /// </summary>
        public VentanaPrincipal()
        {
            InitializeComponent();

            gestorGroq = new GestorGroq();
            gestorVoz = new GestorVoz();

            this.FormClosing += VentanaPrincipal_FormClosing;

            // Inicializa las listas de controles y conecta los eventos.
            InicializarListasDeControles();
            ConectarEventosCheckboxes();

            // Configuramos la interfaz para reflejar el estado inicial.
            ConfigurarComponentesIniciales();
        }

        private void InicializarListasDeControles()
        {
            // Agrupamos los CheckBoxes para el Modo
            // Nota: Se asume que estos controles ya están declarados en VentanaPrincipal.Designer.cs
            checkboxesModo = new List<CheckBox> { checkboxModoIABase, checkboxModoBDPrivada };

            // Agrupamos los CheckBoxes para el Historial
            checkboxesHistorial = new List<CheckBox>
            {
                checkboxHistorial1, checkboxHistorial2, checkboxHistorial3,
                checkboxHistorial4, checkboxHistorial5
            };
        }

        private void ConectarEventosCheckboxes()
        {
            // Conectamos la lógica de exclusividad a los eventos CheckedChanged.
            foreach (var cb in checkboxesModo)
            {
                cb.CheckedChanged += CheckboxModo_CheckedChanged;
            }

            foreach (var cb in checkboxesHistorial)
            {
                cb.CheckedChanged += CheckboxHistorial_CheckedChanged;
            }
        }

        /// <summary>
        /// Documentacion: Lógica para que solo un checkbox de modo esté activo (radio button).
        /// </summary>
        private void CheckboxModo_CheckedChanged(object sender, EventArgs e)
        {
            var checkboxCambiado = (CheckBox)sender;

            if (checkboxCambiado.Checked)
            {
                // Si este se activa, se desactivan los demás.
                foreach (var cb in checkboxesModo.Where(c => c != checkboxCambiado))
                {
                    cb.Checked = false;
                }
            }
            // Si el usuario desactiva el único activo, volvemos a activar el predeterminado (IABase).
            else if (!checkboxesModo.Any(c => c.Checked))
            {
                checkboxModoIABase.Checked = true;
            }

            ActualizarChatConHistorialActivo();
        }

        /// <summary>
        /// Documentacion: Lógica para que solo un checkbox de historial esté activo (radio button).
        /// </summary>
        private void CheckboxHistorial_CheckedChanged(object sender, EventArgs e)
        {
            var checkboxCambiado = (CheckBox)sender;

            if (checkboxCambiado.Checked)
            {
                // Si este se activa, se desactivan los demás.
                foreach (var cb in checkboxesHistorial.Where(c => c != checkboxCambiado))
                {
                    cb.Checked = false;
                }
            }
            // Si el usuario desactiva el único activo, volvemos a activar el predeterminado (Historial1).
            else if (!checkboxesHistorial.Any(c => c.Checked))
            {
                checkboxHistorial1.Checked = true;
            }

            ActualizarChatConHistorialActivo();
        }

        /// <summary>
        /// Documentacion: Obtiene el Modo de Operación y el Identificador de Historial seleccionados.
        /// </summary>
        private (GestorGroq.ModoOperacion Modo, GestorGroq.IdentificadorHistorial IdHistorial) ObtenerEstadoActual()
        {
            // Determinar Modo
            GestorGroq.ModoOperacion modo = checkboxModoBDPrivada.Checked
                                             ? GestorGroq.ModoOperacion.BDPrivada
                                             : GestorGroq.ModoOperacion.IABase;

            // Determinar Historial Activo (Historial1 = 0, Historial2 = 1, etc.)
            GestorGroq.IdentificadorHistorial idHistorial = GestorGroq.IdentificadorHistorial.Historial1;
            for (int i = 0; i < checkboxesHistorial.Count; i++)
            {
                if (checkboxesHistorial[i].Checked)
                {
                    // El índice (0-4) coincide con el valor del Enum.
                    idHistorial = (GestorGroq.IdentificadorHistorial)i;
                    break;
                }
            }

            return (modo, idHistorial);
        }

        /// <summary>
        /// Documentacion: Carga el historial de la conversación activa (desde el archivo TXT) y lo muestra en el chat.
        /// </summary>
        private void ActualizarChatConHistorialActivo()
        {
            var (modo, idHistorial) = ObtenerEstadoActual();

            // Se lee el historial completo del archivo TXT a través del GestorGroq.
            var historial = gestorGroq.HistorialConversacionCompleto(modo, idHistorial);

            areaDeChat.Clear();

            // Mensaje de inicio, indicando el historial activo
            MostrarMensajeEnChat("Sistema", $"Historial {((int)idHistorial) + 1} ({modo}) cargado.");

            // Repoblar el chat con la conversación guardada.
            foreach (var mensaje in historial)
            {
                MostrarMensajeEnChat(mensaje.Rol == "user" ? "Usuario" : "Suriel", mensaje.Contenido);
            }
        }

        // --- MÉTODOS EXISTENTES MODIFICADOS/UTILIZADOS ---

        /// <summary>
        /// Documentacion: Configura los valores iniciales de los controles de la interfaz.
        /// </summary>
        private void ConfigurarComponentesIniciales()
        {
            // El control de voz debe iniciar según el estado del GestorVoz.
            checkboxVozActivada.Checked = gestorVoz.EstaActivada;
            // El volumen inicial de la barra se corresponde con el valor por defecto del sintetizador (100).
            barraVolumen.Value = 100;

            // Establecer el estado inicial predeterminado.
            checkboxModoIABase.Checked = true; // Modo por defecto
            checkboxHistorial1.Checked = true; // Historial por defecto

            // El ActualizarChatConHistorialActivo() se llama por los eventos CheckedChanged, 
            // pero lo forzamos por si es la primera carga.
            if (checkboxModoIABase.Checked && checkboxHistorial1.Checked)
            {
                ActualizarChatConHistorialActivo();
            }

            MostrarMensajeEnChat("Suriel", "¡Hola! Soy Suriel, tu asistente personal. ¿En qué te puedo ayudar hoy?");

            // Seteamos el título de la ventana.
            this.Text = "Suriel - Asistente de Programación";
            this.Width = 717;
            this.Height = 540;

            //fondo.Size = new Size(this.Width, this.Height);
        }

        /// <summary>
        /// Documentacion: Lógica que se ejecuta al presionar el botón de enviar o la tecla Enter.
        /// </summary>
        private async void botonEnviar_Click(object sender, EventArgs e)
        {
            // Llamamos a la función principal que maneja el envío.
            await EnviarMensajeDelUsuario();
        }

        /// <summary>
        /// Documentacion: Muestra un mensaje en el área de chat de la interfaz.
        /// </summary>
        private void MostrarMensajeEnChat(string nombreRemitente, string contenidoMensaje)
        {
            // La fuente y el color cambian para diferenciar al usuario y al asistente, y ahora al sistema.
            Color colorTexto;
            if (nombreRemitente == "Usuario")
                colorTexto = Color.Blue;
            else if (nombreRemitente == "Suriel")
                colorTexto = Color.Green;
            else // Sistema
                colorTexto = Color.DarkGray;

            // Establecemos el color y el formato para el nombre.
            areaDeChat.SelectionStart = areaDeChat.TextLength;
            areaDeChat.SelectionLength = 0;
            areaDeChat.SelectionColor = colorTexto;
            areaDeChat.SelectionFont = new Font(areaDeChat.Font, FontStyle.Bold);

            // Agregamos el nombre y el contenido.
            areaDeChat.AppendText($"\n[{nombreRemitente}]: ");

            // Restablecemos el formato para el cuerpo del mensaje.
            areaDeChat.SelectionColor = Color.Black;
            areaDeChat.SelectionFont = new Font(areaDeChat.Font, FontStyle.Regular);
            areaDeChat.AppendText(contenidoMensaje);

            // Hacemos que la barra de desplazamiento baje automáticamente (scroll).
            areaDeChat.ScrollToCaret();
        }

        /// <summary>
        /// Documentacion: Función asíncrona principal para procesar el mensaje del usuario y obtener la respuesta de Groq.
        /// </summary>
        private async Task EnviarMensajeDelUsuario()
        {
            // Obtenemos el texto de la caja de mensaje.
            string mensajeUsuario = cajaDeTextoMensaje.Text.Trim();

            // filtramos por mensaje vacío porque no tiene sentido enviar una cadena vacía a la IA.
            if (string.IsNullOrEmpty(mensajeUsuario))
            {
                return;
            }

            // 1. Mostrar el mensaje del usuario en el historial.
            MostrarMensajeEnChat("Usuario", mensajeUsuario);

            // 2. Bloquear la interfaz para que el usuario no envíe mensajes múltiples mientras la IA responde.
            AlternarEstadoControles(false);

            // 3. Limpiar la caja de texto.
            cajaDeTextoMensaje.Clear();

            // 4. Obtener el estado activo de los controles.
            var (modoActual, idHistorialActual) = ObtenerEstadoActual();

            // 5. Obtener la respuesta de la IA de forma asíncrona (Groq se encarga de cargar/guardar el historial).
            string respuestaIA = await gestorGroq.ObtenerRespuesta(mensajeUsuario, modoActual, idHistorialActual);

            // 6. Mostrar la respuesta de Suriel.
            MostrarMensajeEnChat("Suriel", respuestaIA);

            // 7. Reproducir el audio de la respuesta.
            gestorVoz.ReproducirAudio(respuestaIA);

            // 8. Desbloquear la interfaz.
            AlternarEstadoControles(true);

            // 9. Recargar el chat para sincronizar la interfaz con el archivo (mostrar el nuevo intercambio).
            ActualizarChatConHistorialActivo();
        }

        /// <summary>
        /// Documentacion: Alterna el estado de activación de los controles principales para evitar que se envíen mensajes antes de recibir la respuesta.
        /// </summary>
        private void AlternarEstadoControles(bool activado)
        {
            // Se desactiva el botón y la caja de texto mientras se espera la respuesta.
            botonEnviar.Enabled = activado;
            cajaDeTextoMensaje.Enabled = activado;

            // Deshabilitamos los selectores de modo/historial mientras se procesa la respuesta para evitar errores.
            foreach (var cb in checkboxesModo) cb.Enabled = activado;
            foreach (var cb in checkboxesHistorial) cb.Enabled = activado;

            // Cambiamos el cursor para indicar que la aplicación está "ocupada".
            this.Cursor = activado ? Cursors.Default : Cursors.WaitCursor;
            if (activado) cajaDeTextoMensaje.Focus();
        }

        /// <summary>
        /// Documentacion: Maneja el cambio del volumen de la voz mediante el control TrackBar.
        /// </summary>
        private void barraVolumen_Scroll(object sender, EventArgs e)
        {
            // Pasamos el valor de la barra directamente al gestor de voz.
            gestorVoz.EstablecerVolumen(barraVolumen.Value);
        }

        /// <summary>
        /// Documentacion: Maneja el cambio de estado del checkbox de Voz Activada.
        /// </summary>
        private void checkboxVozActivada_CheckedChanged(object sender, EventArgs e)
        {
            // Actualizamos la propiedad del gestor de voz y detenemos cualquier reproducción si se desactiva.
            gestorVoz.EstaActivada = checkboxVozActivada.Checked;

            // Detenemos la voz inmediatamente si se desactiva, por si estaba hablando.
            if (!gestorVoz.EstaActivada)
            {
                gestorVoz.DetenerAudio();
            }
        }

        // Este evento permite enviar el mensaje al presionar Enter en la caja de texto.
        private void cajaDeTextoMensaje_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificamos si la tecla presionada es Enter y que el control Enviar esté disponible.
            if (e.KeyCode == Keys.Enter && botonEnviar.Enabled)
            {
                // Evitamos el sonido de "ding" al presionar enter.
                e.SuppressKeyPress = true;
                // Llamamos a la lógica de envío.
                EnviarMensajeDelUsuario();
            }
        }

        /// <summary>
        /// Documentacion: Se llama cuando el formulario está por cerrarse para limpiar recursos.
        /// </summary>
        private void VentanaPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Liberamos los recursos del sintetizador de voz para evitar problemas.
            gestorVoz.DetenerAudio();
        }

        /// <summary>
        /// Documentacion: Maneja el clic en el botón Borrar Historial. Pide confirmación y borra el archivo.
        /// </summary>
        private void botonBorrarHistorial_Click(object sender, EventArgs e)
        {
            var (modo, idHistorial) = ObtenerEstadoActual();

            // Obtenemos los nombres legibles para la confirmación.
            string nombreModo = modo == GestorGroq.ModoOperacion.IABase ? "Base (A)" : "Privado (B)";
            string nombreHistorial = ((int)idHistorial + 1).ToString();

            // 1. Mostrar la ventana de confirmación
            string mensajeConfirmacion = $"¿Estás seguro que querés borrar el Historial {nombreHistorial} del Modo {nombreModo}?\n\n¡Esta acción no se puede deshacer!";

            var resultado = MessageBox.Show(
                mensajeConfirmacion,
                "Confirmar Borrado de Historial",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            // 2. Procesar la respuesta del usuario
            if (resultado == DialogResult.Yes)
            {
                // 3. Ejecutar el borrado
                gestorGroq.BorrarHistorial(modo, idHistorial);

                // 4. Actualizar la interfaz para reflejar el historial vacío
                MostrarMensajeEnChat("Sistema", $"Historial {nombreHistorial} del Modo {nombreModo} ha sido BORRADO.");
                ActualizarChatConHistorialActivo();

                // El historial se recarga automáticamente como una lista vacía.
                // También volvemos a mostrar el mensaje de bienvenida para que se vea bien.
                MostrarMensajeEnChat("Suriel", "¡Hola de nuevo! Historial limpio. Soy Suriel, ¿en qué te puedo ayudar hoy?");
            }
        }

        /// <summary>
        /// Documentacion: Abre un diálogo para seleccionar un archivo .TXT o .PDF y lo copia a la BDPrivada.
        /// </summary>
        private void botonCargarTxt_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Configurar el diálogo para buscar archivos de texto (.txt) y PDFs (.pdf).
                openFileDialog.Filter = "Archivos de BDPrivada (*.txt;*.pdf)|*.txt;*.pdf|Archivos de Texto (*.txt)|*.txt|Archivos PDF (*.pdf)|*.pdf";
                openFileDialog.Title = "Seleccionar Archivo .TXT o .PDF para la BDPrivada";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string rutaOrigen = openFileDialog.FileName;
                        string nombreArchivo = Path.GetFileName(rutaOrigen);

                        // Usar el método de GestorGroq para copiar el archivo
                        gestorGroq.CopiarArchivoABDP(rutaOrigen, nombreArchivo);

                        MostrarMensajeEnChat("Sistema", $"Archivo '{nombreArchivo}' cargado exitosamente a la BDPrivada.");

                        // Forzar una actualización de la BDP (opcional, pero útil si el modo ya está activo)
                        ActualizarChatConHistorialActivo();

                        MessageBox.Show(
                            $"El archivo '{nombreArchivo}' ha sido copiado a la carpeta de ejecución.\n\nRECORDATORIO: El sistema solo lee el contenido si está en formato de TEXTO PLANO. Si subís un PDF normal, el RAG puede fallar.",
                            "Archivo Cargado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MostrarMensajeEnChat("Sistema", $"Error al cargar el archivo: {ex.Message}");
                        MessageBox.Show($"Ocurrió un error al copiar el archivo: {ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // --- MÉTODOS AUTOGENERADOS QUE NO MODIFICAMOS ---
        private void labelVolumen_Click(object sender, EventArgs e)
        {
        }
        private void checkboxVozActivada_CheckedChanged_1(object sender, EventArgs e)
        {
        }
        private void areaDeChat_TextChanged(object sender, EventArgs e)
        {
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }
        private void checkboxModoBDPrivada_CheckedChanged(object sender, EventArgs e)
        {
        }
        private void labelModo_Click(object sender, EventArgs e)
        {
        }
        private void fondo_Paint(object sender, PaintEventArgs e)
        {
        }
        private void VentanaPrincipal_Load(object sender, EventArgs e)
        {
        }

    }
}