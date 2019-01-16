using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfTimer
{
    public class Arc : Shape
    {
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register(nameof(Center), typeof(Point), typeof(Arc),
                new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(90.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(Arc),
                new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty SmallAngleProperty =
            DependencyProperty.Register(nameof(SmallAngle), typeof(bool), typeof(Arc),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        static Arc()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Arc), new FrameworkPropertyMetadata(typeof(Arc)));
        }

        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set {  SetValue(StartAngleProperty, value); }
        }

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public bool SmallAngle
        {
            get { return (bool)GetValue(SmallAngleProperty); }
            set { SetValue(SmallAngleProperty, value); }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                double startAngleRadians = StartAngle * Math.PI / 180;
                double endAngleRadians = EndAngle * Math.PI / 180;

                double a0 = StartAngle < 0 ? startAngleRadians + 2 * Math.PI : startAngleRadians;
                double a1 = EndAngle < 0 ? endAngleRadians + 2 * Math.PI : endAngleRadians;

                if (a1 < a0)
                {
                    a1 += Math.PI * 2;
                }

                SweepDirection d = SweepDirection.Counterclockwise;
                bool large;

                if (SmallAngle)
                {
                    large = false;
                    d = (a1 - a0) > Math.PI ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;
                }
                else
                {
                    large = (Math.Abs(a1 - a0) < Math.PI);
                }

                Point p0 = Center + new Vector(Math.Cos(a0), Math.Sin(a0)) * Radius;
                Point p1 = Center + new Vector(Math.Cos(a1), Math.Sin(a1)) * Radius;

                List<PathSegment> segments = new List<PathSegment>
            {
                new ArcSegment(p1, new Size(Radius, Radius), 0.0, large, d, true)
            };

                List<PathFigure> figures = new List<PathFigure>
            {
                new PathFigure(p0, segments, true)
                {
                    IsClosed = false
                }
            };

                return new PathGeometry(figures, FillRule.EvenOdd, null);
            }
        }
    }
}