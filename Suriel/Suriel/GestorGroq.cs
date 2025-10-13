using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Suriel
{

    // Definición del dominio: Objeto para enviar mensajes a la API de Groq.
    // Estos objetos se mapean directamente al JSON que espera la API.

    public struct Mensaje
    {
        [JsonPropertyName("role")]
        public string Rol { get; set; } // 'user' o 'assistant'

        [JsonPropertyName("content")]
        public string Contenido { get; set; }
    }

    public class SolicitudChat
    {
        [JsonPropertyName("messages")]
        public List<Mensaje> Mensajes { get; set; }

        [JsonPropertyName("model")]
        public string Modelo { get; set; }
    }

    public class RespuestaChat
    {
        [JsonPropertyName("choices")]
        public List<OpcionRespuesta> Opciones { get; set; }
    }

    public class OpcionRespuesta
    {
        [JsonPropertyName("message")]
        public Mensaje Mensaje { get; set; }
    }


    // Definimos la clase para manejar la interacción con la API de Groq y la memoria.
    public class GestorGroq
    {
        // Constante para la URL de la API de Groq.
        private const string URL_GROQ_API = "https://api.groq.com/openai/v1/chat/completions";
        // El modelo de IA especificado en la solicitud.
        private const string NOMBRE_MODELO = "llama-3.1-8b-instant";

        // Obtené tu clave de Groq y colocala acá. En un proyecto real, se obtendría de un lugar más seguro (ej: variables de entorno).
        private const string CLAVE_API_GROQ = "gsk_hYtIuTXw8GT6G0tbEbTqWGd";//yb3FYTjn93xyxTOqS9GAy45sMdGzj

        // Archivo de texto que usaremos para el modo de base de datos privada.
        // Asumimos que este archivo existe en el directorio de la aplicación.
        // Usamos nombres descriptivos como 'rutaCompleta' para que se autodocumente.
        private const string RUTA_BD_PRIVADA = "base_datos_suriel.txt";

        // El cliente HTTP que usaremos para todas las peticiones a Groq.
        private readonly HttpClient clienteHttp;

        // El 'core' de la memoria de Suriel: el historial completo de la conversación.
        // Usamos 'historialConversacion' para que se sepa bien lo que almacena.
        private readonly List<Mensaje> historialConversacion;

        /// <summary>
        /// Documentacion: Enumeración para definir los modos de operación de Suriel.
        /// </summary>
        public enum ModoOperacion
        {
            Normal,
            BDPrivada
        }

        // Propiedad para obtener el historial actual de mensajes.
        public List<Mensaje> HistorialConversacion => historialConversacion;

        // Constructor de la clase.
        public GestorGroq()
        {
            historialConversacion = new List<Mensaje>();
            clienteHttp = new HttpClient();

            // Configuramos la cabecera de autorización una sola vez.
            clienteHttp.DefaultRequestHeaders.Add("Authorization", $"Bearer {CLAVE_API_GROQ}");
        }

        /// <summary>
        /// Documentacion: Envía el mensaje del usuario a la API de Groq, aplicando la lógica del modo de operación actual.
        /// </summary>
        /// <param name="mensajeUsuario">El texto de la pregunta del usuario.</param>
        /// <param name="modoActual">El modo de Suriel (Normal o BD Privada).</param>
        /// <returns>La respuesta generada por el modelo de IA.</returns>
        // El nombre de la función es descriptivo: 'obtenerRespuesta'
        public async Task<string> ObtenerRespuesta(string mensajeUsuario, ModoOperacion modoActual)
        {
            // Creamos la lista de mensajes que se enviará en la solicitud actual.
            // Hacemos una copia para no alterar el historial principal antes de la respuesta.
            var mensajesParaSolicitud = new List<Mensaje>();

            // Agregamos el rol de sistema para definir el comportamiento.
            // El 'system_prompt' es clave para la personalidad y la BD privada.
            string mensajeDeSistema = "Sos Suriel, un asistente de programación Web de Argentina. Respondé siempre en español argentino. Mantené un tono positivo y alentador.";

            // Filtramos por el modo actual porque la lógica de la BD Privada es compleja.
            if (modoActual == ModoOperacion.BDPrivada)
            {
                try
                {
                    // Leemos el contenido completo de la Base de Datos Privada.
                    // Lo hacemos dentro del 'try-catch' porque la lectura de archivos puede fallar.
                    string contenidoBD = File.ReadAllText(RUTA_BD_PRIVADA, Encoding.UTF8);

                    // Sobreescribimos el mensaje de sistema para que use SOLO la BD Privada.
                    // Esta es la clave del modo: forzar al LLM a usar solo la fuente.
                    mensajeDeSistema = "SOS UN ASISTENTE DE CONSULTAS INTERNAS. Basándote ÚNICAMENTE en la siguiente Base de Datos Privada (BDP), respondé la pregunta del usuario. Si la información no está en la BDP, respondé simplemente: 'Disculpá, esa información no está en mi Base de Datos Privada.'\n\n--- INICIO BDP ---\n" + contenidoBD + "\n--- FIN BDP ---";

                }
                catch (FileNotFoundException)
                {
                    // Si el archivo no existe, notificamos al usuario.
                    return $"Error: No se encontró el archivo de la BD Privada en {RUTA_BD_PRIVADA}. ¡Revisá la ruta, che!";
                }
                catch (Exception ex)
                {
                    return $"Ocurrió un error al leer la BD Privada: {ex.Message}";
                }
            }

            // Agregamos el mensaje de sistema como el primer mensaje del contexto.
            mensajesParaSolicitud.Add(new Mensaje { Rol = "system", Contenido = mensajeDeSistema });

            // Agregamos todo el historial de conversación para darle memoria a Suriel.
            mensajesParaSolicitud.AddRange(historialConversacion);

            // Finalmente, agregamos el mensaje nuevo del usuario.
            var nuevoMensajeUsuario = new Mensaje { Rol = "user", Contenido = mensajeUsuario };
            mensajesParaSolicitud.Add(nuevoMensajeUsuario);

            // Preparamos el objeto de la solicitud que será serializado a JSON.
            var solicitud = new SolicitudChat
            {
                Modelo = NOMBRE_MODELO,
                Mensajes = mensajesParaSolicitud
            };

            // Serializamos el objeto en un JSON para enviarlo.
            var contenidoJson = JsonSerializer.Serialize(solicitud);
            var contenidoHttp = new StringContent(contenidoJson, Encoding.UTF8, "application/json");

            try
            {
                // Ejecutamos la petición HTTP asíncrona.
                var respuestaHttp = await clienteHttp.PostAsync(URL_GROQ_API, contenidoHttp);

                // Verificamos si la respuesta fue exitosa (código 200).
                respuestaHttp.EnsureSuccessStatusCode();

                // Leemos el contenido de la respuesta.
                var contenidoRespuesta = await respuestaHttp.Content.ReadAsStringAsync();

                // Deserializamos el JSON de respuesta.
                var objetoRespuesta = JsonSerializer.Deserialize<RespuestaChat>(contenidoRespuesta);

                // Obtenemos el contenido del mensaje de Suriel.
                string respuestaIA = objetoRespuesta.Opciones[0].Mensaje.Contenido.Trim();

                // Una vez que tenemos la respuesta exitosa, ACTUALIZAMOS EL HISTORIAL (la memoria).
                // Lo hacemos acá porque si falla la API, no queremos registrar un mensaje de usuario sin respuesta.
                historialConversacion.Add(nuevoMensajeUsuario);
                historialConversacion.Add(new Mensaje { Rol = "assistant", Contenido = respuestaIA });

                return respuestaIA;
            }
            catch (HttpRequestException ex)
            {
                // Manejo de errores de conexión o API.
                return $"Error de conexión con Groq. Revisá tu clave y conexión a internet. Detalles: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Otros errores, por ejemplo, en la deserialización.
                return $"Ocurrió un error inesperado. Detalles: {ex.Message}";
            }
        }
    }
}
