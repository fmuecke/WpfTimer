using System.Windows;

namespace WpfTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Pause(object sender, RoutedEventArgs e)
        {
            this.Timer.Pause();
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            this.Timer.Reset();
        }

        private void Button_Quit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}