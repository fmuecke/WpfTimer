using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

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
        public static readonly RoutedCommand AddSecsCommand = new RoutedCommand();
        public static readonly RoutedCommand SubtractSecsCommand = new RoutedCommand();
        public static readonly RoutedCommand ChangeSoundCommand = new RoutedCommand();
        private const int SecondsToIncrement = 30;

        public MainWindow()
        {
            InitializeComponent();
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            WindowDrag(this, e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            ToggleFullScreen();
            base.OnMouseDoubleClick(e);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
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
            ToggleFullScreen();
        }

        private void ToggleFullScreen()
        {
            this.Topmost = true;
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                this.Visibility = Visibility.Collapsed;
                // re-show the window after changing style
                this.Visibility = Visibility.Visible;
            }
        }

        private void EnterTimeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ToggleTimeInputBox();
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

        private void UpdateInputBox()
        {
            InputTextBox.Text = this.Timer.Duration.ToString();
        }

        private void AddSecsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Timer.Duration = this.Timer.Duration.TimeSpan.Add(new TimeSpan(0, 0, SecondsToIncrement));
            UpdateInputBox();
        }

        private void SubtractSecsCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var span = new TimeSpan(0, 0, SecondsToIncrement);
            var oldVal = this.Timer.Duration.TimeSpan;
            var newVal = oldVal > span ? oldVal - span : new TimeSpan();
            this.Timer.Duration = newVal;
            UpdateInputBox();
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

        private void ToggleTimeInputBox()
        {
            if (TimeInputBox.Visibility == Visibility.Visible)
            {
                TimeInputBox.Visibility = Visibility.Collapsed;
                Timer.Focus();
            }
            else
            {
                SoundInputBox.Visibility = Visibility.Collapsed;
                TimeInputBox.Visibility = Visibility.Visible;
            }
        }

        private void ToggleSoundInputBox()
        {
            if (SoundInputBox.Visibility == Visibility.Visible)
            {
                SoundInputBox.Visibility = Visibility.Collapsed;
                Timer.Focus();
            }
            else
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
        }

        private void HideInputBox()
        {
            TimeInputBox.Visibility = Visibility.Collapsed;
            SoundInputBox.Visibility = Visibility.Collapsed;
            Timer.Focus();
        }

        private void SelectSoundCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ToggleSoundInputBox();
            TimerSoundListBox.Focus();
        }

        private void ChangeSoundCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Timer.SetCurrentSound(TimerSoundListBox.SelectedValue.ToString());
            HideInputBox();
        }

        //https://stackoverflow.com/questions/611298/how-to-create-a-wpf-window-without-a-border-that-can-be-resized-via-a-grip-only
        //Attach this to the PreviewMousLeftButtonDown event of the grip control in the lower right corner of the form to resize the window
        //private void WindowResize(object sender, MouseButtonEventArgs e) //PreviewMousLeftButtonDown
        //{
        //   HwndSource hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        //   SendMessage(hwndSource.Handle, 0x112, (IntPtr)61448, IntPtr.Zero);
        //}
        //
        //Attach this to the MouseDown event of your drag control to move the window in place of the title bar
        private void WindowDrag(object sender, MouseButtonEventArgs e) // MouseDown
        {
            ReleaseCapture();
            SendMessage(new WindowInteropHelper(this).Handle, 0xA1, (IntPtr)0x2, (IntPtr)0);
        }
    }
}