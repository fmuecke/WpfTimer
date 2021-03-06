﻿using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        public static readonly DependencyProperty TimeRemainingProperty =
            DependencyProperty.Register(nameof(TimeRemaining), typeof(double), typeof(Countdown), new PropertyMetadata(0.0));

        public static readonly DependencyProperty TimeRemainingUnitProperty =
            DependencyProperty.Register(nameof(TimeRemainingUnit), typeof(string), typeof(Countdown), new PropertyMetadata(string.Empty));

        private readonly Storyboard _storyboard = new Storyboard();
        private MediaPlayer _mp3Player = new MediaPlayer();
        private SoundPlayer _soundPlayer = new SoundPlayer();

        private SoundFile _currentSound;

        private SolidColorBrush _activeBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DC141f"));
        private SolidColorBrush _pausedBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("gray"));

        public Countdown()
        {
            InitializeComponent();

            DoubleAnimation animation = new DoubleAnimation(-89.99999, 270, Duration);
            Storyboard.SetTarget(animation, Arc);
            Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(Arc.EndAngle)));
            _storyboard.Children.Add(animation);

            Arc.Stroke = _pausedBrush;

            DataContext = this;
            SetCurrentSound(SoundFile.Beep);
        }

        public event EventHandler Elapsed;

        public enum SoundFile
        {
            Beep, CheeringCrowd, KitchenTimer
        }

        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public int SecondsRemaining
        {
            get { return (int)GetValue(SecondsRemainingProperty); }
            set { SetValue(SecondsRemainingProperty, value); }
        }

        public double TimeRemaining
        {
            get { return (double)GetValue(TimeRemainingProperty); }
            set { SetValue(TimeRemainingProperty, value); }
        }

        public string TimeRemainingUnit
        {
            get { return (string)GetValue(TimeRemainingUnitProperty); }
            set { SetValue(TimeRemainingUnitProperty, value); }
        }

        public void Reset()
        {
            _soundPlayer.Stop();
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
                Arc.Stroke = _activeBrush;
                _storyboard.Resume();
            }
            else
            {
                Arc.Stroke = _pausedBrush;
                _storyboard.Pause();
            }
        }

        public void SetCurrentSound(SoundFile s)
        {
            switch (s)
            {
                case SoundFile.Beep:
                    _soundPlayer.Stream = Properties.Resources._395213__azumarill__door_chime;
                    _currentSound = s;
                    break;

                case SoundFile.KitchenTimer:
                    _soundPlayer.Stream = Properties.Resources._154954__keykrusher__kitchen_timer_x3;
                    _currentSound = s;
                    break;

                case SoundFile.CheeringCrowd:
                default:
                    _soundPlayer.Stream = Properties.Resources._429422__foxzine__audience_clapping_ADPCM;
                    _currentSound = SoundFile.CheeringCrowd;
                    break;
            }
        }

        public void SetCurrentSound(string s)
        {
            SoundFile enumVal;
            if (System.Enum.TryParse<SoundFile>(s, out enumVal))
            {
                SetCurrentSound(enumVal);
            }
            else
            {
                SetCurrentSound(SoundFile.Beep);
            }
        }

        public string GetCurrentSound()
        {
            return _currentSound.ToString();
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

        private void SetTimeRemainingText()
        {
            if (SecondsRemaining >= 3600)
            {
                TimeRemaining = Math.Round((double)SecondsRemaining / 3600, 1);
                TimeRemainingUnit = "h";
            }
            else if (SecondsRemaining >= 60)
            {
                TimeRemaining = Math.Round((double)SecondsRemaining / 60, 1);
                TimeRemainingUnit = "min";
            }
            else
            {
                TimeRemaining = SecondsRemaining;
                TimeRemainingUnit = "sec";
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

            SetTimeRemainingText();
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
            _soundPlayer.Stop();
            try
            {
                _soundPlayer.Play();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void BeepMp3()
        {
            _mp3Player.Close();
            ResourceWriter.DumpResource("403057__vesperia94__hooray.mp3", @"c:\temp\file.mp3");
            try
            {
                _mp3Player.Open(new Uri(@"C:\temp\file.mp3"));
                _mp3Player.Play();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}