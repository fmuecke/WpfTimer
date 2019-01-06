using System;
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

        public static readonly RoutedCommand SelectSoundCommand = new RoutedCommand();

        public static readonly RoutedCommand ChangeTimeCommand = new RoutedCommand();

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
            ShowTimeInputBox();
            InputTextBox.Text = this.Timer.Duration.ToString();
            InputTextBox.SelectAll();
            InputTextBox.Focus();
        }

        private void ChangeTimeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ChangeTime();
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

        private void ChangeTime()
        {
            try
            {
                var ts = System.TimeSpan.Parse(InputTextBox.Text);
                this.Timer.Duration = ts;
            }
            catch
            { }
        }

        private void ShowTimeInputBox()
        {
            SoundInputBox.Visibility = Visibility.Collapsed;
            TimeInputBox.Visibility = Visibility.Visible;
        }

        private void ShowSoundInputBox()
        {
            TimerSoundListBox.Items.Clear();

            foreach (var e in Enum.GetValues(typeof(Countdown.SoundFile)))
            {
                TimerSoundListBox.Items.Add(e.ToString());
            }

            TimerSoundListBox.SelectedItem = Timer.GetCurrentSound();

            TimeInputBox.Visibility = Visibility.Collapsed;
            SoundInputBox.Visibility = Visibility.Visible;
        }

        private void HideInputBox()
        {
            TimeInputBox.Visibility = Visibility.Collapsed;
            SoundInputBox.Visibility = Visibility.Collapsed;
            Timer.Focus();
        }

        private void SelectSoundCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ShowSoundInputBox();
            TimerSoundListBox.Focus();
        }

        private void ChangeSoundClick(object sender, RoutedEventArgs e)
        {
            Timer.SetCurrentSound(TimerSoundListBox.SelectedValue.ToString());
            HideInputBox();
        }
    }
}