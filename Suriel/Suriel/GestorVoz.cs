using System;
using System.Speech.Synthesis;
using System.Globalization;

namespace Suriel
{
    public class GestorVoz
    {
        private readonly SpeechSynthesizer sintetizador;

        public bool EstaActivada { get; set; }

        public GestorVoz()
        {
            sintetizador = new SpeechSynthesizer();

            // --- INICIO DE LA CORRECCIÓN CLAVE ---
            try
            {
                // El error: sintacticamente SelectVoiceByHints no acepta el LCID (1034) como un int.
                // La solución: Convertimos el LCID a CultureInfo.

                // Creamos un objeto CultureInfo para el español (LCID 1034 o "es-ES").
                CultureInfo culturaEspanola = new CultureInfo(1034);

                sintetizador.SelectVoiceByHints(
                    VoiceGender.Female,
                    VoiceAge.Adult,
                    0,
                    culturaEspanola // ⬅️ Usamos el objeto CultureInfo
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Advertencia: No se pudo configurar la voz en español. Se usará la predeterminada. Error: {ex.Message}");
            }
            // --- FIN DE LA CORRECCIÓN CLAVE ---

            sintetizador.Rate = -1; // Velocidad
            EstaActivada = true;
            sintetizador.Volume = 100;
        }

        public void ReproducirAudio(string textoParaReproducir)
        {
            if (EstaActivada)
            {
                sintetizador.SpeakAsyncCancelAll();
                sintetizador.SpeakAsync(textoParaReproducir);
            }
        }

        public void EstablecerVolumen(int nuevoVolumen)
        {
            if (nuevoVolumen >= 0 && nuevoVolumen <= 100)
            {
                sintetizador.Volume = nuevoVolumen;
            }
        }

        public void DetenerAudio()
        {
            sintetizador.SpeakAsyncCancelAll();
        }
    }
}