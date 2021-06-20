using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfCharts
{
    public class SpiderChartPanel : Panel
    {
        private static readonly Pen DashedPen = new Pen(Brushes.Black, 1)
        {
            DashStyle = new DashStyle
            {
                Dashes = new DoubleCollection {
                                                                                                                                              2,
                                                                                                                                              8
                                                                                                                                          }
            }
        };

        private static readonly Pen Pen = new Pen(Brushes.Black, 3) { EndLineCap = PenLineCap.Round };
        private static readonly Typeface LabelFont = new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);

        #region Minimum

        /// <summary>
        ///   Minimum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(SpiderChartPanel), new FrameworkPropertyMetadata((double)0, OnMinimumChanged));

        /// <summary>
        ///   Gets or sets the Minimum property. This dependency property 
        ///   indicates ....
        /// </summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        ///   Handles changes to the Minimum property.
        /// </summary>
        private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (SpiderChartPanel)d;
            var oldMinimum = (double)e.OldValue;
            var newMinimum = target.Minimum;
            target.OnMinimumChanged(oldMinimum, newMinimum);
        }

        /// <summary>
        ///   Provides derived classes an opportunity to handle changes to the Minimum property.
        /// </summary>
        protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
        }

        #endregion

        #region Maximum

        /// <summary>
        ///   Maximum Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(SpiderChartPanel), new FrameworkPropertyMetadata((double)1, OnMaximumChanged));

        /// <summary>
        ///   Gets or sets the Maximum property. This dependency property 
        ///   indicates ....
        /// </summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        ///   Handles changes to the Maximum property.
        /// </summary>
        private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (SpiderChartPanel)d;
            var oldMaximum = (double)e.OldValue;
            var newMaximum = target.Maximum;
            target.OnMaximumChanged(oldMaximum, newMaximum);
        }

        /// <summary>
        ///   Provides derived classes an opportunity to handle changes to the Maximum property.
        /// </summary>
        protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
        }

        #endregion

        #region Ticks

        /// <summary>
        ///   Ticks Dependency Property
        /// </summary>
        public static readonly DependencyProperty TicksProperty = DependencyProperty.Register("Ticks", typeof(int), typeof(SpiderChartPanel),
            new FrameworkPropertyMetadata(10, OnTicksChanged));

        /// <summary>
        ///   Gets or sets the Ticks property. This dependency property 
        ///   indicates how many ticks you wish to display along the axis between min and max.
        /// </summary>
        public int Ticks
        {
            get { return (int)GetValue(TicksProperty); }
            set { SetValue(TicksProperty, value); }
        }

        /// <summary>
        ///   Handles changes to the Ticks property.
        /// </summary>
        private static void OnTicksChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (SpiderChartPanel)d;
            var oldTicks = (int)e.OldValue;
            var newTicks = target.Ticks;
            target.OnTicksChanged(oldTicks, newTicks);
        }

        /// <summary>
        ///   Provides derived classes an opportunity to handle changes to the Ticks property.
        /// </summary>
        protected virtual void OnTicksChanged(double oldTicks, double newTicks)
        {
        }

        #endregion

        #region Lines

        /// <summary>
        /// Lines Dependency Property (acts as a kind of ItemsSource, triggering a redraw when the collection is changed)
        /// </summary>
        public static readonly DependencyProperty LinesProperty =
            DependencyProperty.Register("Lines", typeof(IEnumerable<ChartLine>), typeof(SpiderChartPanel), new FrameworkPropertyMetadata(null, OnLinesChanged));

        /// <summary>
        /// Gets or sets the Lines property.
        /// </summary>
        public IEnumerable<ChartLine> Lines
        {
            get { return (IEnumerable<ChartLine>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Lines property.
        /// </summary>
        private static void OnLinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (SpiderChartPanel)d;
            var oldLines = (IEnumerable<ChartLine>)e.OldValue;
            var newLines = target.Lines;

            // Remove handler
            var oldValueINotifyCollectionChanged = (e.OldValue as IEnumerable) as INotifyCollectionChanged;
            if (oldValueINotifyCollectionChanged != null)
                oldValueINotifyCollectionChanged.CollectionChanged -= target.LinesCollectionChanged;

            // Add handler in case the Lines collection implements INotifyCollectionChanged
            var newValueINotifyCollectionChanged = (e.NewValue as IEnumerable) as INotifyCollectionChanged;
            if (newValueINotifyCollectionChanged != null)
                newValueINotifyCollectionChanged.CollectionChanged += target.LinesCollectionChanged;

            target.OnLinesChanged(oldLines, newLines);
        }

        private void LinesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InvalidateVisual();
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Lines property.
        /// </summary>
        protected virtual void OnLinesChanged(IEnumerable<ChartLine> oldLines, IEnumerable<ChartLine> newLines)
        {
        }

        #endregion

        protected override void OnRender(DrawingContext dc)
        {
            if (Children.Count < 3 || Ticks < 0) return;

            var steps = Children.Count;
            var deltaAngle = 360D / steps;
            var center = new Point(RenderSize.Width / 2, RenderSize.Height / 2);
            var radius = Math.Min(RenderSize.Width, RenderSize.Height) / 2;
            dc.DrawEllipse(null, Pen, center, radius, radius);

            // Draw the background ticks between minRadius and maxRadius
            var minRadius = radius / 10;
            var maxRadius = radius * .8;
            var deltaRadius = (maxRadius - minRadius);
            var deltaTickRadius = deltaRadius / (Ticks - 1);
            var spokeLength = maxRadius + 10;

            for (var i = 0; i < Ticks; i++)
            {
                var curRadius = minRadius + i * deltaTickRadius;
                var angle = 0D;
                var p1 = GetPoint(center, curRadius, angle);
                dc.DrawEllipse(null, DashedPen, center, curRadius, curRadius);
                /*for (var j = 0; j < steps; j++)
                {
                    angle = (j + 1) * deltaAngle;
                    var p2 = GetPoint(center, curRadius, angle);
                    dc.DrawLine(DashedPen, p1, p2);
                    p1 = p2;
                }*/
                // Draw the labels
                p1 = new Point(p1.X + 5, p1.Y -20);
                if (i == 0)
                    dc.DrawText(new FormattedText(Minimum.ToString(CultureInfo.InvariantCulture), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, LabelFont, 14, Brushes.Black), p1);
                else if (i == Ticks - 1)
                    dc.DrawText(new FormattedText(Maximum.ToString(CultureInfo.InvariantCulture), CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight, LabelFont, 14, Brushes.Black), p1);
            }

            // Draw the spokes
            for (var i = 0; i < steps; i++)
            {
                var angle = i * deltaAngle;
                var p1 = GetPoint(center, spokeLength, angle);
                dc.DrawLine(Pen, center, p1);
            }

            // Draw the chart lines
            if (Lines == null) return;
            var scale = Maximum - Minimum;
            if (scale <= 0) return;
            foreach (var line in Lines)
            {
                var angle = 0D;
                var curRadius = minRadius + (line.PointDataSource[0] - Minimum) * deltaRadius / scale;
                var p1 = GetPoint(center, curRadius, angle);
                var myPathFigure = new PathFigure
                {
                    StartPoint = p1,
                    Segments = new PathSegmentCollection()
                };
                var pts = new PointCollection(steps) { p1 };
                for (var j = 1; j < steps; j++)
                {
                    angle = (j) * deltaAngle;
                    curRadius = minRadius + (line.PointDataSource[j] - Minimum) * deltaRadius / scale;
                    var p2 = GetPoint(center, curRadius, angle);
                    myPathFigure.Segments.Add(new LineSegment { Point = p2 });
                    pts.Add(p2);
                }
                myPathFigure.Segments.Add(new LineSegment { Point = p1 });
                var myPathGeometry = new PathGeometry { Figures = new PathFigureCollection { myPathFigure } };
                var pen = new Pen(new SolidColorBrush(line.LineColor), line.LineThickness);
                dc.DrawGeometry(line.FillColor == Colors.Transparent ? null : new SolidColorBrush(line.FillColor), pen, myPathGeometry);
                var brush = new SolidColorBrush(line.LineColor);

                // Draw fat circles on each data point
                foreach (var pt in pts) dc.DrawEllipse(brush, pen, pt, line.LineThickness + 2, line.LineThickness + 2);
            }
        }

        private static Point GetPoint(Point center, double radius, double angle)
        {
            var radAngle = angle * Math.PI / 180;
            var x = center.X + radius * Math.Sin(radAngle);
            var y = center.Y - radius * Math.Cos(radAngle);
            return new Point(x, y);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement elem in Children)
            {
                //Give Infinite size as the avaiable size for all the children
                elem.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }
            InvalidateVisual();
            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        ///   Arrange all children based on the geometric equations for the circle.
        /// </summary>
        /// <param name="finalSize"> </param>
        /// <returns> </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0)
                return finalSize;

            double angle = 0;

            //Degrees converted to Radian by multiplying with PI/180
            var incrementalAngularSpace = (360.0 / Children.Count) * (Math.PI / 180);
            //An approximate radii based on the avialable size , obviusly a better approach is needed here.
            const double d = 2.15;
            var minSide = Math.Min(RenderSize.Width, RenderSize.Height);
            var radiusX = minSide / d;
            var radiusY = minSide / d;

            foreach (UIElement elem in Children)
            {
                //Calculate the point on the circle for the element
                var childPoint = new Point(Math.Sin(angle) * radiusX, -Math.Cos(angle) * radiusY);

                //Offsetting the point to the Avalable rectangular area which is FinalSize.
                var actualChildPoint = new Point(finalSize.Width / 2 + childPoint.X - elem.DesiredSize.Width / 2, finalSize.Height / 2 + childPoint.Y - elem.DesiredSize.Height / 2);

                //Call Arrange method on the child element by giving the calculated point as the placementPoint.
                elem.Arrange(new Rect(actualChildPoint.X, actualChildPoint.Y, elem.DesiredSize.Width, elem.DesiredSize.Height));

                //Calculate the new _angle for the next element
                angle += incrementalAngularSpace;
            }

            return finalSize;
        }
    }
}