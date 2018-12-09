using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WpfTimer
{
    /// <summary>
    /// Interaction logic for Countdown.xaml
    ///
    /// see https://codereview.stackexchange.com/questions/197197/countdown-control-with-arc-animation
    /// </summary>
    public partial class Countdown : UserControl
    {
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(Duration), typeof(Countdown), new PropertyMetadata(new Duration(), OnDurationChanged));

        public static readonly DependencyProperty SecondsRemainingProperty =
            DependencyProperty.Register(nameof(SecondsRemaining), typeof(int), typeof(Countdown), new PropertyMetadata(0));

        private readonly Storyboard _storyboard = new Storyboard();

        public Countdown()
        {
            InitializeComponent();

            DoubleAnimation animation = new DoubleAnimation(-90, 270, Duration);
            Storyboard.SetTarget(animation, Arc);
            Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(Arc.EndAngle)));
            _storyboard.Children.Add(animation);

            DataContext = this;
        }

        public event EventHandler Elapsed;

        public Duration Duration
        {
            get => (Duration)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public int SecondsRemaining
        {
            get => (int)GetValue(SecondsRemainingProperty);
            set => SetValue(SecondsRemainingProperty, value);
        }

        public void Reset()
        {
            _storyboard.Stop();

            _storyboard.CurrentTimeInvalidated -= Storyboard_CurrentTimeInvalidated;
            _storyboard.Completed -= Storyboard_Completed;
            _storyboard.CurrentTimeInvalidated += Storyboard_CurrentTimeInvalidated;
            _storyboard.Completed += Storyboard_Completed;

            _storyboard.Begin();
            _storyboard.Pause();
        }

        public void Pause()
        {
            var state = _storyboard.GetCurrentState();
            if (_storyboard.GetIsPaused())
            {
                _storyboard.Resume();
            }
            else
            {
                _storyboard.Pause();
            }
        }

        private static void OnDurationChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var countdown = obj as Countdown;
            var duration = e.NewValue;
            countdown._storyboard.Children[0].Duration = (Duration)duration;
            countdown.Reset();
        }

        private void Countdown_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
            {
                Reset();
            }
        }

        private void Storyboard_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            ClockGroup cg = (ClockGroup)sender;
            if (cg.CurrentTime == null)
            {
                return;
            }

            TimeSpan elapsedTime = cg.CurrentTime.Value;
            SecondsRemaining = Duration == Duration.Automatic ? 0 :
                (int)Math.Ceiling((Duration.TimeSpan - elapsedTime).TotalSeconds);
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            if (IsVisible)
            {
                Elapsed?.Invoke(this, EventArgs.Empty);
                Beep();
            }
        }

        private void Beep()
        {
            // see: http://windowspresentationfoundationinfo.blogspot.com/2014/10/wpf-sound.html
            SoundPlayer player = new SoundPlayer();
            var direct = Environment.CurrentDirectory + "\\..\\..\\..\\Sounds\\395213__azumarill__door-chime.wav";
            player.SoundLocation = direct;
            try
            {
                player.Load();
                player.Play();
            }
            catch (Exception E) { }
        }
    }
}