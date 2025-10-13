using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace Suriel
{
    // Definimos la clase para manejar la síntesis de voz del asistente Suriel.
    public class GestorVoz
    {
        // Sintetizador es la herramienta principal que usaremos para convertir texto en audio.
        private readonly SpeechSynthesizer sintetizador;
        // Creamos una propiedad para saber si la voz está activa o no.
        public bool EstaActivada { get; set; }

        // Constructor de la clase.
        public GestorVoz()
        {
            sintetizador = new SpeechSynthesizer();
            // Configuramos la velocidad de la voz (un poco más lento para mejor comprensión).
            sintetizador.Rate = -1;
            // La voz comienza activa por defecto, como nos gusta a los argentinos.
            EstaActivada = true;

            // Configuramos el volumen inicial al 100%.
            sintetizador.Volume = 100;
        }

        /// <summary>
        /// Documentacion: Reproduce el texto de la respuesta si la voz está activada.
        /// </summary>
        /// <param name="textoParaReproducir">El texto que Suriel debe "hablar".</param>
        // Usamos 'textoParaReproducir' en lugar de 't' o 'temp' para que se autodocumente el código.
        public void ReproducirAudio(string textoParaReproducir)
        {
            // filtramos por el estado de la propiedad porque solo queremos reproducir si el usuario lo activó.
            if (EstaActivada)
            {
                // Detenemos cualquier audio anterior antes de empezar uno nuevo.
                sintetizador.SpeakAsyncCancelAll();
                // Inicia la reproducción de forma asíncrona para no trabar la interfaz.
                sintetizador.SpeakAsync(textoParaReproducir);
            }
        }

        /// <summary>
        /// Documentacion: Actualiza el volumen del sintetizador de voz.
        /// </summary>
        /// <param name="nuevoVolumen">El valor del volumen (0 a 100).</param>
        public void EstablecerVolumen(int nuevoVolumen)
        {
            // Evitamos que el valor se salga del rango permitido (0-100).
            if (nuevoVolumen >= 0 && nuevoVolumen <= 100)
            {
                sintetizador.Volume = nuevoVolumen;
            }
        }

        /// <summary>
        /// Documentacion: Detiene cualquier reproducción de audio en curso.
        /// </summary>
        public void DetenerAudio()
        {
            sintetizador.SpeakAsyncCancelAll();
        }
    }
}
