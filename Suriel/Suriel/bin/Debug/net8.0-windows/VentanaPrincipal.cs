using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Suriel
{
    // Cambiamos el nombre de la clase para que sea más descriptivo del dominio: VentanaPrincipal.
    public partial class VentanaPrincipal : Form
    {
        // Las dependencias de la aplicación (Gestores de Voz y Groq).
        private readonly GestorGroq gestorGroq;
        private readonly GestorVoz gestorVoz;

        /// <summary>
        /// Documentacion: Constructor de la Ventana Principal. Se inicializan los gestores y la interfaz.
        /// </summary>
        public VentanaPrincipal()
        {
            InitializeComponent();

            // Inicializamos los componentes esenciales para el funcionamiento de Suriel.
            gestorGroq = new GestorGroq();
            gestorVoz = new GestorVoz();

            // Configuramos la interfaz para reflejar el estado inicial.
            ConfigurarComponentesIniciales();
        }

        /// <summary>
        /// Documentacion: Configura los valores iniciales de los controles de la interfaz.
        /// </summary>
        private void ConfigurarComponentesIniciales()
        {
            // El control de voz debe iniciar según el estado del GestorVoz.
            checkboxVozActivada.Checked = gestorVoz.EstaActivada;
            // El volumen inicial de la barra se corresponde con el valor por defecto del sintetizador (100).
            barraVolumen.Value = 100;

            // Mostramos un mensaje de bienvenida.
            MostrarMensajeEnChat("Suriel", "¡Hola! Soy Suriel, tu asistente de programación Web. ¿En qué te puedo ayudar hoy, che?");

            // Seteamos el título de la ventana.
            this.Text = "Suriel - Asistente de Programación";
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
        /// <param name="nombreRemitente">El nombre de quien envía el mensaje (Usuario o Suriel).</param>
        /// <param name="contenidoMensaje">El contenido de texto del mensaje.</param>
        // Usamos 'nombreRemitente' y 'contenidoMensaje' en lugar de 'n' y 'c' para que el código sea claro.
        private void MostrarMensajeEnChat(string nombreRemitente, string contenidoMensaje)
        {
            // La fuente y el color cambian para diferenciar al usuario y al asistente.
            Color colorTexto = (nombreRemitente == "Usuario") ? Color.Blue : Color.Green;

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

            // 4. Determinar el modo de operación actual (Normal o BD Privada).
            GestorGroq.ModoOperacion modoActual = checkboxModoBDPrivada.Checked
                                                 ? GestorGroq.ModoOperacion.BDPrivada
                                                 : GestorGroq.ModoOperacion.Normal;

            // 5. Obtener la respuesta de la IA de forma asíncrona.
            string respuestaIA = await gestorGroq.ObtenerRespuesta(mensajeUsuario, modoActual);

            // 6. Mostrar la respuesta de Suriel.
            MostrarMensajeEnChat("Suriel", respuestaIA);

            // 7. Reproducir el audio de la respuesta.
            gestorVoz.ReproducirAudio(respuestaIA);

            // 8. Desbloquear la interfaz.
            AlternarEstadoControles(true);
        }

        /// <summary>
        /// Documentacion: Alterna el estado de activación de los controles principales para evitar que se envíen mensajes antes de recibir la respuesta.
        /// </summary>
        /// <param name="activado">True para activar, False para desactivar.</param>
        private void AlternarEstadoControles(bool activado)
        {
            // Se desactiva el botón y la caja de texto mientras se espera la respuesta.
            botonEnviar.Enabled = activado;
            cajaDeTextoMensaje.Enabled = activado;
            checkboxModoBDPrivada.Enabled = activado;

            // Cambiamos el cursor para indicar que la aplicación está "ocupada".
            this.Cursor = activado ? Cursors.Default : Cursors.WaitCursor;
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
            // NOTA: El cliente HTTP se liberaría en un destructor o Dispose, pero para el ejemplo simple no es crítico.
        }
    }
}
