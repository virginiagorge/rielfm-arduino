using System.Windows;
using System.Windows.Controls.Primitives;
using CSCore.CoreAudioAPI;


namespace Toggle_Radio
{

    class AudioPlayChecker
    {

        public static MMDevice GetNestorRenderDevice()
        {
            string VoiceMeeterAuxOutputDeviceID = "{0.0.1.00000000}.{69ECE92B-7B9D-4AFC-B4F3-EC0F1F5654B2}";

            using (var enumerator = new MMDeviceEnumerator())
            {
                /*MMDeviceCollection collection = enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);
                foreach (MMDevice device in collection)
                {
                    if (device.DeviceID == VoiceMeeterAuxOutputDeviceID)
                    {
                        return enumerator.GetDevice(device.DeviceID);
                    }
                }
                return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);*/
                return enumerator.GetDevice(VoiceMeeterAuxOutputDeviceID);
            }
        }

        public static MMDevice GetPeceraRenderDevice()
        {
            string VoiceMeeterVAIO3OutputDeviceID = "{0.0.1.00000000}.{9C5A4A33-88A0-42DD-BAAE-7A44BED027D8}";
            using (var enumerator = new MMDeviceEnumerator())
            {
                return enumerator.GetDevice(VoiceMeeterVAIO3OutputDeviceID);
            }
        }

        // Checks if audio is playing on a certain device
        public static bool IsAudioPlaying(MMDevice device)
        {
            using (var meter = AudioMeterInformation.FromDevice(device))
            {
                float threshold = 0.000001f;
                return meter.PeakValue > threshold;
            }
        }
    }

    public class Toggle : ToggleButton

    {
        static Toggle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Toggle), new FrameworkPropertyMetadata(typeof(Toggle)));
        }
    }

 //   public class Program
 //   {
 //       public static void Main(string[] args)
 //       {
 //           bool USAR_CONSOLA = true;
 //           ActualizarConfiguracion(USAR_CONSOLA);
 //       }

 //       public static void ActualizarConfiguracion(bool USAR_CONSOLA)
//        {
//            string archivoFuente = USAR_CONSOLA ? "settings_with_behringer.ini" : "settings_with_virtualcable.ini";
//            string archivoDestino = "default_settings.ini";

//            try
//            {
//                File.Copy(archivoFuente, archivoDestino, true);
//                Console.WriteLine($"Se ha copiado el archivo {archivoFuente} a {archivoDestino}");
//            }
//            catch (IOException e)
//            {
//                Console.WriteLine($"Error al copiar el archivo: {e.Message}");
//            }
//        }

//    }
}
