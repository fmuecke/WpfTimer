using System.Windows;

namespace WpfTimer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            App.Current.Startup += new StartupEventHandler((sender, e) => { PreventMonitorPowerdown(); });
            App.Current.Exit += new ExitEventHandler((sender, e) => { AllowMonitorPowerdown(); });
        }

        private void PreventMonitorPowerdown()
        {
            SafeNativeMethods.SetThreadExecutionState(SafeNativeMethods.EXECUTION_STATE.ES_DISPLAY_REQUIRED | SafeNativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
        }

        private void AllowMonitorPowerdown()
        {
            SafeNativeMethods.SetThreadExecutionState(SafeNativeMethods.EXECUTION_STATE.ES_CONTINUOUS);
        }
    }
}