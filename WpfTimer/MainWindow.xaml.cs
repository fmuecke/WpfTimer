using System.Windows;
using System.Windows.Input;

namespace WpfTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly RoutedCommand PauseCommand = new RoutedCommand();

        public static readonly RoutedCommand ResetCommand = new RoutedCommand();

        public static readonly RoutedCommand FullScreenCommand = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Quit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PauseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Timer.Pause();
        }

        private void ResetCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Timer.Reset();
        }

        private void FullScreenCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.Visibility = Visibility.Collapsed;
                this.Topmost = true;
                this.WindowStyle = WindowStyle.None;
                this.ResizeMode = ResizeMode.NoResize;
                // re-show the window after changing style
                this.Visibility = Visibility.Visible;
            }
        }
    }
}