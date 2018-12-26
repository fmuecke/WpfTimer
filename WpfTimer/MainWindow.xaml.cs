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

        public static readonly RoutedCommand EnterTimeCommand = new RoutedCommand();

        public static readonly RoutedCommand TimeAcceptedCommand = new RoutedCommand();

        public static readonly RoutedCommand HideInputBoxCommand = new RoutedCommand();

        public static readonly RoutedCommand DisplayHelpCommand = new RoutedCommand();

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
            HideHelpText();
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

        private void EnterTimeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ShowInputBox();
            InputTextBox.Text = this.Timer.Duration.ToString();
            InputTextBox.SelectAll();
            InputTextBox.Focus();
        }

        private void TimeAcceptedCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            AcceptNewTime();
            HideInputBox();
        }

        private void HideInputBoxCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            HideInputBox();
        }

        private void DisplayHelpCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (HelpText.Visibility == Visibility.Visible)
            {
                HideHelpText();
            }
            else
            {
                ShowHelpText();
            }
        }

        private void ShowHelpText()
        {
            HelpText.Visibility = Visibility.Visible;
        }

        private void HideHelpText()
        {
            HelpText.Visibility = Visibility.Collapsed;
        }

        private void AcceptNewTime()
        {
            try
            {
                var ts = System.TimeSpan.Parse(InputTextBox.Text);
                this.Timer.Duration = ts;
            }
            catch
            { }
        }

        private void ShowInputBox()
        {
            InputBox.Visibility = Visibility.Visible;
        }

        private void HideInputBox()
        {
            InputBox.Visibility = Visibility.Collapsed;
        }
    }
}