using System.Windows;
using System.Windows.Controls.Primitives;
using System;
using System.IO.Ports;
using System.Management;
using System.Timers;

namespace Toggle_Radio
{

    public partial class MainWindow : Window
    {

        private SerialPort puertoArduino;
        byte[] data = new Byte[8];

        int USAR_CONSOLA = 0;
        int CARTEL_LED_1 = 1;
        int CARTEL_LED_2 = 2;
        int ENCENDER_POTENCIA_1 = 3;
        int ENCENDER_POTENCIA_2 = 4;
        public const int ENCENDER_TRANSMISOR_1 = 5;
        public const int ENCENDER_TRANSMISOR_2 = 6;
        
        public MainWindow()
        {
            InitializeComponent();

            Timer timer = new Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Start();

            puertoArduino = new SerialPort();
            puertoArduino.PortName = AutodetectArduinoPort();
            puertoArduino.BaudRate = 9600;
            puertoArduino.Open();
        }

        private string AutodetectArduinoPort()
        {
            /*ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);
            */
            try
            {
                /*foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();

                    Console.WriteLine(desc);

                    if (desc.Contains("CH340"))
                    {
                        return deviceId;
                    }

                }*/
                return "COM3";
            }
            catch (ManagementException e)
            {
                System.Diagnostics.Debug.WriteLine("Error: no hay Arduino", e);
            }

            return null;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                data[CARTEL_LED_1] = Convert.ToByte(CARTEL_LED_1_TOGGLE.IsChecked == true && AudioPlaybackWithWASAPI.IsAudioPlaying(AudioPlaybackWithWASAPI.GetNestorRenderDevice()));
                Console.WriteLine(data[CARTEL_LED_1]);

                data[CARTEL_LED_2] = Convert.ToByte(CARTEL_LED_2_TOGGLE.IsChecked == true && AudioPlaybackWithWASAPI.IsAudioPlaying(AudioPlaybackWithWASAPI.GetPeceraRenderDevice()));
                Console.WriteLine(data[CARTEL_LED_2]);

                puertoArduino.Write(data, 0, 8);
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (puertoArduino != null && puertoArduino.IsOpen)
            {
                puertoArduino.Close();
            }
        }

        private void USAR_CONSOLA_Changed(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;
            
            if (toggleButton.IsChecked == true)
            {
                MessageBoxResult result = MessageBox.Show("¿Deseas encender este botón?", "Encender botón", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        data[USAR_CONSOLA] = 1;
                        break;

                    case MessageBoxResult.No:
                        toggleButton.IsChecked = false;                       
                        data[USAR_CONSOLA] = 0;
                        break;
                }
              
            }
            else
            {
                data[0] = 0;
            }

            puertoArduino.Write(data, 0, 8);
        }

        private void Cartel_1_Changed(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;

            string toggleName = toggleButton.Name;
            switch (toggleName)
            {
                case "CARTEL_LED_1_TOGGLE":
                    data[CARTEL_LED_1] = Convert.ToByte(toggleButton.IsChecked == true && (
                    AudioPlaybackWithWASAPI.IsAudioPlaying(AudioPlaybackWithWASAPI.GetNestorRenderDevice())
                    ));
                    break;

            }

            puertoArduino.Write(data, 0, 8);
        }

        private void Cartel_2_Changed(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;

            string toggleName = toggleButton.Name;
            switch (toggleName)
            {
                case "CARTEL_LED_2_TOGGLE":
                    data[CARTEL_LED_2] = Convert.ToByte(toggleButton.IsChecked == true && (
                        AudioPlaybackWithWASAPI.IsAudioPlaying(AudioPlaybackWithWASAPI.GetPeceraRenderDevice())
                    ));
                    break;

            }

            puertoArduino.Write(data, 0, 8);
        }

        private void Toggle_Changed(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;

            string toggleName = toggleButton.Name;
            switch (toggleName)
            {
                case "ENCENDER_POTENCIA_1_TOGGLE":                 
                    data[ENCENDER_POTENCIA_1] = Convert.ToByte(toggleButton.IsChecked);
                    break;
                case "ENCENDER_POTENCIA_2_TOGGLE":
                    data[ENCENDER_POTENCIA_2] = Convert.ToByte(toggleButton.IsChecked);
                    break;
                case "ENCENDER_TRANSMISOR_1_TOGGLE":
                    data[ENCENDER_TRANSMISOR_1] = Convert.ToByte(toggleButton.IsChecked);
                    break;
                case "ENCENDER_TRANSMISOR_2_TOGGLE":
                    data[ENCENDER_TRANSMISOR_2] = Convert.ToByte(toggleButton.IsChecked);
                    break;
               
            }

            puertoArduino.Write(data, 0, 8);
        }

    }
}

