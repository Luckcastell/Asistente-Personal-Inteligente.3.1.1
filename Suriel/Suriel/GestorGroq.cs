using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq; // Necesario para Select, OrderByDescending, Take

// --- Estructuras para la Comunicación con la API de Groq ---

public struct Mensaje
{
    [JsonPropertyName("role")]
    public string Rol { get; set; } // 'user', 'assistant' o 'system'

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

// --- Clase Principal de la IA ---

public class GestorGroq
{
    // Constantes de configuración.
    private const string URL_GROQ_API = "https://api.groq.com/openai/v1/chat/completions";
    private const string NOMBRE_MODELO = "llama-3.1-8b-instant";
    private const string CLAVE_API_GROQ = "";
    private const string RUTA_BDP = "Datos/BDPrivada"; // Carpeta para guardar archivos de la Base de Datos Privada.

    // --- LISTA DE ARCHIVOS DE LA BD PRIVADA (Tus PDFs) ---
    private static readonly List<string> RUTAS_BD_PRIVADA = new List<string>
    {
        "Curso-HTML_CSS-fusionado.pdf" // Asume que este archivo ha sido convertido a texto plano
    };

    // Constantes para la lógica de Selección de Relevancia (RAG).
    private const int MAXIMO_MENSAJES_RELEVANTES = 6;
    private const int MAXIMO_FRAGMENTOS_BD_RELEVANTES = 9;

    private readonly HttpClient clienteHttp;

    // --- Enumeraciones ---

    public enum IdentificadorHistorial
    {
        Historial1 = 0, Historial2 = 1, Historial3 = 2, Historial4 = 3, Historial5 = 4
    }

    public enum ModoOperacion
    {
        IABase, BDPrivada
    }

    // --- Constructor ---

    public GestorGroq()
    {
        clienteHttp = new HttpClient();
        clienteHttp.DefaultRequestHeaders.Add("Authorization", $"Bearer {CLAVE_API_GROQ}");
    }

    // --- Funciones de Persistencia ---

    private string ObtenerRutaArchivoHistorial(ModoOperacion modo, IdentificadorHistorial idHistorial)
    {
        string modoChar = modo == ModoOperacion.IABase ? "A" : "B";
        int id = (int)idHistorial + 1;
        return $"Historial{id}{modoChar}.TXT";
    }

    private List<Mensaje> CargarHistorial(ModoOperacion modo, IdentificadorHistorial idHistorial)
    {
        string rutaArchivo = ObtenerRutaArchivoHistorial(modo, idHistorial);

        if (!File.Exists(rutaArchivo))
        {
            return new List<Mensaje>();
        }

        try
        {
            string contenidoJson = File.ReadAllText(rutaArchivo);
            return JsonSerializer.Deserialize<List<Mensaje>>(contenidoJson) ?? new List<Mensaje>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar historial desde {rutaArchivo}: {ex.Message}");
            return new List<Mensaje>();
        }
    }

    private void GuardarHistorial(ModoOperacion modo, IdentificadorHistorial idHistorial, List<Mensaje> historial)
    {
        string rutaArchivo = ObtenerRutaArchivoHistorial(modo, idHistorial);

        try
        {
            string contenidoJson = JsonSerializer.Serialize(historial, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(rutaArchivo, contenidoJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar historial en {rutaArchivo}: {ex.Message}");
        }
    }

    // --- Funciones de Lógica Central (RAG) ---

    private int CalcularPuntajeRelevancia(string consulta, string contenido)
    {
        var palabrasClave = consulta.ToLowerInvariant()
                                     .Split(new char[] { ' ', '.', ',', '?', '!', ':', ';', '(', ')' },
                                            StringSplitOptions.RemoveEmptyEntries)
                                     .Distinct();

        int puntaje = 0;
        string contenidoNormalizado = contenido.ToLowerInvariant();

        foreach (var palabra in palabrasClave)
        {
            if (palabra.Length > 3 && contenidoNormalizado.Contains(palabra))
            {
                puntaje++;
            }
        }
        return puntaje;
    }

    /// <summary>
    /// Documentacion: Envía el mensaje del usuario a la API de Groq, aplicando RAG y persistencia.
    /// </summary>
    public async Task<string> ObtenerRespuesta(
        string mensajeUsuario,
        ModoOperacion modoActual,
        IdentificadorHistorial idHistorial)
    {
        // 1. CARGAR EL HISTORIAL ACTIVO DESDE EL ARCHIVO
        var historialActivo = CargarHistorial(modoActual, idHistorial);

        // 2. Selección de Historial Reciente (RAG conversacional).
        var nuevoMensajeUsuario = new Mensaje { Rol = "user", Contenido = mensajeUsuario };
        var historialCompletoConNuevo = new List<Mensaje>(historialActivo);

        // Aplicamos el filtro de relevancia al historial (Mensajes anteriores)
        var mensajesRelevantes = historialCompletoConNuevo
            .Select(m => new { Mensaje = m, Puntaje = CalcularPuntajeRelevancia(mensajeUsuario, m.Contenido) })
            .OrderByDescending(x => x.Puntaje)
            .Take(MAXIMO_MENSAJES_RELEVANTES)
            .Select(x => x.Mensaje)
            .ToList();

        // 3. Definir y preparar el Mensaje de Sistema (incluyendo lógica BDPrivada si aplica)
        string mensajeDeSistema = "Sos Suriel, un asistente de programación Web de Argentina. Respondé siempre en español argentino. Mantené un tono positivo y alentador.";

        if (modoActual == ModoOperacion.BDPrivada)
        {
            List<string> fragmentosTotales = new List<string>();
            string archivosNoEncontrados = "";

            // --- INICIO LÓGICA RAG MULTI-DOCUMENTO ---
            foreach (var ruta in RUTAS_BD_PRIVADA)
            {
                // La ruta para ReadAllText debe ser completa. Asumimos que los archivos están en la carpeta de ejecución.
                string rutaCompleta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RUTA_BDP, ruta);
                try
                {
                    // Asume que los archivos PDF han sido previamente convertidos a texto plano.
                    string contenidoArchivo = File.ReadAllText(rutaCompleta, Encoding.UTF8);

                    // Separamos el contenido de cada archivo en fragmentos.
                    var fragmentosArchivo = contenidoArchivo.Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

                    // Agregamos la fuente (nombre del archivo) como contexto a cada fragmento.
                    fragmentosTotales.AddRange(fragmentosArchivo.Select(f => $"[Fuente: {Path.GetFileName(ruta)}]\n{f}"));
                }
                catch (FileNotFoundException)
                {
                    archivosNoEncontrados += $"{Path.GetFileName(ruta)}, ";
                }
                catch (Exception ex)
                {
                    // Si ocurre otro error (ej. permisos), avisamos al usuario.
                    return $"Ocurrió un error al leer el archivo {ruta}: {ex.Message}";
                }
            }

            if (!string.IsNullOrEmpty(archivosNoEncontrados.TrimEnd(' ', ',')))
            {
                return $"Error: Los archivos de la BD Privada no se encontraron: {archivosNoEncontrados.TrimEnd(' ', ',')}. ¡Asegurate de que estén en la carpeta de ejecución!";
            }

            // Seleccionamos los fragmentos más relevantes de TODO el conjunto de documentos.
            var fragmentosRelevantes = fragmentosTotales
                .Select(f => new { Fragmento = f, Puntaje = CalcularPuntajeRelevancia(mensajeUsuario, f) })
                .OrderByDescending(x => x.Puntaje)
                .Take(MAXIMO_FRAGMENTOS_BD_RELEVANTES)
                .Select(x => x.Fragmento);

            string bdAumentada = string.Join("\n\n---\n\n", fragmentosRelevantes);

            // Instrucciones estrictas para el modo privado.
            mensajeDeSistema = "SOS UN ASISTENTE DE CONSULTAS INTERNAS. Basándote ÚNICAMENTE en la siguiente Base de Datos Privada (BDP) seleccionada, respondé la pregunta del usuario. Siempre citá la [Fuente: Nombre del archivo] de donde sacaste la información. Si la información NO está en la BDP, respondé: 'Disculpá, esa información no está en mi Base de Datos Privada seleccionada.'\n\n--- INICIO BDP RELEVANTE ---\n" + bdAumentada + "\n--- FIN BDP RELEVANTE ---";
        }

        // 4. Construir y enviar la Solicitud Final.
        var mensajesParaSolicitud = new List<Mensaje>();
        mensajesParaSolicitud.Add(new Mensaje { Rol = "system", Contenido = mensajeDeSistema });
        mensajesParaSolicitud.AddRange(mensajesRelevantes);
        mensajesParaSolicitud.Add(nuevoMensajeUsuario);

        var solicitud = new SolicitudChat { Modelo = NOMBRE_MODELO, Mensajes = mensajesParaSolicitud };
        var contenidoJson = JsonSerializer.Serialize(solicitud);
        var contenidoHttp = new StringContent(contenidoJson, Encoding.UTF8, "application/json");

        try
        {
            var respuestaHttp = await clienteHttp.PostAsync(URL_GROQ_API, contenidoHttp);
            respuestaHttp.EnsureSuccessStatusCode();

            var contenidoRespuesta = await respuestaHttp.Content.ReadAsStringAsync();
            var objetoRespuesta = JsonSerializer.Deserialize<RespuestaChat>(contenidoRespuesta);

            string respuestaIA = objetoRespuesta.Opciones[0].Mensaje.Contenido.Trim();

            // 5. ACTUALIZACIÓN Y PERSISTENCIA DE LA MEMORIA
            historialActivo.Add(nuevoMensajeUsuario);
            historialActivo.Add(new Mensaje { Rol = "assistant", Contenido = respuestaIA });
            GuardarHistorial(modoActual, idHistorial, historialActivo);

            return respuestaIA;
        }
        catch (HttpRequestException ex)
        {
            return $"Error de conexión con Groq. Revisá tu clave y conexión a internet. Detalles: Response status code does not indicate success: {(int)ex.StatusCode} ({ex.StatusCode}).";
        }
        catch (Exception ex)
        {
            return $"Ocurrió un error inesperado. Detalles: {ex.Message}";
        }
    }

    /// <summary>
    /// Documentacion: Carga y retorna el historial completo para un modo e ID dados.
    /// </summary>
    public List<Mensaje> HistorialConversacionCompleto(ModoOperacion modo, IdentificadorHistorial idHistorial)
    {
        return CargarHistorial(modo, idHistorial);
    }

    /// <summary>
    /// Documentacion: Elimina el archivo de historial correspondiente al modo e ID dados.
    /// </summary>
    public void BorrarHistorial(ModoOperacion modo, IdentificadorHistorial idHistorial)
    {
        string rutaArchivo = ObtenerRutaArchivoHistorial(modo, idHistorial);

        try
        {
            if (File.Exists(rutaArchivo))
            {
                File.Delete(rutaArchivo);
                Console.WriteLine($"Historial borrado: {rutaArchivo}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al borrar historial {rutaArchivo}: {ex.Message}");
        }
    }

    // <<--- MÉTODO PARA COPIAR ARCHIVOS (Implementación solicitada para VentanaPrincipal.cs)
    /// <summary>
    /// Copia un archivo cargado por el usuario a la Base de Datos Privada (BDP).
    /// </summary>
    public void CopiarArchivoABDP(string rutaOrigen, string nombreNuevo)
    {
        string directorioDestino = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, RUTA_BDP);

        if (!Directory.Exists(directorioDestino))
        {
            Directory.CreateDirectory(directorioDestino);
        }

        string rutaDestinoCompleta = Path.Combine(directorioDestino, nombreNuevo);

        try
        {
            File.Copy(rutaOrigen, rutaDestinoCompleta, true); // true para sobrescribir
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al copiar el archivo a la BDP: {ex.Message}");
            throw; // Re-lanzar para que VentanaPrincipal pueda mostrar el error.
        }
    }
}